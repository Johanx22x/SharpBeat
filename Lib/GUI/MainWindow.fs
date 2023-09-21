namespace SharpBeat.Lib.GUI

open Avalonia.Controls

module MainWindow =
    open Avalonia
    open Avalonia.FuncUI.Hosts
    open Avalonia.FuncUI
    open Avalonia.FuncUI.DSL
    open Avalonia.Layout
    open Avalonia.FuncUI.Types
    open SharpBeat.Lib.GUI
    open SharpBeat.Lib.Models.Song
    open SharpBeat.Lib.Models
    open SharpBeat.Lib.Backend
    open SharpBeat.Lib.DB.Playlist
    open SharpBeat.Lib.Backend.PlayerLib
    open System

    let view () =
        Component(fun ctx ->
            let genres = ctx.useState<string list>(["All"])
            let songs = ctx.useState<Song list>([])
            let current = ctx.useState<Song option>(None)
            let currentPlaylist = ctx.useState<string option>(None)
            let playlists = ctx.useState<Playlist list>(getPlaylists())
            let player = getEmptyPlayer
            let playerState = ctx.useState<Types.PlayState>(Types.PlayState.Stop)
            let lastVolume = ctx.useState<int>(0)

            let getCachedGenres () =
                if genres.Current.Length <= 1 then
                    ["All"] :: [Api.getSongs "" |> List.map (fun song -> song.Genre) |> List.distinct] |> List.concat
                else
                    genres.Current

            genres.Set(getCachedGenres())

            let setCurrent (song: obj) =
                match song with
                | :? Song as song -> 
                    let media = getMediaFromUri(new Uri(song.url()))
                    player.Media <- media
                    player.Play() |> ignore

                    playerState.Set(Types.PlayState.Play)

                    current.Set(Some(song))
                | _ -> ()

            let selectPlaylist (playlist: obj) =
                match playlist with
                | :? string as playlist -> 
                    let _songs = getPlaylistSongs playlist
                    let serverSongs = getPlaylistSongs playlist |> List.map (fun song -> Api.getSongs song) |> List.concat

                    // for song in songs if song not in serversongs then delete song from playlist
                    _songs |> List.iter (fun song -> 
                        if serverSongs |> List.exists (fun serverSong -> serverSong.Hash = song) then
                            ()
                        else
                            removePlaylistSong (playlist, song)
                    )

                    currentPlaylist.Set(Some(playlist))
                    songs.Set(serverSongs)
                | _ -> ()

            let songsPageContent = 
                DockPanel.create [ 
                    DockPanel.children [
                        DockPanel.create [ 
                            DockPanel.horizontalAlignment HorizontalAlignment.Center
                            DockPanel.children [
                                // Search bar
                                SearchBar.searchBar "Songs" (fun query -> 
                                    // TODO: Implement a better filtering system
                                    songs.Set(Api.getSongs query)
                                )

                                // Genre dropdown
                                StackPanel.create [
                                    StackPanel.children [
                                        TextBlock.create [
                                            TextBlock.text "Genre:"
                                            TextBlock.fontSize 15.
                                        ]
                                        ComboBox.create [
                                            ComboBox.borderThickness 1.
                                            ComboBox.borderBrush Colors.Light.background
                                            ComboBox.dock Dock.Left
                                            ComboBox.width 100.
                                            ComboBox.dataItems genres.Current
                                            ComboBox.selectedItem genres.Current.[0]

                                            ComboBox.itemTemplate (
                                                DataTemplateView<string>.create(fun genre -> 
                                                    TextBlock.create [
                                                        TextBlock.text genre
                                                    ]
                                                )
                                            )

                                            ComboBox.onSelectedItemChanged (fun genre ->
                                                // TODO: Implement a better filtering system
                                                let _songs = Api.getSongs ""
                                                let _songs = if genre = "All" then _songs else _songs |> List.filter (fun song -> song.Genre = string genre)
                                                songs.Set(_songs)
                                            )
                                        ]
                                    ]
                                    StackPanel.orientation Orientation.Vertical
                                    StackPanel.horizontalAlignment HorizontalAlignment.Center
                                    StackPanel.margin (Thickness (10., 0., 0., 0.))
                                ]

                                // Duration filter
                                // Like: 0s - 300s or 100s - 200s
                                DurationFilter.durationFilter (fun min max ->
                                    // TODO: Implement a better filtering system
                                    let _songs = Api.getSongs ""
                                    let _songs = if min = 0 && max = -1 then _songs else _songs |> List.filter (fun song -> if max = -1 then song.Duration >= int16 min else song.Duration >= int16 min && song.Duration <= int16 max)

                                    songs.Set(_songs)
                                )
                            ]

                            DockPanel.dock Dock.Top
                            DockPanel.margin 10.
                        ]

                        // NOTE: Consider to change this
                        ToolBar.toolBar false songs playlists
                        
                        // Song list
                        ListBox.create [
                            ListBox.dataItems songs.Current
                            ListBox.dock Dock.Top
                            ListBox.onSelectedItemChanged (fun song -> setCurrent song)

                            ListBox.itemTemplate (
                            DataTemplateView<Song>.create(fun song -> 
                                    TextBlock.create [
                                        TextBlock.text $"{song.Title} - {song.Artist}"
                                    ]
                                )
                            )

                            ListBox.contextMenu (
                                ContextMenu.create [
                                    ContextMenu.viewItems [
                                        MenuItem.create [
                                            MenuItem.header "Add to playlist"
                                            MenuItem.viewItems (
                                                playlists.Current |> List.filter (fun playlist ->
                                                    not (playlist.songs |> List.exists (fun song -> 
                                                        if current.Current.IsSome then
                                                            song = current.Current.Value.Hash
                                                        else
                                                            false
                                                    ))
                                                ) |> List.map (fun playlist ->
                                                    MenuItem.create [
                                                        MenuItem.header playlist.name
                                                        MenuItem.onClick (fun _ ->
                                                            let _songs = getSongs()
                                                            if _songs |> List.exists (fun song -> fst song = current.Current.Value.Hash) then
                                                                addPlaylistSong (playlist.name, current.Current.Value.Hash)
                                                            else 
                                                                addSong (current.Current.Value.Hash, current.Current.Value.Title)
                                                                addPlaylistSong (playlist.name, current.Current.Value.Hash)
                                                            playlists.Set(getPlaylists())
                                                        )
                                                    ]
                                                )
                                            )
                                        ]
                                        MenuItem.create [
                                            MenuItem.header "Remove from playlist"
                                            MenuItem.viewItems (
                                                playlists.Current |> List.filter (fun playlist ->
                                                    playlist.songs |> List.exists (fun song -> 
                                                        if current.Current.IsSome then
                                                            song = current.Current.Value.Hash
                                                        else
                                                            false
                                                    )
                                                ) |> List.map (fun playlist ->
                                                    MenuItem.create [
                                                        MenuItem.header playlist.name
                                                        MenuItem.onClick (fun _ ->
                                                            removePlaylistSong (playlist.name, current.Current.Value.Hash)
                                                            playlists.Set(getPlaylists())
                                                        )
                                                    ]
                                                )
                                            )
                                        ]
                                    ]
                                ]
                            )
                        ]
                    ]
                    DockPanel.background Colors.Light.background
                ]

            let playlistPageContent = 
                DockPanel.create [ 
                    DockPanel.children [
                        // Search bar
                        DockPanel.create [ 
                            DockPanel.horizontalAlignment HorizontalAlignment.Center
                            DockPanel.children [
                                SearchBar.searchBar "Playlists" (fun (value: string) -> 
                                    let _playlists = getPlaylists()
                                    if _playlists.Length = 0 then
                                        playlists.Set(_playlists)
                                    else
                                        if value = null || value = "" then
                                            playlists.Set(_playlists)
                                        else
                                            playlists.Set(_playlists |> List.filter (fun playlist -> playlist.name.ToLower().Contains(value.ToLower())))
                                )
                            ]
                            DockPanel.dock Dock.Top
                            DockPanel.margin 10.
                        ]

                        // NOTE: Consider to change this
                        ToolBar.toolBar true songs playlists

                        // Playlist list
                        ListBox.create [
                            ListBox.dataItems (playlists.Current |> List.map (fun playlist -> playlist.name))
                            ListBox.dock Dock.Top
                            ListBox.onSelectedItemChanged (fun playlist ->
                                selectPlaylist playlist
                                currentPlaylist.Set(Some(string playlist))
                            )
                            ListBox.contextMenu (
                                ContextMenu.create [
                                    ContextMenu.viewItems [
                                        MenuItem.create [
                                            MenuItem.header "Delete playlist"
                                            MenuItem.onClick (fun _ ->
                                                removePlaylist (currentPlaylist.Current.Value)

                                                // Remove each entry in PlaylistSong table that has the same playlist name 
                                                removePlaylistSongs (currentPlaylist.Current.Value)

                                                playlists.Set(getPlaylists())
                                            )
                                        ]
                                    ]
                                ]
                            )
                        ]

                    ]
                    DockPanel.background Colors.Light.background
                ]

            let tabs : IView list = [
                TabItem.create [
                    TabItem.foreground Colors.Light.foreground
                    TabItem.header "Songs"
                    TabItem.content songsPageContent
                ]
                TabItem.create [
                    TabItem.foreground Colors.Light.foreground
                    TabItem.header "Playlists"
                    TabItem.content playlistPageContent
                ]
            ]

            let onPlayStateChange (state: Types.PlayState) =
                match state with
                | Types.PlayState.Play -> ()
                | Types.PlayState.Pause -> player.Pause()
                | Types.PlayState.Stop -> player.Stop()

            let getNextSong (songs: Song list) (current: Song option) =
                match current with
                | Some current -> 
                    let index = songs |> List.findIndex (fun song -> song = current)
                    if index = songs.Length - 1 then
                        songs.[0]
                    else
                        songs.[index + 1]
                | None -> songs.[0]

            let getPreviousSong (songs: Song list) (current: Song option) =
                match current with
                | Some current -> 
                    let index = songs |> List.findIndex (fun song -> song = current)
                    if index = 0 then
                        songs.[songs.Length - 1]
                    else
                        songs.[index - 1]
                | None -> songs.[0]

            let onRequestPlay (direction: Types.PlayDirection) =
                match direction with
                | Types.PlayDirection.Next -> 
                    let nextSong = getNextSong songs.Current current.Current
                    setCurrent nextSong
                | Types.PlayDirection.Previous -> 
                    let previousSong = getPreviousSong songs.Current current.Current
                    setCurrent previousSong

            let shuffle (original: _ list) =
                let rng = Random.Shared
                let arr = Array.copy (original |> List.toArray)
                let max = (arr.Length - 1)

                let randomSwap (arr: _ []) i =
                    let pos = rng.Next(max)
                    let tmp = arr.[pos]
                    arr.[pos] <- arr.[i]
                    arr.[i] <- tmp
                    arr

                [| 0..max |]
                |> Array.fold randomSwap arr
                |> Array.toList

            let onShuffleRequested() = 
                let shuffledSongs = songs.Current |> shuffle
                songs.Set(shuffledSongs)
                setCurrent current.Current

            let checkIfIsPlaying () =
                if float (player.Position * float32 100.0) >= 99.0 then
                    setCurrent (getNextSong songs.Current current.Current)
                elif player.IsPlaying then
                    playerState.Set(Types.PlayState.Play)
                else
                    playerState.Set(Types.PlayState.Stop)

            checkIfIsPlaying()

            DockPanel.create [
                DockPanel.children [
                    // Play bar
                    Border.create [
                        Border.child (
                            DockPanel.create [
                                DockPanel.children [
                                    StackPanel.create [
                                        StackPanel.verticalAlignment VerticalAlignment.Bottom
                                        StackPanel.horizontalAlignment HorizontalAlignment.Center
                                        StackPanel.orientation Orientation.Horizontal
                                        StackPanel.children [
                                            // Play bar
                                            PlayBar.playBar (
                                                if current.Current.IsSome then 
                                                    current.Current.Value.Title
                                                else 
                                                    ""
                                                ,
                                                if current.Current.IsSome then 
                                                    current.Current.Value.Artist
                                                else
                                                    ""
                                                ,
                                                playerState.Current,
                                                (float player.Position * 100.) |> int,
                                                player,
                                                onPlayStateChange,
                                                onRequestPlay,
                                                onShuffleRequested
                                            )

                                            StackPanel.create [
                                                StackPanel.verticalAlignment VerticalAlignment.Bottom
                                                StackPanel.horizontalAlignment HorizontalAlignment.Center
                                                StackPanel.orientation Orientation.Vertical
                                                StackPanel.children [
                                                    // Volume slider
                                                    Slider.create [
                                                        Slider.orientation Orientation.Vertical
                                                        Slider.minimum 0.
                                                        Slider.maximum 100.
                                                        Slider.value player.Volume
                                                        Slider.height 90.
                                                        Slider.onValueChanged (fun value ->
                                                            player.Volume <- (int value)
                                                        )
                                                        Slider.onPointerPressed (fun _ ->
                                                            if player.Volume = 0 then
                                                                player.Volume <- lastVolume.Current
                                                            else
                                                                lastVolume.Set(player.Volume)
                                                                player.Volume <- 0
                                                        )
                                                    ]

                                                    // Volume icon 
                                                    Icons.volume
                                                ]
                                            ]
                                        ]
                                    ]
                                ]
                            ]
                        )

                        Border.background Colors.Light.primary
                        Border.dock Dock.Bottom
                    ]

                    // Tab for songs, playlists, and albums
                    TabControl.create [
                        TabControl.tabStripPlacement Dock.Left
                        TabControl.viewItems tabs
                        TabControl.dock Dock.Top
                        TabControl.background Colors.Light.background
                        TabControl.foreground Colors.Light.foreground
                    ]
                ]
            ]
        )

    type MainWindow() =
        inherit HostWindow()
        do
            base.Title <- "SharpBeat"
            base.Content <- view()
            base.Icon <- WindowIcon("Assets/Icons/icon.ico")
            base.MinWidth <- 800.0
            base.MinHeight <- 500.0

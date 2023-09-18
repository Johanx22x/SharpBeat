namespace SharpBeat.Lib.GUI

open Avalonia.Controls

module MainWindow =
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
            let playlists = ctx.useState<Playlist list>(getPlaylists())
            let player = getEmptyPlayer
            let playerState = ctx.useState<Types.PlayState>(Types.PlayState.Stop)

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

                    current.Set(Some song)
                | _ -> ()

            let checkIfIsPlaying () =
                if player.IsPlaying then
                    playerState.Set(Types.PlayState.Play)
                else
                    playerState.Set(Types.PlayState.Stop)

            checkIfIsPlaying()

            let selectPlaylist (playlist: obj) =
                match playlist with
                | :? string as playlist -> 
                    printfn "Selected playlist: %s" playlist 
                    printfn "Songs: %A" (getPlaylistSongs playlist)
                    songs.Set(getPlaylistSongs playlist |> List.map (fun song -> Api.getSongs song) |> List.concat)
                | _ -> ()

            let songsPageContent = 
                DockPanel.create [ 
                    DockPanel.children [
                        DockPanel.create [ 
                            DockPanel.horizontalAlignment HorizontalAlignment.Center
                            DockPanel.children [
                                // Refresh button
                                Button.create [
                                    Button.content Icons.refresh
                                    Button.width 50.0
                                    Button.horizontalAlignment HorizontalAlignment.Center
                                    Button.onClick (fun _ -> 
                                        songs.Set(Api.getSongs "")
                                    )
                                ]

                                // Search bar
                                SearchBar.searchBar "Songs" (fun query -> 
                                    // TODO: Implement a better filtering system
                                    songs.Set(Api.getSongs query)
                                )

                                // Genre dropdown
                                ComboBox.create [
                                    ComboBox.background Colors.Light.background
                                    ComboBox.foreground Colors.Light.foreground
                                    ComboBox.borderThickness 1.
                                    ComboBox.borderBrush Colors.Light.background
                                    ComboBox.dock Dock.Left
                                    ComboBox.width 100.
                                    ComboBox.margin 10.

                                    ComboBox.dataItems genres.Current

                                    ComboBox.itemTemplate (
                                        DataTemplateView<string>.create(fun genre -> 
                                            TextBlock.create [
                                                TextBlock.text genre
                                            ]
                                        )
                                    )

                                    ComboBox.onSelectedItemChanged (fun genre ->
                                        // TODO: Implement a better filtering system
                                        songs.Set(Api.getSongs (string genre))
                                    )
                                ]
                            ]

                            DockPanel.dock Dock.Top
                            DockPanel.margin 10.
                        ]
                        
                        // Song list
                        ListBox.create [
                            ListBox.background Colors.Light.background
                            ListBox.foreground Colors.Light.foreground
                            ListBox.dataItems songs.Current
                            ListBox.dock Dock.Top
                            ListBox.onSelectedItemChanged (fun song -> setCurrent song)

                            ListBox.itemTemplate (
                            DataTemplateView<Song>.create(fun song -> 
                                    TextBlock.create [
                                        TextBlock.text $"{song.Artist} - {song.Title}"
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
                                                            addPlaylistSong (playlist.name, current.Current.Value.Hash)
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
                                SearchBar.searchBar "Playlists" (fun a -> a |> ignore)
                            ]
                            DockPanel.dock Dock.Top
                            DockPanel.margin 10.
                        ]


                        // Tool bar
                        ToolBar.toolBar

                        // Playlist list
                        ListBox.create [
                            ListBox.dataItems (playlists.Current |> List.map (fun playlist -> playlist.name))
                            ListBox.dock Dock.Top
                            ListBox.onSelectedItemChanged (fun playlist ->
                                selectPlaylist playlist
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

            let onShuffleRequested () = 
                let shuffledSongs = songs.Current |> shuffle
                songs.Set(shuffledSongs)

            DockPanel.create [
                DockPanel.children [
                    // Play bar
                    Border.create [
                        Border.child (
                            DockPanel.create [
                                DockPanel.children [
                                    // Play bar
                                    PlayBar.playBar (
                                        playerState.Current,
                                        (float player.Position * 100.) |> int,
                                        player,
                                        onPlayStateChange,
                                        onRequestPlay,
                                        onShuffleRequested
                                    )
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
            base.Title <- "SharpTune"
            base.Content <- view()
            base.Icon <- WindowIcon("Assets/Icons/icon.ico")
            base.MinWidth <- 800.0
            base.MinHeight <- 500.0

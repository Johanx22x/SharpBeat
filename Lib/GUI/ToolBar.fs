namespace SharpBeat.Lib.GUI

module ToolBar =
    open Avalonia.Controls 
    open Avalonia.FuncUI.DSL
    open Avalonia.Layout
    open Avalonia
    open Avalonia.FuncUI
    open SharpBeat.Lib.Models.Song
    open SharpBeat.Lib.Backend
    open SharpBeat.Lib.DB.Playlist

    let mutable killableWindow : Window = null

    let createPlaylistView(playlist: IWritable<Playlist list>) =
        Component(fun ctx ->
            let playlistName = ctx.useState<string>("")

            StackPanel.create [
                StackPanel.children [
                    TextBlock.create [
                        TextBlock.text "Playlist name:"
                        TextBlock.margin (Thickness(0., 10., 0., 0.))
                    ]

                    TextBox.create [
                        TextBox.width 200.
                        TextBox.text playlistName.Current
                        TextBox.onTextChanged (fun _text -> 
                            playlistName.Set(_text)
                        )
                    ]

                    StackPanel.create [
                        StackPanel.children [
                            Button.create [
                                Button.content "Create"
                                Button.margin (Thickness(0., 10., 0., 0.))
                                Button.onClick (fun _ -> 
                                    let name = string playlistName.Current
                                    if name <> "" then
                                        addPlaylist name
                                        playlist.Set(getPlaylists())
                                        killableWindow.Close()
                                )
                            ]
                            Button.create [
                                Button.content "Cancel"
                                Button.margin (Thickness(0., 10., 0., 0.))
                                Button.onClick (fun _ -> 
                                    killableWindow.Close()
                                )
                            ]
                        ]
                        StackPanel.orientation Orientation.Horizontal 
                        StackPanel.horizontalAlignment HorizontalAlignment.Center 
                        StackPanel.verticalAlignment VerticalAlignment.Center
                    ]
                ]
                StackPanel.horizontalAlignment HorizontalAlignment.Center
                StackPanel.verticalAlignment VerticalAlignment.Center
                StackPanel.orientation Orientation.Vertical
            ]
        )

    type CreatePlaylistWindow(playlists: IWritable<Playlist list>) =
        inherit Window()
        do 
            base.Width <- 300.
            base.Height <- 200. 
            base.Title <- "Create playlist" 
            base.WindowStartupLocation <- WindowStartupLocation.CenterOwner
            base.Content <- createPlaylistView(playlists)
            base.MaxWidth <- 300.
            base.MaxHeight <- 200.

    let toolBar (option: bool) (songs: IWritable<Song list>) (playlists: IWritable<Playlist list>) =
        Border.create [
            Border.child (
                StackPanel.create [
                    StackPanel.verticalAlignment VerticalAlignment.Top
                    StackPanel.horizontalAlignment HorizontalAlignment.Center
                    StackPanel.orientation Orientation.Vertical
                    StackPanel.dock Dock.Right

                    StackPanel.children [
                        if option then
                            Button.create [
                                Button.content Icons.create
                                Button.width 50.0
                                Button.horizontalAlignment HorizontalAlignment.Center
                                Button.onClick (fun _ -> 
                                    killableWindow <- CreatePlaylistWindow(playlists)
                                    killableWindow.Show()
                                )
                            ]
                        else 
                            // Refresh button
                            Button.create [
                                Button.content Icons.refresh
                                Button.width 50.0
                                Button.onClick (fun _ -> 
                                    songs.Set(Api.getSongs "")
                                )
                            ]
                    ]
                ]
            )
            Border.dock Dock.Right
            Border.padding 15.
        ]

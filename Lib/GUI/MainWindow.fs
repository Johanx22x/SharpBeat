namespace SharpBeat.Lib.GUI
open Song

open Avalonia
open Avalonia.Controls
open Avalonia.Media
open LibVLCSharp.Avalonia
open LibVLCSharp.Shared

module MainWindow =
    open Avalonia.FuncUI.Hosts
    open Avalonia.Controls
    open Avalonia.FuncUI
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI.Types
    open SharpBeat.Lib.GUI

    let view () =
        Component(fun ctx ->
            let songs = ctx.useState<Song list>([])
            let current = ctx.useState<Song Option>(None)

            let songsPageContent = 
                DockPanel.create [ 
                    DockPanel.children [
                        // Search bar
                        SearchBar.searchBar "Songs" (fun query -> songs.Set(Api.getSongs query))
                        
                        // Tool bar
                        ToolBar.toolBar

                        // Song list

                        ListBox.create [
                            ListBox.background Colors.Light.background
                            ListBox.foreground Colors.Light.foreground
                            ListBox.dataItems songs.Current
                            ListBox.onSelectedItemChanged (fun (item) -> printfn "%A" (item))
                            ListBox.dock Dock.Top

                            // TODO: Have a song selected by default
                            ListBox.itemTemplate (
                            DataTemplateView<Song>.create(fun song -> 
                                    TextBlock.create [
                                        TextBlock.text $"{song.Artist} - {song.Title}"
                                    ]
                                )
                            )
                        ]
                    ]
                    DockPanel.background Colors.Light.background
                ]

            // TODO: sqlite playlist database
            // NOTE: here you should use some playlist state

            let playlistPageContent = 
                DockPanel.create [ 
                    DockPanel.children [
                        // Search bar
                        SearchBar.searchBar "Playlists" (fun a -> a |> ignore)

                        // Tool bar
                        ToolBar.toolBar

                        // Playlist list
                        ListBox.create [
                            ListBox.dataItems ["Playlist 1"; "Playlist 2"]
                            ListBox.dock Dock.Top
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

            DockPanel.create [
                DockPanel.children [
                    // Play bar
                    Border.create [
                        Border.child (
                            DockPanel.create [
                                DockPanel.children [
                                    Border.create [
                                        Border.background (
                                            "white"
                                        )
                                        Border.dock Dock.Left
                                        Border.width 133.0
                                        Border.height 133.0
                                    ]

                                    // Play bar
                                    PlayBar.playBar current
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

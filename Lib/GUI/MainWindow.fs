namespace SharpBeat.Lib.GUI

module MainWindow =
    open Avalonia.FuncUI.Hosts
    open Avalonia.Controls
    open Avalonia.FuncUI
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI.Types
    open SharpBeat.Lib.GUI

    let view () =
        Component(fun _ ->

            let songsPageContent = 
                DockPanel.create [ 
                    DockPanel.children [
                        // Search bar
                        SearchBar.searchBar "Songs"
                        
                        // Tool bar
                        ToolBar.toolBar

                        // Song list
                        ListBox.create [
                            ListBox.dataItems ["Song 1"; "Song 2"; "Song 3"; "Song 4"; "Song 5"; "Song 1"; "Song 2"; "Song 3"; "Song 4"; "Song 5"; "Song 1"; "Song 2"; "Song 3"; "Song 4"; "Song 5"; "Song 1"; "Song 2"; "Song 3"; "Song 4"; "Song 5"]
                            ListBox.dock Dock.Top
                        ]
                    ]
                    DockPanel.background Colors.lightBackground
                ]

            let playlistPageContent = 
                DockPanel.create [ 
                    DockPanel.children [
                        // Search bar
                        SearchBar.searchBar "Playlists"

                        // Tool bar
                        ToolBar.toolBar

                        // Playlist list
                        ListBox.create [
                            ListBox.dataItems ["Playlist 1"; "Playlist 2"]
                            ListBox.dock Dock.Top
                        ]

                    ]
                    DockPanel.background Colors.lightBackground
                ]

            let tabs : IView list = [
                TabItem.create [
                    TabItem.header "Songs"
                    TabItem.content songsPageContent
                ]
                TabItem.create [
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
                                    PlayBar.playBar
                                ]
                            ]
                        )

                        Border.background Colors.primaryColor
                        Border.dock Dock.Bottom
                    ]

                    // Tab for songs, playlists, and albums
                    TabControl.create [
                        TabControl.tabStripPlacement Dock.Left
                        TabControl.viewItems tabs
                        TabControl.dock Dock.Top
                        TabControl.background Colors.darkBackground
                        TabControl.foreground Colors.foreground
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

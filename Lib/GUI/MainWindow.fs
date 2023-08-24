namespace SharpBeat.Lib.GUI

module MainWindow =
    open Avalonia
    open Avalonia.Controls.ApplicationLifetimes
    open Avalonia.Themes.Fluent
    open Avalonia.FuncUI.Hosts
    open Avalonia.Controls
    open Avalonia.FuncUI
    open Avalonia.FuncUI.DSL
    open Avalonia.Layout
    open Avalonia.FuncUI.Types
    open SharpBeat.Lib.GUI

    let view () =
        Component(fun ctx ->

            let songsPageContent = 
                DockPanel.create [ 
                    DockPanel.children [
                        // Search bar
                        SearchBar.searchBar "Songs"
                        
                        // Song list
                        ListBox.create [
                            ListBox.dataItems ["Song 1"; "Song 2"; "Song 3"; "Song 4"; "Song 5"; "Song 1"; "Song 2"; "Song 3"; "Song 4"; "Song 5"; "Song 1"; "Song 2"; "Song 3"; "Song 4"; "Song 5"; "Song 1"; "Song 2"; "Song 3"; "Song 4"; "Song 5"]
                        ]
                    ]
                    DockPanel.background Colors.lightBackground
                ]

            let playlistPageContent = 
                DockPanel.create [ 
                    DockPanel.children [
                        // Search bar
                        SearchBar.searchBar "Playlists"

                        // Playlist list
                        ListBox.create [
                            ListBox.dataItems ["Playlist 1"; "Playlist 2"]
                        ]
                    ]
                    DockPanel.background Colors.lightBackground
                ]

            let albumPageContent = 
                DockPanel.create [ 
                    DockPanel.children [
                        // Search bar
                        SearchBar.searchBar "Albums"

                        // Album list
                        ListBox.create [
                            ListBox.dataItems ["Album 1"; "Album 2"]
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
                TabItem.create [
                    TabItem.header "Albums"
                    TabItem.content albumPageContent
                ]
            ]

            DockPanel.create [
                DockPanel.children [
                    Border.create [
                        Border.child (
                            PlayBar.playBar
                        )

                        Border.background Colors.primaryColor
                        Border.dock Dock.Bottom
                        Border.padding 40.
                    ]
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
            base.Icon <- WindowIcon("Assets\Icons\icon.ico")
            base.MinWidth <- 800.0
            base.MinHeight <- 500.0
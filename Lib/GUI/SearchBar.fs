namespace SharpBeat.Lib.GUI

module SearchBar =
    open Avalonia.Controls
    open Avalonia.FuncUI.DSL
    open Avalonia.Layout

    let searchBar (tag: string) =
        let searchText = "Search " + tag + "..."

        DockPanel.create [ 
            DockPanel.horizontalAlignment HorizontalAlignment.Center
            DockPanel.children [
                // Search text box
                TextBox.create [
                    TextBox.background Colors.darkBackground
                    TextBox.foreground Colors.foreground
                    TextBox.borderThickness 1.
                    TextBox.borderBrush Colors.lightDarkForeground
                    TextBox.dock Dock.Left
                    TextBox.width 280.

                    TextBox.watermark searchText
                ]
            ]

            DockPanel.dock Dock.Top
            DockPanel.margin 10.
        ]

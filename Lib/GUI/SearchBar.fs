namespace SharpBeat.Lib.GUI

open System
open Avalonia
open Avalonia.Controls

module SearchBar =
    open Avalonia.Controls
    open Avalonia.FuncUI.DSL
    open Avalonia.Layout

    let searchBar (tag: string) (onChanged: (string -> unit)) =
        let searchText = "Search " + tag + "..."

        DockPanel.create [ 
            DockPanel.horizontalAlignment HorizontalAlignment.Center
            DockPanel.children [
                // Search text box
                TextBox.create [
                    TextBox.background Colors.Light.background
                    TextBox.foreground Colors.Light.foreground
                    TextBox.borderThickness 1.
                    TextBox.borderBrush Colors.Light.background // NOTE: this used to be light dark background, I'll see if I can find a suitable replacement
                    TextBox.dock Dock.Left
                    TextBox.width 280.
                    // TODO: add a debounce here because just
                    // spamming the server with requests every
                    // second is not optimal
                    TextBox.onTextChanged onChanged

                    TextBox.watermark searchText
                ]
            ]

            DockPanel.dock Dock.Top
            DockPanel.margin 10.
        ]

namespace SharpBeat.Lib.GUI

module SearchBar =
    open Avalonia.Controls
    open Avalonia.FuncUI.DSL
    open Avalonia.Layout
    open Avalonia

    let searchBar (tag: string) (onChanged: (string -> unit)) =
        let searchText = "Search " + tag + "..."
        // Search text box
        StackPanel.create [
            StackPanel.children [
                TextBlock.create [
                    TextBlock.text "Search:"
                    TextBlock.fontSize 15.
                ]
                TextBox.create [
                    TextBox.borderThickness 1.
                    TextBox.borderBrush Colors.Light.background
                    TextBox.dock Dock.Left
                    TextBox.width 280.
                    // NOTE: add a debounce here because just
                    // spamming the server with requests every
                    // second is not optimal
                    TextBox.onTextChanged onChanged

                    TextBox.watermark searchText
                ]
            ]
            StackPanel.orientation Orientation.Vertical
            StackPanel.horizontalAlignment HorizontalAlignment.Center 
            StackPanel.verticalAlignment VerticalAlignment.Center
            StackPanel.margin (Thickness (10., 0., 0., 0.))
        ]

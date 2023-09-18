namespace SharpBeat.Lib.GUI

module SearchBar =
    open Avalonia.Controls
    open Avalonia.FuncUI.DSL

    let searchBar (tag: string) (onChanged: (string -> unit)) =
        let searchText = "Search " + tag + "..."
        // Search text box
        TextBox.create [
            TextBox.background Colors.Light.background
            TextBox.foreground Colors.Light.foreground
            TextBox.borderThickness 1.
            TextBox.borderBrush Colors.Light.background
            TextBox.dock Dock.Left
            TextBox.width 280.
            // TODO: add a debounce here because just
            // spamming the server with requests every
            // second is not optimal
            TextBox.onTextChanged onChanged

            TextBox.watermark searchText
        ]


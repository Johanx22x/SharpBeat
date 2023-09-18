namespace SharpBeat.Lib.GUI

module DurationFilter =
    open Avalonia
    open Avalonia.Controls
    open Avalonia.FuncUI.DSL
    open Avalonia.Layout
    open Avalonia.Media

    let durationFilter (onChanged: int -> int -> unit) =
        let initial: int = 0
        let final: int = -1

        StackPanel.create [
            StackPanel.children [
                TextBlock.create [
                    TextBlock.text "Duration (s):"
                    TextBlock.fontSize 15.
                ]

                StackPanel.create [
                    StackPanel.children [
                        TextBox.create [
                            TextBox.borderThickness 1.
                            TextBox.borderBrush Colors.Light.background
                            TextBox.dock Dock.Left
                            TextBox.width 20.

                            TextBox.onTextChanged (fun text ->
                                match System.Int64.TryParse (text) with
                                | true, value -> onChanged (int value) (final)
                                | _ -> onChanged (initial) (final)
                            )

                            TextBox.watermark "0"
                        ]

                        TextBlock.create [
                            TextBlock.text " - "
                            TextBlock.fontSize 25.
                            TextBlock.fontWeight FontWeight.Bold
                        ]

                        TextBox.create [
                            TextBox.borderThickness 1.
                            TextBox.borderBrush Colors.Light.background
                            TextBox.dock Dock.Left
                            TextBox.width 20.

                            TextBox.onTextChanged (fun text ->
                                match System.Int64.TryParse (text) with
                                | true, value -> onChanged (initial) (int value)
                                | _ -> onChanged (initial) (final)
                            )

                            TextBox.watermark "∞"
                        ]
                    ]
                    StackPanel.orientation Orientation.Horizontal
                    StackPanel.horizontalAlignment HorizontalAlignment.Center
                    StackPanel.verticalAlignment VerticalAlignment.Center
                ]
            ]
            StackPanel.orientation Orientation.Vertical
            StackPanel.horizontalAlignment HorizontalAlignment.Center 
            StackPanel.verticalAlignment VerticalAlignment.Center
            StackPanel.margin (Thickness (10., 0., 0., 0.))
]

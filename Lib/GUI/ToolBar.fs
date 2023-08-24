namespace SharpBeat.Lib.GUI

module ToolBar =
    open Avalonia.Controls 
    open Avalonia.FuncUI.DSL
    open Avalonia.Layout

    let toolBar = 
        Border.create [
            Border.child (
                StackPanel.create [
                    StackPanel.verticalAlignment VerticalAlignment.Top
                    StackPanel.horizontalAlignment HorizontalAlignment.Center
                    StackPanel.orientation Orientation.Vertical
                    StackPanel.dock Dock.Right

                    StackPanel.children [
                        Button.create [
                            Button.content Icons.create
                            Button.width 50.0
                            Button.horizontalAlignment HorizontalAlignment.Center
                        ]
    
                        // Gap
                        Border.create [
                            Border.height 5.
                        ]

                        Button.create [
                            Button.content Icons.remove
                            Button.width 50.0
                            Button.horizontalAlignment HorizontalAlignment.Center
                        ]
                    ]
                ]
            )

            Border.background Colors.lightBackground
            Border.dock Dock.Right
            Border.padding 15.
        ]

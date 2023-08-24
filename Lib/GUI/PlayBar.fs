namespace SharpBeat.Lib.GUI

module PlayBar =
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

    let mediaButtons = 
        StackPanel.create [
            StackPanel.verticalAlignment VerticalAlignment.Bottom
            StackPanel.horizontalAlignment HorizontalAlignment.Center
            StackPanel.orientation Orientation.Horizontal
            StackPanel.dock Dock.Bottom

            StackPanel.children [
                Button.create [
                    Button.content "Previous"
                    Button.width 100.0
                    Button.horizontalAlignment HorizontalAlignment.Center
                ]
                Button.create [
                    Button.content "Play"
                    Button.width 100.0
                    Button.horizontalAlignment HorizontalAlignment.Center
                ]
                Button.create [
                    Button.content "Next"
                    Button.width 100.0
                    Button.horizontalAlignment HorizontalAlignment.Center
                ]
            ]
        ]

    let progressBar =
        StackPanel.create [
            StackPanel.verticalAlignment VerticalAlignment.Bottom
            StackPanel.horizontalAlignment HorizontalAlignment.Center
            StackPanel.orientation Orientation.Horizontal
            StackPanel.dock Dock.Bottom

            StackPanel.children [
                Slider.create [
                    Slider.minimum 0.0
                    Slider.maximum 100.0
                    Slider.width 428.0
                    Slider.horizontalAlignment HorizontalAlignment.Center
                ]
            ]
        ]

    let playBar = 
        StackPanel.create [
            StackPanel.verticalAlignment VerticalAlignment.Bottom
            StackPanel.horizontalAlignment HorizontalAlignment.Center
            StackPanel.orientation Orientation.Vertical
            StackPanel.dock Dock.Bottom

            StackPanel.children [
                mediaButtons
                progressBar
            ]
        ]

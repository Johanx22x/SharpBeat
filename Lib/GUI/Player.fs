module SharpBeat.Lib.GUI

open Avalonia.FuncUI
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.Layout

type Player =
    static member MediaButtons
        (
            isPlaying: bool,
            loopState: LoopState,
            onPlayStateChange,
            onRequestPlay,
            onLoopStateChanged,
            onShuffleRequested
        ) =

        StackPanel.create [
            StackPanel.verticalAlignment VerticalAlignment.Bottom
            StackPanel.horizontalAlignment HorizontalAlignment.Left
            StackPanel.orientation Orientation.Horizontal
            StackPanel.dock Dock.Top
            StackPanel.children [
                Button.create [
                    Button.content Icons.previous
                    Button.classes [ "mediabtn" ]
                    Button.onClick (fun _ -> onRequestPlay PlayDirection.Previous)
                ]
                if isPlaying then
                    Button.create [
                        Button.content Icons.pause
                        Button.classes [ "mediabtn" ]
                        Button.onClick (fun _ -> onPlayStateChange PlayState.Pause)
                    ]

                    Button.create [
                        Button.content Icons.stop
                        Button.classes [ "mediabtn" ]
                        Button.onClick (fun _ -> onPlayStateChange PlayState.Stop)
                    ]
                else
                    Button.create [
                        Button.content Icons.play
                        Button.classes [ "mediabtn" ]
                        Button.onClick (fun _ -> onPlayStateChange PlayState.Play)
                    ]
                Button.create [
                    Button.content Icons.next
                    Button.classes [ "mediabtn" ]
                    Button.onClick (fun _ -> onRequestPlay PlayDirection.Next)
                ]
                Button.create [
                    Button.content Icons.shuffle
                    Button.classes [ "mediabtn" ]
                    Button.onClick (fun _ -> onShuffleRequested ())
                ]
                match loopState with
                | LoopState.All ->
                    Button.create [
                        Button.content Icons.repeat
                        Button.classes [ "mediabtn" ]
                        Button.onClick (fun _ -> onLoopStateChanged LoopState.Single)
                    ]
                | LoopState.Single ->
                    Button.create [
                        Button.content Icons.repeatOne
                        Button.classes [ "mediabtn" ]
                        Button.onClick (fun _ -> onLoopStateChanged LoopState.Off)
                    ]
                | LoopState.Off ->
                    Button.create [
                        Button.content Icons.repeatOff
                        Button.classes [ "mediabtn" ]
                        Button.onClick (fun _ -> onLoopStateChanged LoopState.All)
                    ]
            ]
        ]

    static member ProgressBar(sliderPosition: int) =
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
                    Slider.value sliderPosition
                ]
            ]
        ]

    static member Player(children: IView list) =
        DockPanel.create [
            DockPanel.classes [ "mediabar" ]
            DockPanel.dock Dock.Bottom
            DockPanel.horizontalAlignment HorizontalAlignment.Center
            DockPanel.children children
        ]

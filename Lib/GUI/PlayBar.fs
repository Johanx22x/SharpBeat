namespace SharpBeat.Lib.GUI

module PlayBar =
    open Avalonia
    open Avalonia.Controls
    open Avalonia.FuncUI.DSL
    open Avalonia.Layout
    open Avalonia.Media
    open SharpBeat.Lib.GUI
    open SharpBeat.Lib.Models.Types
    open LibVLCSharp.Shared

    let mediaButtons (
        playerState: PlayState,
        onPlayStateChange,
        onRequestPlay,
        onShuffleRequested
    ) =
        StackPanel.create [
            StackPanel.verticalAlignment VerticalAlignment.Bottom
            StackPanel.horizontalAlignment HorizontalAlignment.Center
            StackPanel.orientation Orientation.Horizontal
            StackPanel.dock Dock.Bottom

            StackPanel.children [
                Button.create [
                    Button.content Icons.previous
                    Button.width 100.0
                    Button.horizontalAlignment HorizontalAlignment.Center
                    Button.classes [ "mediabtn" ]
                    Button.onClick (fun _ -> onRequestPlay PlayDirection.Previous)
                ]
                match playerState with 
                | PlayState.Play ->
                    Button.create [
                        Button.content Icons.pause
                        Button.width 100.0
                        Button.horizontalAlignment HorizontalAlignment.Center
                        Button.classes [ "mediabtn" ]
                        Button.onClick (fun _ -> onPlayStateChange PlayState.Pause)
                    ]
                | _ ->
                    Button.create [
                        Button.content Icons.play
                        Button.width 100.0
                        Button.horizontalAlignment HorizontalAlignment.Center
                        Button.classes [ "mediabtn" ]
                        Button.onClick (fun _ -> onPlayStateChange PlayState.Pause)
                    ]
                Button.create [
                    Button.content Icons.next
                    Button.width 100.0
                    Button.horizontalAlignment HorizontalAlignment.Center
                    Button.classes [ "mediabtn" ]
                    Button.onClick (fun _ -> onRequestPlay PlayDirection.Next)
                ]
                Button.create [
                    Button.content Icons.shuffle
                    Button.width 100.0
                    Button.horizontalAlignment HorizontalAlignment.Center
                    Button.classes [ "mediabtn" ]
                    Button.onClick (fun _ -> onShuffleRequested())
                ]
            ]
            StackPanel.height 100.0
        ]


    let progressBar (curr: int) (player: MediaPlayer) =
        StackPanel.create [
            StackPanel.verticalAlignment VerticalAlignment.Bottom
            StackPanel.horizontalAlignment HorizontalAlignment.Center
            StackPanel.orientation Orientation.Horizontal
            StackPanel.dock Dock.Bottom

            StackPanel.children [
                Slider.create [
                    Slider.minimum 0.0
                    Slider.maximum 100.0
                    Slider.value curr
                    Slider.width 428.0
                    Slider.horizontalAlignment HorizontalAlignment.Center

                    Slider.onValueChanged (fun value ->
                        if (int value) <> (int (float player.Position * 100.0)) then
                            player.Position <- (float value / 100.0) |> float32
                    )
                ]
            ]
        ]

    let playBar ( // (song: IWritable<Option<Song>>) = 
        songName: string,
        playerState: PlayState,
        progress: int,
        player: MediaPlayer,
        onPlayStateChange,
        onRequestPlay,
        onShuffleRequested
    ) =

        StackPanel.create [
            StackPanel.verticalAlignment VerticalAlignment.Bottom
            StackPanel.horizontalAlignment HorizontalAlignment.Center
            StackPanel.orientation Orientation.Vertical
            StackPanel.children [
                TextBlock.create [
                    TextBlock.text songName
                    TextBlock.horizontalAlignment HorizontalAlignment.Center
                    TextBlock.fontWeight FontWeight.Bold
                    TextBlock.fontSize 20.
                    TextBlock.margin (Thickness (0., 20., 0., 0.))
                ]

                mediaButtons (
                    playerState,
                    onPlayStateChange,
                    onRequestPlay,
                    onShuffleRequested
                )

                progressBar progress player
            ]
        ]

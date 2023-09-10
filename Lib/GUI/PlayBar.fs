namespace SharpBeat.Lib.GUI

module PlayBar =
    open Song
    open Avalonia.FuncUI
    open Avalonia.Controls
    open Avalonia.FuncUI.DSL
    open Avalonia.Layout
    open SharpBeat.Lib.GUI
    open LibVLCSharp.Shared
    open LibVLCSharp
    open PlayerLib
    open System

    open LibVLCSharp.Avalonia

    let mediaButtons = 
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
                ]
                Button.create [
                    Button.content Icons.play
                    Button.width 100.0
                    Button.horizontalAlignment HorizontalAlignment.Center
                ]
                Button.create [
                    Button.content Icons.next
                    Button.width 100.0
                    Button.horizontalAlignment HorizontalAlignment.Center
                ]
            ]
        ]


    let progressBar min max curr = 
        StackPanel.create [
            StackPanel.verticalAlignment VerticalAlignment.Bottom
            StackPanel.horizontalAlignment HorizontalAlignment.Center
            StackPanel.orientation Orientation.Horizontal
            StackPanel.dock Dock.Bottom

            StackPanel.children [
                Slider.create [
                    Slider.minimum min
                    Slider.maximum max
                    Slider.value curr
                    Slider.width 428.0
                    Slider.horizontalAlignment HorizontalAlignment.Center
                ]
            ]
        ]

    let playBar (song: IWritable<Option<Song>>) = 

        let player = getEmptyPlayer
        //let media = getMediaFromUri(new Uri(song.Current.Value.url()))
        let media = song.Current |>
                     function 
                     | Some(song) -> getMediaFromUri(new Uri(song.url()))
                     | None -> getMediaFromUri(new Uri("file://"))
        //let media = getMediaFromUri(new Uri("http://localhost:8080/c9244df1-2995-3938-a014-51b8e5cde4ac/outputlist.m3u8"))
    
        player.Media <- media
        player.Play() |> ignore

        StackPanel.create [
            StackPanel.verticalAlignment VerticalAlignment.Bottom
            StackPanel.horizontalAlignment HorizontalAlignment.Center
            StackPanel.orientation Orientation.Vertical
            StackPanel.dock Dock.Right
            StackPanel.children [
                mediaButtons
                progressBar 0 100 46
            ]
        ]

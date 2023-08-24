namespace SharpBeat.Lib.GUI

module Icons =
    open Avalonia.Controls
    open Avalonia.Controls.Shapes
    open Avalonia.FuncUI.DSL

    let shuffle =
        Canvas.create [
            Canvas.width 24.0
            Canvas.height 24.0
            Canvas.children [
                Path.create [
                    Path.fill "black"
                    Path.data "M17,3L22.25,7.5L17,12L22.25,16.5L17,21V18H14.26L11.44,15.18L13.56,13.06L15.5,15H17V12L17,9H15.5L6.5,18H2V15H5.26L14.26,6H17V3M2,6H6.5L9.32,8.82L7.2,10.94L5.26,9H2V6Z"
                ]
            ]
        ]

    let repeat =
        Canvas.create [
            Canvas.width 24.0
            Canvas.height 24.0
            Canvas.children [
                Path.create [
                    Path.fill "black"
                    Path.data "M17,17H7V14L3,18L7,22V19H19V13H17M7,7H17V10L21,6L17,2V5H5V11H7V7Z"
                ]
            ]
        ]

    let repeatOne =
        Canvas.create [
            Canvas.width 24.0
            Canvas.height 24.0
            Canvas.children [
                Path.create [
                    Path.fill "black"
                    Path.data "M13,15V9H12L10,10V11H11.5V15M17,17H7V14L3,18L7,22V19H19V13H17M7,7H17V10L21,6L17,2V5H5V11H7V7Z"
                ]
            ]
        ]

    let repeatOff =
        Canvas.create [
            Canvas.width 24.0
            Canvas.height 24.0
            Canvas.children [
                Path.create [
                    Path.fill "black"
                    Path.data "M2,5.27L3.28,4L20,20.72L18.73,22L15.73,19H7V22L3,18L7,14V17H13.73L7,10.27V11H5V8.27L2,5.27M17,13H19V17.18L17,15.18V13M17,5V2L21,6L17,10V7H8.82L6.82,5H17Z"
                ]
            ]
        ]

    let stop =
        Canvas.create [
            Canvas.width 24.0
            Canvas.height 24.0
            Canvas.children [
                Path.create [
                    Path.fill "black"
                    Path.data "M18,18H6V6H18V18Z"
                ]
            ]
        ]

    let play =
        Canvas.create [
            Canvas.width 24.0
            Canvas.height 24.0
            Canvas.children [
                Path.create [
                    Path.fill "black"
                    Path.data "M8,5.14V19.14L19,12.14L8,5.14Z"
                ]
            ]
        ]

    let pause =
        Canvas.create [
            Canvas.width 24.0
            Canvas.height 24.0
            Canvas.children [
                Path.create [
                    Path.fill "black"
                    Path.data "M14,19H18V5H14M6,19H10V5H6V19Z"
                ]
            ]
        ]

    let previous =
        Canvas.create [
            Canvas.width 24.0
            Canvas.height 24.0
            Canvas.children [
                Path.create [
                    Path.fill "black"
                    Path.data "M6,18V6H8V18H6M9.5,12L18,6V18L9.5,12Z"
                ]
            ]
        ]

    let next =
        Canvas.create [
            Canvas.width 24.0
            Canvas.height 24.0
            Canvas.children [
                Path.create [
                    Path.fill "black"
                    Path.data "M16,18H18V6H16M6,18L14.5,12L6,6V18Z"
                ]
            ]
        ]

    let create =
        Canvas.create [
            Canvas.width 24.0
            Canvas.height 24.0
            Canvas.children [
                Path.create [
                    Path.fill "black"
                    Path.data "M12,2C6.5,2 2,6.5 2,12C2,17.5 6.5,22 12,22C17.5,22 22,17.5 22,12C22,6.5 17.5,2 12,2M12,20C7.58,20 4,16.42 4,12C4,7.58 7.58,4 12,4C16.42,4 20,7.58 20,12C20,16.42 16.42,20 12,20M16.5,7H14.5V9H16.5C17.33,9 18,9.67 18,10.5V13.5C18,14.33 17.33,15 16.5,15H14.5V17H16.5C17.88,17 19,15.88 19,14.5V10.5C19,9.12 17.88,8 16.5,8M9.5,7H11.5V17H9.5V7Z"
                ]
            ]
        ]

    let remove =
        Canvas.create [
            Canvas.width 24.0
            Canvas.height 24.0
            Canvas.children [
                Path.create [
                    Path.data "M19,13H5V11H19V13Z"
                ]
            ]
        ]

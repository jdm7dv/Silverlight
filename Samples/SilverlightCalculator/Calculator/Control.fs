namespace Controls

open View
open ViewModel
open System.Windows.Controls

type CalculatorControl () as this =
    inherit UserControl (Width=240.0,Height=240.0)    
    let panel = StackPanel(Orientation=Orientation.Vertical)              
    do  let grid = Grid()
        display |> grid.Children.Add |> ignore
        ghostDisplay |> grid.Children.Add |> ignore
        grid |> panel.Children.Add |> ignore
    do keys |> panel.Children.Add |> ignore
    do this.Content <- panel
    let calculator = Calculator()           
    do  calculator.DisplayChanged.Add(fun _ -> fadeOut.Begin() )    
    do this.DataContext <- calculator
    

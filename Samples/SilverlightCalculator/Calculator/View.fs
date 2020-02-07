module View

open Keys
open Bindings

open System
open System.Globalization
open System.Windows
open System.Windows.Controls
open System.Windows.Data
open System.Windows.Media
open System.Windows.Media.Effects
open System.Windows.Media.Animation

let CreateTextBox (color,opactity) =
    TextBox(HorizontalContentAlignment=HorizontalAlignment.Right,
            Height=48.0,
            FontSize=32.0,
            Background=SolidColorBrush(color),
            Opacity=opactity,
            IsReadOnly=true)

let display =
    let binding = 
        Binding("Display",Mode=BindingMode.OneWay,StringFormat="0.#####")    
    CreateTextBox (Colors.White,1.0) + TextBox.TextBinding(binding)

let ghostDisplay =
    let binding = 
        Binding("LastDisplay",Mode=BindingMode.OneWay,StringFormat="0.#####")    
    CreateTextBox (Colors.Transparent,0.0) + TextBox.TextBinding(binding)       

let fadeOut =       
    let anim = DoubleAnimation()
    anim.Duration <- Duration(TimeSpan.FromSeconds(1./3.))
    anim.From <- Nullable(1.0)
    anim.To <- Nullable(0.0)    
    Storyboard.SetTarget(anim,ghostDisplay)
    Storyboard.SetTargetProperty(anim,PropertyPath("Opacity"))
    let story = Storyboard() 
    story.Children.Add anim
    story
    
let CreateValueConverter f =
    { new System.Windows.Data.IValueConverter with
        member this.Convert
            (value:obj,targetType:System.Type,parameter:obj,
             culture:System.Globalization.CultureInfo) =
            f(value,parameter)
        member this.ConvertBack
            (value:obj,targetType:System.Type,parameter:obj,
             culture:System.Globalization.CultureInfo) =
            raise (new System.NotImplementedException())
    } 

let operationEffectConverter = 
    fun (value:obj,parameter:obj) -> 
        let op = value :?> Operator option
        let key = parameter :?> Key
        match op,key with
        | Some(op),Operator(x) when op = x -> DropShadowEffect() :> Effect
        | _ -> null :> Effect
        |> box
    |> CreateValueConverter     

let keys =
    let grid = new Grid()
    for i = 1 to 4 do
        ColumnDefinition() |> grid.ColumnDefinitions.Add 
        RowDefinition() |> grid.RowDefinitions.Add
    [ 
    ['7',Digit(7);'8',Digit(8);'9',Digit(9);'/',Operator(Divide)]
    ['4',Digit(4);'5',Digit(5);'6',Digit(6);'*',Operator(Multiply)]
    ['1',Digit(1);'2',Digit(2);'3',Digit(3);'-',Operator(Minus)]
    ['0',Digit(0);'.',Dot;'=',Evaluate;'+',Operator(Plus)]
    ]    
    |> List.mapi (fun y ys ->
        ys |> List.mapi (fun x (c,key) ->
            let color =
                match key with
                | Operator(_) | Evaluate -> Colors.Yellow
                | Digit(_) | Dot -> Colors.LightGray                        
            let effect =
                Binding("Operator",
                        Converter=operationEffectConverter,
                        ConverterParameter=key)    
            Button(Content=c,CommandParameter=key,
                Width=40.0,Height=40.0,Margin=Thickness(4.0),
                Background=SolidColorBrush(color)) +                
                Button.CommandBinding(Binding("KeyCommand")) +
                Button.EffectBinding(effect) +
                Grid.Column(x) + Grid.Row(y)                         
        )
    )
    |> List.concat
    |> List.iter (grid.Children.Add >> ignore)
    grid         


module Bindings

open System.Windows
open System.Windows.Data

type DependencyPropertyBindingPair(dp:DependencyProperty,binding:Binding) =
    member this.Property = dp
    member this.Binding = binding
    static member (+) 
        (target:#FrameworkElement,pair:DependencyPropertyBindingPair) =
        target.SetBinding(pair.Property,pair.Binding) |> ignore
        target

type DependencyPropertyValuePair(dp:DependencyProperty,value:obj) =
    member this.Property = dp
    member this.Value = value
    static member (+) 
        (target:#UIElement,pair:DependencyPropertyValuePair) =
        target.SetValue(pair.Property,pair.Value)
        target

open System.Windows.Controls

type Button with
    static member CommandBinding (binding:Binding) = 
        DependencyPropertyBindingPair(Button.CommandProperty,binding)
    static member EffectBinding (binding:Binding) =
        DependencyPropertyBindingPair(Button.EffectProperty,binding)

type Grid with
    static member Column (value:int) =
        DependencyPropertyValuePair(Grid.ColumnProperty,value)
    static member Row (value:int) =
        DependencyPropertyValuePair(Grid.RowProperty,value)

type TextBox with
    static member TextBinding (binding:Binding) =
        DependencyPropertyBindingPair(TextBox.TextProperty,binding)


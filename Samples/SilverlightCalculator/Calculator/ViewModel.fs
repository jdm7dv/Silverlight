module ViewModel

let Command (action:'T -> unit) =
    let event = new DelegateEvent<System.EventHandler>()
    { new System.Windows.Input.ICommand with
        member this.Execute (param:obj) = action(param :?> 'T)
        member this.CanExecute (param:obj) = true
        [<CLIEvent>]
        member this.CanExecuteChanged = event.Publish
    }

open Keys

type Calculator() as this =   
    let propertyChanged = new Event<_,_>()
    let displayChanged = new Event<_>()
    let mutable display = 0M  
    let mutable lastDisplay = display  
    let mutable acc = None    
    let mutable operation = None        
    let PropertyChanged name = 
        (this,System.ComponentModel.PropertyChangedEventArgs(name)) 
        |> propertyChanged.Trigger
    let SetOperation value =
        operation <- value
        PropertyChanged "Operator"
        PropertyChanged "Operand"
    let SetDisplay value =
        lastDisplay <- display
        PropertyChanged "LastDisplay"
        (this,System.EventArgs()) |> displayChanged.Trigger 
        display <- value
        PropertyChanged "Display"        
    let Calculate () =
        match operation,acc with
        | Some(op,a), Some(b,_) -> 
            Operator.Eval op a b |> SetDisplay
        | _,_ -> ()
    let HandleKey = function
        | Digit(n) ->  
            let value,dp =       
                match acc with
                | Some(x,Some dp) ->                     
                    x + (dp * decimal n), Some(dp * 0.1M)                             
                | Some(x,None) ->                                          
                    (x * 10M) + decimal n, None                    
                | None -> decimal n, None
            acc <- Some(value,dp)
            value |> SetDisplay
        | Dot -> 
            acc <-
                match acc with 
                | Some(x,None) -> Some(x,Some(0.1M))
                | Some(x,Some(dp)) -> acc
                | None -> Some(0.0M,Some(0.1M))            
        | Operator(op) -> 
            Calculate ()
            SetOperation (Some(op,display))
            acc <- None     
        | Evaluate -> 
            Calculate ()
            SetOperation None
            acc <- None                      
    let command = Command(fun (param:Key) -> HandleKey (param))
    member this.KeyCommand = command                
    member this.Display = display 
    member this.LastDisplay = lastDisplay       
    member this.Operator = operation |> Option.map fst
    member this.Operand = operation |> Option.map snd    
    member this.DisplayChanged = displayChanged.Publish   
    interface System.ComponentModel.INotifyPropertyChanged with
        [<CLIEvent>]
        member this.PropertyChanged = propertyChanged.Publish

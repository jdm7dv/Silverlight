module Keys

type Operator =
    | Plus | Minus | Multiply | Divide
    static member Eval op (a:decimal) (b:decimal) =
        match op with
        | Plus -> a + b
        | Minus-> a - b
        | Multiply -> a * b
        | Divide -> a / b

type Key =    
    | Digit of int
    | Dot
    | Operator of Operator
    | Evaluate
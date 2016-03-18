type term = Var of string 
            | Const of string
            | Fn of string * (term list);;

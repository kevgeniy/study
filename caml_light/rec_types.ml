type ('a, 'b)sum = inl of 'a | inr of 'b;;

type ('a) list = Nil | Cons of 'a * ('a)list;;

type expression = Integer of int
                | Sum of expression * expression
                | Produce of expression * expression;;

type term = Var of string
            | Const of string
            | Comb of term * term
            | Abs of string * term;;

let rec free_in x =
    fun (Var v) -> x = v
    | (Const c) -> false
    | (Comb(s,t)) -> free_in x s or free_in x t
    | (Abs(v, t)) -> not (x = v) & free_in x t;;

let rec variant =
    fun x t ->
        if free_in x t 
        then variant (x^"'") t
        else x;;

let rec subst = 
    fun u (s, x) ->
        match u with
        Var y -> if x = y then s else Var y
        | Const c -> Const c
        | Comb(s1, s2) -> Comb(subst s1 (s,x), subst s2 (s,x))
        | Abs(y, t) ->  if not (free_in x (Abs(y, t))) then Abs(y, t)
                        else if (free_in y s) then 
                            let z = variant y (Comb(s, t)) in
                            Abs (z, subst(subst t (Var y, z)) (s, x))
                        else Abs(y, subst t (s, x));;


let rec Rec = fun f -> f (fun x -> Rec f x);;

let fact = Rec (fun f n -> if n = 0 then 1 else n * f(n - 1));;

type ('a)embedding = K of ('a) embedding -> 'a;;

let Y h =
    let g (K x) z = h (x (K x)) z in
    g (K g);;

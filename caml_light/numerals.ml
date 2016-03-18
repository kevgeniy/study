let rec to_church = fun n f x ->
    if n = 0 then x
    else to_church (n - 1) f (f x);;

let from_church = fun n -> n (fun x -> x + 1) 0;;

let add = fun m n f x ->  m f (n f x);;

let mul = fun m n f x -> m (n f) x;;

let exp = fun m n f x -> n m f x;;

let test bop x y = from_church(bop (to_church x) (to_church y));;

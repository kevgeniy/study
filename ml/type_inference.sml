(* f : T1 -> T2 [must be a function; all functions take 1 arg]
* x : T1
* y : T3
* z : T4
*
* T1 = T3 * T4 [else pattern match does not type-check]
* T3 = int [abs has type int -> int]
* T4 = int [because we added z to an int]
* so T1 = int * int
* T2 = int
* f ; int * int -> int
*)
fun f x =
let val (y, z) = x in
  (abs y) + z
end

(*
* sum : T1 -> T2
* xs : T1
* x : T3
* xs : T3 list [pattern matching]
*
* T1 = T3 list
* T2 = int [because 0 might be returned]
* T3 = int [because x : T3 and we add x to something]
*
* from T1 = T3 list and T3 = int, we know T1 = int list
* from that and T2 = int, we know sum : int list -> int
*)
fun sum xs = 
  case xs of
       [] => 0

(* sum : T1 -> T2
* xs : T1
* x : T3
* xs' : T3 list
* xs : T3 list
*
* T2 = int
* T3 = int
* T1 = int list
*)
fun sum xs = 
  case xs of
       [] => 0
     | x::xs' => x + (sum xs')
(* length : T1 -> T2
*  xs : T1
*  x : T3
*  xs' : T3 list
*
*  T1 = T3 list
*  T2 = int
*
*  That's all. length : T3 list -> int
*
*  T3 list -> int
*  'a list -> int
*)
fun length xs =
  case xs of
       [] => 0
     | _::xs' => 1 + length xs'
     
(* f : T1 -> T2
* x : T3, y : T4, z : T5
* T1 = T3 * T4 * T5
* T2 = T3 * T4 * T5
* T2 = T4 * T3 * T5
*
* T3 = T4
*
* f : T3 * T3 * T5 -> T3 * T3 * T5
* f : 'a * 'a * 'b -> 'a * 'a * 'b
*)
fun f (x, y, z) =
  if true
  then (x, y, z)
  else (y, x, z)

(* compose : T1 * T2 -> T3
*  f : T1
*  g : T2
*  x : T4
*  
*  T2 = T4 -> T5
*  T1 = T5 -> T6
*  T3 = T4 -> T6
*
*  compose : (T5 -> T6) * (T4 -> T5) -> T4 -> T6
*  compose : ('a -> 'b) * ('c -> 'a) -> 'c -> 'b
*)
fun compose (f, g) = fn x => f(g x)


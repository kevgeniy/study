fun hd xs =
  case xs of
       []                  => raise           List.Empty
     | x::_ =>            x
exception MyUndesirableCondition

exception  MyOtherException of int * int

fun mydiv (x, y) = 
  if y = 0
  then raise MyUndesirableCondition
  else x div y
fun maxlist (xs, ex) = (* int list * exn => int *)
  case xs of 
       [] => raise ex
     | x::[] => raise MyOtherException(4, 3)
     | x::xs' => Int.max(x, maxlist(xs', ex))

(* e1 handle ex => e2 *)
val z = maxlist([3, 3], MyUndesirableCondition)
  handle MyOtherException(i, j) => i + j
    | MyUndersirableCondition => 42


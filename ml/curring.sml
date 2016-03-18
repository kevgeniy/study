fun sorted3_tupled(x, y, z) = z >= y andalso y >= x
val t1 = sorted3_tupled(7, 9, 11)

(* new way - curring *)

val sorted3 = fn x => fn y => fn z => z >=y andalso y >= x

val t2 = sorted3 7 9 11

(* helper functions *)

fun uncurry f (x, y) = f x y

fun curry f x y = f(x, y)

fun other_curry f x y => f y x

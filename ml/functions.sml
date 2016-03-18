fun n_times(f, n, x) =
  if n = 0
  then x
  else f(n_times(f, n - 1, x))

fun doubling x = x + x

fun increment_n_times(n, x) = 
let
  fun increment x = x + 1
in
  n_times(increment, n, x)
end

fun double_n_times(n, x) = n_times(doubling, n, x)
fun nth_tail(n, x) = n_times(tl, n, x)

fun map (f, xs) =
  case xs of
       [] => []
     | head :: tail => f(head) :: map(f,tail)

val x1 = map((fn x => x + 1), [4, 8, 12, 16])
val x2 = map(hd, [[1, 2], [3, 4], [5, 6, 7]])

fun filter(f, xs) =
  case xs of
       [] => []
     | head :: tail => if f head
                       then head :: (filter(f, tail))
                       else filter(f, tail)

fun is_even v =
  (v mod 2 = 0)
fun all_even_and xs = filter((fn (_, v) => is_even v), xs)
  (*
fun all_even_snd xs = filter((fn (_, v) => is_even v), xs)
  *)

fun fold(f, acc, xs) = 
  case xs of
       [] => acc
     | x :: xs => fold(f, f(acc, x), xs)

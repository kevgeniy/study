datatype 'a myList = Empty
                | Cons of 'a * 'a myList

fun map f xs =
  case xs of
       Empty => Empty
     | Cons(head, tail) => Cons(f head, map f tail)

fun filter f xs =
  case xs of
       Empty => Empty
     | Cons(head, tail) => if f head
                           then Cons(head, filter f xs)
                           else filter f xs
fun length xs =
  case xs of
       Empty => 0
     | Cons(_, tail) => 1 + length tail

val doubleAll = map (fn x => x * 2)

fun countNs (xs, n) = length (filter (fn x => x = n) xs )

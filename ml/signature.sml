signature MATHLIB = 
sig
  val fact : int -> int
  val half_pi : real
  val doubler : int -> int
end

structure MyMathLib :> MATHLIB =
struct
  fun fact x =
    if x = 0
    then 1
    else x * fact (x - 1)

  val half_pi = Math.pi / 2.0
  
  fun doubler y = y + y
end

val pi = MyMathLib.half_pi + MyMathLib.half_pi

val twenty_eight = MyMathLib.doubler 14

signature RATIONAL_A =
sig
  type rational
  exception BadFrac
  val Whole : int -> rational
  val make_frac : int * int -> rational
  val add : rational * rational -> rational
  val toString : rational -> string
end

structure Rational1 :> RATIONAL_A = 
struct
  (* Invariant 1: all denominators > 0
  * Invariant 2: rationals kept in reduced form *)

  datatype rational = Whole of int | Frac of int * int
  exception BadFrac

  (* gcd and reduce help keep fractions reduced,
  * but clients need not know about them *)
  (* they _assume_ their inputs are not negative *)

  fun gcd (x, y) =
    if x = y
    then x
    else if x < y
    then gcd (x, y - x)
    else gcd (x - y, y)

  fun reduce r =
    case r of
         Whole _ => r
       | Frac(x, y) =>
           if x = 0
           then Whole 0
           else
             let val d = gcd(abs x, y) in
               if d = y
               then Whole(x div d)
               else Frac(x div d, y div d)
             end

  (* zero denominators are baned *)
  fun make_frac (x, y) =
    if y = 0
    then raise BadFrac
    else if y < 0
    then reduce(Frac(~x, ~y))
    else reduce(Frac(x, y))

  fun add (r1, r2) =
    case (r1, r2) of
         (Whole(i), Whole(j)) => Whole(i + j)
       | (Whole(i), Frac(j, k)) => Frac(j + k * i, k)
       | (Frac(j, k), Whole(i)) => Frac(j + k * i, k)
       | (Frac(a, b), Frac(c, d)) => reduce(Frac(a * d + b * c, b * d))

  fun toString r =
    case r of
         Whole i => Int.toString i
       | Frac(a, b) => (Int.toString a) ^ "/" ^ (Int.toString b)
end  


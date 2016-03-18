val only_capitals = List.filter (fn line => Char.isUpper (String.sub (line, 0)))

val longest_string1 = foldl (fn (a, b) => if String.size a > String.size b 
                                          then a 
                                          else b) ""

val longest_string2 = foldl (fn (a, b) => if String.size a >= String.size b 
                                          then a 
                                          else b) ""

fun longest_string_helper f = foldl (fn (a, b) => if f(String.size a, String.size b) 
                                          then a 
                                          else b) ""

val longest_string3 = longest_string_helper (fn (a, b) => a > b)

val longest_string4 = longest_string_helper (fn (a, b) => a >= b)

val longest_capitalized = longest_string3 o only_capitals

val rev_string = implode o rev o explode

exception NoAnswer

fun first_answer f xs =
  case xs of
       [] => raise NoAnswer
     | head :: tail => case f head of
                            SOME v => v
                          | _ => first_answer f tail

fun all_answers f xs = 
let
  fun all_answers_tail acc xs =
    case xs of
         [] => SOME acc
       | head :: tail => case f head of
                              NONE => NONE
                            | SOME lst => all_answers_tail (acc @ lst) tail
in
  all_answers_tail [] xs
end

datatype pattern = Wildcard
                 | Variable of string
                 | UnitP
                 | ConstP of int
                 | TupleP of pattern list
                 | ConstructorP of string * pattern

datatype valu = Const of int
              | Unit
              | Tuple of valu list
              | Constructor of string * valu

fun g f1 f2 p =
let 
	val r = g f1 f2 
in
  case p of
       Wildcard          => f1 ()
     | Variable x        => f2 x
     | TupleP ps         => List.foldl (fn (p,i) => (r p) + i) 0 ps
     | ConstructorP(_,p) => r p
     | _                 => 0
	end
val count_wildcards = g (fn () => 1) (fn x => 0)

val count_wild_and_variable_lengths = g (fn () => 1) (fn x => String.size x)

fun count_some_var(s, p) = g (fn () => 0) (fn x => if x = s then 1 else 0) p

val check_pat =
let
  fun get_list p =
    case p of
         Variable x         => [x]
       | TupleP ps          => List.foldl (fn (p, xs) => (get_list p) @ xs) [] ps
       | ConstructorP(_, p) => get_list p
       | _                  => []
    fun are_distinct xs =
      case xs of
           [] => true
         | head :: tail => (not (List.exists (fn x => x = head) tail)) andalso are_distinct tail
in
    are_distinct o get_list
end


fun match (v, p) =
    case p of
         Wildcard => SOME[]
       | Variable name => SOME[(name, v)]
       | UnitP => (case v of
                       Unit => SOME[]
                     | _ => NONE)
       | ConstP c1 => if v = Const(c1) then SOME[] else NONE
       | TupleP ps => (case v of
                           Tuple ps' => (all_answers 
                           (fn (el, el') => match (el, el')) (ListPair.zipEq(ps',
                           ps)) 
                           handle _ => NONE)
                         | _ => NONE)
       | ConstructorP(name, value) => (case v of
                                           Constructor(name', value') => 
                                                if name = name' 
                                                then match (value',value)
                                                else NONE
                                         | _ => NONE)

fun first_match v xsp =
let fun curry f x y = f(x, y) in
  (SOME (first_answer (curry match v) xsp)) handle NoAnswer => NONE
end

datatype set = S of {   insert : int -> set,
                        is_member : int -> bool, 
                        size : unit -> int }

fun empty_list () =
let
  fun make_list xs =
  let
    fun length xs =
      case xs of
           [] => 0
         | _ :: tail => 1 + length tail
    fun contains_in collection element =
      case collection of
           [] => false
         | head :: tail => if head = element
                            then true
                            else contains_in tail element
  in
    S { insert = fn(element) =>  if contains_in xs element
                                 then make_list xs
                                 else make_list (element :: xs),
        is_member = contains_in xs,
        size = fn () => length xs }
  end
in
  make_list []
end


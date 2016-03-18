(* exceptions *)

exception Died;;

exception Failed of string;;

(* raise (Failed "I don't know why");; *)

exception Head_of_empty;;

let hd = fun [] -> raise Head_of_empty
            | (h::t) -> h;;

let headstring sl = 
    try hd sl
    with Head_of_empty -> ""
    | Failed s -> "Failure because "^s;;


(* traditional variables, access through references *)

let x = ref 1;;
!x;;

x := 2;;
!x;;

x := !x + !x;;
x;;

let contents_of (ref x) = x;;

(* arrays - vectors *)

let v = make_vect 10 3;;

vect_item v 3;;
vect_assign v 3 2;;
vect_item v 3;;




let rec fold f =
    fun [] b -> b
        | (h :: t) b -> f h (fold f t b);;

let sum xs = fold (prefix +) xs 0;;

let product xs = fold (prefix *) xs 1;;

let filter p xs = fold (fun hd tl -> if p(hd) 
                                     then (hd::tl) 
                                     else tl) xs [];;

let forall p xs = fold (fun hd tl -> p(hd) & tl) xs true;;

let exist p xs = fold (fun hd tl -> p(hd) or tl) xs false;;

let length xs = fold (fun hd tl -> tl + 1) xs 0;;

let append xs bs = fold :: xs bs;;



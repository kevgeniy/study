fun quick [] = []
    | quick [x] = [x]
    | quick (a::bs) = 
        let fun partition (left, right, []) : real list = 
                (quick left) @ (a :: quick right)
            | partition (left, right, x::xs) =
                if x <= a then partition (x::left, right, xs)
                else partition(left, x::right, xs)
        in partition([], [], bs) end;

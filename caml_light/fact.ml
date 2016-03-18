let fact = let rec fact_fast = fun ans n -> 
    if n = 0 
    then ans 
    else fact_fast (ans * n) (n - 1) in
    fact_fast 1;;

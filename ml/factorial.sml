fun fact(n, x) =
  case n of
    0 => x
    | _ => fact(n - 1, x * n)

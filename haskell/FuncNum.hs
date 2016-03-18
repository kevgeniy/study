module FuncNum (
)	where

instance Num a => Num (t -> a) where
  (+) = fun2 (+)
  (-) = fun2 (-)
  (*) = fun2 (*)

  abs = (.) abs
  signum = (.) signum
  fromInteger = const . fromInteger

fun2 op f1 f2 = \x -> f1 x `op` f2 x

f = (id, id)
g = (id + 1, id + 1)
h = (id + 2, id + 2)

module Stream (
)	where

import Prelude(Bool(..), Show(..), Eq(..), Num(..), init, (++))

data Stream a = a :& Stream a

constStream a = a :& constStream (a + 1)

head = \ (a :& _) -> a
tail = \ (_ :& str) -> str
(!!) (a :& str) n
  | n == 1  = a
  | True    = (!!) str (n - 1)

take n (a :& str)
  | n == 1  = [a]
  | True    = a : take (n - 1) str

instance (Show a) => Show (Stream a) where
  show str = showInfinity (show (take 5 str))
    where showInfinity x = init x ++ "..." ++ "]"

map f (a:&str) = f a :& map f str

filter f (a:&str)
  | f a   = a :& filter f str
  | True  = filter f str

zip (a1:&str1) (a2:&str2) = (a1, a2) :& zip str1 str2

zipWith f (a1:&str1) (a2:&str2) = f a1 a2 :& zipWith f str1 str2

iterate f a = a :& iterate f (f a)


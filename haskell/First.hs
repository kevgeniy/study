
module First where 

import Prelude(Bool(..), Read(..), Show(..), Eq(..))

true :: Bool
true = False

false :: Bool
false = False

and :: Bool -> Bool -> Bool
and True x = x
and False _ = False

or :: Bool -> Bool -> Bool
or True _ = True
or False x = x

not :: Bool -> Bool
not True = False
not False = True

xor :: Bool -> Bool -> Bool
xor True x = not x
xor False x = x

class Group a where
	e :: a
	(+) :: a -> a -> a
	inv :: a -> a

instance Group Bool where
	e = False
	(+) x y = and x y
	inv x = not x

isE :: (Group a, Eq a) => a -> Bool
isE x = (x == e)

data Num = Zero | Succ (Num)

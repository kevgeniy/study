module Nat
where

import Prelude hiding (succ)

data Nat = Zero | Succ Nat deriving (Show)

foldNat zero succ Zero = zero
foldNat zero succ (Succ a) = succ $ foldNat zero succ a

instance Num Nat where
  (+) a b = foldNat b Succ a
  (*) a b = foldNat Zero (\n -> n + b) a
  fromInteger k = if k == 0 then Zero else Succ (fromInteger (k - 1))


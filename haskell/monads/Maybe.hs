-- ПРИМЕР: ЧАСТИЧНО ОПРЕДЕЛЕННЫЕ ФУНКЦИИ

module Maybe
where
import Prelude hiding (Maybe(..), maybe, (>>), pred)
import Category
import Nat

data Maybe a = Nothing | Just a deriving (Show)

pred :: Nat -> Maybe Nat
pred Zero = Nothing
pred (Succ a) = Just a

-- а хотим pred2 = pred >> pred 
-- pred3 = pred2 >> pred = pred >> pred >> pred

--Аналог foldr
maybe :: t1 -> (t2-> t1) -> Maybe t2 -> t1
maybe n _ Nothing = n
maybe _ f (Just x) = f x

--Теперь можно написать так:
instance Kleisli Maybe where
  idK = Just
  f *> g = f >> maybe Nothing g

{-- Без maybe пришлось бы так:
  f *> g = \x -> case f x of
                  Just a -> g a
                  _ -> Nothing
--}

-- Теперь можно использовать *> и +>
pred2 :: Nat -> Maybe Nat
pred2 = pred *> pred
pairPred :: Nat -> Maybe (Nat, Nat)
pairPred = pred +> \a -> (a, a + 2)


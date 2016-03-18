--Категории, обобщение функций и их комбинаций

module Category
where

import Prelude hiding (id, (>>), Maybe(..), pred, maybe, ($), succ, sequence) 

class Category cat where
  id :: cat a a
  (>>) :: cat a b -> cat b c -> cat a c

instance Category (->) where
  id = \x->x
  f >> g = \x -> g (f x)

class Kleisli m where
  idK :: a -> m a
  (*>) :: (a -> m b) -> (b -> m c) -> (a -> m c)

(+>) :: Kleisli m => (a->m b) -> (b -> c) -> (a -> m c)
f +> g = f *> (g >> idK)

--применить n раз
generate :: Kleisli m => Int -> (a -> m a) -> a -> m a
generate 0 _ = idK
generate n f = f *> generate (n-1) f

generate2 :: Kleisli m => Int -> (a -> m a) -> a -> m a
generate2 n f = iterate (*>f) idK !! n

--Для многозначных функций
instance Kleisli [] where
  idK = \a -> [a]
  f *> g = f >> map g >> concat

-----------------------------------------------------------
--Nat
-----------------------------------------------------------

data Nat = Zero | Succ Nat deriving (Show)

foldNat :: a -> (a -> a) -> Nat -> a
foldNat zero _ Zero = zero
foldNat zero succ (Succ a) = succ $ foldNat zero succ a

instance Num Nat where
  (+) a b = foldNat b Succ a
  (*) a b = foldNat Zero (\n -> n + b) a
  fromInteger k = if k == 0 then Zero else Succ (fromInteger (k - 1))

-----------------------------------------------------------
--Maybe
-----------------------------------------------------------
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

-----------------------------------------------------------
--FuncApply
-----------------------------------------------------------

($) :: (a -> b) -> a -> b
f $ a = (const a >> f) ()

(*$) :: Kleisli m => (a -> m b) -> m a -> m b
f *$ a = (const a *> f) ()

(+$) :: Kleisli m => (a -> b) -> m a -> m b
f +$ a = (const a +> f) ()

($$) :: Kleisli m => m (a -> b) -> m a -> m b
mf $$ ma = (+$ ma) *$ mf

lift1 :: Kleisli m => (a->b) -> m a -> m b
lift1 = (+$)

lift2 :: Kleisli m => (a -> b -> c) -> m a -> m b -> m c
lift2 f a b = lift1 f a $$ b

lift3 :: Kleisli m => (a -> b -> c -> d) -> m a -> m b -> m c -> m d
lift3 f a b c = lift2 f a b $$ c

--  liftN f a1 ... an = liftN-1 f a1 ... an-1 $$ an

--Интересный пример:
-- [(+1), (+2), (+3)] $$ [10, 20, 30] = [11,21,31,12,22,32,13,23,33]

-----------------------------------------------------------
--Useful Functions
-----------------------------------------------------------

sequence :: Kleisli m => [m a] -> m [a]
sequence = foldr (lift2 (:)) (idK [])

mapK :: Kleisli m => (a -> m b) -> [a] -> m [b]
mapK f = sequence . map f




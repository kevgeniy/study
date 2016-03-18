module FuncApply
where
import Prelude hiding ((>>))
import Category

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


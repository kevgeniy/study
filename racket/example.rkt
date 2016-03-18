; Comment
; This is example file

#lang racket

(provide (all-defined-out))

(define x 3) ; val x = 3
(define y (+ x 2)) ; + is no more than a function)

(define cube1 
  (lambda (x) 
    (* x (* x x)))) ; x * (x * x)
; lambda is like ML's fn - anonymus function

(define cube2
  (lambda (x y)
         (* x x x))) ; because * can take any number of arguments

(define (cube3 x y)
  (* x x y))

(define (pow1 x y) ; x to yth power (y must be nonnegative)
  (if (= y 0)
      1
      (* x (pow1 x (- y 1)))))

(define pow2
  (lambda (x)
    (lambda (y)
      (pow1 x y))))

(define three-to-the (pow2 3))


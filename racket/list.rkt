#lang racket

(provide (all-defined-out))

; sum all elements off the list
(define sum-list
  (lambda (xs)
    (if (null? xs)
        0
        (+ (car xs) (sum-list (cdr xs))))))

; append
(define (my-append xs ys)
  (if (null? ys)
      null
      (my-append (cons xs (car ys)) (cdr ys))))

; another append
(define (my-append2 xs ys)
  (if (null? xs)
      ys
      (cons (car xs) (my-append (cdr xs) ys))))

; map
(define (my-map f xs)
  (if (null? xs)
      null
      (cons (f (car xs)) (my-map f (cdr xs)))))

; fold
(define (fold f acc xs)
  (if (null? xs)
      acc
      (fold f (f acc (car xs)) (cdr xs))))

; reverse fold
(define (rev-fold f acc xs)
  (if (null? xs)
      acc
      (f (fold f acc (cdr xs)) (car xs))))
      
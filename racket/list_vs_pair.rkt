#lang racket

(define pr (cons 1 (cons #t "hi")))

(define lst (cons 1 (cons #t (cons "hi" null))))


; mutable lists

(define x (mcons 1 (mcons 4 "hi")))

(define y x)

(set-mcar! x 4)

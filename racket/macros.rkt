#lang racket

(provide (all-defined-out))

(define-syntax my-if
  (syntax-rules (then else)
    [(my-if e1 then e2 else e3)
     (if e1 e2 e3)]
    [(my-if e1 else e2)
     (if e1 0 e2)]))

(define-syntax comment-out
  (syntax-rules ()
    [(comment-out ignore instead) instead]))

(define-syntax for
  (syntax-rules (to do)
    [(for input to output do res)
     (let ([in input]
           [out output])
       (letrec ([loop (lambda (it)
                        (cond [(> it out) #f]
                               [(= it out) res]
                               [#t (begin res (loop (+ it 1)))]))])
         (loop in)))]))
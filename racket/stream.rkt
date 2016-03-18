#lang racket

(provide (all-defined-out))

(define (number-until stream tester)
  (letrec ([f (lambda (stream ans)
                (let ([pr (stream)])
                  (if (tester (car pr))
                      ans
                      (f (cdr pr) (+ ans 1)))))])
    (f stream 1)))

; 1 1 1 1 1 1 1 ...
(define ones (lambda () (cons 1 ones)))

(define (twos) (letrec ([pow-of-two (lambda (x) (cons x (lambda () (pow-of-two (* 2 x)))))
                                            ])
                         (pow-of-two 1)))

(define (f x) (cons x (lambda () (f (+ x 1)))))
(define (nats) (f 1))


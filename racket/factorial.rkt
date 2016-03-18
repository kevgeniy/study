#lang racket

(provide (all-defined-out))

(define fact
  (lambda (n x)
    (if (= n 0)
      x
      (fact (- n 1) (* x n)))))


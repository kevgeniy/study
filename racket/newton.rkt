#lang racket

(provide (all-defined-out))

(define epsilon1 0.01)
(define epsilon2 0.0001)
(define lst-1 (list 1 -2.52 -16.1 17.3 -1 -1.34))
(define lst-2 (list 1 -9 0 16 20))
(define lst-3 (list 1 12 -6 -18))


(define (find-solutions xs)
  (append (devide-conquero xs (/ 1 (find-boundary (inverse-x xs))) (find-boundary xs))
        (devide-conquero xs (- (find-boundary (neg-x xs))) (/ -1 (find-boundary (neg-x (inverse-x xs)))))))

(define (find-boundary xs)
  (+ 1 (expt (/ (abs (min-neg-smart xs)) (car xs)) (/ 1 (first-neg-num xs)))))

(define (neg-x xs)
  (letrec [(f (lambda (xs n)
                (if (null? xs)
                    null
                    (if (= 0 (remainder n 2))
                        (cons (car xs) (f (cdr xs) (- n 1)))
                        (cons (* -1 (car xs)) (f (cdr xs) (- n 1)))))))]
    (normalize (f xs (- (length xs) 1)))))

(define (normalize xs)
    (if (< (car xs) 0)
        (map (lambda (s) (* -1 s)) xs)
        xs))

(define (inverse-x xs)
    (letrec [(f (lambda (xs new-xs)
                  (if (null? (cdr xs))
                      (cons (car xs) new-xs)
                      (f (cdr xs) (cons (car xs) new-xs)))))
             (new-xs (reverse xs))]
      (normalize new-xs)))

(define (first-neg-num xs)
    (letrec [(f (lambda (xs n)
                  (if (< (car xs) 0)
                      n
                      (f (cdr xs) (+ n 1)))))]
      (f xs 0)))

(define (min-neg-smart xs)
  (foldl (lambda (a result)
           (if (and (< a 0) (< a result))
               a
               result)) 0 xs))

(define devide-conquero
  (lambda (xs a b)
    (if (> a b)
        null
        (if (< (* (f xs a) (f xs (+ a epsilon1))) 0)
            (cons (find-solution xs (+ a (/ epsilon1 2))) (devide-conquero xs (+ a epsilon1) b))
            (devide-conquero xs (+ a epsilon1) b)))))

(define (find-solution xs p-old)
    (letrec [(p-new (- p-old (/ (f xs p-old) (f-p xs p-old))))]
      ;(if (or (< (abs (- p-old p-new)) epsilon2) (< (abs (f xs p-new)) epsilon2))
      (if (< (abs (f xs p-new)) epsilon2)
          p-new
          (find-solution xs p-new))))

; compute f(x0)
(define (f xs x0)
    (letrec [(func (lambda (xs n)
                  (if (= n 0)
                      (car xs)
                      (+ (* (car xs) (expt x0 n)) (func (cdr xs) (- n 1))))))]
      (func xs (- (length xs) 1))
    ))

;compute f'(x0)
(define (f-p xs x0)
    (letrec [(f-pro (lambda (xs n)
                  (if (= n 0)
                      null
                      (cons (* (car xs) n) (f-pro (cdr xs) (- n 1))))))]
      (f (f-pro xs (- (length xs) 1)) x0)))
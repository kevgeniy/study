#lang racket

(provide (all-defined-out))

(define (sequence low high stride)
  (if (> low high)
      null
      (cons low (sequence (+ low stride) high stride))))

(define (string-append-map xs suffix)
  (map (lambda (str) (string-append str suffix)) xs))

(define (list-nth-mod xs n)
  (cond [(< n 0) (error "list-nth-mod: negative number")]
        [(null? xs) (error "list-nth-mod: empty list")]
        [#t (car (list-tail xs (remainder n (length xs))))]))

(define (stream-for-n-steps s n)
  (if (= n 0)
      null
      (let ([pr (s)])
      (cons (car pr) (stream-for-n-steps (cdr pr) (- n 1))))))

(define (funny-number-stream)
  (letrec ([f (lambda (n)
                (if (= (remainder n 5) 0)
                    (cons (- 0 n) (lambda () (f (+ n 1))))
                    (cons n (lambda () (f (+ n 1))))))])
    (f 1)))

(define (dan-then-dog) 
  (let ([f (lambda () (cons "dog.jpg" dan-then-dog))])
    (cons "dan.jpg" f)))

(define (stream-add-zero s)
  (let ([pr (s)])
    (lambda () (cons (cons 0 (car pr)) (lambda () ((stream-add-zero (cdr pr))))))))

(define (cycle-lists xs ys)
  (letrec ([f (lambda (n) 
                (cons (cons (list-nth-mod xs n) (list-nth-mod ys n)) (lambda () (f (+ n 1)))))])
    (lambda () (f 0))))

(define (vector-assoc v vec)
  (letrec ([len (vector-length vec)]
           [f (lambda (pos) 
                (if (= pos len)
                    #f
                    (let ([elem (vector-ref vec pos)])
                        (if (and (pair? elem) (equal? (car elem) v))
                            elem
                            (f (+ pos 1))))))])
    (f 0)))

(define (cached-assoc xs n)
  (letrec ([pos 0]
           [vec (make-vector n #f)]
           [f (lambda (v) 
                (let ([val (vector-assoc v vec)])
                  (if val
                      val
                      (let ([find_val (assoc v xs)])
                           (if (not find_val)
                               #f
                               (begin (vector-set! vec pos find_val)
                                      (set! pos (if (= pos (- n 1)) 0 (+ pos 1)))
                                      find_val))))))])
    f))
                                
(define-syntax while-less
  (syntax-rules (do)
        [(while con do ex)
         (letrec ([condition con]
                  [f (lambda ()
                       (let ([res ex])
                         (if (>= res condition)
                             #t
                             (f))))])
           (f))]))

    

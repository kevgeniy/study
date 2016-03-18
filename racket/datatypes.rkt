#lang racket
(provide (all-defined-out))

(define (funny-sum xs)
  (cond [(null? xs) 0]
        [(number? (car xs)) (+ (car xs) (funny-sum (cdr xs)))]
        [(string? (car xs)) (+ (string-length (car xs)) (funny-sum (cdr xs)))]))

(define (Const e) (list 'Const e))
(define (Negate e) (list 'Negate e))
(define (Add e1 e2) (list 'Add e1 e2))
(define (Multiply e1 e2) (list 'Multiply e1 e2))

(define (Const? e) (eq? (car e) 'Const))
(define (Negate? e) (eq? (car e) 'Negate))
(define (Add? e) (eq? (car e) 'Add))
(define (Multiply? e) (eq? (car e) 'Multiply))

(define (Const_e e) (car (cdr e)))
(define (Negate_e e) (car (cdr e)))
(define (Add_e1 e) (car (cdr e)))
(define (Add_e2 e) (car (cdr (cdr e))))
(define (Multiply_e1 e) (car (cdr e)))
(define (Multiply_e2 e) (car (cdr (cdr e))))

(define (eval-exp e)
  (cond [(Const? e) e]
        [(Negate? e) (Const (- (Const_e (eval-exp (Negate e)))))]
        [(Add? e) (Const (+ (Const_e (eval-exp (Add_e1 e))) (Const_e (eval-exp (Add_e2 e)))))]
        [(Muыltiply? e) (Const (let ([v1 (Const_e (eval-exp (Multiply_e1 e)))]
                                    [v2 (Const_e (eval-exp (Multiply_e2 e)))])
                              (* v1 v2)))]
        [#t error "eval-exp expected an exp"]))

(struct const (int) #:transparent)
(struct negate (e) #:transparent)
(struct add (e1 e2) #:transparent)
(struct multiply (e1 e2) #:transparent)

(define (eval-exp-new e)
  (cond [(const? e) e]
        [(negate? e) (const (- (const-int (eval-exp-new (negate-e e)))))]
        [(add? e) (const (+ (const-int (eval-exp-new (add-e1 e))) (const-int (eval-exp-new (add-e2 e)))))]
        [(multiply? e) (const (* (const-int (eval-exp-new (multiply-e1 e))) (const-int (eval-exp-new (multiply-e2 e)))))]
        [#t error "eval-exp-new expected exp"]))
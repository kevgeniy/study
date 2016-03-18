	.file	"main.c"
	.text
	.globl	getsim
	.type	getsim, @function
getsim:
.LFB39:
	.cfi_startproc
	pushq	%rbp
	.cfi_def_cfa_offset 16
	.cfi_offset 6, -16
	pushq	%rbx
	.cfi_def_cfa_offset 24
	.cfi_offset 3, -24
	subq	$8, %rsp
	.cfi_def_cfa_offset 32
	movq	%rdi, %rbp
	movq	%rdi, %rbx
	jmp	.L2
.L4:
	movb	%al, (%rbx)
	leaq	1(%rbx), %rbx
.L2:
	movq	stdin(%rip), %rdi
	call	_IO_getc
	cmpl	$10, %eax
	je	.L3
	cmpl	$-1, %eax
	jne	.L4
.L3:
	cmpl	$-1, %eax
	sete	%dl
	cmpq	%rbp, %rbx
	sete	%al
	testb	%al, %dl
	je	.L6
	movl	$0, %eax
	jmp	.L5
.L6:
	movq	%rbp, %rax
.L5:
	addq	$8, %rsp
	.cfi_def_cfa_offset 24
	popq	%rbx
	.cfi_def_cfa_offset 16
	popq	%rbp
	.cfi_def_cfa_offset 8
	ret
	.cfi_endproc
.LFE39:
	.size	getsim, .-getsim
	.globl	echo
	.type	echo, @function
echo:
.LFB40:
	.cfi_startproc
	subq	$24, %rsp
	.cfi_def_cfa_offset 32
	movq	%rsp, %rdi
	call	getsim
	movq	%rsp, %rdi
	call	puts
	addq	$24, %rsp
	.cfi_def_cfa_offset 8
	ret
	.cfi_endproc
.LFE40:
	.size	echo, .-echo
	.globl	main
	.type	main, @function
main:
.LFB41:
	.cfi_startproc
	movl	$0, %eax
	ret
	.cfi_endproc
.LFE41:
	.size	main, .-main
	.ident	"GCC: (Ubuntu 4.9.2-10ubuntu13) 4.9.2"
	.section	.note.GNU-stack,"",@progbits

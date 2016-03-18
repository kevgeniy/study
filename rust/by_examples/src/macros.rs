fn main() {
	//dry();
}

macro_rules! say_hello {
    () => (
    	println!("Hello!");
    )
}

// variable = $var_name: designator
// designators: block, ident (variable/function name), expr(expression), item, pat(pattern),
// path, stmt(statement), tt(token tree), ty(type)

macro_rules! create_function {
	($func_name: ident) => (
		fn $func_name(expr: &str) {
			println!("{}", expr);
		}
	)
}

macro_rules! print_result {
    ($expression: expr) => (
    	println!("{:?} = {:?}", stringify!($expression), $expression))
}

#[allow(dead_code)]
fn designators() {
	say_hello!();
	create_function!(print_string);
	print_string("Hello, world!");
	print_result!(1_u32 + 4);
	print_result!({
		let a = 4;
		let b = 6;
		if a == b {4} else {5}
	});
}

// можно "перегружать" макросы.
// кроме того  параметры можно разделять как угодно, совсем не обязательно использовать ,
macro_rules! test {
    ($left: expr; and $right: expr) => (
    	println!("{:?} and {:?} is {:?}", stringify!($left), stringify!($right), $left && $right);
    );
    ($left: expr; or $right: expr) => (
    	println!("{:?} or {:?} is {:?}", stringify!($left), stringify!($right), $left || $right);
    )
}

#[allow(dead_code)]
fn overload() {
	test!(1_i32 + 1 == 2_i32; and 2_i32 * 2 == 4_i32);
	test!(true; or false);
}

macro_rules! find_min {
	($x:expr) => ($x);
	($x:expr, $($y:expr),+) => (
		std::cmp::min($x, find_min!($($y),+));
	)
}

#[allow(dead_code)]
fn repeat() {
	println!("{}", find_min!(1u32, 3u32, 4u32));
}

// сложный пример на DRY (don't repeat yourself)

macro_rules! assert_equal_len{
    ($a: ident, $b: ident, $fun: ident, $op: tt) => (
    	assert!($a.len() == $b.len(), 
    			"{:?}: dimension mismatch: {:?} {:?} {:?}", 
    			stringify!($fun), 
    			$a.len(), 
    			stringify!($op), 
    			$b.len());
    ) 
}

#[allow(dead_code)]
fn dry() {
	let v1 = vec![1, 2, 3];
	let v2 = vec![1, 3];
	assert_equal_len!(v1, v2, add_assign, +=);
}
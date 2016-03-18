fn main() {
	array_slice();
	}

// LITERALS

#[allow(dead_code)]
#[allow(unused_variables)]
fn literals() {
	let default_float = 1.0; // f64
	let default_int = 5; // i32

	let prefix_int = 5i64;
	let float: f32 = 1.0;

	// 0x -- hexadecimal, 0o -- octal, 0b -- binary format
	println!("0011 AND 0101 is {:04b} ", 0b0011 & 0b0101);
	println!("0011 OR 0101 is {:04b}", 0b0011 | 0b0101);
	println!("0011 XOR 0101 is {:04b}", 0b0011 ^ 0b0101);
	println!("1 << 5 is {}", 1u32 << 5);
	println!("0x80 >> 2 is {}", 0x80 >> 2);

	// underscore can be used to improve readability
	println!("{}", 1_000_000);
	// и чтобы отметить тип литерала
	let a = 4555_i32;
}

// TUPLES

#[allow(dead_code)]
fn reverse(pair: (i32, bool)) -> (bool, i32) {
	let (int, boolean) = pair;
	(boolean, int)
}

#[allow(dead_code)]
fn tuples() {
	let pair = (1, true);
	println!("{:?}", reverse(pair));

	// one-element Tuples
	println!("{:?}", (5,));

	println!("{}", Matrix(4.0,3.0,2.0,1.0));
	println!("{}", Matrix(4.0, 3.0, 2.0, 1.0).transponse())
}

// TUPLE ACTIVITY

use std::fmt;
#[derive(Debug)]
struct Matrix(f32, f32, f32, f32);

impl fmt::Display for Matrix {
	fn fmt(&self, f: &mut fmt::Formatter) -> fmt::Result {
		try!(writeln!(f, "( {} {} )", self.0, self.1));
		writeln!(f, "( {} {} )", self.2, self.3)
	}
}

impl Matrix {
	fn transponse(self) -> Matrix {
		let Matrix(v1, v2, v3, v4) = self;
		Matrix(v1, v3, v2, v4)	
	}
}

// ARRAYS 

// array - compile-time sized sequence values of one type, [T; N]
// slice - pointer to the data + length, &[T]

fn analyze_slice(slice: &[i32]) {
	println!("slice length {}", slice.len());
	println!("first element of the slice {}", slice[0]);
}

use std::mem;
fn array_slice() {
	let xs: [i32; 10] = [1, 2, 3, 4, 5,6, 7, 8, 9, 10];

	// arrays are stack allocated
	println!("array occupies {} bytes", mem::size_of_val(&xs));
	println!("one array element occupies {} bytes", mem::size_of_val(&xs[0]));
	println!("array consume {} extra bytes", mem::size_of_val(&xs) - mem::size_of_val(&xs[0]) * xs.len());

	// slices creation
	analyze_slice(&xs);
	analyze_slice(&xs[1..4]);

	// runtime boundary check gives an error
	// println!("{}", xs[11]);
}
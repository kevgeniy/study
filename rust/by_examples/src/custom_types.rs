fn main() {
	enums();
}
// STRUCTURES

// unit structure
#[allow(dead_code)]
struct Unit;

// tuple structures, diffrenet from tuples (...) in that it is a NEW type
#[derive(Debug)]
#[allow(dead_code)]
struct Pair(i32, f64);

// c-like structure
#[derive(Debug)]
#[allow(dead_code)]
struct Point {
	x: f64,
	y: f64,
}

// nested structures
#[derive(Debug)]
#[allow(dead_code)]
struct Rectangle {
	p1: Point, 
	p2: Point,
}

#[allow(dead_code)]
#[allow(unused_variables)]
fn structs() {
	let point = Point{ x: 3.0, y: 3.0 };  
	let rtg = Rectangle {
		p1: Point { x: 1.0, y: 1.0 },
		p2: point,
	};

	let pair = Pair(1, 2.0);
	println!("{:?} {:?}", rtg, pair);

}

// ENUMS

#[allow(dead_code)]
enum Person {
	// unit-like variants
	Skinny,
	Fair,
	// tuple-like variants
	Height(i32),
	Weight(i32),
	// c-like-structure variants
	Info {name: String, height: i32}
}

#[allow(dead_code)]
fn inspect(p: Person) {
	// import part on namespace of the enum, мб использовано в любой области видимости
	// use Person::*; for all names
	use Person::{Fair, Weight};
	match p {
		Person::Skinny => println!("Skinny"),
		Fair => println!("Fair"),
		Person::Height(i) => println!("{} cm", i),
		Weight(i) => println!("{} kg", i),
		Person::Info{name, height} => println!("{} name and {} cm", name, height),
	}
 }

// C-LIKE 

#[allow(dead_code)]
enum Number {
	Zero,
	One,
	Two,
}

#[allow(dead_code)]
enum Color {
	Red = 0xff0000,
	Green = 0x00ff00,
	Blue = 0x0000ff,
}

// COMPLEX EXAMPLE

// enum это полноценный тип - сумма типов, так же как struct - тип-произведение типов
// конструкторы его членов можно рассматривать как функции
use List::*;

#[allow(dead_code)]
enum List {
	// element and pointer to the next element
	Cons(u32, Box<List>),
	// empty list
	Nil,
}

impl List{
	fn new() -> List {
		Nil
	}
	fn prepend(self, elem: u32) -> List {
		Cons(elem, Box::new(self))
	}
	#[allow(dead_code)]
	fn len(&self) -> i32{
		match *self{
			Cons(_, ref tail) => 1 + tail.len(),
			Nil => 0,
		}
	}
	fn stringify(&self) -> String {
		// взятие вариантов в скобки подчеркивает, что значение match есть соответствующая строка
		match *self {
			Cons(head, ref tail) => { format!("{} {}", head, tail.stringify()) },
			Nil => { format!("Nil") },
		}
	}
}



#[allow(dead_code)]
fn enums() {
 	let a = Person::Weight(4);
 	// move a
 	inspect(a);

	// C-LIKE
 	println!("zero is {}", Number::Zero as i32);
 	println!("roses are #{:06x}", Color::Red as i32);

 	// COMPLEX EXAMPLE
 	let mut lst = List::new();
 	lst = lst.prepend(3);
 	lst = lst.prepend(4);
 	lst = lst.prepend(5);
 	println!("{}", lst.stringify());
 }

// CONSTANTS

// const: unchangeable value, generic value or constant function, декларация типа обязательна
// именно значения, свегда встраиваемые(inline)  в код, никак не связаны с адресами
#[allow(dead_code)]
const TRESHOLD: i32 = 10;

// возможно изменяемое значение, связано со ссылками. Нужно крайне редко, не считая строковых литералов.
// декларация типа обязательна
#[allow(dead_code)]
static LANGUAGE: &'static str = "Hello"; 
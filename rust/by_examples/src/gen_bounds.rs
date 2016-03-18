use std::fmt::{Debug, Display};

fn main() {
//	empty();
//	compare_print(&"ddd");	
//	associated_type();
//	phantom_data();
	test();
}

// Для удобной работы с обобщениями можно использовать ограничения (bounds)
#[allow(dead_code)]
fn print_debug<T: Debug>(t: T) {
	println!("{:?}", t);
}
#[allow(dead_code)]
fn print<T: Display>(t: T) {
	println!("{}", t);
}

#[derive(Debug)]
#[allow(dead_code)]
struct Rectangle(f64, f64);
#[allow(dead_code)]
struct Triangle(f64, f64, f64);

trait HasArea {
	fn area(&self) -> f64;
}

impl HasArea for Rectangle {
	fn area(&self) -> f64 {
		self.0 * self.1
	}
}

// удивительно, но так тоже можно
#[allow(dead_code)]
fn area<T: HasArea>(t: &T) -> f64 {
	t.area()
}

#[allow(dead_code)]
fn bound() {
	let rectangle = Rectangle(4.0, 3.0);
	let _triangle =  Triangle(4.0, 3.0, 2.0);
	print_debug(&rectangle);
	// используем разные функции area
	// из trait
	rectangle.area();
	// обособленная
	area(&rectangle);
}

// важно что тип реализует именно этот trait, а не определенный набор функций:
trait Red{}

struct Figure;

impl Red for Figure {}

#[allow(dead_code)]
fn red<T: Red>(_: &T) -> &'static str { "red"}

#[allow(dead_code)]
fn empty() {
	let figure = Figure;
	println!("{} figure", red(&figure));

}

// можно указывать сразу несколько ограничений
#[allow(dead_code)]
fn compare_print<T: Debug + Display>(t: &T) {
	println!("debug mode: {:?}", t);
	println!("display mode: {}", t);
}

// where позволяет вынести ограничения отдельно, не все, указанное с помощью 
// where можно сделать без него.
trait PrintInOption {
	fn print_in_option(self);
}

impl<T> PrintInOption for T where Option<T> : Debug {
	fn print_in_option(self) {
		println!("{:?}", Some(self));
	}
} 

// Иногда удобно, чтобы trait формализовывало выходные типы

#[allow(dead_code)]
struct Container(i32, i32);

trait Contains<A, B> {
	fn contains(&self, &A, &B) -> bool;
	fn first(&self) -> i32;
	fn second(&self) -> i32;
}

impl Contains<i32,i32> for Container {
	fn contains(&self, number_1: &i32, number_2: &i32) -> bool {
		&self.0 == number_1 && &self.1 == number_2
	}
	fn first(&self) -> i32 {
		self.0
	}
	fn second(&self) -> i32 {
		self.1
	}
}

// вынуждены писать так
#[allow(dead_code)]
fn difference<A, B, C>(container: &C) -> i32 where C: Contains<A, B> {
	container.second() - container.first()
}

#[allow(dead_code)]
fn associated_type() {
	let number_1 = 3;
	let number_2 = 10;
	let container = Container(3, number_2);
	println!("{:?}", container.contains(&number_1, &number_2))
}

trait Contains2 {
	type A;
	type B;
	fn contains2(&self, &Self::A, &Self::B) -> bool;
	fn first2(&self) -> i32;
	fn second2(&self) -> i32;
}

impl Contains2 for Container {
	type A = i32;
	type B = i32;
	fn contains2(&self, number_1: &Self::A, number_2: &Self::B) -> bool {
		&self.0 == number_1 && &self.1 == number_2	
	}
	fn first2(&self) -> i32 {
		self.0
	}
	fn second2(&self) -> i32 {
		self.1
	}
}

#[allow(dead_code)]
fn difference2<C>(container: &C) -> i32 where C: Contains2 {
	container.second2() - container.first2()
}

// интересные примеры на обобщения
trait Ord {
	type Output;
	fn add(&self, second: &Self) -> Self::Output { 
		self.minus(second)
	}
	fn minus(&self, second: &Self) -> Self::Output;
}

trait AddGeneral<T = Self> {
	type Output;
	fn add(self, t: &T) -> Self::Output	;
	fn minus(self, t: &T) -> Self::Output;
}

#[derive(Debug)]
struct Pair(i32, i32);

impl Ord for Pair {
	type Output = Pair;
	fn add(&self, p: &Pair) -> Pair {
		Pair(self.0 + p.0 + 10, self.1 + p.1)
	}
	fn minus(&self, p: &Pair) -> Pair {
		Pair(self.0 - p.0, self.1 - p.1)
	}
}

impl AddGeneral for Pair {
	type Output = Pair;
	fn add(self, p: &Pair) -> Pair {
		Pair(self.0 + p.0, self.1 + p.1)
	}
	fn minus(self, p: &Pair) -> Pair {
		Pair(self.0 - p.0, self.1 - p.1)
	}
}

fn complex_samples() {
	let a = Pair(4, 4);
	let b = Pair(3, 6);
	println!("{:?}", (&a).add(&b));
}


// Иногда нам удобно статически разделять варианты так, чтобы это не отражалось на рантайме
// Для этого используют фантомные параметры типов

use std::marker::PhantomData;

#[derive(PartialEq)]
struct PhantomTuple<A, B>(A, PhantomData<B>);

#[derive(PartialEq)]
struct PhantomStruct<A, B>{ first: A, phantom: PhantomData<B> }

// Storage doesn't allocated for B. And all parameters with B cannot be used in computations
#[allow(dead_code)]
fn phantom_data() {
	let _a: PhantomTuple<i32, i32> = PhantomTuple(4, PhantomData);
	let _b: PhantomTuple<i32, f64> = PhantomTuple(4, PhantomData);
	// type mismatch
	// println!("{:?}", _a == _b);
}

// Это удобно, например, чтобы иметь тип "Точка", который с одной стороны обладает общими методами
// и свойствами, а сдругой а) свой для каждого фантомного типа б) не допускает странных действий
// вроде сложения длины корабля с длиной дома.
use std::ops::Add;

#[allow(dead_code)]
enum Inch {}
#[allow(dead_code)]
enum Mm {}

struct Length<Unit>(f64, PhantomData<Unit>);
impl<Unit> Add for Length<Unit> {
	type Output = Length<Unit>;

	fn add(self, rhs:Length<Unit>) -> Self::Output {
		Length(self.0 + rhs.0, PhantomData)
	}
}

fn phantom() {
	let one_foot: Length<Inch> = Length(12.0, PhantomData);
	let one_meter: Length<Mm> = Length(4.0, PhantomData);
	let two_feet = one_foot + one_foot;
	// Будет ошибка
	// let feet_meters = one_foot + one_meter;
}
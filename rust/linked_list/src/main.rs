pub mod first;

fn main() {
	let mut lst: List<i32> = List::new();
	lst = lst.add(4);
	lst = lst.add(3);
	lst = lst.add(2);
	lst = lst.add(1);
	
	println!("{}", lst.string());
	lst = lst.reverse();
	println!("{}", lst.string());
	
}

use std::fmt::Display;

enum List<T> where T: Display {
	Cons(Box<List<T>>, T),
	Nil,
}

impl<T> List<T> where T: Display {
	fn new() -> List<T> {
		List::Nil
	}
	fn add(self, elem: T) -> List<T> {
		List::Cons(Box::new(self), elem)
	}
	fn reverse(self) -> List<T> {
		self.reverse_t(List::Nil)
	}
	fn reverse_t(self: List<T>, new: List<T>) -> List<T> {
		match self {
			List::Cons(tail, head) => tail.reverse_t(List::Cons(Box::new(new), head)),
			List::Nil => new
		}
	}
	fn string(&self) -> String {
		match *self {
			List::Cons(ref tail, ref head) => format!("{}, {}", head, tail.string()),
			List::Nil => format!("Nil"),
		}

	}
}
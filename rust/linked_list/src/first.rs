pub enum BadList {
	Empty,
	Elem(i32, Box<BadList>),
}

pub struct List<T> {
	head: Link<T>
}

enum Link<T> {
	Empty,
	More(Box<Node<T>>),
}

struct Node<T> {
	value: T,
	next: Link<T>,
}

use std::mem;
impl<T> List<T> {
	pub fn new() -> Self {
		List { head: Link::Empty }
	}
	pub fn push(&mut self, elem: T) {
		let new_node = Node { value: elem, next: mem::replace(&mut self.head, Link::Empty)};
		self.head = Link::More(Box::new(new_node));
	}
	pub fn pop(&mut self) -> Option<T> {
		match mem::replace(&mut self.head, Link::Empty) {
			Link::Empty => None,
			Link::More(boxed_node) => {
				let node = *boxed_node;
				self.head = node.next;
				Some(node.value)
			}
		}
	}
}

impl<T> Drop for List<T> {
	fn drop(&mut self) {
		let mut link = mem::replace(&mut self.head, Link::Empty);
		while let Link::More(mut boxed_node) = link {
			link = mem::replace(&mut boxed_node.next, Link::Empty)
		}
	}
}

#[cfg(test)]
mod test {
	use super::List;

	#[test]
	fn basics() {
		let mut list = List::new();

		assert_eq!(list.pop(), None);

		list.push(4);
		list.push(3);
		list.push(2);
		list.push(1);

		assert_eq!(list.pop(), Some(1));
		assert_eq!(list.pop(), Some(2));

		list.push(5);
		list.push(6);

		assert_eq!(list.pop(), Some(6));
		assert_eq!(list.pop(), Some(5));

		assert_eq!(list.pop(), Some(3));
		assert_eq!(list.pop(), Some(4));
	}
}
fn main() {
	let p = Point::new(1.0, 1.0);
	println!("{}", p.distance());

	let pair = Pair(Box::new(3), Box::new(4));
	// a.b() ~ b(a) | b(&a) | b(&mut a)
	pair.destroy();
}

struct Point{
	x: f64,
	y: f64
}

impl Point {
	// можно сделать метод origin для получения дефолтной (0.0, 0.0) пары
	// static method, available through type (Pointer)
	fn new(x: f64, y: f64) -> Point {
		Point { x: x, y: y }
	}

	// method, available through type instance (p.distance())
	// &self ~ self: &Self, here Self = Point
	fn distance(&self /*self: &Self*/ ) -> f64 {
		(self.x * self.x + self.y * self.y).sqrt()
	}
}

struct Pair(Box<i32>, Box<i32>);

impl Pair {
	fn destroy(self) {
	}
}


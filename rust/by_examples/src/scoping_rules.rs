fn main() {
	ref_pattern();
}
#[allow(dead_code)]
fn create_box(_: Box<i32>) -> Box<i32> {
	Box::new(3_i32)
	// box1 deallocated
}

#[allow(dead_code)]
#[allow(unused_variables)]
fn raii() {
	let mut box2 = Box::new(5_i32);

	{
		let box3 = Box::new(4_i32);
		// box3 deallocated
	}

	box2 = create_box(box2);
	*box2 = 7;
	create_box(box2);
	// box2 deallocated
}

struct Point {
	x: i32,
	y: i32,
}

#[allow(unused_variables)]
fn ref_pattern() {
	let c = 'E';

	let ref_c1 = &c;
	let ref ref_c2 = c;

	// true
	println!("{}", *ref_c1 == *ref_c2);

	let point = Point{ x: 0, y: 4};

	let copy_of_x = {
		let Point {x: ref ref_to_x, y: _} = point;
		*ref_to_x	
	};

	let mut mut_point = point;

	{
		let Point { x: _, y: ref mut mut_ref_to_y } = mut_point;
		*mut_ref_to_y = 1;	
	}

	println!("{} {}", mut_point.x, mut_point.y);

}
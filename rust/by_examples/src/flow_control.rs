fn main() {
	let_constructions();
}
// условие не нужно брать в скобки, а вот за каждым условием следует блок {...}
// это выражение, поэтому все ветви должны иметь одинаковый тип результата
#[allow(dead_code)]
#[allow(unused_variables)]
fn if_else() {
    let n = 10;
    let big_n = if n < 10 && n > -10 {
        println!("small number");
        n * 10
    } else {
        println!("big number");
        n / 2
    };
}

// loop {...} - infinity loop
// while condition {...} - usual while
// for variables in iterator {...} - for + foreach
#[allow(unreachable_code)]
#[allow(dead_code)]
fn loops() {
    // Для всех циклов допустимы метки
    'outer: loop {
        println!("Entered the outer loop.");
        'inner: loop {
            println!("Entered the inner loop.");
            break 'outer;
        } println!("This point will never be reached.");
    } println!("Exited outer loop.");
}

#[allow(dead_code)]
fn matching() {
    // match - перебор значений
    let number = 13;
    match number {
        1 => println!("One"),
        2 | 3 | 4 => println!("Two..four"),
        13...19 => println!("A teen"),
        _ => println!("Ain't special"),
    }

    // match - destructuring tuples
    // При этом по-прежнему можно использовать значения
    let pair = (1, 0);
    match pair {
        (0, x) => println!("First is 0 and second is {}", x),
        (x, 0) => println!("Second is 0 and first is {}", x),
        _ => println!("It doesn't match any variant"),
    }

    // match - destructuring enums
    // При этом также могут быть использованы константы
    let color = Colors::RGB(1, 1, 1);
    match color {
        Colors::RGB(0, 0, b) => println!("Only blue {}", b),
        Colors::RGB(0, g, b) => println!("No red, only green {} and blue {}", g, b),
        _ => println!("Any other variant"),
    }

    // match - destructuring pointers and references 
    // важно различать разыменовывание (*) и деструктуризацию (&, ref, ref mut)
    // для переменных также можно использовать модификатор mut (см let_constructions -> while let)
    let reference = &4;
    match reference {
        &0 => println!("references 0"),
        &val => println!("references anything but 0: {}", val)
    }

    match *reference {
        0 => println!("dereferences into 0"),
        _ => println!("dereferences into other variant"),
    }

    // ref/ref mut позволяют сразу получить ссылку на значение, являющееся подходящим паттерном:
    // Использовать константы напрямую тут не получится, но можно с помощью guards
    let value = 5;
    match value {
        ref r if *r == 0 => println!("Got a reference to matching zero: {:?}", r),
        ref r if *r == 4 => println!("Got a reference to value four {:?}", r),
        _ => ()
    }

    // изменяемый вариант
    let mut mut_value = 10;
    match mut_value {
        ref mut m => {
            *m += 10;
            println!("We added 10. `mut_value`: {:?}", m)
        },
    }

    // match - destructuring structs

    let rtg = Rectangle { p1: Point{x: 1.0, y: 1.0}, p2: Point{x: 2.0, y: 2.0}};
	//let Rectangle {ref p1, ref p2} = rtg;
	let Rectangle {p1: Point {x, y}, p2: Point{ x: x1, y: y1}} = rtg;
	// let Rectangle { p1: a, p2: Point {x, y}} = rtg;
	// let Rectangle { p1: a, ..} = rtg
	println!("x1: {} y1: {} x2: {} y2: {}", x, y, x1, y1);

	// Как уже гворилось выше на ветки можно накладывать условия (guard) ... if condition => ...
	// Кроме того можно вводить локальные связывания для полоного использования деструктуризации
	match value {
		0 => println!("Zero"),
		n @ 1...3 => println!("n + 2 = {}", n + 2),
		n => println!("n = {}", n),
	}
}

#[allow(dead_code)]
fn let_constructions(){
	// в if условием может быть деструктуризация let a = b (if let), но ее нельзя совмещать через && || и т.п.
	// при этом связывание существует только в этой ветке:
	let optional = Some(2);
	let i = 3;
	if let Some(i) = optional {
		println!("destructured value is {}", i);
	} else if i == 3 {
		println!("value is {}", i);
	}

	// по аналогии условием while тоже мб деструктуризация (while let)
	let mut optional = Some(0);
	while let Some(/* mut */ i) = optional {
		// i += 1; //можно так из-за mut перед i
		if i > 9 {
			println!("Greater than nine");
			optional = None;
		} else{
			println!("`i` is {:?}. Try again", i);
			optional = Some(i + 1);
		}

	}
}

#[allow(dead_code)]
#[derive(Debug)]
enum Colors {
    Red,
    Green,
    Blue,
    RGB(u32, u32, u32),
    HSL(u32, u32, u32),
}

#[derive(Debug)]
#[allow(dead_code)]
struct Point {
	x: f64,
	y: f64,
}

#[derive(Debug)]
#[allow(dead_code)]
struct Rectangle {
	p1: Point, 
	p2: Point,
}


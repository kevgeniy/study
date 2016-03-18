fn main() {
   println!("{subject} {verb} {info}", 
   			subject = "Предмет обсудения",
   			verb = "Глагол для описания",
   			info = "Доп. информация");

   // Смешанное использование нумерованного и именованного стиля
   println!("{0:>width$}", "sdf", width = 5);
   // Можно дополнять нулями, ничем другим нельзя
   println!("{name:>0width$}", name = 1, width = 5);

   // Существует второй отличный от Display режим печати - Debug {:?}
   println!("{:?}", "string debug");

   println!("{:?}", Printable(3));
   println!("{:?}", Deep(Printable(3)));

   println!("{}", Complex{a: 3,b: 4});
   println!("{:?}", Complex{a: 3,b: 4});

   let list = List(vec![1, 2, 3, 4]);
   println!("{}", list);

   let color = Color{red: 3, green: 10, blue: 44};
   println!("{}", color);
}

// DEBUG
// struct UnPrintable(i32);

// fmt::Debug может быть автоматически выведен в отличие от Display, т.к. менее специфичен
// и красив. Можно сказать реализован для обобщенных типов. 
#[derive(Debug)]
struct Printable(i32);

// можно если для вложенных типов определен Debug
#[derive(Debug)]
struct Deep(Printable);

// DISPLAY
use std::fmt;

#[allow(dead_code)]
struct Structure(i32);

impl fmt::Display for Structure {
	fn fmt(&self, f: &mut fmt::Formatter) -> fmt::Result {
		write!(f, "{}", self.0)
	}
}

#[derive(Debug)]
struct Complex {
	a: i32,
	b: i32,
}

impl fmt::Display for Complex {
    fn fmt(&self, f: &mut fmt::Formatter) -> fmt::Result {
    	write!(f, "{} + {}i", self.a, self.b)
    }
}


// complex sample : Display for librusts
struct List(Vec<i32>);

impl fmt::Display for List {
    fn fmt(&self, f:&mut fmt::Formatter) -> fmt::Result {
    	let List(ref vec) = *self;
    	// try!(exp)  -> match exp {Err(err) => return err, Ok(n) => n, }
    	try!(write!(f, "["));
    	
    	for (count, v) in vec.iter().enumerate() {
    		try!(write!(f, "{}{}", if count == 0 { "" } else { ", " }, *v));
    	}	

    	write!(f, "]")
    }
}

// Debub -> {:?} Display -> {} существуют и тругие traits для реализации {:x}, {:o} и т.п.
#[derive(Debug)]
struct Color{
	red: u8,
	green: u8,
	blue: u8,
}

impl fmt::Display for Color {
    fn fmt(&self, f: &mut fmt::Formatter) -> fmt::Result {
    	write!(f, "RGB ({0}, {1}, {2}) {3}", self.red, self.green, self.blue,
    	 format!("{:#04x}{:02x}{:02x}", self.red, self.green, self.blue))
    	// 04 - вывести не менее 4 символов, дополнив 0 если нужно
    }
}
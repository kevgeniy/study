fn main() {
	gen_trait();
}

// STRUCT

// обобщенные параметры типа скрывают предыдущие имена
#[allow(dead_code)]
struct T(i32);

#[allow(dead_code)]
struct GenStr<T>(T);

#[allow(dead_code)]
fn gen_struct() {
	// неявная специализация
	let _implicit = GenStr(4);
	// явная специализация
	let _explicit: GenStr<char> = GenStr('a');
}

// FUNCTION 

// не обобщенная функция
// имя можно заменить на пропуск, если оно не используется
#[allow(dead_code)]
fn die_regular(_t: T) {}


// не обобщенная
#[allow(dead_code)]
fn die_generic_specialized_t(_t: GenStr<T>) {}


#[allow(dead_code)]
fn die_generic_specialized_i32(_t: GenStr<i32>) {}

#[allow(dead_code)]
fn die_generic<T>(_t: GenStr<T>) {}

#[allow(dead_code)]
fn get_fn() {
	die_regular(T(4));
	die_generic_specialized_t(GenStr(T(4)));
	die_generic_specialized_i32(GenStr(4));

	// явная специализация проводится не как обычно
	// die_generic<char>(GenStr('a')); // так нельзя!
	die_generic::<char>(GenStr('a'));
	// неявная специализация обычно работает, но не всегда
	die_generic(GenStr(4));
}

// IMPLEMENTATION

#[allow(dead_code)]
impl GenStr<i32> {
	fn print(&self) {
		println!("specialized i32, {}", self.0);
	}
}

#[allow(dead_code)]
impl GenStr<f64> {
	fn print(&self) {
		println!("specialized f64, {}", self.0);
	}
}

#[allow(dead_code)]
impl<T> GenStr<T> {
	fn print(&self) {
		println!("generic");
	}
}

#[allow(dead_code)]
fn gen_impl() {
	let x: GenStr<i32> = GenStr(4);
	let y = GenStr('a');
	// будет ошибка, т.е. доступны несколько реализаций
	// наверное можно исправит через виртуальные методы
	// x.print();
	y.print();
}

// TRAITS

trait DropOther<T> {
	fn drop_other(&self, _t:T); 
}

impl<T, U> DropOther<T> for U {
    fn drop_other(&self, _t: T) {}
}

#[allow(dead_code)]
fn gen_trait() {
	let x = GenStr(4);
	let y = GenStr(5);
	x.drop_other(y);
	drop(x);
}
fn main() {
    input_param();
    output_param();
    std_samples();
    lazy_iter();
}
// Замыкания(closures/анонимные функции/лямбды) - специальные объекты, состоящие из функции и контекста	
// || вместо ()
// и тип результата и типы аргументов необязательно указывать
// {} нужно только если тело - не единое выражение
// можно захватывать переменные из окружающей среды
// вызов аналогичен вызову функции

#[allow(dead_code)]
fn closures() {
    fn function(i: i32) -> i32 {
        i + 1
    }
    let closure_annotated = |i: i32| -> i32 { i + 1 };
    let closure_inferred = |i| i + 1;

    let i = 1;
    println!("{} {} {}", closure_inferred(i), closure_annotated(i), function(i));

    let professor = "Advard Harrison";
    let print_professor = || println!("Professor {}", professor);
    print_professor();
}

// CAPTURING

#[allow(dead_code)]
fn capturing() {
	// автоматически будет селано все чтобы замыкание работало. В частности переменные будут захвачены:
	// по ссылке: &T
	// по изменяемой ссылке: &mut T
	// по значению: T
	// (по списку сверху вниз по необходимости)

    // &T
	let color = "green";
	let print = || println!("`color`: {}", color);
    print();
    print();

    // &mut T, необходимо mut  перед именем замыкания
    let mut count = 0;
    let mut inc = || {
        count += 1;
        println!("`count`: {}", count);
    };
    inc(); // count = 1
    inc(); // count = 2

    let movable = Box::new(3);
    // drop требует T, замыкание захватывает владение, если объект не копируем
    let consume = || {
        println!("`movable`: {:?}", movable);
        // уничтожить объект, передав владение. Не применимо к заимствованиям.
        // приведет к тому, что consume можно вызвать только раз
        std::mem::drop(movable);
    };
    consume();
}

// INPUT PARAMETERS

// Все бы хорошо, но если функция принимает замыкание, надо указать его полный тип, включая метод захвата
// Fn -- &T
// FnMut -- &mut T, &T
// FnOnce -- T, &mut T, &T

// Альтернативный синтаксис
fn apply<F>(f: F) -> i32 where F: Fn(i32) -> i32 {
    f(1)
}

fn input_param() {
    let k = 2;
    println!("{}", apply(|i| i*k));
}
// Замыкания захватывают переменные, для их хранения компилятор неявно создает анонимную структуру и реализует один
// из traits: Fn, FnMut или FnOnce. До вызова переменная имеет этот (неизвестный тип) и потому нам ужны обобщения. Но
// не просто обобщения, а с ограничениями на один из трех traits
// поскольку функция это замыкание без захвата переменных, то таким образом описанные функции могут принимать в виде
// фактических аргументов и функции.

// Замыкания можно использовать и в качестве возвращаемого значения, но поскольку сейчас Rust не поддерживает 
// возвращение обобщений, нужно использовать Box. Кроме того нужно использовать move, чтобы показать что все значения 
// нужно захватить по значению (иначе как локальные переменные они исчезнут). С FnOnce работа сложнее и требует 
// нестабильного FnBox
#[allow(dead_code)]
fn create_closure() -> Box<Fn(i32) -> i32> {
    let a = 2;
    Box::new(move |i:i32| { 
        let b = a * i;
        println!("{}", b);
        b
    })
}

#[allow(dead_code)]
fn output_param() {
   create_closure()(3); 
}

#[allow(dead_code)]
fn std_samples() {
    let vec1: Vec<i32> = vec![1, 10, 7, 6, 19, 21, 99, 2, 3];
    println!("2 in vec {}", vec1.iter().any(|&i| i == 2));
    let b = if let Some(&a) = vec1.iter().find(|&&i| i > 15) {a} else {1};
    println!("{} > 15 in vec", b);
}

// Засчет того, что интераторы ленивые, можно писать интересные вещи в функциональном стиле
fn lazy_iter() {
    // итератор, а они ленивые
    let sum_of_squares = 
    (0..).map(|n| n * n)
         .filter(|n| *n % 2 == 0);
    let mut k = 10;
    let to_k = sum_of_squares.take_while(|&n| n < k*k)
                              .fold(0, |sum, i| sum + i);
    println!("{}", to_k);
    // не влияет на замыкание!!!
    k = 3;
    println!("{}", to_k);
}
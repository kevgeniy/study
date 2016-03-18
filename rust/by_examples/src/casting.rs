// Нет никакого неявного приведения между примитивными типами. Явное приведение 
// осуществляется с помощью ключевого слова as

// Убирает все предупреждения при переполнении от приведения констант
#![allow(overflowing_literals)]
#[allow(unused_variables)]
fn main() {
    // floating-point -> integer -> char
    let decimal = 64.4321_f32;
    let integer = decimal as u8;
    let character = integer as char;
    println!("Casting: {} -> {} -> {}", decimal, integer, character);

    let a = -9223372036854775809 > 0;
    println!("{:?}", a);
    // signed -> unsigned type T

    // std::T::MAX + 1 is added or subtracted until the value
    // fits into the new type

    println!("{}", 1000 as u16);
    // 1000 - 256 - 256 = 232
    println!("{}", 1000 as u8);

    // uncorrect: println!("{}", (-1) as u8);
    // -1 + 256 = 255
    println!("{}", (-1_i8) as u8);

    // Для положительных чисел это просто взятие по модулю
    println!("1000 mod 256 is : {}", 1000 % 256);

    // Если u_k -> i_l и начальное число помещается в диапазон конечного типа, то оно не меняется, иначе
    // u_k -> u_l а затем берем two's complement 
    println!("{}", 128 as i16);
    println!("{}", 128 as i8);

    // Подробнее:
    println!("{}", 1000 as i8);
    println!("{}", 1000 as u8); // 232
    println!("{}", 232 as i8);

    // ЛИТЕРАЛЫ. Если тип не указан, то смотрит на ограничения контекста, если их нет то по умолчанию
    // i32 и f64.  Пример контекста:
    let a = 10_i64;
    let b = 10; // i64, а не i 32 из-за следующей строчки
    let c = a + b;
    println!("{}", std::mem::size_of_val(&b));

    let elem = 5u8;
    let mut vec= Vec::new(); // не можем вывести тип далее Vec<_>, была бы ошибка, но
    vec.push(elem); // можно вывести тип отсюда

    // type позволяет вводить псевдонимы типа
    // это используется для сокращения ввода, это НЕ новый тип и компилятор в лучшем случае 
    // сгенерирует предупреждение если вы сложите Un8 и u8
    type Un8 = u8;

    // по умолчанию компилятор считает что все типы кроме базовых должны иметь CamelCase имена, 
    // в противном случае он выдаст предупреждение
    #[allow(non_camel_case_types)]
    type un8 = u8;
}


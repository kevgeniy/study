extern crate libsmp;

// mod: подкючить локальный модуль
// extern crate: подкючить внешний crate, который состоит из модуля,
// возможно с несколькими подмодулями
// use: импортируем имена, скоращая их
// причем можно указывать сразу несколько имен в конце:
// use std::io::{BufRead, BufReader, Result, stdin, Write};
use libsmp::it_works;

fn main() {
    println!("Hello, world, {}!", libsmp::mymod::one() + 1);
    println!("I am, {}", it_works());
}

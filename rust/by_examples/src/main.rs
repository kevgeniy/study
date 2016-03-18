extern crate rand;

use std::io;
use rand::Rng;
use std::cmp::Ordering;

fn main() {
    println!("This is a guess game!");

    let answer = rand::thread_rng().gen_range(1, 101);

    loop {
	    println!("Please, enter your guess: ");
	    
	    let mut guess = String::new(); // это важно, вынести за цикл почему-то нельзя
        io::stdin().read_line(&mut guess).expect("You faile to enter line.");

        let guess: u32 = match guess.trim().parse() {
            Err(error) => {
                println!("Uncorrect value, error {}", error);
                continue;
            }
            Ok(num) => num,
        };

        match guess.cmp(&answer) {
            Ordering::Less => println!("Too little!"),
            Ordering::Greater => println!("Too much!"),
            Ordering::Equal => {
                println!("Congratulations, you win!");
                break;
            }
        }
    }
}
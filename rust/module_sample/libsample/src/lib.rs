pub mod mymod;

pub fn it_works() -> String {
    "one".to_string();
    mymod::one().to_string()
}

pub struct Car {
    model: String
}

pub struct Person<'a> {
    car: Option<&'a Car> 
}

impl<'a> Person<'a> {
    fn new() -> Person<'a> {
        Person {
            car: None
        }
    }
    fn buy_car(&mut self, c: &'a Car) {
        self.car = Some(c);
    }
    fn sell_car(&mut self) {
        self.car = None;
    }
    fn change_car(&mut self, person: &mut Person<'a>) {
        let temp = person.car;

        person.car = self.car;
        self.car = temp;
    }
}

fn Person_test() {
    let my_car: Car;
    let other_car: Car;
    let mut me: Person;
    let mut he: Person;

    my_car = Car{model: "Hyonday".to_string()};
    other_car = Car{model: "Toyota".to_string()};
    me = Person::new();
    me.buy_car(&my_car);
    he = Person::new();    
    he.buy_car(&other_car);

    he.change_car(&mut me);
}
        
pub struct Ref<'a> {
    count: i32,
    reference: &'a Vec<i32>
}

impl<'a> Ref<'a> {
    fn new(rf: &'a Vec<i32>) -> Ref<'a> {
        Ref {count: 0, reference: rf}
    }
    fn get_ref(& self) -> &'a Vec<i32> {
        self.reference
    }
}


fn ref_test() {
    let v = vec![1,2,3];
    let cnt_ref = Ref::new(&v);
    let ref1 = cnt_ref.get_ref();
    let ref2 = cnt_ref.get_ref();
}

fn main() {
    let mut v = vec![1,2,3];
    for i in &mut v {
        println!("{}", i);
        *i += 1;
    }
    for i in &v {
        println!("{}", i);
    }
}
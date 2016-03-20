#include <iostream>
#include "vessel.h"

using namespace std;

int main()
{
    Destination *dest = new Destination("yno", 1, 1);
    ShipProperties props;
    string a = "";
    string b = "";
    props.repaired = &a;
    props.damaged = &b;
    CargoVessel vessel(dest, 3, &props, 1, 1, 3, CargoVessel::GOOD, CargoVessel::FOOD, CargoVessel::FOOD);
    cout << vessel << endl;

    return 0;
}


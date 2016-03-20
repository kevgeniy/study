// Vessels v1, Георгий Чернышев, 16.11.2013


// Необходимо реализовать функциональность, где надо исправить ошибки и недописанные куски.
// Интерфейс можно менять в целях исправления, при этом надо быть готовым обосновать.
// Интерфейс можно расширять в целях улучшения, то же необходимо обосновать. Расставить const-ы.
// Типы данных менять нельзя, то есть если храним где-то указатель, нельзя начать хранить по значению.
// Кроме того, в конце будет необходимо модуляризовать код и написать вызовы демонстрирующие работу.

// Обратите внимание, сейчас код компилируется!

#include "vessel.h"
#include <iostream>
#include <cstring>
#include <cstdarg>

int Vessel::max_id(-1);

Vessel::Vessel(Destination* destination, int ETA, ShipProperties* properties)
    : id(++max_id), ETA(ETA) {
    this->dest = new Destination(destination->description, destination->x, destination->y);
    props = new ShipProperties();
    props->damaged = new std::string(*properties->damaged);
    props->repaired = new std::string(*properties->repaired);
}

std::ostream& operator <<(std::ostream& os, const Vessel &vessel) {
    os << "ID: " << vessel.id
              << "\nDestination: " << vessel.dest->description
              << "\nETA: " << vessel.ETA
              << "\nRepaired: "
              << (vessel.props->repaired->empty() ? "-" : *vessel.props->repaired)
              << "\nDamaged: "
              << (vessel.props->damaged->empty() ? "-" : *vessel.props->damaged);
    return os;
}

// CARGO VESSEL

CargoVessel::CargoVessel(Destination *dest, int eta, ShipProperties *props, int maxcargo, int curcargo, int container_num, ...)
    :Vessel(dest, eta, props), maxcargo(maxcargo), curcargo(curcargo), container_num(container_num) {
    va_list params;
    va_start(params, container_num);
    containercontent = new CargoType[container_num]();
    for(int i = 0; i < container_num; ++i)
        containercontent[i] = CargoType(va_arg(params, int));
    va_end(params);
}

CargoVessel::CargoVessel(const CargoVessel &vessel)
    :Vessel(vessel), maxcargo(vessel.maxcargo), curcargo(vessel.curcargo), container_num(vessel.container_num) {
    containercontent = new CargoType[container_num]();
    memcpy(containercontent, vessel.containercontent, container_num);
}

std::ostream& operator <<(std::ostream &os, const CargoVessel &vessel) {
    os << (Vessel)vessel
              << "\nMax cargo: " << vessel.maxcargo
              << "\nCurrent cargo: " << vessel.curcargo
              << "\nNumber of containers: " << vessel.container_num;
    return os;
}

// PASSENGER VESSEL

PassengerVessel::PassengerVessel(Destination *dest, int eta, ShipProperties *props, int maxpax, int curpax, int cabin_num, ...)
    :Vessel(dest, eta, props), maxpax(maxpax), curpax(curpax), cabin_num(cabin_num) {
    va_list params;
    va_start(params, cabin_num);
    cabincontent = new CabinType[cabin_num]();
    for(int i = 0; i < cabin_num; ++i)
        cabincontent[i] = CabinType(va_arg(params, int));
    va_end(params);
}

std::ostream& operator <<(std::ostream &os, const PassengerVessel &vessel) {
    os << (Vessel)vessel
              << "\nMax pax: " << vessel.maxpax
              << "\nCurrent pax: " << vessel.curpax
              << "\nNumber of cabin: " << vessel.cabin_num;
    return os;
}

PassengerVessel::PassengerVessel(const PassengerVessel &vessel)
    :Vessel(vessel), maxpax(vessel.maxpax), curpax(vessel.curpax), cabin_num(vessel.cabin_num) {
    cabincontent = new CabinType[cabin_num]();
    memcpy(cabincontent, vessel.cabincontent, cabin_num);
}

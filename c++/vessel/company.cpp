#include"company.h"
#include <iostream>
#include <string>
#include <algorithm>
#include <functional>

void NavalFreightCompany::markDamaged(int id, std::string *date_damaged) {
    Vessel *result = find_by_id(id);
    if(result == NULL)
        throw -1;
    result->props->damaged->append(date_damaged->c_str());
}

void NavalFreightCompany::markRepaired(int id, std::string *date_repaired) {
    Vessel *result = find_by_id(id);
    if(result == NULL)
        throw -1;
    result->props->repaired->append(date_repaired->c_str());
}

void NavalFreightCompany::changeDestination(int id, Destination *dest) {
    Vessel *result = find_by_id(id);
    if(result == NULL)
        throw -1;
    result->dest = dest;
}

void NavalFreightCompany::printHeadingTo(const std::string &str_dest) const {
    int result = count_where2([str_dest](const Vessel *ship)
    { return (ship->dest->description == str_dest); });
    std::cout << result;
}

void NavalFreightCompany::printStats() const {
    std::cout << "Cargo vessels: ";
    std::cout << count_where2(CargoVessel::IsCargo);
    std::cout << "\nEver damaged: ";
    std::cout << count_where2([](const Vessel *ship) { return (CargoVessel::IsCargo(ship) &&
                                                               Vessel::IsDamaged(ship)); });
    std::cout << "\nPassenger vessels: ";
    std::cout << count_where2(PassengerVessel::IsPassenger);
    std::cout << "\nEver damaged: ";
    std::cout << count_where2([](const Vessel *ship) { return (PassengerVessel::IsPassenger(ship) &&
                                                               Vessel::IsDamaged(ship)); });
    std::cout << "\nOther vessels: ";
    std::cout << count_where2([](const Vessel *ship) { return (!PassengerVessel::IsPassenger(ship) &&
                                                               !CargoVessel::IsCargo(ship)); });
    std::cout << "\nEver damaged: ";
    std::cout << count_where2([](const Vessel *ship) { return (!PassengerVessel::IsPassenger(ship) &&
                                                               !CargoVessel::IsCargo(ship) &&
                                                               Vessel::IsDamaged(ship)); });
}


//template<class Type>
//int NavalFreightCompany::count_where(bool pred(Vessel * ship, Type param), Type param) const {
//    std::list<Vessel*>::const_iterator begin = ships.cbegin(), end = ships.cend();
//    int result = 0;
//    while(begin != end) {
//        if(pred(*begin, param))
//            ++result;
//        ++begin;
//    }
//    return result;
//}

int NavalFreightCompany::count_where2(std::function<bool(const Vessel*)> pred) const {
    std::list<Vessel*>::const_iterator begin = ships.cbegin(), end = ships.cend();
    int result = 0;
    while(begin != end) {
        if(pred(*begin))
            ++result;
        ++begin;
    }
    return result;
}


std::ostream &operator<<(std::ostream &os, const NavalFreightCompany &company) {
    os << "Location: " << company.location->description;
    company.printStats();
    return os;
}

Vessel *NavalFreightCompany::find_by_id(int id) {
    Vessel *result = NULL;
    std::list<Vessel *>::const_iterator begin = ships.cbegin(), end = ships.cend();
    while(begin != end) {
        if((*begin)->id == id) {
            result = *begin;
            break;
        }
        else {
            begin++;
        }
    }
    return result;
}

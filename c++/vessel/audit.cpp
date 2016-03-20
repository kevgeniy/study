#include "company.h"
#include <typeinfo>

bool Audit::isPassengerVesselsInPort(const NavalFreightCompany &company) const{
    for(auto ship : company.ships) {
        if(typeid(ship) == typeid(PassengerVessel *))
            return true;
    }
    return false;
}

int Audit::compare1(const NavalFreightCompany &first, const NavalFreightCompany &second) const{
    int size1 = first.ships.size(), size2 = second.ships.size();
    return size1 < size2 ? -1 : size1 == size2 ? 0 : 1;
}

int Audit::compare2(const NavalFreightCompany &first, const NavalFreightCompany &second) const{
    int first_mark = first.count_where2(Vessel::IsRepaired);
    int second_mark = second.count_where2(Vessel::IsRepaired);
    return first_mark < second_mark ? -1 : first_mark == second_mark ? 0 : 1;
}

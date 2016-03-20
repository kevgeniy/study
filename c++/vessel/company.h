#ifndef COMPANY_H
#define COMPANY_H
#include "vessel.h"
#include <string>
#include <list>
#include <functional>

class NavalFreightCompany {
    public:
        // имеющиеся корабли
        std::list<Vessel*> ships;
        // добавление корабля
        void add(Vessel *vessel, Destination *dest);
        void add(Vessel *vessel);
        // удаление корабля
        void remove(const Vessel &vessel);
        void remove(int id);
        // списать весь хлам - когда-либо ремонтировавшиеся суда
        void draft();
        // напечатать статистику: сколько пассажирских, сколько грузовых, из них когда-либо ломавшихся
        void printStats() const;
        // внести запись о ремонте корабля по id
        void markRepaired(int id, std::string *date_repaired);
        // внести запись о повреждении корабля по id
        void markDamaged(int id, std::string *date_damaged);
        // место где порт расположен
        Destination *location;
        // поменять пункт назначения корабля
        void changeDestination(int id, Destination *dest);
        // напечатать сколько кораблей идут в пункт назначения X
        void printHeadingTo(const std::string &str_dest) const;
        int count_where2(std::function<bool(const Vessel *ship)>) const;
    private:
        // Печать всех данных о компании
        friend std::ostream& operator<<(std::ostream& os, const NavalFreightCompany &company);
        Vessel *find_by_id(int id);
};

inline void NavalFreightCompany::add(Vessel *vessel, Destination *dest) {
    vessel->dest = dest;
    add(vessel);
}
inline void NavalFreightCompany::add(Vessel *vessel) {
    ships.push_back(vessel);
}

inline void NavalFreightCompany::remove(const Vessel &vessel) {
    remove(vessel.id);
}

inline void NavalFreightCompany::remove(int id) {
    ships.remove_if([id](Vessel *ship) { return ship->id == id; });
}

inline void NavalFreightCompany::draft() {
    ships.remove_if([](Vessel *ship) { return !ship->props->repaired->empty(); });
}



// класс аудитор компании, делает различные проверки
class Audit {
    public:
        // Узнать есть ли в порту данной компании дежурящие пассажирские суда (у которых destination совпадает с location)
        bool isPassengerVesselsInPort(const NavalFreightCompany &company) const;

        // Сравнить две компании, вывести более крупную (считается суммарно), сигнатуру придумать самостоятельно
        int compare1(const NavalFreightCompany &first, const NavalFreightCompany &second) const;

        // Сравнить две компании, вывести более надежную (меньше когда-либо ремонтировавшихся судов), сигнатуру придумать самостоятельно
        int compare2(const NavalFreightCompany &first, const NavalFreightCompany &second) const;

        // проинспектировать пассажирский корабль на предмет наличия большего количества людей чем положено по нормативу
        // return true if all right, false otherwise
        bool inspectPassengerVessel(const PassengerVessel &vessel) const;

        // проинспектировать пассажирский корабль на предмет наличия большего количества контейнеров чем положено по нормативу
        // return true if all right, false otherwise
        bool inspectCargoVessel(const CargoVessel &vessel) const;
};

inline bool Audit::inspectPassengerVessel(const PassengerVessel &vessel) const{
    return vessel.curpax <= vessel.maxpax;
}

inline bool Audit::inspectCargoVessel(const CargoVessel &vessel) const{
    return vessel.curcargo <= vessel.maxcargo;
}
#endif // COMPANY_H

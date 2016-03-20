#ifndef VESSEL_H
#define VESSEL_H
#include <string>
#include <typeinfo>

// Свойства корабля: был ли корабль поврежден и отремонтирован; дата соответствующих событий.
// Если не был поврежден, то пустая строка.
struct ShipProperties {
    // дата повреждения
    std::string* damaged;
    // дата ремонта
    std::string* repaired;
};

// Место назначения
class Destination {
    public:
        // название порта назначения
        std::string description;
        // координаты места назначения
        int x,y;
        Destination (std::string description, int x, int y) {
            this->description = description;
            this->x = x;
            this->y = y;
        }
};

class Vessel {
    public:
        // уникальный идентификатор
        const int id;
        // пункт назначения
        Destination* dest;
        // сколько дней осталось время до прибытия
        int ETA;
        // свойства корабля
        ShipProperties* props;
        // constructors
        Vessel(Destination* dest, int ETA, ShipProperties* props);
        Vessel();
        // destructor
        ~Vessel();
        // операция клонирования, должна присутствовать во всех наследуемых классах
        Vessel(const Vessel &vessel);
        static bool IsDamaged(const Vessel *ship);
        static bool IsRepaired(const Vessel *ship);
    private:
        static int max_id;
    protected:
        // Печать судна
        friend std::ostream& operator<<(std::ostream& os, const Vessel& vessel);
};


inline Vessel::Vessel()
    :id(max_id++), dest(NULL), ETA(0), props(NULL) {}


inline Vessel::Vessel(const Vessel &vessel)
    :Vessel(vessel.dest, vessel.ETA, vessel.props) {}

inline Vessel::~Vessel() {
    delete(dest);
    delete(props);
}

inline bool Vessel::IsDamaged(const Vessel *ship) {
    return !ship->props->damaged->empty();
}

inline bool Vessel::IsRepaired(const Vessel *ship) {
    return !ship->props->repaired->empty();
}


class CargoVessel : public Vessel {
    public:
        // Типы  груза
        enum CargoType {FOOD, WOOD, GOOD};
        // максимальная грузоподъемность
        int maxcargo;
        // сколько сейчас загружено
        int curcargo;
        // массив контейнеров,
        int container_num;
        CargoType* containercontent;
        // Конструктор
        CargoVessel(Destination* dest, int eta, ShipProperties* props, int maxcargo, int curcargo, int container_num, ...);
        CargoVessel(const CargoVessel &vessel);
        ~CargoVessel();
        static bool IsCargo(const Vessel *ship);
    protected:
        // Печать грузового судна
        friend std::ostream& operator<<(std::ostream& os, const CargoVessel& v);
};

inline CargoVessel::~CargoVessel() {
    delete(containercontent);
}
inline bool CargoVessel::IsCargo(const Vessel *ship) {
    return (typeid(ship) == typeid(CargoVessel *));
}



class PassengerVessel : public Vessel {
    public:
        // максимальная вместимость пассажиров
        int maxpax;
        // сколько сейчас пассажиров
        int curpax;
        // типы кают
        enum CabinType {LUX, FIRSTCLASS, SECONDCLASS};
        // сколько кают на данном корабле
        int cabin_num;
        // список кают с типом
        CabinType* cabincontent;
        // конструктор
        PassengerVessel(Destination* dest, int ETA, ShipProperties* props, int maxpax, int curpax, int cabin_num, ...);
        PassengerVessel(const PassengerVessel &vessel);
        ~PassengerVessel();
        static bool IsPassenger(const Vessel *ship);
    protected:
        // Печать пассажирского судна
        friend std::ostream& operator<<(std::ostream& os, const CargoVessel& v);
};

inline PassengerVessel::~PassengerVessel() {
    delete(cabincontent);
}
inline bool PassengerVessel::IsPassenger(const Vessel *ship) {
    return (typeid(ship) == typeid(PassengerVessel *));
}

#endif // VESSEL_H

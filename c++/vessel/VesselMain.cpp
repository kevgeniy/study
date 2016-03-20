// Vessels v1, ������� ��������, 16.11.2013

#include <stdio.h>
#include <list>
#include <string>

// ���������� ����������� ����������������, ��� ���� ��������� ������ � ������������ �����.
// ��������� ����� ������ � ����� �����������, ��� ���� ���� ���� ������� ����������.
// ��������� ����� ��������� � ����� ���������, �� �� ���������� ����������. ���������� const-�.
// ���� ������ ������ ������, �� ���� ���� ������ ���-�� ���������, ������ ������ ������� �� ��������.
// ����� ����, � ����� ����� ���������� �������������� ��� � �������� ������ ��������������� ������.

// �������� ��������, ������ ��� �������������!

// �������� �������: ��� �� ������� ��������� � ��������������; ���� ��������������� �������.
// ���� �� ��� ���������, �� ������ ������.
struct ShipProperties {
	// ���� �����������
	std::string* damaged;
	// ���� �������
	std::string* repaired;
};

// ����� ����������
class Destination {
	public:
		// �������� ����� ����������
		std::string description;
		// ���������� ����� ����������
		int x,y;
		Destination (std::string description, int x, int y) {
			this->description = description;
			this->x = x;
			this->y = y;
		}
};

class Vessel {
	public:
		// ���������� �������������
		int id;
		// ����� ����������
		Destination* destination;
		// ������� ���� �������� ����� �� ��������
		int ETA;
		// �������� �������
		ShipProperties* props;
		Vessel(int id, Destination* dest, int ETA, ShipProperties* props) { 
			this->id = id;
			this->destination = dest;
			this->ETA = ETA;
			this->props = props;
		}
		Vessel(){}
		~Vessel(){}
		// �������� ������������, ������ �������������� �� ���� ����������� �������
		Vessel* clone();

		// ������ �����
		friend std::ostream& operator<<(std::ostream& os, const Vessel& v);
};

class CargoVessel : public Vessel {
	public:
		// ����  �����
		enum CargoType {FOOD, WOOD, GOOD};
		// ������������ ����������������
		int maxcargo;
		// ������� ������ ���������
		int curcargo;
		// ������ �����������, 
		int container_num;
		CargoType* containercontent;
		// �����������
		CargoVessel(int id, Destination* dest, int eta, ShipProperties* props, int maxcargo, int curcargo, int container_num, CargoType ...){}
	private:
		// ������ ��������� �����
		friend std::ostream& operator<<(std::ostream& os, const CargoVessel& v);

};

class PassengerVessel : public Vessel {
	public:
		// ������������ ����������� ����������
		int maxpax;
		// ������� ������ ����������
		int curpax;
		// ���� ����
		enum CabinType {LUX, FIRSTCLASS, SECONDCLASS};
		// ������� ���� �� ������ �������
		int cabin_num;
		// ������ ���� � �����
		CabinType* cabincontent;
		// �����������
		PassengerVessel(int id, Destination* dest, int ETA, ShipProperties* props, int maxpax, int curpax, int cabin_num, CabinType ...){};

	private:
		// ������ ������������� �����
		friend std::ostream& operator<<(std::ostream& os, const CargoVessel& v);
};

class NavalFreightCompany {
	private:
		// ��������� �������
		std::list<Vessel*> ships;
		// ���������� �������
		void add(Vessel* v, Destination* d);
		void add(Vessel* v);
		// �������� �������
		int remove(Vessel* v);
		int remove(int id);
		// ����� ��� ���� ����������
		Destination* location;
		// ������� ���� ���� - �����-���� ����������������� ����
		void draft();
		// ���������� ����������: ������� ������������, ������� ��������, �� ��� �����-���� ����������
		void printStats();
		// ������ ������ � ������� ������� �� id
		void markRepaired(int id, std::string* date_repaired);
		// ������ ������ � ����������� ������� �� id
		void markDamaged(int id, std::string* date_damaged);
		// �������� ����� ���������� �������
		int changeDestination(int id, Destination* dest);
		// ���������� ������� �������� ���� � ����� ���������� X
		void printHeadingTo(std::string s);
	public:
		// ������ ���� ������ � ��������
		friend std::ostream& operator<<(std::ostream& os, const CargoVessel& v);


};

// ����� ������� ��������, ������ ��������� ��������
class Audit {
	public:
		// ������ ���� �� � ����� ������ �������� ��������� ������������ ���� (� ������� destination ��������� � location)
		bool isPassengerVesselsInPort(NavalFreightCompany& nfc);
		// �������� ��� ��������, ������� ����� ������� (��������� ��������), ��������� ��������� ��������������
		void compare1();
		// �������� ��� ��������, ������� ����� �������� (������ �����-���� ����������������� �����), ��������� ��������� ��������������
		void compare2();
		// ����������������� ������������ ������� �� ������� ������� �������� ���������� ����� ��� �������� �� ���������
		bool inspectPassengerVessel(Vessel v);
		// ����������������� ������������ ������� �� ������� ������� �������� ���������� ����������� ��� �������� �� ���������
		bool inspectCargoVessel(CargoVessel v);
};


void main(){
	printf("test");
}

#include <iostream>
//#include <algorithm>
#include "vector.h"
#include "multiset.h"
using namespace std;

void string_test();
void int_test();
int main()
{

    string_test();
    cout << "Hello World!" << endl;
    return 0;
}

void int_test() {
    vector<int> vc1(4);
    vc1.push_back(3);
    vc1.push_back(4);
    vc1.push_back(5);
    vector<int>::iterator it = vc1.begin();
    while(it != vc1.end()) {
        cout << *it << endl;
        ++it;
    }
    vector<int>::iterator it2 = vc1.iterator_where(4);
    vc1.erase(it2);
    cout << vc1;

    multiset<int> mlt(4);
    mlt.add(4);
    mlt.add(5);
    mlt.add(6);
    mlt.add(5, 2);
    multiset<int>::iterator it3 = mlt.begin();
    while(it3 != mlt.end()) {
        cout << *it3 << endl;
        ++it3;
    }
    cout << mlt;
}

void string_test() {
    vector<string> vc1(4);
    vc1.push_back(string("first"));
    vc1.push_back(string("second"));
    vc1.push_back(string("third"));

    vector<string>::iterator it = vc1.begin();
    while(it != vc1.end()) {
        cout << *it << endl;
        ++it;
    }


    vector<string>::iterator it2 = vc1.iterator_where(string("second"));
    vc1.erase(it2);
    cout << vc1;


    multiset<string> mlt(4);
    mlt.add(string("first"));
    mlt.add(string("second"));
    mlt.add(string("third"));
    mlt.add(string("second"), 2);

    multiset<string>::iterator it3 = mlt.begin();
    while(it3 != mlt.end()) {
        cout << *it3 << endl;
        ++it3;
    }
    mlt.remove(string("second"), 1);
    cout << mlt;
}


#ifndef MULTISET_H
#define MULTISET_H
#include "vector.h"
#include <functional>

template <typename Type>
class multiset {
public:
    typedef typename vector<pair<Type, int> >::iterator iterator;
    explicit multiset(int count);

    multiset(const multiset<Type> &other);
    multiset<Type> &operator=(const multiset<Type> &other);
    ~multiset();

    void add(const Type &element);
    void add(const Type &element, int count);
    void remove(const Type &element);
    void remove(const Type &element, int count);

    multiset<Type> cross(const multiset<Type>& first, const multiset<Type>& second) const;
    multiset<Type> combine(const multiset<Type>& first, const multiset<Type>& second) const;
    multiset<Type> sub(const multiset<Type>& first, const multiset<Type>& second) const;


    typename vector<pair<Type, int> >::iterator find_iterator(const Type& element);
    typename vector<pair<Type, int> >::iterator begin();
    typename vector<pair<Type, int> >::iterator end();
    int compare(const Type& first, const Type& second) const;
    void sort(typename vector<pair<Type, int> >::iterator begin, typename vector<pair<Type, int> >::iterator end, std::function<int(pair<Type, int>, pair<Type, int>)> cmp);

    friend std::ostream &operator<<(std::ostream &os, const multiset<int> &mltset);
    friend std::ostream &operator<<(std::ostream &os, const multiset<std::string> &mltset);
    friend std::ostream &operator<<(std::ostream &os, const multiset<char *> &mltset);
private:
    vector<pair<Type, int> > elements;
};

template <typename Type>
multiset<Type>::multiset(int count)
    : elements(count){}

template <typename Type>
multiset<Type>::multiset(const multiset<Type> &other)
    :elements(other.elements) {}

template <typename Type>
multiset<Type> &multiset<Type>::operator =(const multiset<Type> &other) {
    elements = other.elements;
}

template <typename Type>
multiset<Type>::~multiset() {
   // elements.~vector();
}

template <typename Type>
void multiset<Type>::add(const Type &element) {
    add(element, 1);
}

template <typename Type>
void multiset<Type>::add(const Type &element, int count) {
    typename vector<pair<Type, int>>::iterator it = find_iterator(element);
    if (it == end()) {
//        pair<Type, int> pr(element, count);
        elements.push_back(pair<Type, int>(element, count));
    }
    else
        (*it).second += count;
}

template <typename Type>
void multiset<Type>::remove(const Type& element) {
    remove(element, 1);
}

template <typename Type>
void multiset<Type>::remove(const Type& element, int count) {
    typename vector<pair<Type, int> >::iterator it = find_iterator(element);
    if(it == end())
        return;
    if((*it).second <= count)
        elements.erase(it);
    else
        (*it).second -= count;
    return;
}

template <typename Type>
multiset<Type> multiset<Type>::cross(const multiset<Type>& first, const multiset<Type>& second) const {
    multiset<Type> result();
    for(typename vector<Type>::iterator first_it = first.begin(); first_it != first.end(); ++first_it) {
        typename vector<Type>::iterator second_it = second.find_iterator(*first_it.first);
        if(second_it != second.end())
            result->add(*first_it.first, min(*first_it.second, *second_it.second));
    }
    return result;
}

template <typename Type>
multiset<Type> multiset<Type>::combine(const multiset<Type>& first, const multiset<Type>& second) const {
    multiset<Type> result();
    for(typename vector<Type>::iterator first_it = first.begin(); first_it != first.end(); ++first_it) {
        typename vector<Type>::iterator second_it = second.find_iterator(*first_it.first);
        if(second_it != second.end())
            result->add(*first_it.first, max(*first_it.second, *second_it.second));
        else
            result.add(*first_it);
    }
    for(typename vector<Type>::iterator second_it = second.begin(); second_it != second.end(); ++second_it) {
        if(first.find_iterator(*second_it) < first.begin())
            result.add(*second_it);
    }
    return result;
}

template <typename Type>
multiset<Type> multiset<Type>::sub(const multiset<Type>& first, const multiset<Type>& second) const {
    multiset<Type> result();
    for(typename vector<Type>::iterator first_it = first.begin(); first_it != first.end(); ++first_it) {
        typename vector<Type>::iterator second_it = second.find_iterator(*first_it.first);
        if(second_it == second.end())
            result->Add(*first_it);
        else if(*second_it.second < *first_it.second)
            result->Add(*first_it.first, *first_it.second - *second_it.second);
    }
    return result;
}

template <typename Type>
typename vector<pair<Type, int> >::iterator multiset<Type>::find_iterator(const Type& element) {
           return elements.iterator_where([element](const pair<Type, int> &pr) {return pr.first == element; });
}

template <typename Type>
typename vector<pair<Type, int> >::iterator multiset<Type>::begin() {
    return elements.begin();
}

template <typename Type>
typename vector<pair<Type, int> >::iterator multiset<Type>::end() {
    return elements.end();
}

template <typename Type>
void multiset<Type>::sort(typename vector<pair<Type, int> >::iterator begin,
                          typename vector<pair<Type, int> >::iterator end,
                          std::function<int(pair<Type, int>, pair<Type, int>)> cmp) {
    elements.Sort(begin, end, cmp);
}

std::ostream &operator<<(std::ostream &os, const multiset<int> &mltset) {
    os << mltset.elements;
    return os;
}

std::ostream &operator<<(std::ostream &os, const multiset<std::string> &mltset) {
    os << mltset.elements;
    return os;
}

std::ostream &operator<<(std::ostream &os, const multiset<char *> &mltset) {
    os << mltset.elements;
    return os;
}

#endif // MULTISET_H

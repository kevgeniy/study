#ifndef VECTOR_H
#define VECTOR_H
#include <functional>
#include <string.h>
#include <iostream>
#include <string>

template <typename TKey, typename TValue>
class pair {
public:
    TKey first;
    TValue second;
    pair() {}
    explicit pair(TKey key, TValue value)
        : first(key), second(value) {}
    pair(const pair<TKey, TValue> &other)
        : pair<TKey, TValue>(other.first, other.second) { }

    bool operator ==(const pair<TKey, TValue> &other) {
        return (other.first == first && other.second == second);
    }
    bool operator <(const pair<TKey, TValue> &other) {
        return first < other.first || first == other.first && second < other.second;
    }
    bool operator >(const pair<TKey, TValue> &other) {
        return other < this;
    }
    bool operator >=(const pair<TKey, TValue> &other) {
        return !(this < other);
    }
    bool operator  <=(const pair<TKey, TValue> &other) {
        return !(this > other);
    }

    bool operator !=(const pair<TKey, TValue> &other) {
        return !(this == other);
    }
    template <typename StrKey, typename StrValue>
    friend std::ostream &operator <<(std::ostream &os, const pair<StrKey, StrValue> &element);
};

template <typename StrKey, typename StrValue>
std::ostream &operator <<(std::ostream &os, const pair<StrKey, StrValue> &element) {
    os << "1: " << element.first << std::endl << "2: " << element.second << std::endl;
    return os;
}

template <typename Type>
class vector {
public:
    explicit vector(int count);
    vector()
        :vector(4){}

    vector(const vector<Type> &other);
    vector<Type> &operator=(const vector<Type> &other);
    ~vector();

    const Type& operator [](int position) const;
    Type& operator [](int position);

    void push_back(const Type &element);

    class iterator{
    public:
        explicit iterator(int position, vector<Type> &object);

        iterator(const iterator &other);
        iterator &operator =(const iterator &other) const;
        ~iterator();

        iterator operator +(int count) const;
        iterator operator -(int count) const;
        int operator -(const iterator &other) const;

        iterator &operator ++();
        iterator operator ++(int);
        iterator &operator --();
        iterator operator --(int);

        bool operator ==(const iterator &other) const;
        bool operator !=(const iterator &other) const;
        bool operator <(const iterator &other) const;
        bool operator >(const iterator &other) const;
        bool operator <=(const iterator &other) const;
        bool operator >=(const iterator &other) const;

        iterator &operator +=(int count);
        iterator &operator -=(int count);

        Type &operator *();
        Type *operator ->();

        const Type& operator [](int position) const;
        Type& operator [](int position);
        friend class vector<Type>;
    private:
        int position;
        vector<Type> &collection;     // const??
    };

    void erase(iterator position);
    iterator begin();
    iterator end();

    void Sort(iterator begin, iterator end, std::function<int(Type, Type)> cmp);
    iterator iterator_where(std::function<bool(const Type&)> pred);
    iterator iterator_where(const Type &element);

    friend std::ostream &operator <<(std::ostream &os, const vector<int> &vtr);
    friend std::ostream &operator <<(std::ostream &os, const vector<std::string> &vtr);
    friend std::ostream &operator <<(std::ostream &os, const vector<pair<std::string, int> > &vtr);
    friend std::ostream &operator <<(std::ostream &os, const vector<pair<int, int> > &vtr);
    friend std::ostream &operator <<(std::ostream &os, const vector<pair<char *, int> > &vtr);

private:
    int size;
    int max_size;
    Type *elements;
};

// ITERATORS
template <typename Type>
vector<Type>::iterator::iterator(int pos, vector<Type> &object)
    :position(pos), collection(object) {}

template <typename Type>
vector<Type>::iterator::iterator(const iterator &other)
    :position(other.position), collection(other.collection) {}

template <typename Type>
vector<Type>::iterator::~iterator() { }


template <typename Type>
typename vector<Type>::iterator vector<Type>::iterator::operator +(int count) const {
    return iterator(position + count <= collection.size ? position + count : collection.size, collection);
}

template <typename Type>
typename vector<Type>::iterator vector<Type>::iterator::operator -(int count) const {
    return iterator(position - count >= 0 ? position - count : 0, collection);
}

template <typename Type>
int vector<Type>::iterator::operator -(const iterator &other) const {
    return (this->position - other.position);
}


template <typename Type>
typename vector<Type>::iterator &vector<Type>::iterator::operator ++(){
    if(position < collection.size)
        ++position;
    return *this;
}

template <typename Type>
typename vector<Type>::iterator vector<Type>::iterator::operator ++(int){
    iterator result(this);
    ++(*this);
    return result;
}

template <typename Type>
typename vector<Type>::iterator &vector<Type>::iterator::operator --(){
    if(position > 0)
        --position;
    return *this;
}

template <typename Type>
typename vector<Type>::iterator vector<Type>::iterator::operator --(int){
    iterator result(this);
    --(*this);
    return result;
}

template <typename Type>
bool vector<Type>::iterator::operator ==(const vector<Type>::iterator &other) const{
    return (position == other.position); //&& collection == other.collection);
}

template <typename Type>
bool vector<Type>::iterator::operator !=(const vector<Type>::iterator &other) const{
    return !(*this == other);
}

template <typename Type>
bool vector<Type>::iterator::operator <(const vector<Type>::iterator &other) const {
    return (collection == other.collection && position < other.position);
}

template <typename Type>
bool vector<Type>::iterator::operator >(const vector<Type>::iterator &other) const {
    return !(*this < other);
}

template <typename Type>
bool vector<Type>::iterator::operator <=(const vector<Type>::iterator &other) const {
    return !(*this > other);
}

template <typename Type>
bool vector<Type>::iterator::operator >=(const vector<Type>::iterator &other) const {
    return !(*this < other);
}

template <typename Type>
typename vector<Type>::iterator &vector<Type>::iterator::operator +=(int count) {
    position = position + count > elements.size ? elements.size : position + count;
    return *this;
}

template <typename Type>
typename vector<Type>::iterator &vector<Type>::iterator::operator -=(int count) {
    position = position - count > 0 ? position - count : 0;
    return *this;
}

template <typename Type>
Type &vector<Type>::iterator::operator *(){
    return collection[position];
}

template <typename Type>
Type *vector<Type>::iterator::operator ->() {
    return elements.elements + position;
}

template<typename Type>
Type &vector<Type>::iterator::operator [](int position) {
    return elements[position];
}


template<typename Type>
const Type &vector<Type>::iterator::operator [](int position) const {
    return elements[position];
}


// VECTOR

template <typename Type>
vector<Type>::vector(int count)
    :size(0), max_size(count), elements(new Type[count]){ }

template <typename Type>
vector<Type>::vector(const vector<Type> &other)
    :size(other.size), max_size(other.max_size) {
    elements = new Type[max_size]();
    for(int i = 0; i < size; ++i)
        elements[i](other.elements[i]);
}

template <typename Type>
vector<Type> &vector<Type>::operator =(const vector<Type> &other){
    if(this != other) {
        size = other.size;
        max_size = other.max_size;
        delete [](elements);
        elements = new Type[max_size]();
        for(int i = 0; i < size; ++i)
            elements[i] = other.elements[i];
    }
    return *this;
}

//template<>
//vector<std::string>::~vector() {
//    for(int i = 0; i < max_size; i++) {
//        elements[i].clear();
//    }
//    delete[](elements);           //?????
//}

//template<>
//vector<pair<std::string, int> >::~vector() {
//    for(int i = 0; i < max_size; i++)
//        elements[i].first.clear();
//    delete[](elements);
//}

//template<>
//vector<char *>::~vector() {
//    delete[](elements);
//}

template <typename Type>
vector<Type>::~vector() {
    delete[](elements);
}



template  <typename Type>
const Type &vector<Type>::operator [](int position) const {
    if(position < 0 || position >= size) {
        throw "Index is out of range";
    }
    return elements[position];
}

template <typename Type>
Type& vector<Type>::operator [](int position) {
    if(position < 0 || position >= size) {
        throw "Index is out of range";
    }
    return elements[position];
}


template <typename Type>
void vector<Type>::push_back(const Type &element) {
    if(size >= max_size) {
        Type *new_elements = new Type[max_size * 2]();
        memcpy(new_elements, elements, size * sizeof(Type));
        delete[](elements);
        elements = new_elements;
        max_size *= 2;
    }
    elements[size] = element;
    ++size;
}

template <typename Type>
void vector<Type>::erase(iterator iter) {
    if(iter.position < 0 || iter.position >= size) {
        throw "Index is out of range";
    }
    for(int i = iter.position; i < size - 1; i++)
        elements[i] = elements[i + 1];
//    if(iter.position != size - 1)
//        memmove(elements + iter.position, elements + iter.position + 1, (size - iter.position - 1) * sizeof(Type));
    --size;
}


template <typename Type>
typename vector<Type>::iterator vector<Type>::begin() {
    return iterator(0, *this);
}

template <typename Type>
typename vector<Type>::iterator vector<Type>::end() {
    return iterator(size, *this);
}

template <typename Type>
typename vector<Type>::iterator vector<Type>::iterator_where(std::function<bool (const Type &)> pred) {
    for(iterator it = begin(); it != end(); ++it)
        if(pred(*it))
            return it;
    return end();
}

template <typename Type>
typename vector<Type>::iterator vector<Type>::iterator_where(const Type &element) {
    for(iterator it = begin(); it != end(); ++it)
        if(*it == element)
            return it;
    return end();
}

template <typename Type>
void vector<Type>::Sort(vector<Type>::iterator begin, vector<Type>::iterator end, std::function<int(Type, Type)> cmp) {
    int begin_index = -1, end_index = -1;
    for(int i = 0; i < size; ++i)
        if(elements[i] == *begin) {
            begin_index = i;
            break;
        }
    for(int i = 0; i <size; ++i)
        if(elements[i] == *end) {
            end_index = i;
            break;
        }

    for(int i = begin_index; i < end_index; i++)
        for(int j = begin_index; j < end_index + begin_index - i; j++)
            if(cmp(elements[j], elements[j + 1]) > 0) {
                Type temp = elements[j + 1];
                elements[j + 1] = elements[j];
                elements[j] = temp;
            }
}

std::ostream &operator<<(std::ostream &os, const vector<int> &vtr) {
    for(int i = 0; i < vtr.size; i++)
        os << vtr.elements[i] << std::endl;
        return os;
}

std::ostream &operator <<(std::ostream &os, const vector<std::string> &vtr) {
    for(int i = 0; i < vtr.size; i++)
        os << vtr.elements[i] << std::endl;
        return os;
}

std::ostream &operator <<(std::ostream &os, const vector<pair<int, int> > &vtr) {
    for(int i = 0; i < vtr.size; i++)
        os << vtr.elements[i] << std::endl;
        return os;
}

std::ostream &operator <<(std::ostream &os, const vector<pair<std::string, int> > &vtr) {
    for(int i = 0; i < vtr.size; i++)
        os << vtr.elements[i] << std::endl;
        return os;
}

std::ostream &operator <<(std::ostream &os, const vector<pair<char *, int> > &vtr) {
    for(int i = 0; i < vtr.size; i++)
        os << vtr.elements[i] << std::endl;
        return os;
}

#endif // VECTOR_H

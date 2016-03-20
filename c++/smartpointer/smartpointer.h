#ifndef SMARTPOINTER_H
#define SMARTPOINTER_H
#include <cstdlib>
template <class Type>
class smartpointer {
public:
    smartpointer()
        :ptr(NULL), count(new int(0)){ }
    explicit smartpointer(Type *pointer);
    smartpointer(const smartpointer<Type> &sm_pointer);
    smartpointer<Type> &operator=(const smartpointer<Type> &sm_pointer);
    ~smartpointer();

    operator Type*();
    Type *operator->();

    smartpointer<Type> operator +(int shift);
    smartpointer<Type> operator -(int shift);
    std::ptrdiff_t operator -(const smartpointer<Type> &other);
private:
Type* ptr;
int *count;
int *der_count;
int *mem_count;
};

template<typename Type>
smartpointer<Type>::smartpointer(Type *pointer)
    :ptr(pointer), count(new int(1)), der_count(new int(0)), mem_count(new int(0)) {
    if(pointer == NULL)
        *count = 0;
}

template<typename Type>
smartpointer<Type>::smartpointer(const smartpointer<Type> &sm_pointer)
    :ptr(sm_pointer.ptr), count(sm_pointer.count), der_count(sm_pointer.der_count), mem_count(sm_pointer.mem_count) {
    *count += 1;
}

template<typename Type>
smartpointer<Type> &smartpointer<Type>::operator =(const smartpointer<Type> &sm_pointer) {
    if(this != sm_pointer) {
        *count -= 1;
        if(*count == 0) {
            delete (ptr);
            delete (count);
            delete (mem_count);
            delete (der_count);
        }
        ptr = sm_pointer.ptr;
        count = sm_pointer.count;
        mem_count = sm_pointer.mem_count;
        der_count = sm_pointer.der_count;
        ++*count;
    }
    return this;
}

template<typename Type>
smartpointer<Type>::~smartpointer() {
    *count -= 1;
    if(*count == 0) {
        delete (ptr);
        delete (count);
        delete (mem_count);
        delete (der_count);
    }
}

template<typename Type>
smartpointer<Type>::operator Type*() {
    if(ptr == NULL)
        throw "NULL POINTER DEREFERENCING!";
    *der_count += 1;
    return *ptr;
}

template<typename Type>
Type *smartpointer<Type>::operator ->() {
    if(ptr == NULL)
        throw "NULL POINTER DEREFERENCING!";
    *mem_count += 1;
    return ptr;
}

template<typename Type>
smartpointer<Type> smartpointer<Type>::operator +(int count) {
    return smartpointer(ptr + count);
}

template<typename Type>
smartpointer<Type> smartpointer<Type>::operator -(int count) {
    return smartpointer(ptr - count);
}

template<typename Type>
std::ptrdiff_t smartpointer<Type>::operator -(const smartpointer<Type> &other) {
    return ptr - other.ptr;
}

#endif // SMARTPOINTER_H

TEMPLATE = app
CONFIG += console
CONFIG -= app_bundle
CONFIG -= qt

SOURCES += main.cpp

HEADERS += \
    vector.h \
    multiset.h

QMAKE_CXXFLAGS += -std=c++11

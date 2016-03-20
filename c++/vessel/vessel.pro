TEMPLATE = app
CONFIG += console
CONFIG -= app_bundle
CONFIG -= qt

SOURCES += main.cpp \
    company.cpp \
    audit.cpp \
    vessel.cpp

HEADERS += \
    company.h \
    vessel.h

QMAKE_CXXFLAGS += -std=c++11

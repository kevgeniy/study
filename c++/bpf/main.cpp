#include <iostream>
#include <vector>
#include <complex>
#include <cmath>
#include <cstdio>
#include <sys/time.h>
#include <cstdlib>
#include <ctime>
#include <unistd.h>
#include <fcntl.h>

#define SECOND  1

using namespace std;

typedef complex<double> base;

const char *input_file = "./2.txt";
const char *output_file = "./1.txt";

void read_stream(vector<double> &);
void print_vector(vector<double> &);
void print_vector_complex(vector<complex<double> > &);
void multiply_polynoms_fast(const vector<double> &v1, const vector<double> &v2, vector<double> &result);
void multiply_polynoms_simple(const vector<double> &v1, const vector<double> &v2, vector<double> &result);

long long mtime() {
  struct timeval t;

  gettimeofday(&t, NULL);
  long long mt = (long long)t.tv_sec * 1000 + t.tv_usec / 1000;
  return mt;
}

int main()
{
    if(input_file != 0)
        freopen(input_file, "r", stdin);

    vector<double> input, output, result_fast, result_simple;

#ifndef THIRD
    cout << "First polynom:" << endl;
    read_stream(input);

    cout << "Second polynom:" << endl;
    read_stream(output);

    multiply_polynoms_fast(input, output, result_fast);
    multiply_polynoms_simple(input, output, result_simple);

    print_vector(result_simple);
    print_vector(result_fast);
#else
    int max_degree, current_degree;

    cout << "Please, enter max polynom degree:" << endl;
    cin >> max_degree;
    freopen(output_file, "w", stdout);

    cout << "degree\t" << "fast\t" << "simple\n";

    current_degree = 1;
    while(current_degree <= max_degree) {
        input.resize(current_degree);
        input.clear();
        for(int i = 0; i < current_degree; ++i)
            input.push_back(rand());

        output.resize(current_degree);
        output.clear();
        for(int i = 0; i < current_degree; ++i)
            output.push_back(rand());

        long fast_time, simple_time;
        fast_time = clock();
        multiply_polynoms_fast(input, output, result_fast);
        fast_time = clock() - fast_time;

        simple_time = clock();
        multiply_polynoms_simple(input, output, result_simple);
        simple_time = clock() - simple_time;

        cout << current_degree << "\t" << fast_time << "\t" << simple_time << endl;
        ++current_degree;
    }
#endif
    return 0;
}

void print_vector(vector<double> &v) {
    cout << endl;
    double eps = 0.01;
    size_t real_size;
    for(real_size = v.size(); real_size > 0; --real_size)
        if(abs(v[real_size - 1]) > eps)
            break;
    for(size_t i = 0; i < real_size; ++i)
        printf("%.2f ", v[i]);
    cout << endl;
}


void print_complex_vector(vector<complex<double> > &v) {
    cout << endl;
    for(size_t i = 0; i < v.size(); ++i)
        cout << v[i].real() << " + i*" << v[i].imag() << endl;
    cout << endl;
}

void read_stream(vector<double> &v) {
    cout << "Enter number of coefficients:" << endl;
    int count;
    cin >> count;

    double coef;
    cout << "Please, enter a0 -- a" << count << " coefficients:" << endl;

    while(count != 0) {
        cin >> coef;
        v.push_back(coef);
        --count;
    }
    cout << endl;
}

void dpf(vector<complex<double> > &v, bool inverse) {
    int count = v.size();
    if(count == 1)
        return;

    vector<complex<double> > v0(count/2), v1(count/2);
    for(int i = 0; i < count; i += 2) {
        v0[i/2] = v[i];
        v1[i/2] = v[i + 1];
    }

    dpf(v0, inverse);
    dpf(v1, inverse);

    double angle = 2*M_PI /count * (inverse ? 1 : -1);
    complex<double> w(1), wn(cos(angle), sin(angle));
    for(int i = 0; i < count/2; ++i) {
        v[i] = v0[i] + w*v1[i];
        v[i + count/2] = v0[i] - w*v1[i];
        if(inverse) {
            v[i] /= 2;
            v[i + count/2] /= 2;
        }
        w *= wn;
    }
}

void multiply_polynoms_fast(const vector<double> &v1, const vector<double> &v2, vector<double> &result) {
    size_t new_length = 1;
    while(new_length < max (v1.size(), v2.size())) new_length <<= 1;
    new_length <<= 1;

    vector<complex<double> > v_nor1(v1.begin(), v1.end()), v_nor2(v2.begin(), v2.end());
    v_nor1.resize(new_length);
    v_nor2.resize(new_length);

    dpf(v_nor1, false);
    dpf(v_nor2, false);

#ifdef SECOND
    cout << "First dpf vector:";
    print_complex_vector(v_nor1);
    cout << "Second dpf vector:";
    print_complex_vector(v_nor2);
#endif

    for(size_t i = 0; i < new_length; ++i)
        v_nor1[i] *= v_nor2[i];
    dpf(v_nor1, true);

#ifdef SECOND
    cout << "result dpf vector:";
    print_complex_vector(v_nor1);
#endif

    result.resize(new_length);
    for(size_t i = 0; i < new_length; ++i)
        result[i] = v_nor1[i].real();
}

void multiply_polynoms_simple(const vector<double> &v1, const vector<double> &v2, vector<double> &result) {
    size_t new_length = v1.size() + v2.size() - 1;
    result.resize(new_length);

    for(size_t i = 0; i < v1.size(); ++i)
        for(size_t j = 0; j < v2.size(); ++j)
            result[i + j] += v1[i] * v2[j];
}

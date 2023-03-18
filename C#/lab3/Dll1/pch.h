#pragma once
#include "mkl.h"

extern "C" _declspec(dllexport)
double func(int n, const double a[], double y[], int mode);

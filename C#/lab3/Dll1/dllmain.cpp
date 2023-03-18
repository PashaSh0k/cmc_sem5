// dllmain.cpp : Определяет точку входа для приложения DLL.
#include "pch.h"
#include "mkl.h"
#include <time.h>

extern "C" _declspec(dllexport)
double func(int n, const double a[], double y[], int mode) {
	double time = 0;
	try {
		if (mode) {
			clock_t start = clock();
			vmdTan(n, a, y, VML_HA);
			clock_t end = clock();
			time = (double)(end - start) / CLOCKS_PER_SEC;
		}
		else {
			clock_t start = clock();
			vmdTan(n, a, y, VML_EP);
			clock_t end = clock();
			time = (double)(end - start) / CLOCKS_PER_SEC;
		}
	}
	catch (...) {
		return -1;
	}
	return time;
}


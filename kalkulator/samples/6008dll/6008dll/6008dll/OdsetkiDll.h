// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the MY6008DLL_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// MY6008DLL_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
//#ifdef MY6008DLL_EXPORTS
#define ODSETKI_API __declspec(dllexport)
//#else
//#define MY6008DLL_API __declspec(dllimport)
//#endif


//extern ODSETKI_API int nMy6008dll;

ODSETKI_API int fnMy6008dll(int a, int b);

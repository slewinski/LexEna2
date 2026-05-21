// 6008dll.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "OdsetkiDll.h"
#include "string.h"


// This is an example of an exported variable
//MY6008DLL_API int nMy6008dll=0;

// This is an example of an exported function.
ODSETKI_API int fnMy6008dll(int a, int b)
{
	return a+b;
}


ODSETKI_API  char* HelloWorld(char* name)
{
	return strcat(name, "Hello World ");
}

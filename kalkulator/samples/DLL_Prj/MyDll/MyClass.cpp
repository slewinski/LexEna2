// MyClass.cpp: implementation of the CMyClass class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "MyDll.h"
#include "MyClass.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CMyClass::CMyClass()
{

}

CMyClass::~CMyClass()
{

}


CString CMyClass::SayHello(CString strName)
{
	//Return the input string with a Hello prefixed 
	return "Hello " + strName;
}

CString CMyClass::BullShit(CString strName)
{
	return "Bull Shit";
}

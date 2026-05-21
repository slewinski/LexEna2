// MyClass.h: interface for the CMyClass class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_MYCLASS_H__1FAFEE60_88D6_11D6_89D7_00010302158B__INCLUDED_)
#define AFX_MYCLASS_H__1FAFEE60_88D6_11D6_89D7_00010302158B__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


__declspec(dllexport)   CString  SayHello1 (CString strName)
{
	
return "Dupa" + strName;

}


class CMyClass  
{
public:
	__declspec(dllexport)   CString SayHello (CString strName);
	 CString BullShit (CString strName);
	__declspec(dllexport) CMyClass();
	__declspec(dllexport) virtual ~CMyClass();

};

#endif // !defined(AFX_MYCLASS_H__1FAFEE60_88D6_11D6_89D7_00010302158B__INCLUDED_)

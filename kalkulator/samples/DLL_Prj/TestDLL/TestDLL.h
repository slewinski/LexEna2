// TestDLL.h : main header file for the TESTDLL application
//

#if !defined(AFX_TESTDLL_H__1FAFEE65_88D6_11D6_89D7_00010302158B__INCLUDED_)
#define AFX_TESTDLL_H__1FAFEE65_88D6_11D6_89D7_00010302158B__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols

/////////////////////////////////////////////////////////////////////////////
// CTestDLLApp:
// See TestDLL.cpp for the implementation of this class
//

class CTestDLLApp : public CWinApp
{
public:
	CTestDLLApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTestDLLApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation

	//{{AFX_MSG(CTestDLLApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TESTDLL_H__1FAFEE65_88D6_11D6_89D7_00010302158B__INCLUDED_)

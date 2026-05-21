// TestDLLDlg.h : header file
//

#if !defined(AFX_TESTDLLDLG_H__1FAFEE67_88D6_11D6_89D7_00010302158B__INCLUDED_)
#define AFX_TESTDLLDLG_H__1FAFEE67_88D6_11D6_89D7_00010302158B__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

/////////////////////////////////////////////////////////////////////////////
// CTestDLLDlg dialog


#include "..\MyDll\MyClass.h"

class CTestDLLDlg : public CDialog
{
// Construction
public:
	CTestDLLDlg(CWnd* pParent = NULL);	// standard constructor
	CMyClass objMyClass;

// Dialog Data
	//{{AFX_DATA(CTestDLLDlg)
	enum { IDD = IDD_TESTDLL_DIALOG };
	CString	m_edit;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTestDLLDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	//{{AFX_MSG(CTestDLLDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	virtual void OnOK();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TESTDLLDLG_H__1FAFEE67_88D6_11D6_89D7_00010302158B__INCLUDED_)

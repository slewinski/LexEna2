#if !defined(AFX_DATEEDIT_H__95D76601_5F21_11D7_BFE0_00E04C393105__INCLUDED_)
#define AFX_DATEEDIT_H__95D76601_5F21_11D7_BFE0_00E04C393105__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// DateEdit.h : header file
//
#include <afxdisp.h>
/////////////////////////////////////////////////////////////////////////////
// CDateEdit window

class CDateEdit : public CEdit
{
// Construction
public:
	CDateEdit();

// Attributes
public:

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDateEdit)
	//}}AFX_VIRTUAL

protected:
	CString m_editText;
	CString passstr;

// Implementation
public:
	CString GetCurrentDateStr();
	void SetDate(CString curstr);
	CString GetText();
	BOOL IsValidDate();
	
	virtual ~CDateEdit();

	// Generated message map functions
protected:
	//{{AFX_MSG(CDateEdit)
	afx_msg void OnChange();
	afx_msg void OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags);
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_DATEEDIT_H__95D76601_5F21_11D7_BFE0_00E04C393105__INCLUDED_)

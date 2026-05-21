#if !defined(AFX_NUMERICEDIT_H__95D76601_5F21_11D7_BFE0_00E04C393105__INCLUDED_)
#define AFX_NUMERICEDIT_H__95D76601_5F21_11D7_BFE0_00E04C393105__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// NumericEdit.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CNumericEdit window

class CNumericEdit : public CEdit
{
// Construction
public:
	CNumericEdit(BYTE nFract = 3);

// Attributes
public:

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CNumericEdit)
	//}}AFX_VIRTUAL

protected:
	CString m_editText;
	BYTE m_nFractionalDigits;
// Implementation
public:
	void SetText(CString curstr);
	DWORD GetLongValue();
	double GetDoubleValue();
	BYTE GetFractDigits();
	void SetFractDigits(BYTE nFract);
	BOOL Create(const RECT &rect,CWnd *pParentWnd,UINT nID);
	virtual ~CNumericEdit();

	// Generated message map functions
protected:
	void SetPoints(CString &curText,int &iStartSel);
	//{{AFX_MSG(CNumericEdit)
	afx_msg void OnChange();
	afx_msg void OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags);
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_NUMERICEDIT_H__95D76601_5F21_11D7_BFE0_00E04C393105__INCLUDED_)

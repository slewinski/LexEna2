// newokr.h : header file
//
#include "DateEdit.h"
#include "numericEdit.h"
/////////////////////////////////////////////////////////////////////////////
// CNewOkr dialog

class CNewOkr : public CDialog
{
// Construction
public:
    CNewOkr(CWnd* pParent = NULL);  // standard constructor

// Dialog Data
    //{{AFX_DATA(CNewOkr)
	enum { IDD = IDD_NEW_OKR };
    //CString m_oDateOd;
    CNumericEdit m_Stopa, m_Factor;
    CDateEdit m_sDate;
    //DWORD   m_oStopa;
	//}}AFX_DATA
    DWORD m_dwStopa, m_dwFactor;
    CString m_strDate;

// Implementation
protected:
    virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

    // Generated message map functions
    //{{AFX_MSG(CNewOkr)
	virtual void OnOK();
	afx_msg void OnChanges();
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
    DECLARE_MESSAGE_MAP()
};

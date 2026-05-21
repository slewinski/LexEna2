// odsust.h : header file
//
#include "newokr.h"
/////////////////////////////////////////////////////////////////////////////
// COdsUst dialog

class COdsUst : public CDialog
{
// Construction
public:
    COdsUst(CWnd* pParent = NULL);  // standard constructor

// Dialog Data
    //{{AFX_DATA(COdsUst)
    enum { IDD = IDD_ODS_UST };
    CListBox   m_OdsUst;
    //}}AFX_DATA

// Implementation
protected:
    virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
    void PutList();
    virtual BOOL OnInitDialog();
    // Generated message map functions
    //{{AFX_MSG(COdsUst)
    afx_msg void OnNewElem();
		virtual void OnOK();
		afx_msg void OnDblclkOds();
	//}}AFX_MSG
    DECLARE_MESSAGE_MAP()
};



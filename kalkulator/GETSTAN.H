// getstan.h : header file
// 96.06.10 08:42

#include "DateEdit.h"

/////////////////////////////////////////////////////////////////////////////
// CGetStan dialog

class CGetStan : public CDialog
{
// Construction
public:
    CGetStan(CWnd* pParent = NULL); // standard constructor
// Dialog Data
    //{{AFX_DATA(CGetStan)
	enum { IDD = IDD_GET_STAN };
    int m_Kal_Bank;
    CListBox    m_listStan;
  	CString	m_Sprawa;
	//}}AFX_DATA
    CKmDoc* m_pDoc;
// Implementation
protected:
    virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
    virtual BOOL OnInitDialog();
    void PutList();
	void SortStan();
    // Generated message map functions
    //{{AFX_MSG(CGetStan)
    afx_msg void OnNewElem();
    afx_msg void OnDelElem();
    virtual void OnOK();
    afx_msg void OnEditElem();
    //}}AFX_MSG
    DECLARE_MESSAGE_MAP()
};
/////////////////////////////////////////////////////////////////////////////
// CGetDate dialog

class CGetDate : public CDialog
{
// Construction
public:
    CGetDate(CWnd* pParent = NULL); // standard constructor

// Dialog Data
    //{{AFX_DATA(CGetDate)
    enum { IDD = IDD_DATE_STAN };
    CDateEdit m_sDate;
    //}}AFX_DATA
    CString m_strDate;


// Implementation
protected:
    virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

    // Generated message map functions
    //{{AFX_MSG(CGetDate)
    afx_msg void OnChangeDate();
	  virtual void OnOK();
	  virtual BOOL OnInitDialog();
	
	//}}AFX_MSG
    DECLARE_MESSAGE_MAP()
};


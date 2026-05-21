// dlg.h : header file
//

#include "DateEdit.h"
#include "numericEdit.h"
/////////////////////////////////////////////////////////// CDlgNal dialog
class CDlgNal : public CDialog
{
public:
    CDlgNal(CWnd* pParent = NULL);  // standard constructor
    CKmDoc* m_pDoc;
    
// Dialog Data
    //{{AFX_DATA(CDlgNal)
	  enum { IDD = IDD_DNAL };
    CComboBox   m_nListTyp;
    int m_nTyp;  
    CNumericEdit m_nGroszy;
    UINT    m_nNo;
	  BOOL	m_skrOds;
    CDateEdit m_sDate;
    //}}AFX_DATA
    CString m_strDate;
    DWORD m_dwGroszy;

// Implementation
protected:
    virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

    // Generated message map functions
    //{{AFX_MSG(CDlgNal)
    virtual void OnOK();
    virtual BOOL OnInitDialog();    
    afx_msg void OnChange();
	  
	//}}AFX_MSG
    DECLARE_MESSAGE_MAP()
};

////////////////////////////////////////////////////////// CDlgWpl dialog
class CDlgWpl : public CDialog
{
public:
    CDlgWpl(CWnd* pParent = NULL);  // standard constructor
    CKmDoc* m_pDoc;
    
// Dialog Data
    //{{AFX_DATA(CDlgWpl)
    enum { IDD = IDD_DWPL };
    int m_wTyp;
    CDateEdit m_sDate;
    CNumericEdit m_nGroszy;
    UINT    m_wNo;
	int     m_wSQLTyp;
    //}}AFX_DATA
    CString m_strDate;
    DWORD   m_dwGroszy;

// Implementation
protected:
    virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

    // Generated message map functions
    //{{AFX_MSG(CDlgWpl)
    virtual void OnOK();
    virtual BOOL OnInitDialog();
    afx_msg void OnChange();
	  
	//}}AFX_MSG
    DECLARE_MESSAGE_MAP()
};

//////////////////////////////////////////////////////// CDlgOkr dialog
class CDlgOkr : public CDialog
{
public:
    CDlgOkr(CWnd* pParent = NULL);  // standard constructor
    CKmDoc* m_pDoc;
    
// Dialog Data
    //{{AFX_DATA(CDlgOkr)
    enum { IDD = IDD_DOKR };
        // NOTE: the ClassWizard will add data members here 
   UINT      m_oNum;       // Numer Okresu
   UINT      m_oParent;    // Numer Naleznosci
   int       m_TypO;       // Typ Odsetok ('UST','UMN','SKR')
   int       m_TypW;       // Typ Wplaty Stopy ( 'M', 'R', 'D' )
   CDateEdit m_DateP; 
   CDateEdit m_DateK;
   CNumericEdit m_Stopa;
   CNumericEdit m_Factor;   
   //}}AFX_DATA
   CString   m_strDateP;
   CString   m_strDateK;
   DWORD     m_dwStopa, m_dwFactor;
 
// Implementation
protected:
    virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

    // Generated message map functions
    //{{AFX_MSG(CDlgOkr)
    virtual BOOL OnInitDialog();
    afx_msg void OnChange();
    virtual void OnOK();
	  
	//}}AFX_MSG
    DECLARE_MESSAGE_MAP()
};

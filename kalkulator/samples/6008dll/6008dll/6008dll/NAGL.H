// nagl.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CNagl dialog

class CNagl : public CDialog
{
// Construction
public:
	CNagl(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	//{{AFX_DATA(CNagl)
	enum { IDD = IDD_NAG };
	CString	m_sPlik;
	//}}AFX_DATA

// Implementation
protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support

	// Generated message map functions
	//{{AFX_MSG(CNagl)
	virtual BOOL OnInitDialog();
	virtual void OnOK();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};
/////////////////////////////////////////////////////////////////////////////
// CStop dialog

class CStop : public CDialog
{
// Construction
public:
	CStop(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	//{{AFX_DATA(CStop)
	enum { IDD = IDD_STOP };
	CString	m_sPlik;
	//}}AFX_DATA

// Implementation
protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support

	// Generated message map functions
	//{{AFX_MSG(CStop)
	virtual BOOL OnInitDialog();
	virtual void OnOK();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

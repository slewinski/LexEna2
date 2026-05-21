// nagl.cpp : implementation file
//

#include "stdafx.h"
#include "kkm.h"
#include "nagl.h"

#ifdef _DEBUG
#undef THIS_FILE
static char BASED_CODE THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CNagl dialog


CNagl::CNagl(CWnd* pParent /*=NULL*/)
	: CDialog(CNagl::IDD, pParent)
{
	//{{AFX_DATA_INIT(CNagl)
	m_sPlik = "";
	//}}AFX_DATA_INIT
}

void CNagl::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CNagl)
	DDX_Text(pDX, IDC_EDIT1, m_sPlik);
	DDV_MaxChars(pDX, m_sPlik, 12);
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CNagl, CDialog)
	//{{AFX_MSG_MAP(CNagl)
	ON_BN_CLICKED(IDOK, OnOK)
	ON_BN_DOUBLECLICKED(IDOK, OnOK)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()


/////////////////////////////////////////////////////////////////////////////
// CNagl message handlers

BOOL CNagl::OnInitDialog()
{ 
  CString Row(theApp.GetProfileString("Config", "Nag", NULL));
  m_sPlik = Row;
  CDialog::OnInitDialog();
  return TRUE;  // return TRUE  unless you set the focus to a control
}

void CNagl::OnOK()
{  CDialog::OnOK();
 if (m_sPlik.GetLength() == 0)  
    theApp.WriteProfileString("Config", "Nag", NULL);
 else  
    theApp.WriteProfileString("Config", "Nag", (const char*)m_sPlik);
}
/////////////////////////////////////////////////////////////////////////////
// CStop dialog


CStop::CStop(CWnd* pParent /*=NULL*/)
	: CDialog(CStop::IDD, pParent)
{
	//{{AFX_DATA_INIT(CStop)
	m_sPlik = "";
	//}}AFX_DATA_INIT
}

void CStop::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CStop)
	DDX_Text(pDX, IDC_EDIT1, m_sPlik);
	DDV_MaxChars(pDX, m_sPlik, 12);
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CStop, CDialog)
	//{{AFX_MSG_MAP(CStop)         
	ON_BN_CLICKED(IDOK, OnOK)
	ON_BN_DOUBLECLICKED(IDOK, OnOK)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()


/////////////////////////////////////////////////////////////////////////////
// CStop message handlers

BOOL CStop::OnInitDialog()
{  CString Row(theApp.GetProfileString("Config", "Stop", NULL));
   m_sPlik = Row;
   CDialog::OnInitDialog();
   return TRUE;  // return TRUE  unless you set the focus to a control
}

void CStop::OnOK()
{  CDialog::OnOK();
 if (m_sPlik.GetLength() == 0)  
  theApp.WriteProfileString("Config", "Stop", NULL);
 else  
  theApp.WriteProfileString("Config", "Stop", (const char*)m_sPlik);
}

// newokr.cpp : implementation file
// 96.06.10 08:28

#include "stdafx.h"
#include "kkm.h"
#include "newokr.h"

#ifdef _DEBUG
#undef THIS_FILE
static char BASED_CODE THIS_FILE[] = __FILE__;
#endif

#define FRACT_DIGITS 2
extern int Changes;
/////////////////////////////////////////////////////////////////////////////
// CNewOkr dialog


CNewOkr::CNewOkr(CWnd* pParent /*=NULL*/)
    : CDialog(CNewOkr::IDD, pParent)
{
    //{{AFX_DATA_INIT(CNewOkr)
    //m_oDateOd = "";
    //m_oStopa = 0;
	//}}AFX_DATA_INIT
}

void CNewOkr::DoDataExchange(CDataExchange* pDX)
{
    CDialog::DoDataExchange(pDX);
    //{{AFX_DATA_MAP(CNewOkr)
    DDX_Control(pDX, IDC_DATEP, m_sDate);
    DDX_Control(pDX, IDC_STOPA, m_Stopa);
 //   DDX_Control(pDX, IDC_FACTOR, m_Factor); Hejmus 15.08.09
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CNewOkr, CDialog)
    //{{AFX_MSG_MAP(CNewOkr)
	ON_EN_CHANGE(IDC_DATEP, OnChanges)
	ON_EN_CHANGE(IDC_STOPA, OnChanges)
	//ON_EN_CHANGE(IDC_FACTOR, OnChanges) // Hejmus 15.08.09
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()


/////////////////////////////////////////////////////////////////////////////
// CNewOkr message handlers
//--------------------------------------------------------
inline void CNewOkr::OnChanges() { Changes = TRUE; }
//--------------------------------------------------------

void CNewOkr::OnOK() 
{
	UpdateData();
  if (!m_sDate.IsValidDate() || m_sDate.GetText() == "----------") 
  {
    m_sDate.SetFocus();
	  AfxMessageBox(IDS_INVALID_DATE);
    return;
  }	
  m_strDate = m_sDate.GetText();
	m_strDate = m_strDate.Left(6)+m_strDate.Right(2);
	
  m_dwStopa = m_Stopa.GetLongValue();
  m_dwStopa *= 1000;
  m_dwFactor = m_Stopa.GetLongValue();
  m_dwFactor *= 1000;  
  CDialog::OnOK();
}


BOOL CNewOkr::OnInitDialog() 
{
	char buf[100];
  CString strTyp;
  //COleDateTime DateMin( 1951, 1, 1, 0, 0, 0 );
  //COleDateTime DateMax( 2049, 12, 31, 0, 0, 0 );

  CDialog::OnInitDialog();
  //m_sDate.SetSeparator('-');
  //m_sDate.SetRange(DateMin,DateMax);
  //m_sDate.SetDateToToday();
  m_sDate.SetDate(m_sDate.GetCurrentDateStr());

  m_Stopa.SetFractDigits(FRACT_DIGITS);
  ltoa(m_dwStopa,buf,10);
  strTyp = buf;
  while (strTyp.GetLength() < 3) strTyp = "0" + strTyp;
  strTyp = strTyp.Left(strTyp.GetLength()-FRACT_DIGITS)+ ',' + strTyp.Right(FRACT_DIGITS);
 
  /* hejmus 15.08.09
  m_Factor.SetText(strTyp);
  m_Factor.SetFractDigits(FRACT_DIGITS);
  ltoa(m_dwFactor,buf,10);
  strTyp = buf;
  while (strTyp.GetLength() < 3) strTyp = "0" + strTyp;
  strTyp = strTyp.Left(strTyp.GetLength()-FRACT_DIGITS)+ ',' + strTyp.Right(FRACT_DIGITS);
  m_Factor.SetText(strTyp); */
  
	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE

}


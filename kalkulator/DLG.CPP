// dlg.cpp : implementation file
// 96.06.20 10:17

#include "stdafx.h"
#include "kkm.h"
#include "dlg.h"


#ifdef _DEBUG
#undef THIS_FILE
static char BASED_CODE THIS_FILE[] = __FILE__;
#endif

#define FRACT_DIGITS 2
#define FRACT_DIGITS_STOPA 3

extern int Changes;
CString Dat00;
/////////////////////////////////////////////////// CDlgNal dialog
CDlgNal::CDlgNal(CWnd* pParent /*=NULL*/)
    : CDialog(CDlgNal::IDD, pParent)
{
 //{{AFX_DATA_INIT(CDlgNal)
    m_skrOds = FALSE;
    //}}AFX_DATA_INIT
}
//--------------------------------------------------------
void CDlgNal::DoDataExchange(CDataExchange* pDX)
{  CDialog::DoDataExchange(pDX);
   //{{AFX_DATA_MAP(CDlgNal)
   DDX_Control(pDX, IDC_TYPN, m_nListTyp);
   DDX_Text(pDX, IDC_NALNO, m_nNo);
   DDX_Control(pDX, IDC_DATE, m_sDate); 
   DDX_Control(pDX, IDC_GROSZY, m_nGroszy);
   //}}AFX_DATA_MAP
   //DDX_DollarsCents(pDX, IDC_GROSZY, m_nGroszy);
   //DDX_StrDate(pDX, IDC_DATE, m_nDate);
}

BEGIN_MESSAGE_MAP(CDlgNal, CDialog)
    //{{AFX_MSG_MAP(CDlgNal)
    ON_EN_CHANGE(IDC_DATE, OnChange)
    ON_EN_CHANGE(IDC_GROSZY, OnChange)
    ON_CBN_SELCHANGE(IDC_TYPN, OnChange)
	
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()


//////////////////////////////////// CDlgNal message handlers
void CDlgNal::OnOK()
{ 
  UpdateData();
  
  if (Changes) 
  {
    m_nTyp = m_nListTyp.GetCurSel();
    if (m_nTyp == LB_ERR) return;
    
    // date
  	UpdateData();
    if (!m_sDate.IsValidDate() || m_sDate.GetText() == "----------") 
    {
      m_sDate.SetFocus();
	    AfxMessageBox(IDS_INVALID_DATE);
      return;
    }	
    m_strDate = m_sDate.GetText();
    m_strDate = m_strDate.Left(6)+m_strDate.Right(2);
    // money
    m_dwGroszy = m_nGroszy.GetLongValue();

    CDialog::OnOK();

    m_pDoc->UpdateNal(NULL,m_nNo,m_dwGroszy,m_nTyp,m_strDate,m_skrOds);
  }
  else CDialog::OnOK();
}
//--------------------------------------------------------
inline void CDlgNal::OnChange() { Changes = TRUE; }
//--------------------------------------------------------

BOOL CDlgNal::OnInitDialog()
{ int i;		//,j
 CString strTyp;
 char buf[100];
 //COleDateTime DateMin( 1951, 1, 1, 0, 0, 0 );
 //COleDateTime DateMax( 2049, 12, 31, 0, 0, 0 );

 Changes = FALSE;
 i =m_pDoc->GetNal(m_pDoc->m_nalActive, m_dwGroszy, m_nTyp, m_strDate);
 m_nNo = m_pDoc->m_nalActive;
 m_skrOds=i;
 CDialog::OnInitDialog();
 
 // date
 //m_sDate.SetSeparator('-');
 //m_sDate.SetRange(DateMin,DateMax);
 if (m_strDate[6] < '5')
    {
      m_strDate = m_strDate.Left(6)+"20"+m_strDate.Right(2);
    }
    else
    {
      m_strDate = m_strDate.Left(6)+"19"+m_strDate.Right(2);
    }
 m_sDate.SetDate(m_strDate);

 // money
 m_nGroszy.SetFractDigits(FRACT_DIGITS);
 ltoa(m_dwGroszy,buf,10);
 strTyp = buf;
 while (strTyp.GetLength() < 3) strTyp = "0" + strTyp;
 strTyp = strTyp.Left(strTyp.GetLength()-FRACT_DIGITS)+ ',' + strTyp.Right(FRACT_DIGITS);
 m_nGroszy.SetText(strTyp);
//
 m_nListTyp.ResetContent();
 for (i = 0; i<12;  i++) {   /*IleNazNal*/
  strTyp.LoadString(IDS_NTYP0+i);  m_nListTyp.AddString(strTyp);
 }
 m_nListTyp.SetCurSel(m_nTyp);

 if (m_nTyp==T_NALEZNOSC || m_nTyp==T_KWOTA_ZAB) {
   CWnd* pWnd; pWnd = GetDlgItem(IDC_SKR_ODS);
   pWnd->EnableWindow(FALSE); pWnd->ShowWindow(SW_HIDE);
 }
 return TRUE;
}


//////////////////////////////////////////////////// CDlgWpl dialog
CDlgWpl::CDlgWpl(CWnd* pParent /*=NULL*/)
    : CDialog(CDlgWpl::IDD, pParent)
{
    //{{AFX_DATA_INIT(CDlgWpl)
    //}}AFX_DATA_INIT
}
//--------------------------------------------------------
void CDlgWpl::DoDataExchange(CDataExchange* pDX)
{
    CDialog::DoDataExchange(pDX);
    //{{AFX_DATA_MAP(CDlgWpl)
    DDX_CBIndex(pDX, IDC_TYPW, m_wTyp);
    DDX_Text(pDX, IDC_WPLNO, m_wNo);
    DDX_Control(pDX, IDC_GROSZY, m_nGroszy);
    DDX_Control(pDX, IDC_DATE, m_sDate);
    //}}AFX_DATA_MAP
    //DDX_DollarsCents(pDX, IDC_GROSZY, m_wGroszy);
    //DDX_StrDate(pDX, IDC_DATE, m_wDate);
}

BEGIN_MESSAGE_MAP(CDlgWpl, CDialog)
    //{{AFX_MSG_MAP(CDlgWpl)
    ON_EN_CHANGE(IDC_DATE, OnChange)
    ON_EN_CHANGE(IDC_GROSZY, OnChange)
    ON_CBN_SELCHANGE(IDC_TYPW, OnChange)
	
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()


///////////////////////////////////////////// CDlgWpl message handlers
void CDlgWpl::OnOK()
{ 
  UpdateData();
  if (Changes) 
  {
  // date
  	if (!m_sDate.IsValidDate() || m_sDate.GetText() == "----------") 
    {
      m_sDate.SetFocus();
	    AfxMessageBox(IDS_INVALID_DATE);
      return;
    }	
    m_strDate = m_sDate.GetText();
	  m_strDate = m_strDate.Left(6)+m_strDate.Right(2);
    // money
    m_dwGroszy = m_nGroszy.GetLongValue();
  
    CDialog::OnOK();
   
    m_pDoc->UpdateWpl(NULL,m_wNo,m_dwGroszy,m_wTyp,m_strDate,m_wSQLTyp); 
  }
  else CDialog::OnOK();
}
//--------------------------------------------------------
inline void CDlgWpl::OnChange() { Changes = TRUE; }
//--------------------------------------------------------
BOOL CDlgWpl::OnInitDialog()
{ CString strTyp;
  char buf[100];
  //COleDateTime DateMin( 1951, 1, 1, 0, 0, 0 );
  //COleDateTime DateMax( 2049, 12, 31, 0, 0, 0 );
 
 Changes = FALSE;
 m_pDoc->GetWpl(m_pDoc->m_wplActive, m_dwGroszy, m_wTyp, m_strDate,m_wSQLTyp);
 m_wNo = m_pDoc->m_wplActive;
 CDialog::OnInitDialog();
 // date
 //m_sDate.SetSeparator('-');
 //m_sDate.SetRange(DateMin,DateMax);
 if (m_strDate[6] < '5')
 {
    m_strDate = m_strDate.Left(6)+"20"+m_strDate.Right(2);
 }
 else
 {
    m_strDate = m_strDate.Left(6)+"19"+m_strDate.Right(2);
 }
 m_sDate.SetDate(m_strDate);

 // money
 m_nGroszy.SetFractDigits(FRACT_DIGITS);
 ltoa(m_dwGroszy,buf,10);
 strTyp = buf;
 while (strTyp.GetLength() < 3) strTyp = "0" + strTyp;
 strTyp = strTyp.Left(strTyp.GetLength()-FRACT_DIGITS)+ ',' + strTyp.Right(FRACT_DIGITS);
 m_nGroszy.SetText(strTyp);
//
 
 return TRUE;
}


//////////////////////////////////////////////////// CDlgOkr dialog
CDlgOkr::CDlgOkr(CWnd* pParent /*=NULL*/)
    : CDialog(CDlgOkr::IDD, pParent)
{
    //{{AFX_DATA_INIT(CDlgOkr)
    //}}AFX_DATA_INIT
}
//--------------------------------------------------------
void CDlgOkr::DoDataExchange(CDataExchange* pDX)
{  CDialog::DoDataExchange(pDX);
   //{{AFX_DATA_MAP(CDlgOkr)
    DDX_Text(pDX, IDC_OKRNO, m_oNum);
    DDX_Radio(pDX, IDC_RADIOW0, m_TypW);
    DDX_Radio(pDX, IDC_RADIOO0, m_TypO);
    DDX_Control(pDX, IDC_DATEP, m_DateP);
    DDX_Control(pDX, IDC_DATEK, m_DateK);
    DDX_Control(pDX, IDC_STOPA, m_Stopa);
    DDX_Control(pDX, IDC_FACTOR,m_Factor);
	//}}AFX_DATA_MAP
    //DDX_Stopa(pDX, IDC_STOPA, m_Stopa);
    //DDX_StrDate(pDX, IDC_DATEP, m_DateP);
    //DDX_StrDate(pDX, IDC_DATEK, m_DateK);
}

BEGIN_MESSAGE_MAP(CDlgOkr, CDialog)
    //{{AFX_MSG_MAP(CDlgOkr)
    ON_EN_CHANGE(IDC_DATEK, OnChange)
    ON_EN_CHANGE(IDC_DATEP, OnChange)
    ON_EN_CHANGE(IDC_STOPA, OnChange)
	ON_EN_CHANGE(IDC_FACTOR, OnChange)
    ON_BN_CLICKED(IDC_RADIOW0, OnChange )
    ON_BN_CLICKED(IDC_RADIOW1, OnChange )
    ON_BN_CLICKED(IDC_RADIOW2, OnChange )
    ON_BN_CLICKED(IDC_RADIOO0, OnChange )
    ON_BN_CLICKED(IDC_RADIOO1, OnChange )
    ON_BN_DOUBLECLICKED(IDC_RADIOW0, OnChange )
    ON_BN_DOUBLECLICKED(IDC_RADIOW1, OnChange )
    ON_BN_DOUBLECLICKED(IDC_RADIOW2, OnChange )
    ON_BN_DOUBLECLICKED(IDC_RADIOO1, OnChange )
    ON_BN_DOUBLECLICKED(IDC_RADIOO2, OnChange )
	
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//////////////////////////////////////////////// CDlgOkr message handlers
BOOL CDlgOkr::OnInitDialog()
{ CString strTyp;
  //COleDateTime DateMin( 1951, 1, 1, 0, 0, 0 );
  //COleDateTime DateMax( 2049, 12, 31, 0, 0, 0 );
  char buf[100];

 Changes = FALSE; 
 COkrOd* pOkrOd;
 CNal* pNal;

 m_oNum = m_pDoc->m_odsActive;
 m_oParent = m_pDoc->m_nalActive;
 pOkrOd = m_pDoc->GetOkrOd(m_oNum,m_oParent,m_dwStopa,m_dwFactor,m_TypO,m_TypW,m_strDateP,m_strDateK);
 if (pOkrOd) {
   pNal =  m_pDoc->GetParentNal(pOkrOd);
   Dat00= pNal->m_nalDate;
 }
 else Dat00="00-00-00";
 CDialog::OnInitDialog();
 
 // date
 //m_DateP.SetSeparator('-');
 //m_DateP.SetRange(DateMin,DateMax);
 //m_DateK.SetSeparator('-');
 //m_DateK.SetRange(DateMin,DateMax);

 if (m_strDateP[6] < '5') m_strDateP = m_strDateP.Left(6)+"20"+m_strDateP.Right(2);
 else m_strDateP = m_strDateP.Left(6)+"19"+m_strDateP.Right(2);

 if (m_strDateK != "--------")
 {
  if (m_strDateK[6] < '5') m_strDateK = m_strDateK.Left(6)+"20"+m_strDateK.Right(2);
  else m_strDateK = m_strDateK.Left(6)+"19"+m_strDateK.Right(2);
 }
 else m_strDateK = "----------";

 m_DateP.SetDate(m_strDateP);
 //if (m_strDateK[0]=='-') 
  // ;//m_DateK.SetDate(DateMax);
 //else 
 m_DateK.SetDate(m_strDateK);

 // procent
 m_Stopa.SetFractDigits(FRACT_DIGITS_STOPA);
 m_dwStopa = m_dwStopa / 10;
 ltoa(m_dwStopa,buf,10);
 strTyp = buf;
 while (strTyp.GetLength() < 3) strTyp = "0" + strTyp;
 strTyp = strTyp.Left(strTyp.GetLength()-FRACT_DIGITS_STOPA)+ ',' + strTyp.Right(FRACT_DIGITS_STOPA);
 m_Stopa.SetText(strTyp);
//
// factor - mnoznik
 m_Factor.SetFractDigits(FRACT_DIGITS_STOPA);
 m_dwFactor = m_dwFactor / 10;
 ltoa(m_dwFactor,buf,10);
 strTyp = buf;
 while (strTyp.GetLength() < 3) strTyp = "0" + strTyp;
 strTyp = strTyp.Left(strTyp.GetLength()-FRACT_DIGITS_STOPA)+ ',' + strTyp.Right(FRACT_DIGITS_STOPA);
 m_Factor.SetText(strTyp);
// 
 return TRUE;  // return TRUE  unless you set the focus to a control
}
//--------------------------------------------------------
inline void CDlgOkr::OnChange() { Changes = TRUE; }
//--------------------------------------------------------
void CDlgOkr::OnOK()
{ UpdateData(TRUE);
  
  // date /////////////////////////////////////////
    if (!m_DateP.IsValidDate()|| m_DateP.GetText() == "----------") 
    {
      m_DateP.SetFocus();
	    AfxMessageBox(IDS_INVALID_DATE);
      return;
    }	
    if (!m_DateK.IsValidDate()) 
    {
      m_DateK.SetFocus();
	    AfxMessageBox(IDS_INVALID_DATE);
      return;
    }
    
  	m_strDateP = m_DateP.GetText();
	  m_strDateP = m_strDateP.Left(6)+m_strDateP.Right(2);

    m_strDateK = m_DateK.GetText();
	  if (m_strDateK == "----------") m_strDateK = "--------";
    else
        m_strDateK = m_strDateK.Left(6)+m_strDateK.Right(2);
  // procent
    m_dwStopa = m_Stopa.GetLongValue();
    m_dwStopa *= 10; // R - 00
   // factor - mnoznik
    m_dwFactor = m_Factor.GetLongValue();
    m_dwFactor *= 10; // R - 00
  
  
  CDialog::OnOK();
  if (Changes) {
    if (m_strDateK != "--------" && ( GetStrDate(m_strDateK) < GetStrDate(m_strDateP))) 
      m_strDateP = m_strDateK;
    m_pDoc->UpdateOkrOd(NULL,m_oNum,m_oParent,m_dwStopa,m_dwFactor,m_TypO,m_TypW,m_strDateP,m_strDateK);
  }
}





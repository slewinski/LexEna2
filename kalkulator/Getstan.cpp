// getstan.cpp : implementation file
// 96.06.10 08:29

#include "stdafx.h"
#include "kkm.h"
#include "zlgr.h"
#include "getstan.h"

#ifdef _DEBUG
#undef THIS_FILE
static char BASED_CODE THIS_FILE[] = __FILE__;
#endif

extern int Changes;
int Changes1;
/////////////////////////////////////////////////////////////////////////////
// CGetStan dialog


CGetStan::CGetStan(CWnd* pParent /*=NULL*/) : CDialog((CGetStan::IDD /*+theApp.m_nTyp*/), pParent)
{
    //{{AFX_DATA_INIT(CGetStan)
	//}}AFX_DATA_INIT
}

void CGetStan::DoDataExchange(CDataExchange* pDX)
{
    CDialog::DoDataExchange(pDX);
    //{{AFX_DATA_MAP(CGetStan)
    DDX_Control(pDX, IDC_STAN_LIST, m_listStan);
	  DDX_Text(pDX, IDC_EDIT1, m_Sprawa);
	  DDV_MaxChars(pDX, m_Sprawa, 12);
	  //}}AFX_DATA_MAP
    DDX_Radio(pDX, IDC_STAN_KAL, m_Kal_Bank);   //    if (!theApp.m_nTyp)
}

BEGIN_MESSAGE_MAP(CGetStan, CDialog)
    //{{AFX_MSG_MAP(CGetStan)
    ON_BN_CLICKED(ID_NEW_ELEM, OnNewElem)
    ON_BN_CLICKED(ID_DEL_ELEM, OnDelElem)
    ON_LBN_DBLCLK(IDC_STAN_LIST, OnEditElem)
    ON_BN_CLICKED(ID_EDIT_ELEM, OnEditElem)
    //}}AFX_MSG_MAP
END_MESSAGE_MAP()

//----------------------------------------------------------
BOOL CGetStan::OnInitDialog()
{
 CDialog::OnInitDialog();
 Changes1=0;
 PutList();
 return TRUE;  // return TRUE  unless you set the focus to a control
}

//----------------------------------------------------------
void CGetStan::PutList()
{
CStanD* pStan;
POSITION pos;
 m_listStan.ResetContent();
 for ( pos=m_pDoc->GetFirstStanPos(); pos!=NULL; m_pDoc->GetNextStan(pos)) {
   pStan = (CStanD*)m_pDoc->m_stanList.GetAt(pos);
   m_listStan.AddString(pStan->m_stanD);
 }
}


/////////////////////////////////////////////////////////////////////////////
// CGetStan message handlers
//---------------------------------------
void CGetStan::OnNewElem()
{ int r;
  CGetDate NewDate;
  Changes=0;
  //NewDate.m_strDate = "01-01-02";
  
  r = NewDate.DoModal();
  if (r==2) return;                  // CANCEL
  if (r==1 && !Changes) return;      // OK
  m_listStan.AddString(NewDate.m_strDate);
  SortStan();
  Changes1=1;
}
//---------------------------------------------------------------
void CGetStan::SortStan()
{
  char st[10];
  CStringArray ll;
  CString w;
  int rok,rok1,n, mies, mies1, day, day1;
  int i,j;
  
  n = m_listStan.GetCount();
  for ( i=0;i < n;i++) {
    m_listStan.GetText(i,st);
    w = st;
    ll.Add(w);
  }
  for ( i=0;i < n -1; i++) {
    rok = atoi((const char *)ll[i].Mid(6,2));
    if (rok >49) rok+=1900; else rok += 2000;
    mies = atoi((const char *)ll[i].Mid(3,2));
    day = atoi((const char *)ll[i].Left(2));
    for ( j=i+1; j<n;j++) {
      //m_listStan.GetText(j,st1);
      rok1 = atoi((const char *)ll[j].Mid(6,2));
      if (rok1 >49) rok1+=1900; else rok1 += 2000;
      mies1 = atoi((const char *)ll[j].Mid(3,2));
      day1 = atoi((const char *)ll[j].Left(2));
      if (rok > rok1 || ((rok==rok1)&&(mies>mies1)) || ((rok==rok1)&&(mies==mies1)&&(day>day1)))
      {
        w = ll[i];         ll[i]=ll[j];         ll[j]=w;
      }
    }
  }
 m_listStan.ResetContent();
 for ( i=0; i<n; i++) {
  m_listStan.AddString(ll[i]);
 }
}
//---------------------------------------
void CGetStan::OnDelElem()
{ int i;
  i = m_listStan.GetCurSel();
  if (i == LB_ERR) return;
  m_listStan.DeleteString(i);
  Changes1=1;
}

//---------------------------------------
void CGetStan::OnOK()
{ char st[10];
  int i,n;
  if (Changes1) {
    while (!m_pDoc->m_stanList.IsEmpty()) delete m_pDoc->m_stanList.RemoveHead();
    n = m_listStan.GetCount();
    for ( i=0; i<n; i++) {
       m_listStan.GetText(i,st);
       CStanD* pStanItem = new CStanD();
       pStanItem->m_stanD = st;
       m_pDoc->m_stanList.AddTail(pStanItem);
    }
    m_pDoc->SetModifiedFlag();   // Mark document as modified to confirm File Close
  }
  CDialog::OnOK();
}

//---------------------------------------
void CGetStan::OnEditElem()
{ int r,i;
  CGetDate NewDate;
  Changes=0;
  i = m_listStan.GetCurSel();
  if (i == LB_ERR) return;
  m_listStan.GetText(i,NewDate.m_strDate);
  Changes1=1;
  r = NewDate.DoModal();
  if (r==2) return;                  // CANCEL
  if (r==1 && !Changes) return;      // OK
  m_listStan. DeleteString(i);  
  m_listStan.AddString(NewDate.m_strDate);
  SortStan();
  Changes1=1;
  GotoDlgCtrl(GetDlgItem(IDC_STAN_LIST));
}

//////////////////////////////////////////////////////// CGetDate dialog
CGetDate::CGetDate(CWnd* pParent /*=NULL*/)
    : CDialog(CGetDate::IDD, pParent)
{
    //{{AFX_DATA_INIT(CGetDate)
    //}}AFX_DATA_INIT
}
//------------------------------------------------
void CGetDate::DoDataExchange(CDataExchange* pDX)
{
    CDialog::DoDataExchange(pDX);
    //{{AFX_DATA_MAP(CGetDate)
    DDX_Control(pDX, IDC_DATEP, m_sDate);
    //}}AFX_DATA_MAP
}
//------------------------------------------------
BEGIN_MESSAGE_MAP(CGetDate, CDialog)
    //{{AFX_MSG_MAP(CGetDate)
    ON_EN_CHANGE(IDC_DATEP, OnChangeDate)
	
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//////////////////////////////////////////// CGetDate message handlers
void CGetDate::OnChangeDate() { Changes=1;}

void CGetDate::OnOK() 
{
	UpdateData();
  if (!m_sDate.IsValidDate() || m_sDate.GetText()=="----------") 
  {
    m_sDate.SetFocus();
	  AfxMessageBox(IDS_INVALID_DATE);
    return;
  }	
  m_strDate = m_sDate.GetText();
  m_strDate = m_strDate.Left(6)+m_strDate.Right(2);
	Changes = 1;
  CDialog::OnOK();
}

BOOL CGetDate::OnInitDialog() 
{
	//COleDateTime DateMin( 1951, 1, 1, 0, 0, 0 );
  //COleDateTime DateMax( 2049, 12, 31, 0, 0, 0 );

  CDialog::OnInitDialog();
  //m_sDate.SetSeparator('-');
  //m_sDate.SetRange(DateMin,DateMax);
  //m_sDate.SetDateToToday();
  m_sDate.SetDate(m_sDate.GetCurrentDateStr());
  //m_sDate.SetText( m_strDate );	
	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE

}



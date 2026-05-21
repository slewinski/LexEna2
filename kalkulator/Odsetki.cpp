// odsetki.cpp : implementation file
// 11-20-96 10:49pm

#include "stdafx.h"
#include "kkm.h"
#include "odsetki.h"
#include "zlgr.h"
#include "stdio.h"
#include "date.inl"

#ifdef _DEBUG
#undef THIS_FILE
static char BASED_CODE THIS_FILE[] = __FILE__;
#endif

int Changes;
extern int Changes1;
/////////////////////////////////////////////////////////////////////////////
// COdsUst dialog

COdsUst::COdsUst(CWnd* pParent)
    : CDialog(COdsUst::IDD, pParent) { }

void COdsUst::DoDataExchange(CDataExchange* pDX)
{ CDialog::DoDataExchange(pDX);
  //{{AFX_DATA_MAP(COdsUst)
  DDX_Control(pDX, IDC_ODS_COMBO, m_OdsUst);
  //}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(COdsUst, CDialog)
    //{{AFX_MSG_MAP(COdsUst)
    ON_BN_CLICKED(ID_NEW_ELEM, OnNewElem)
    ON_LBN_DBLCLK(IDC_ODS_COMBO, OnDblclkOds)
    //}}AFX_MSG_MAP
END_MESSAGE_MAP()


///////////////////////////////////////// COdsUst message handlers
//----------------------------------------------------------
BOOL COdsUst::OnInitDialog()
{ CDialog::OnInitDialog(); PutList();Changes1=0; return TRUE; }

//----------------------------------------------------------
void COdsUst::PutList()
{
int i; char buf[60];
CString str2;
CString strStopa;
  m_OdsUst.ResetContent();
  for (i=0; i<IleOkrOdsUst; i++) {
    if (i!=IleOkrOdsUst-1)
      { CDate datC(OOUV[i+1].lDP-1L); datC.DateFormat(str2); }
    else 
      { str2 = "--------";}
    sprintf(buf,"%s   %s  %4lu,%02lu%%",
             (const char*)OOUV[i].strD,
             (const char*)str2,
             OOUV[i].Stopa/100,OOUV[i].Stopa%100);
    m_OdsUst.AddString(buf);
  }
}

//----------------------------------------------------------
void COdsUst::OnNewElem()
{ int r, rf;
  long d;
  CString str2, W;
  char buf[40];
  CNewOkr NewOkr;
  Changes=0;
  NewOkr.m_strDate = theApp.m_Date;
  NewOkr.m_dwStopa  =  0;
  NewOkr.m_dwFactor =  1000;

  r = NewOkr.DoModal();
  if (r==2) return;                  // CANCEL
  if (r==1 && !Changes) return;      // OK
  r = m_OdsUst.GetCount()-1;
  m_OdsUst.GetText(r,buf);
  
  d=GetStrDate(NewOkr.m_strDate);
	W = buf; 
	if (GetStrDate(W) >= d)  { MessageBeep(MB_ICONHAND); return; }
	m_OdsUst.DeleteString(r);
  CDate datC(d-1L); datC.DateFormat(str2);
  memcpy(buf+11,(const char *)str2,8);
  m_OdsUst.AddString(buf);
  r = (int)(NewOkr.m_dwStopa/1000L); //Marzena bylo 1000L  R - 00
  rf = (int)(NewOkr.m_dwFactor/1000L); 

  sprintf(buf,"%s   --------  %4u,%02lu%% x %4u,%02lu",(const char *)NewOkr.m_strDate,r/100,r%100,rf/100,rf%100);
  m_OdsUst.AddString(buf);

  m_OdsUst.SetCurSel(m_OdsUst.GetCount()-1);
  Changes1=1;
}
//----------------------------------------------------------
void COdsUst::OnOK()
{ int i;
DWORD oStopa,oStopaD;
 if (Changes1) {
   IleOkrOdsUst = m_OdsUst.GetCount();
   //char *pszOdsPath="odsetki.ust";
   //char pszOdsPath[70]="";
   CFile OdsFile;
   char szBuffer[20],buf[60];
	 CString W;
		W = theApp.GetProfileString("Config","PathToOds");
		//GetPrivateProfileString("Config","PathToOds","_ustaw.ods",pszOdsPath,50,"km.ini");
		if (W.IsEmpty()) {
			W = "_ustaw.ods";
		}
		else { if (W.Right(1) == '\\') W += "_ustaw.ods";
		else	W += "\\_ustaw.ods";
		}
   OdsFile.Remove((const char *)W);
   if (! OdsFile.Open((const char *)W, CFile::modeCreate|CFile::modeWrite))
     { AfxMessageBox("Brak pliku odsetek ustawowych");   return; }
   wsprintf(szBuffer,"%u\r\n",IleOkrOdsUst);
   OdsFile.Write(szBuffer,4);

   for (i=0; i<IleOkrOdsUst; i++) {
     m_OdsUst.GetText(i,buf);
     sscanf(buf,"%s   %s  %4lu,%2lu",&szBuffer[0],&szBuffer[10],&oStopa, &oStopaD);
     OOUV[i].strD = szBuffer;
     OOUV[i].Stopa = oStopa*100+oStopaD;//100L;
     OOUV[i].lDP=(date_t)GetStrDate(OOUV[i].strD);
     OdsFile.Write(szBuffer,8);
     wsprintf(szBuffer,"%6lu\r\n",OOUV[i].Stopa);
     OdsFile.Write(szBuffer,8);
   }
   OdsFile.Close();
 }
 CDialog::OnOK();
}

//------------------------------------------------------------------
void COdsUst::OnDblclkOds()
{ m_OdsUst.DeleteString(m_OdsUst.GetCount()-1); Changes1=1; }

//----------------------------------------------------------


// kkmDoc.cpp : implementation of the CKkmDoc class
//
#include "stdafx.h"
#include <math.h>
#include <stdio.h>
#include <string.h>
#include "kkm.h"
#include "odsetki.h"
#include "dlg.h"
#include "mainfrm.h"
#include "nagl.h"
#include "date.inl"

#ifdef _DEBUG
#undef THIS_FILE
static char BASED_CODE THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CKmDoc

IMPLEMENT_DYNCREATE(CKmDoc, CDocument)

BEGIN_MESSAGE_MAP(CKmDoc, CDocument)
    //{{AFX_MSG_MAP(CKmDoc)
    ON_COMMAND(ID_NEW_ELEM, OnNewElem)
    ON_UPDATE_COMMAND_UI(ID_NEW_ELEM, OnUpdateNewElem)
    ON_COMMAND(ID_DEL_ELEM, OnDelElem)
    ON_UPDATE_COMMAND_UI(ID_DEL_ELEM, OnUpdateDelElem)
    ON_COMMAND(ID_NEXT, OnNext)
    ON_COMMAND(ID_PREV, OnPrev)
    ON_UPDATE_COMMAND_UI(ID_PREV, OnUpdatePrev)
    ON_UPDATE_COMMAND_UI(ID_NEXT, OnUpdateNext)
    ON_COMMAND(ID_ODSETKI, OnOdsetki)
    ON_COMMAND(ID_OBL_STANDATE, OnStanDate)
    ON_COMMAND(IDM_NAG, OnNag)
    ON_UPDATE_COMMAND_UI(IDM_NAG, OnUpdateNag)
    ON_COMMAND(IDM_STOP, OnStop)
    ON_UPDATE_COMMAND_UI(IDM_STOP, OnUpdateStop)
	ON_COMMAND(ID_KONWERT, OnKonwert)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CKmDoc construction/destruction

CKmDoc::CKmDoc()  { }
CKmDoc::~CKmDoc() { }

//---------------------------------------------------------------
#include "docnal.cpp"
#include "docokr.cpp"
#include "docwpl.cpp"
#include "dock_w.cpp"
//---------------------------------------------------------------
void CKmDoc::DeleteContents()
{ CWpl * pWpl;
  while (!m_wplList.IsEmpty()) {
    pWpl = (CWpl*)m_wplList.GetHead();
    while (!pWpl->m_Po_Wpl.IsEmpty())  delete pWpl->m_Po_Wpl.RemoveHead();
    delete m_wplList.RemoveHead();
  }
  while (!m_nalList.IsEmpty())     delete m_nalList.RemoveHead();
  while (!m_odsList.IsEmpty())     delete m_odsList.RemoveHead();
  while (!m_stanList.IsEmpty())    delete m_stanList.RemoveHead();
  while (!m_k_Wpl.IsEmpty())       delete m_k_Wpl.RemoveHead();

	Konwert_Yes = 0;			// 15.06.02   Nie trzeba konwertowac = 0
}

//---------------------------------------------------------------
void CKmDoc::InitDocument()
{ CMainFrame* pFrameWnd = (CMainFrame*)theApp.m_pMainWnd;
  pFrameWnd->ViewIn();
  m_Sprawa = "";

  if (m_stanList.IsEmpty()) {
    CStanD* pStanItem = new CStanD();
    pStanItem->m_stanD = theApp.m_Date;
    m_stanList.AddTail(pStanItem);
  }
  m_wplActive = m_nalActive = m_odsActive = 0;  m_oblActive = -1;
  if (!m_wplList.IsEmpty()) m_wplActive = 1;
  if (!m_nalList.IsEmpty()) {
    m_nalActive++;
    if (GetOkrCount(m_nalActive)) m_odsActive++;
  }
}

//---------------------------------------------------------------
BOOL CKmDoc::OnNewDocument()
{ if (!CDocument::OnNewDocument())  return FALSE;
  m_TypSpr =0;   //theApp.m_nTyp;
  if (TypEnter==1) {
	  InitDocument(); 
	  return TRUE;
  }
  else
  {
    OnNewDocumentFile();
	return TRUE;
  }
}
//---------------------------------------------------------------
BOOL CKmDoc::OnNewDocumentFile()
{
  DWORD dwGroszy, dwGroszyF=1000;
  FILE *in; 
  int number, typ_wpl, typ_SQL;
  UINT 	  NumNal=0;
  UINT 	  NumWpl=0;
  UINT    m_oNum=0;
  int     nTypW;
  CString strDateP;
  CString strDateK;
  CString pszPathName;
	CString strDat;
  char str[20];
  char line0[1000],Bufer[1000];
  int len,len1,n,i;
  char *ch;
  int Count_Kwot;

  n=0;
  strDat = theApp.m_Date;
	//AfxMessageBox(theApp.m_lpCmdLine);
  if((in=fopen(theApp.m_lpCmdLine,"r"))!=NULL)
  {
	while( !feof( in ) )
	{
      if( fgets( line0, 1000, in ) != NULL)
	  {
        switch(n)
		{
case 0: Bufer[6]=line0[8];
		Bufer[7]=line0[9];
        Bufer[2]='-';
		Bufer[3]=line0[3];
		Bufer[4]=line0[4];
        Bufer[5]='-';
		Bufer[0]=line0[0];
		Bufer[1]=line0[1];
		Bufer[8]='\0';
		theApp.m_Date = Bufer;
		InitDocument();
		break;
case 1: strcpy(Bufer,line0);
		len=strlen(Bufer);
		for (i=0;i<len;i++) {if(Bufer[i]=='/') Bufer[i]='_';}
		break;
case 2: 
		if (line0[0]>32) {Bufer[len-1]=line0[0];len++;grupa=line0[0];}
		Bufer[len-1]='\0';
		m_Sprawa=Bufer;
		break;
case 3: number=atoi(line0); 
		flag_typ_month = number-1;
		break;
case 4: Count_Kwot=atoi(line0);
		break;
default:switch(line0[0]){
case '.':	break;
case ' ':	m_oNum++;
			NewOkrOd(NumNal);	
			Bufer[6]=line0[9];
			Bufer[7]=line0[10];
			Bufer[2]='-';
			Bufer[3]=line0[4];
			Bufer[4]=line0[5];
			Bufer[5]='-';
			Bufer[0]=line0[1];
			Bufer[1]=line0[2];
			Bufer[8]='\0';
			strDateP = Bufer;
			Bufer[6]=line0[20];
			Bufer[7]=line0[21];
			Bufer[2]='-';
			Bufer[3]=line0[15];
			Bufer[4]=line0[16];
			Bufer[5]='-';
			Bufer[0]=line0[12];
			Bufer[1]=line0[13];
			Bufer[8]='\0';
			strDateK = Bufer;
            str[0]=line0[23];
			str[1]='\0';
			number=atoi(str);
			number--;
			if (number==0) {	// odsetki ustawowe
				dwGroszy = 0;dwGroszyF = 1000;
				nTypW = 1;
			}
			else{		// odsetki umowne
				len=25;
				len1=strlen(line0)-len-1;
				ch=&line0[len];
    			strncpy(str,ch,len1);
				str[len1]='\0';
	    	    dwGroszy=(DWORD)(atof(str)*10000.0+10E-5);
				//dwGroszy=(DWORD)(atof(str)*100000.0+10E-5); v1.5 import z Komornika SQL
				nTypW = 1;
			}
				UpdateOkrOd(NULL,m_oNum,NumNal,dwGroszy, dwGroszyF, number,
					        nTypW,strDateP,strDateK);
			break;  
default : 	
			// wczytanie naleznosci, wplat z Komornika SQL
				len1=strlen(line0);		
				ch=&line0[len1 -2];
				if(ch[0] =='w' || ch[0]=='z' || ch[0]=='d' || ch[0]=='s')  {
				// jednak wplata
				typ_wpl=0;  //KON
				typ_SQL=(int)ch[0];
				// rozwiklujemy wplaty bezposrednie
				ch=&line0[len1 -3];
				if(ch[0] == 'b') typ_SQL = _toupper(typ_SQL);
				NumWpl++;
				NewWpl();
				Bufer[6]=line0[8];
				Bufer[7]=line0[9];
				Bufer[2]='-';
				Bufer[3]=line0[3];
				Bufer[4]=line0[4];
				Bufer[5]='-';
				Bufer[0]=line0[0];
				Bufer[1]=line0[1];
				Bufer[8]='\0';
				strDateP = Bufer;
			
				ch=strchr(line0,',');
				ch++;
				strcpy(str,ch);
				ch=strrchr(str,',');
				ch[0]='\0';
				dwGroszy=(DWORD)(atof(str)*100+10E-5);
				UpdateWpl(NULL,NumWpl,dwGroszy,typ_wpl,strDateP,typ_SQL);
			}
			else {
				//  kolejna naleznosc
				NumNal++;m_oNum = 0;
				NewNal();
				Bufer[6]=line0[8];
				Bufer[7]=line0[9];
				Bufer[2]='-';
				Bufer[3]=line0[3];
				Bufer[4]=line0[4];
				Bufer[5]='-';
				Bufer[0]=line0[0];
				Bufer[1]=line0[1];
				Bufer[8]='\0';
				strDateP = Bufer;
				ch=&line0[11];
				if (line0[12]==',') {str[0]=line0[11];str[1]='\0';len=13;}
				else {str[0]=line0[11];str[1]=line0[12];str[2]='\0';len=14;}
				number=atoi(str);
				switch (number){
				case  1:number=8;break;
				case  2:number=9;break;					
				case  3:number=3;break;
				case  4:number=5;break;
				case  5:number=0;break;
	//			case  6:number=6;break;
				case  6:number=7;break;//*****
				case  7:number=6;break;
				case  8:number=1;break;
				case  9:number=2;break;
				case 10:number=3;break;
				case 11:number=4;break;
				case 12:number=10;break;
				case 21:number=11;break;
				default:{
					CString strD;
					strD.LoadString(IDS_ZLY_TYP);
					AfxMessageBox(strD, MB_OK);
					_exit(0);break;
					}
				}
				len1=strlen(line0)-len-1;
				ch=&line0[len];
				strncpy(str,ch,len1);
				str[len1]='\0';
				dwGroszy=(DWORD)(atof(str)*100+10E-5);
				UpdateNal(NULL,NumNal,dwGroszy,number,strDateP,0);
			}
			break;   
		}
		}
n++;
	  }
	} 
      fclose(in);
	  }
pszPathName = (CString)theApp.m_lpCmdLine;
n=pszPathName.ReverseFind( '\\' )+1;
pszPathName=pszPathName.Left(n);
pszPathName+=m_Sprawa;
strDateP=".km";
pszPathName+=strDateP;
theApp.m_Date = strDat;
OnSaveDocument(pszPathName);
SetTitle(m_Sprawa);
if (!m_nalList.IsEmpty()) {
    m_nalActive++;
    if (GetOkrCount(m_nalActive)) m_odsActive++;
}
return TRUE;
}
//---------------------------------------------------------------
BOOL CKmDoc::OnOpenDocument(const char* pszPathName)
{
  if (!CDocument::OnOpenDocument(pszPathName))   return FALSE;
  if (m_TypSpr !=0 /*theApp.m_nTyp*/)
    { MessageBeep(MB_ICONHAND); return FALSE; }
  theApp.UpdateIniFileWithDocPath(pszPathName);
  InitDocument();  return TRUE;
}

//---------------------------------------------------------------
BOOL CKmDoc::OnSaveDocument(const char* pszPathName)
{ if (!CDocument::OnSaveDocument(pszPathName))   return FALSE;
  theApp.UpdateIniFileWithDocPath(pszPathName);
  return TRUE;
}

//---------------------------------------------------------------
void CKmDoc::OnCloseDocument()
{ CMainFrame* pFrameWnd = (CMainFrame*)theApp.m_pMainWnd;
 CDocument::OnCloseDocument();
 pFrameWnd->ViewOut();
 theApp.m_nAct=0;  //Hej@ho bylo '=='  
 TypEnter=1;
 return; }

/////////////////////////////////////////////////////////////////////////////
// CKmDoc serialization

void CKmDoc::Serialize(CArchive& ar)
{ if (ar.IsStoring())  ar << m_TypSpr;
  else                 ar >> m_TypSpr;
  m_nalList.Serialize ( ar );
  m_wplList.Serialize ( ar );
  m_odsList.Serialize ( ar );
  m_stanList.Serialize ( ar );
}

/////////////////////////////////////////////////////////////////////////////
// CKmDoc diagnostics

#ifdef _DEBUG
void CKmDoc::AssertValid() const
{ CDocument::AssertValid(); }
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CKmDoc comands

void CKmDoc::OnNewElem()
{ if (theApp.m_nAct==6)       DNewWpl();    // Wplata
  else if (theApp.m_nAct==2 ) DNewNal();    // Naleznosc
  else if (theApp.m_nAct==4 ) DNewOkrOd();  // Odsetki
}

//-----------------------------------------------------------------------
void CKmDoc::OnUpdateNewElem(CCmdUI* pCmdUI)
{
  if (theApp.m_nAct == 4)    pCmdUI->Enable(CanOds());
  else if (theApp.m_nAct==7 || theApp.m_nAct==0) pCmdUI->Enable(FALSE);
}

//-----------------------------------------------------------------------
void CKmDoc::OnDelElem()
{ if (theApp.m_nAct==7) return;

//--------------------------------------------- YES
 if (theApp.m_nAct==6) DelWpl();         // Wplata
 else if (theApp.m_nAct==2 ) DelNal();   // Naleznosc
 else if (theApp.m_nAct==4 ) DelOkrOd(); // Odsetki
}

//--------------------------------------------------------------------
void CKmDoc::OnUpdateDelElem(CCmdUI* pCmdUI)
{ if (theApp.m_nAct==6)      pCmdUI->Enable(m_wplList.GetCount() );
  else if (theApp.m_nAct==2) pCmdUI->Enable(m_nalList.GetCount() );
  else if (theApp.m_nAct==4) pCmdUI->Enable(GetOkrCount(m_nalActive));
  else                       pCmdUI->Enable(FALSE);
}

//--------------------------------------------------------------------
void CKmDoc::OnNext()
{ if (theApp.m_nAct==6)      ChangeSelectionNextWplNo(TRUE);
  else if (theApp.m_nAct==2) ChangeSelectionNextNalNo(TRUE);
  else if (theApp.m_nAct==4) ChangeSelectionNextOkrOdNo(TRUE);
}

//--------------------------------------------------------------------
void CKmDoc::OnPrev()
{ if (theApp.m_nAct==6)      ChangeSelectionNextWplNo(FALSE);
  else if (theApp.m_nAct==2) ChangeSelectionNextNalNo(FALSE);
  else if (theApp.m_nAct==4) ChangeSelectionNextOkrOdNo(FALSE);
}

//--------------------------------------------------------------------
void CKmDoc::OnUpdatePrev(CCmdUI* pCmdUI)
{ if (theApp.m_nAct==6)      pCmdUI->Enable(m_wplActive > 1);
  else if (theApp.m_nAct==2) pCmdUI->Enable(m_nalActive > 1);
  else if (theApp.m_nAct==4) pCmdUI->Enable(m_odsActive > 1);
  else                       pCmdUI->Enable(FALSE);
}
//--------------------------------------------------------------------
void CKmDoc::OnUpdateNext(CCmdUI* pCmdUI)
{ if (theApp.m_nAct==6)      pCmdUI->Enable((int)m_wplActive<m_wplList.GetCount());
  else if (theApp.m_nAct==2) pCmdUI->Enable((int)m_nalActive<m_nalList.GetCount());
  else if (theApp.m_nAct==4) pCmdUI->Enable(m_odsActive<GetOkrCount(m_nalActive));
  else                       pCmdUI->Enable(FALSE);
}

//--------------------------------------------------------------------
void CKmDoc::OnOdsetki() {  COdsUst OdsUst;   OdsUst.DoModal(); }

//--------------------------------------------------------------------
void CKmDoc::OnNag() { CNagl Nagl; Nagl.DoModal(); }

//--------------------------------------------------------------------
void CKmDoc::OnUpdateNag(CCmdUI* pCmdUI)
{ pCmdUI->Enable(theApp.m_nAct!=7&&theApp.m_nAct!=0); } 

//--------------------------------------------------------------------
void CKmDoc::OnStop()
{ CStop Stop; Stop.DoModal(); }                    

//--------------------------------------------------------------------
void CKmDoc::OnUpdateStop(CCmdUI* pCmdUI)
{ pCmdUI->Enable(theApp.m_nAct!=7&&theApp.m_nAct!=0); }

void CKmDoc::OnKonwert() 
{
  CNal* pNal;
  POSITION pos;
  CString strDate;
  for ( pos = m_nalList.GetHeadPosition(); pos != NULL; m_nalList.GetNext(pos)) {
    pNal = (CNal*)m_nalList.GetAt(pos);
    strDate = pNal->m_nalDate;
    pNal->m_nalDate = strDate.Right(2) + strDate.Mid(2,4) +	strDate.Left(2);
  }

  CWpl* pWpl;
  for ( pos = m_wplList.GetHeadPosition(); pos != NULL; m_wplList.GetNext(pos)) {
    pWpl = (CWpl*)m_wplList.GetAt(pos);
    strDate = pWpl->m_wplDate;
	pWpl->m_wplDate = strDate.Right(2) + strDate.Mid(2,4) +	strDate.Left(2);
  }
  COkrOd* pOkrOd;
  
  for ( pos = m_odsList.GetHeadPosition(); pos != NULL; m_odsList.GetNext(pos)) {
    pOkrOd = (COkrOd*)m_odsList.GetAt(pos);
    strDate = pOkrOd->m_okrDateP;
	pOkrOd->m_okrDateP = strDate.Right(2) + strDate.Mid(2,4) +	strDate.Left(2);
	strDate = pOkrOd->m_okrDateK;
	pOkrOd->m_okrDateK = strDate.Right(2) + strDate.Mid(2,4) +	strDate.Left(2);
  }

  CStanD* pStan;
	for ( pos=GetFirstStanPos(); pos!=NULL; GetNextStan(pos)) {
		pStan = (CStanD*)m_stanList.GetAt(pos);
	    strDate = pStan->m_stanD;
		pStan->m_stanD = strDate.Right(2) + strDate.Mid(2,4) +	strDate.Left(2);
	}
	UpdateAllViews(NULL,55555,NULL);
	
}

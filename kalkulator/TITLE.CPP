// titleobj.cpp : implementation file
//

#include "stdafx.h"
#include "kkm.h"
#include "title.h"

extern int indW;

#ifdef _DEBUG
#undef THIS_FILE
static char BASED_CODE THIS_FILE[] = __FILE__;
#endif

#define YL(z_) (tm.tmHeight+z_)
#define XL(z_) ((z_)*tm.tmAveCharWidth)

/////////////////////////////////////////////////////////////////////////////
// CTitleNal

IMPLEMENT_DYNCREATE(CTitleNal, CView)

CTitleNal::CTitleNal() { }
CTitleNal::~CTitleNal() { }
void CTitleNal::OnUpdate(CView*, LPARAM lHint, CObject* pHint){}

BEGIN_MESSAGE_MAP(CTitleNal, CView)
    //{{AFX_MSG_MAP(CTitleNal)
    //}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTitleNal drawing
void CTitleNal::OnDraw(CDC* pDC)
{
 CBrush brushBackground;
 CPen penL,*penOld;
 
 COLORREF crOldBackground, crNewBackG = RGB(192,192,192);
 COLORREF crOldText, crNewText = RGB(128,0,0);
 COLORREF crNewPen = RGB(255,255,255);
 TEXTMETRIC tm;
 CString num;
 
 if (!pDC->IsPrinting()) {
   penL.CreatePen(PS_SOLID,1,crNewPen);
   brushBackground.CreateSolidBrush(crNewBackG);
   crOldBackground = pDC->SetBkColor(crNewBackG);
   penOld = pDC->SelectObject(&penL );
   pDC->GetTextMetrics(&tm);
   CRect rectClient;
   GetClientRect(&rectClient);  pDC->DPtoLP(&rectClient);
   (rectClient.top)++; pDC->FillRect(&rectClient, &brushBackground);

// line 1
  crOldText = pDC->SetTextColor(crNewText);
  num.LoadString(IDS_NALEZNOSCI);  pDC->TextOut(XL(GROSZY_NAL_COL-3), 2, num);
  pDC->SetTextColor(crOldText);
  
  pDC->MoveTo(0, YL(3));  pDC->LineTo(rectClient.right, YL(3));
  pDC->MoveTo(XL(NALNO_COL+NALNO_LEN+1), YL(3));
  pDC->LineTo(XL(NALNO_COL+NALNO_LEN+1),rectClient.bottom);
  pDC->MoveTo(XL(DATE_NAL_COL+DATE_NAL_LEN+1), YL(3));
  pDC->LineTo(XL(DATE_NAL_COL+DATE_NAL_LEN+1),rectClient.bottom);
  pDC->MoveTo(XL(GROSZY_NAL_COL+GROSZY_NAL_LEN+1), YL(3));
  pDC->LineTo(XL(GROSZY_NAL_COL+GROSZY_NAL_LEN+1),rectClient.bottom);
  pDC->SelectObject((CPen*)penOld);  penL.DeleteObject();
  
  pDC->MoveTo(0, YL(2));  pDC->LineTo(rectClient.right, YL(2));
  pDC->MoveTo(XL(NALNO_COL+NALNO_LEN+1)+2, YL(2));
  pDC->LineTo(XL(NALNO_COL+NALNO_LEN+1)+2,rectClient.bottom);
  pDC->MoveTo(XL(DATE_NAL_COL+DATE_NAL_LEN+1)+2, YL(2));
  pDC->LineTo(XL(DATE_NAL_COL+DATE_NAL_LEN+1)+2,rectClient.bottom);
  pDC->MoveTo(XL(GROSZY_NAL_COL+GROSZY_NAL_LEN+1)+2, YL(2));
  pDC->LineTo(XL(GROSZY_NAL_COL+GROSZY_NAL_LEN+1)+2,rectClient.bottom);

// line 2  
  num.LoadString(IDS_NUMER);     pDC->TextOut(XL(NALNO_COL), YL(4), num);  
  num.LoadString(IDS_DATA);      pDC->TextOut(XL(DATE_NAL_COL+2),YL(4), num);  
  num.LoadString(IDS_KWOTA);     pDC->TextOut(XL(GROSZY_NAL_COL+4),YL(4), num);  
  num.LoadString(IDS_TYP_NALEZ); pDC->TextOut(XL(TYP_NAL_COL+1),YL(4), num);  
  pDC->SetBkColor(crOldBackground);
 }
}

/////////////////////////////////////////////////////////////////////////////
// CTitleWpl

IMPLEMENT_DYNCREATE(CTitleWpl, CView)

CTitleWpl::CTitleWpl() { }
CTitleWpl::~CTitleWpl() { }
void CTitleWpl::OnUpdate(CView*, LPARAM lHint, CObject* pHint){}

BEGIN_MESSAGE_MAP(CTitleWpl, CView)
    //{{AFX_MSG_MAP(CTitleWpl)
    //}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTitleWpl drawing
void CTitleWpl::OnDraw(CDC* pDC)
{
 CBrush brushBackground;
 CPen penL,*penOld;
 
 COLORREF crOldBackground, crNewBackG = RGB(192,192,192);
 COLORREF crOldText, crNewText = RGB(128,0,0);
 COLORREF crNewPen = RGB(255,255,255);
 TEXTMETRIC tm;
 CString num;
 
 if (!pDC->IsPrinting()) {
   penL.CreatePen(PS_SOLID,1,crNewPen);
   brushBackground.CreateSolidBrush(crNewBackG);
   crOldBackground = pDC->SetBkColor(crNewBackG);
   penOld = pDC->SelectObject(&penL );
   pDC->GetTextMetrics(&tm);
   CRect rectClient;
   GetClientRect(&rectClient);  pDC->DPtoLP(&rectClient);
   (rectClient.top)++; pDC->FillRect(&rectClient, &brushBackground);

// line 1
  crOldText = pDC->SetTextColor(crNewText);
  num.LoadString(IDS_WPLATY);  pDC->TextOut(XL(GROSZY_COL-3), 2, num);
  pDC->SetTextColor(crOldText);
  
  pDC->MoveTo(0, YL(3));  pDC->LineTo(rectClient.right, YL(3));
  pDC->MoveTo(XL(WPLNO_COL+WPLNO_LEN+1)-4, YL(3));
  pDC->LineTo(XL(WPLNO_COL+WPLNO_LEN+1)-4,rectClient.bottom);
  pDC->MoveTo(XL(DATE_COL+DATE_LEN+1), YL(3));
  pDC->LineTo(XL(DATE_COL+DATE_LEN+1),rectClient.bottom);
//  if (!theApp.m_nTyp) {
  pDC->MoveTo(XL(GROSZY_COL+GROSZY_LEN+1)+4, YL(3));
  pDC->LineTo(XL(GROSZY_COL+GROSZY_LEN+1)+4,rectClient.bottom);
//  }  
  pDC->MoveTo(XL(GROSZY_COL+GROSZY_LEN+11)+4, YL(3));  //SQL
  pDC->LineTo(XL(GROSZY_COL+GROSZY_LEN+11)+4,rectClient.bottom);

  pDC->SelectObject((CPen*)penOld);  penL.DeleteObject();
  
  pDC->MoveTo(0, YL(2));  pDC->LineTo(rectClient.right, YL(2));
  pDC->MoveTo(XL(WPLNO_COL+WPLNO_LEN+1)-2, YL(2));
  pDC->LineTo(XL(WPLNO_COL+WPLNO_LEN+1)-2,rectClient.bottom);
  pDC->MoveTo(XL(DATE_COL+DATE_LEN+1)+2, YL(2));
  pDC->LineTo(XL(DATE_COL+DATE_LEN+1)+2,rectClient.bottom);
  pDC->MoveTo(XL(GROSZY_COL+GROSZY_LEN+1)+6, YL(2));
  pDC->LineTo(XL(GROSZY_COL+GROSZY_LEN+1)+6,rectClient.bottom);
  pDC->MoveTo(XL(GROSZY_COL+GROSZY_LEN+11)+6, YL(2));  //SQL
  pDC->LineTo(XL(GROSZY_COL+GROSZY_LEN+11)+6,rectClient.bottom);

// line 2  
  num.LoadString(IDS_NUMER);     pDC->TextOut(XL(WPLNO_COL), YL(4), num);  
  num.LoadString(IDS_DATA);      pDC->TextOut(XL(DATE_COL+1)+4,YL(4), num);  
  num.LoadString(IDS_KWOTA);     pDC->TextOut(XL(GROSZY_COL+4),YL(4), num);  
  num.LoadString(IDS_SPOSOB); pDC->TextOut(XL(TYP_COL-1-indW*2),YL(4), num);  
  num.LoadString(IDS_KSP); pDC->TextOut(XL(TYP_COL+10-indW*2),YL(4), num);  //SQL
  pDC->SetBkColor(crOldBackground);
 }
}

/////////////////////////////////////////////////////////////////////////////
// CTitleOkr

IMPLEMENT_DYNCREATE(CTitleOkr, CView)

CTitleOkr::CTitleOkr() { }
CTitleOkr::~CTitleOkr() { }
void CTitleOkr::OnUpdate(CView*, LPARAM lHint, CObject* pHint){}

BEGIN_MESSAGE_MAP(CTitleOkr, CView)
    //{{AFX_MSG_MAP(CTitleOkr)
    //}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTitleOkr drawing
void CTitleOkr::OnDraw(CDC* pDC)
{
 CBrush brushBackground;
 CPen penL,*penOld;
 
 COLORREF crOldBackground, crNewBackG = RGB(192,192,192);
 COLORREF crOldText, crNewText = RGB(128,0,0);
 COLORREF crNewPen = RGB(255,255,255);
 TEXTMETRIC tm;
 CString num;
 
 if (!pDC->IsPrinting()) {
   penL.CreatePen(PS_SOLID,1,crNewPen);
   brushBackground.CreateSolidBrush(crNewBackG);
   crOldBackground = pDC->SetBkColor(crNewBackG);
   penOld = pDC->SelectObject(&penL );
   pDC->GetTextMetrics(&tm);
   CRect rectClient;
   GetClientRect(&rectClient);  pDC->DPtoLP(&rectClient);
   (rectClient.top)++; pDC->FillRect(&rectClient, &brushBackground);

// line 1
  crOldText = pDC->SetTextColor(crNewText);
  num.LoadString(IDS_OKRODS);  pDC->TextOut(XL(DATE_K_COL-2), 2, num);
  pDC->SetTextColor(crOldText);
  
  pDC->MoveTo(0, YL(3));  pDC->LineTo(rectClient.right, YL(3));
  pDC->MoveTo(XL(OKRODNO_COL+OKRODNO_LEN)+1,YL(3));
  pDC->LineTo(XL(OKRODNO_COL+OKRODNO_LEN)+1,rectClient.bottom);
  pDC->MoveTo(XL(DATE_P_COL+DATE_P_LEN+1),YL(3));
  pDC->LineTo(XL(DATE_P_COL+DATE_P_LEN+1),rectClient.bottom);
  pDC->MoveTo(XL(DATE_K_COL+DATE_K_LEN+1),YL(3));
  pDC->LineTo(XL(DATE_K_COL+DATE_K_LEN+1),rectClient.bottom);
  pDC->MoveTo(XL(TYP_O_COL-1),YL(3));
  pDC->LineTo(XL(TYP_O_COL-1),rectClient.bottom);
  pDC->SelectObject((CPen*)penOld);  penL.DeleteObject();
  
  pDC->MoveTo(0, YL(2));  pDC->LineTo(rectClient.right, YL(2));
  pDC->MoveTo(XL(OKRODNO_COL+OKRODNO_LEN)+3,YL(2));
  pDC->LineTo(XL(OKRODNO_COL+OKRODNO_LEN)+3,rectClient.bottom);
  pDC->MoveTo(XL(DATE_P_COL+DATE_P_LEN+1)+2,YL(2));
  pDC->LineTo(XL(DATE_P_COL+DATE_P_LEN+1)+2,rectClient.bottom);
  pDC->MoveTo(XL(DATE_K_COL+DATE_K_LEN+1)+2,YL(2));
  pDC->LineTo(XL(DATE_K_COL+DATE_K_LEN+1)+2,rectClient.bottom);
  pDC->MoveTo(XL(TYP_O_COL-1)+2,YL(2));
  pDC->LineTo(XL(TYP_O_COL-1)+2,rectClient.bottom);
// line 2  
  num.LoadString(IDS_NUMER);     pDC->TextOut(4, YL(4), num);  
  num.LoadString(IDS_DATAPOCZ);  pDC->TextOut(XL(DATE_P_COL+1),YL(4), num);  
  num.LoadString(IDS_DATAKONC);  pDC->TextOut(XL(DATE_K_COL+1),YL(4), num);  
  num.LoadString(IDS_STOPA_);    pDC->TextOut(XL(STOPA_COL),YL(4), num);  
  num.LoadString(IDS_ODSETKI);   pDC->TextOut(XL(TYP_O_COL+2),YL(4), num);  
  pDC->SetBkColor(crOldBackground);
 }
}

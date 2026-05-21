// odsvw.cpp : implementation of the COView class
// 96.06.10 09:01

#include "stdafx.h"
#include "kkm.h"
#include "zlgr.h"
#include "mainfrm.h"
#include "dlg.h"

#include <stdlib.h>

#ifdef _DEBUG
#undef THIS_FILE
static char BASED_CODE THIS_FILE[] = __FILE__;
#endif

IMPLEMENT_DYNCREATE(COView, CRowView)

BEGIN_MESSAGE_MAP(COView,CRowView)
    //{{AFX_MSG_MAP(COView)
    ON_WM_KEYDOWN()
    ON_WM_LBUTTONDBLCLK()
    ON_WM_LBUTTONDOWN()
    ON_UPDATE_COMMAND_UI(ID_EDIT_ELEM, OnUpdateEditElem)
    ON_COMMAND(ID_EDIT_ELEM, OnEditElem)
    ON_WM_RBUTTONDBLCLK()
    //}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// COView

COView::COView() { }

//------------------------------------------------------------
// Overrides of CView and CRowView
//------------------------------------------------------------ 

void COView::OnUpdateEditElem(CCmdUI* pCmdUI)
{
 CKmDoc* pDoc = GetDocument();
 if (theApp.m_nAct==4)  pCmdUI->Enable((int)pDoc->m_odsActive);
 else  pCmdUI->Enable(FALSE);
}
//-----------------------------------------------------------
void COView::OnEditElem()
{ if (theApp.m_nAct==4) {
    CDlgOkr DOkr; DOkr.m_pDoc = GetDocument();
    if (DOkr.m_pDoc->GetOkrCount(DOkr.m_pDoc->m_nalActive)) DOkr.DoModal();
  }  
}

//-----------------------------------------------------------
void COView::OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags)
{ switch (nChar)  {
  case VK_TAB: {
    CKmDoc* pDoc = GetDocument();
    CMainFrame* pFrameWnd = (CMainFrame*)theApp.m_pMainWnd;
    pFrameWnd-> CreateOrActivateFrame(theApp.m_pListWplTemplate, RUNTIME_CLASS(CWView));
    return;
  }  

  case VK_INSERT: GetDocument()->DNewOkrOd(); return;
  case VK_DELETE: GetDocument()->DelOkrOd(); return;
  case VK_RETURN: 
    { CDlgOkr DOkr;
      DOkr.m_pDoc = GetDocument();
      if (DOkr.m_pDoc->GetOkrCount(DOkr.m_pDoc->m_nalActive)) DOkr.DoModal();
      return;
    }  
  default:  CRowView::OnKeyDown(nChar, nRepCnt, nFlags);
  }
}

//------------------------------------------------------------
void COView::OnLButtonDblClk(UINT nFlags, CPoint point)
{ int h;
  CRowView::OnLButtonDown( nFlags, point);
  CDlgOkr DOkr; DOkr.m_pDoc = GetDocument();
  h = DOkr.m_pDoc->GetOkrCount(DOkr.m_pDoc->m_nalActive)*m_nRowHeight;
  if (point.y<h) DOkr.DoModal();
  else DOkr.m_pDoc->DNewOkrOd();
}

//------------------------------------------------------------
void COView::OnRButtonDblClk(UINT nFlags, CPoint point)
{ CRowView::OnLButtonDown(nFlags, point); GetDocument()->DelOkrOd();}

//------------------------------------------------------------
void COView::OnLButtonDown(UINT nFlags, CPoint point)
{ CKmDoc* pDoc = GetDocument();  
 if (pDoc->GetOkrCount(pDoc->m_nalActive)) CRowView::OnLButtonDown( nFlags, point);
}

//------------------------------------------------------------
void COView::OnUpdate(CView*, LPARAM lHint, CObject* pHint)
{
 if (pHint != NULL && pHint->IsKindOf(RUNTIME_CLASS(COkrOd)))
   { int nRow = LOWORD(lHint)-1; UpdateRow(nRow); }
 else 
   if (theApp.m_nAct != 6) Invalidate(); 
   else if (LOWORD(lHint) == 55555) Invalidate();
}

//------------------------------------------------------------
void COView::OnActivateView(
    BOOL bActivate, CView* pActivateView, CView* pDeactiveView)
{CRowView::OnActivateView(bActivate, pActivateView, pDeactiveView);
  if (bActivate) {
    if (theApp.m_nAct==7) {
      CMainFrame* pFrameWnd = (CMainFrame*)theApp.m_pMainWnd;
      pFrameWnd->ViewIn();  
    }   
    theApp.m_nAct = 4; 
  }  
  else theApp.m_nAct = 0; 
  Invalidate(); 
}
//------------------------------------------------------------
void COView::GetRowWidthHeight(CDC* pDC, int& nRowWidth, int& nRowHeight)
{  TEXTMETRIC tm;
 pDC->GetTextMetrics(&tm);
 nRowWidth = tm.tmAveCharWidth * ROW_O_WIDTH;
 nRowHeight = tm.tmHeight;
}
//------------------------------------------------------------
int COView::GetActiveRow() { return (GetDocument()->GetActiveOkrOdNo()-1); }
//------------------------------------------------------------
int COView::GetRowCount()
{ CKmDoc* pDoc = GetDocument();
  CNal* pNal;
  POSITION pos;
  for ( pos = pDoc->GetFirstNalPos(); pos != NULL; pDoc->GetNextNal(pos)) {
    pNal = (CNal*)pDoc->m_nalList.GetAt(pos);
    if (pNal->m_nalNum == pDoc->m_nalActive) { return pNal->m_nalOkrCount; }
  }
  return 0;
}

//------------------------------------------------------------
void COView::ChangeSelectionNextRow(BOOL bNext)
             { GetDocument()->ChangeSelectionNextOkrOdNo(bNext); }
//------------------------------------------------------------
void COView::ChangeSelectionToRow(int nRow)
             { GetDocument()->ChangeSelectionToOkrOdNo(nRow + 1); }

//------------------------------------------------------------
BOOL COView::Maybe() { return (theApp.m_nAct ==4); }

//------------------------------------------------------------
void COView::OnDrawRow(CDC* pDC, int nRow, int y, BOOL bSelected)
{
// Prepare for highlighting or un-highlighting the nal, depending
// on whether it is the currently selected nal or not.  And paint
// the background (behind the text) accordingly.
 CBrush brushBackground;
    // save colors for drawing selected item on the screen
 COLORREF crNewBackG;
 COLORREF crNewText;

 COLORREF crOldText = 0;
 COLORREF crOldBackground = 0;

 if (nRow<0) ::MessageBeep(MB_ICONHAND);

 if (!pDC->IsPrinting()) {
   if (bSelected) {
     if (theApp.m_nAct !=4) {
       crNewBackG = ::GetSysColor(COLOR_WINDOW);
       crNewText = ::GetSysColor(COLOR_HIGHLIGHT);
     }
     else {
       crNewBackG = ::GetSysColor(COLOR_HIGHLIGHT);
       crNewText=::GetSysColor(COLOR_HIGHLIGHTTEXT);
     }
   }
   else {
     crNewBackG =::GetSysColor(COLOR_WINDOW);
     crNewText = ::GetSysColor(COLOR_WINDOWTEXT);
   }
   brushBackground.CreateSolidBrush(crNewBackG);
   crOldBackground = pDC->SetBkColor(crNewBackG);
   crOldText = pDC->SetTextColor(crNewText);
   CRect rectSelection;
   pDC->GetClipBox(&rectSelection);
   rectSelection.top = y;
   rectSelection.bottom = y + m_nRowHeight;
   pDC->FillRect(&rectSelection, &brushBackground);
 }

// Get the data for the specific nal.
  DWORD dwStopa, dwFactor;
  int nTypO;
  int nTypW;
  CKmDoc* pDoc = GetDocument();
  int nNalNo = pDoc->GetActiveNalNo();
  CString strDateP,strDateK,strTyp;
  int nOkrOdNo = nRow+1; //+ GetDocument()->GetFirstNalNo();
  pDoc->GetOkrOd(nOkrOdNo, nNalNo, dwStopa, dwFactor, nTypO, nTypW, strDateP, strDateK);

  TEXTMETRIC tm;
  pDC->GetTextMetrics(&tm);

  char num[10];
// line 1
  wsprintf(num, "%u", nOkrOdNo);
  pDC->TextOut(OKRODNO_COL*tm.tmAveCharWidth, y, num, strlen(num));

  pDC->TextOut(DATE_P_COL*tm.tmAveCharWidth, y, strDateP);
  pDC->TextOut(DATE_K_COL*tm.tmAveCharWidth, y, strDateK);
  CString strStopa;
  if (nTypO) { strStopa = GetStopaFormatted(dwStopa);
    // right justify the nal dollar/cent amount
    pDC->TextOut(
      (STOPA_COL+STOPA_LEN+1-strStopa.GetLength())*tm.tmAveCharWidth, y, strStopa);
    nTypW+= IDS_OWTYP0;
    strTyp.LoadString(nTypW);
    pDC->TextOut(TYP_O_COL*tm.tmAveCharWidth, y, strTyp);  
  }   
  else {
    strTyp.LoadString(IDS_OOTYP0);
    pDC->TextOut(TYP_O_COL*tm.tmAveCharWidth, y, strTyp);
  }

// Restore the DC.
   if (!pDC->IsPrinting()) {
     pDC->SetBkColor(crOldBackground);
     pDC->SetTextColor(crOldText);
   }
}


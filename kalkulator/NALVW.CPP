// nalvw.cpp : implementation of the CNView class
// 96.09.18 20:52

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

IMPLEMENT_DYNCREATE(CNView, CRowView)

BEGIN_MESSAGE_MAP(CNView,CRowView)
    //{{AFX_MSG_MAP(CNView)
    ON_WM_KEYDOWN()
    ON_WM_LBUTTONDBLCLK()
    ON_UPDATE_COMMAND_UI(ID_EDIT_ELEM, OnUpdateEditElem)
    ON_COMMAND(ID_EDIT_ELEM, OnEditElem)
    ON_WM_RBUTTONDBLCLK()
    //}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CNView

CNView::CNView() { }

//-----------------------------------------------------------
void CNView::OnUpdateEditElem(CCmdUI* pCmdUI)
{
 CKmDoc* pDoc = GetDocument();
 if (theApp.m_nAct==2)  pCmdUI->Enable((int)pDoc->m_nalActive);
 else  pCmdUI->Enable(FALSE);
}
//-----------------------------------------------------------
void CNView::OnEditElem()
{ if (theApp.m_nAct==2) { CDlgNal DNal; DNal.m_pDoc = GetDocument(); DNal.DoModal(); } }

//------------------------------------------------------------
// Overrides of CView and CRowView
//--------------------------------------------------------------
void CNView::OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags)
{ switch (nChar)  {
  case VK_TAB: {
    CKmDoc* pDoc = GetDocument();
    CMainFrame* pFrameWnd = (CMainFrame*)theApp.m_pMainWnd;
//    if (theApp.m_nTyp)
//      pFrameWnd-> CreateOrActivateFrame(theApp.m_pListWplTemplate, RUNTIME_CLASS(CWView));
//    else
    if (pDoc->CanOds())
      pFrameWnd-> CreateOrActivateFrame(theApp.m_pListOkrOdTemplate, RUNTIME_CLASS(COView));
    else
      pFrameWnd-> CreateOrActivateFrame(theApp.m_pListWplTemplate, RUNTIME_CLASS(CWView));
    return;
  }
  case VK_INSERT: GetDocument()->DNewNal(); return;
  case VK_DELETE: GetDocument()->DelNal(); return;
  case VK_RETURN: { CDlgNal DNal; DNal.m_pDoc = GetDocument(); DNal.DoModal(); return; }
  default:  CRowView::OnKeyDown(nChar, nRepCnt, nFlags);
  }
}

//------------------------------------------------------
void CNView::OnLButtonDblClk(UINT nFlags, CPoint point)
{ int h;
  CRowView::OnLButtonDown( nFlags, point);
  h =GetDocument()->m_nalList.GetCount()*m_nRowHeight;
  if (point.y>=h) GetDocument()->DNewNal();
  else { CDlgNal DNal; DNal.m_pDoc = GetDocument(); DNal.DoModal(); }
}

//--------------------------------------------------------------
void CNView::OnRButtonDblClk(UINT nFlags, CPoint point)
{ CRowView::OnLButtonDown(nFlags, point);  GetDocument()->DelNal(); }

//--------------------------------------------------------------
void CNView::OnUpdate(CView* Sender, LPARAM lHint, CObject* pHint)
{
 if (pHint != NULL && LOWORD(lHint))
   if (pHint->IsKindOf(RUNTIME_CLASS(CNal)))
     { int nRow = LOWORD(lHint)-1; UpdateRow(nRow); }
   else return;
 else
   if (theApp.m_nAct == 2 || theApp.m_nAct == 0) Invalidate();
   else if (LOWORD(lHint) == 55555) Invalidate();
}

//------------------------------------------------------------
void CNView::OnActivateView( BOOL bActivate, CView* pActivateView, CView* pDeactiveView)
{ CRowView::OnActivateView(bActivate, pActivateView, pDeactiveView);
  if (bActivate) {
    if (theApp.m_nAct==7) {
      CMainFrame* pFrameWnd = (CMainFrame*)theApp.m_pMainWnd;
      pFrameWnd->ViewIn();  
    }  
    theApp.m_nAct = 2; 
  } 
  else theApp.m_nAct = 0;

  //if (bActivate) theApp.m_nAct = 2; else theApp.m_nAct = 0;
  Invalidate();
}
//------------------------------------------------------------
void CNView::GetRowWidthHeight(CDC* pDC, int& nRowWidth, int& nRowHeight)
{  TEXTMETRIC tm;
 pDC->GetTextMetrics(&tm);
 nRowWidth = tm.tmAveCharWidth * (ROW_NAL_WIDTH /*+theApp.m_nTyp*7*/);
 nRowHeight = tm.tmHeight;
}

//------------------------------------------------------------
int CNView::GetActiveRow() { return (GetDocument()->GetActiveNalNo()-1); }
//------------------------------------------------------------
int CNView::GetRowCount() { return (GetDocument()->m_nalList.GetCount()); }
//------------------------------------------------------------
void CNView::ChangeSelectionNextRow(BOOL bNext)
             { GetDocument()->ChangeSelectionNextNalNo(bNext); }
//------------------------------------------------------------
void CNView::ChangeSelectionToRow(int nRow)
             { GetDocument()->ChangeSelectionToNalNo(nRow + 1); }
//------------------------------------------------------------
BOOL CNView::Maybe() { return (theApp.m_nAct ==2); }
//------------------------------------------------------------
void CNView::OnDrawRow(CDC* pDC, int nRow, int y, BOOL bSelected)
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

 if (!pDC->IsPrinting()) {
   if (bSelected) {
     if (theApp.m_nAct !=2) {
       crNewBackG =::GetSysColor(COLOR_WINDOW);
       crNewText = ::GetSysColor(COLOR_HIGHLIGHT);
     }
     else {
       crNewBackG = ::GetSysColor(COLOR_HIGHLIGHT);
       crNewText = ::GetSysColor(COLOR_HIGHLIGHTTEXT);
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
  DWORD dwGroszy;
  int nTypN;	//,ii
  CString strDate,strTyp;
  int nNalNo = nRow+1; //+ GetDocument()->GetFirstNalNo();
  CKmDoc* pDoc = GetDocument();
/*  ii = */ pDoc->GetNal(nNalNo, dwGroszy, nTypN, strDate);

  TEXTMETRIC tm;
  pDC->GetTextMetrics(&tm);

  char num[10];
// line 1
  wsprintf(num, "%u", nNalNo);
  pDC->TextOut(NALNO_COL*tm.tmAveCharWidth, y, num, strlen(num));

  pDC->TextOut(DATE_NAL_COL*tm.tmAveCharWidth, y, strDate);

  CString strDollarCents;
  strDollarCents = GetDCFormatted(dwGroszy);
// right justify the nal dollar/cent amount
  pDC->TextOut(
   (GROSZY_NAL_COL+GROSZY_NAL_LEN+1-strDollarCents.GetLength())*tm.tmAveCharWidth,
   y, strDollarCents);
  strTyp.LoadString(IDS_NTYP0+nTypN);
  pDC->TextOut(TYP_NAL_COL*tm.tmAveCharWidth, y, strTyp);
//  if (theApp.m_nTyp && nTypN==0) {
//    strTyp.LoadString(IDS_ODS_SKR_YES+ii);
//    pDC->TextOut((ROW_NAL_WIDTH+6)*tm.tmAveCharWidth, y, strTyp);
//  }
// Restore the DC.
   if (!pDC->IsPrinting()) {
     pDC->SetBkColor(crOldBackground);
     pDC->SetTextColor(crOldText);
   }
}


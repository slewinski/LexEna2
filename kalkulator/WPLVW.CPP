// wplvw.cpp : implementation of the CWView class
// 11-20-96 10:49pm

#include "stdafx.h"
#include "kkm.h"
#include "zlgr.h"
#include "mainfrm.h"
#include "dlg.h"

#include <stdlib.h>

extern int indW;

#ifdef _DEBUG
#undef THIS_FILE
static char BASED_CODE THIS_FILE[] = __FILE__;
#endif

IMPLEMENT_DYNCREATE(CWView, CRowView)

BEGIN_MESSAGE_MAP(CWView,CRowView)
    //{{AFX_MSG_MAP(CWView)
    ON_WM_KEYDOWN()
    ON_WM_LBUTTONDBLCLK()
    ON_UPDATE_COMMAND_UI(ID_EDIT_ELEM, OnUpdateEditElem)
    ON_COMMAND(ID_EDIT_ELEM, OnEditElem)
    ON_WM_RBUTTONDBLCLK()
    //}}AFX_MSG_MAP
END_MESSAGE_MAP()


/////////////////////////////////////////////////////////////////////////////
// CWView

CWView::CWView() { }

//-------------------------------------------------------------
// Overrides of CView and CRowView
//-------------------------------------------------------------
void CWView::OnUpdateEditElem(CCmdUI* pCmdUI)
{ CKmDoc* pDoc = GetDocument();
 if (theApp.m_nAct==6)  pCmdUI->Enable((int)pDoc->m_wplActive);
 else  pCmdUI->Enable(FALSE);
}

//-------------------------------------------------------------
void CWView::OnEditElem()
{ if (theApp.m_nAct==6) { CDlgWpl DWpl; DWpl.m_pDoc = GetDocument(); DWpl.DoModal(); } }

//-------------------------------------------------------------
void CWView::OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags)
{ switch (nChar)  {
  case VK_TAB: {
    CKmDoc* pDoc = GetDocument();
    CMainFrame* pFrameWnd = (CMainFrame*)theApp.m_pMainWnd;
    pFrameWnd-> CreateOrActivateFrame(theApp.m_pListNalTemplate, RUNTIME_CLASS(CNView));
    return;
  }
  case VK_INSERT: GetDocument()->DNewWpl(); return;
  case VK_DELETE: GetDocument()->DelWpl(); return;
  case VK_RETURN: { CDlgWpl DWpl; DWpl.m_pDoc = GetDocument(); DWpl.DoModal(); return; }
  default:  CRowView::OnKeyDown(nChar, nRepCnt, nFlags);
  }
}

//----------------------------------------------------------
void CWView::OnLButtonDblClk(UINT nFlags, CPoint point)
{ int h;
  CRowView::OnLButtonDown( nFlags, point);
  h = GetDocument()->m_wplList.GetCount()*m_nRowHeight;
  if (point.y>=h) GetDocument()->DNewWpl();
  else { CDlgWpl DWpl; DWpl.m_pDoc = GetDocument(); DWpl.DoModal(); }
}

//----------------------------------------------------------
void CWView::OnRButtonDblClk(UINT nFlags, CPoint point)
{ CRowView::OnLButtonDown(nFlags, point); GetDocument()-> DelWpl(); }

//----------------------------------------------------------
void CWView::OnUpdate(CView* Sender, LPARAM lHint, CObject* pHint)
{
 if (pHint != NULL && LOWORD(lHint) && pHint->IsKindOf(RUNTIME_CLASS(CWpl)))
   { int nRow = LOWORD(lHint)-1; UpdateRow(nRow); }
 else {
   if (theApp.m_nAct == 6 || theApp.m_nAct == 0) {
     if (LOWORD(lHint)>0 && LOWORD(lHint)!= 55555)
       ChangeSelectionToRow(LOWORD(lHint));
     Invalidate();
   }
   else if (LOWORD(lHint) == 55555) Invalidate();
 }
}

//------------------------------------------------------------
void CWView::OnActivateView(BOOL bActivate, CView* pActivateView, CView* pDeactiveView)
{CRowView::OnActivateView(bActivate, pActivateView, pDeactiveView);
  if (bActivate) {
    if (theApp.m_nAct==7) {
      CMainFrame* pFrameWnd = (CMainFrame*)theApp.m_pMainWnd;
      pFrameWnd->ViewIn();
    }
  theApp.m_nAct = 6;
  }
  else theApp.m_nAct = 0;
  Invalidate();
}

//------------------------------------------------------------
void CWView::GetRowWidthHeight(CDC* pDC, int& nRowWidth, int& nRowHeight)
{  TEXTMETRIC tm;
 pDC->GetTextMetrics(&tm);
 nRowWidth = tm.tmAveCharWidth * (ROW_WIDTH/*-theApp.m_nTyp*7*/); // Brak sposobu
 nRowHeight = tm.tmHeight; // 2 lines of text
}

//------------------------------------------------------------
int CWView::GetActiveRow()
{ CKmDoc* pDoc = GetDocument(); return (pDoc->GetActiveWplNo()-1); }

//------------------------------------------------------------
int CWView::GetRowCount()
{ CKmDoc* pDoc = GetDocument(); return (pDoc->m_wplList.GetCount()); }

//------------------------------------------------------------
void CWView::ChangeSelectionNextRow(BOOL bNext)
{ CKmDoc* pDoc = GetDocument(); pDoc->ChangeSelectionNextWplNo(bNext); }

//------------------------------------------------------------
void CWView::ChangeSelectionToRow(int nRow)
{ CKmDoc* pDoc = GetDocument(); pDoc->ChangeSelectionToWplNo(nRow + 1); }

//------------------------------------------------------------
BOOL CWView::Maybe() { return (theApp.m_nAct ==6); }
//------------------------------------------------------------
void CWView::OnDrawRow(CDC* pDC, int nRow, int y, BOOL bSelected)
{
// Prepare for highlighting or un-highlighting the wpl, depending
// on whether it is the currently selected wpl or not.  And paint
// the background (behind the text) accordingly.
 CBrush brushBackground;
    // save colors for drawing selected item on the screen

 COLORREF crNewBackG;
 COLORREF crNewText;
 COLORREF crOldText = 0;
 COLORREF crOldBackground = 0;

 if (!pDC->IsPrinting()) {
   if (bSelected) {
     if (theApp.m_nAct !=6) {
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

// Get the data for the specific wpl.
  DWORD dwGroszy;
  int nTypW, nSQLTyp;
  CString strDate,strTyp;
  char strSQL[14];

  int nWplNo = nRow+1; //+ GetDocument()->GetFirstWplNo();
  CKmDoc* pDoc = GetDocument();

  pDoc->GetWpl(nWplNo, dwGroszy, nTypW, strDate, nSQLTyp);

  TEXTMETRIC tm;
  pDC->GetTextMetrics(&tm);

  char num[10];
// line 1
  wsprintf(num, "%u", nWplNo);
  pDC->TextOut(WPLNO_COL*tm.tmAveCharWidth, y, num, strlen(num));

  pDC->TextOut(DATE_COL*tm.tmAveCharWidth, y, strDate);

  CString strDollarCents;
  strDollarCents = GetDCFormatted(dwGroszy);
// right justify the wpl dollar/cent amount
  pDC->TextOut(
   (GROSZY_COL + GROSZY_LEN +2 - strDollarCents.GetLength()) * tm.tmAveCharWidth,
   y, strDollarCents);
   nTypW+= IDS_WTYP0;
   strTyp.LoadString(nTypW);
   pDC->TextOut((TYP_COL-indW)*tm.tmAveCharWidth, y, strTyp);
   
	switch(nSQLTyp) 
	{
		case 'w': strcpy(strSQL,"wp³ata");
				break;
		case 'W': strcpy(strSQL,"wp³ata bezp."); 
				break;
	    case 'd': strcpy(strSQL,"depozyt"); 
				break;
		case 's': strcpy(strSQL,"saldo zal."); 
				break;
		default: strcpy(strSQL," ");
				break;
	}
	pDC->TextOut((TYP_COL-indW+10)*tm.tmAveCharWidth, y, strSQL); //hejho
   
   // Restore the DC.
   if (!pDC->IsPrinting()) {
     pDC->SetBkColor(crOldBackground);
     pDC->SetTextColor(crOldText);
   }
}

/////////////////////////////////////////////////////////////////////////////
// CWView


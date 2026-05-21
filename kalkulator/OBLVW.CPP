// oblvw.cpp : implementation of the COblView class
// 96.06.10 09:02

#include "stdafx.h"
#include "kkm.h"
#include "zlgr.h"
#include "mainfrm.h"

#include <stdlib.h>

#ifdef _DEBUG
#undef THIS_FILE
static char BASED_CODE THIS_FILE[] = __FILE__;
#endif

LOGFONT lf;

IMPLEMENT_DYNCREATE(COblView, CRowView)

/////////////////////////////////////////////////////////////////////////////
// COblView
COblView::COblView()
{
 lf.lfHeight = -14;        lf.lfWidth = 0;
 lf.lfEscapement = 0;      lf.lfOrientation = 0;
 lf.lfWeight = 400;        lf.lfItalic = 0;
 lf.lfUnderline = 0;       lf.lfStrikeOut = 0;
 lf.lfCharSet = DEFAULT_CHARSET; lf.lfOutPrecision = 3;
 lf.lfClipPrecision = 2;   lf.lfQuality = 1;
 lf.lfPitchAndFamily = 31; lstrcpy(lf.lfFaceName,"Courier New");    //  CE
}

BEGIN_MESSAGE_MAP(COblView, CRowView)
    //{{AFX_MSG_MAP(COblView)
    ON_WM_KEYDOWN()
    //}}AFX_MSG_MAP
END_MESSAGE_MAP()


//------------------------------------------------------------
// Overrides of CView and CRowView
void COblView::OnUpdate(CView*, LPARAM lHint, CObject* pHint) { Invalidate(); }
//------------------------------------------------------------
void COblView::OnActivateView(
    BOOL bActivate, CView* pActivateView, CView* pDeactiveView)
{ CView::OnActivateView(bActivate, pActivateView, pDeactiveView);
   if (bActivate && theApp.m_nAct != 7)
    { CMainFrame* pFrameWnd = (CMainFrame*)theApp.m_pMainWnd;
      pFrameWnd->ViewKalk();
      theApp.m_nAct = 7;
      CRowView::UpdateScrollSizes();
    }
  //if (bActivate && theApp.m_nAct != 7)
    //{ theApp.m_nAct = 7; CRowView::UpdateScrollSizes();}
}

//------------------------------------------------------------
void COblView::GetRowWidthHeight(CDC* pDC, int& nRowWidth, int& nRowHeight)
{ TEXTMETRIC tm;
  CFont FontNew,*FontOld;

 pDC->GetTextMetrics(&tm);
 lf.lfHeight = -14;
 FontNew.CreateFontIndirect(&lf);
 FontOld = pDC->SelectObject((CFont*)&FontNew);
 if (pDC->IsPrinting()) {
   lf.lfHeight=tm.tmHeight-2;
   pDC->SelectObject((CFont*)FontOld);
   FontNew.DeleteObject();
   FontNew.CreateFontIndirect(&lf);
   FontOld = pDC->SelectObject((CFont*)&FontNew);
   pDC->GetTextMetrics(&tm);
 }
 nRowWidth = tm.tmAveCharWidth * 80; /* ROW_WIDTH; */
 nRowHeight = tm.tmHeight; // 2 lines of text
 pDC->SelectObject((CFont*)FontOld);
 FontNew.DeleteObject();
}

//------------------------------------------------------------
int COblView::GetActiveRow()
{ CKmDoc* pDoc = GetDocument(); return (pDoc->m_oblActive); }
//------------------------------------------------------------
int COblView::GetRowCount()
{ CKmDoc* pDoc = GetDocument(); return (pDoc->m_Obl.GetSize()); }
//------------------------------------------------------------
void COblView::ChangeSelectionNextRow(BOOL bNext){}
//------------------------------------------------------------
void COblView::ChangeSelectionToRow(int nRow){ }
//------------------------------------------------------------
void COblView::OnDrawRow(CDC* pDC, int nRow, int y, BOOL bSelected)
{ CBrush brushBackground;
  COLORREF crNewBackG, crNewText, crOldText = 0, crOldBackground = 0;
  CFont FontNew,*FontOld;
  TEXTMETRIC tm;
 pDC->GetTextMetrics(&tm);

 lf.lfHeight = -14;
 FontNew.CreateFontIndirect(&lf);
 FontOld = pDC->SelectObject((CFont*)&FontNew);
  if (!pDC->IsPrinting()) {
   crNewBackG =::GetSysColor(COLOR_WINDOW);
   crNewText = ::GetSysColor(COLOR_WINDOWTEXT);
   brushBackground.CreateSolidBrush(crNewBackG);
   crOldBackground = pDC->SetBkColor(crNewBackG);
   crOldText = pDC->SetTextColor(crNewText);
   CRect rectSelection;
   pDC->GetClipBox(&rectSelection);
   rectSelection.top = y;
   rectSelection.bottom = y + m_nRowHeight;
   pDC->FillRect(&rectSelection, &brushBackground);
 }
 else {
   lf.lfHeight=tm.tmHeight-2;
   pDC->SelectObject((CFont*)FontOld);
   FontNew.DeleteObject();
   FontNew.CreateFontIndirect(&lf);
   FontOld = pDC->SelectObject((CFont*)&FontNew);
   pDC->GetTextMetrics(&tm);
 }

// Get the data for the specific wpl.
  CKmDoc* pDoc = GetDocument();
  CString strD;
  char strD1[120];
  int Ctrl,Pos;
  CSize  cs;

  strD = pDoc->m_Obl.GetAt(nRow);
  Pos=40;
next:
  Ctrl = strD.Find('^');

  if (Ctrl<0)    pDC->TextOut(Pos, y, strD);
  else  {
    //if (strD[Ctrl+1]=='B')
    strcpy(strD1,(const char *)strD);
    pDC->TextOut(Pos, y, strD1,Ctrl);
    if (!pDC->IsPrinting()) {
      cs  = pDC->GetTextExtent(strD1,Ctrl);
      Pos+=cs.cx+1;
    }
    else Pos+=tm.tmAveCharWidth*Ctrl;
    if (strD[Ctrl+1]=='B') {
      pDC->SelectObject((CFont*)FontOld);
      FontNew.DeleteObject();
      if (lf.lfWeight==400) lf.lfWeight=700;
      else                  lf.lfWeight=400;
      FontNew.CreateFontIndirect(&lf);
      FontOld = pDC->SelectObject((CFont*)&FontNew);
      lstrcpy(strD1,strD1+Ctrl+2);
    }
    else lstrcpy(strD1,strD1+Ctrl+1);
    strD = strD1;
    goto next;
  }
  if (!pDC->IsPrinting())
    { pDC->SetBkColor(crOldBackground); pDC->SetTextColor(crOldText); }
 pDC->SelectObject((CFont*)FontOld);
 FontNew.DeleteObject();
}

//--------------------------------------------------------------
void COblView::RectLPtoRowRange(const CRect& rect, int& nFirstRow,
                               int& nLastRow, BOOL bIncludePartiallyShownRows)
{ int nRounding = bIncludePartiallyShownRows? 0 : (m_nRowHeight - 1);
  nFirstRow = m_nPr? m_nActPage*8*m_nRowHeight : 0;
  nFirstRow = (rect.top + nRounding-nFirstRow) / m_nRowHeight;
  nLastRow = m_nPr? (m_nActPage*8+9)*m_nRowHeight : 0;
  nLastRow = min((rect.bottom - nRounding - nLastRow)/m_nRowHeight, GetRowCount()-1);
}

//--------------------------------------------------------------
void COblView::OnDraw(CDC* pDC)
{ if (GetRowCount() == 0) return;
  int nFirstRow, nLastRow;
  CRect rectClip;
  pDC->GetClipBox(&rectClip); // Get the invalidated region.
  RectLPtoRowRange(rectClip, nFirstRow, nLastRow, TRUE);
  int nActiveRow = GetActiveRow();
  int nRow, y;
  int nLastViewableRow = LastViewableRow();
  int nDod = m_nPr?m_nActPage*8+4 : 0;
  for (nRow = nFirstRow, y = m_nRowHeight*(nFirstRow+nDod); nRow <= nLastRow;
                                                            nRow++, y += m_nRowHeight)
    { if (nRow > nLastViewableRow)  {
        CString strWarning; strWarning.LoadString(IDS_TOO_MANY_ROWS);
        pDC->TextOut(0, y, strWarning); break;
      }
      OnDrawRow(pDC, nRow, y, nRow == nActiveRow);
    }
}

//--------------------------------------------------------------
void COblView::OnPrepareDC(CDC* pDC, CPrintInfo* pInfo)
{ CScrollView::OnPrepareDC(pDC, pInfo);
  CalculateRowMetrics(pDC);  m_nPr = pDC->IsPrinting();  m_nActPage = 0;
}
//--------------------------------------------------------------
BOOL COblView::OnPreparePrinting(CPrintInfo* pInfo) { return DoPreparePrinting(pInfo); }
//--------------------------------------------------------------
void COblView::OnBeginPrinting(CDC* pDC, CPrintInfo* pInfo)
{ int nPageHeight = pDC->GetDeviceCaps(VERTRES);
  CalculateRowMetrics(pDC);
  m_nRowsPerPrintedPage = nPageHeight / m_nRowHeight;
  m_nRowsFact = m_nRowsPerPrintedPage-8;
  int nPrintableRowCount = LastViewableRow() + 1;
  if (GetRowCount() < nPrintableRowCount) nPrintableRowCount = GetRowCount();
  pInfo->SetMaxPage((nPrintableRowCount + m_nRowsFact - 1) / m_nRowsFact);
  pInfo->m_nCurPage = 1;  // start previewing at page# 1
}

//--------------------------------------------------------------
void COblView::OnPrint(CDC* pDC, CPrintInfo* pInfo) // Print current page
{ int yTopPage = (pInfo->m_nCurPage -1) * m_nRowsPerPrintedPage * m_nRowHeight;
  m_nActPage = (pInfo->m_nCurPage -1);
 pDC->SetViewportOrg(0, -yTopPage);
 int nPageWidth = pDC->GetDeviceCaps(HORZRES);
 CRect rectClip =
       CRect(0,yTopPage,nPageWidth,yTopPage+m_nRowsPerPrintedPage*m_nRowHeight);
 pDC->IntersectClipRect(&rectClip);
 OnDraw(pDC);
}

//--------------------------------------------------------------
void COblView::OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags)
{ switch (nChar)  {
  case VK_ESCAPE:
  case VK_TAB:       OnViewNal();  return;
  case VK_HOME:
    OnVScroll(SB_TOP, 0, GetScrollBarCtrl(SB_VERT));      break;
  case VK_END:
    OnVScroll(SB_BOTTOM, 0, GetScrollBarCtrl(SB_VERT));   break;
  case VK_UP:
    OnVScroll(SB_LINEUP, 0, GetScrollBarCtrl(SB_VERT));   break;
  case VK_DOWN:
    OnVScroll(SB_LINEDOWN, 0, GetScrollBarCtrl(SB_VERT)); break;
  default:           CRowView::OnKeyDown(nChar, nRepCnt, nFlags);
  }
}

//--------------------------------------------------------------
void COblView::OnViewNal()
{ CMainFrame* pFrameWnd = (CMainFrame*)theApp.m_pMainWnd;
  pFrameWnd-> CreateOrActivateFrame(theApp.m_pListNalTemplate, RUNTIME_CLASS(CNView));
}


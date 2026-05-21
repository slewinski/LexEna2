// rowview.cpp : implementation of the CRowView class
// 96.06.10 08:35
//--------------------------------------------------------------

#include "stdafx.h"
#include "afxext.h"
#include "afxpriv.h"

#include "kkm.h"
#include "cpv.h"

#include <stdlib.h>
#include <limits.h> // for INT_MAX

#ifdef _DEBUG
#undef THIS_FILE
static char BASED_CODE THIS_FILE[] = __FILE__;
#endif
extern int FlNal,FlOkr,Flwpl;
IMPLEMENT_DYNAMIC(CRowView, CScrollView)
int FlLB=0;
//--------------------------------------------------------------
// CRowView ----------------------------------------------------
//--------------------------------------------------------------
BEGIN_MESSAGE_MAP(CRowView, CScrollView)
    //{{AFX_MSG_MAP(CRowView)
    ON_WM_KEYDOWN()
    ON_WM_SIZE()
    ON_WM_LBUTTONDOWN()
    ON_UPDATE_COMMAND_UI(ID_FILE_NEW, OnUpdateFileNew)
    ON_UPDATE_COMMAND_UI(ID_FILE_OPEN, OnUpdateFileOpen)
    ON_COMMAND(ID_FILE_PRINT_PREVIEW, OnFileEditPreview)
    ON_UPDATE_COMMAND_UI(ID_FILE_PRINT_PREVIEW, OnUpdatePrV)
    ON_WM_LBUTTONUP()
    ON_WM_MOUSEMOVE()
    //}}AFX_MSG_MAP
    // Standard printing commands
    ON_COMMAND(ID_FILE_PRINT, CView::OnFilePrint)
    //ON_COMMAND(ID_FILE_PRINT_PREVIEW, CView::OnFilePrintPreview)
END_MESSAGE_MAP()

//--------------------------------------------------------------
// CRowView construction, initialization, and destruction ------
//--------------------------------------------------------------
CRowView::CRowView() { m_nPrevSelectedRow = 0; }
CRowView::~CRowView() { }

//--------------------------------------------------------------
// CRowView updating and drawing -------------------------------
//--------------------------------------------------------------
void CRowView::OnActivateView(
    BOOL bActivate, CView* pActivateView, CView* pDeactiveView)
{ CView::OnActivateView(bActivate, pActivateView, pDeactiveView); FlLB=0; }

void CRowView::OnInitialUpdate()
{
    m_nPrevRowCount = GetRowCount();  m_nPrevSelectedRow = GetActiveRow();
    UpdateScrollSizes();
}

//--------------------------------------------------------------
void CRowView::UpdateRow(int nInvalidRow)
{
  int nRowCount = GetRowCount();
// If the number of rows has changed, then adjust the scrolling range.
  if (nRowCount != m_nPrevRowCount) {
    UpdateScrollSizes();
    m_nPrevRowCount = nRowCount;
  }
// When the currently selected row changes:
// scroll the view so that the newly selected row is visible, and
// ask the derived class to repaint the selected and previously selected rows.

  CClientDC dc(this);
  OnPrepareDC(&dc);
// Determine the range of the rows that are currently fully visible in the
// window. We want to do discrete scrolling by so that the next or previous
// row is always fully visible.
  int nFirstRow, nLastRow;
  CRect rectClient;
  GetClientRect(&rectClient);
  dc.DPtoLP(&rectClient);
  RectLPtoRowRange(rectClient, nFirstRow, nLastRow, FALSE);
// If necessary, scroll the window so the newly selected row is visible.
  POINT pt;
  pt.x = 0;
  BOOL bNeedToScroll = TRUE;
  if (nInvalidRow < nFirstRow) {
// The newly selected row is above those currently visible in the window.
// Scroll so the newly selected row is at the very top of the window.
// The last row in the window might be only partially visible.
    pt.y = RowToYPos(nInvalidRow);
  }
  else
    if (nInvalidRow > nLastRow) {
// The newly selected row is below those currently visible in the window.
// Scroll so the newly selected row is at the very bottom of the window.
// The first row in the window might be only partially visible.
      pt.y = max(0, RowToYPos(nInvalidRow+1) - rectClient.Height());
    }
    else { bNeedToScroll = FALSE; }
    if (bNeedToScroll) {
      ScrollToDevicePosition(pt);
// Scrolling will cause the newly selected row to be redrawn in the
// invalidated area of the window.
      OnPrepareDC(&dc);  // Need to prepare the DC again because
// ScrollToDevicePosition() will have changed the viewport origin.  The DC is
// used some more below.
    }
    CRect rectInvalid = RowToWndRect(&dc, nInvalidRow);
    InvalidateRect(&rectInvalid);

// Give the derived class an opportunity to repaint the previously selected
// row, perhaps to un-highlight it.

    int nSelectedRow = GetActiveRow();
    if (m_nPrevSelectedRow != nSelectedRow) {
      CRect rectOldSelection = RowToWndRect(&dc, m_nPrevSelectedRow);
      InvalidateRect(&rectOldSelection);
      m_nPrevSelectedRow = nSelectedRow;
    }
}

//--------------------------------------------------------------
void CRowView::UpdateScrollSizes()
{
// UpdateScrollSizes() is called when it is necessary to adjust the scrolling
// range or page/line sizes.  There are two occassions where this is necessary:
// (1) when a new row is added-- see UpdateRow()-- and
// (2) when the window size changes-- see OnSize().

  CRect rectClient;
  GetClientRect(&rectClient);
  CClientDC dc(this);
  CalculateRowMetrics(&dc);

// The vert scrolling range is the total display height of all of the rows.
  CSize sizeTotal(m_nRowWidth,
                  m_nRowHeight * (min(GetRowCount(), LastViewableRow())));

// The vertical per-page scrolling distance is equal to the how many rows can
// be displayed in the current window, less one row for paging overlap.
  CSize sizePage(m_nRowWidth/5, max(m_nRowHeight,
                 ((rectClient.bottom/m_nRowHeight)-1)*m_nRowHeight));

// The vertical per-line scrolling distance is equal to the height of the row.
  CSize sizeLine(m_nRowWidth/20, m_nRowHeight);
  SetScrollSizes(MM_TEXT, sizeTotal, sizePage, sizeLine);
}

//--------------------------------------------------------------
BOOL CRowView::Maybe() { return FALSE;}
//--------------------------------------------------------------
void CRowView::OnDraw(CDC* pDC)
{ if (GetRowCount() == 0 && !pDC->IsPrinting() && !Maybe()) {
    CBrush brushBackground;
    COLORREF crOldBackground, crNewBackG = RGB(192,192,192);
    brushBackground.CreateSolidBrush(crNewBackG);
    crOldBackground = pDC->SetBkColor(crNewBackG);
    CRect rectClient;
    GetClientRect(&rectClient);  pDC->DPtoLP(&rectClient);
    pDC->FillRect(&rectClient, &brushBackground);
    pDC->SetBkColor(crOldBackground);
    return;
  } 

// The window has been invalidated and needs to be repainted;  or a page needs
// to be printed (or previewed). First, determine the range of rows that need
// to be displayed or printed.

  int nFirstRow, nLastRow;
  CRect rectClip;
  pDC->GetClipBox(&rectClip); // Get the invalidated region.
  RectLPtoRowRange(rectClip, nFirstRow, nLastRow, TRUE);
  if (pDC->IsPrinting()) {
    nFirstRow = 0;
  }
// Draw each row in the invalidated region of the window, or on the printed
// (previewed) page.
  int nActiveRow = GetActiveRow();
  int nRow, y;
  int nLastViewableRow = LastViewableRow();
  for (nRow = nFirstRow, y = m_nRowHeight * nFirstRow; nRow <= nLastRow;
                           nRow++, y += m_nRowHeight) {
      if (nRow > nLastViewableRow)  {
        CString strWarning;
        strWarning.LoadString(IDS_TOO_MANY_ROWS);
        pDC->TextOut(0, y, strWarning);
        break;
      }
      OnDrawRow(pDC, nRow, y, nRow == nActiveRow);
    }
}

//--------------------------------------------------------------
// Implementation ----------------------------------------------
//--------------------------------------------------------------
int CRowView::RowToYPos(int nRow)
              { return (nRow * m_nRowHeight); }

//--------------------------------------------------------------
CRect CRowView::RowToWndRect(CDC* pDC, int nRow)
{
  int nHorzRes = pDC->GetDeviceCaps(HORZRES);;
  CRect rect(0, nRow * m_nRowHeight, nHorzRes, (nRow + 1) * m_nRowHeight);
  pDC->LPtoDP(&rect);
  return rect;
}

//--------------------------------------------------------------
int CRowView::LastViewableRow()
              { return (INT_MAX / m_nRowHeight - 1); }

//--------------------------------------------------------------
void CRowView::RectLPtoRowRange(const CRect& rect, int& nFirstRow,
                               int& nLastRow, BOOL bIncludePartiallyShownRows)
{
 int nRounding = bIncludePartiallyShownRows? 0 : (m_nRowHeight - 1);
 nFirstRow = (rect.top + nRounding) / m_nRowHeight;
 nLastRow = min( (rect.bottom - nRounding) / m_nRowHeight, GetRowCount() - 1);
}

//--------------------------------------------------------------
void CRowView::OnPrepareDC(CDC* pDC, CPrintInfo* pInfo)
{
// The size of text that is displayed, printed or previewed changes depending
// on the DC.  We explicitly call OnPrepareDC() to prepare CClientDC objects
// used for calculating text positions and to prepare the text metric member
// variables of the CRowView object. The framework also calls OnPrepareDC()
// before passing the DC to OnDraw().

  CScrollView::OnPrepareDC(pDC, pInfo);
  CalculateRowMetrics(pDC);
}

//--------------------------------------------------------------
// Overrides of CView for implementing printing. ---------------
//--------------------------------------------------------------
BOOL CRowView::OnPreparePrinting(CPrintInfo* pInfo)
               { return DoPreparePrinting(pInfo); }

//--------------------------------------------------------------
void CRowView::OnBeginPrinting(CDC* pDC, CPrintInfo* pInfo)
{
// OnBeginPrinting() is called after the user has committed to printing by
// OK'ing the Print dialog, and after the framework has created a CDC
// object for the printer or the preview view.
// This is the right opportunity to set up the page range. Given the CDC
// object, we can determine how many rows will fit on a page, so we can
// in turn determine how many printed pages represent the entire document.

  int nPageHeight = pDC->GetDeviceCaps(VERTRES);
  CalculateRowMetrics(pDC);
  m_nRowsPerPrintedPage = nPageHeight / m_nRowHeight;
  int nPrintableRowCount = LastViewableRow() + 1;
  if (GetRowCount() < nPrintableRowCount) nPrintableRowCount = GetRowCount();
  pInfo->SetMaxPage((nPrintableRowCount + m_nRowsPerPrintedPage - 1)
                    / m_nRowsPerPrintedPage);
  pInfo->m_nCurPage = 1;  // start previewing at page# 1
}

//--------------------------------------------------------------
void CRowView::OnPrint(CDC* pDC, CPrintInfo* pInfo)
{  // Print the rows for the current page.
 int yTopOfPage = (pInfo->m_nCurPage -1) * m_nRowsPerPrintedPage * m_nRowHeight;

// Orient the viewport so that the first row to be printed
// has a viewport coordinate of (0,0).
 pDC->SetViewportOrg(0, -yTopOfPage);

// Draw as many rows as will fit on the printed page. Clip the printed page
// so that there is no partially shown row at the bottom of the page (the
// same row which will be fully shown at the top of the next page).

 int nPageWidth = pDC->GetDeviceCaps(HORZRES);
 CRect rectClip = CRect(0, yTopOfPage, nPageWidth,
                        yTopOfPage + m_nRowsPerPrintedPage * m_nRowHeight);
 pDC->IntersectClipRect(&rectClip);
 OnDraw(pDC);
}

//--------------------------------------------------------------
// CRowView commands -------------------------------------------
//--------------------------------------------------------------
void CRowView::OnSize(UINT nType, int cx, int cy)
{  UpdateScrollSizes();  CScrollView::OnSize(nType, cx, cy); }

//--------------------------------------------------------------
void CRowView::OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags)
{ switch (nChar)  {
  case VK_ADD: 
    { CKmDoc* pDoc = (CKmDoc*)GetDocument(); pDoc->GetStDt(); break; }  
  case VK_HOME:  
    if (GetActiveRow()!=0) ChangeSelectionToRow(1);   break;
  case VK_END:   
    if (GetActiveRow()!=GetRowCount()-1) ChangeSelectionToRow(GetRowCount()); break;
  case VK_UP:
    if (GetActiveRow()!=0) ChangeSelectionNextRow(FALSE); break;
  case VK_DOWN:  
    if (GetActiveRow()!=GetRowCount()-1) ChangeSelectionNextRow(TRUE);            break;
  case VK_PRIOR: OnVScroll(SB_PAGEUP,  0,  // nPos not used for PAGEUP
                           GetScrollBarCtrl(SB_VERT));    break;
  case VK_NEXT:  OnVScroll(SB_PAGEDOWN, 0,  // nPos not used for PAGEDOWN
                           GetScrollBarCtrl(SB_VERT));    break;
  default:       CScrollView::OnKeyDown(nChar, nRepCnt, nFlags);
  }
}


//--------------------------------------------------------------
void CRowView::OnUpdateFileNew(CCmdUI* pCmdUI)  { pCmdUI->Enable(FALSE); }

//--------------------------------------------------------------
void CRowView::OnUpdateFileOpen(CCmdUI* pCmdUI) { pCmdUI->Enable(FALSE); }

//--------------------------------------------------------------
void CRowView::OnFileEditPreview()
{  // CView::OnFilePrintPreview();
char Temp1[200];
CFile OdsFile;
UINT i,r;
char szBuffer[256];

  if (theApp.m_nAct == 7) {
    GetPrivateProfileString("CONFIG","WorkDir",".",Temp1,150,"edit_kom.ini");
    lstrcat(Temp1,"\\TEMP.CTR");
    WritePrivateProfileString ("CONFIG","Mode","Txt","EDIT_KOM.INI");
    WritePrivateProfileString("CONFIG", "PlikTitle","temp.CTR", "edit_kom.ini");
    WritePrivateProfileString("CONFIG", "Plik", Temp1, "edit_kom.ini");
    WritePrivateProfileString ("PAINT","Font","Courier New CE","EDIT_KOM.INI");
    WritePrivateProfileString ("PAINT","FIRST_H","10","EDIT_KOM.INI");
    WritePrivateProfileString ("PAINT","DEF_ODS","1","EDIT_KOM.INI");
    
    CString Row(theApp.GetProfileString("Config", "Nag", NULL));
    WritePrivateProfileString ("EKZEM","Naglowek",(const char *)Row,"EDIT_KOM.INI");
    CString Row1(theApp.GetProfileString("Config", "Stop", NULL));
    WritePrivateProfileString ("EKZEM","Stopka",(const char *)Row1,"EDIT_KOM.INI");

    if (! OdsFile.Open(Temp1, CFile::modeCreate|CFile::modeWrite))
      { AfxMessageBox(IDS_INVALID_START); return; }
    CKmDoc* pDoc = (CKmDoc*)GetDocument();
    r = pDoc->m_Obl.GetSize();
    for (i=0; i<r; i++) {
      lstrcpy(szBuffer,(const char *)pDoc->m_Obl.GetAt(i));
      lstrcat(szBuffer,"\r\n");
      OdsFile.Write(szBuffer,strlen(szBuffer));
    }
    OdsFile.Close();

    GetPrivateProfileString("CONFIG","EDIT_EXE",".",Temp1,150,"edit_kom.ini");
    lstrcat(Temp1," 1");
    if (WinExec(Temp1,SW_SHOWMAXIMIZED)<=32)
           { AfxMessageBox(IDS_INVALID_START); }
//           else
//           { bCh_send = 1;
//             CheckMenuItem(GetMenu(hWnd),ID_VIEW,MF_CHECKED);
//           }
  }
  else {
    // In derived classes, implement special window handling here
    // Be sure to Unhook Frame Window close if hooked.

    // must not create this on the frame.  Must outlive this function
    CPrintPreviewState* pState = new CPrintPreviewState;

    // DoPrintPreview's return value does not necessarily indicate that
    // Print preview succeeded or failed, but rather what actions are necessary
    // at this point.  If DoPrintPreview returns TRUE, it means that
    // OnEndPrintPreview will be (or has already been) called and the
    // pState structure will be/has been deleted.
    // If DoPrintPreview returns FALSE, it means that OnEndPrintPreview
    // WILL NOT be called and that cleanup, including deleting pState
    // must be done here.

    if (!DoPrintPreview(AFX_IDD_PREVIEW_TOOLBAR, this,
                            RUNTIME_CLASS(CPV), pState))
    {
        // In derived classes, reverse special window handling here for
        // Preview failure case

        TRACE0("Error: DoPrintPreview failed");
        AfxMessageBox(AFX_IDP_COMMAND_FAILURE);
        delete pState;      // preview failed to initialize, delete State now
    }
   }
}
//--------------------------------------------------------------------
void CRowView::OnUpdatePrV(CCmdUI* pCmdUI)
     { pCmdUI->Enable(theApp.m_nAct == 7); }
     
//--------------------------------------------------------------
void CRowView::OnLButtonDown(UINT, CPoint point)
{ 
  CClientDC dc(this);
  OnPrepareDC(&dc);
  dc.DPtoLP(&point);
  CRect rect(point, CSize(1,1));
  int nFirstRow, nLastRow;
  RectLPtoRowRange(rect, nFirstRow, nLastRow, TRUE);
  if (nFirstRow <= (GetRowCount() - 1))   ChangeSelectionToRow(nFirstRow+1);
  FlLB=theApp.m_nAct;
  SetCapture();       // Capture the mouse until button up.
}
/*
//--------------------------------------------------------------------
void CRowView::OnLButtonDblClk(UINT nFlags, CPoint point)
 { CScrollView::OnLButtonDblClk(nFlags, point); }
*/

//--------------------------------------------------------------------
void CRowView::OnMouseMove(UINT nFlags, CPoint point)
{ 
  if (GetCapture() != this)
    return; // If this window (view) didn't capture the mouse,
            // then the user isn't drawing in this window.
  if (FlLB==0 || (UINT)FlLB != theApp.m_nAct) FlLB=0;
  else { 
    CClientDC dc(this);
    OnPrepareDC(&dc);
    dc.DPtoLP(&point);
    CRect rect(point, CSize(1,1));
    int nFirstRow, nLastRow;
    RectLPtoRowRange(rect, nFirstRow, nLastRow, TRUE);
    if (nFirstRow < 0) nFirstRow=0;
    if (nFirstRow!=m_nPrevSelectedRow)
      if (nFirstRow <= (GetRowCount() - 1)) ChangeSelectionToRow(nFirstRow+1);
  }
}
//--------------------------------------------------------------------
void CRowView::OnLButtonUp(UINT nFlags, CPoint point)
 {
  if (GetCapture() != this)
    return; // If this window (view) didn't capture the mouse,
            // then the user isn't drawing in this window.
            
  FlLB=0; ReleaseCapture();   // Release the mouse capture established at
                        // the beginning of the mouse drag.
  CScrollView::OnLButtonUp(nFlags, point); 
}


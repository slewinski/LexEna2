// splitter.cpp : implementation file
// 96.06.20 09:34
//---------------------------------------------------------------

#include "stdafx.h"
#include "kkm.h"
#include "splitter.h"
#include "title.h"

#ifdef _DEBUG
#undef THIS_FILE
static char BASED_CODE THIS_FILE[] = __FILE__;
#endif

int indW;

BEGIN_MESSAGE_MAP(CSplitterFrame, CMDIChildWnd)
    //{{AFX_MSG_MAP(CSplitterFrame)
    ON_UPDATE_COMMAND_UI(ID_FILE_OPEN, OnUpdateFile)
    ON_UPDATE_COMMAND_UI(ID_FILE_NEW, OnUpdateFile)
    //}}AFX_MSG_MAP
END_MESSAGE_MAP()

//---------------------------------------------------------------
// CSplitterFrame

// Create a splitter window which splits an output text view and an input view
//              |
//    CNalView  | CWplView                 sp
//  ----------- | ----------                |
//    CNView    |  CWView               --- | ---sp2
//              |                           |
// -------------|                   sp1 --- |
//   COkrOdView |                           |
//  ----------- |                       --- |
//    COOView   |
//

IMPLEMENT_DYNCREATE(CSplitterFrame, CMDIChildWnd)

CSplitterFrame::CSplitterFrame()   { }
CSplitterFrame::~CSplitterFrame()  { }

//--------------------------------------------------------------------
BOOL CSplitterFrame::OnCreateClient(LPCREATESTRUCT, CCreateContext* pContext)
{
  MDIMaximize();
  RECT Rect;
  UINT SizeW1[2][4]={ {40,240,40,180},{40,192,40,88}};
 // create a splitter with 1 row, 2 columns
  GetClientRect(&Rect);
  if (Rect.bottom>300) indW=0; else indW=1;
   if (!m_wndSplitter.CreateStatic(this, 1, 2))
     { TRACE("Failed to CreateStaticSplitter\n"); return FALSE; }
   m_wndSplitter.SetColumnInfo(0,400-indW*32,140);
   m_wndSplitter.SetColumnInfo(1,230,140);

   if (!m_wndSplitter1.CreateStatic(&m_wndSplitter, 4, 1,
       WS_CHILD | WS_VISIBLE | WS_BORDER,       // style, WS_BORDER is needed
       m_wndSplitter.IdFromRowCol(0, 0)))  {
            TRACE("Failed to create nested splitter\n");
            return FALSE;
   }
   if (!m_wndSplitter2.CreateStatic(&m_wndSplitter, 2, 1,
       WS_CHILD | WS_VISIBLE | WS_BORDER,       // style, WS_BORDER is needed
       m_wndSplitter.IdFromRowCol(0, 1)))  {
            TRACE("Failed to create nested splitter\n");
            return FALSE;
   }
   m_wndSplitter1.SetRowInfo(0,SizeW1[indW][0],SizeW1[indW][0]);
   m_wndSplitter1.SetRowInfo(1,SizeW1[indW][1],40);
   m_wndSplitter1.SetRowInfo(2,SizeW1[indW][2],SizeW1[indW][2]);
   m_wndSplitter1.SetRowInfo(3,SizeW1[indW][3],16);

   m_wndSplitter2.SetRowInfo(0,SizeW1[indW][0],SizeW1[indW][0]);
   m_wndSplitter2.SetRowInfo(1,420-40*indW,40);

 // add the first splitter pane - the view in row 0
   if (!m_wndSplitter1.CreateView(0, 0,
       RUNTIME_CLASS(CTitleNal), CSize(0, SizeW1[indW][0]), pContext))
     { TRACE("Failed to create first pane\n"); return FALSE; }

 // add the first splitter pane - the view in row 1
   if (!m_wndSplitter1.CreateView(1, 0,
       RUNTIME_CLASS(CNView), CSize(0, SizeW1[indW][1]), pContext))
     { TRACE("Failed to create first pane\n"); return FALSE; }

  // add the first splitter pane - an view in row 2
   if (!m_wndSplitter1.CreateView(2, 0,
       RUNTIME_CLASS(CTitleOkr), CSize(0, SizeW1[indW][2]), pContext))
     { TRACE("Failed to create second pane\n"); return FALSE; }

 // add the first splitter pane - an view in row 3
   if (!m_wndSplitter1.CreateView(3, 0,
       RUNTIME_CLASS(COView), CSize(0, SizeW1[indW][3]), pContext))
     { TRACE("Failed to create first pane\n"); return FALSE; }

// add the second splitter pane - the view in row 0
   if (!m_wndSplitter2.CreateView(0, 0,
       RUNTIME_CLASS(CTitleWpl), CSize(0, SizeW1[indW][0]), pContext))
     { TRACE("Failed to create second pane\n"); return FALSE; }

 // add the second splitter pane - an view in row 1
   if (!m_wndSplitter2.CreateView(1, 0,
       RUNTIME_CLASS(CWView), CSize(0, 420-40*indW), pContext))
     { TRACE("Failed to create first pane\n"); return FALSE; }

 // activate the input view
   theApp.m_nAct =2;
   SetActiveView((CView*)m_wndSplitter1.GetPane(1,0));
   //MDIMaximize();
   return TRUE;
}

//--------------------------------------------------------------------
void CSplitterFrame::OnUpdateFile(CCmdUI* pCmdUI) { pCmdUI->Enable(FALSE); }

//--------------------------------------------------------
BOOL CSplitterFrame::PreCreateWindow( CREATESTRUCT& cs )
{
    // By turning off the default MFC-defined FWS_ADDTOTITLE style,
    // the framework will use first string in the document template
    // STRINGTABLE resource instead of the document name.

    cs.style &= ~(LONG)FWS_ADDTOTITLE;
    cs.style |= (LONG)WS_MAXIMIZE;
    return CMDIChildWnd::PreCreateWindow(cs);
}


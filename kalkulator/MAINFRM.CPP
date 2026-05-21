// MainFrm.cpp : implementation of the CMainFrame class
//

#include "stdafx.h"
#include "kkm.h"
#include "splitter.h"
#include "mainfrm.h"

#ifdef _DEBUG
#undef THIS_FILE
static char BASED_CODE THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CMainFrame

IMPLEMENT_DYNAMIC(CMainFrame, CMDIFrameWnd)

BEGIN_MESSAGE_MAP(CMainFrame, CMDIFrameWnd)
    //{{AFX_MSG_MAP(CMainFrame)
    ON_WM_CREATE()
    ON_COMMAND(ID_WPL, OnViewWpl)
    ON_COMMAND(ID_NAL, OnViewNal)
    ON_COMMAND(IDM_KALK, OnCloseKalk)
    ON_COMMAND(ID_OKROD, OnViewOkrOd)
    ON_UPDATE_COMMAND_UI(IDM_KALK, OnUpdateKalk)
    ON_UPDATE_COMMAND_UI(ID_FILE_NEW, OnUpdateFile)
    ON_UPDATE_COMMAND_UI(ID_NAL, OnUpdateNal)
    ON_UPDATE_COMMAND_UI(ID_WPL, OnUpdateWpl)
    ON_UPDATE_COMMAND_UI(ID_OKROD, OnUpdateOkrod)
    ON_WM_CLOSE()
    ON_UPDATE_COMMAND_UI(ID_FILE_OPEN, OnUpdateFile)
    ON_COMMAND(ID_HELP_INDEX, OnHelpIndex)
    ON_UPDATE_COMMAND_UI(IDM_DO_WYDR_R, OnUpdateDoWydrR)
    ON_COMMAND(IDM_DO_WYDR_R, OnDoWydrR)

  ON_UPDATE_COMMAND_UI(IDM_DO_WYDR, OnUpdateDoWydr)
	ON_COMMAND(IDM_DO_WYDR, OnDoWydr)
	//}}AFX_MSG_MAP
    //ON_COMMAND(ID_HELP_INDEX, CMDIFrameWnd::OnHelpIndex)
	ON_COMMAND(ID_HELP_USING, CMDIFrameWnd::OnHelpUsing)
	ON_COMMAND(ID_HELP, CMDIFrameWnd::OnHelp)
	ON_COMMAND(ID_CONTEXT_HELP, CMDIFrameWnd::OnContextHelp)
	ON_COMMAND(ID_DEFAULT_HELP, CMDIFrameWnd::OnHelpIndex)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// arrays of IDs used to initialize control bars

// toolbar buttons - IDs are command buttons
/*static UINT BASED_CODE buttons[] =
{
    // same order as in the bitmap 'toolbar.bmp'
    ID_FILE_NEW,
    ID_FILE_OPEN,
    ID_FILE_SAVE,
        ID_SEPARATOR,
    ID_NEW_ELEM,
    ID_EDIT_ELEM,
    ID_DEL_ELEM,
    ID_FILE_PRINT,    
        ID_SEPARATOR,        
    ID_PREV,
    ID_NEXT,
        ID_SEPARATOR,
    ID_OBL_STANDATE,
    ID_FILE_PRINT_PREVIEW,    
    IDM_KALK,
        ID_SEPARATOR,
        ID_SEPARATOR,
    IDM_POMOC,

};
*/

static UINT BASED_CODE indicators[] =
{
    ID_SEPARATOR,           // status line indicator
    ID_INDICATOR_REC0,
    ID_INDICATOR_CAPS,
    ID_INDICATOR_NUM,
    ID_INDICATOR_SCRL,
};

/////////////////////////////////////////////////////////////////////////////
// CMainFrame construction/destruction

CMainFrame::CMainFrame()  { }
CMainFrame::~CMainFrame() { }

/////////////////////////////////////////////////////////////////////////////
// CMainFrame diagnostics

#ifdef _DEBUG
void CMainFrame::AssertValid() const
{
    CMDIFrameWnd::AssertValid();
}

void CMainFrame::Dump(CDumpContext& dc) const
{
    CMDIFrameWnd::Dump(dc);
}

#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CMainFrame message handlers

void CMainFrame::CreateOrActivateFrame(CDocTemplate* pTemplate,
                                       CRuntimeClass* pViewClass)
{
// If a check view or book view (specified by pViewClass) already exists,
// then activate the MDI child window containing the view.  Otherwise,
// create a new view for the document.
// CMainFrame* pFrameWnd = (CMainFrame*)GetMDIFrame();

 CMDIChildWnd* pMDIActive = MDIGetActive();
 ASSERT(pMDIActive != NULL);
 CDocument* pDoc = pMDIActive->GetActiveDocument();
 ASSERT(pDoc != NULL);

 CView* pView;
 POSITION pos = pDoc->GetFirstViewPosition();
 while (pos != NULL) {
   pView = pDoc->GetNextView(pos);
   if (pView->IsKindOf(pViewClass)) {
   
     if (pView->IsKindOf(RUNTIME_CLASS(COblView))) { pView->OnInitialUpdate(); }
   
     pView->GetParentFrame()->ActivateFrame();
     pView->GetParentFrame()->SetActiveView(pView);
     pDoc->UpdateAllViews(NULL,0L,NULL);
     return;
   }
 }

 CMDIChildWnd* pNewFrame = (CMDIChildWnd*)(pTemplate->CreateNewFrame(pDoc, NULL));
 if (pNewFrame == NULL) return;     // not created
 ASSERT(pNewFrame->IsKindOf(RUNTIME_CLASS(CMDIChildWnd)));
 pTemplate->InitialUpdateFrame(pNewFrame, pDoc);
 //MDITile(MDITILE_VERTICAL);
 //pNewFrame->MDIMaximize();
}

//--------------------------------------------------------------------
void CMainFrame::OnOblicz() // theApp.m_nAct = 7;
     { CreateOrActivateFrame(theApp.m_pStanDateTemplate, RUNTIME_CLASS(COblView)); }

//--------------------------------------------------------------------
void CMainFrame::OnViewWpl() // theApp.m_nAct = 6;
     { CreateOrActivateFrame(theApp.m_pListWplTemplate, RUNTIME_CLASS(CWView)); }
//--------------------------------------------------------------------
void CMainFrame::OnViewNal() //theApp.m_nAct = 2;
     { CreateOrActivateFrame(theApp.m_pListNalTemplate, RUNTIME_CLASS(CNView)); }
//--------------------------------------------------------------------
void CMainFrame::OnViewOkrOd() //theApp.m_nAct = 4;
{ //if (theApp.m_nTyp==0)
  CreateOrActivateFrame(theApp.m_pListOkrOdTemplate, RUNTIME_CLASS(COView)); 
}

//--------------------------------------------------------------------
void CMainFrame::OnCloseKalk() //theApp.m_nAct = 2;
{
//  CMDIChildWnd* pMDIActive = MDIGetActive();
 //ASSERT(pMDIActive != NULL);
 //pMDIActive->DestroyWindow();
 CreateOrActivateFrame(theApp.m_pListNalTemplate, RUNTIME_CLASS(CNView)); }

//--------------------------------------------------------------------
int CMainFrame::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
  if (CMDIFrameWnd::OnCreate(lpCreateStruct) == -1) return -1;
  if (!m_wndToolBarIn.Create(this) ||	!m_wndToolBarIn.LoadToolBar(IDR_TBIN))
  {	TRACE0("Failed to create toolbar\n"); return -1; }// fail to create	

  if (!m_wndToolBarOut.Create(this) ||	!m_wndToolBarOut.LoadToolBar(IDR_TBOUT))
  {	TRACE0("Failed to create toolbar\n"); return -1; }// fail to create	

  if (!m_wndToolBarKalk.Create(this) ||	!m_wndToolBarKalk.LoadToolBar(IDR_TBKALK))
  {	TRACE0("Failed to create toolbar\n"); return -1; }// fail to create	

  if (!m_wndStatusBar.Create(this) || 
	  !m_wndStatusBar.SetIndicators(indicators,sizeof(indicators)/sizeof(UINT)))
  { TRACE0("Failed to create status bar\n"); return -1; }     // fail to create

  // TODO: Remove this if you don't want tool tips or a resizeable toolbar
	m_wndToolBarIn.SetBarStyle(m_wndToolBarIn.GetBarStyle() |
		CBRS_TOOLTIPS | CBRS_FLYBY | CBRS_SIZE_DYNAMIC);
	m_wndToolBarOut.SetBarStyle(m_wndToolBarOut.GetBarStyle() |
		CBRS_TOOLTIPS | CBRS_FLYBY | CBRS_SIZE_DYNAMIC);
	m_wndToolBarKalk.SetBarStyle(m_wndToolBarKalk.GetBarStyle() |
		CBRS_TOOLTIPS | CBRS_FLYBY | CBRS_SIZE_DYNAMIC);

	// TODO: Delete these three lines if you don't want the toolbar to
	//  be dockable
	m_wndToolBarIn.EnableDocking(CBRS_ALIGN_ANY);
	m_wndToolBarOut.EnableDocking(CBRS_ALIGN_ANY);
	m_wndToolBarKalk.EnableDocking(CBRS_ALIGN_ANY);
	
	EnableDocking(CBRS_ALIGN_ANY);
	DockControlBar(&m_wndToolBarIn);
	DockControlBar(&m_wndToolBarOut);
	DockControlBar(&m_wndToolBarKalk);
	ViewIn();
	return 0;
}

//--------------------------------------------------------------------
void CMainFrame::ViewIn()
{ 
  ShowControlBar(&m_wndToolBarKalk,FALSE, FALSE );
  ShowControlBar(&m_wndToolBarOut,FALSE, FALSE );
  ShowControlBar(&m_wndToolBarIn,TRUE, FALSE );	
/*
  m_wndToolBar.SetButtons(NULL,14); 
  m_wndToolBar.SetButtonInfo(0,ID_FILE_CLOSE,TBBS_BUTTON,13);
  m_wndToolBar.SetButtonInfo(1,ID_FILE_SAVE,TBBS_BUTTON,2);
  m_wndToolBar.SetButtonInfo(2,ID_SEPARATOR,TBBS_SEPARATOR,12);
  m_wndToolBar.SetButtonInfo(3,ID_NEW_ELEM,TBBS_BUTTON,3);
  m_wndToolBar.SetButtonInfo(4,ID_EDIT_ELEM,TBBS_BUTTON,4);
  m_wndToolBar.SetButtonInfo(5,ID_DEL_ELEM,TBBS_BUTTON,5);
  m_wndToolBar.SetButtonInfo(6,ID_SEPARATOR,TBBS_SEPARATOR,12);
  m_wndToolBar.SetButtonInfo(7,ID_PREV,TBBS_BUTTON,7);
  m_wndToolBar.SetButtonInfo(8,ID_NEXT,TBBS_BUTTON,8);
  m_wndToolBar.SetButtonInfo(9,ID_SEPARATOR,TBBS_SEPARATOR,12);
  m_wndToolBar.SetButtonInfo(10,ID_OBL_STANDATE,TBBS_BUTTON,9);
  //m_wndToolBar.SetButtonInfo(11,ID_FILE_PRINT_PREVIEW,TBBS_BUTTON,10);
  //m_wndToolBar.SetButtonInfo(12,IDM_KALK,TBBS_BUTTON,11);
  m_wndToolBar.SetButtonInfo(11,ID_SEPARATOR,TBBS_SEPARATOR,24);
  m_wndToolBar.SetButtonInfo(12,IDM_POMOC,TBBS_BUTTON,12);
  m_wndToolBar.SetButtonInfo(13,ID_CONTEXT_HELP,TBBS_BUTTON,14);
  m_wndToolBar.Invalidate();
// ID_FILE_NEW,    ID_FILE_OPEN,    ID_FILE_SAVE,         ID_SEPARATOR,
// ID_NEW_ELEM, ID_EDIT_ELEM, ID_DEL_ELEM, ID_FILE_PRINT, ID_SEPARATOR,
// ID_PREV,    ID_NEXT,                                   ID_SEPARATOR,
// ID_OBL_STANDATE,  ID_FILE_PRINT_PREVIEW,  IDM_KALK,    ID_SEPARATOR,
//                                                        ID_SEPARATOR,
// IDM_POMOC,
*/
}

//--------------------------------------------------------------------
void CMainFrame::ViewKalk()
{ 
	ShowControlBar(&m_wndToolBarIn,FALSE, FALSE );
	ShowControlBar(&m_wndToolBarOut,FALSE, FALSE );
	ShowControlBar(&m_wndToolBarKalk,TRUE, FALSE );
/*	
  m_wndToolBar.SetButtons(NULL,11);
  m_wndToolBar.SetButtonInfo(0,ID_FILE_CLOSE,TBBS_BUTTON,13);
  m_wndToolBar.SetButtonInfo(1,ID_FILE_SAVE,TBBS_BUTTON,2);
  m_wndToolBar.SetButtonInfo(2,ID_SEPARATOR,TBBS_SEPARATOR,12);
  m_wndToolBar.SetButtonInfo(3,ID_FILE_PRINT_PREVIEW,TBBS_BUTTON,10);
  m_wndToolBar.SetButtonInfo(4,ID_FILE_PRINT,TBBS_BUTTON,6);
  m_wndToolBar.SetButtonInfo(5,ID_SEPARATOR,TBBS_SEPARATOR,12);
  m_wndToolBar.SetButtonInfo(6,ID_OBL_STANDATE,TBBS_BUTTON,9);
  m_wndToolBar.SetButtonInfo(7,IDM_KALK,TBBS_BUTTON,11);
  m_wndToolBar.SetButtonInfo(8,ID_SEPARATOR,TBBS_SEPARATOR,24);
  m_wndToolBar.SetButtonInfo(9,IDM_POMOC,TBBS_BUTTON,12);
  m_wndToolBar.SetButtonInfo(10,ID_CONTEXT_HELP,TBBS_BUTTON,14);
  m_wndToolBar.Invalidate();
// ID_FILE_NEW,    ID_FILE_OPEN,    ID_FILE_SAVE,         ID_SEPARATOR,
// ID_NEW_ELEM, ID_EDIT_ELEM, ID_DEL_ELEM, ID_FILE_PRINT, ID_SEPARATOR,
// ID_PREV,    ID_NEXT,                                   ID_SEPARATOR,
// ID_OBL_STANDATE,  ID_FILE_PRINT_PREVIEW,  IDM_KALK,    ID_SEPARATOR,
//                                                        ID_SEPARATOR,
// IDM_POMOC,
*/
}

//--------------------------------------------------------------------
void CMainFrame::ViewOut()
{ 
	ShowControlBar(&m_wndToolBarIn,FALSE, FALSE );
	ShowControlBar(&m_wndToolBarKalk,FALSE, FALSE );
	ShowControlBar(&m_wndToolBarOut,TRUE, FALSE );
/*		
  m_wndToolBar.SetButtons(NULL,5);
  m_wndToolBar.SetButtonInfo(0,ID_FILE_NEW,TBBS_BUTTON,0);
  m_wndToolBar.SetButtonInfo(1,ID_FILE_OPEN,TBBS_BUTTON,1);
  m_wndToolBar.SetButtonInfo(2,ID_SEPARATOR,TBBS_SEPARATOR,24);
  m_wndToolBar.SetButtonInfo(3,IDM_POMOC,TBBS_BUTTON,12);
  m_wndToolBar.SetButtonInfo(4,ID_CONTEXT_HELP,TBBS_BUTTON,14);
  m_wndToolBar.Invalidate();
// ID_FILE_NEW,    ID_FILE_OPEN,    ID_FILE_SAVE,         ID_SEPARATOR,
// ID_NEW_ELEM, ID_EDIT_ELEM, ID_DEL_ELEM, ID_FILE_PRINT, ID_SEPARATOR,
// ID_PREV,    ID_NEXT,                                   ID_SEPARATOR,
// ID_OBL_STANDATE,  ID_FILE_PRINT_PREVIEW,  IDM_KALK,    ID_SEPARATOR,
//                                                        ID_SEPARATOR,
// IDM_POMOC,
*/
}
//--------------------------------------------------------------------
/*
BOOL CMainFrame::CreateToolBar()
{ if (!m_wndToolBar.Create(this) || !m_wndToolBar.LoadBitmap(IDR_MAINFRAME))
    { TRACE("Failed to create toolbar\n"); return FALSE; } // fail to create
  ViewIn();
  return TRUE;
}
*/

//--------------------------------------------------------
void CMainFrame::OnUpdateKalk(CCmdUI* pCmdUI)  { pCmdUI->Enable(theApp.m_nAct == 7); }
void CMainFrame::OnUpdateFile(CCmdUI* pCmdUI)  { pCmdUI->Enable(!(theApp.m_nAct>0)); }

//--------------------------------------------------------------------
BOOL CMainFrame::CanOds()
{ CMDIChildWnd* pMDIActive = MDIGetActive();
 if (pMDIActive == NULL) return(FALSE);
 CKmDoc* pDoc = (CKmDoc*) pMDIActive->GetActiveDocument();
 if (pDoc == NULL) return(FALSE);
 return(pDoc->CanOds());
}

//--------------------------------------------------------------------
void CMainFrame::OnUpdateNal(CCmdUI* pCmdUI)
 { if (theApp.m_nAct) pCmdUI->SetCheck(theApp.m_nAct==2); 
   else pCmdUI->Enable(FALSE);
 }
//--------------------------------------------------------------------
void CMainFrame::OnUpdateWpl(CCmdUI* pCmdUI)
 { if (theApp.m_nAct) pCmdUI->SetCheck(theApp.m_nAct==6);
   else pCmdUI->Enable(FALSE);
 }
//--------------------------------------------------------------------
void CMainFrame::OnUpdateOkrod(CCmdUI* pCmdUI)
 { pCmdUI->Enable(CanOds()); pCmdUI->SetCheck(theApp.m_nAct==4); }
 


void CMainFrame::OnUpdateDoWydr(CCmdUI* pCmdUI)
{ pCmdUI->SetCheck(theApp.m_nDoW);}

void CMainFrame::OnDoWydr()
{ if (theApp.m_nDoW==0) theApp.WriteProfileString("Config", "TabStan","1");
  else                  theApp.WriteProfileString("Config", "TabStan","0");
  theApp.m_nDoW=1-theApp.m_nDoW;
}
//--------------------------------------------------------------------
void CMainFrame::OnUpdateDoWydrR(CCmdUI* pCmdUI)
{  pCmdUI->SetCheck(theApp.m_nDoW_R); pCmdUI->Enable(theApp.m_nDoW); }

//--------------------------------------------------------------------
void CMainFrame::OnDoWydrR()
{ if (theApp.m_nDoW_R==0) theApp.WriteProfileString("Config", "TabStanR","1");
  else                  theApp.WriteProfileString("Config", "TabStanR","0");
  theApp.m_nDoW_R=1-theApp.m_nDoW_R;
}


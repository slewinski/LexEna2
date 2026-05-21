
#include "stdafx.h"
#include "afxext.h"
#include "afxpriv.h"
#include "cpv.h"

#ifdef AFX_PRINT_SEG
#pragma code_seg(AFX_PRINT_SEG)
#endif

#ifdef _DEBUG
#undef THIS_FILE
static char BASED_CODE THIS_FILE[] = __FILE__;
#endif


IMPLEMENT_DYNCREATE(CPV, CPreviewView)

//--------------------------------------------------------------

BEGIN_MESSAGE_MAP(CPV, CPreviewView)
    //{{AFX_MSG_MAP(CPV)
      ON_WM_LBUTTONDOWN()
      ON_UPDATE_COMMAND_UI(AFX_ID_PREVIEW_ZOOMIN, OnUpdateZoomIn)
      ON_UPDATE_COMMAND_UI(AFX_ID_PREVIEW_ZOOMOUT, OnUpdateZoomOut)    
    //}}AFX_MSG_MAP
    // Standard printing commands
END_MESSAGE_MAP()


//--------------------------------------------------------------
// CRowView construction, initialization, and destruction ------
//--------------------------------------------------------------
CPV::CPV() { }
CPV::~CPV() { }

void CPV::OnUpdateZoomIn(CCmdUI* pCmdUI)
{
    pCmdUI->Enable(FALSE);
}

void CPV::OnUpdateZoomOut(CCmdUI* pCmdUI)
{
    pCmdUI->Enable(FALSE);
}

void CPV::SetZoomState(UINT nNewState, UINT nPage, CPoint point)  { }
void CPV::OnLButtonDown(UINT, CPoint point) { }


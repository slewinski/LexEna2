// kkmView.cpp : implementation of the CKkmView class
//

#include "stdafx.h"
#include "kkm.h"

#include "kkmDoc.h"
#include "kkmView.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CKkmView

IMPLEMENT_DYNCREATE(CKkmView, CView)

BEGIN_MESSAGE_MAP(CKkmView, CView)
	//{{AFX_MSG_MAP(CKkmView)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG_MAP
	// Standard printing commands
	ON_COMMAND(ID_FILE_PRINT, CView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_DIRECT, CView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_PREVIEW, CView::OnFilePrintPreview)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CKkmView construction/destruction

CKkmView::CKkmView()
{
	// TODO: add construction code here

}

CKkmView::~CKkmView()
{
}

BOOL CKkmView::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs

	return CView::PreCreateWindow(cs);
}

/////////////////////////////////////////////////////////////////////////////
// CKkmView drawing

void CKkmView::OnDraw(CDC* pDC)
{
	CKkmDoc* pDoc = GetDocument();
	ASSERT_VALID(pDoc);

	// TODO: add draw code for native data here
}

/////////////////////////////////////////////////////////////////////////////
// CKkmView printing

BOOL CKkmView::OnPreparePrinting(CPrintInfo* pInfo)
{
	// default preparation
	return DoPreparePrinting(pInfo);
}

void CKkmView::OnBeginPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: add extra initialization before printing
}

void CKkmView::OnEndPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: add cleanup after printing
}

/////////////////////////////////////////////////////////////////////////////
// CKkmView diagnostics

#ifdef _DEBUG
void CKkmView::AssertValid() const
{
	CView::AssertValid();
}

void CKkmView::Dump(CDumpContext& dc) const
{
	CView::Dump(dc);
}

CKkmDoc* CKkmView::GetDocument() // non-debug version is inline
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CKkmDoc)));
	return (CKkmDoc*)m_pDocument;
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CKkmView message handlers

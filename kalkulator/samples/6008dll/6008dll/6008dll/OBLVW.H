// oblvw.h : interface of the CWplView class
// 96.06.10 08:45
//------------------------------------------------------------------
// Class CRowView implements a generic row-based scroll view.
// This derived class, COblView, implements the details specific
// to the OBLICZ application.
//------------------------------------------------------------------

/////////////////////////////////////////////////////////////////////////////
// COblView

//------------------------------------------------------------------
class COblView : public CRowView
{
    DECLARE_DYNCREATE(COblView)
public:
    COblView();

// Attributes

protected:
    int m_nRowsFact;  // how many rows fit on a printed page
    int m_nActPage;
    int m_nPr;

public:
    CKmDoc* GetDocument()
        {
            ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CKmDoc)));
            return (CKmDoc*) m_pDocument;
        }

// Activation
    void OnUpdate(CView* pSender, LPARAM lHint = 0L, CObject* pHint = NULL);
    virtual void OnActivateView(BOOL bActivate, CView* pActiveView, CView* pDeactiveView);

// Overrides of CRowView
    void GetRowWidthHeight(CDC* pDC, int& nRowWidth, int& nRowHeight);
    int  GetActiveRow();
    int  GetRowCount();
    void OnDrawRow(CDC* pDC, int nRowNo, int y, BOOL bSelected);
    void ChangeSelectionNextRow(BOOL bNext);
    void ChangeSelectionToRow(int nRow);

    virtual void OnDraw(CDC* pDC);  // overridden to draw this view
    virtual void OnPrepareDC(CDC* pDC, CPrintInfo* pInfo = NULL);
    virtual BOOL OnPreparePrinting(CPrintInfo* pInfo);
    virtual void OnBeginPrinting(CDC* pDC, CPrintInfo* pInfo);
    virtual void OnPrint(CDC* pDC, CPrintInfo* pInfo);
    virtual void RectLPtoRowRange(const CRect& rectLP,
            int& nFirstRow, int& nLastRow, BOOL bIncludePartiallyShownRows);
    void OnViewNal();            
// Implementation
protected:
    virtual ~COblView() {}
    // Generated message map functions
    //{{AFX_MSG(COblView)
    afx_msg void OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags);
    //}}AFX_MSG
    DECLARE_MESSAGE_MAP()

};


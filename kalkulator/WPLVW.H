// wplvw.h : interface of the CWplView class
// 96.06.10 08:45
//------------------------------------------------------------------
// Class CRowView implements a generic row-based scroll view.
// This derived class, CWplView, implements the details specific
// to the wpl book application.
//------------------------------------------------------------------

// Format of Wpl View line 1:
#define WPLNO_COL 1
#define WPLNO_LEN 3
#define DATE_COL    (WPLNO_COL + WPLNO_LEN + 2)
#define DATE_LEN    8
#define GROSZY_COL  (DATE_COL + DATE_LEN + 2)
#define GROSZY_LEN  11  // ########.##
#define TYP_COL     (GROSZY_COL + GROSZY_LEN + 4)
#define TYP_LEN     3
#define ROW_WIDTH   (TYP_COL + TYP_LEN)


//------------------------------------------------------------------
class CWView : public CRowView
{
    DECLARE_DYNCREATE(CWView)
public:
    CWView();

// Attributes
public:
    CKmDoc* GetDocument()
        {
            ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CKmDoc)));
            return (CKmDoc*) m_pDocument;
        }

// Overrides of CView
    void OnUpdate(CView* pSender, LPARAM lHint = 0L, CObject* pHint = NULL);
    // Activation
    virtual void OnActivateView(BOOL bActivate, CView* pActiveView, CView* pDeactiveView);

// Overrides of CRowView
    void GetRowWidthHeight(CDC* pDC, int& nRowWidth, int& nRowHeight);
    int  GetActiveRow();
    int  GetRowCount();
    void OnDrawRow(CDC* pDC, int nRowNo, int y, BOOL bSelected);
    void ChangeSelectionNextRow(BOOL bNext);
    void ChangeSelectionToRow(int nRow);
    BOOL Maybe();    
// Implementation
protected:
    virtual ~CWView() {}
// Generated message map functions
    //{{AFX_MSG(CWView)
    afx_msg void OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags);
    afx_msg void OnLButtonDblClk(UINT nFlags, CPoint point);
    afx_msg void OnUpdateEditElem(CCmdUI* pCmdUI);
    afx_msg void OnEditElem();
    afx_msg void OnRButtonDblClk(UINT nFlags, CPoint point);
    //}}AFX_MSG
    DECLARE_MESSAGE_MAP()
};

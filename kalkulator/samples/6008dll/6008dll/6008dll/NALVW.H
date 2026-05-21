// nalvw.h : interface of the CNView class
// 96.06.10 08:45
//------------------------------------------------------------------
// Class CRowView implements a generic row-based scroll view.
// This derived class, CNView, implements the details specific
// to the wpl book application.
//------------------------------------------------------------------

// Format of Nal View line 1:
#define NALNO_COL   1
#define NALNO_LEN   3
#define DATE_NAL_COL (NALNO_COL + NALNO_LEN + 2)
#define DATE_NAL_LEN 8
#define GROSZY_NAL_COL   (DATE_NAL_COL + DATE_NAL_LEN + 2)
#define GROSZY_NAL_LEN   11  // ########.##
#define TYP_NAL_COL  (GROSZY_NAL_COL + GROSZY_NAL_LEN + 4)
#define TYP_NAL_LEN  18
#define ROW_NAL_WIDTH (TYP_NAL_COL + TYP_NAL_LEN)


//------------------------------------------------------------------
class CNView : public CRowView
{
    DECLARE_DYNCREATE(CNView)
public:
    CNView();

// Attributes
public:
    CKmDoc* GetDocument()
        { ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CKmDoc)));
          return (CKmDoc*) m_pDocument;
        }

// Overrides of CView
    void OnUpdate(CView* pSender, LPARAM lHint = 0L, CObject* pHint = NULL);
    // Activation
    virtual void OnActivateView(BOOL bActivate, CView* pActivateView, CView* pDeactiveView);

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
    virtual ~CNView() {}
// Generated message map functions
    //{{AFX_MSG(CNView)
    afx_msg void OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags);
    afx_msg void OnLButtonDblClk(UINT nFlags, CPoint point); 
    afx_msg void OnUpdateEditElem(CCmdUI* pCmdUI);
    afx_msg void OnEditElem();
    afx_msg void OnRButtonDblClk(UINT nFlags, CPoint point);
    //}}AFX_MSG
    DECLARE_MESSAGE_MAP()
};

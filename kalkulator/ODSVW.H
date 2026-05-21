// odsvw.h : interface of the COView class
// 96.06.10 08:44
//------------------------------------------------------------------
// Class CRowView implements a generic row-based scroll view.
// This derived class, COView, implements the details specific
// to the okr od application.
//------------------------------------------------------------------

// Format of OkrOd View line 1:
#define OKRODNO_COL   1
#define OKRODNO_LEN   2
#define DATE_P_COL    (OKRODNO_COL + OKRODNO_LEN + 1)
#define DATE_P_LEN    8
#define DATE_K_COL    (DATE_P_COL + DATE_P_LEN + 2)
#define DATE_K_LEN    8
#define STOPA_COL     (DATE_K_COL + DATE_K_LEN + 2)
#define STOPA_LEN     6  // ##,####
#define TYP_O_COL     (STOPA_COL + STOPA_LEN + 4)
#define TYP_O_LEN     10
#define ROW_O_WIDTH   (TYP_O_COL + TYP_O_LEN)


//------------------------------------------------------------------
class COView : public CRowView
{
    DECLARE_DYNCREATE(COView)
public:
    COView();

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
    virtual ~COView() {}
// Generated message map functions
    //{{AFX_MSG(COView)
    afx_msg void OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags);
    afx_msg void OnLButtonDblClk(UINT nFlags, CPoint point);
    afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
    afx_msg void OnUpdateEditElem(CCmdUI* pCmdUI);
    afx_msg void OnEditElem();
    afx_msg void OnRButtonDblClk(UINT nFlags, CPoint point);
    //}}AFX_MSG
    DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////
// CPV
class CPV : public CPreviewView
{
    DECLARE_DYNCREATE(CPV)
// Constructors
public:
    CPV();

// Attributes
protected:
// Implementation
public:
    virtual ~CPV();
    void SetZoomState(UINT nNewState, UINT nPage, CPoint point);
protected:
    //{{AFX_MSG(CPV)
    afx_msg void OnUpdateZoomIn(CCmdUI* pCmdUI);
    afx_msg void OnUpdateZoomOut(CCmdUI* pCmdUI); 
    afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
    //}}AFX_MSG
    DECLARE_MESSAGE_MAP()
};


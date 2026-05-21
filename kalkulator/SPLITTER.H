// splitter.h : custom splitter control and frame that contains it
// 96.06.20 09:31
//-----------------------------------------------------------------
// CSplitterFrame frame with splitter/wiper

class CSplitterFrame : public CMDIChildWnd
{
    DECLARE_DYNCREATE(CSplitterFrame)
protected:
    CSplitterFrame();   // protected constructor used by dynamic creation

// Attributes
protected:
    CSplitterWnd m_wndSplitter;
    CSplitterWnd m_wndSplitter1;
    CSplitterWnd m_wndSplitter2;

// Operations

public:
// Implementation
    virtual ~CSplitterFrame();
    virtual BOOL OnCreateClient(LPCREATESTRUCT lpcs, CCreateContext* pContext);
    BOOL PreCreateWindow( CREATESTRUCT& cs );
    // Generated message map functions
    //{{AFX_MSG(CSplitterFrame)
    afx_msg void OnUpdateFile(CCmdUI* pCmdUI);
    //}}AFX_MSG
    DECLARE_MESSAGE_MAP()
};

//-----------------------------------------------------------------
// CSpFrameSKR name with splitter/wiper

class CSpFrameSKR : public CMDIChildWnd
{
    DECLARE_DYNCREATE(CSpFrameSKR)
protected:
    CSpFrameSKR();   // protected constructor used by dynamic creation

// Attributes
protected:
    CSplitterWnd m_wndSplitter;
    CSplitterWnd m_wndSplitter1;
    CSplitterWnd m_wndSplitter2;

public:
// Implementation
    virtual ~CSpFrameSKR();
    virtual BOOL OnCreateClient(LPCREATESTRUCT lpcs, CCreateContext* pContext);
    BOOL PreCreateWindow( CREATESTRUCT& cs );
    // Generated message map functions
    //{{AFX_MSG(CSpFrameSKR)
    afx_msg void OnUpdateFile(CCmdUI* pCmdUI);
    //}}AFX_MSG
    DECLARE_MESSAGE_MAP()
};

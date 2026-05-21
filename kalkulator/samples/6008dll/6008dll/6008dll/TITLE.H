// titlenal.h : header file
//
/////////////////////////////////////////////////////////////////////////////
// CTitleNal view
class CTitleNal : public CView
{ DECLARE_DYNCREATE(CTitleNal)
protected:
    CTitleNal();            // protected constructor used by dynamic creation
    virtual ~CTitleNal();
    virtual void OnDraw(CDC* pDC);      // overridden to draw this view
    virtual void OnUpdate(CView*, LPARAM lHint, CObject* pHint);
    // Generated message map functions
    //{{AFX_MSG(CTitleNal)
    //}}AFX_MSG
    DECLARE_MESSAGE_MAP()
};
/////////////////////////////////////////////////////////////////////////////
// CTitleWpl view
class CTitleWpl : public CView
{ DECLARE_DYNCREATE(CTitleWpl)
protected:
    CTitleWpl();            // protected constructor used by dynamic creation
    virtual ~CTitleWpl();
    virtual void OnDraw(CDC* pDC);      // overridden to draw this view
    virtual void OnUpdate(CView*, LPARAM lHint, CObject* pHint);
    // Generated message map functions
    //{{AFX_MSG(CTitleWpl)
    //}}AFX_MSG
    DECLARE_MESSAGE_MAP()
};
/////////////////////////////////////////////////////////////////////////////
// CTitleOkr view
class CTitleOkr : public CView
{ DECLARE_DYNCREATE(CTitleOkr)
protected:
    CTitleOkr();            // protected constructor used by dynamic creation
    virtual ~CTitleOkr();
    virtual void OnDraw(CDC* pDC);      // overridden to draw this view
    virtual void OnUpdate(CView*, LPARAM lHint, CObject* pHint);
    // Generated message map functions
    //{{AFX_MSG(CTitleOkr)
    //}}AFX_MSG
    DECLARE_MESSAGE_MAP()
};
/////////////////////////////////////////////////////////////////////////////

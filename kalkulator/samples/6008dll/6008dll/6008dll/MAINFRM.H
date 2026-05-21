// MainFrm.h : interface of the CMainFrame class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_MAINFRM_H__54DA43E9_F961_11D2_BAD3_0080AD17BE21__INCLUDED_)
#define AFX_MAINFRM_H__54DA43E9_F961_11D2_BAD3_0080AD17BE21__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000

class CMainFrame : public CMDIFrameWnd
{
    DECLARE_DYNAMIC(CMainFrame)
public:
    CMainFrame();
// Attributes


public:
    BOOL CreateToolBar();
    void ViewIn();
    void ViewOut();
    void ViewKalk();        
    virtual ~CMainFrame();
#ifdef _DEBUG
    virtual void AssertValid() const;
    virtual void Dump(CDumpContext& dc) const;
#endif

 void CreateOrActivateFrame(CDocTemplate* pTemplate,
                                       CRuntimeClass* pViewClass);
 void OnOblicz();
 BOOL CanOds();
protected:  // control bar embedded members
    CStatusBar  m_wndStatusBar;
    CToolBar    m_wndToolBarIn;
    CToolBar    m_wndToolBarOut;
    CToolBar    m_wndToolBarKalk;


// Generated message map functions
protected:
    //{{AFX_MSG(CMainFrame)
    afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
    afx_msg void OnViewWpl();
    afx_msg void OnViewNal();
    afx_msg void OnCloseKalk();    
    afx_msg void OnViewOkrOd();
    afx_msg void OnUpdateKalk(CCmdUI* pCmdUI);
    afx_msg void OnUpdateFile(CCmdUI* pCmdUI);
    afx_msg void OnUpdateNal(CCmdUI* pCmdUI);
    afx_msg void OnUpdateWpl(CCmdUI* pCmdUI);
    afx_msg void OnUpdateOkrod(CCmdUI* pCmdUI);
	afx_msg void OnUpdateDoWydr(CCmdUI* pCmdUI);
	afx_msg void OnDoWydr();
  afx_msg void OnDoWydrR();
  afx_msg void OnUpdateDoWydrR(CCmdUI* pCmdUI);
   //}}AFX_MSG
    DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_MAINFRM_H__54DA43E9_F961_11D2_BAD3_0080AD17BE21__INCLUDED_)

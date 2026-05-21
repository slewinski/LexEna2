// kkmDoc.h : interface of the CKkmDoc class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_KKMDOC_H__54DA43ED_F961_11D2_BAD3_0080AD17BE21__INCLUDED_)
#define AFX_KKMDOC_H__54DA43ED_F961_11D2_BAD3_0080AD17BE21__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000


class CKmDoc : public CDocument
{
protected: // create from serialization only
    CKmDoc();
    DECLARE_DYNCREATE(CKmDoc)

// Attributes
//protected:
public:
    CObList   m_nalList;    // Each element is CNal
    CObList   m_wplList;    // Each element is CWpl
    CObList   m_odsList;    // Each element is COkrOd
    CObList   m_stanList;   // Each element is dzien oblicz.stanu
    UINT      m_nalActive;  // Active record from CNal
    UINT      m_wplActive;  // Active record from CWpl
    UINT      m_odsActive;  // Active record from COkrOd
    CObList   m_k_Wpl;      // Each element is CK_Wpl
    CStringArray m_Obl;     // Each element is CStrObl
    UINT      m_oblActive;  // Active record from CStrObl
    BYTE      m_TypSpr;     // Typ=0 - KM, Typ=1 - SKR
    CString   m_Sprawa;     // Numer sprawy
// Operations
public:
    void DeleteContents ();
//--------------------------------- include "dock_w.cpp"
    CK_Wpl* NewK_Wpl(int Typ,int TypNal,POSITION pos, double Kwota, DWORD LDate);
    POSITION GetFirstK_WplPos();
    CK_Wpl* GetNextK_Wpl(POSITION& pos);
    void UpdateK_Wpl(POSITION pos, double Kwota);
    double CKmDoc::RemoveK_Wpl(POSITION pos);
//--------------------------------- include "docnal.cpp"
    CNal* NewNal();
    void DNewNal();
    POSITION GetFirstNalPos();
    CNal* GetNextNal(POSITION& pos);
    UINT GetActiveNalNo();
    UINT NalNoToRecordIndex(UINT nNewActNalNo );
    UINT RecordIndexToNalNo(UINT nRecInd);
    int  GetNal(UINT NalNo, DWORD& dwGroszy, int& nTypN, CString& strDate);
        //Return= CountOkrOd;
    void UpdateNal(CView* pSrcView,UINT NalNo,DWORD& dwGroszy,int& nTypN,CString& strDate,int n);
    void RemoveNal(UINT NalNo);
    void ChangeSelectionNextNalNo(BOOL bNext);
    void ChangeSelectionToNalNo(UINT nNewActNalNo);
    void DelNal();

//--------------------------------- include "docwpl.cpp"
    CWpl* NewWpl();
    void DNewWpl();
    POSITION GetFirstWplPos();
    CWpl* GetNextWpl(POSITION& pos);
    UINT GetActiveWplNo();
    UINT WplNoToRecordIndex(UINT nNewActWplNo );
    UINT RecordIndexToWplNo(UINT nRecInd);
    void GetWpl(UINT WplNo, DWORD& dwGroszy, int& nTypW, CString& strDate, int &nSQLTyp);
    void UpdateWpl(CView* pSrcView,UINT WplNo,DWORD& dwGroszy,int& nTypW,CString& strDate,int& nTypSQL);
    void SortWpl(POSITION pos,CString& strDate);
    void RemoveWpl(UINT WplNo);
    void ChangeSelectionNextWplNo(BOOL bNext);
    void ChangeSelectionToWplNo(UINT nNewActWplNo);
    void DelWpl();
//--------------------------------- include "docokr.cpp"
    COkrOd* NewOkrOd(UINT NumNal);
    void DNewOkrOd();
    CNal* GetParentNal(COkrOd* pOds);
    POSITION GetFirstOkrOdPos();
    COkrOd* GetNextOkrOd(POSITION& pos);
    UINT GetActiveOkrOdNo();
    UINT OkrOdNoToRecordIndex(UINT nNewActOkrOdNo );
    UINT RecordIndexToOkrOdNo(UINT nRecInd);
    UINT GetOkrCount(UINT NalNo);
    BOOL CanOds();
    void UpdateActiveOkrOd(UINT NalOld,UINT NalNew);
    void RemoveOkrOd(UINT OkrOdNo);
    COkrOd* GetOkrOd(UINT OkrOdNo,UINT ParentNo,DWORD& dwStopa,DWORD& dwFactor,int& nTypO,int& nTypW,
                        CString& strDateP,CString& strDateK);
    void UpdateOkrOd(CView* pSrcView, UINT OkrOdNo,UINT ParentNo,DWORD& dwStopa,DWORD& dwFactor,
                     int& nTypO,int& nTypW, CString& strDateP,CString& strDateK);
    void ChangeSelectionNextOkrOdNo(BOOL bNext);
    void ChangeSelectionToOkrOdNo(UINT nNewActOkrOdNo);
    void DelOkrOd();

//--------------------------------- include "oblicz.cpp"
    void CKmDoc::GetStDt();
    POSITION GetFirstStanPos();
    CString* GetNextStan(POSITION& pos);
    BOOL Oblicz();
    void LongDate();
    void PutOblHead();
    void PutOblStanDay();
    void PutOblWpl();
    void Put_OkrOds(int nalNum);
    void FormDoc(double Kw);
    POSITION NewWplStanu();
    void PutDodatek();

//--------------------------------- include "obcomput.cpp"    
    double ComputeWpl();
    double DoWpl(int OdOldWpl, double Kw, POSITION pzW);
    void RealNadplata(POSITION& pzW, POSITION pos, int typ, long dat,
                      double& Kw, double kwota,int m_TypNal);
    double KwotaOds(COkrOd* pOkrOd, DWORD daysF, DWORD daysE);
    double RealWplaty(double Kw);
    void PoWplate();
    void PredDoWpl(int OdOldWpl);
    void ClearKWpl();
    double Plata(int Typ,CWpl *pWpl, double Kwota);
    void TSLine(CWpl *pWpl);
		void FormOstNal();
		BOOL DocOstNal();
		void FormIsxNal();
// Implementation
public:
    virtual ~CKmDoc();
    virtual void Serialize(CArchive& ar);   // overridden for document i/o
#ifdef _DEBUG
    virtual void AssertValid() const;
#endif

protected:
    void InitDocument();
    BOOL OnNewDocument();
    BOOL OnNewDocumentFile();
    BOOL OnOpenDocument(const char* pszPathName);
    BOOL OnSaveDocument(const char* pszPathName);
    void OnCloseDocument();

// Generated message map functions
protected:
    //{{AFX_MSG(CKmDoc)
    afx_msg void OnNewElem();
    afx_msg void OnUpdateNewElem(CCmdUI* pCmdUI);
    afx_msg void OnDelElem();
    afx_msg void OnUpdateDelElem(CCmdUI* pCmdUI);
    afx_msg void OnNext();
    afx_msg void OnPrev();
    afx_msg void OnUpdatePrev(CCmdUI* pCmdUI);
    afx_msg void OnUpdateNext(CCmdUI* pCmdUI);
    afx_msg void OnOdsetki();
    afx_msg void OnStanDate();                   // from include "oblicz.cpp"
		afx_msg void OnNag();
		afx_msg void OnUpdateNag(CCmdUI* pCmdUI);
		afx_msg void OnStop();
		afx_msg void OnUpdateStop(CCmdUI* pCmdUI);
	afx_msg void OnKonwert();
	//}}AFX_MSG
    DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_KKMDOC_H__54DA43ED_F961_11D2_BAD3_0080AD17BE21__INCLUDED_)

// km.h : main header file for the KM application
//

#if !defined(AFX_KKM_H__54DA43E5_F961_11D2_BAD3_0080AD17BE21__INCLUDED_)
#define AFX_KKM_H__54DA43E5_F961_11D2_BAD3_0080AD17BE21__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"       // main symbols

/////////////////////////////////////////////////////////////////////////////
// CkmApp:
// See km.cpp for the implementation of this class
//

class CNal;
class CWpl;
class COkrOd;
class CK_Wpl;
class CPo_Wpl;

#include "kkmdoc.h"
#include "date.h"
#include "kkmobj.h"
#include "rowview.h"
#include "wplvw.h"
#include "oblvw.h"
#include "nalvw.h"
#include "odsvw.h"

//--------------------------- w rekordzie Naleznosc
#define T_NALEZNOSC 0
#define T_KOSZTY_PROCESU 1
#define T_KOSZTY_KLAUZULI 2
#define T_KOSZTY_ADWOK 3
#define T_KOSZTY_ADW_E 4
#define T_KOSZTY_EKZEK 5
#define T_ODSETKI_ZAL 6
#define T_NALEZ_SPLACONA 7
#define T_GRZYWNA 8
#define T_KARA_PIEN 9

//--------------------------- w rekordach LineTime, K_Wpl i Po_Wpl
#define TYP_KOSZTY 8
#define TYP_NALEZNOSC 0
#define TYP_ODSETKI_POCZ 1
#define TYP_ODSETKI_KON 2
#define TYP_WPLATA 3               // Tylko LineTime
#define TYP_STAN_DATE 4            // Tylko LineTime
#define TYP_ODSETKI_OPRACOWANE 5   // Tylko LineTime

struct oouv {
  CString strD;
  long lDP;
  DWORD Stopa;
};
#define MAXOKRODSUST 40
//#define MAXOKRODSSKR 40

extern struct oouv  OOUV[MAXOKRODSUST];
//extern struct oouv  OOSK[MAXOKRODSSKR];
extern int IleOkrOdsUst;
//extern int IleOkrOdsSkr;
extern int FlLB;
/////////////////////////////////////////////////////////////////////////////
// CKmApp:
// See km.cpp for the implementation of this class
//

class CKmApp : public CWinApp
{
public:
    CKmApp();
    CMultiDocTemplate* m_pListNalTemplate;
    CMultiDocTemplate* m_pListWplTemplate;
    CMultiDocTemplate* m_pListOkrOdTemplate;
    CMultiDocTemplate* m_pNT;
    CMultiDocTemplate* m_pWT;
    CMultiDocTemplate* m_pOT;
    CMultiDocTemplate* m_pStanDateTemplate;
    
    UINT m_nAct;
    CString m_Date; 
    BYTE m_nTyp;                      // 0 - KM , 1 - SK
    int  m_nDoW;                      // 0 - nic, 1 - dodac do wydruku tabelu stanow
    int  m_nDoW_R;                    // 0 - wlaczyc w tabelu, 1 - nie
// Operations
public:
    void UpdateIniFileWithDocPath(const char* pszPathName);
    BOOL InitOOUV();
//    BOOL InitOOSK();
// Overrides
    virtual BOOL InitInstance();
    CString GetDocPathFromIniFile();
// Implementation

    //{{AFX_MSG(CKmApp)
    afx_msg void OnAppAbout();
    afx_msg void OnPomoc();
    afx_msg void OnFileNew();
    //}}AFX_MSG
    DECLARE_MESSAGE_MAP()
};

extern CKmApp theApp;
/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_KKM_H__54DA43E5_F961_11D2_BAD3_0080AD17BE21__INCLUDED_)

// km.cpp : Defines the class behaviors for the application.
//

#include "stdafx.h"
#include "kkm.h"
#include "splitter.h"
#include "mainfrm.h"
#include "title.h"
//#include "controls.h"

#ifdef _DEBUG
#undef THIS_FILE
static char BASED_CODE THIS_FILE[] = __FILE__;
#endif

char  szIniFileSection[] = "KM Most Recent File";
/////////////////////////////////////////////////////////////////////////////
// The one and only CKmApp object


CKmApp theApp;

struct oouv  OOUV[MAXOKRODSUST];
int IleOkrOdsUst;
///////////////////////////////////
double SummaOdsetek;
double SummaWplatOdsetek;
double SummaWplatKwotyGlawne;
int TypEnter=1;
int flag_typ_month;
char grupa;
int Konwert_Yes;
///////////////////////////////////
//--------------------------------------------------------
BOOL CKmApp::InitOOUV()
{ //char *pszOdsPath="_ustaw.ods";
  CFile OdsFile;
  int i;
  UINT r;
  char szBuffer[256];
	CString W;
	W = GetProfileString("Config", "PathToOds");
	if (W.IsEmpty()) W = "_ustaw.ods";
	else {
		if (W.Right(1) == '\\') W += "_ustaw.ods";
		else {W += "\\_ustaw.ods";}
	}
 if (! OdsFile.Open((const char *)W, CFile::modeRead))
   { AfxMessageBox("Brak pliku odsetek ustawowych");   return FALSE; }
 r = OdsFile.Read(szBuffer,4);
 if (r!=4)
   { AfxMessageBox("Z³y plik odsetek ustawowych");   return FALSE; }
 sscanf(szBuffer,"%u",&IleOkrOdsUst);
 for (i = 0; i<IleOkrOdsUst; i++) {
   r = OdsFile.Read(szBuffer,16);
   if (r!=16)
	   { AfxMessageBox("Z³y plik odsetek ustawowych");   return FALSE; }
   szBuffer[8] = 0;
   OOUV[i].strD=szBuffer;
   sscanf(szBuffer+9,"%5lu",&OOUV[i].Stopa);
   OOUV[i].lDP=(date_t)GetStrDate(OOUV[i].strD);
 }
 OdsFile.Close();
 return TRUE;
}

/////////////////////////////////////////////////////////////////////////////
// CKmApp

BEGIN_MESSAGE_MAP(CKmApp, CWinApp)
    //{{AFX_MSG_MAP(CKmApp)
    ON_COMMAND(ID_APP_ABOUT, OnAppAbout)
    ON_COMMAND(IDM_POMOC, OnPomoc)
    ON_COMMAND(ID_FILE_NEW, OnFileNew)
    ON_COMMAND(ID_FILE_OPEN, OnFileOpen)  
    //}}AFX_MSG_MAP
    // Standard file based document commands
//    ON_COMMAND(ID_FILE_OPEN, CWinApp::OnFileOpen)  
    // Standard print setup command
    ON_COMMAND(ID_FILE_PRINT_SETUP, CWinApp::OnFilePrintSetup)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CKmApp construction

CKmApp::CKmApp()
{ char Date[20]="  -  -  ";
  m_nAct=0;
  _strdate(Date+10);
  Date[6] = Date[16]; Date[7] = Date[17];
  Date[0] = Date[13]; Date[1] = Date[14];
  Date[3] = Date[10]; Date[4] = Date[11];
  m_Date = Date;
              // Place all significant initialization in InitInstance
}

/////////////////////////////////////////////////////////////////////////////
// CKmApp initialization

BOOL CKmApp::InitInstance()
{
	InitCommonControls();
//  HINSTANCE m_hInstance;
  m_nDoW = GetProfileInt("Config", "TabStan", 1);
  m_nDoW_R = GetProfileInt("Config", "TabStanR", 1);
  //SetDialogBkColor();        // Set dialog background color to gray
//      m_hInstance = GetModuleHandle((LPCTSTR) "controls.dll");
//    if (!m_hPrevInstance)
//       if (!RegisterCustomControls(m_hInstance)) return FALSE;
//	m_nTyp=0;
    m_pListNalTemplate = new CMultiDocTemplate(  IDR_LIST, RUNTIME_CLASS(CKmDoc),
        RUNTIME_CLASS(CSplitterFrame),
        RUNTIME_CLASS(CNView));
    AddDocTemplate(m_pListNalTemplate);

    m_pListWplTemplate = new CMultiDocTemplate(  IDR_LIST, RUNTIME_CLASS(CKmDoc),
        RUNTIME_CLASS(CSplitterFrame),
        RUNTIME_CLASS(CWView));
    AddDocTemplate(m_pListWplTemplate);

    m_pListOkrOdTemplate = new CMultiDocTemplate(IDR_LIST, RUNTIME_CLASS(CKmDoc),
        RUNTIME_CLASS(CSplitterFrame),
        RUNTIME_CLASS(COView));
    AddDocTemplate(m_pListOkrOdTemplate);

    m_pNT = new CMultiDocTemplate(               IDR_LIST, RUNTIME_CLASS(CKmDoc),
        RUNTIME_CLASS(CSplitterFrame),
        RUNTIME_CLASS(CTitleNal));
    AddDocTemplate(m_pNT);

    m_pWT = new CMultiDocTemplate(               IDR_LIST, RUNTIME_CLASS(CKmDoc),
        RUNTIME_CLASS(CSplitterFrame),
        RUNTIME_CLASS(CTitleWpl));
    AddDocTemplate(m_pWT);

    m_pOT = new CMultiDocTemplate(               IDR_LIST, RUNTIME_CLASS(CKmDoc),
        RUNTIME_CLASS(CSplitterFrame),
        RUNTIME_CLASS(CTitleOkr));
    AddDocTemplate(m_pOT);

    m_pStanDateTemplate = new CMultiDocTemplate( IDR_RRRTYPE, RUNTIME_CLASS(CKmDoc),
        RUNTIME_CLASS(CMDIChildWnd),
        //RUNTIME_CLASS(CMDIcw),
        RUNTIME_CLASS(COblView));
    AddDocTemplate(m_pStanDateTemplate);
	if (!InitOOUV()) return FALSE;
  // create main MDI Frame window
  CMainFrame* pMainFrame = new CMainFrame;
  if (!pMainFrame->LoadFrame(IDR_MAINFRAME))  return FALSE;
    // The main window has been initialized, so show and update it.
  pMainFrame->ShowWindow(SW_SHOWMAXIMIZED);
  pMainFrame->UpdateWindow();
  m_pMainWnd = pMainFrame;
	Konwert_Yes = 0;			// 15.06.02   Nie trzeba konwertowac = 0

  CString strDocPath;
  if (m_lpCmdLine[0] == '\0'){
   	strDocPath = GetDocPathFromIniFile();
		TypEnter = 1;
		if (!strDocPath.IsEmpty())   OpenDocumentFile(strDocPath);
		else m_pListNalTemplate->OpenDocumentFile(NULL);
  }
  else {
		TypEnter = 2;
		//strDocPath = m_lpCmdLine;
		if (!strDocPath.IsEmpty())   OpenDocumentFile(NULL);
		else m_pListNalTemplate->OpenDocumentFile(NULL);
  }
  return TRUE;
}

//-----------------------------------------------------
void CKmApp::OnFileNew()
{ m_pListNalTemplate->OpenDocumentFile(NULL); }

//-----------------------------------------------------
void CKmApp::OnFileOpen()
{ 
  CString strDocPath;
 	strDocPath = GetDocPathFromIniFile();
 	// prompt the user (with all document templates)
////////////////////////////////////////////////////////////////////
	if (!DoPromptFileName(strDocPath, AFX_IDS_OPENFILE,
	  OFN_HIDEREADONLY | OFN_FILEMUSTEXIST, TRUE, NULL))
		return; // open cancelled
	AfxGetApp()->OpenDocumentFile(strDocPath);
		// if returns NULL, the user has already been alerted
}

//----------------------------------------------------------------
// INI file implementation ---------------------------------------
//----------------------------------------------------------------

static char BASED_CODE szIniFileEntry[] = "File";

//----------------------------------------------------------------
void CKmApp::UpdateIniFileWithDocPath(const char* pszPathName)
{  WriteProfileString(szIniFileSection, szIniFileEntry, pszPathName); }

//----------------------------------------------------------------
CString CKmApp::GetDocPathFromIniFile()
{  return GetProfileString(szIniFileSection, szIniFileEntry, NULL); }

/////////////////////////////////////////////////////////////////////////////
// CPomoc dialog
class CPomoc : public CDialog
{
// Construction
public:
    CPomoc(CWnd* pParent = NULL);   // standard constructor
// Dialog Data
    //{{AFX_DATA(CPomoc)
    enum { IDD = IDD_POMOC };
        // NOTE: the ClassWizard will add data members here
    //}}AFX_DATA

// Implementation
protected:
    virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

    // Generated message map functions
    //{{AFX_MSG(CPomoc)
        // NOTE: the ClassWizard will add member functions here
    //}}AFX_MSG
    DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////
// CPomoc dialog

CPomoc::CPomoc(CWnd* pParent /*=NULL*/)
    : CDialog(CPomoc::IDD, pParent)
{ //{{AFX_DATA_INIT(CPomoc)
    // NOTE: the ClassWizard will add member initialization here
  //}}AFX_DATA_INIT
}

void CPomoc::DoDataExchange(CDataExchange* pDX)
{
    CDialog::DoDataExchange(pDX);
    //{{AFX_DATA_MAP(CPomoc)
        // NOTE: the ClassWizard will add DDX and DDV calls here
    //}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CPomoc, CDialog)
    //{{AFX_MSG_MAP(CPomoc)
        // NOTE: the ClassWizard will add message map macros here
    //}}AFX_MSG_MAP
END_MESSAGE_MAP()

//----------------------------------------------------------------
void CKmApp::OnPomoc() { CPomoc PomocDlg; PomocDlg.DoModal(); }

/////////////////////////////////////////////////////////////////////////////
// CAboutDlg dialog used for App About
class CAboutDlg : public CDialog
{
public:
    CAboutDlg();
// Dialog Data
    //{{AFX_DATA(CAboutDlg)
    enum { IDD = IDD_ABOUTBOX };
    //}}AFX_DATA
// Implementation
protected:
    virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
    //{{AFX_MSG(CAboutDlg)
    //}}AFX_MSG
    DECLARE_MESSAGE_MAP()
public:
	//afx_msg void OnBnClickedRadioo0();
};

CAboutDlg::CAboutDlg() : CDialog(CAboutDlg::IDD) { }

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{ CDialog::DoDataExchange(pDX);
  //{{AFX_DATA_MAP(CAboutDlg)
  //}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialog)
    //{{AFX_MSG_MAP(CAboutDlg)
    //}}AFX_MSG_MAP
//	ON_BN_CLICKED(IDC_RADIOO0, OnBnClickedRadioo0)
END_MESSAGE_MAP()

//----------------------------------------------------------------
void CKmApp::OnAppAbout()  { CAboutDlg aboutDlg; aboutDlg.DoModal(); }
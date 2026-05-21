// zlgr.h: interface for DDX_DollarsCents
// 96.03.19 14:13
//--------------------------------------------------------------------------

#define INVALID_DOLLARS_CENTS 0xffffffff
#define INVALID_STOPA 0xffffffff
#define INVALID_DATE  "?"

void AFXAPI DDX_DollarsCents(CDataExchange* pDX, int nIDC, DWORD& dwCents);
BOOL GetDollarsCents(CWnd* pWnd, DWORD& dwCents);
BOOL GetDollarsCents(HWND hWnd, DWORD& dwCents);
void SetDollarsCents(CWnd* pWnd, DWORD dwCents);
CString GetDollarsCentsFormatted(DWORD dwCents);
CString GetDCFormatted(DWORD dwCents);
CString GetDCFormatted(double dwCents);

void AFXAPI DDX_Stopa(CDataExchange* pDX, int nIDC, DWORD& dwStopa);
BOOL GetStopa(CWnd* pWnd, DWORD& dwStopa);
BOOL GetStopa(HWND hWnd, DWORD& dwStopa);
void SetStopa(CWnd* pWnd, DWORD dwStopa);
CString GetStopaFormatted(DWORD dwStopa);
CString GetStopaFormattedD(DWORD dwStopa);
void AFXAPI DDX_StrDate(CDataExchange* pDX, int nIDC, CString& strD);
BOOL GetStrDate(CWnd* pWnd, CString& strD);
BOOL GetStrDate(HWND hWnd, CString& strD);
void SetStrDate(CWnd* pWnd, CString& strD);

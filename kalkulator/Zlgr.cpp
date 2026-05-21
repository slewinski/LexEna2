// zlgr.cpp : implementation of DDX_DollarsCents
// 11-20-96 10:50pm

#include "stdafx.h"
#include "kkm.h"
#include "zlgr.h"
#include <stdio.h>
#include <stdlib.h>
#include <math.h>

#ifdef _DEBUG
#undef THIS_FILE
static char BASED_CODE THIS_FILE[] = __FILE__;
#endif
void SetDollarsCents(HWND hWnd, DWORD dwCents);

void SetStopa(HWND hWnd, DWORD dwStopa);
void SetStrDate(HWND hWnd, CString& strD);
//----------------------------------------------------------------
// Public functions ----------------------------------------------
//----------------------------------------------------------------

void AFXAPI DDX_DollarsCents(CDataExchange* pDX, int nIDC, DWORD& dwCents)
{
 HWND hWndCtrl = pDX->PrepareEditCtrl(nIDC);
 if (pDX->m_bSaveAndValidate) {
   if (!GetDollarsCents(hWndCtrl, dwCents)) {
     AfxMessageBox(IDS_INVALID_DOLLAR_CENT);
     pDX->Fail();
   }
 }
 else { SetDollarsCents(hWndCtrl, dwCents); }
}

//----------------------------------------------------------------
BOOL GetDollarsCents(CWnd* pWnd, DWORD& dwCents)
{
  ASSERT(pWnd != NULL);
  return GetDollarsCents(pWnd->m_hWnd, dwCents);
}

//----------------------------------------------------------------
BOOL GetDollarsCents(HWND hWnd, DWORD& dwCents)
{ char szWindowText[20];
 ::GetWindowText(hWnd, szWindowText, 19);
 DWORD dwDollars;
 int nCents;
 char* pc;
 char* szDollars;
 char* szCents;

// strip leading blanks
  for (szDollars = szWindowText;  *szDollars == ' ';  szDollars++) {
    if (*szDollars == 0) { dwCents = 0; return TRUE; }
  }
// parse dollar amount, before optional decimal point
  for (pc = szDollars; (*pc != ',') && (*pc != ' ') && (*pc != 0); pc++) {
    if ((*pc < '0') || (*pc > '9')) return FALSE;
  }
  BOOL bDollarsOnly = (*pc == 0);
  *pc = 0;
  if (strlen(szDollars) > 8)  return FALSE;
  if (strlen(szDollars) == 0) { dwDollars = 0L; }
  else {
    dwDollars = atol(szDollars);
    if (dwDollars > ((DWORD)0xffffffff)/100) return FALSE;
  }

  if (bDollarsOnly) { nCents = 0; }
  else { // decimal point was found
    // parse cents
    for (szCents = ++pc; (*pc != 0) && (*pc != ' '); pc++) {
      if ((*pc < '0') || (*pc > '9')) return FALSE;
    }
    if (*pc == ' ') {
      for (pc++ ; *pc != 0; pc++) {
        if (*pc != ' ') return FALSE;
      }
    }
    int nCentsStrLen = strlen(szCents);
    switch (nCentsStrLen) {
    case 0:   nCents = 0;                  break;
    case 1:   nCents = atoi(szCents) * 10; break;
    case 2:   nCents = atoi(szCents);      break;
    default:  return FALSE;
    }
  }
  dwCents = dwDollars * 100 + nCents;
  return TRUE;
}

//----------------------------------------------------------------
void SetDollarsCents(CWnd* pWnd, DWORD dwCents)
{ ASSERT(pWnd != NULL);
  SetDollarsCents(pWnd->m_hWnd, dwCents);
}

//----------------------------------------------------------------
void SetDollarsCents(HWND hWnd, DWORD dwCents)
{
// Convert the DWORD dollars/cents value to a string and display it in the
// dol/cents control. If the dollar/cent field has been previously determined
// by DDX_DollarsCents() to be invalid, then don't update it. Leave the invalid
// data in the field so the user can correct it, rather than replace it with
//  the literal translation of the INVALID_DOLLARS_CENTS #define value.
 if (dwCents == INVALID_DOLLARS_CENTS) return;
  CString str = GetDollarsCentsFormatted(dwCents);
  ::SetWindowText(hWnd, str.GetBufferSetLength(20));
}

//----------------------------------------------------------------
CString GetDollarsCentsFormatted(DWORD dwCents)
{
  if (dwCents == INVALID_DOLLARS_CENTS) { return "???"; }
  char szWindowText[20];
  DWORD dwDollars = dwCents / 100;
  WORD wCents = (WORD)(dwCents - 100 * dwDollars);
  char szCents[6];
  sprintf(szCents, "%u", wCents+100);
  sprintf(szWindowText, "%lu,%s", dwDollars, szCents+1);
  return CString(szWindowText);
}

//----------------------------------------------------------------
CString GetDCFormatted(double dwCents)
{
//  if (dwCents == INVALID_DOLLARS_CENTS) { return "???"; }
  char szWindowText[30];
  DWORD dwD1,dwD2,dwD3;
  double dwDollars;
  
  
  BYTE zn=' ';
  dwCents = ceil(dwCents); // cale groszy
  if (dwCents < 0) { dwCents = 0-dwCents; zn='-'; }

  dwDollars= dwCents / (double)100.;
  dwDollars= floor(dwDollars);
  WORD wCents = (WORD)( fmod( dwCents, (double)100. ));
  char szCents[6];
  sprintf(szCents, "%u", wCents+100);
  if (dwDollars>=(double)1000000000.) {
     dwD1 = (DWORD)(dwDollars / (double)1000000000.);
     dwDollars = fmod( dwDollars, (double)1000000000. );
     dwD2 = (DWORD)(dwDollars / (double)1000000.);
     dwDollars = fmod( dwDollars, (double)1000000. );
     dwD3 = (DWORD)(dwDollars / (double)1000.);
     dwDollars = fmod( dwDollars, (double)1000. );
     sprintf(szWindowText, "%c%lu.%03lu.%03lu.%03lu,%s",zn,dwD1,dwD2,dwD3,(DWORD)dwDollars,szCents+1);
  }
  else if (dwDollars>=(double)1000000.) {
     dwD1 = (DWORD)(dwDollars / 1000000.);
     dwDollars = fmod( dwDollars, (double)1000000. );
     dwD2 = (DWORD)(dwDollars / (double)1000.);
     dwDollars = fmod( dwDollars, (double)1000. );
     sprintf(szWindowText, "%c%lu.%03lu.%03lu,%s",zn,dwD1,dwD2,(DWORD)dwDollars,szCents+1);
  }
  else if (dwDollars>=1000L) {
     dwD1 = (DWORD)(dwDollars / (double)1000.);
     dwDollars = fmod( dwDollars, (double)1000. );
     sprintf(szWindowText, "%c%lu.%03lu,%s",zn,dwD1,(DWORD)dwDollars,szCents+1);
  }
  else
      sprintf(szWindowText, "%c%lu,%s",zn,(DWORD)dwDollars, szCents+1);
  return CString(szWindowText);
}

//----------------------------------------------------------------
CString GetDCFormatted(DWORD dwCents)
{
  if (dwCents == INVALID_DOLLARS_CENTS) { return "???"; }
  char szWindowText[20];
  DWORD dwD1,dwD2;
  DWORD dwDollars;
  BYTE zn=' ';
  if (dwCents & 0x80000000) { dwCents = 0-dwCents; zn='-'; }

  dwDollars= dwCents / 100;

  WORD wCents = (WORD)(dwCents - 100 * dwDollars);
  char szCents[6];
  sprintf(szCents, "%u", wCents+100);
  if (dwDollars>=1000000L) {
     dwD1 = dwDollars / 1000000L;
     dwDollars -= dwD1*1000000L;
     dwD2 = dwDollars / 1000L;
     dwDollars -= dwD2*1000L;
     sprintf(szWindowText, "%c%lu.%03lu.%03lu,%s",zn,dwD1,dwD2,dwDollars,szCents+1);
  }
  else
    if (dwDollars>=1000L) {
     dwD1 = dwDollars / 1000L;
     dwDollars -= dwD1*1000L;
     sprintf(szWindowText, "%c%lu.%03lu,%s",zn,dwD1,dwDollars,szCents+1);
    }
    else
      sprintf(szWindowText, "%c%lu,%s",zn,dwDollars, szCents+1);
  return CString(szWindowText);
}


//===================================================================
//----------------------------------------------------------------
// STOPA-servis
//----------------------------------------------------------------

void AFXAPI DDX_Stopa(CDataExchange* pDX, int nIDC, DWORD& dwStopa)
{
 HWND hWndCtrl = pDX->PrepareEditCtrl(nIDC);
 if (pDX->m_bSaveAndValidate) {
   if (!GetStopa(hWndCtrl, dwStopa)) {
     AfxMessageBox(IDS_INVALID_STOPA);
     pDX->Fail();
   }
 }
 else { SetStopa(hWndCtrl, dwStopa); }
}

//----------------------------------------------------------------
BOOL GetStopa(CWnd* pWnd, DWORD& dwStopa)
     { ASSERT(pWnd != NULL);  return GetStopa(pWnd->m_hWnd, dwStopa); }

//----------------------------------------------------------------
BOOL GetStopa(HWND hWnd, DWORD& dwStopa)
{ char szWindowText[20];
 ::GetWindowText(hWnd, szWindowText, 19);
 WORD wD;
 DWORD nC; //Marzena 29.05.98 bylo int
 char* pc;
 char* szD;
 char* szC;

  for (szD = szWindowText;  *szD == ' ';  szD++) { // strip leading blanks
    if (*szD == 0) { dwStopa = 0; return TRUE; }
  }
  for (pc=szD; (*pc!=',') && (*pc!=' ') && (*pc!=0); pc++) { // before decimal point
    if ((*pc < '0') || (*pc > '9')) return FALSE;
  }
  BOOL bDOnly = (*pc == 0);
  *pc = 0;
  if (strlen(szD) > 3)  return FALSE;
  if (strlen(szD) == 0) { wD = 0; }
  else wD = atoi(szD);
  if (bDOnly) { nC = 0; }
  else { // decimal point was found
    for (szC = ++pc; (*pc != 0) && (*pc != ' '); pc++) {
      if ((*pc < '0') || (*pc > '9')) return FALSE;
    }
    if (*pc == ' ') { for (pc++ ; *pc != 0; pc++) { if (*pc != ' ') return FALSE; } }
    int nCStrLen = strlen(szC);
    switch (nCStrLen) {
    case 0:   nC = 0;                break;
    case 1:   nC = (DWORD)(atol(szC) * 10000);  break; //Marzena
    case 2:   nC = (DWORD)(atol(szC) * 1000);   break; //Marzena
    case 3:   nC = (DWORD)(atol(szC) * 100);    break; //Marzena
    case 4:   nC = (DWORD)(atol(szC) * 10);     break; //Marzena
    case 5:   nC = (DWORD)(atol(szC) * 1);      break; //Marzena
    default:  return FALSE;
    }
  }
  dwStopa = (long)(wD * 100000) + nC; //Marzena
  return TRUE;
}

//----------------------------------------------------------------
void SetStopa(CWnd* pWnd, DWORD dwStopa)
    { ASSERT(pWnd != NULL); SetStopa(pWnd->m_hWnd, dwStopa); }
//----------------------------------------------------------------
void SetStopa(HWND hWnd, DWORD dwStopa)
{ if (dwStopa == INVALID_STOPA) return;
  CString str = GetStopaFormatted(dwStopa);
  ::SetWindowText(hWnd, str.GetBufferSetLength(20));
}
//----------------------------------------------------------------
CString GetStopaFormatted(DWORD dwStopa)
{ if (dwStopa == INVALID_STOPA) { return "???"; }
  char szWindowText[20];
  DWORD wD = dwStopa / 10000L;
  DWORD wC = dwStopa - 10000L * wD;
  char szC[10];
  int len;
  sprintf(szC, "%u", wC+10000);
  sprintf(szWindowText, "%lu,%s", wD, szC+1);
  len = lstrlen(szWindowText)-1;
  for (;; len--)
    if (szWindowText[len]=='0') szWindowText[len]=0;
    else
      if (szWindowText[len]==',') { szWindowText[len]=0; break; }
      else break;

  return CString(szWindowText);
}
//----------------------------------------------------------------
CString GetStopaFormattedD(DWORD dwStopa)
{ if (dwStopa == INVALID_STOPA) { return "???"; }
  char szWindowText[20];
  DWORD wD = dwStopa / 10000L;
  DWORD wC = dwStopa - 10000L * wD;
  char szC[10];
  int len;
  sprintf(szC, "%u", wC+10000);
  sprintf(szWindowText, "%lu,%s", wD, szC+1);
  len = lstrlen(szWindowText)-1;
  if (szWindowText[len]=='0') szWindowText[len]=0;
  return CString(szWindowText);
}

//===================================================================
//----------------------------------------------------------------
// DATE-servis
//----------------------------------------------------------------

void AFXAPI DDX_StrDate(CDataExchange* pDX, int nIDC, CString& strD)
{
 HWND hWndCtrl = pDX->PrepareEditCtrl(nIDC);
 if (pDX->m_bSaveAndValidate) {
   if (!GetStrDate(hWndCtrl, strD)) {
     AfxMessageBox(IDS_INVALID_DATE);
     pDX->Fail();
   }
 }
 else { SetStrDate(hWndCtrl, strD); }
}

//----------------------------------------------------------------
BOOL GetStrDate(CWnd* pWnd, CString& strD)
{
  ASSERT(pWnd != NULL);
  return GetStrDate(pWnd->m_hWnd, strD);
}

//----------------------------------------------------------------
BOOL GetStrDate(HWND hWnd, CString& strD)
{ char szWindowText[20];
 ::GetWindowText(hWnd, szWindowText, 19);

 int Year=0;
 int Month=0;
 int Day=0;
 char* pc = szWindowText;
 if (!lstrcmp(pc,"--------")) {  strD = szWindowText; return TRUE; }
 Day= atoi(pc);
 while (*pc) { if (*pc<'0' ||*pc>'9') break; else *pc++;}
 if(!*pc) return FALSE;
 if (*pc !='-') return FALSE;
 *pc++;
 Month= atoi(pc);
 while (*pc) { if (*pc<'0' ||*pc>'9') break; else *pc++;}
 if(!*pc) return FALSE;
 if (*pc !='-') return FALSE;
 *pc++;
 Year = atoi(pc);
 if (Year>150) return FALSE;
 if (Month<=0 || Month>12) return FALSE;
 if (Day<=0 || Day> GetDaysInMonth( Year, Month-1)) return FALSE;
 sprintf(szWindowText,"%02u-%02u-%02u",Day,Month,Year);
 strD = szWindowText;
 return TRUE;
}

//----------------------------------------------------------------
void SetStrDate(CWnd* pWnd, CString& strD)
    { ASSERT(pWnd != NULL); SetStrDate(pWnd->m_hWnd, strD); }
//----------------------------------------------------------------
void SetStrDate(HWND hWnd, CString& strD)
{ if ((const char *)strD == INVALID_DATE) return;
  CString str= (const char *)strD;
  ::SetWindowText(hWnd, str.GetBufferSetLength(20));
}

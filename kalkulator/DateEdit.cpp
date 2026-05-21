// NumericEdit.cpp : implementation file
//

#include "stdafx.h"
#include "DateEdit.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif
extern int Changes;

/////////////////////////////////////////////////////////////////////////////
// CDateEdit

CDateEdit::CDateEdit()
{
	m_editText="00-00-0000";
  passstr = "00-00-0000";
}

CDateEdit::~CDateEdit()
{
}

BEGIN_MESSAGE_MAP(CDateEdit, CEdit)
	//{{AFX_MSG_MAP(CDateEdit)
	ON_CONTROL_REFLECT(EN_CHANGE, OnChange)
	ON_WM_KEYDOWN()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CDateEdit message handlers

void CDateEdit::OnChange() 
{
  int iStartSel = 0,iEndSel;
	CString curText;
  
  GetSel(iStartSel,iEndSel);
  GetWindowText(curText);

  if (m_editText.Compare(curText))
  {
    SetWindowText(m_editText);
    SetSel(iStartSel,iEndSel);
  }
}

void CDateEdit::OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags) 
{
	int iStartSel,iEndSel;
  CString tmp0 = GetCurrentDateStr();
  CString tmp1 = "00-00-0000";

  if (m_editText.GetLength()<10) m_editText = tmp0;
  if (m_editText[0] == '-') m_editText = tmp0;
  GetSel(iStartSel,iEndSel);
  if (nChar >=96 && nChar <=105) nChar -= 48;
	switch (nChar)
	{
  case 0x30:
  case 0x31:
  case 0x32:
  case 0x33:
  case 0x34:
  case 0x35:
  case 0x36:
  case 0x37:
  case 0x38:
  case 0x39:
    
    m_editText = m_editText.Left(iStartSel)+tmp1.Mid(iStartSel,iEndSel-iStartSel)+m_editText.Right(m_editText.GetLength()-iEndSel);
    if (iStartSel == 10) break;
    if (iStartSel == 2 || iStartSel == 5)
    { 
      iStartSel ++;
      iEndSel ++;
    }
    if (m_editText==tmp1) m_editText=tmp0; 
    m_editText.SetAt(iStartSel,nChar); 
    SetWindowText(m_editText);
    SetSel(iStartSel,iStartSel+1);
  break;
  case VK_BACK:
		if (iStartSel > 0)
    {
      if (m_editText[iStartSel - 1] == '-') 
      {
        m_editText.SetAt(iStartSel - 2,'0');
        if (m_editText==tmp1) m_editText=tmp0;
        SetWindowText(m_editText);
        SetSel(iStartSel-1,iStartSel-1);
      }
      else 
      {
        m_editText.SetAt(iStartSel - 1,'0');
        if (m_editText==tmp1) m_editText=tmp0;
        SetWindowText(m_editText);
        if (iStartSel>1) SetSel(iStartSel,iStartSel);
      }
    }
    break;
	case VK_DELETE: 
    if (iStartSel == iEndSel)
    {
      if (iStartSel!=10)
      {
        if (iStartSel == 2 || iStartSel == 5)
        { 
          iStartSel ++;
          iEndSel ++;
        }
        m_editText.SetAt(iStartSel,'0');
        if (m_editText==tmp1) m_editText=tmp0;
        SetWindowText(m_editText);
        if (iStartSel<10) SetSel(iStartSel+1,iStartSel+1);
      }
    }
    else
    {
      m_editText = m_editText.Left(iStartSel)+tmp1.Mid(iStartSel,iEndSel-iStartSel)+m_editText.Right(m_editText.GetLength()-iEndSel);
      if (m_editText==tmp1) m_editText=tmp0;
        SetWindowText(m_editText);
        if (iStartSel>=1) SetSel(iStartSel,iStartSel);
    }
    break;
  case VK_LEFT:
  case VK_RIGHT:
  		CEdit::OnKeyDown(nChar, nRepCnt, nFlags);
	break;
  case 189:
    if (iStartSel == 2 || iStartSel == 5) 
    {
      iStartSel ++;
      iEndSel ++;
      SetWindowText(m_editText);
      SetSel(iStartSel,iStartSel) ;
    }
    else
    {
      m_editText = "----------";
      SetWindowText(m_editText);
  	  if (iStartSel>=1) SetSel(iStartSel,iStartSel);
    }
  break;
  }
}

BOOL CDateEdit::IsValidDate()
{
  COleDateTime d;
  int iDay,iMonth,iYear,ind;

  if (m_editText == "----------") return TRUE;
  if (m_editText.GetLength() != 10) return FALSE;
  if (m_editText[0]>'9' && m_editText[0]<'0') return FALSE;
  if (m_editText[1]>'9' && m_editText[1]<'0') return FALSE;
  if (m_editText[2] !='-') return FALSE;
  if (m_editText[3]>'9' && m_editText[3]<'0') return FALSE;
  if (m_editText[4]>'9' && m_editText[4]<'0') return FALSE;
  if (m_editText[5] !='-') return FALSE;
  if (m_editText[6]>'9' && m_editText[6]<'0') return FALSE;
  if (m_editText[7]>'9' && m_editText[7]<'0') return FALSE;
  if (m_editText[8]>'9' && m_editText[8]<'0') return FALSE;
  if (m_editText[9]>'9' && m_editText[9]<'0') return FALSE;
  iDay = atoi(m_editText.Left(2).GetBuffer(2));
  iMonth = atoi(m_editText.Mid(3,2).GetBuffer(2));
  iYear = atoi(m_editText.Right(4).GetBuffer(4));
  ind = d.SetDate(iYear,iMonth,iDay);
  if (ind == 0) return TRUE;
  else return FALSE;
}

CString CDateEdit::GetText()
{
  return m_editText;
}


void CDateEdit::SetDate(CString curstr)
{
  m_editText = curstr;
  SetWindowText(m_editText);
}

CString CDateEdit::GetCurrentDateStr()
{
    COleDateTime d = COleDateTime::GetCurrentTime();
    int iDay,iMonth,iYear;
    CString sDay,sMonth,sYear;
    char buf[5];

    iDay = d.GetDay();
    iMonth = d.GetMonth();
    iYear = d.GetYear();
    itoa(iDay,buf,10);
    sDay = buf ;
    if (sDay.GetLength()<2) sDay = "0"+sDay; 
    sDay += "-";
    itoa(iMonth,buf,10);
    sMonth = buf;
    if (sMonth.GetLength()<2) sMonth = "0"+ sMonth;
    sMonth += "-";
    itoa(iYear,buf,10);
    sYear = buf;
    return sDay+sMonth+sYear;
}

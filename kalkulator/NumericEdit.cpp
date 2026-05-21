// NumericEdit.cpp : implementation file
//

#include "stdafx.h"
//#include "Numeric.h"
#include "NumericEdit.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif
extern int Changes; 

/////////////////////////////////////////////////////////////////////////////
// CNumericEdit

CNumericEdit::CNumericEdit(BYTE nFract)
{
	m_nFractionalDigits = nFract; 
  m_editText="0,00";
}

CNumericEdit::~CNumericEdit()
{
}


BEGIN_MESSAGE_MAP(CNumericEdit, CEdit)
	//{{AFX_MSG_MAP(CNumericEdit)
	ON_CONTROL_REFLECT(EN_CHANGE, OnChange)
	ON_WM_KEYDOWN()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CNumericEdit message handlers

BOOL CNumericEdit::Create(const RECT &rect, CWnd *pParentWnd, UINT nID)
{
	BOOL bRes = CEdit::Create(ES_LEFT | WS_CHILD | WS_VISIBLE | WS_TABSTOP | WS_BORDER | 
		                      ES_NUMBER,rect,pParentWnd,nID);
	if (bRes)
	{
		m_editText = "0,00";
		SetWindowText("0,00");
	}
	return bRes;
}

void CNumericEdit::OnChange() 
{
	CString curText;
  Changes = 1;
	GetWindowText(curText);
	if (m_editText.Compare(curText))
	{
		int iPointPos,iStartSel = 0,iEndSel,iLen;

//если строка пустая, в нее записывается значение по умолчанию
		if (!(iLen = curText.GetLength()))
			m_editText = "0,00";
		else
		{
			GetSel(iStartSel,iEndSel);
			iPointPos = curText.Find(',');
//если запятая удалена, восстанавливаем ее в текущей позиции курсора
			if (iPointPos == -1)
			{
				curText.Insert(iStartSel,',');
				iLen ++;
				iPointPos = iStartSel;
			}
//если до запятой нет цифр, добавляем спереди ноль
			if (!iPointPos)
			{
				curText.Insert(0,'0');
				iLen ++;
			}
//если после запятой нет цифр, добавляем после нее ноль
			iPointPos = curText.Find(',');
      if (iPointPos == iLen - 2)
			{
				curText += "0";
				iLen +=1;
			}
      else
      {
        if (iPointPos == iLen-1)
			  { 
				  curText += "00";
				  iLen +=2;
			  }
      }
//если до точки был только ноль и мы вводим цифру с первой позиции, этот ноль удаляется 
			if (m_editText[0] == '0' && iStartSel < iPointPos && curText[0]!='0')
				curText.Delete(1,1);
//удаление нуля в старшей позиции
			if (curText[0] == '0' && curText[1] != ',' && iLen > 3)
			{
				curText.Delete(0,1);
				iStartSel --;
			}

//контроль количества цифр после запятой, если ввод идет после нее
			if (iPointPos < iStartSel)
			{	
				BYTE nFract = iLen - iPointPos - 1;

				while ((nFract --) > m_nFractionalDigits)
					curText.Delete(curText.GetLength() - 1,1);
			}
//контроль количества цифр после запятой
      
//контроль деления целой части числа на тройки цифр, разделенных точками
			else
				SetPoints(curText,iStartSel);
			m_editText = curText;
		}
		SetWindowText((LPCSTR) m_editText);
		SetSel(iStartSel,iStartSel);
	}
  
}

void CNumericEdit::SetFractDigits(BYTE nFract)
{
	if (nFract > 0)
	{
		if (nFract < m_nFractionalDigits)
		{
			BYTE nFractional = m_editText.GetLength() - m_editText.Find(',',0) - 1;

			while ((nFractional --) > nFract)
				m_editText.Delete(m_editText.GetLength() - 1,1);				
			SetWindowText(m_editText);
		}
		m_nFractionalDigits = nFract;
	}
}

BYTE CNumericEdit::GetFractDigits()
{
	return m_nFractionalDigits;
}

double CNumericEdit::GetDoubleValue()
{
	CString tmp = m_editText;
	int iPos = tmp.Find('.',0);

	while (iPos != -1)
	{
		tmp.Delete(iPos,1);
		iPos = tmp.Find('.',iPos);
	}
	tmp.Replace(',','.');
	return atof(tmp);
}

DWORD CNumericEdit::GetLongValue()
{
  CString tmp = m_editText;
	int iPos = tmp.Find('.',0);
  int len,fract;

	while (iPos != -1)
	{
		tmp.Delete(iPos,1);
		iPos = tmp.Find('.',iPos);
	}
	
  iPos = tmp.Find(',',0);
  len = tmp.GetLength();
  fract = GetFractDigits();
  while (iPos >= len-fract)
  {
    tmp +='0';
    len = tmp.GetLength();
  }
  tmp.Delete(iPos,1);
	
  return atol(tmp);
}

void CNumericEdit::OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags) 
{
	int iStartSel,iEndSel, iPos;

	GetSel(iStartSel,iEndSel);
//	if (iStartSel == iEndSel && iStartSel)
  if (nChar == 110 ) nChar +=78;
		switch (nChar)
		{
		case VK_BACK:
			if (iStartSel > 0)
      {
        if (m_editText[iStartSel - 1] == '.')
			  {
				  m_editText.Delete(iStartSel - 2,1);
				  SetPoints(m_editText,iStartSel);
				  SetWindowText(m_editText);
				  if (iStartSel>1) SetSel(iStartSel,iStartSel);
			  }
			  else
       // if (!iStartSel == m_editText.GetLength() )
      		CEdit::OnKeyDown(nChar, nRepCnt, nFlags);
      }
      else
        SetWindowText(m_editText);
      break;
		case VK_DELETE:
			if (iStartSel < m_editText.GetLength())
      {
        if (m_editText[iStartSel] == '.')
			  {
				  m_editText.Delete(iStartSel + 1,1);
				  SetPoints(m_editText,iStartSel);
				  SetWindowText(m_editText);
				  SetSel(iStartSel,iStartSel);
			  }
			  else
				  CEdit::OnKeyDown(nChar, nRepCnt, nFlags);
      }
		break;
    case 188:
      iPos = m_editText.Find(',',0);
      SetSel(iPos+1,iPos+1);
      /*if (iPos == iStartSel)
      {
        SetSel(iStartSel+1,iStartSel+1);
      }*/
      /*else if (iPos > iStartSel)
      {
        m_editText=m_editText.Left(iStartSel)+ ",00";
        SetPoints(m_editText,iStartSel);
			  SetWindowText(m_editText);
			  SetSel(iStartSel+1,iStartSel+1);
      }*/
		break;
    default:
			CEdit::OnKeyDown(nChar, nRepCnt, nFlags);
	    break;	
    }
//	else
//		CEdit::OnKeyDown(nChar, nRepCnt, nFlags);
}

void CNumericEdit::SetPoints(CString &curText, int &iStartSel)
{
	int iPos = curText.Find('.',0),iPointPos = curText.Find(',',0);

	while (iPos != -1)
	{
		curText.Delete(iPos,1);
		iPos = curText.Find('.',iPos);
		iStartSel --;
		iPointPos --;
	}
	for (int i=iPointPos-3;i>0;i-=3)
		if (curText[i] != '.')
		{
			curText.Insert(i,'.');
			iStartSel ++;
		}
}

void CNumericEdit::SetText(CString curstr)
{
  int beg;

  m_editText = curstr;
  SetPoints(m_editText,beg);
  SetWindowText(m_editText);
}




// amsEdit.cpp : implementation file for CEdit-derived classes
// Created by: Alvaro Mendez - 07/17/2000
//

#include "stdafx.h"
#include "amsEdit.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

#pragma warning (disable:4355)  // disables: 'this': used in base member initializer list

/////////////////////////////////////////////////////////////////////////////
// CAMSEdit

// Constructs the object with the default attributes
CAMSEdit::CAMSEdit() :
	m_rgbText(0),
	m_uInternalFlags(None)
{
}

// Destroys the object (virtual).
CAMSEdit::~CAMSEdit()
{
}

BEGIN_MESSAGE_MAP(CAMSEdit, CEdit)
	//{{AFX_MSG_MAP(CAMSEdit)
	ON_WM_KEYDOWN()
	//}}AFX_MSG_MAP
	ON_MESSAGE(WM_CUT, OnCut)
	ON_MESSAGE(WM_PASTE, OnPaste)
	ON_MESSAGE(WM_CLEAR, OnClear)
	ON_MESSAGE(WM_SETTEXT, OnSetText)
END_MESSAGE_MAP()

// Returns the control's text.
CString CAMSEdit::GetText() const
{
	CString strText;
	GetWindowText(strText);
	return strText;
}

// Returns the control's text without leading or trailing blanks.
CString CAMSEdit::GetTrimmedText() const
{
    CString strText = GetText();
    strText.TrimLeft();
    strText.TrimRight();
    return strText;
}

// Sets the control's text to the given string value.
void CAMSEdit::SetText(const CString& strText)
{
	SetWindowText(strText);
}

// Sets the background color to the given rgb.
void CAMSEdit::SetBackgroundColor(COLORREF rgb)
{
	m_brushBackground.DeleteObject();
	m_brushBackground.CreateSolidBrush(rgb);
	Invalidate();
}

// Returns the RGB for the background color.
COLORREF CAMSEdit::GetBackgroundColor() const
{
	CAMSEdit* pThis = const_cast<CAMSEdit*>(this);

	if (!m_brushBackground.GetSafeHandle())
	{
		COLORREF rgb = pThis->GetDC()->GetBkColor();
		pThis->m_brushBackground.CreateSolidBrush(rgb);
		return rgb;
	}

	LOGBRUSH lb;
	pThis->m_brushBackground.GetLogBrush(&lb);
	return lb.lbColor;
}

// Sets the text color to the given rgb.
void CAMSEdit::SetTextColor(COLORREF rgb)
{
	m_rgbText = rgb;
	m_uInternalFlags |= TextColorHasBeenSet;
	Invalidate();
}

// Returns the RGB for the text color.
COLORREF CAMSEdit::GetTextColor() const
{
	if (!(m_uInternalFlags & TextColorHasBeenSet))
	{
		CAMSEdit* pThis = const_cast<CAMSEdit*>(this);
		pThis->m_rgbText = pThis->GetDC()->GetTextColor();
		pThis->m_uInternalFlags |= TextColorHasBeenSet;
	}
	return m_rgbText;
}

// Returns true if the control is read only
bool CAMSEdit::IsReadOnly() const
{
	return !!(GetStyle() & ES_READONLY);
}

// Returns the control's value in a valid format.
CString CAMSEdit::GetValidText() const
{
	return GetText();
}

// Redraws the window's text.
void CAMSEdit::Redraw()
{
	if (!::IsWindow(m_hWnd))
		return;

	CString strText = GetValidText();
	if (strText != GetText())
		SetWindowText(strText);
}

// Returns true if the given character should be entered into the control.
bool CAMSEdit::ShouldEnter(TCHAR c) const
{
	return true;
}

// Cuts the current selection into the clipboard.
LONG CAMSEdit::OnCut(UINT, LONG)
{
	int nStart, nEnd;
	GetSel(nStart, nEnd);

	if (nStart < nEnd)
	{
		SendMessage(WM_COPY);				// copy the selection and...
		SendMessage(WM_KEYDOWN, VK_DELETE); // delete it
	}
	
	return 0;
}
	
// Clears the current selection.
LONG CAMSEdit::OnClear(UINT wParam, LONG lParam)
{
	int nStart, nEnd;
	GetSel(nStart, nEnd);

	if (nStart < nEnd)
		SendMessage(WM_KEYDOWN, VK_DELETE); // delete the selection
	
	return 0;
}

// Pastes the text from the clipboard onto the current selection.
LONG CAMSEdit::OnPaste(UINT, LONG)
{
	int nStart, nEnd;
	GetSel(nStart, nEnd);

	CEdit::Default();
	CString strText = GetValidText();

	if (strText != GetText())
	{
		SetWindowText(strText);
		SetSel(nStart, nEnd);
	}

	return 0;
}

// Handles drawing the text and background using the designated colors
BOOL CAMSEdit::OnChildNotify(UINT message, WPARAM wParam, LPARAM lParam, LRESULT* pLResult) 
{
	if ((message == WM_CTLCOLOREDIT || message == WM_CTLCOLORSTATIC) && (m_brushBackground.GetSafeHandle() || m_uInternalFlags & TextColorHasBeenSet))
	{
		CDC* pDC = CDC::FromHandle((HDC)wParam);

		if (m_rgbText)
			pDC->SetTextColor(m_rgbText);

		// Set the text background to the requested background color
		pDC->SetBkColor(GetBackgroundColor());

		*pLResult = (LRESULT)m_brushBackground.GetSafeHandle();
		return TRUE;
	}

	return CEdit::OnChildNotify(message, wParam, lParam, pLResult);
}

// Handles the WM_SETTEXT message to ensure that text (set via SetWindowText) is valid.
LONG CAMSEdit::OnSetText(UINT wParam, LONG lParam)
{
	LONG nResult = CEdit::Default();

	CString strText = GetValidText();
	if (strText != (LPCTSTR)lParam)
		SetWindowText(strText);

	return nResult;
}


/////////////////////////////////////////////////////////////////////////////
// CAMSEdit::SelectionSaver

// Constructs the selection saver object for the given edit control.
// It then saves the edit control's current selection.
CAMSEdit::SelectionSaver::SelectionSaver(CEdit* pEdit) :
	m_pEdit(pEdit)
{
	ASSERT(pEdit);
	pEdit->GetSel(m_nStart, m_nEnd);
}

// Constructs the selection saver object for the given edit control.
// It then saves the given nStart and nEnd values.
CAMSEdit::SelectionSaver::SelectionSaver(CEdit* pEdit, int nStart, int nEnd) :
	m_pEdit(pEdit),
	m_nStart(nStart),
	m_nEnd(nEnd)
{
	ASSERT(pEdit);
	ASSERT(nStart <= nEnd);
}

// Destroys the object and restores the selection to the saved start and end values.
CAMSEdit::SelectionSaver::~SelectionSaver()
{
	if (m_pEdit)
		m_pEdit->SetSel(m_nStart, m_nEnd, TRUE);
}

// Changes the start and end values to nStart and nEnd respectively.
void CAMSEdit::SelectionSaver::MoveTo(int nStart, int nEnd)
{
	ASSERT(nStart <= nEnd);

	m_nStart = nStart;
	m_nEnd = nEnd;
}

// Changes the start and end values by nStart and nEnd respectively.
void CAMSEdit::SelectionSaver::MoveBy(int nStart, int nEnd)
{
	m_nStart += nStart;
	m_nEnd += nEnd;

	ASSERT(m_nStart <= m_nEnd);
}

// Changes both the start and end values by nPos.
void CAMSEdit::SelectionSaver::MoveBy(int nPos)
{
	m_nStart += nPos;
	m_nEnd += nPos;
}

// Changes both the start and end values by nPos.
void CAMSEdit::SelectionSaver::operator+=(int nPos)
{
	MoveBy(nPos);
}

// Returns the value for the selection's start.
int CAMSEdit::SelectionSaver::GetStart() const
{
	return m_nStart;
}

// Returns the value for the selection's end.
int CAMSEdit::SelectionSaver::GetEnd() const
{
	return m_nEnd;
}

// Updates the selection's start and end with the current selection.
void CAMSEdit::SelectionSaver::Update()
{
	if (m_pEdit)
		m_pEdit->GetSel(m_nStart, m_nEnd);
}

// Disables resetting the selection in the destructor
void CAMSEdit::SelectionSaver::Disable()
{
	m_pEdit = NULL;
}


/////////////////////////////////////////////////////////////////////////////
// CAMSEdit::Behavior

// Constructs the object from the given control.
CAMSEdit::Behavior::Behavior(CAMSEdit* pEdit) :
	m_pEdit(pEdit),
	m_uFlags(0)
{
	ASSERT(m_pEdit);
}

// Destroys the object (virtual).
CAMSEdit::Behavior::~Behavior()
{
}

// Adds and removes flags from the behavior and then redraws the control
void CAMSEdit::Behavior::ModifyFlags(UINT uAdd, UINT uRemove)
{
	UINT uFlags = (m_uFlags & ~uRemove) | uAdd;

	if (m_uFlags != uFlags)
	{
		m_uFlags = uFlags;
		_Redraw();
	}
}

// Returns the flags
UINT CAMSEdit::Behavior::GetFlags() const
{
	return m_uFlags;
}

// Handles the WM_CHAR message by passing it to the control.
void CAMSEdit::Behavior::_OnChar(UINT uChar, UINT nRepCnt, UINT nFlags)
{
	m_pEdit->OnChar(uChar, nRepCnt, nFlags);
}

// Handles the WM_KEYDOWN message by passing it to the control.
void CAMSEdit::Behavior::_OnKeyDown(UINT uChar, UINT nRepCnt, UINT nFlags)
{
	m_pEdit->OnKeyDown(uChar, nRepCnt, nFlags);
}

// Handles the WM_KILLFOCUS message by passing it to the control.
void CAMSEdit::Behavior::_OnKillFocus(CWnd* pNewWnd) 
{
	m_pEdit->OnKillFocus(pNewWnd);
}

// Handles the WM_PASTE message by passing it to the control.
LONG CAMSEdit::Behavior::_OnPaste(UINT wParam, LONG lParam)
{
	return m_pEdit->OnPaste(wParam, lParam);
}

// Calls the default handler for the current message
LRESULT CAMSEdit::Behavior::_Default()
{
	return m_pEdit->Default();
}

// Redraws the control so that its value is valid
void CAMSEdit::Behavior::_Redraw()
{
	m_pEdit->Redraw();
}

// Returns true if the given character should be entered into the control.
bool CAMSEdit::Behavior::_ShouldEnter(TCHAR c) const
{
	return m_pEdit->ShouldEnter(c);
}



/////////////////////////////////////////////////////////////////////////////
// CAMSEdit::DateBehavior

// Constructs the object with the given control.
CAMSEdit::DateBehavior::DateBehavior(CAMSEdit* pEdit) :
	Behavior(pEdit),
	m_dateMin(AMS_MIN_OLEDATETIME),
	m_dateMax(AMS_MAX_OLEDATETIME),
	m_cSep('/')
{
	// Get the system's date separator
	if (::GetLocaleInfo(LOCALE_USER_DEFAULT, LOCALE_SDATE, NULL, 0))
		::GetLocaleInfo(LOCALE_USER_DEFAULT, LOCALE_SDATE, &m_cSep, sizeof(m_cSep));

	// Determine if the day should go before the month
//	TCHAR szShortDate[MAX_PATH];
//	if (::GetLocaleInfo(LOCALE_USER_DEFAULT, LOCALE_SSHORTDATE, NULL, 0))
//	{
//		::GetLocaleInfo(LOCALE_USER_DEFAULT, LOCALE_SSHORTDATE, szShortDate, sizeof(szShortDate));

//		for (int iPos = 0; szShortDate[iPos]; iPos++)
//		{
//			TCHAR c = _totupper(szShortDate[iPos]);
//			if (c == 'M')	// see if the month is first
//				break;
//			if (c == 'D')	// see if the day is first, and then set the flag
//			{
//				m_uFlags |= DayBeforeMonth;
//				break;
//			}
//		}
//	}
	m_uFlags |= DayBeforeMonth;			// ROM Sopot
}

// Handles the WM_CHAR message, which is called when the user enters a regular character or Backspace
void CAMSEdit::DateBehavior::_OnChar(UINT uChar, UINT nRepCnt, UINT nFlags) 
{
	// Check to see if it's read only
	if (m_pEdit->IsReadOnly())
		return;

	TCHAR c = static_cast<TCHAR>(uChar);

	int nStart, nEnd;
	m_pEdit->GetSel(nStart, nEnd);

	CString strText = m_pEdit->GetText();
	int nLen = strText.GetLength();

	// Check for a non-printable character (such as Ctrl+C)
	if (!_istprint(c))
	{
		if (c == VK_BACK && nStart != nLen)
		{
			m_pEdit->SendMessage(WM_KEYDOWN, VK_LEFT); // move the cursor left
			return;
		}
		
		// Allow backspace only if the cursor is all the way to the right
		if (_ShouldEnter(c))
			Behavior::_OnChar(uChar, nRepCnt, nFlags);
		return;
	}

	// Add the digit depending on its location
	switch (nStart)
	{
		case 0:		// FIRST DIGIT
		{
			if (m_uFlags & DayBeforeMonth)
			{
				if (IsValidDayDigit(c, 0) && _ShouldEnter(c))
				{
					if (nLen > nStart)
					{
						m_pEdit->SetSel(nStart, nStart + 1);
						m_pEdit->ReplaceSel(CString(c), TRUE);

						if (nLen > nStart + 1)
						{
							if (!IsValidDay(GetDay()))
							{
								m_pEdit->SetSel(nStart + 1, nStart + 2);
								m_pEdit->ReplaceSel(CString(GetMinDayDigit(1)), TRUE);
								m_pEdit->SetSel(nStart + 1, nStart + 2);
							}
						}
					}
					else
						Behavior::_OnChar(uChar, nRepCnt, nFlags);
				}
				// Check if we can insert the digit with a leading zero
				else if (nLen == nStart && GetMinDayDigit(0) == '0' && IsValidDayDigit(c, 1) && _ShouldEnter(c))
				{
					m_pEdit->SetSel(nStart, nStart + 2);
					m_pEdit->ReplaceSel(CString('0') + c, TRUE);					
				}
			}
			else
			{
				if (IsValidMonthDigit(c, 0) && _ShouldEnter(c))
				{
					if (nLen > nStart)
					{
						m_pEdit->SetSel(nStart, nStart + 1);
						m_pEdit->ReplaceSel(CString(c), TRUE);

						if (nLen > nStart + 1)
						{
							if (!IsValidMonth(GetMonth()))
							{
								m_pEdit->SetSel(nStart + 1, nStart + 2);
								m_pEdit->ReplaceSel(CString(GetMinMonthDigit(1)), TRUE);
								m_pEdit->SetSel(nStart + 1, nStart + 2);
							}
						}
						AdjustMaxDay();
					}
					else
						Behavior::_OnChar(uChar, nRepCnt, nFlags);
				}
				// Check if we can insert the digit with a leading zero
				else if (nLen == nStart && GetMinMonthDigit(0) == '0' && IsValidMonthDigit(c, 1) && _ShouldEnter(c))
				{
					m_pEdit->SetSel(nStart, nStart + 2);
					m_pEdit->ReplaceSel(CString('0') + c, TRUE);					
				}
			}
			break;
		}
		case 1:		// SECOND DIGIT
		{
			if (m_uFlags & DayBeforeMonth)
			{
				if (IsValidDayDigit(c, 1) && _ShouldEnter(c))
				{
					if (nLen > nStart)
					{
						m_pEdit->SetSel(nStart, nStart + 1);
						m_pEdit->ReplaceSel(CString(c), TRUE);
					}
					else
						Behavior::_OnChar(uChar, nRepCnt, nFlags);
				}
				// Check if it's a slash and the first digit (preceded by a zero) is a valid month
				else if (c == m_cSep && nLen == nStart && GetMinDayDigit(0) == '0' && IsValidDay(_ttoi(CString('0') + strText[0])) && _ShouldEnter(c))
				{
					m_pEdit->SetSel(0, nStart);
					m_pEdit->ReplaceSel(CString('0') + strText[0] + c, TRUE);					
				}
			}
			else
			{
				if (IsValidMonthDigit(c, 1) && _ShouldEnter(c))
				{
					if (nLen > nStart)
					{
						m_pEdit->SetSel(nStart, nStart + 1);
						m_pEdit->ReplaceSel(CString(c), TRUE);

						if (GetDay() > 0 && AdjustMaxDay())
							m_pEdit->SetSel(GetDayStartPosition(), GetDayStartPosition() + 2);
					}
					else
						Behavior::_OnChar(uChar, nRepCnt, nFlags);
				}				
				// Check if it's a slash and the first digit (preceded by a zero) is a valid month
				else if (c == m_cSep && nLen == nStart && GetMinMonthDigit(0) == '0' && IsValidMonth(_ttoi(CString('0') + strText[0])) && _ShouldEnter(c))
				{
					m_pEdit->SetSel(0, nStart);
					m_pEdit->ReplaceSel(CString('0') + strText[0] + c, TRUE);					
				}
			}
			break;
		}
		
		case 2:		// FIRST SLASH
		{
			int nSlash = 0;
			if (c == m_cSep)
				nSlash = 1;
			else
			{
				if (m_uFlags & DayBeforeMonth)
					nSlash = (IsValidMonthDigit(c, 0) || (nLen == nStart && GetMinMonthDigit(0) == '0' && IsValidMonthDigit(c, 1))) ? 2 : 0;
				else
					nSlash = (IsValidDayDigit(c, 0) || (nLen == nStart && GetMinDayDigit(0) == '0' && IsValidDayDigit(c, 1))) ? 2 : 0;
			}

			// If we need the slash, enter it
			if (nSlash && _ShouldEnter(c))
			{
				m_pEdit->SetSel(nStart, nStart + 1, FALSE);
				m_pEdit->ReplaceSel(CString(m_cSep), TRUE);
			}

			// If the slash is to be preceded by a valid digit, "type" it in.
			if (nSlash == 2)
				keybd_event((BYTE)c, 0, 0, 0);
			break;
		}

		case 3:		// THIRD DIGIT
		{
			if (m_uFlags & DayBeforeMonth)
			{
				if (IsValidMonthDigit(c, 0) && _ShouldEnter(c))
				{
					if (nLen > nStart)
					{
						m_pEdit->SetSel(nStart, nStart + 1);
						m_pEdit->ReplaceSel(CString(c), TRUE);

						if (nLen > nStart + 1)
						{
							if (!IsValidMonth(GetMonth()))
							{
								m_pEdit->SetSel(nStart + 1, nStart + 2);
								m_pEdit->ReplaceSel(CString(GetMinMonthDigit(1)), TRUE);
								m_pEdit->SetSel(nStart + 1, nStart + 2);
							}
						}
					}
					else
						Behavior::_OnChar(uChar, nRepCnt, nFlags);

					AdjustMaxDay();
				}
				// Check if we can insert the digit with a leading zero
				else if (nLen == nStart && GetMinMonthDigit(0) == '0' && IsValidMonthDigit(c, 1) && _ShouldEnter(c))
				{
					m_pEdit->SetSel(nStart, nStart + 2);
					m_pEdit->ReplaceSel(CString('0') + c, TRUE);					
					AdjustMaxDay();
				}
			}
			else
			{
				if (IsValidDayDigit(c, 0) && _ShouldEnter(c))
				{
					if (nLen > nStart)
					{
						m_pEdit->SetSel(nStart, nStart + 1);
						m_pEdit->ReplaceSel(CString(c), TRUE);

						if (nLen > nStart + 1)
						{
							if (!IsValidDay(GetDay()))
							{
								m_pEdit->SetSel(nStart + 1, nStart + 2);
								m_pEdit->ReplaceSel(CString(GetMinDayDigit(1)), TRUE);
								m_pEdit->SetSel(nStart + 1, nStart + 2);
							}
						}
					}
					else
						Behavior::_OnChar(uChar, nRepCnt, nFlags);
				}
				// Check if we can insert the digit with a leading zero
				else if (nLen == nStart && GetMinDayDigit(0) == '0' && IsValidDayDigit(c, 1) && _ShouldEnter(c))
				{
					m_pEdit->SetSel(nStart, nStart + 2);
					m_pEdit->ReplaceSel(CString('0') + c, TRUE);					
				}
			}
			break;			
		}

		case 4:		// FOURTH DIGIT
		{
			if (m_uFlags & DayBeforeMonth)
			{
				if (IsValidMonthDigit(c, 1) && _ShouldEnter(c))
				{
					if (nLen > nStart)
					{
						m_pEdit->SetSel(nStart, nStart + 1);
						m_pEdit->ReplaceSel(CString(c), TRUE);

						if (GetDay() > 0 && AdjustMaxDay())
							m_pEdit->SetSel(GetDayStartPosition(), GetDayStartPosition() + 2);
					}
					else
					{
						Behavior::_OnChar(uChar, nRepCnt, nFlags);
						AdjustMaxDay();
					}
				}
				// Check if it's a slash and the first digit (preceded by a zero) is a valid month
				else if (c == m_cSep && nLen == nStart && GetMinMonthDigit(0) == '0' && IsValidMonth(_ttoi(CString('0') + strText[3])) && _ShouldEnter(c))
				{
					m_pEdit->SetSel(3, nStart);
					m_pEdit->ReplaceSel(CString('0') + strText[3] + c, TRUE);					
				}
			}
			else
			{
				if (IsValidDayDigit(c, 1) && _ShouldEnter(c))
				{
					if (nLen > nStart)
					{
						m_pEdit->SetSel(nStart, nStart + 1);
						m_pEdit->ReplaceSel(CString(c), TRUE);
					}
					else
						Behavior::_OnChar(uChar, nRepCnt, nFlags);
				}
				// Check if it's a slash and the first digit (preceded by a zero) is a valid month
				else if (c == m_cSep && nLen == nStart && GetMinDayDigit(0) == '0' && IsValidDay(_ttoi(CString('0') + strText[3])) && _ShouldEnter(c))
				{
					m_pEdit->SetSel(3, nStart);
					m_pEdit->ReplaceSel(CString('0') + strText[3] + c, TRUE);					
				}
			}
			break;
		}

		case 5:		// SECOND SLASH	(year's first digit)
		{
			int nSlash = 0;
			if (c == m_cSep)
				nSlash = 1;
			else
				nSlash = (IsValidYearDigit(c, 0) ? 2 : 0);

			// If we need the slash, enter it
			if (nSlash && _ShouldEnter(c))
			{
				m_pEdit->SetSel(nStart, nStart + 1, FALSE);
				m_pEdit->ReplaceSel(CString(m_cSep), TRUE);
			}

			// If the slash is to be preceded by a valid digit, "type" it in.
			if (nSlash == 2)
				keybd_event((BYTE)c, 0, 0, 0);
			break;			
		}

		case 6:		// YEAR (all 4 digits)
		case 7:
		case 8:
		case 9:
		{
			if (IsValidYearDigit(c, nStart - GetYearStartPosition()) && _ShouldEnter(c))
			{
				if (nLen > nStart)
				{
					m_pEdit->SetSel(nStart, nStart + 1, FALSE);
					m_pEdit->ReplaceSel(CString(c), TRUE);

					for (; nStart + 1 < nLen && nStart < 9; nStart++)
					{
						if (!IsValidYearDigit(strText[nStart + 1], nStart - (GetYearStartPosition() - 1)))
						{
							m_pEdit->SetSel(nStart + 1, 10, FALSE);
							CString strPortion;
							for (int iPos = nStart + 1; iPos < nLen && iPos < 10; iPos++)
								strPortion += GetMinYearDigit(iPos - GetYearStartPosition(), false);
							
							m_pEdit->ReplaceSel(strPortion, TRUE);
							m_pEdit->SetSel(nStart + 1, 10, FALSE);
							break;
						}
					}
				}
				else
					Behavior::_OnChar(uChar, nRepCnt, nFlags);

				if (IsValidYear(GetYear()))
				{
					AdjustMaxDay();			// adjust the day first
					AdjustMaxMonthAndDay();	// then adjust the month and the day, if necessary
				}
			}
			break;
		}
	}
}

// Handles the WM_KEYDOWN message, which is called when the user enters a special character such as Delete or the arrow keys.
void CAMSEdit::DateBehavior::_OnKeyDown(UINT uChar, UINT nRepCnt, UINT nFlags) 
{
	switch (uChar)
	{
		case VK_BACK:
      {
        int nStart, nEnd;
        m_pEdit->GetSel(nStart, nEnd);

        if (nStart!=0) nStart-=1;
        m_pEdit->SetSel(nStart, nEnd);
        break;
      
      }
    case VK_DELETE:
		{
			// If deleting make sure it's the last character or that
			// the selection goes all the way to the end of the text
      
			int nStart, nEnd;
			m_pEdit->GetSel(nStart, nEnd);
      CString strText=m_pEdit->GetText();
      int nLen = strText.GetLength();
      
      if (nEnd!=nLen) 
      {
        if (strText.GetAt(nEnd)==m_cSep) nEnd++;
        nEnd++; 
      }
      if ((nEnd - nStart) == nLen)
      {
        m_pEdit->SetText("");
        return;
      }
      
      m_pEdit->SetSel(nStart, nEnd);
      
			return;
		}

		case VK_UP:
		{
			// If pressing the UP arrow, increment the corresponding value.

			int nStart, nEnd;
			m_pEdit->GetSel(nStart, nEnd);

			if (nStart >= GetYearStartPosition() && nStart <= GetYearStartPosition() + 4)
			{
				int nYear = GetYear();
				if (nYear >= m_dateMin.GetYear() && nYear < m_dateMax.GetYear())
					SetYear(++nYear);
			}

			else if (nStart >= GetMonthStartPosition() && nStart <= GetMonthStartPosition() + 2)
			{
				int nMonth = GetMonth();
				if (nMonth >= GetMinMonth() && nMonth < GetMaxMonth())
					SetMonth(++nMonth);
			}

			else if (nStart >= GetDayStartPosition() && nStart <= GetDayStartPosition() + 2)
			{
				int nDay = GetDay();
				if (nDay >= GetMinDay() && nDay < GetMaxDay())
					SetDay(++nDay);
			}
			
			return;
		}

		case VK_DOWN:
		{
			// If pressing the DOWN arrow, decrement the corresponding value.

			int nStart, nEnd;
			m_pEdit->GetSel(nStart, nEnd);

			if (nStart >= GetYearStartPosition() && nStart <= GetYearStartPosition() + 4)
			{
				int nYear = GetYear();
				if (nYear > m_dateMin.GetYear())
					SetYear(--nYear);
			}

			else if (nStart >= GetMonthStartPosition() && nStart <= GetMonthStartPosition() + 2)
			{
				int nMonth = GetMonth();
				if (nMonth > GetMinMonth())
					SetMonth(--nMonth);
			}

			else if (nStart >= GetDayStartPosition() && nStart <= GetDayStartPosition() + 2)
			{
				int nDay = GetDay();
				if (nDay > GetMinDay())
					SetDay(--nDay);
			}
			
			return;
		}
	}

	Behavior::_OnKeyDown(uChar, nRepCnt, nFlags);
}

// Handles the WM_KILLFOCUS message, which is called when the user leaves the control.
// It's used here to check if any action needs to be taken based on the control's value.
void CAMSEdit::DateBehavior::_OnKillFocus(CWnd* pNewWnd) 
{
	Behavior::_OnKillFocus(pNewWnd);

	// Check if any of the OnKillFocus flags is set
	if (!(m_uFlags & OnKillFocus_Max))
		return;

	CString strText = m_pEdit->GetText();

	// If it's empty, take action based on the flag
	if (strText.IsEmpty())
	{
		if (m_uFlags & OnKillFocus_Beep_IfEmpty)
			MessageBeep(MB_ICONEXCLAMATION);
			
		if (m_uFlags & OnKillFocus_SetValid_IfEmpty)
			m_pEdit->SetWindowText(_T(" "));

		if ((m_uFlags & OnKillFocus_ShowMessage_IfEmpty) == OnKillFocus_ShowMessage_IfEmpty)
			ShowErrorMessage();		

		if (m_uFlags & OnKillFocus_SetFocus_IfEmpty)
			m_pEdit->SetFocus();

		return;
	}
		
	if (!IsValid())
	{
		if (m_uFlags & OnKillFocus_Beep_IfInvalid)
			MessageBeep(MB_ICONEXCLAMATION);
			
		if (m_uFlags & OnKillFocus_SetValid_IfInvalid)
			_Redraw();
		
		if ((m_uFlags & OnKillFocus_ShowMessage_IfInvalid) == OnKillFocus_ShowMessage_IfInvalid)
			ShowErrorMessage();		

		if (m_uFlags & OnKillFocus_SetFocus_IfInvalid)
			m_pEdit->SetFocus();
	}
}

// Returns the given value as a string with or without leading zeros.
CString CAMSEdit::DateBehavior::GetString(int nValue, bool bTwoDigitWithLeadingZero /*= true*/)
{
	CString strValue;
	if (bTwoDigitWithLeadingZero)
		strValue.Format(_T("%02d"), nValue);
	else
		strValue.Format(_T("%d"), nValue);
	return strValue;
}

// Returns the zero-based position of the month inside the control.
// This is based on whether the month is shown before or after the day.
int CAMSEdit::DateBehavior::GetMonthStartPosition() const
{
	return ((m_uFlags & DayBeforeMonth) ? 3 : 0);
}

// Returns the zero-based position of the day inside the control.
// This is based on whether the day is shown before or after the month.
int CAMSEdit::DateBehavior::GetDayStartPosition() const
{
	return ((m_uFlags & DayBeforeMonth) ? 0 : 3);
}

// Returns the zero-based position of the year inside the control.
int CAMSEdit::DateBehavior::GetYearStartPosition() const
{
	return 6;
}

// Returns the maximum value for the month based on the allowed range.
int CAMSEdit::DateBehavior::GetMaxMonth() const
{
	if (GetValidYear() == m_dateMax.GetYear())
		return m_dateMax.GetMonth();
	return 12;
}

// Returns the minimum value for the month based on the allowed range.
int CAMSEdit::DateBehavior::GetMinMonth() const
{
	if (GetValidYear() == m_dateMin.GetYear())
		return m_dateMin.GetMonth();
	return 1;
}

// Returns the maximum value for the day based on the allowed range.
int CAMSEdit::DateBehavior::GetMaxDay() const
{
	int nYear = GetValidYear();
	int nMonth = GetValidMonth();

	if (nYear == m_dateMax.GetYear() && nMonth == m_dateMax.GetMonth())
		return m_dateMax.GetDay();

	return GetMaxDayOfMonth(nMonth, nYear);
}

// Returns the minimum value for the day based on the allowed range.
int CAMSEdit::DateBehavior::GetMinDay() const
{
	int nYear = GetValidYear();
	int nMonth = GetValidMonth();

	if (nYear == m_dateMin.GetYear() && nMonth == m_dateMin.GetMonth())
		return m_dateMin.GetDay();

	return 1;
}

// Returns the maximum value for the day based on the given month and year.
int CAMSEdit::DateBehavior::GetMaxDayOfMonth(int nMonth, int nYear)
{
	ASSERT(nMonth >= 1 && nMonth <= 12);

	switch (nMonth)
	{
		case 4:
		case 6:
		case 9:
		case 11:
			return 30;

		case 2:
			return IsLeapYear(nYear) ? 29 : 28;
	}
	return 31;
}

// Returns the digit at the given position (0 or 1) for the maximum value of the month allowed.
TCHAR CAMSEdit::DateBehavior::GetMaxMonthDigit(int nPos) const
{
	ASSERT(nPos >= 0 && nPos <= 1);

	int nYear = GetValidYear();
	int nMaxMonth = m_dateMax.GetMonth();
	int nMaxYear = m_dateMax.GetYear();

	// First digit
	if (nPos == 0)
	{
		// If the year is at the max, then use the first digit of the max month
		if (nYear == nMaxYear)
			return GetString(nMaxMonth)[0];

		// Otherwise, it's always '1'
		return '1';
	}

	// Second digit
	CString strText = m_pEdit->GetText();
	TCHAR cFirstDigit = (strText.GetLength() > GetMonthStartPosition()) ? strText[GetMonthStartPosition()] : '0';
	ASSERT(cFirstDigit);  // must have a valid first digit at this point

	// If the year is at the max, then check if the first digits match
	if (nYear == nMaxYear && (IsValidYear(GetYear()) || nMaxYear == m_dateMin.GetYear()))
	{
		// If the first digits match, then use the second digit of the max month
		if (GetString(nMaxMonth)[0] == cFirstDigit)
			return GetString(nMaxMonth)[1];

		// Assuming the logic for the first digit is correct, then it must be '0'
		ASSERT(cFirstDigit == '0');
		return '9';  
	}

	// Use the first digit to determine the second digit's max
	return (cFirstDigit == '1' ? '2' : '9');
}

// Returns the digit at the given position (0 or 1) for the minimum value of the month allowed.
TCHAR CAMSEdit::DateBehavior::GetMinMonthDigit(int nPos) const
{
	ASSERT(nPos >= 0 && nPos <= 1);

	int nYear = GetValidYear();
	int nMinMonth = m_dateMin.GetMonth();
	int nMinYear = m_dateMin.GetYear();

	// First digit
	if (nPos == 0)
	{
		// If the year is at the min, then use the first digit of the min month
		if (nYear == nMinYear)
			return GetString(nMinMonth)[0];

		// Otherwise, it's always '0'
		return '0';
	}

	// Second digit
	CString strText = m_pEdit->GetText();
	TCHAR cFirstDigit = (strText.GetLength() > GetMonthStartPosition()) ? strText[GetMonthStartPosition()] : '0';
	if (!cFirstDigit)
		return '1';

	// If the year is at the max, then check if the first digits match
	if (nYear == nMinYear && (IsValidYear(GetYear()) || nMinYear == m_dateMax.GetYear()))
	{
		// If the first digits match, then use the second digit of the max month
		if (GetString(nMinMonth)[0] == cFirstDigit)
			return GetString(nMinMonth)[1];

		return '0';  
	}

	// Use the first digit to determine the second digit's min
	return (cFirstDigit == '1' ? '0' : '1');
}

// Returns true if the digit at the given position (0 or 1) is within the allowed range for the month.
bool CAMSEdit::DateBehavior::IsValidMonthDigit(TCHAR c, int nPos) const
{
	return (c >= GetMinMonthDigit(nPos) && c <= GetMaxMonthDigit(nPos));
}

// Returns true if the given month is valid and falls within the range.
bool CAMSEdit::DateBehavior::IsValidMonth(int nMonth) const
{
	int nYear = GetValidYear();
	int nDay = GetValidDay();
	return IsValid(COleDateTime(nYear, nMonth, nDay, 0, 0, 0));
}

// Returns the digit at the given position (0 or 1) for the maximum value of the day allowed.
TCHAR CAMSEdit::DateBehavior::GetMaxDayDigit(int nPos) const
{
	ASSERT(nPos >= 0 && nPos <= 1);

	int nMonth = GetValidMonth();
	int nYear = GetValidYear();
	int nMaxDay = m_dateMax.GetDay();
	int nMaxMonth = m_dateMax.GetMonth();
	int nMaxYear = m_dateMax.GetYear();

	// First digit
	if (nPos == 0)
	{
		// If the year and month are at the max, then use the first digit of the max day
		if (nYear == nMaxYear && nMonth == nMaxMonth)
			return GetString(nMaxDay)[0];
		return GetString(GetMaxDayOfMonth(nMonth, nYear))[0];
	}

	// Second digit
	CString strText = m_pEdit->GetText();
	TCHAR cFirstDigit = (strText.GetLength() > GetDayStartPosition()) ? strText[GetDayStartPosition()] : '0';
	ASSERT(cFirstDigit);  // must have a valid first digit at this point

	// If the year and month are at the max, then use the second digit of the max day
	if (nYear == nMaxYear && nMonth == nMaxMonth && GetString(nMaxDay)[0] == cFirstDigit)
		return GetString(nMaxDay)[1];

	if (cFirstDigit == '0' || 
		cFirstDigit == '1' || 
		(cFirstDigit == '2' && nMonth != 2) || 
		(nMonth == 2 && !IsValidYear(GetYear())))
		return '9';
	return GetString(GetMaxDayOfMonth(nMonth, nYear))[1];
}

// Returns the digit at the given position (0 or 1) for the minimum value of the day allowed.
TCHAR CAMSEdit::DateBehavior::GetMinDayDigit(int nPos) const
{
	ASSERT(nPos >= 0 && nPos <= 1);

	int nMonth = GetValidMonth();
	int nYear = GetValidYear();
	int nMinDay = m_dateMin.GetDay();
	int nMinMonth = m_dateMin.GetMonth();
	int nMinYear = m_dateMin.GetYear();

	// First digit
	if (nPos == 0)
	{
		// If the year and month are at the min, then use the first digit of the min day
		if (nYear == nMinYear && nMonth == nMinMonth)
			return GetString(nMinDay)[0];
		return '0';
	}

	// Second digit
	CString strText = m_pEdit->GetText();
	TCHAR cFirstDigit = (strText.GetLength() > GetDayStartPosition()) ? strText[GetDayStartPosition()] : '0';
	if (!cFirstDigit)  // must have a valid first digit at this point
		return '1';

	// If the year and month are at the max, then use the first second of the max day
	if (nYear == nMinYear && nMonth == nMinMonth && GetString(nMinDay)[0] == cFirstDigit)
		return GetString(nMinDay)[1];

	// Use the first digit to determine the second digit's min
	return (cFirstDigit == '0' ? '1' : '0');
}

// Returns true if the digit at the given position (0 or 1) is within the allowed range for the day.
bool CAMSEdit::DateBehavior::IsValidDayDigit(TCHAR c, int nPos) const
{
	return (c >= GetMinDayDigit(nPos) && c <= GetMaxDayDigit(nPos));
}

// Returns true if the given day is valid and falls within the range.
bool CAMSEdit::DateBehavior::IsValidDay(int nDay) const
{
	return IsValid(COleDateTime(GetValidYear(), GetValidMonth(), nDay, 0, 0, 0));
}

// Returns true if the given year is valid and falls within the range.
bool CAMSEdit::DateBehavior::IsValidYear(int nYear) const
{
	return (nYear >= m_dateMin.GetYear() && nYear <= m_dateMax.GetYear());
}

// Adjusts the month (to the minimum) if not valid; otherwise adjusts the day (to the maximum) if not valid.
bool CAMSEdit::DateBehavior::AdjustMaxMonthAndDay()
{
	int nMonth = GetMonth();	
	if (nMonth && !IsValidMonth(nMonth))
	{
		SetMonth(GetMinMonth());  // this adjusts the day automatically
		return true;
	}

	return AdjustMaxDay();
}

// Adjusts the day (to the maximum) if not valid.
bool CAMSEdit::DateBehavior::AdjustMaxDay()
{
	int nDay = GetDay();
	if (nDay && !IsValidDay(nDay))
	{
		SetDay(GetMaxDay());
		return true;
	}
	
	return false;	// nothing had to be adjusted
}

// Returns the digit at the given position (0 to 3) for the maximum value of the year allowed.
TCHAR CAMSEdit::DateBehavior::GetMaxYearDigit(int nPos) const
{
	ASSERT(nPos >= 0 && nPos <= 3);

	CString strYear = GetString(GetYear(), false);
	CString strMaxYear = GetString(m_dateMax.GetYear(), false);

	if (nPos == 0 || _ttoi(strMaxYear.Left(nPos)) <= _ttoi(strYear.Left(nPos)))
		return strMaxYear[nPos];
	return '9';
}

// Returns the digit at the given position (0 to 3) for the minimum value of the year allowed.
// If bValidYear is true, the current year is made sure to be valid.
TCHAR CAMSEdit::DateBehavior::GetMinYearDigit(int nPos, bool bValidYear /*= false*/) const
{
	ASSERT(nPos >= 0 && nPos <= 3);

	int nYear = GetYear();
	if (!IsValidYear(nYear) && bValidYear)
		nYear = GetValidYear();

	CString strYear = GetString(nYear, false);
	CString strMinYear = GetString(m_dateMin.GetYear(), false);

	if (nPos == 0 || _ttoi(strMinYear.Left(nPos)) >= _ttoi(strYear.Left(nPos)))
		return strMinYear[nPos];
	return '0';
}

// Returns true if the digit at the given position (0 to 3) is within the allowed range for the year.
bool CAMSEdit::DateBehavior::IsValidYearDigit(TCHAR c, int nPos) const
{
	return (c >= GetMinYearDigit(nPos) && c <= GetMaxYearDigit(nPos));
}

// Returns the month currently shown on the control or 0.
int CAMSEdit::DateBehavior::GetMonth() const
{
	CString strText = m_pEdit->GetText();

	int nStartPos = GetMonthStartPosition();
	if (strText.GetLength() >= nStartPos + 2)
		return _ttoi(strText.Mid(nStartPos, nStartPos + 2));
	return 0;
}

// Returns the current month as a valid value.  
// If it is less than the minimum allowed, the minimum is returned; 
// if it is more than the maximum allowed, the maximum is returned.
int CAMSEdit::DateBehavior::GetValidMonth() const
{
	int nMonth = GetMonth();
	
	// It it's outside the range, fix it
	if (nMonth < GetMinMonth())
		nMonth = GetMinMonth();
	else if (nMonth > GetMaxMonth())
		nMonth = GetMaxMonth();

	return nMonth;
}

// Returns the day currently shown on the control or 0.
int CAMSEdit::DateBehavior::GetDay() const
{
	CString strText = m_pEdit->GetText();

	int nStartPos = GetDayStartPosition();
	if (strText.GetLength() >= nStartPos + 2)
		return _ttoi(strText.Mid(nStartPos, nStartPos + 2));
	return 0;
}

// Returns the current day as a valid value.  
// If it is less than the minimum allowed, the minimum is returned; 
// if it is more than the maximum allowed, the maximum is returned.
int CAMSEdit::DateBehavior::GetValidDay() const
{
	int nDay = GetDay();

	// It it's outside the range, fix it
	if (nDay < GetMinDay())
		nDay = GetMinDay();
	else if (nDay > GetMaxDay())
		nDay = GetMaxDay();

	return nDay;
}

// Returns the year currently shown on the control or 0.
int CAMSEdit::DateBehavior::GetYear() const
{
	CString strText = m_pEdit->GetText();

	int nSlash = strText.ReverseFind(m_cSep);
	if (nSlash > 0)
		return _ttoi(strText.Mid(nSlash + 1));
	return 0;
}

// Returns the current year as a valid value.  
// If it is less than the minimum allowed, the minimum is returned; 
// if it is more than the maximum allowed, the maximum is returned.
int CAMSEdit::DateBehavior::GetValidYear() const
{
	int nYear = GetYear();
	if (nYear < m_dateMin.GetYear())
	{
		nYear = COleDateTime::GetCurrentTime().GetYear();
		if (nYear < m_dateMin.GetYear())
			nYear = m_dateMin.GetYear();
	}
	if (nYear > m_dateMax.GetYear())
		nYear = m_dateMax.GetYear();

	return nYear;
}

// Sets the control's month to the given value, which must be valid.
void CAMSEdit::DateBehavior::SetMonth(int nMonth)
{
	SelectionSaver selection = m_pEdit;	// remember the current selection
	
	if (GetMonth() > 0)		// see if there's already a month
		m_pEdit->SetSel(GetMonthStartPosition(), GetMonthStartPosition() + 3);

	CString strText;
	strText.Format(_T("%02d%c"), nMonth, m_cSep);
	m_pEdit->ReplaceSel(strText, TRUE);	// set the month

	AdjustMaxDay();	// adjust the day if it's out of range
	ASSERT(IsValidMonth(nMonth));
}

// Sets the control's day to the given value, which must be valid.
void CAMSEdit::DateBehavior::SetDay(int nDay)
{
	ASSERT(IsValidDay(nDay));

	SelectionSaver selection = m_pEdit;	// remember the current selection
	
	if (GetDay() > 0)		// see if there's already a day
		m_pEdit->SetSel(GetDayStartPosition(), GetDayStartPosition() + 3);

	CString strText;
	strText.Format(_T("%02d%c"), nDay, m_cSep);

	m_pEdit->ReplaceSel(strText, TRUE);	// set the day
}

// Sets the control's year to the given value, which must be valid.
void CAMSEdit::DateBehavior::SetYear(int nYear)
{
	ASSERT(IsValidYear(nYear));

	SelectionSaver selection = m_pEdit;	// remember the current selection

	if (GetYear() > 0)		// see if there's already a year
		m_pEdit->SetSel(GetYearStartPosition(), GetYearStartPosition() + 4);

	CString strText;
	strText.Format(_T("%4d"), nYear);
	m_pEdit->ReplaceSel(strText, TRUE);	// set the year
	
	AdjustMaxMonthAndDay();	// adjust the month and/or day if they're out of range
}

// Returns the date on the control as a CTime object.
CTime CAMSEdit::DateBehavior::GetDate() const
{
	return CTime(GetYear(), GetMonth(), GetDay(), 0, 0, 0);
}

// Returns the date on the control as a COleDateTime object.
COleDateTime CAMSEdit::DateBehavior::GetOleDate() const
{
	return COleDateTime(GetYear(), GetMonth(), GetDay(), 0, 0, 0);
}

// Sets the month, day, and year on the control to the given values, which must be valid.
void CAMSEdit::DateBehavior::SetDate(int nYear, int nMonth, int nDay)
{
	ASSERT(IsValid(COleDateTime(nYear, nMonth, nDay, 0, 0, 0))); 
	m_pEdit->SetWindowText(GetFormattedDate(nYear, nMonth, nDay));
}

// Sets the month, day, and year on the control based on the given CTime object, which must be valid.
void CAMSEdit::DateBehavior::SetDate(const CTime& date)
{
	SetDate(date.GetYear(), date.GetMonth(), date.GetDay());
}

// Sets the month, day, and year on the control based on the given COleDateTime object, which must be valid.
void CAMSEdit::DateBehavior::SetDate(const COleDateTime& date)
{
	SetDate(date.GetYear(), date.GetMonth(), date.GetDay());
}

// Sets the month, day, and year on the control to today's date.
void CAMSEdit::DateBehavior::SetDateToToday()
{
	SetDate(COleDateTime::GetCurrentTime());
}

// Returns true if the given year is a leap year.
bool CAMSEdit::DateBehavior::IsLeapYear(int nYear)
{
	return (nYear & 3) == 0 && (nYear % 100 != 0 || nYear % 400 == 0); 
}

// Returns true if the control's date is valid and falls within the allowed range.
bool CAMSEdit::DateBehavior::IsValid() const
{
	return IsValid(COleDateTime(GetYear(), GetMonth(), GetDay(), 0, 0, 0));
}

// Returns true if the control's date is valid and falls within the allowed range.
// If bShowErrorIfNotValid is true, an error message box is shown and the control gets the focus.
bool CAMSEdit::DateBehavior::CheckIfValid(bool bShowErrorIfNotValid /*= true*/)
{
	if (!m_pEdit->IsWindowEnabled())
		return true;

	bool bValid = IsValid();
	if (!bValid && bShowErrorIfNotValid)
	{
		ShowErrorMessage();
		m_pEdit->SetFocus();
	}
	
	return bValid;
}

// Shows a message box informing the user to enter a valid date within the allowed range.
void CAMSEdit::DateBehavior::ShowErrorMessage() const
{
	AfxMessageBox(_T("Please specify a date between ") + GetFormattedDate(m_dateMin.GetYear(), m_dateMin.GetMonth(), m_dateMin.GetDay()) + _T(" and ") + GetFormattedDate(m_dateMax.GetYear(), m_dateMax.GetMonth(), m_dateMax.GetDay()) + '.', MB_ICONEXCLAMATION);
}

// Sets the range of allowed date values to the given minimum and maximum CTime values.
void CAMSEdit::DateBehavior::SetRange(const CTime& dateMin, const CTime& dateMax)
{
	ASSERT(dateMin >= AMS_MIN_CTIME);
	ASSERT(dateMax <= AMS_MAX_CTIME);
	ASSERT(dateMin <= dateMax);

	m_dateMin.SetDate(dateMin.GetYear(), dateMin.GetMonth(), dateMin.GetDay());
	m_dateMax.SetDate(dateMax.GetYear(), dateMax.GetMonth(), dateMax.GetDay());

	_Redraw();
}

// Sets the range of allowed date values to the given minimum and maximum COleDateTime values.
void CAMSEdit::DateBehavior::SetRange(const COleDateTime& dateMin, const COleDateTime& dateMax)
{
	ASSERT(dateMin >= AMS_MIN_OLEDATETIME);
	ASSERT(dateMax <= AMS_MAX_OLEDATETIME);
	ASSERT(dateMin <= dateMax);

	m_dateMin = dateMin;
	m_dateMax = dateMax;
	_Redraw();
}

// Retrieves the range of allowed date values inside the given set of CTime pointers.
void CAMSEdit::DateBehavior::GetRange(CTime* pDateMin, CTime* pDateMax) const
{
	if (pDateMin)
		*pDateMin = CTime(m_dateMin.GetYear(), m_dateMin.GetMonth(), m_dateMin.GetDay(), 0, 0, 0);
	if (pDateMax)
		*pDateMax = CTime(m_dateMax.GetYear(), m_dateMax.GetMonth(), m_dateMax.GetDay(), 0, 0, 0);
}

// Retrieves the range of allowed date values inside the given set of COleDateTime pointers.
void CAMSEdit::DateBehavior::GetRange(COleDateTime* pDateMin, COleDateTime* pDateMax) const
{
	if (pDateMin)
		*pDateMin = m_dateMin;
	if (pDateMax)
		*pDateMax = m_dateMax;
}

// Returns true if the given date is valid and falls within the range.
bool CAMSEdit::DateBehavior::IsValid(const COleDateTime& date, bool bDateOnly /*= true*/) const
{
	return (date.GetStatus() == COleDateTime::valid && date >= m_dateMin && date <= m_dateMax);
}

// Sets the character used to separate the month, day, and year values.
void CAMSEdit::DateBehavior::SetSeparator(TCHAR cSep)
{
	ASSERT(cSep);
	ASSERT(!_istdigit(cSep));

	if (m_cSep != cSep)
	{
		m_cSep = cSep;
		_Redraw();
	}
}

// Returns the character used to separate the month, day, and year values.
TCHAR CAMSEdit::DateBehavior::GetSeparator() const
{
	return m_cSep;
}

// Sets whether the day should be shown before the month or after it.
void CAMSEdit::DateBehavior::ShowDayBeforeMonth(bool bDayBeforeMonth /*= true*/)
{
	ModifyFlags(bDayBeforeMonth ? DayBeforeMonth : 0, bDayBeforeMonth ? 0 : DayBeforeMonth);
}

// Returns true if the day will be shown before the month (instead of after it).
bool CAMSEdit::DateBehavior::IsDayShownBeforeMonth() const
{
	return (m_uFlags & DayBeforeMonth) ? true : false;
}

// Returns the control's value in a valid format.
CString CAMSEdit::DateBehavior::_GetValidText() const
{
	CString strText = m_pEdit->GetText();

	if (strText.IsEmpty())
		return strText;

	if (IsValid())
		return GetFormattedDate(GetYear(), GetMonth(), GetDay());

	// If the date is empty, try using today
	if (GetYear() == 0 && GetMonth() == 0 && GetDay() == 0)
		((CAMSEdit::DateBehavior*)this)->SetDateToToday();

	int nYear = GetValidYear();
	int nMonth = GetValidMonth();
	int nDay = GetValidDay();

	if (!IsValid(COleDateTime(nYear, nMonth, nDay, 0, 0, 0)))
		nMonth = GetMinMonth();

	if (!IsValid(COleDateTime(nYear, nMonth, nDay, 0, 0, 0)))
		nDay = GetMaxDay();

	return GetFormattedDate(nYear, nMonth, nDay);
}

// Formats the given year, month, and day values into a string based on the proper format.
CString CAMSEdit::DateBehavior::GetFormattedDate(int nYear, int nMonth, int nDay) const
{
	CString strText;	
	if (m_uFlags & DayBeforeMonth)
		strText.Format(_T("%02d%c%02d%c%4d"), nDay, m_cSep, nMonth, m_cSep, nYear);
	else
		strText.Format(_T("%02d%c%02d%c%4d"), nMonth, m_cSep, nDay, m_cSep, nYear);

	return strText;
}


/////////////////////////////////////////////////////////////////////////////
// CAMSDateEdit

CAMSDateEdit::CAMSDateEdit() :
	DateBehavior(this),
	Behavior(this) // required because DateBehavior derives virtually from Behavior
{
}


BEGIN_MESSAGE_MAP(CAMSDateEdit, CEdit)
	//{{AFX_MSG_MAP(CAMSDateEdit)
	ON_WM_CHAR()
	ON_WM_KEYDOWN()
	ON_WM_KILLFOCUS()
	//}}AFX_MSG_MAP
	ON_MESSAGE(WM_CUT, OnCut)
	ON_MESSAGE(WM_PASTE, OnPaste)
	ON_MESSAGE(WM_CLEAR, OnClear)
	ON_MESSAGE(WM_SETTEXT, OnSetText)
END_MESSAGE_MAP()

// Returns the control's value in a valid format.
CString CAMSDateEdit::GetValidText() const
{
	return _GetValidText();
}

// Handles the WM_CHAR message, which is called when the user enters a regular character or Backspace
void CAMSDateEdit::OnChar(UINT uChar, UINT nRepCnt, UINT nFlags)
{
	_OnChar(uChar, nRepCnt, nFlags);
}

// Handles the WM_KEYDOWN message, which is called when the user enters a special character such as Delete or the arrow keys.
void CAMSDateEdit::OnKeyDown(UINT uChar, UINT nRepCnt, UINT nFlags) 
{
	_OnKeyDown(uChar, nRepCnt, nFlags);
}

// Handles the WM_KILLFOCUS message, which is called when the user leaves the control.
void CAMSDateEdit::OnKillFocus(CWnd* pNewWnd) 
{
	_OnKillFocus(pNewWnd);
}

// Handles the WM_PASTE message to ensure that the text being pasted is a valid date.
LONG CAMSDateEdit::OnPaste(UINT, LONG)
{
	return _OnPaste(0, 0);
}



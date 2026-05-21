#if !defined(AFX_AMS_EDIT_H__AC5ACB94_4363_11D3_9123_00105A6E5DE4__INCLUDED_)
#define AFX_AMS_EDIT_H__AC5ACB94_4363_11D3_9123_00105A6E5DE4__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000
// Edit.h : header file
// Created by: Alvaro Mendez - 07/17/2000
//

#include <afxwin.h>
#include <afxtempl.h>

/////////////////////////////////////////////////////////////////////////////
// CAMSEdit window

// Class CAMSEdit is the base class for all the other AMS CEdit classes.  
// It provides some base functionality to set and get the text and change
// its text and background color.
//
class CAMSEdit : public CEdit
{
public:
	// Construction/destruction
	CAMSEdit();
	virtual ~CAMSEdit();

	// Operations
	void SetText(const CString& strText);
	CString GetText() const;
	CString GetTrimmedText() const;

	void SetBackgroundColor(COLORREF rgb);
	COLORREF GetBackgroundColor() const;

	void SetTextColor(COLORREF rgb);
	COLORREF GetTextColor() const;

	bool IsReadOnly() const;
	
protected:
	virtual void Redraw();
	virtual CString GetValidText() const;
	virtual BOOL OnChildNotify(UINT message, WPARAM wParam, LPARAM lParam, LRESULT* pLResult);
	virtual bool ShouldEnter(TCHAR c) const;

protected:
	CBrush	m_brushBackground;
	COLORREF m_rgbText;

private:
	enum InternalFlags
	{
		None				= 0x0000,
		TextColorHasBeenSet = 0x0001
	};
	UINT m_uInternalFlags;

public:
	// Class SelectionSaver is used to save an edit box's current
	// selection and then restore it on destruction.
	class SelectionSaver
	{
	public:
		SelectionSaver(CEdit* pEdit);
		SelectionSaver(CEdit* pEdit, int nStart, int nEnd);
		~SelectionSaver();

		void MoveTo(int nStart, int nEnd);
		void MoveBy(int nStart, int nEnd);
		void MoveBy(int nPos);
		void operator+=(int nPos);

		int GetStart() const;
		int GetEnd() const;

		void Update();
		void Disable();

	protected:
		CEdit* m_pEdit;
		int m_nStart, m_nEnd;
	};

	// Class Behavior is an abstract base class used to define how an edit
	// box will behave when it is used.   Note that its virtual member functions start
	// with an underscore; this avoids naming conflicts when multiply inheriting.
	class Behavior
	{
	protected:
		Behavior(CAMSEdit* pEdit);
		virtual ~Behavior();

	public:
		void ModifyFlags(UINT uAdd, UINT uRemove);
		UINT GetFlags() const;

	public:
		virtual CString _GetValidText() const = 0;

		virtual void _OnChar(UINT uChar, UINT nRepCnt, UINT nFlags) = 0;
		virtual void _OnKeyDown(UINT uChar, UINT nRepCnt, UINT nFlags);
		virtual void _OnKillFocus(CWnd* pNewWnd);
		virtual LONG _OnPaste(UINT wParam, LONG lParam);

	protected:
		// Wrappers to allow access to protected members of CAMSEdit
		virtual LRESULT _Default();		
		virtual void _Redraw();
		virtual bool _ShouldEnter(TCHAR c) const;

	protected:
		CAMSEdit* m_pEdit;
		UINT m_uFlags;
	};
	friend class Behavior;



	// The DateBehavior class is used to allow the entry of date values.
	class DateBehavior : virtual public Behavior
	{
	public:
		// Construction
		DateBehavior(CAMSEdit* pEdit);

	public:
		// Operations
		void SetDate(int nYear, int nMonth, int nDay);
		void SetDate(const CTime& date);
		void SetDate(const COleDateTime& date);
		void SetDateToToday();

		CTime GetDate() const;
		COleDateTime GetOleDate() const;

		int GetYear() const;
		int GetMonth() const;
		int GetDay() const;
		void SetYear(int nYear);
		void SetMonth(int nMonth);
		void SetDay(int nDay);
		virtual bool IsValid() const;
		bool CheckIfValid(bool bShowErrorIfNotValid = true);

		void SetRange(const CTime& dateMin, const CTime& dateMax);
		void SetRange(const COleDateTime& dateMin, const COleDateTime& dateMax);
		void GetRange(CTime* pDateMin, CTime* pDateMax) const;
		void GetRange(COleDateTime* pDateMin, COleDateTime* pDateMax) const;
		void SetSeparator(TCHAR cSep);
		TCHAR GetSeparator() const;

		void ShowDayBeforeMonth(bool bDayBeforeMonth = true);
		bool IsDayShownBeforeMonth() const;

		enum Flags
		{
			None								= 0x0000,
			DayBeforeMonth						= 0x1000,

			OnKillFocus_Beep_IfInvalid			= 0x0001,
			OnKillFocus_Beep_IfEmpty			= 0x0002,
			OnKillFocus_Beep					= 0x0003,
			OnKillFocus_SetValid_IfInvalid		= 0x0004,
			OnKillFocus_SetValid_IfEmpty		= 0x0008,
			OnKillFocus_SetValid				= 0x000C,
			OnKillFocus_SetFocus_IfInvalid		= 0x0010,
			OnKillFocus_SetFocus_IfEmpty		= 0x0020,
			OnKillFocus_SetFocus				= 0x0030,
			OnKillFocus_ShowMessage_IfInvalid	= 0x0050,
			OnKillFocus_ShowMessage_IfEmpty		= 0x00A0,
			OnKillFocus_ShowMessage				= 0x00F0,
			OnKillFocus_Max						= 0x00FF
		};

	protected:
		virtual CString _GetValidText() const;
		virtual void _OnChar(UINT uChar, UINT nRepCnt, UINT nFlags);
		virtual void _OnKeyDown(UINT uChar, UINT nRepCnt, UINT nFlags);
		virtual void _OnKillFocus(CWnd* pNewWnd);

	protected:
		// Helpers
		bool AdjustMaxMonthAndDay();
		bool AdjustMaxDay();

		int GetValidMonth() const;
		int GetMaxMonth() const;
		int GetMinMonth() const;
		int GetMonthStartPosition() const;
		TCHAR GetMaxMonthDigit(int nPos) const;
		TCHAR GetMinMonthDigit(int nPos) const;
		bool IsValidMonthDigit(TCHAR c, int nPos) const;
		bool IsValidMonth(int nMonth) const;

		int GetValidDay() const;
		int GetMaxDay() const;
		int GetMinDay() const;
		int GetDayStartPosition() const;
		TCHAR GetMaxDayDigit(int nPos) const;
		TCHAR GetMinDayDigit(int nPos) const;
		bool IsValidDayDigit(TCHAR c, int nPos) const;
		bool IsValidDay(int nDay) const;
		
		int GetValidYear() const;
		int GetYearStartPosition() const;
		TCHAR GetMaxYearDigit(int nPos) const;
		TCHAR GetMinYearDigit(int nPos, bool bValidYear = false) const;
		bool IsValidYearDigit(TCHAR c, int nPos) const;
		bool IsValidYear(int nYear) const;

		virtual bool IsValid(const COleDateTime& date, bool bDateOnly = true) const;
		virtual void ShowErrorMessage() const;
		CString GetFormattedDate(int nYear, int nMonth, int nDay) const;

	public:
		static bool IsLeapYear(int nYear);
		static CString GetString(int nValue, bool bTwoDigitWithLeadingZero = true);
		static int GetMaxDayOfMonth(int nMonth, int nYear);

	protected:
		// Attributes
		COleDateTime m_dateMin;
		COleDateTime m_dateMax;
		TCHAR m_cSep;
	};

	// Generated message map functions (for CAMSEdit)
protected:
	//{{AFX_MSG(CAMSEdit)
	//}}AFX_MSG
	afx_msg LONG OnCut(UINT wParam, LONG lParam);
	afx_msg LONG OnPaste(UINT wParam, LONG lParam);
	afx_msg LONG OnClear(UINT wParam, LONG lParam);
	afx_msg LONG OnSetText(UINT wParam, LONG lParam);

	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////
// CAMSDateEdit window

// The CAMSDateEdit is a CAMSEdit control which supports the DateBehavior class.
//
class CAMSDateEdit : public CAMSEdit, 
                     public CAMSEdit::DateBehavior
{
public:
	// Construction
	CAMSDateEdit();

protected:
	virtual CString GetValidText() const;

public:
// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAMSDateEdit)
	//}}AFX_VIRTUAL

	// Generated message map functions
protected:
	//{{AFX_MSG(CAMSDateEdit)
	afx_msg void OnChar(UINT uChar, UINT nRepCnt, UINT nFlags);
	afx_msg void OnKeyDown(UINT uChar, UINT nRepCnt, UINT nFlags);
	afx_msg void OnKillFocus(CWnd* pNewWnd);
	//}}AFX_MSG

	afx_msg LONG OnPaste(UINT wParam, LONG lParam);

	DECLARE_MESSAGE_MAP()
};




#define AMS_MIN_NUMBER			-1.7976931348623158e+308
#define AMS_MAX_NUMBER			 1.7976931348623158e+308
#define AMS_MIN_CTIME			CTime(1970, 1, 1, 0, 0, 0)
#define AMS_MAX_CTIME			CTime(2037, 12, 31, 23, 59, 59)
#define AMS_MIN_OLEDATETIME		COleDateTime(1900, 1, 1, 0, 0, 0)
#define AMS_MAX_OLEDATETIME		COleDateTime(9998, 12, 31, 23, 59, 59)
#define AMS_AM_SYMBOL			_T("AM")
#define AMS_PM_SYMBOL			_T("PM")


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_AMS_EDIT_H__AC5ACB94_4363_11D3_9123_00105A6E5DE4__INCLUDED_)

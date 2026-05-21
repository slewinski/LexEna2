//-----------------------------------------------------------
// 96.03.17 23:25
int GetDaysInMonth(int year,int mon);
long mkdate(struct dt& dat );
struct dt* DaysStruct(struct dt *dat, long days);
long GetStrDate(CString& strD);  // Format in str=  dd-mm-yy\0 yy-mm-dd\0

#ifndef _DATE_DEFINED
struct dt {
    int dt_mday;    /* day of the month - [1,31] */
    int dt_mon; /* months since January - [0,11] */
    int dt_year;    /* years since 1900 */
    };
#define _DATE_DEFINED
typedef long date_t;
#endif

                      // absolute time/date

/////////////////////////////////////////////////////////////////////////////
// CDate

class CDate
{
public:

// Constructors
   static CDate PASCAL GetCurrentDate();

   CDate();
   CDate(date_t date);
   CDate(int nYear, int nMonth, int nDay);
   CDate(const CDate& dateSrc);
   CDate(CString& strD);

   const CDate& operator=(const CDate& dateSrc);
   const CDate& operator=(date_t d);
// Attributes

   date_t GetDate() const;
   
// Operations
   
   // date math
   long operator-(CDate date) const;
   CDate operator-(long dateSpan) const;
   CDate operator+(long dateSpan) const;
   const CDate& operator+=(long dateSpan);
   const CDate& operator-=(long dateSpan);
   BOOL operator==(CDate date) const;
   BOOL operator!=(CDate date) const;
   BOOL operator<(CDate date) const;
   BOOL operator>(CDate date) const;
   BOOL operator<=(CDate date) const;
   BOOL operator>=(CDate date) const;

   void DateFormat(CString& strD);
    // serialization
#ifdef _DEBUG
   friend CDumpContext& AFXAPI operator<<(CDumpContext& dc, CDate date);
#endif
   friend CArchive& AFXAPI operator<<(CArchive& ar, CDate date);
   friend CArchive& AFXAPI operator>>(CArchive& ar, CDate& rdate);

private:
   date_t m_date;
};


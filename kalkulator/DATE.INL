//---------------------------------------------------------
// 96.03.17 23:22   ver 2.0

// CDate

inline CDate::CDate(date_t date)          { m_date = date; }
inline CDate::CDate(const CDate& dateSrc) { m_date = dateSrc.m_date; }
inline const CDate& CDate::operator=(const CDate& dateSrc)
        { m_date = dateSrc.m_date; return *this; }
inline const CDate& CDate::operator=(date_t t)
        { m_date = t; return *this; }
inline date_t CDate::GetDate() const  { return m_date; }
inline long CDate::operator-(CDate date) const
        { return (m_date - date.m_date); }
inline CDate CDate::operator-(long dateSpan) const
        { return CDate(m_date - dateSpan); }
inline CDate CDate::operator+(long dateSpan) const
        { return CDate(m_date + dateSpan); }
inline const CDate& CDate::operator+=(long dateSpan)
        { m_date += dateSpan; return *this; }
inline const CDate& CDate::operator-=(long dateSpan)
        { m_date -= dateSpan; return *this; }
inline BOOL CDate::operator==(CDate date) const
        { return m_date == date.m_date; }
inline BOOL CDate::operator!=(CDate date) const
        { return m_date != date.m_date; }
inline BOOL CDate::operator<(CDate date) const
        { return m_date < date.m_date; }
inline BOOL CDate::operator>(CDate date) const
        { return m_date > date.m_date; }
inline BOOL CDate::operator<=(CDate date) const
        { return m_date <= date.m_date; }
inline BOOL CDate::operator>=(CDate date) const
        { return m_date >= date.m_date; }

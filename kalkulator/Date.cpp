//--------------------------------------------------
//  99.24.01

#include "stdafx.h"
#include "date.h"
  #ifdef _DEBUG
  #undef THIS_FILE
  static char BASED_CODE THIS_FILE[] = __FILE__;
  #endif

#ifndef maxDateBufferSize
#define maxDateBufferSize   12
#endif

int DaysInMonth[2][12] = {{31,28,31,30,31,30,31,31,30,31,30,31},
                          {31,29,31,30,31,30,31,31,30,31,30,31}};

/////////////////////////////////////////////////////////////////////////////
// CDate - absolute date

CDate::CDate() { }

CDate::CDate(int nYear, int nMonth, int nDay)
{
  struct dt dat;
    ASSERT(nDay >= 1 && nDay <= 31);
  dat.dt_mday = nDay;
    ASSERT(nMonth >= 1 && nMonth <= 12);
  dat.dt_mon = nMonth - 1;        // tm_mon is 0 based
    ASSERT(nYear >= 1900);
  dat.dt_year = nYear - 1900;     // tm_year is 1900 based
  m_date = mkdate(dat);
  ASSERT(m_date != -1);       // indicates an illegal input time
}

//---------------------------------------------------
CDate::CDate(CString& strD)
{ struct dt dat;
  if (strD.GetLength()==0) { m_date=0L; return; }
  if (strD=="--------") { m_date=50000L; return; }
  if (strD[6]<'0' || strD[6]>'9')  { m_date=0L; return; }
  else if (strD[7]<'0' || strD[7]>'9')  { m_date=0L; return; }
			else  {
				dat.dt_year = (strD[6]-'0')*10+(strD[7]-'0');
				if ( dat.dt_year < 50)  dat.dt_year+=100;      // dt_year is 1900 based
			}
  if (strD[3]<'0' || strD[3]>'9') { m_date=0L; return; }
  else if (strD[4]<'0' || strD[4]>'9') { m_date=0L; return; }
       else  dat.dt_mon= (strD[3]-'0')*10+(strD[4]-'0');

  if (strD[0]<'0' || strD[0]>'9') { m_date=0L; return; }
  else if (strD[1]<'0' || strD[1]>'9') { m_date=0L; return; }
       else  dat.dt_mday= (strD[0]-'0')*10+(strD[1]-'0');
  if (dat.dt_mon<1 || dat.dt_mon>12) { m_date=0L; return; }     
  
  dat.dt_mon--;                   // tm_mon is 0 based      
  if (dat.dt_mday<1 || dat.dt_mday>GetDaysInMonth( dat.dt_year, dat.dt_mon)) 
    { m_date=0L; return; }
  m_date = mkdate(dat);
}

//---------------------------------------------------
CDate PASCAL CDate::GetCurrentDate() // return the current date
{ char buf[10];
  int y,m,d;
   _strdate(buf);
   d = (buf[6]-'0') * 10 + (buf[7]-'0');
   m = (buf[3]-'0') * 10 + (buf[4]-'0');
   y = (buf[0]-'0') * 10 + (buf[1]-'0')+1900;
	 if (y < 1950) y+=100;
   return CDate(y,m,d);
}

//---------------------------------------------------
void CDate::DateFormat(CString& strD)
{ char szBuffer[maxDateBufferSize];
  struct dt date;
  if (m_date==50000L) strD = "--------";
  else {
    DaysStruct(&date,m_date);
	if (date.dt_year >= 100) date.dt_year -= 100;
    sprintf(szBuffer,"%02u-%02u-%02u",date.dt_mday,date.dt_mon+1,date.dt_year%100);
    strD =szBuffer;
  }  
}

#ifdef _DEBUG
//---------------------------------------------------
CDumpContext& AFXAPI operator <<(CDumpContext& dc, CDate date)
{
 CString strD;
 date.DateFormat(strD);
 if (strD.GetLength()==0)
    return dc << "CDate-(invalid)";
 else
  return dc << "CDate(\"" << (const char*)strD << "\")";
}
#endif


//---------------------------------------------------
CArchive& AFXAPI operator << (CArchive& ar, CDate date)
                              { return ar << (DWORD) date.m_date; }
//---------------------------------------------------
CArchive& AFXAPI operator >>(CArchive& ar, CDate& rdate)
                              { return ar >> (DWORD&) rdate.m_date; }
//---------------------------------------------------
long GetStrDate(CString& strD)
{ struct dt dat;
  if (strD == "--------") return 50000L;
  if (strD[6]<'0' || strD[6]>'9')   return -1L;
  else if (strD[7]<'0' || strD[7]>'9')   return -1L;
			else  {
				dat.dt_year = (strD[6]-'0')*10+(strD[7]-'0');
				if ( dat.dt_year < 50)  dat.dt_year+=100;      // dt_year is 1900 based
			}
  if (strD[3]<'0' || strD[3]>'9') return -1L;
  else if (strD[4]<'0' || strD[4]>'9') return -1L;
       else  dat.dt_mon= (strD[3]-'0')*10+(strD[4]-'0');

  if (strD[0]<'0' || strD[0]>'9') return -1L;
  else if (strD[1]<'0' || strD[1]>'9') return -1L;
       else  dat.dt_mday= (strD[0]-'0')*10+(strD[1]-'0');
  if (dat.dt_mon<1 || dat.dt_mon>12) return -1L;
  
  dat.dt_mon--;                   // tm_mon is 0 based      
  if (dat.dt_mday<1 || dat.dt_mday>GetDaysInMonth( dat.dt_year, dat.dt_mon)) return -1L;
  return (mkdate(dat));
}

//---------------------------------------------------
int GetDaysInMonth(int year,int mon)
{ int leap;
 if (year&3) leap=0; else leap = 1;
 return(DaysInMonth[leap][mon]);
}

//---------------------------------------------------
long mkdate(struct dt& dat )
{
long ldate_S;
int i;
  
  if (!dat.dt_mday || dat.dt_mday>GetDaysInMonth( dat.dt_year, dat.dt_mon))   return(0L);
  ldate_S = (long)(dat.dt_year)*365L;
  ldate_S+=(long)((dat.dt_year+3)/4);
  for (i=0; i<dat.dt_mon;i++)       ldate_S+=(long)GetDaysInMonth( dat.dt_year,i);
  ldate_S +=(long)(dat.dt_mday-2);
  return(ldate_S);
}

//--------------------------------------------------------
struct dt* DaysStruct(struct dt *dat, long days)
{ int i;
  int leap;
  dat->dt_year = 0; dat->dt_mon = 0; dat->dt_mday = 1;  leap = 0;  days+=1;
  if (days > 1461) {
    dat->dt_year = (int)(days/1461L);
    days-=(long)dat->dt_year*1461L;
    dat->dt_year *=4;
    if (days==0)  return dat;
  }  
  if (days>=366) {
    days-=366;(dat->dt_year)++; if (days==0)  return dat;
    while (days>=365) { days-=365;(dat->dt_year)++; if (days==0)  return dat;}
  }  
 int s;
  for (i=0; i<12;i++) {
    s = GetDaysInMonth( dat->dt_year,i);
    if (days < s) break;
    dat->dt_mon++; days-=s;
  }
  dat->dt_mday += (int)days;
  return (dat);
}

#include "date.inl"
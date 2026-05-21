// kkmobj.cpp : implementation of the CKmDoc structures classes
// 96.06.10 08:32

#include "stdafx.h"
#include "kkm.h"

#ifdef _DEBUG
#undef THIS_FILE
static char BASED_CODE THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CNal
IMPLEMENT_SERIAL(CNal, CObject,3)
//------------------------------------------------------------- Constructor
CNal::CNal() { } // This empty constructor
//------------------------------------------------------------- Serialize
void CNal::Serialize(CArchive& ar)
{ CString kw;
	if (ar.IsStoring())    {   // storing code here
    ar<<(WORD)m_nalNum;
    ar<<m_nalDate; ar<<m_nalGroszy; ar<<m_nalTyp; ar<<m_nalOkrCount;
  }
  else    {                 // loading code here
    WORD nNewNum; ar>>nNewNum;
    m_nalNum = nNewNum;
    ar>>m_nalDate;
		kw = m_nalDate.Left(2);		// 15.06.02 
		if (kw > "31" || kw == "00") 	Konwert_Yes = 1;	
		if (Konwert_Yes == 1) {
			m_nalDate = m_nalDate.Right(2) + m_nalDate.Mid(2,4) +	m_nalDate.Left(2);
		}
		ar>>m_nalGroszy; ar>>m_nalTyp; ar>>m_nalOkrCount;
  }
}

/////////////////////////////////////////////////////////////////////////////
// CWpl
IMPLEMENT_SERIAL(CWpl, CObject,3)
//--------------------------------------------------------------- Constructor
CWpl::CWpl() { }  // This empty constructor
//--------------------------------------------------------------- Serialize
void CWpl::Serialize(CArchive& ar)
{ if (ar.IsStoring()) {        // storing code here
    ar<<(WORD)m_wplNum; ar<<m_wplDate; ar<<m_wplGroszy; ar<<m_wplTyp;
  }
  else    {                    // loading code here
    WORD nNewNum; ar >> nNewNum; m_wplNum = nNewNum;
    ar >> m_wplDate; 
		if (Konwert_Yes == 1) {
			m_wplDate = m_wplDate.Right(2) + m_wplDate.Mid(2,4) +	m_wplDate.Left(2);
		}
		ar >> m_wplGroszy; ar >> m_wplTyp;
  }
}

/////////////////////////////////////////////////////////////////////////////
// COkrOd
IMPLEMENT_SERIAL(COkrOd, CObject,3)
//--------------------------------------------------------------- Constructor
COkrOd::COkrOd() { } // This empty constructor
//--------------------------------------------------------------- Serialize
void COkrOd::Serialize(CArchive& ar)
{ 
	/* dla %% umownych:
	okrStopaTyp - rodzaj odsetek: 0 -miesieczne, 1-roczne, 2-dzienne
	okrStopa - wysokosc odsetek np. 13,5% daje 135000
	okrTyp - 1 oznacza umowne
	*/
	if (ar.IsStoring())    {  // storing code here
	ar << (WORD)m_okrNum;  
	ar << (WORD)m_okrParent;
    ar << m_okrDateP;      
	ar << m_okrDateK;
    ar << m_okrStopa; 
	ar << m_okrTyp; 
	// kiedys 1*10 oznaczala umowne w tysiecznych
	ar << (long)m_okrStopaTyp*m_okrFactor*10 +10;	// czyli nowa kalkulacja, bo mnoznik >=1000
  }
  else  {                   // loading code here
    WORD nNewNum;
    DWORD l_okrStopaTyp;
	m_okrFactor=1000;  // inicjalnie "1,000"
	ar >> nNewNum; 
	m_okrNum = nNewNum; 
	ar >> nNewNum; 
	m_okrParent = nNewNum;
    ar >> m_okrDateP;
 		if (Konwert_Yes == 1) {
			m_okrDateP = m_okrDateP.Right(2) + m_okrDateP.Mid(2,4) +	m_okrDateP.Left(2);
		}
		ar >> m_okrDateK;
		if (Konwert_Yes == 1) {
			m_okrDateK = m_okrDateK.Right(2) + m_okrDateK.Mid(2,4) +	m_okrDateK.Left(2);
		}

    ar >> m_okrStopa; 
	ar >> m_okrTyp; 
	ar >> l_okrStopaTyp;// m_okrStopaTyp;
	
	if(l_okrStopaTyp <10)	{ // jezeli stara,
		m_okrStopa *= 10;	// to z tysiecznych na setne
		m_okrStopaTyp = (WORD)l_okrStopaTyp;  //chyba 0 lub 1
	}
	else {
		m_okrFactor = (l_okrStopaTyp-10)/10;// if nowa kalkulacja poprawiamy typ 1-> 10	
		m_okrStopaTyp = 1;// a typ przechowuje mnoznik w tysiecznych procenta
	}
  }
}

/////////////////////////////////////////////////////////////////////////////
// CK_Wpl
//IMPLEMENT_SERIAL(CK_Wpl, CObject,3)
IMPLEMENT_DYNCREATE(CK_Wpl, CObject)
//--------------------------------------------------------------- Constructor
CK_Wpl::CK_Wpl() { }

/////////////////////////////////////////////////////////////////////////////
// CPo_Wpl
IMPLEMENT_DYNCREATE(CPo_Wpl, CObject)
//--------------------------------------------------------------- Constructor
CPo_Wpl::CPo_Wpl() { }
//---------------------------------------------------------------
CPo_Wpl* CWpl::NewPo_Wpl(int Typ,POSITION pos, double Kwota, double KwotaDo)
{ CPo_Wpl* pWI = new CPo_Wpl();
  pWI->m_Typ = Typ;
  pWI->m_Pos = pos;
  pWI->m_Kwota = Kwota;
  pWI->m_KwotaDo = KwotaDo;
  m_Po_Wpl.AddTail(pWI);
////////////////////////////////////////////////
   switch (Typ) {
   case 0: SummaWplatKwotyGlawne+=Kwota;break;
   case 1: SummaWplatOdsetek+=Kwota;break;
   }
////////////////////////////////////////////////
  return pWI;
}
//---------------------------------------------------------------
POSITION CWpl::GetFirstPo_WplPos() { return m_Po_Wpl.GetHeadPosition(); }
//---------------------------------------------------------------
CPo_Wpl* CWpl::GetNextPo_Wpl(POSITION& pos)
   { return (CPo_Wpl*)m_Po_Wpl.GetNext(pos); }

/////////////////////////////////////////////////////////////////////////////
// CStanD
IMPLEMENT_SERIAL(CStanD, CObject,3)
//--------------------------------------------------------------- Constructor
CStanD::CStanD() { }  // This empty constructor
//--------------------------------------------------------------- Serialize
void CStanD::Serialize(CArchive& ar)
{ if (ar.IsStoring()) ar << m_stanD;   // storing code here
	else {
		ar >> m_stanD;   // loading code here
		if (Konwert_Yes == 1) {
			m_stanD = m_stanD.Right(2) + m_stanD.Mid(2,4) +	m_stanD.Left(2);
		}
	}
}

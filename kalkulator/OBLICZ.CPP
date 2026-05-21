//---------------------------------------------------------------
// Part of CKmDoc realization    -  Obliczenie
// kkmdoc.cpp : implementation of the CKmDoc class
// 11-20-96 10:47pm

#include "stdafx.h"
#include "kkm.h"
#include "getstan.h"
#include "zlgr.h"
#include "date.inl"
#include "mainfrm.h"
#include <stdlib.h>
#include <string.h>
#include <math.h>

#ifdef _DEBUG
#undef THIS_FILE
static char BASED_CODE THIS_FILE[] = __FILE__;
#endif

struct LineTime {
 long dat;
 int   typ;
 POSITION pos;
};

struct TabStan {
 long dat;
 double doN;
 double doO;
 double doK;
 double poN;
 double poO;
 double poK;
 double kw;
 BYTE  typ;
};

struct TabOstatNal {
 CString DateN;//data naleznosci libo nac.okresu odsetok
 CString DateK;//data konec.okresu odsetok 
 double OstKwota;//ostatok nesplacz.naleznosci libo odsetok,libo procent na rok.
 int TypOds;//typ odsetok:1-ustawowe,2-umowne.
 int TypNal;//typ naleznosci:1-10-standart;100-odsetki.
};
struct TabOstatNal MasOstatNal[1000],MasOstatNalI[1000];
int m_OstatNal=0;
int m_OstatNalI=0;
int CountKwot=0;
int CountKwotI=0;
CString   m_stanDate;   // Date of computing status
int m_stanActive;

double f_ods;
double precision;

struct TabStan TS[1000];       // Tabela stanow
int tsActive;
struct LineTime LnT[1000];      // Line of time points //Marzena bylo 100 bo tak poprawil Romanow z 400; ja z porwotem na 400
int LT_Count;                   // Number of time points
int LT_Wpl;                    // Active time point of Wplata

//extern int flag_typ_month;
int compareL( const void *arg1, const void *arg2 );
int compareT( const void *arg1, const void *arg2 );

/////////////////////////////////////////////////////////////////////////////
// CKmDoc

#include "obcomput.cpp"
          // void  CKmDoc::ComputeWpl()
          // void  CKmDoc::DoWpl(int OdOldWpl)
          // double CKmDoc::KwotaOds(COkrOd* pOkrOd, DWORD days)
          // BOOL  CKmDoc::RealWplaty()
          // void  CKmDoc::PoWplate()
          // void  CKmDoc::PredDoWpl(int OdOldWpl)
          // void  CKmDoc::ClearKWpl()
          // double CKmDoc::Plata(int Typ,CWpl *pWpl, double Kwota)
//---------------------------------------------------------------
POSITION CKmDoc::GetFirstStanPos()  { return m_stanList.GetHeadPosition(); }
//---------------------------------------------------------------
CString* CKmDoc::GetNextStan(POSITION& pos) { return (CString*)m_stanList.GetNext(pos); }
//---------------------------------------------------------------
void CKmDoc::GetStDt()
{ POSITION pos;
  int i;
  CGetStan StanDate;
  CStanD* pStan;
  char buf[120], c1, c2, c3;
//  LONG wrk;
  double wwrrkk;
  CString strD,strN,strO,strK,strR;
 CMainFrame* pFrameWnd = (CMainFrame*)theApp.m_pMainWnd;
 flag_typ_month=theApp.GetProfileInt("Config", "Alg",0);
 StanDate.m_Kal_Bank = flag_typ_month;
 StanDate.m_pDoc = this;
 if (m_Sprawa.GetLength()==0)  {
    m_Sprawa = GetTitle();
    lstrcpy(buf,(const char *)m_Sprawa);
    buf[lstrlen(buf)-3]=0;
    m_Sprawa = buf;
 }
 StanDate.m_Sprawa = m_Sprawa;
 if (StanDate.DoModal()!=IDOK) return;
// if (!theApp.m_nTyp)  
 flag_typ_month = StanDate.m_Kal_Bank;
 if (flag_typ_month) theApp.WriteProfileString("Config", "Alg","1");
 else               theApp.WriteProfileString("Config", "Alg","0");
 m_Sprawa = StanDate.m_Sprawa;
 m_stanActive=0;
 m_Obl.RemoveAll();
 m_oblActive=0;
 tsActive=0;

 for ( pos=GetFirstStanPos(); pos!=NULL; GetNextStan(pos)) {
    pStan = (CStanD *)m_stanList.GetAt(pos);
    m_stanDate = pStan->m_stanD;
    if (!Oblicz())  return;
    m_stanActive++;
 }
 if (theApp.m_nDoW) {
   qsort( (void *)&TS[0], (size_t)tsActive, sizeof(struct TabStan), compareT );
   m_Obl.Add (CString(""));
   m_Obl.Add (CString(""));
   m_Obl.Add (CString("**************************************************************************"));
   m_Obl.Add (CString("----------------------------- TABELA STANÓW ------------------------------"));
   m_Obl.Add (CString("--------------------------------------------------------------------------"));
   strD.LoadString(IDS_TST);
   m_Obl.Add (CString(strD));
   m_Obl.Add (CString("--------------------------------------------------------------------------"));
   for (i=0; i<tsActive; i++) {
     if (TS[i].typ == 'R' && theApp.m_nDoW_R==0) continue;
     if ((TS[i].typ == 'R') &&
         (i<tsActive-1) &&
         (TS[i+1].typ == 'R') &&
         (TS[i].dat==TS[i+1].dat)) {
           TS[i+1].doN+=TS[i].doN; TS[i+1].doK+=TS[i].doK; TS[i+1].doO+=TS[i].doO;
           TS[i+1].kw = TS[i].kw;
           continue;
     }
     strN = GetDCFormatted(TS[i].doN);
     strO = GetDCFormatted(TS[i].doO);
     strK = GetDCFormatted(TS[i].doK);
     strR = GetDCFormatted(TS[i].doN+TS[i].doO+TS[i].doK);
     CDate d(TS[i].dat);
     d.DateFormat(strD);
     if (TS[i].typ == 'S')
       wsprintf(buf,"%3u.^B(%c)^B ^B%8s^B ^B%13s^B ^B%13s^B ^B%13s^B ^B%13s^B",i+1, TS[i].typ,
                          (const char *)strD,(const char *)strN,
                          (const char *)strO,(const char *)strK,(const char *)strR);
     else if (TS[i].typ == 'R') {
       if (strN==" 0,00") strN="-  ";
       if (strK==" 0,00") strK="-  ";
       if (strO==" 0,00") strO="-  ";
       wsprintf(buf,"%3u.(%c) %8s %13s %13s %13s",i+1, TS[i].typ,
                          (const char *)strD,(const char *)strN,
                          (const char *)strO,(const char *)strK);
       strD = buf; m_Obl.Add (CString(strD));
       if (strN!="-  ") strN = GetDCFormatted(TS[i].poN);
       if (strO!="-  ") strO = GetDCFormatted(TS[i].poO);
       if (strK!="-  ") strK = GetDCFormatted(TS[i].poK);
       //wrk=(TS[i].kw - ((TS[i].doN+TS[i].doO + TS[i].doK)-(TS[i].poN + TS[i].poO + TS[i].poK)));
       wwrrkk = (TS[i].kw - ((TS[i].doN+TS[i].doO + TS[i].doK)-(TS[i].poN + TS[i].poO + TS[i].poK)));
       if ( wwrrkk>0) strR = GetDCFormatted(-wwrrkk);
       else strR = "-  ";
       //strR = GetDCFormatted(TS[i].poN+TS[i].poO+TS[i].poK);
       strD = GetDCFormatted(TS[i].kw);
       wsprintf(buf,"   %13s %13s %13s %13s %13s",
                          (const char *)strD,(const char *)strN,
                          (const char *)strO,(const char *)strK,(const char *)strR);
     }
     else {
       wsprintf(buf,"%3u.(%c) %8s %13s %13s %13s %13s",i+1, TS[i].typ,
                          (const char *)strD,(const char *)strN,
                          (const char *)strO,(const char *)strK,(const char *)strR);
       strD = buf; m_Obl.Add (CString(strD));
       strN = GetDCFormatted(TS[i].poN);
       strO = GetDCFormatted(TS[i].poO);
       strK = GetDCFormatted(TS[i].poK);
       strR = GetDCFormatted(TS[i].poN+TS[i].poO+TS[i].poK);
       strD = GetDCFormatted(TS[i].kw);
       wsprintf(buf,"   %13s %13s %13s %13s %13s",
                          (const char *)strD,(const char *)strN,
                          (const char *)strO,(const char *)strK,(const char *)strR);
     }
     strD = buf; m_Obl.Add (CString(strD));
   }
   c1 = strN.Right(1)[0];
   c2 = strO.Right(1)[0];
   c3 = strK.Right(1)[0];
	buf[0] = strR.Right(1)[0];

   strO = GetDCFormatted(TS[i-1].poO);
   wsprintf(buf,"        %%%%/dzien               ^B%13s^B",(const char *)strO);
   strD = buf; 
   if (((c1 & 1) + (c2 & 1) + (c3 & 1)& 1) - (buf[0] & 1)) {
	    sprintf(buf,"  (ró¿nice wynik. z zaokr¹g. -0.01)");
		strD += buf;   
   }
   m_Obl.Add (CString(strD));
   m_Obl.Add (CString("--------------------------------------------------------------------------"));
 }

 pFrameWnd->OnOblicz();  theApp.m_nAct = 7;
}

//---------------------------------------------------------------
void CKmDoc::OnStanDate(){ GetStDt();}

//------------------------------------------------------ Computing result
BOOL CKmDoc::Oblicz()
{
  LongDate();
  if (m_stanActive==0) { PutOblHead(); PutOblWpl(); }
  //PutOblStanDay();
  FormDoc(ComputeWpl());
  if (m_stanActive+1==m_stanList.GetCount())  PutDodatek();
  CWpl* pWpl;
  POSITION pos;
  for ( pos = GetFirstWplPos(); pos != NULL; GetNextWpl(pos)) {
     pWpl = (CWpl*)m_wplList.GetAt(pos);
     while (!pWpl->m_Po_Wpl.IsEmpty())  delete pWpl->m_Po_Wpl.RemoveHead();
  }
  delete m_wplList.RemoveTail();
  return TRUE;
}

//---------------------------------------------------------------
POSITION CKmDoc::NewWplStanu()
{ CWpl* pWplItem = new CWpl();
  pWplItem->m_wplNum = m_wplList.GetCount()+1;
  pWplItem->m_wplDate = m_stanDate;
  pWplItem->m_wplGroszy = -1L;
  pWplItem->m_wplTyp = 0;
  CDate dat(pWplItem->m_wplDate);
  pWplItem->m_LDate = dat;
  return m_wplList.AddTail(pWplItem);
}

//---------------------------------------------------------------
void CKmDoc::Put_OkrOds(int nalNum)
{
COkrOd* pOkrOd;
POSITION pos;
char buf[120];
CString strD,strD1;
  for ( pos = GetFirstOkrOdPos(); pos != NULL; GetNextOkrOd(pos)) {
    pOkrOd = (COkrOd*)m_odsList.GetAt(pos);
    if (GetStrDate(pOkrOd->m_okrDateP) >GetStrDate(m_stanDate)) continue;
    if ((int)pOkrOd->m_okrParent == nalNum) {
      if (pOkrOd->m_okrTyp ==0) {      // Ustawowe
        strD.LoadString(IDS_OOTYP0);
        sprintf(buf,"%45s - %s  %s",(const char *)pOkrOd->m_okrDateP,
                    (const char *)pOkrOd->m_okrDateK,(const char *)strD);
		if(pOkrOd->m_okrFactor != 10000) {
	        strD1 = GetStopaFormatted(pOkrOd->m_okrFactor);
			sprintf(buf+strlen(buf),"* %s", strD1);
		}
 
      }
      else {
        strD1 = GetStopaFormatted(pOkrOd->m_okrStopa);
        strD.LoadString(IDS_OWTYP0+pOkrOd->m_okrStopaTyp);
        sprintf(buf,"%45s - %s  %6s%%%% %s",(const char *)pOkrOd->m_okrDateP,
                    (const char *)pOkrOd->m_okrDateK,(const char *)strD1,(const char *)strD);
		if(pOkrOd->m_okrFactor != 10000) {
	        strD1 = GetStopaFormatted(pOkrOd->m_okrFactor);
			sprintf(buf+strlen(buf),"* %s", strD1);
		}
       }
      strD = buf;
      m_Obl.Add (CString(strD));
    }
  }
}


//------------------------------------------------
void CKmDoc::FormDoc(double Kw)
{
CString strD,strD1;
int nTypN,Fl;

CString strTyp;
double dwSum=0,dwSum1, OstNal, OstOds,AllSumNal=0,AllSumKoszty=0,AllSumOds=0,AllSumProc=0;
char buf1[120], c1, c2, c3;
CWpl* pWpl;
CNal* pNal;
CNal* pNal1;
COkrOd* pOkrOd;
CPo_Wpl* pPo_Wpl;
CK_Wpl* pKW;
CString strGrZl;
POSITION pos,posP,posN,posK;
CStanD* pStan;

  pStan = (CStanD *)m_stanList.GetTail();
  if (m_stanDate == pStan->m_stanD) Fl=1; else Fl=0;    // Fl=1  - koncowy stan
// Koszty-----------------------------------------------------------------------------
if (Fl) {
  m_Obl.Add (CString(""));
  m_Obl.Add (CString("--------------------------------- KOSZTY ---------------------------------"));
  for ( pos = GetFirstNalPos(); pos != NULL; GetNextNal(pos)) {
    pNal = (CNal*)m_nalList.GetAt(pos);
    if ((pNal->m_nalTyp>= T_KOSZTY_PROCESU && pNal->m_nalTyp<=T_KOSZTY_EKZEK) ||
			   pNal->m_nalTyp == T_KOSZTY_INNE) {
      nTypN= IDS_NTYP0+pNal->m_nalTyp;        // Koszty
      strTyp.LoadString(nTypN);
      strGrZl = GetDCFormatted(pNal->m_nalGroszy);
      sprintf(buf1,"  %-20s%13s   %s",(const char*)strTyp,
                                      (const char*)strGrZl,(const char*)pNal->m_nalDate);
      strD = buf1;
      m_Obl.Add (CString(strD));
      AllSumKoszty +=(double)pNal->m_nalGroszy;
    }
  }
  if (AllSumKoszty) {
    m_Obl.Add (CString("-------------------------------"));
    strGrZl = GetDCFormatted(AllSumKoszty);
    sprintf(buf1,"^B%13s^B", (const char*)strGrZl);
    strD ="  Razem               "; strD+=buf1;
    m_Obl.Add (CString(strD));
    m_Obl.Add (CString("--------------------------------------------------------------------------"));
    for ( pos = GetFirstWplPos(); pos != NULL; GetNextWpl(pos)) {
      pWpl = (CWpl*)m_wplList.GetAt(pos);
      if (pWpl->m_Po_Wpl.IsEmpty()) continue;
      dwSum=0;
      for ( posP = pWpl->GetFirstPo_WplPos(); posP != NULL; pWpl->GetNextPo_Wpl(posP)) {
        pPo_Wpl = (CPo_Wpl*)pWpl->m_Po_Wpl.GetAt(posP);
        if (pPo_Wpl->m_Typ!=8) continue;    // Nie koszty
        dwSum+=pPo_Wpl->m_Kwota;
      }
      if (dwSum) {
        strGrZl = GetDCFormatted(AllSumKoszty);
        AllSumKoszty-=dwSum;
        strD = GetDCFormatted(dwSum);
        strTyp = GetDCFormatted(AllSumKoszty);
        if (pWpl->m_wplGroszy!=-1L)
          sprintf(buf1,"  (%2u) %s     %13s -%13s =%13s",pWpl->m_wplNum,
                                                         (const char*)pWpl->m_wplDate,
                                                         (const char*)strGrZl,
                                                         (const char*)strD,
                                                         (const char*)strTyp);
        else sprintf(buf1,"  (N)  %s     %13s -%13s =%13s",
                                                         (const char*)pWpl->m_wplDate,
                                                         (const char*)strGrZl,
                                                         (const char*)strD,
                                                         (const char*)strTyp);
        strD = buf1;  m_Obl.Add (CString(strD));
        if (AllSumKoszty==0) break;
      }
    }
  }
  else {
    m_Obl.RemoveAt(m_Obl.GetSize()-1);
  }

  m_Obl.Add (CString(""));
  m_Obl.Add (CString("------------------------------- NALE¯NOŒCI -------------------------------"));
  for ( posN = GetFirstNalPos(); posN != NULL; GetNextNal(posN)) {
    pNal = (CNal*)m_nalList.GetAt(posN);

    if (GetStrDate(pNal->m_nalDate) > GetStrDate(m_stanDate)) continue;
    if ((pNal->m_nalTyp>= T_KOSZTY_PROCESU && pNal->m_nalTyp<=T_KOSZTY_EKZEK) ||
			   pNal->m_nalTyp == T_KOSZTY_INNE)
      continue;    // Koszty
    m_Obl.Add (CString("--------------------------------------------------------------------------"));
    nTypN= IDS_NTYP0+pNal->m_nalTyp;
    strTyp.LoadString(nTypN);
    strGrZl = GetDCFormatted(pNal->m_nalGroszy);
    OstNal = (double)pNal->m_nalGroszy;
    if (pNal->m_nalTyp == T_NALEZNOSC || 
				pNal->m_nalTyp == T_NALEZ_SPLACONA ||
				pNal->m_nalTyp == T_KWOTA_ZAB ) {// Maybe odsetki
      if (pNal->m_nalOkrCount==0) goto brakods;
      if (pNal->m_nalTyp == T_NALEZ_SPLACONA) OstNal =0;
      sprintf(buf1,"  %-18s%13s   %8s     Odsetki:",(const char*)strTyp,
                                                 (const char*)strGrZl,(const char*)pNal->m_nalDate);
      strD = buf1; m_Obl.Add (CString(strD));
      Put_OkrOds(pNal->m_nalNum);
      //DWORD dwS=0L;
      m_Obl.Add (CString("--------------------------------------------------------------------------"));
      for ( pos = GetFirstWplPos(); pos != NULL; GetNextWpl(pos)) { // Circle of WPLATY
        pWpl = (CWpl*)m_wplList.GetAt(pos);
        if (pWpl->m_Po_Wpl.IsEmpty()) continue;
        dwSum = dwSum1 = OstOds = 0;
        for ( posP = pWpl->GetFirstPo_WplPos(); posP != NULL; pWpl->GetNextPo_Wpl(posP)) {
          pPo_Wpl = (CPo_Wpl*)pWpl->m_Po_Wpl.GetAt(posP);
          if (pPo_Wpl->m_Typ==0 && dwSum==0) { // naleznosc
            pNal1 = (CNal*)m_nalList.GetAt(pPo_Wpl->m_Pos);
            if (pNal1->m_nalNum != pNal->m_nalNum) continue;
            dwSum = pPo_Wpl->m_Kwota;
          }
          else {
            if (pPo_Wpl->m_Typ==1 || pPo_Wpl->m_Typ==2) { // odsetki
              pOkrOd = (COkrOd*)m_odsList.GetAt(pPo_Wpl->m_Pos);
              if (pNal->m_nalNum != pOkrOd->m_okrParent) continue;
              OstOds+=pPo_Wpl->m_Kwota;
              dwSum1+=pPo_Wpl->m_KwotaDo;
        //      if (pPo_Wpl->m_Kwota==0) dwS=pPo_Wpl->m_KwotaDo;
            }
          }
        }
        //if (OstOds) dwSum1-=dwS;
        
        if (dwSum+OstOds==0) continue;
        if (pNal->m_nalTyp == T_NALEZ_SPLACONA) {
          strGrZl = GetDCFormatted(dwSum1);
          strD = GetDCFormatted(OstOds);
          dwSum1-=OstOds;
          strTyp = GetDCFormatted(dwSum1);
          if (pWpl->m_wplGroszy!=-1L)
            sprintf(buf1,"  (%2u) %s     %13s -%13s =%13s",
                         pWpl->m_wplNum,(const char*)pWpl->m_wplDate,
                         (const char*)strGrZl,(const char*)strD,(const char*)strTyp);
          else   sprintf(buf1,"  (N)  %s     %13s -%13s =%13s",
                         (const char*)pWpl->m_wplDate,
                         (const char*)strGrZl,(const char*)strD,(const char*)strTyp);
          strD = buf1; m_Obl.Add (CString(strD));
        }
        else {
          strGrZl = GetDCFormatted(OstNal);
          strD = GetDCFormatted(dwSum1);
          strTyp = GetDCFormatted(dwSum1+OstNal);
          if (pWpl->m_wplGroszy!=-1L)
            sprintf(buf1,"  (%2u) %s     %13s +%13s =%13s",
                       pWpl->m_wplNum,(const char*)pWpl->m_wplDate,
                       (const char*)strGrZl,(const char*)strD,(const char*)strTyp);
          else
            sprintf(buf1,"  (N)  %s     %13s +%13s =%13s",
                       (const char*)pWpl->m_wplDate,
                       (const char*)strGrZl,(const char*)strD,(const char*)strTyp);
          strD = buf1; m_Obl.Add (CString(strD));
          strD = GetDCFormatted(dwSum+OstOds);
          OstNal-=dwSum;
          strGrZl = GetDCFormatted(OstNal);
          dwSum1-=OstOds;
          strTyp = GetDCFormatted(dwSum1);
          strD1 = GetDCFormatted(dwSum1+OstNal);
          sprintf(buf1,"  %18s%13s +%13s =%13s",(const char*)strD,
                       (const char*)strGrZl,(const char*)strTyp,(const char*)strD1);

          strD = buf1; m_Obl.Add (CString(strD));
          if (OstNal+dwSum1==0) goto nextnal;
        }
      } // Next WPLATA
      OstNal = dwSum1= dwSum =0;
      for ( posK = GetFirstK_WplPos(); posK != NULL; GetNextK_Wpl(posK)) {
        pKW = (CK_Wpl*)m_k_Wpl.GetAt(posK);
        if (pKW->m_Typ==0) {   // Nal
          pNal1 = (CNal*)m_nalList.GetAt(pKW->m_Pos);
          if (pNal->m_nalNum!=pNal1->m_nalNum) continue;
          OstNal = pKW->m_Kwota;
        }
        else
          if (pKW->m_Typ!=0 && pKW->m_Typ!=8) {  // Odsetki
            pOkrOd = (COkrOd*)m_odsList.GetAt(pKW->m_Pos);
            if (pNal->m_nalNum!= pOkrOd->m_okrParent) continue;
            dwSum1+=pKW->m_Kwota;
            if (pKW->m_Typ==1 /*&& pKW->m_Kwota > 0.*/) {
              CDate dat(m_stanDate);
              dwSum = KwotaOds(pOkrOd, dat.GetDate()+1, dat.GetDate()+1);
            }
          }
      }
      if (OstNal>0. || dwSum1>0. || dwSum>0.)  {
        strGrZl = GetDCFormatted(OstNal); // Kwota k wplacie
        c1 = strGrZl.Right(1)[0];
		strTyp = GetDCFormatted(dwSum1);
        c2 = strTyp.Right(1)[0];
		strD1 = GetDCFormatted(dwSum);
        strD = GetDCFormatted(dwSum1+OstNal);
	    c3 = strD.Right(1)[0];
        sprintf(buf1,"       %s     %13s +%13s =^B%13s^B",
                                                          (const char*)m_stanDate,
                                                          (const char*)strGrZl,
                                                          (const char*)strTyp,
                                                          (const char*)strD);

        strD = buf1; m_Obl.Add (CString(strD));
		if (( ((c1&1) + (c2&1)) & 1) - (c3 & 1)) 
	        sprintf(buf1,"  (ró¿nice wynikaj¹ce z zaokr¹glenia -0.01)           %%%%/1dzieñ ^B");
		else
			sprintf(buf1,"                                                      %%%%/1dzieñ ^B");
        strD = buf1; strD +=strD1+"^B";  m_Obl.Add (CString(strD));
        AllSumOds+=dwSum1; AllSumProc+=dwSum;
      }
    }  // Naleznosc z odsetkami - koniec
    else {  // Brak odsetek
brakods:
      sprintf(buf1,"  %-18s%13s   %8s",(const char*)strTyp,
                                       (const char*)strGrZl,(const char*)pNal->m_nalDate);
      strD = buf1;
      m_Obl.Add (CString(strD));
      m_Obl.Add (CString("--------------------------------------------------------------------------"));
      for ( pos = GetFirstWplPos(); pos != NULL; GetNextWpl(pos)) {
        pWpl = (CWpl*)m_wplList.GetAt(pos);
        if (pWpl->m_Po_Wpl.IsEmpty()) continue;
        for ( posP = pWpl->GetFirstPo_WplPos(); posP != NULL; pWpl->GetNextPo_Wpl(posP)) {
          pPo_Wpl = (CPo_Wpl*)pWpl->m_Po_Wpl.GetAt(posP);
          if (pPo_Wpl->m_Typ && pPo_Wpl->m_Typ!=4) continue;    // Nie naleznosc
          pNal1 = (CNal*)m_nalList.GetAt(pPo_Wpl->m_Pos);
          if (pNal1->m_nalNum != pNal->m_nalNum) continue;
          strTyp = GetDCFormatted(OstNal);
          strD = GetDCFormatted(pPo_Wpl->m_Kwota);
          OstNal-=pPo_Wpl->m_Kwota;
          strGrZl = GetDCFormatted(OstNal);
          if (pWpl->m_wplGroszy!=-1L)
            sprintf(buf1,"  (%2u) %s     %13s -%13s =%13s",
                         pWpl->m_wplNum,(const char*)pWpl->m_wplDate,
                         (const char*)strTyp,(const char*)strD,(const char*)strGrZl);
          else
            sprintf(buf1,"  (N)  %s     %13s -%13s =%13s",
                         (const char*)pWpl->m_wplDate,
                         (const char*)strTyp,(const char*)strD,(const char*)strGrZl);

          strD = buf1; m_Obl.Add (CString(strD));
          if (OstNal==0) goto nextnal;
        }
      }
      strGrZl = GetDCFormatted(OstNal); // Kwota k wplacie
      sprintf(buf1,"       %s     ^B%13s^B",(const char*)m_stanDate,
                                          (const char*)strGrZl);
      strD = buf1; m_Obl.Add (CString(strD));
    } // Brak odsetek - koniec

    AllSumNal+=OstNal;

nextnal: m_Obl.Add (CString(""));
  }
 sprintf(buf1,"-----------------------  STAN SPRAWY na %s  ------------------------",
                         (const char*)m_stanDate);
 strD = buf1; m_Obl.Add (CString(strD));
}  // For middle results
else {
 m_Obl.Add (CString("--------------------------------------------------------------------------"));
}
 AllSumKoszty=AllSumOds=AllSumNal=AllSumProc=0.0; precision = 0.0;
 CDate dat(m_stanDate);
 for ( posK = GetFirstK_WplPos(); posK != NULL; GetNextK_Wpl(posK)) {
   pKW = (CK_Wpl*)m_k_Wpl.GetAt(posK);
   switch (pKW->m_Typ) {
   case 0:  pNal1 = (CNal*)m_nalList.GetAt(pKW->m_Pos); AllSumNal+= pKW->m_Kwota; break;
   case 8:  pNal1 = (CNal*)m_nalList.GetAt(pKW->m_Pos); AllSumKoszty+= pKW->m_Kwota; break;
   default: pOkrOd = (COkrOd*)m_odsList.GetAt(pKW->m_Pos);
            AllSumOds+=pKW->m_Kwota;
            if (pKW->m_Typ==1 /*&& pKW->m_Kwota*/) {
              AllSumProc += KwotaOds(pOkrOd, dat.GetDate()+1, dat.GetDate()+1);
              precision +=f_ods;  // 97-01-22
            }
   }
 }
 if (Kw==0.0) {
   strGrZl = GetDCFormatted(AllSumKoszty);
   sprintf(buf1,"  ^BKOSZTY    %13s^B",(const char*)strGrZl);
   c1 = strGrZl.Right(1)[0];
   strD = buf1; m_Obl.Add (CString(strD));
   strGrZl = GetDCFormatted(AllSumOds);
   sprintf(buf1,"  ^BODSETKI   %13s^B",(const char*)strGrZl);
   c2 = strGrZl.Right(1)[0];
   strD = buf1; m_Obl.Add (CString(strD));
   strGrZl = GetDCFormatted(AllSumNal);
   sprintf(buf1,"  ^BNALE¯NOŒCI %12s^B",(const char*)strGrZl);
   c3 = strGrZl.Right(1)[0];
   strD = buf1; m_Obl.Add (CString(strD));

   strGrZl = GetDCFormatted(AllSumNal+AllSumOds+AllSumKoszty);
   buf1[0] = strGrZl.Right(1)[0];
   if (((c1 & 1) + (c2 & 1) + (c3 & 1)& 1) - (buf1[0] & 1)) {
	    sprintf(buf1,"  ^B           %12s^B  - ró¿nice wynikaj¹ce z zaokr¹glenia","-0.01");
		strD = buf1; m_Obl.Add (CString(strD));
   }
   m_Obl.Add (CString("^B-------------------------^B"));
   
   AllSumProc = ceil(precision);          
   { //float rest_ods = precision - precision);   //???????
     //if (rest_ods>0.) AllSumProc+=1;
     strTyp = GetDCFormatted(AllSumProc);  // 97-01-22
   }
   sprintf(buf1,"  ^BRAZEM     %13s^B   %%%%/1dzieñ ^B",(const char*)strGrZl);
   strD = buf1+strTyp; strD+="^B";  m_Obl.Add (CString(strD));
//   if (theApp.m_nDoW) {
     TS[tsActive].doN = AllSumNal;
     TS[tsActive].doO = AllSumOds;
     TS[tsActive].doK = AllSumKoszty;
//   }
 }
 else {
   strTyp = GetDCFormatted(Kw);
   sprintf(buf1,"  ^BNADP£ATA^B  ^B%13s^B",(const char*)strTyp);
   strD = buf1;
   m_Obl.Add (CString(strD));
//   if (theApp.m_nDoW) {
     TS[tsActive].doN = -Kw; TS[tsActive].doO = TS[tsActive].doK = 0.0;
//   }
 }
// if (theApp.m_nDoW) {
   TS[tsActive].typ = 'S'; TS[tsActive].dat = dat.GetDate();
   if (Fl)  TS[tsActive].poO = AllSumProc;
   tsActive++;
// }
 m_Obl.Add (CString(""));
 if (Fl) {
   Fl=0;
   for ( pos = GetFirstWplPos(); pos != NULL; GetNextWpl(pos)) {
     pWpl = (CWpl*)m_wplList.GetAt(pos);
     if (GetStrDate(m_stanDate) < GetStrDate(pWpl->m_wplDate) || pWpl->m_wplGroszy==-1L) break;
     if (pWpl->m_ZostaloGroszy) {
        if (Fl==0) {
        m_Obl.Add (CString(""));
        m_Obl.Add (CString("-------------------------------  NADP£ATY  -------------------------------"));
        }
        Fl=1;
        strGrZl = GetDCFormatted(pWpl->m_ZostaloGroszy);
        sprintf(buf1,"  (%2u)   %s   %13s",pWpl->m_wplNum,(const char*)pWpl->m_wplDate,(const char*)strGrZl);
        strD = buf1;  m_Obl.Add (CString(strD));
     }
   }
   if (Fl) m_Obl.Add (CString(""));
 }
/////////////////////////////////////////
 if (m_stanActive+1==m_stanList.GetCount()){
   m_Obl.Add (CString("--------------------------------------------------------------------------"));
  strGrZl = GetDCFormatted(SummaOdsetek);
//   strGrZl = GetDCFormatted(AllSumOds);
   sprintf(buf1,"  ^BSUMA NALICZONYCH ODSETEK %15s^B",(const char*)strGrZl);
   strD = buf1; m_Obl.Add (CString(strD));
   strGrZl = GetDCFormatted(SummaWplatOdsetek);
   sprintf(buf1,"  ^BSUMA WP£AT NA ODSETKI    %15s^B",(const char*)strGrZl);
   strD = buf1; m_Obl.Add (CString(strD));
   strGrZl = GetDCFormatted(SummaWplatKwotyGlawne);
   sprintf(buf1,"  ^BSUMA WP£AT NA KWOTY      %15s^B",(const char*)strGrZl);
   strD = buf1; m_Obl.Add (CString(strD));
   m_Obl.Add (CString("--------------------------------------------------------------------------"));
   m_Obl.Add (CString(""));
 }
/////////////////////////////////////////
}


//------------------------------------------------
void CKmDoc::PutOblHead()
{ CString strD; int fl;
  strD ="  Sprawa ^BKM ("+m_Sprawa;  strD +=")^B";
//  if (!theApp.m_nTyp) {
  fl=theApp.GetProfileInt("Config", "Alg",0);
  if (fl==0)     strD +=", miesi\271c kalendarzowy";
  else           strD +=", miesi\271c bankowy";
//  }
  m_Obl.Add (CString(strD));
}

//------------------------------------------------
void CKmDoc::PutOblStanDay()
{
 CString strD;
 char buf[50];
   if (m_stanList.GetCount()== m_stanActive+1)
     m_Obl.Add (CString("**************************************************************************"));
   else
     m_Obl.Add (CString("=========================================================================="));
   sprintf(buf,"%2u. Stan sprawy na dzieñ  ^B",m_stanActive+1);
   strD +=buf + m_stanDate;   strD +="^B";   m_Obl.Add (CString(strD));
}


//------------------------------------------------
void CKmDoc::LongDate()
{
  CNal* pNal;
  POSITION pos;
  LT_Count = 0;
  CDate datStan(m_stanDate);

	SummaOdsetek=0.0;
	SummaWplatOdsetek=0.0;
	SummaWplatKwotyGlawne=0.0;

  while(!m_k_Wpl.IsEmpty()) delete m_k_Wpl.RemoveHead();
  for ( pos = GetFirstNalPos(); pos != NULL; GetNextNal(pos)) {
    pNal = (CNal*)m_nalList.GetAt(pos);
    CDate dat(pNal->m_nalDate);
    pNal->m_LDate = dat;
    if ((dat>0L) && datStan >= dat && pNal->m_nalTyp != T_NALEZ_SPLACONA) {    // 6 (=7? 2002)

      LnT[LT_Count].dat = dat.GetDate();
      LnT[LT_Count].typ = TYP_NALEZNOSC;      // 0
      LnT[LT_Count++].pos = pos;
    }
  }
COkrOd* pOkrOd;
  for ( pos = GetFirstOkrOdPos(); pos != NULL; GetNextOkrOd(pos)) {
    pOkrOd = (COkrOd*)m_odsList.GetAt(pos);
    pNal = GetParentNal(pOkrOd);
    CDate datN(pNal->m_nalDate);
    if (datStan >= datN) {
//      CDate dat(pOkrOd->m_okrDateP);
//      CDate dat1(pOkrOd->m_okrDateK);
//	  if (dat1>=datN){
//		if (dat<=datN) dat = datN;
//		pOkrOd->m_PLDate = dat;
//		LnT[LT_Count].dat = dat.GetDate();
//		LnT[LT_Count].typ = TYP_ODSETKI_POCZ;    // 1
//		LnT[LT_Count++].pos = pos;
//		pOkrOd->m_KLDate = dat1;
//		LnT[LT_Count].dat = dat1.GetDate();
//		LnT[LT_Count].typ = TYP_ODSETKI_KON;    // 2
//		LnT[LT_Count++].pos = pos;
//	  }
      CDate dat(pOkrOd->m_okrDateP);
      pOkrOd->m_PLDate = dat;
      LnT[LT_Count].dat = dat.GetDate();
      LnT[LT_Count].typ = TYP_ODSETKI_POCZ;    // 1
      LnT[LT_Count++].pos = pos;
      CDate dat1(pOkrOd->m_okrDateK);
      pOkrOd->m_KLDate = dat1;
      LnT[LT_Count].dat = dat1.GetDate();
      LnT[LT_Count].typ = TYP_ODSETKI_KON;    // 2
      LnT[LT_Count++].pos = pos;
//      if (LT_Count >=ILOSC_PUNKTOW) return FALSE;
//    }
    }
  }
CWpl* pWpl;
  for ( pos = GetFirstWplPos(); pos != NULL; GetNextWpl(pos)) {
    pWpl = (CWpl*)m_wplList.GetAt(pos);
    while (!pWpl->m_Po_Wpl.IsEmpty()) delete pWpl->m_Po_Wpl.RemoveHead();
    CDate dat(pWpl->m_wplDate);
    if ((dat>0L) && datStan >= dat) {
      pWpl->m_LDate = dat;
      LnT[LT_Count].dat = dat.GetDate();
      LnT[LT_Count].typ = TYP_WPLATA;       // 3;
      LnT[LT_Count++].pos = pos;
    }
  }
  LnT[LT_Count].dat = datStan.GetDate();
  LnT[LT_Count].typ = TYP_STAN_DATE;         // 4;
  LnT[LT_Count++].pos = NewWplStanu();
  qsort( (void *)&LnT[0], (size_t)LT_Count, sizeof(struct LineTime), compareL );
}

//------------------------------------------------
int compareL( const void *arg1, const void *arg2 )
{ LineTime *l1,*l2;
l1 = (LineTime *) arg1; l2 = (LineTime *) arg2;
if (l1->dat > l2->dat)  return 1;
if (l1->dat < l2->dat)  return -1;
if (l1->typ > l2->typ)  return 1;
else if (l1->typ < l2->typ) return -1;
if (l1->pos >= l2->pos)  return 1;
else  return -1;
}

//------------------------------------------------
int compareT( const void *arg1, const void *arg2 )
{ TabStan *l1,*l2;
  l1 = (TabStan *) arg1; l2 = (TabStan *) arg2;
  if (l1->dat > l2->dat)  return 1;
  if (l1->dat < l2->dat)  return -1;
  if (l1->typ == 'S')  return 1;
  if (l1->typ==l2->typ) {
    if (l1->typ != 'R')  return 1;
    if (l1->kw <= l2->kw) return 1;
    return -1;
  }
  return -1;
}
//------------------------------------------------
void CKmDoc::PutOblWpl()
{
  m_Obl.Add (CString(""));
  m_Obl.Add (CString("--------------------------------- WP£ATY ---------------------------------"));
CString strD;
char buf[10],buf1[120];
CWpl* pWpl;
double dwSum=0;
CString strGrZl;
POSITION pos;
CStanD* pStan;
  pStan = (CStanD *)m_stanList.GetTail();

  for ( pos = GetFirstWplPos(); pos != NULL; GetNextWpl(pos)) {
    pWpl = (CWpl*)m_wplList.GetAt(pos);
    if (GetStrDate(pWpl->m_wplDate) > GetStrDate(pStan->m_stanD) || pWpl->m_wplGroszy==-1L) break;
    sprintf(buf,"%4u. ", pWpl->m_wplNum);
    strGrZl = GetDCFormatted(pWpl->m_wplGroszy);
    sprintf(buf1," %13s   ", (const char*)strGrZl);
    strD = buf + pWpl->m_wplDate + buf1;
//    if (!theApp.m_nTyp)
    { strGrZl.LoadString(pWpl->m_wplTyp+IDS_WTYP0); strD +=strGrZl; }
    m_Obl.Add (CString(strD));
    dwSum +=(double)pWpl->m_wplGroszy;
  }
 m_Obl.Add (CString("---------------------------------"));
 strGrZl = GetDCFormatted(dwSum);
 sprintf(buf1,"^B%13s^B", (const char*)strGrZl);
 strD ="Razem          "; strD+=buf1; m_Obl.Add (CString(strD));
 m_Obl.Add (CString(""));
}


//------------------------------------------------
void CKmDoc::PutDodatek()
{
CString strD;
char buf1[120];
CWpl* pWpl;
CNal* pNal;
double dwSum=0;
CString strGrZl;
POSITION pos;
int Fl;
CStanD* pStan;
  pStan = (CStanD *)m_stanList.GetTail();

 Fl=0;
 for ( pos = GetFirstWplPos(); pos != NULL; GetNextWpl(pos)) {
   pWpl = (CWpl*)m_wplList.GetAt(pos);
   if (GetStrDate(pWpl->m_wplDate) <= GetStrDate(pStan->m_stanD)) continue;
   if (Fl==0) {
     m_Obl.Add (CString(""));
     m_Obl.Add (CString("**************************************************************************"));
     m_Obl.Add (CString("  W rozliczeniach nie uwzglêdniono:"));
     m_Obl.Add (CString("  wp³aty -"));
     Fl=1;
   }
   strGrZl = GetDCFormatted(pWpl->m_wplGroszy);
   sprintf(buf1,"  (%2u)   %s   %13s",pWpl->m_wplNum,(const char*)pWpl->m_wplDate,(const char*)strGrZl);
   strD = buf1;  m_Obl.Add (CString(strD));
 }
 for ( pos = GetFirstNalPos(); pos != NULL; GetNextNal(pos)) {
    pNal = (CNal*)m_nalList.GetAt(pos);
    if (GetStrDate(pNal->m_nalDate) <= GetStrDate(pStan->m_stanD)) continue;
    if (Fl==0) {
      m_Obl.Add (CString(""));
      m_Obl.Add (CString("**************************************************************************"));
      m_Obl.Add (CString("  W rozliczeniach nie uwzglêdniono:"));
      Fl=1;
    }
    if (Fl==1) {
      m_Obl.Add (CString("  nale\277noœci -"));
     Fl=2;
    }
   strGrZl = GetDCFormatted(pNal->m_nalGroszy);
   sprintf(buf1,"  (%2u)   %s   %13s",pNal->m_nalNum,(const char*)pNal->m_nalDate,(const char*)strGrZl);
   strD = buf1;  m_Obl.Add (CString(strD));
 }
 if (Fl) m_Obl.Add (CString(""));
}

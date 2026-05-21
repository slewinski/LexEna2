
//------------------------------------------------------ Computing result
BOOL CKmDoc::ObliczSkr()
{
  LongDateSkr();
  if (m_stanActive==0) { PutOblHead(); PutOblWpl(); }
  PutOblStanDay();
  FormDocSkr(ComputeWplSkr());
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

//------------------------------------------------
void CKmDoc::LongDateSkr()
{
  CNal* pNal;
  POSITION pos;
  LT_Count = 0;
  CDate datStan(m_stanDate);

  while(!m_k_Wpl.IsEmpty()) delete m_k_Wpl.RemoveHead();
  for ( pos = GetFirstNalPos(); pos != NULL; GetNextNal(pos)) {
    pNal = (CNal*)m_nalList.GetAt(pos);
    CDate dat(pNal->m_nalDate);
    if ((dat>0L) && datStan >= dat) {
      pNal->m_LDate = dat;
      LnT[LT_Count].dat = dat.GetDate();
      LnT[LT_Count].typ = TYP_NALEZNOSC;      // 0
      LnT[LT_Count++].pos = pos;
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
double CKmDoc::ComputeWplSkr()
{ CNal* pNal;
  int typ;
  DWORD Date0;
  double Kw=0;
  for (LT_Wpl=0; LnT[LT_Wpl].typ !=4; LT_Wpl++)
    if (LnT[LT_Wpl].typ == TYP_NALEZNOSC) {       // Nal
      pNal = (CNal*)m_nalList.GetAt(LnT[LT_Wpl].pos);
      if (pNal->m_nalTyp) typ=8; else typ=0;
      if (pNal->m_nalOkrCount) Date0=0L;  // Brak odsetek
      else Date0=pNal->m_LDate.GetDate();
      NewK_Wpl(typ,pNal->m_nalTyp,LnT[LT_Wpl].pos, (double)pNal->m_nalGroszy,Date0);
    }
    else Kw=RealWplSkr(Kw);  // Wpl
  //if (Kw>0L) Kw=RealWplSkr(Kw+1L);
  if (Kw>0) Kw=RealWplSkr(ceil(Kw));
  return Kw;
}

double CKmDoc::RealWplSkr(double Kw)
{ CWpl* pWpl;
  pWpl = (CWpl*)m_wplList.GetAt(LnT[LT_Wpl].pos);
  if (pWpl->m_wplGroszy == -1L)
     pWpl->m_ZostaloGroszy= (DWORD)Kw;
  else
     pWpl->m_ZostaloGroszy= pWpl->m_wplGroszy + (DWORD)Kw;
  pWpl->m_ZostaloGroszy = (DWORD)(ceil(Plata(8,(CWpl *)pWpl,pWpl->m_ZostaloGroszy)));
  ClearKWpl();
  if ((pWpl->m_ZostaloGroszy)==0) return 0;
  PlataSkr((CWpl *)pWpl);
  return (double)pWpl->m_ZostaloGroszy;
}

//------------------------------------------------
void CKmDoc::PlataSkr(CWpl * pWpl)
{
 CK_Wpl* pKW;
 double f_ods=0.0, Procent,SumProc,KwDoNal,KwDoOds,IleOds;
 while (!m_k_Wpl.IsEmpty()) {
  pKW = (CK_Wpl*)m_k_Wpl.GetHead();
  if (pKW->m_LDate0) { // Odsetki
    Procent = CalcProc(pKW->m_LDate0, (DWORD)pWpl->m_LDate.GetDate());
    SumProc = Procent + pKW->m_Procent;
    f_ods = (double)pWpl->m_ZostaloGroszy*(double)10000000.0 / (double)(SumProc+10000000.); //Marzena 29.05.98 bylo 10000
    modf(f_ods+(double)0.5, &KwDoNal);  // Wariant1
    
    // Wariant10 (dodatkowe okreglenie do 10gr)
    modf(  (KwDoNal/(double)10.0) +(double)0.5, &KwDoNal);  
    KwDoNal *=(double)10.0;
    
    
    KwDoOds = (double)pWpl->m_ZostaloGroszy - KwDoNal;
    IleOds = (pKW->m_Kwota * SumProc)/(double)10000000.0; //Marzena 29.05.98 bylo 10000
    modf(IleOds+(double)0.5, &IleOds);  // cala czesc
    if ((double)pWpl->m_ZostaloGroszy >= IleOds+pKW->m_Kwota) { // Koniec naleznosci
       pWpl->NewPo_Wpl(0,pKW->m_Pos,pKW->m_Kwota,pKW->m_Kwota);
       pWpl->NewPo_Wpl(1,pKW->m_Pos,IleOds,IleOds);
       pWpl->m_ZostaloGroszy -= DWORD(IleOds+pKW->m_Kwota);
       delete m_k_Wpl.RemoveHead();
    }
    else {    // Czesc naleznosci
     pWpl->NewPo_Wpl(0,pKW->m_Pos,KwDoNal,pKW->m_Kwota);
     pWpl->NewPo_Wpl(1,pKW->m_Pos,KwDoOds,IleOds);
     pKW->m_Kwota-=KwDoNal;
     pKW->m_Procent= SumProc;
     pKW->m_LDate0 = (DWORD)pWpl->m_LDate.GetDate()+1;
     pWpl->m_ZostaloGroszy = 0;
     return;
    }
  }
  else {        // Brak odsetek
     if ((double)pWpl->m_ZostaloGroszy >= pKW->m_Kwota) { // Koniec naleznosci
       pWpl->NewPo_Wpl(0,pKW->m_Pos,pKW->m_Kwota,pKW->m_Kwota);
       pWpl->m_ZostaloGroszy -= (DWORD)pKW->m_Kwota;
       delete m_k_Wpl.RemoveHead(); 
     }
     else {     // Czesc naleznosci
       pWpl->NewPo_Wpl(0,pKW->m_Pos,pWpl->m_ZostaloGroszy,pKW->m_Kwota);
       pKW->m_Kwota-=(double)pWpl->m_ZostaloGroszy;
       pWpl->m_ZostaloGroszy = 0;
       return;
     }
  } // Brak odsetek
 }  // while Wpl
}

//------------------------------------
double CKmDoc::CalcProc(DWORD daysF, DWORD daysE)
{ DWORD daysEi, dni;
  double ods=0;
  int i;
 if (daysF < (DWORD)OOSK[0].lDP)
      daysF = OOSK[0].lDP;          // period before 1990 without percents
 for (i=0; i< IleOkrOdsSkr; i++)  {
   if ((i< IleOkrOdsSkr-1) && (daysF >= (DWORD)OOSK[i+1].lDP)) continue;
   if (i< IleOkrOdsSkr-1)
      daysEi =  (daysE >=(DWORD)OOSK[i+1].lDP) ? OOSK[i+1].lDP-1 : daysE;
   else                          // last interval till 1999
     daysEi = daysE;
   dni = (DWORD)(daysEi - daysF+1);
   ods += (double) (dni * OOSK[i].Stopa);
   if (daysEi == daysE) break;
   daysF = daysEi+1;
 }  // for i
 return ods;
}

//------------------------------------------------
void CKmDoc::FormDocSkr(double Kw)
{
CString strD,strD1;
int nTypN,Fl,i;

CString strTyp;
double dwSum=0,dwSum1, OstNal, OstOds,
       AllSumNal=0,AllSumKoszty=0,AllSumOds=0,AllSumProc=0;
char buf1[150];
CWpl* pWpl;
CNal* pNal;
CNal* pNal1;
CPo_Wpl* pPo_Wpl;
CK_Wpl* pKW;
CString strGrZl;
POSITION pos,posP,posN,posK;
CStanD* pStan;
double f_ods=0.0;

  pStan = (CStanD *)m_stanList.GetTail();
  if (m_stanDate == pStan->m_stanD) Fl=1; else Fl=0;    // Fl=1  - konieczny stan
// Koszty-----------------------------------------------------------------------------
if (Fl) {
  m_Obl.Add (CString(""));
  m_Obl.Add (CString("--------------------------------- KOSZTY ---------------------------------"));
  for ( pos = GetFirstNalPos(); pos != NULL; GetNextNal(pos)) {
    pNal = (CNal*)m_nalList.GetAt(pos);
    if (GetStrDate(pNal->m_nalDate) > GetStrDate(m_stanDate)) continue;
    if (pNal->m_nalTyp) {
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
      dwSum=0L;
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
  m_Obl.Add (CString(""));
  m_Obl.Add (CString("------------------------------- NALE¯NOŒCI -------------------------------"));
  for ( posN = GetFirstNalPos(); posN != NULL; GetNextNal(posN)) {
    pNal = (CNal*)m_nalList.GetAt(posN);

    if (GetStrDate(pNal->m_nalDate) > GetStrDate(m_stanDate)) continue;
    if (pNal->m_nalTyp)  continue;    // Koszty
    m_Obl.Add (CString("--------------------------------------------------------------------------"));
    strTyp.LoadString(IDS_NTYP0);
    strGrZl = GetDCFormatted(pNal->m_nalGroszy);
    OstNal = (double)pNal->m_nalGroszy;
    if (!pNal->m_nalOkrCount) {// Maybe odsetki
      sprintf(buf1,"  %-18s%13s   %8s     Odsetki skarbowe",(const char*)strTyp,
                   (const char*)strGrZl,(const char*)pNal->m_nalDate);
      strD = buf1; m_Obl.Add (CString(strD));
      m_Obl.Add (CString("--------------------------------------------------------------------------"));
      for ( pos = GetFirstWplPos(); pos != NULL; GetNextWpl(pos)) { // Circle of WPLATY
        pWpl = (CWpl*)m_wplList.GetAt(pos);
        if (pWpl->m_Po_Wpl.IsEmpty()) continue;
        dwSum = dwSum1 = OstOds = 0L;
        for ( posP = pWpl->GetFirstPo_WplPos(); posP != NULL; pWpl->GetNextPo_Wpl(posP)) {
          pPo_Wpl = (CPo_Wpl*)pWpl->m_Po_Wpl.GetAt(posP);
          if (pPo_Wpl->m_Typ==0 && dwSum==0) { // naleznosc
            pNal1 = (CNal*)m_nalList.GetAt(pPo_Wpl->m_Pos);
            if (pNal1->m_nalNum != pNal->m_nalNum) continue;
            dwSum = pPo_Wpl->m_Kwota;
            pWpl->GetNextPo_Wpl(posP);
            pPo_Wpl = (CPo_Wpl*)pWpl->m_Po_Wpl.GetAt(posP);
            OstOds+=pPo_Wpl->m_Kwota;
            dwSum1+=pPo_Wpl->m_KwotaDo;
            break;
          }
        }
        if (dwSum+OstOds==0L) continue;

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
        if (OstNal+dwSum1==0L) goto nextnal;
      } // Next WPLATA

      dwSum = OstNal = dwSum1=0L;
      for ( posK = GetFirstK_WplPos(); posK != NULL; GetNextK_Wpl(posK)) {
        pKW = (CK_Wpl*)m_k_Wpl.GetAt(posK);
        if (pKW->m_Typ==0) {   // Nal
          pNal1 = (CNal*)m_nalList.GetAt(pKW->m_Pos);
          if (pNal->m_nalNum!=pNal1->m_nalNum) continue;
          OstNal = pKW->m_Kwota;
          dwSum1 = CalcProc(pKW->m_LDate0, LnT[LT_Wpl].dat) + pKW->m_Procent;
          f_ods= ((float)pKW->m_Kwota * (float)dwSum1)/(float)10000000.0; //Marzena 29.05.98 bylo 10000
          dwSum1 = (DWORD)f_ods;
          for (i=IleOkrOdsSkr-1; i>=0; i--)
             if ((LnT[LT_Wpl].dat+1) >= (long)OOSK[i].lDP) break;
          if (i>=0){   
            f_ods= ((float)OstNal * (float)OOSK[i].Stopa)/(float)10000000.0; //Marzena 29.05.98 bylo 10000 
            dwSum = (DWORD)f_ods;
          }
          break;
        }
      }
      strGrZl = GetDCFormatted(OstNal); // Kwota k wplacie
      strTyp = GetDCFormatted(dwSum1);
      strD1 = GetDCFormatted(dwSum);
      strD = GetDCFormatted(dwSum1+OstNal);
      sprintf(buf1,"       %s     %13s +%13s =^B%13s^B",
                   (const char*)m_stanDate,(const char*)strGrZl,
                   (const char*)strTyp,    (const char*)strD);
      strD = buf1; m_Obl.Add (CString(strD));
      sprintf(buf1,"                                                      %%%%/1dzieñ ^B");                
      strD = buf1; strD +=strD1+"^B";  m_Obl.Add (CString(strD));
      AllSumOds+=dwSum1; AllSumProc+=dwSum;
    }  // Naleznosc z odsetkami - koniec

    else {  // Brak odsetek
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
          if (pPo_Wpl->m_Typ) continue;    // Nie naleznosc
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
 sprintf(buf1,"-----------------------  STAN SPRAWY na %s  -----------------------",
                         (const char*)m_stanDate);
 strD = buf1; m_Obl.Add (CString(strD));
}  // For middle results
else {
 m_Obl.Add (CString("--------------------------------------------------------------------------"));
}
 
 AllSumKoszty=AllSumOds=AllSumNal=AllSumProc=0;
 CDate dat(m_stanDate);
 for ( posK = GetFirstK_WplPos(); posK != NULL; GetNextK_Wpl(posK)) {
   pKW = (CK_Wpl*)m_k_Wpl.GetAt(posK);
   switch (pKW->m_Typ) {
   case 0: AllSumNal+= pKW->m_Kwota;
           if (pKW->m_LDate0) {
             OstNal = CalcProc(pKW->m_LDate0, LnT[LT_Wpl].dat) + pKW->m_Procent;
             f_ods= ((float)pKW->m_Kwota * (float)OstNal)/(float)10000000.0; //Marzena 29.05.98 bylo 10000
             AllSumOds += (DWORD)f_ods;
             for (i=IleOkrOdsSkr-1; i>=0; i--)
               if ((LnT[LT_Wpl].dat+1) >= (long)OOSK[i].lDP) break;
             if (i>=0) {
               f_ods= ((float)pKW->m_Kwota * (float)OOSK[i].Stopa)/(float)10000000.0; //Marzena 29.05.98 bylo 10000
               AllSumProc += (DWORD)f_ods;
               //AllSumProc += (pKW->m_Kwota * OOSK[i].Stopa+4999L)/10000L;
             }   
           }
      break;
   case 8:  pNal1 = (CNal*)m_nalList.GetAt(pKW->m_Pos); AllSumKoszty+= pKW->m_Kwota; break;
   }
 }
 if (Kw==0L) {
   strGrZl = GetDCFormatted(AllSumKoszty);
   sprintf(buf1,"  ^BKOSZTY    %13s^B",(const char*)strGrZl);
   strD = buf1; m_Obl.Add (CString(strD));
   strGrZl = GetDCFormatted(AllSumOds);
   sprintf(buf1,"  ^BODSETKI   %13s^B",(const char*)strGrZl);
   strD = buf1; m_Obl.Add (CString(strD));
   strGrZl = GetDCFormatted(AllSumNal);
   sprintf(buf1,"  ^BNALE¯NOŒCI%13s^B",(const char*)strGrZl);
   strD = buf1; m_Obl.Add (CString(strD));
   m_Obl.Add (CString("^B-------------------------^B"));
   strGrZl = GetDCFormatted(AllSumNal+AllSumOds+AllSumKoszty);
   strTyp = GetDCFormatted(AllSumProc);
   sprintf(buf1,"  ^BRAZEM     %13s^B %%%%/1dzieñ ^B",(const char*)strGrZl);
   strD = buf1+strTyp; strD+="^B";  m_Obl.Add (CString(strD));
 }
 else {
   strTyp = GetDCFormatted(Kw);
   sprintf(buf1,"  ^BNADP£ATA  %13s^B",(const char*)strTyp);
   strD = buf1;
   m_Obl.Add (CString(strD));
 }
 m_Obl.Add (CString(""));

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


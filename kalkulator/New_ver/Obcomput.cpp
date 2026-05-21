DWORD mc_30(DWORD daysF, DWORD daysE);

//---------------------------- OBLICZ - compute
// include in OBLICZ.CPP
//
// void  CKmDoc::ComputeWpl()
// void  CKmDoc::DoWpl(int OdOldWpl)
// double CKmDoc::KwotaOds(COkrOd* pOkrOd, DWORD daysF, DWORD daysE)
// BOOL  CKmDoc::RealWplaty(double Kw)
// void  CKmDoc::PoWplate()
// void  CKmDoc::PredDoWpl(int OdOldWpl)
// void  CKmDoc::ClearKWpl()
// double CKmDoc::Plata(int Typ,CWpl *pWpl, double Kwota)

//------------------------------------------------
double CKmDoc::ComputeWpl()
{ int i=0;
CStanD* pStan;
double Kw=0;
POSITION pzW=NULL;
pStan = (CStanD *)m_stanList.GetTail();9
  for (LT_Wpl=0; LnT[LT_Wpl].typ !=4; LT_Wpl++) {
    if (LnT[LT_Wpl].typ !=3) continue;
    if (i) PredDoWpl(i);
    Kw=DoWpl(i,Kw,pzW);
    i = LT_Wpl+1;
    Kw=ceil(RealWplaty(Kw));
    if (Kw>0) { TS[tsActive].poN -=Kw; TS[tsActive].typ = 'N'; }
    if (m_stanDate == pStan->m_stanD) tsActive++;
    PoWplate();
    if (Kw>0L) pzW = LnT[LT_Wpl].pos; else pzW = NULL;
  }
  PredDoWpl(i); 
  Kw = DoWpl(i,Kw,pzW);
  if (Kw>0L) {  Kw=ceil(RealWplaty(Kw)); PoWplate(); }
  if (TypEnter==2) {FormIsxNal();FormOstNal();DocOstNal();}
  return Kw;
}

//------------------------------------------------
double CKmDoc::DoWpl(int OdOldWpl, double Kw, POSITION pzW)
{ CNal* pNal;
  COkrOd* pOkrOd;
  double kwota;
  LONG daysF,daysE;
  int typ;
  int Od=OdOldWpl-1;

  for (; OdOldWpl<LT_Wpl; OdOldWpl++)
   switch (LnT[OdOldWpl].typ) {

   case 0:  // Naleznosc
     pNal = (CNal*)m_nalList.GetAt(LnT[OdOldWpl].pos);
     if ((pNal->m_nalTyp>= T_KOSZTY_PROCESU && pNal->m_nalTyp<=T_KOSZTY_EKZEK) ||
			   pNal->m_nalTyp == T_KOSZTY_INNE) typ=8;
     else
       if (pNal->m_nalTyp== T_ODSETKI_ZAL) typ=4;
       else typ=0;

     if (pzW==NULL) { 
		 NewK_Wpl(typ,pNal->m_nalTyp,LnT[OdOldWpl].pos, (double)pNal->m_nalGroszy,0L); 
		 break; 
	 }
     RealNadplata(pzW,LnT[OdOldWpl].pos,typ, pNal->m_LDate.GetDate(),Kw, (double)pNal->m_nalGroszy,pNal->m_nalTyp);
     if (Kw>=0 && typ==0) pNal->m_LDate = 0L;

     break;
   case 1:  // Poczatok okresu odsetkowego
     pOkrOd = (COkrOd*)m_odsList.GetAt(LnT[OdOldWpl].pos);
     if (pOkrOd->m_KLDate > LnT[LT_Wpl].dat) { // Koniec okresu dalej
       typ = 1; daysE = LnT[LT_Wpl].dat; daysF = (LONG)pOkrOd->m_PLDate.GetDate();
       kwota = KwotaOds(pOkrOd, daysF, daysE); if (kwota==0.0) break;
	   NewK_Wpl(typ,6,LnT[OdOldWpl].pos,kwota,0L);
       break;
     }

     for (typ = OdOldWpl+1;LnT[OdOldWpl].pos!=LnT[typ].pos; typ++);

     LnT[typ].typ = 5;
     typ = 2; daysE = pOkrOd->m_KLDate.GetDate(); daysF =pOkrOd->m_PLDate.GetDate();
     kwota = KwotaOds(pOkrOd, daysF, daysE); if (kwota==0) break;
     { pNal = GetParentNal(pOkrOd);  CDate datN(pNal->m_nalDate);
	 if (pzW==NULL || datN > LnT[LT_Wpl].dat){
          NewK_Wpl(typ,6,LnT[OdOldWpl].pos,kwota,0); 
		  break; 
	 }
     }
	 RealNadplata(pzW,LnT[OdOldWpl].pos,typ, pOkrOd->m_PLDate.GetDate(),Kw, kwota,6);
     break;

   case 2:  // Koniec okresu odsetkowego
     typ = 2;
     pOkrOd = (COkrOd*)m_odsList.GetAt(LnT[OdOldWpl].pos);
     daysE = pOkrOd->m_KLDate.GetDate(); daysF = LnT[Od].dat+1;
     kwota = KwotaOds(pOkrOd, daysF, daysE); if (kwota==0) break;

     { pNal = GetParentNal(pOkrOd);  CDate datN(pNal->m_nalDate);
	 if (pzW==NULL || datN > LnT[LT_Wpl].dat){
          NewK_Wpl(typ,6,LnT[OdOldWpl].pos,kwota,0); 
		  break; 
	 }
     }
     kwota+=RemoveK_Wpl(LnT[OdOldWpl].pos);
     RealNadplata(pzW,LnT[OdOldWpl].pos,typ, pOkrOd->m_KLDate.GetDate(),Kw, kwota,6);
     break;
   case 5: break;     // Koniec okresu do splaty
   }
  return Kw;
}
//--------------------------------------------------
//  miesiac bankowy - 30 dniowy??
//  Wejscie:
//        daysE - data koncowa   daysF - data pochatkowa
//  Wyiscie: liczba dni
//
//--------------------------------------------------
//  miesiac bankowy - 30 dniowy
//  Wejscie:  /daysF/ - data pochatkowa  /daysE/ - data koncowa
//  Wyiscie: liczba dni
//
DWORD mc_30(DWORD daysF, DWORD daysE)
{
  DWORD dni;
  CDate Begin, End;
  CString Cbeg, Cend, Ctemp, Ctemp0;
  int yearBeg, yearEnd;
  
	dni = daysE - daysF+1;
  
	Begin = daysF;
  Begin.DateFormat(Cbeg);
  Ctemp = Cbeg.Right(2);
  Ctemp0 = Cbeg.Left(6);
  yearBeg = atoi(Ctemp);
	if (yearBeg < 50) Cbeg = "20"+Ctemp;
	else		Cbeg = "19"+Ctemp;
  Cbeg +=  Ctemp0.Right(4) + Ctemp0.Left(2);
  
	End   = daysE;
  End.DateFormat(Cend);
  if (Cend[0] == '-') Cend = "2020-12-31";
	else {
		Ctemp = Cend.Right(2);
		Ctemp0 = Cend.Left(6);
  	yearEnd = atoi(Ctemp);
		if (yearEnd < 50) Cend = "20"+Ctemp;
		else		Cend = "19"+Ctemp;
	  Cend += Ctemp0.Right(4) + Ctemp0.Left(2);
	}
	if ((Cend >="1972-02-29") && (Cbeg <= "1972-02-29")) dni--;
	if ((Cend >="1976-02-29") && (Cbeg <= "1976-02-29")) dni--;
	if ((Cend >="1980-02-29") && (Cbeg <= "1980-02-29")) dni--;
	if ((Cend >="1984-02-29") && (Cbeg <= "1984-02-29")) dni--;
	if ((Cend >="1988-02-29") && (Cbeg <= "1988-02-29")) dni--;
	if ((Cend >="1992-02-29") && (Cbeg <= "1992-02-29")) dni--;
	if ((Cend >="1996-02-29") && (Cbeg <= "1996-02-29")) dni--;
	if ((Cend >="2000-02-29") && (Cbeg <= "2000-02-29")) dni--; 
	if ((Cend >="2004-02-29") && (Cbeg <= "2004-02-29")) dni--;
	if ((Cend >="2008-02-29") && (Cbeg <= "2008-02-29")) dni--;
	if ((Cend >="2012-02-29") && (Cbeg <= "2012-02-29")) dni--;
	if ((Cend >="2016-02-29") && (Cbeg <= "2016-02-29")) dni--;
	if ((Cend >="2020-02-29") && (Cbeg <= "2020-02-29")) dni--;
	if ((Cend >="2024-02-29") && (Cbeg <= "2024-02-29")) dni--;
	if ((Cend >="2028-02-29") && (Cbeg <= "2028-02-29")) dni--;

	return(dni);
}


//------------------------------------------------
double CKmDoc::KwotaOds(COkrOd* pOkrOd, DWORD daysF, DWORD daysE)
{
  double odc=0,Nal;
  DWORD dni, daysEi;
  CNal* pNal;
  POSITION pos,posK;
  CK_Wpl* pKW;
  int i;

  f_ods=0.0;  // 97-01-22
  if ( pOkrOd->m_KLDate < daysF || pOkrOd->m_PLDate > daysE )
     return 0;

  for ( pos = GetFirstNalPos(); pos != NULL; GetNextNal(pos)) {
    pNal = (CNal*)m_nalList.GetAt(pos);
    if (pNal->m_nalNum == pOkrOd->m_okrParent) break;
  }

  Nal = -1.0;
  for ( posK = GetFirstK_WplPos(); posK != NULL; GetNextK_Wpl(posK)) {
    pKW = (CK_Wpl*)m_k_Wpl.GetAt(posK);
    if (pKW->m_Pos == pos )  {
      Nal = pKW->m_Kwota;
      break;
    }
  }
  if (Nal == -1.0) {
    if (pNal->m_LDate==0L) return 0L;
    Nal=(double)pNal->m_nalGroszy;
  }

  if (pOkrOd->m_okrTyp == 1) { // UMN
    if (flag_typ_month == 0)  { // ordinary
      dni = (DWORD)(daysE - daysF+1);
      if (pOkrOd->m_okrStopaTyp == 0) {     //M
        f_ods = Nal * (double)dni * (double)pOkrOd->m_okrStopa*(double)12./
                (double)365000000.;
      }
      else if (pOkrOd->m_okrStopaTyp == 1) { //R
        f_ods = Nal * (double)dni * (double)pOkrOd->m_okrStopa/(double)365000000.;
      }
      else { //D
        f_ods = Nal * (double)dni * (double)pOkrOd->m_okrStopa/(double)1000000.;
      }
    }
    else  {                      // bankowy
      dni = (DWORD)mc_30(daysF, daysE);
      if (pOkrOd->m_okrStopaTyp == 0) {     //M
        f_ods = Nal * (double)dni * (double)pOkrOd->m_okrStopa*(double)12./
                (double)365000000.;
      }
      else if (pOkrOd->m_okrStopaTyp == 1) { //R
        f_ods = Nal * (double)dni * (double)pOkrOd->m_okrStopa/(double)365000000.;
      }
      else { //D
        f_ods = Nal * (double)dni * (double)pOkrOd->m_okrStopa/(double)1000000.;
      }
    }
  }
  else if (pOkrOd->m_okrTyp == 0) { //UST
    if (daysF < (DWORD)OOUV[0].lDP)
      daysF = OOUV[0].lDP;          // period before 1970 without percents
    for (i=0; i< IleOkrOdsUst; i++)  {
      if ((i< IleOkrOdsUst-1) && (daysF >= (DWORD)OOUV[i+1].lDP)) continue;
      if (i< IleOkrOdsUst-1)
        daysEi =  (daysE >=(DWORD)OOUV[i+1].lDP) ? OOUV[i+1].lDP-1 : daysE;
      else                          // last interval till 1999
        daysEi = daysE;
      if (flag_typ_month == 0)  { // ordinary
        dni = (DWORD)(daysEi - daysF+1);
        f_ods += Nal * (double)dni * (double)OOUV[i].Stopa/(double)3650000.; // R
      }
      else  {                      // bankowy
        dni = (DWORD)mc_30(daysF,daysEi);
        f_ods += Nal * (double)dni * (double)OOUV[i].Stopa/(double)3650000.; // R
      }
      if (daysEi == daysE) break;
      daysF = daysEi+1;
    }  // for i
  }
  return (f_ods);
}

//------------------------------------------------
double CKmDoc::RealWplaty(double Kw)
{
double Kwota;
BYTE Sposob;
CWpl* pWpl;
  pWpl = (CWpl*)m_wplList.GetAt(LnT[LT_Wpl].pos);
  TSLine(pWpl);
  if (pWpl->m_wplGroszy == -1L) {
    TS[tsActive].kw = Kw;      // ??? Nie jasne
    Kwota = Kw;
  }
  else {
    TS[tsActive].kw = (double)pWpl->m_wplGroszy+Kw;
    Kwota = (double)pWpl->m_wplGroszy + Kw;
  }
  pWpl->m_ZostaloGroszy=0L;
  Sposob = pWpl->m_wplTyp;
  if (Sposob<2) {      //  KON || KNO
    if ((Kwota = Plata(8,(CWpl *)pWpl,Kwota))==0) return Kwota;
    if (Sposob==0) {    // KON
      if ((Kwota = Plata(1,(CWpl *)pWpl,Kwota))==0) return Kwota;
      if ((Kwota = Plata(0,(CWpl *)pWpl,Kwota))==0) return Kwota;
    }
    else {              // KNO
      Kwota = Plata(0,(CWpl *)pWpl,Kwota);
      if ((Kwota = Plata(1,(CWpl *)pWpl,Kwota))==0) return Kwota;
    }
  }
  else {
    if (Sposob<4) {      //  OKN || ONK
      if ((Kwota = Plata(1,(CWpl *)pWpl,Kwota))==0) return Kwota;
      if (Sposob==2) {    // OKN
        if ((Kwota = Plata(8,(CWpl *)pWpl,Kwota))==0) return Kwota;
        if ((Kwota = Plata(0,(CWpl *)pWpl,Kwota))==0) return Kwota;
      }
      else {              // ONK
        if ((Kwota = Plata(0,(CWpl *)pWpl,Kwota))==0) return Kwota;
        if ((Kwota = Plata(8,(CWpl *)pWpl,Kwota))==0) return Kwota;
      }
    }
    else {            // NKO || NOK
      if ((Kwota = Plata(0,(CWpl *)pWpl,Kwota))==0)
        { Kwota = Plata(1,(CWpl *)pWpl,Kwota); return Kwota; }
      if (Sposob==4) {    // NKO
        Kwota = Plata(8,(CWpl *)pWpl,Kwota);
        if ((Kwota = Plata(1,(CWpl *)pWpl,Kwota))==0) return Kwota;
      }
      else {               // NOK
        if ((Kwota = Plata(1,(CWpl *)pWpl,Kwota))==0) return Kwota;
        if ((Kwota = Plata(8,(CWpl *)pWpl,Kwota))==0) return Kwota;
      }
    }  // Sposob >3
  }  // Sposob >1
  pWpl->m_ZostaloGroszy = (DWORD)Kwota;
  return Kwota;
}

//------------------------------------------------
void CKmDoc::PoWplate()
{
 COkrOd* pOkrOd;
 CNal* pNal;
 double dwSum=0;
 CK_Wpl* pKW;
 POSITION posN,posK;
  for ( posN = GetFirstNalPos(); posN != NULL; GetNextNal(posN)) {
    pNal = (CNal*)m_nalList.GetAt(posN);
    if (GetStrDate(pNal->m_nalDate) > GetStrDate(m_stanDate)) continue;
    if (pNal->m_nalTyp !=T_NALEZNOSC &&
				pNal->m_nalTyp !=T_NALEZ_SPLACONA &&
				pNal->m_nalTyp!=T_KWOTA_ZAB ) continue;
    dwSum=0;
    if (pNal->m_nalTyp ==T_NALEZNOSC || pNal->m_nalTyp ==T_KWOTA_ZAB) {
      for ( posK = GetFirstK_WplPos(); posK != NULL; GetNextK_Wpl(posK)) {
        pKW = (CK_Wpl*)m_k_Wpl.GetAt(posK);
        if (pKW->m_Typ) continue;
        if (posN != pKW->m_Pos) continue;
        dwSum+=pKW->m_Kwota; break;
      }
    }
    for ( posK = GetFirstK_WplPos(); posK != NULL; GetNextK_Wpl(posK)) {
      pKW = (CK_Wpl*)m_k_Wpl.GetAt(posK);
      if ((pKW->m_Typ&3)==0) continue;
      pOkrOd = (COkrOd*)m_odsList.GetAt(pKW->m_Pos);
      if (pNal->m_nalNum != pOkrOd->m_okrParent) continue;
      dwSum+=pKW->m_Kwota;
    }
    if (dwSum!=0) continue;   // Next Nal

    if (pNal->m_nalTyp ==T_NALEZNOSC || pNal->m_nalTyp ==T_KWOTA_ZAB) {
      for ( posK = GetFirstK_WplPos(); posK != NULL; GetNextK_Wpl(posK)) {
        pKW = (CK_Wpl*)m_k_Wpl.GetAt(posK);
        if (pKW->m_Typ) continue;
        if (posN != pKW->m_Pos) continue;
        m_k_Wpl.RemoveAt(posK);
        delete pKW;
        break;
      }
    }
cc2:
    for ( posK = GetFirstK_WplPos(); posK != NULL; GetNextK_Wpl(posK)) {
      pKW = (CK_Wpl*)m_k_Wpl.GetAt(posK);
      if ((pKW->m_Typ&3)==0) continue;
      pOkrOd = (COkrOd*)m_odsList.GetAt(pKW->m_Pos);
      if (pNal->m_nalNum != pOkrOd->m_okrParent) continue;
      if (pNal->m_nalTyp ==T_NALEZ_SPLACONA ) continue;
      m_k_Wpl.RemoveAt(posK);
      delete pKW; goto cc2;
    }
  }
}

//------------------------------------------------
void CKmDoc::PredDoWpl(int OdOldWpl)
{ int i;
 COkrOd* pOkrOd;
 LONG daysE, daysF;
 CK_Wpl* pKW;
 POSITION posK;
 double Rab;

  for ( posK = GetFirstK_WplPos(); posK != NULL; GetNextK_Wpl(posK)) {
    pKW = (CK_Wpl*)m_k_Wpl.GetAt(posK);
    if ( pKW->m_Typ == 1) {  // Nie koniec okresu odsetek
      for (i=OdOldWpl; i<=LT_Wpl; i++)
         if (pKW->m_Pos == LnT[i].pos ) goto next;
      pOkrOd = (COkrOd*)m_odsList.GetAt(pKW->m_Pos);
      daysE = LnT[LT_Wpl].dat; daysF = LnT[OdOldWpl-1].dat+1;
	  Rab=KwotaOds(pOkrOd, daysF, daysE);
	  SummaOdsetek+=Rab;
	  pKW->m_Kwota +=Rab;
    }
next:;
  }
  ClearKWpl();
}

//------------------------------------------------
void CKmDoc::ClearKWpl()
{  CK_Wpl* pKW;
  POSITION posK,posOld;

  while (!m_k_Wpl.IsEmpty()) {
    pKW = (CK_Wpl*)m_k_Wpl.GetHead();
    if (pKW->m_Typ >1 && pKW->m_Kwota==0)
      { m_k_Wpl.RemoveHead(); delete pKW; continue; }
    break;
  }
  for ( posK = GetFirstK_WplPos(); posK != NULL; GetNextK_Wpl(posK)) {
cir: pKW = (CK_Wpl*)m_k_Wpl.GetAt(posK);
     if ( pKW->m_Typ >1 && pKW->m_Kwota==0) {  // Pusty rekord
        posOld = posK;
        GetNextK_Wpl(posK);
        if (posK == NULL) { m_k_Wpl.RemoveTail(); delete pKW; return; }
        else m_k_Wpl.RemoveAt(posOld); delete pKW;
        goto cir;
     }
   }
}

//------------------------------------------------
double CKmDoc::Plata(int Typ,CWpl *pWpl, double Kwota)
{
CK_Wpl* pKW;
CNal* pNal;
COkrOd* pOkrOd;
double OldKw;
POSITION posK;
BYTE t;
CStanD* pStan;
	pStan = (CStanD *)m_stanList.GetTail();
  OldKw = Kwota;
  for ( posK = GetFirstK_WplPos(); posK != NULL; GetNextK_Wpl(posK)) {
     pKW = (CK_Wpl*)m_k_Wpl.GetAt(posK);
    t=pKW->m_Typ;
    if (t==2) t=1;
    if (t==0 && pKW->m_Kwota==0) continue;

    if (t == Typ || (Typ ==1 && t==4)) {
      if (Typ==1 && t==1) {
        pOkrOd = (COkrOd*)m_odsList.GetAt(pKW->m_Pos);
        pNal = GetParentNal(pOkrOd);
        if (GetStrDate(pNal->m_nalDate) > GetStrDate(pWpl->m_wplDate)) continue;
      }
      else if (t==4 && Kwota==0) continue;      //
      if ( pKW->m_Kwota <= Kwota) {  // Wszystko
        Kwota-=pKW->m_Kwota;
        pWpl->NewPo_Wpl(t,pKW->m_Pos, pKW->m_Kwota,pKW->m_Kwota);
        pKW->m_Kwota = 0;
        if (t==0) ((CNal*)m_nalList.GetAt(pKW->m_Pos))->m_LDate = 0L;
      }
      else {  // Nie wszystko moze byc splacone
        pWpl->NewPo_Wpl(t,pKW->m_Pos, Kwota,pKW->m_Kwota);
        pKW->m_Kwota -= Kwota; Kwota=0;
        if (Typ!=1 || t==4) break;
      }
    }
  }
 if (m_stanDate == pStan->m_stanD)  {
   switch (Typ) {
   case 0: TS[tsActive].poN -=(OldKw-Kwota); break;
   case 1: TS[tsActive].poO -=(OldKw-Kwota); break;
   case 8: TS[tsActive].poK -=(OldKw-Kwota); break;
   }
 }
 return (Kwota);
}

//------------------------------------------------
void CKmDoc::TSLine(CWpl *pWpl)
{
CK_Wpl* pKW;
CNal*pNal;
COkrOd* pOkrOd;
POSITION posK;
  TS[tsActive].doN = TS[tsActive].doO = TS[tsActive].doK = 0;

  for ( posK = GetFirstK_WplPos(); posK != NULL; GetNextK_Wpl(posK)) {
    pKW = (CK_Wpl*)m_k_Wpl.GetAt(posK);
    if (pKW->m_Kwota==0) continue;
    switch (pKW->m_Typ) {
    case 0: TS[tsActive].doN+=pKW->m_Kwota; break;
    case 2:  case 1:
      pOkrOd = (COkrOd*)m_odsList.GetAt(pKW->m_Pos);
      pNal = GetParentNal(pOkrOd);
      if (GetStrDate(pNal->m_nalDate) > GetStrDate(pWpl->m_wplDate)) break;
    case 4:
            TS[tsActive].doO+=pKW->m_Kwota; break;
    case 8: TS[tsActive].doK+=pKW->m_Kwota; break;
    }
  }
  TS[tsActive].poN = TS[tsActive].doN;
  TS[tsActive].poO = TS[tsActive].doO;
  TS[tsActive].poK = TS[tsActive].doK;
  TS[tsActive].dat = pWpl->m_LDate.GetDate();
  TS[tsActive].typ = 'W';
}

//--------------------------------------------------
void CKmDoc::RealNadplata(POSITION& pzW, POSITION pos, int typ, long dat, double& Kw, double kwota,int m_TypNal)
{ CWpl *pWpl;
  CStanD* pStan;
	pStan = (CStanD *)m_stanList.GetTail();
  pWpl = (CWpl*)m_wplList.GetAt(pzW);
  TS[tsActive].doN= TS[tsActive].doO= TS[tsActive].doK=
  TS[tsActive].poN= TS[tsActive].poO= TS[tsActive].poK =0;
  TS[tsActive].dat = dat;
  TS[tsActive].kw = Kw;
  TS[tsActive].typ = 'R';
  switch(typ) {
  case 0:  TS[tsActive].poN = TS[tsActive].doN =  kwota; break;
  case 8:  TS[tsActive].poK = TS[tsActive].doK =  kwota; break;
  default: TS[tsActive].poO = TS[tsActive].doO =  kwota; break;// += RemoveK_Wpl(LnT[OdOldWpl].pos);
  }
  if ( kwota <= Kw) {  // Wszystko
    Kw-=kwota;
    pWpl->NewPo_Wpl(typ,pos, kwota,kwota);
    if (Kw==0) pzW=NULL;
  }
  else {  // Nie wszystko moze byc splacone
    pWpl->NewPo_Wpl(typ,pos, Kw,kwota);  //LnT[OdOldWpl].pos
    NewK_Wpl(typ,m_TypNal,pos, kwota-Kw,0);
    Kw=0; pzW=NULL;
  }
  switch(typ) {
  case 0:   TS[tsActive].poN -=(TS[tsActive].kw-Kw); break;
  case 8:   TS[tsActive].poK -=(TS[tsActive].kw-Kw); break;
  default:  TS[tsActive].poO -=(TS[tsActive].kw-Kw); break;
  }
  if (m_stanDate == pStan->m_stanD) tsActive++;
}

////////////////////////////////////////////////////
void CKmDoc::FormOstNal()
{
CK_Wpl* pKW;
POSITION posK;
CK_Wpl* pKW1;
POSITION posK1;
CNal* pNal;
COkrOd* pOkrOd;
COkrOd* pOkrOd1;
POSITION pos,posOd;
CString   m_stanDateN;
CDate dat;
int k=0;

CountKwot=0;
m_OstatNal = 0;
dat = GetStrDate(m_stanDate)+1;
dat.DateFormat(m_stanDateN);

//Naleznosc
for ( posK = GetFirstK_WplPos(); posK != NULL; GetNextK_Wpl(posK)) {
	pKW = (CK_Wpl*)m_k_Wpl.GetAt(posK);
	if ((pKW->m_Kwota>0.0) && (pKW->m_TypNal==0 || pKW->m_TypNal==11)){
		MasOstatNal[m_OstatNal].DateN = m_stanDateN;
		MasOstatNal[m_OstatNal].OstKwota = pKW->m_Kwota;
		MasOstatNal[m_OstatNal].TypNal = pKW->m_TypNal;
		MasOstatNal[m_OstatNal].TypOds = 0;
		pNal = (CNal*)m_nalList.GetAt(pKW->m_Pos);
		m_OstatNal++;CountKwot++;
/////////////////////////////////////
		for ( pos = GetFirstOkrOdPos(); pos != NULL; GetNextOkrOd(pos)) {
			pOkrOd = (COkrOd*)m_odsList.GetAt(pos);
			if (pOkrOd->m_okrParent == pNal->m_nalNum) {
				if (GetStrDate(m_stanDate)<GetStrDate(pOkrOd->m_okrDateK)){
					if( GetStrDate(m_stanDate)>=GetStrDate(pOkrOd->m_okrDateP)) MasOstatNal[m_OstatNal].DateN = m_stanDateN;
					else MasOstatNal[m_OstatNal].DateN = pOkrOd->m_okrDateP;
					MasOstatNal[m_OstatNal].DateK = pOkrOd->m_okrDateK;
					MasOstatNal[m_OstatNal].TypOds = pOkrOd->m_okrTyp+1;
					if (pOkrOd->m_okrTyp==0) MasOstatNal[m_OstatNal].OstKwota = 0.0;
					else{
						MasOstatNal[m_OstatNal].OstKwota = pOkrOd->m_okrStopa;
						switch(pOkrOd->m_okrStopaTyp)
						{
						case 0:MasOstatNal[m_OstatNal].OstKwota = MasOstatNal[m_OstatNal].OstKwota*12;break;
						case 2:MasOstatNal[m_OstatNal].OstKwota = MasOstatNal[m_OstatNal].OstKwota*365;break;
						}
					}
					m_OstatNal++;
				}
			}
		}
/////////////////////////////////////
		k=0;
    MasOstatNal[m_OstatNal].OstKwota =0.0;
		for ( posK1 = GetFirstK_WplPos(); posK1 != NULL; GetNextK_Wpl(posK1)) {
			pKW1 = (CK_Wpl*)m_k_Wpl.GetAt(posK1);
			if ((pKW1->m_Kwota>0.0) && (pKW1->m_TypNal==6)){
				pOkrOd = (COkrOd*)m_odsList.GetAt(pKW1->m_Pos);
				if (pOkrOd->m_okrParent == pNal->m_nalNum) {
					if (k==0){
						MasOstatNal[m_OstatNal].DateN = m_stanDateN;
						MasOstatNal[m_OstatNal].TypNal = 6;
						MasOstatNal[m_OstatNal].TypOds = 0;
					}
					MasOstatNal[m_OstatNal].OstKwota += pKW1->m_Kwota;
					k++;
					pKW1->prizn = 1;
					m_k_Wpl.SetAt(posK1,pKW1);
				}
			}
		}
		if (k>0){ m_OstatNal++;CountKwot++;}
		/////////////////////////////////////
	}
}
/////////////////////////////////////////////
//Odsetki od naleznosci == 7 --T_NALEZ_SPLACONA ???
for ( pos = GetFirstNalPos(); pos != NULL; GetNextNal(pos)) {
  pNal = (CNal*)m_nalList.GetAt(pos);
	if ((pNal->m_nalGroszy>0.0) && (pNal->m_nalTyp==T_NALEZ_SPLACONA)){
		MasOstatNal[m_OstatNal].DateN = m_stanDateN;
		MasOstatNal[m_OstatNal].OstKwota = pNal->m_nalGroszy;
		MasOstatNal[m_OstatNal].TypNal = pNal->m_nalTyp;
		MasOstatNal[m_OstatNal].TypOds = 0;
		m_OstatNal++;CountKwot++;
/////////////////////////////////////
		for ( posOd = GetFirstOkrOdPos(); posOd != NULL; GetNextOkrOd(posOd)) {
			pOkrOd = (COkrOd*)m_odsList.GetAt(posOd);
			if (pOkrOd->m_okrParent == pNal->m_nalNum) {
				if (GetStrDate(m_stanDate)<GetStrDate(pOkrOd->m_okrDateK)){
					if( GetStrDate(m_stanDate)>=GetStrDate(pOkrOd->m_okrDateP)) MasOstatNal[m_OstatNal].DateN = m_stanDateN;
					else MasOstatNal[m_OstatNal].DateN = pOkrOd->m_okrDateP;
					MasOstatNal[m_OstatNal].DateK = pOkrOd->m_okrDateK;
					MasOstatNal[m_OstatNal].TypOds = pOkrOd->m_okrTyp+1;
					if (pOkrOd->m_okrTyp==0) MasOstatNal[m_OstatNal].OstKwota = 0.0;
					else{
						MasOstatNal[m_OstatNal].OstKwota = pOkrOd->m_okrStopa;
						switch(pOkrOd->m_okrStopaTyp)
						{
						case 0:MasOstatNal[m_OstatNal].OstKwota = MasOstatNal[m_OstatNal].OstKwota*12;break;
						case 2:MasOstatNal[m_OstatNal].OstKwota = MasOstatNal[m_OstatNal].OstKwota*365;break;
						}
					}
					m_OstatNal++;
				}
			}
		}
/////////////////////////////////////
		for ( posOd = GetFirstOkrOdPos(); posOd != NULL; GetNextOkrOd(posOd)) {
			pOkrOd = (COkrOd*)m_odsList.GetAt(posOd);
			if (pOkrOd->m_okrParent == pNal->m_nalNum) {
				k=0;
        MasOstatNal[m_OstatNal].OstKwota = 0.0;
				for ( posK1 = GetFirstK_WplPos(); posK1 != NULL; GetNextK_Wpl(posK1)) {
					pKW1 = (CK_Wpl*)m_k_Wpl.GetAt(posK1);
					if ((pKW1->m_Kwota>0.0) && (pKW1->m_TypNal==6)){
						pOkrOd1 = (COkrOd*)m_odsList.GetAt(pKW1->m_Pos);
						if (pOkrOd== pOkrOd1) {
							if (k==0){
								MasOstatNal[m_OstatNal].DateN = m_stanDateN;
								MasOstatNal[m_OstatNal].TypNal = 6;
								MasOstatNal[m_OstatNal].TypOds = 0;
							}
							MasOstatNal[m_OstatNal].OstKwota += pKW1->m_Kwota;
							k++;
							pKW1->prizn = 1;
							m_k_Wpl.SetAt(posK1,pKW1);
						}
					}
				}
				if (k>0){ m_OstatNal++;CountKwot++;}
			}
		}
/////////////////////////////////////
	}
}
/////////////////////////////////////////////
for ( posK = GetFirstK_WplPos(); posK != NULL; GetNextK_Wpl(posK)) {
	pKW = (CK_Wpl*)m_k_Wpl.GetAt(posK);
	if ((pKW->m_Kwota>0.0) && ((pKW->m_Typ==8) || (pKW->m_Typ==4 )||
		(pKW->m_TypNal==8 )||(pKW->m_TypNal==9 )||
		(pKW->m_TypNal==6) && (pKW->prizn==0))){
		// tak bylo - liczu ze zle (pKW->m_Typ==6) && (pKW->prizn==0)))
		MasOstatNal[m_OstatNal].DateN = m_stanDateN;
		MasOstatNal[m_OstatNal].TypNal = pKW->m_TypNal;
		MasOstatNal[m_OstatNal].OstKwota = pKW->m_Kwota;
		MasOstatNal[m_OstatNal].TypOds = 0;
		m_OstatNal++;CountKwot++;
	}
}
}

/////////////////////////////////////////////
BOOL CKmDoc::DocOstNal()
{
  CFile OutFile;
  CString strGrZl;
  CString pszPathName;
  int len,j,number,p;
  int i;
  char stroka[100],Bufer[100];
  pszPathName = theApp.m_lpCmdLine;
  pszPathName.TrimRight();
  len = pszPathName.GetLength();
  if (pszPathName[len-1]=='.') pszPathName += "out";
  else pszPathName += ".out";
    if (! OutFile.Open(pszPathName, CFile::modeCreate | CFile::modeWrite))
   { TRACE ("Brak open file return");   return FALSE; }
	Bufer[0] = m_stanDate[0];
	Bufer[1] = m_stanDate[1];
	Bufer[2] = '/';
	Bufer[3] = m_stanDate[3];
	Bufer[4] = m_stanDate[4];
	Bufer[5] = '/';
	if (m_stanDate[6]>'5') {Bufer[6] = '1';Bufer[7] = '9';}
	else {Bufer[6] = '2';Bufer[7] = '0';}
	Bufer[8] = m_stanDate[6];
	Bufer[9] = m_stanDate[7];
	Bufer[10]='\r';
	Bufer[11]='\n';
	Bufer[12]='\0';
	OutFile.Write(Bufer,12);
	len =m_Sprawa.GetLength();
	for (i=0;i<len;i++) {Bufer[i]=m_Sprawa[i]; if (Bufer[i]=='_') Bufer[i]='/';}
	if (grupa>32) i--;
	Bufer[i++]='\r';
	Bufer[i++]='\n';
	Bufer[i]='\0';
	OutFile.Write(Bufer,i);
	len=0;
	if (grupa>32) Bufer[len++]=grupa; 
	Bufer[len++]='\r';
	Bufer[len++]='\n';
	Bufer[len]='\0';
	OutFile.Write(Bufer,len);
	if (flag_typ_month==0) Bufer[0]='1';else Bufer[0]='2';
	Bufer[1]='\r';
	Bufer[2]='\n';
	Bufer[3]='\0';
	OutFile.Write(Bufer,3);
/////////////////////////////////////////
	itoa(CountKwotI,Bufer,10);
	len=strlen(Bufer);
	Bufer[len++]='\r';
	Bufer[len++]='\n';
	Bufer[len]='\0';
	OutFile.Write(Bufer,len);
	for (i=0;i<m_OstatNalI;i++)
	{
		if(MasOstatNalI[i].TypOds == 0){
			Bufer[0] = MasOstatNalI[i].DateN[0];
			Bufer[1] = MasOstatNalI[i].DateN[1];
			Bufer[2] = '/';
			Bufer[3] = MasOstatNalI[i].DateN[3];
			Bufer[4] = MasOstatNalI[i].DateN[4];
			Bufer[5] = '/';
			if (MasOstatNalI[i].DateN[6]>'5') {Bufer[6] = '1';Bufer[7] = '9';}
			else {Bufer[6] = '2';Bufer[7] = '0';}
			Bufer[8] = MasOstatNalI[i].DateN[6];
			Bufer[9] =MasOstatNalI[i].DateN[7];
			Bufer[10]=',';
			switch(MasOstatNalI[i].TypNal){
			case  0:number=5;break;
			case  1:number=8;break;
			case  2:number=9;break;
			case  3: if (m_Sprawa[0] =='M' )
									number=10;
								else 
									number=3;
								break;
			case  4:number=11;break;
			case  5:number=4;break;
			case  6:number=7;break;
			case  7:number=6;break;//***
			case  8:number=1;break;
			case  9:number=2;break;					
			case  10:number=12;break;					
			case  11:number=21;break;					
			}
			itoa(number,stroka,10);
			len=strlen(stroka);
			for (j=0;j<len;j++) Bufer[11+j]=stroka[j];
			len+=11;Bufer[len]=',';len++;
			strGrZl = GetDCFormatted(MasOstatNalI[i].OstKwota);
			number=strGrZl.GetLength();
			if (strGrZl.GetAt(0)==' ') {
					number--;
					strGrZl =strGrZl.Right(number);
				}
			if (MasOstatNalI[i].OstKwota>=1.0E+11){
				p = strGrZl.Find((TCHAR)'.');
				number--;
				strGrZl =strGrZl.Left(p) + strGrZl.Right(number-p);
			}
		
			if (MasOstatNalI[i].OstKwota>=1.0E+8){
				p = strGrZl.Find((TCHAR)'.');
				number--;
				strGrZl =strGrZl.Left(p) + strGrZl.Right(number-p);
			}
		
			if (MasOstatNalI[i].OstKwota>=1.0E+5){
				p = strGrZl.Find((TCHAR)'.');
				number--;
				strGrZl =strGrZl.Left(p) + strGrZl.Right(number-p);
			}
			for (j=0;j<number;j++) { 
				Bufer[len+j]=strGrZl.GetAt(j);if(Bufer[len+j]==',') Bufer[len+j]='.';
			}
			len+=number;
			Bufer[len++]='\r';
			Bufer[len++]='\n';
			Bufer[len]='\0';
			OutFile.Write(Bufer,len);
		}
		else{
			Bufer[0] =' '; 
			Bufer[1] = MasOstatNalI[i].DateN[0];
			Bufer[2] = MasOstatNalI[i].DateN[1];
			Bufer[3] = '/';
			Bufer[4] = MasOstatNalI[i].DateN[3];
			Bufer[5] = MasOstatNalI[i].DateN[4];
			Bufer[6] = '/';
			if (MasOstatNalI[i].DateN[6]>'5') {Bufer[7] = '1';Bufer[8] = '9';}
			else {Bufer[7] = '2';Bufer[8] = '0';}
			Bufer[9] = MasOstatNalI[i].DateN[6];
			Bufer[10] =MasOstatNalI[i].DateN[7];
			Bufer[11]=',';
			if (MasOstatNalI[i].DateK[0]=='-'){
				Bufer[12] = '-';
				Bufer[13] = '-';
				Bufer[14] = '/';
				Bufer[15] = '-';
				Bufer[16] = '-';
				Bufer[17] = '/';
				Bufer[18] = '-';
				Bufer[19] = '-';
				Bufer[20] = '-';
				Bufer[21] = '-';
			}
			else{
			Bufer[12] = MasOstatNalI[i].DateK[0];
			Bufer[13] = MasOstatNalI[i].DateK[1];
			Bufer[14] = '/';
			Bufer[15] = MasOstatNalI[i].DateK[3];
			Bufer[16] = MasOstatNalI[i].DateK[4];
			Bufer[17] = '/';
			if (MasOstatNalI[i].DateK[6]>'5') {Bufer[18] = '1';Bufer[19] = '9';}
			else {Bufer[18] = '2';Bufer[19] = '0';}
			Bufer[20] = MasOstatNalI[i].DateK[6];
			Bufer[21] = MasOstatNalI[i].DateK[7];
			}
			Bufer[22]=',';
			itoa(MasOstatNalI[i].TypOds,stroka,10);
			len=strlen(stroka);
			for (j=0;j<len;j++) Bufer[23+j]=stroka[j];
			len+=23;
			if (MasOstatNalI[i].TypOds==2){
				Bufer[len]=',';len++;
				strGrZl = GetStopaFormattedD((DWORD)MasOstatNalI[i].OstKwota);
				number=strGrZl.GetLength();
				if (strGrZl.GetAt(0)==' ') {
					number--;
					strGrZl =strGrZl.Right(number);
				}
				for (j=0;j<number;j++) { 
					Bufer[len+j]=strGrZl.GetAt(j);if(Bufer[len+j]==',') Bufer[len+j]='.';
				}
				len+=number;
			}
			Bufer[len++]='\r';
			Bufer[len++]='\n';
			Bufer[len]='\0';
			OutFile.Write(Bufer,len);
		}
	}
			for (i=0;i<20;i++) Bufer[i] ='*'; 
			len=10;
			Bufer[len++]='\r';
			Bufer[len++]='\n';
			Bufer[len]='\0';
			OutFile.Write(Bufer,len);
/////////////////////////////////////////
	itoa(CountKwot,Bufer,10);
	len=strlen(Bufer);
	Bufer[len++]='\r';
	Bufer[len++]='\n';
	Bufer[len]='\0';
	OutFile.Write(Bufer,len);
	for (i=0;i<m_OstatNal;i++)
	{
		if(MasOstatNal[i].TypOds == 0){
			Bufer[0] = MasOstatNal[i].DateN[0];
			Bufer[1] = MasOstatNal[i].DateN[1];
			Bufer[2] = '/';
			Bufer[3] = MasOstatNal[i].DateN[3];
			Bufer[4] = MasOstatNal[i].DateN[4];
			Bufer[5] = '/';
			if (MasOstatNal[i].DateN[6]>'5') {Bufer[6] = '1';Bufer[7] = '9';}
			else {Bufer[6] = '2';Bufer[7] = '0';}
			Bufer[8] = MasOstatNal[i].DateN[6];
			Bufer[9] =MasOstatNal[i].DateN[7];
			Bufer[10]=',';
			switch(MasOstatNal[i].TypNal){
			case  0:number=5;break;
			case  1:number=8;break;
			case  2:number=9;break;
			case  3: if (m_Sprawa[0] =='M' )
									number=10;
								else 
									number=3;
								break;
			case  4:number=11;break;
			case  5:number=4;break;
			case  6:number=7;break;
			case  7:number=6;break;//***
			case  8:number=1;break;
			case  9:number=2;break;					
			case  10:number=12;break;					
			case  11:number=21;break;					
			}
			itoa(number,stroka,10);
			len=strlen(stroka);
			for (j=0;j<len;j++) Bufer[11+j]=stroka[j];
			len+=11;Bufer[len]=',';len++;
			strGrZl = GetDCFormatted(MasOstatNal[i].OstKwota);
			number=strGrZl.GetLength();
			if (strGrZl.GetAt(0)==' ') {
					number--;
					strGrZl =strGrZl.Right(number);
				}
			if (MasOstatNal[i].OstKwota>=1.0E+11){
				p = strGrZl.Find((TCHAR)'.');
				number--;
				strGrZl =strGrZl.Left(p) + strGrZl.Right(number-p);
			}
		
			if (MasOstatNal[i].OstKwota>=1.0E+8){
				p = strGrZl.Find((TCHAR)'.');
				number--;
				strGrZl =strGrZl.Left(p) + strGrZl.Right(number-p);
			}
		
			if (MasOstatNal[i].OstKwota>=1.0E+5){
				p = strGrZl.Find((TCHAR)'.');
				number--;
				strGrZl =strGrZl.Left(p) + strGrZl.Right(number-p);
			}
			for (j=0;j<number;j++) { 
				Bufer[len+j]=strGrZl.GetAt(j);if(Bufer[len+j]==',') Bufer[len+j]='.';
			}
			len+=number;
			Bufer[len++]='\r';
			Bufer[len++]='\n';
			Bufer[len]='\0';
			OutFile.Write(Bufer,len);
		}
		else{
			Bufer[0] =' '; 
			Bufer[1] = MasOstatNal[i].DateN[0];
			Bufer[2] = MasOstatNal[i].DateN[1];
			Bufer[3] = '/';
			Bufer[4] = MasOstatNal[i].DateN[3];
			Bufer[5] = MasOstatNal[i].DateN[4];
			Bufer[6] = '/';
			if (MasOstatNal[i].DateN[6]>'5') {Bufer[7] = '1';Bufer[8] = '9';}
			else {Bufer[7] = '2';Bufer[8] = '0';}
			Bufer[9] = MasOstatNal[i].DateN[6];
			Bufer[10] =MasOstatNal[i].DateN[7];
			Bufer[11]=',';
			if (MasOstatNal[i].DateK[0]=='-'){
				Bufer[12] = '-';
				Bufer[13] = '-';
				Bufer[14] = '/';
				Bufer[15] = '-';
				Bufer[16] = '-';
				Bufer[17] = '/';
				Bufer[18] = '-';
				Bufer[19] = '-';
				Bufer[20] = '-';
				Bufer[21] = '-';
			}
			else{
			Bufer[12] = MasOstatNal[i].DateK[0];
			Bufer[13] = MasOstatNal[i].DateK[1];
			Bufer[14] = '/';
			Bufer[15] = MasOstatNal[i].DateK[3];
			Bufer[16] = MasOstatNal[i].DateK[4];
			Bufer[17] = '/';
			if (MasOstatNal[i].DateK[6]>'5') {Bufer[18] = '1';Bufer[19] = '9';}
			else {Bufer[18] = '2';Bufer[19] = '0';}
			Bufer[20] = MasOstatNal[i].DateK[6];
			Bufer[21] = MasOstatNal[i].DateK[7];
			}
			Bufer[22]=',';
			itoa(MasOstatNal[i].TypOds,stroka,10);
			len=strlen(stroka);
			for (j=0;j<len;j++) Bufer[23+j]=stroka[j];
			len+=23;
			if (MasOstatNal[i].TypOds==2){
				Bufer[len]=',';len++;
				strGrZl = GetStopaFormattedD((DWORD)MasOstatNal[i].OstKwota);
				number=strGrZl.GetLength();
				if (strGrZl.GetAt(0)==' ') {
					number--;
					strGrZl =strGrZl.Right(number);
				}
				for (j=0;j<number;j++) { 
					Bufer[len+j]=strGrZl.GetAt(j);if(Bufer[len+j]==',') Bufer[len+j]='.';
				}
				len+=number;
			}
			Bufer[len++]='\r';
			Bufer[len++]='\n';
			Bufer[len]='\0';
			OutFile.Write(Bufer,len);
		}
	}
	Bufer[0]='.';
	Bufer[1]='\r';
	Bufer[2]='\n';
	Bufer[3]='\0';
	OutFile.Write(Bufer,3);
  OutFile.Close( );
  return TRUE;
  }

/////////////////////////
void CKmDoc::FormIsxNal(){
CNal* pNal;
POSITION pos,posOd;
COkrOd* pOkrOd;
CountKwotI=0;
m_OstatNalI = 0;

  for ( pos = GetFirstNalPos(); pos != NULL; GetNextNal(pos)) {
    pNal = (CNal*)m_nalList.GetAt(pos);
	MasOstatNalI[m_OstatNalI].DateN = pNal->m_nalDate;
	MasOstatNalI[m_OstatNalI].TypNal = pNal->m_nalTyp;
	MasOstatNalI[m_OstatNalI].TypOds = 0;
	MasOstatNalI[m_OstatNalI].OstKwota = pNal->m_nalGroszy;
	CountKwotI++;m_OstatNalI++;
//////////////////
	for ( posOd = GetFirstOkrOdPos(); posOd != NULL; GetNextOkrOd(posOd)) {
		pOkrOd = (COkrOd*)m_odsList.GetAt(posOd);
		if (pOkrOd->m_okrParent == pNal->m_nalNum) {
			MasOstatNalI[m_OstatNalI].DateN = pOkrOd->m_okrDateP;
			MasOstatNalI[m_OstatNalI].DateK = pOkrOd->m_okrDateK;
			MasOstatNalI[m_OstatNalI].TypOds = pOkrOd->m_okrTyp+1;
			if (pOkrOd->m_okrTyp==0) MasOstatNalI[m_OstatNalI].OstKwota = 0.0;
			else {
				MasOstatNalI[m_OstatNalI].OstKwota = pOkrOd->m_okrStopa;
				switch(pOkrOd->m_okrStopaTyp)
				{
				case 0:MasOstatNalI[m_OstatNalI].OstKwota = MasOstatNalI[m_OstatNalI].OstKwota*12;break;
				case 2:MasOstatNalI[m_OstatNalI].OstKwota = MasOstatNalI[m_OstatNalI].OstKwota*365;break;
				}
			}
			m_OstatNalI++;
		}
	}
  }
}
/////////////////////////////////////////////


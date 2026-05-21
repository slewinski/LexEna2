// Part of CKmDoc realization    -  Okresy Odsetkowe
//  96.06.20 10:15

//---------------------------------------------------------------
COkrOd* CKmDoc::NewOkrOd(UINT NalNo)
{ COkrOd* pOdsItem = new COkrOd();
  COkrOd* pOds;
  COkrOd* pOkrOd;
  CNal* pNal;
  date_t d;
  POSITION pos;
  CString str2;
  for ( pos = GetFirstNalPos(); pos != NULL; GetNextNal(pos)) {
    pNal = (CNal*)m_nalList.GetAt(pos);
    if (pNal->m_nalNum == NalNo ) {
      pNal->m_nalOkrCount++;
      SetModifiedFlag();
      pOdsItem->m_okrNum = pNal->m_nalOkrCount;
      pOdsItem->m_okrParent = NalNo;
      if (pNal->m_nalOkrCount==1)  pOdsItem->m_okrDateP=pNal->m_nalDate;
      else {
        for ( pos = GetFirstOkrOdPos(); pos != NULL; GetNextOkrOd(pos)) {
          pOkrOd = (COkrOd*)m_odsList.GetAt(pos);
          if (pOkrOd->m_okrParent == NalNo) pOds = pOkrOd;
        }
        if (pOds->m_okrDateK!="--------") {
          d = (date_t)GetStrDate(pOds->m_okrDateK);
          CDate datC(d+1L); datC.DateFormat(pOdsItem->m_okrDateP);
        }
        else pOdsItem->m_okrDateP = pNal->m_nalDate;
      }
      pOdsItem->m_okrDateK="--------";
      pOdsItem->m_okrStopa=0L;
	  pOdsItem->m_okrFactor=10000L; //HEJMUS 15.08.09 zmienne trzeba inicjalizowac ;)
      pOdsItem->m_okrTyp=0;
      pOdsItem->m_okrStopaTyp=1;
      m_odsList.AddTail(pOdsItem);
      m_odsActive=pOdsItem->m_okrNum;
      return pOdsItem;
    }
  }
  return NULL;
}
//---------------------------------------------------------------
void CKmDoc::DNewOkrOd()
{ COkrOd* pOkrOd;
  if (!CanOds())   { AfxMessageBox(IDS_CANNOT_ODS); return; }
  pOkrOd=NewOkrOd(m_nalActive);
  UpdateAllViews(NULL,(LPARAM)(MAKELONG(m_odsActive,0)),pOkrOd);
  CDlgOkr DOkr;
  DOkr.m_pDoc = this;
  if (DOkr.DoModal()!=IDOK)  RemoveOkrOd(m_odsActive);
}

//---------------------------------------------------------------
CNal* CKmDoc::GetParentNal(COkrOd* pOds)
{ CNal* pNal;
  POSITION pos;
  for ( pos = GetFirstNalPos(); pos != NULL; GetNextNal(pos)) {
    pNal = (CNal*)m_nalList.GetAt(pos);
    if (pNal->m_nalNum == pOds->m_okrParent) return pNal;
  }
  return NULL;
}

//---------------------------------------------------------------
POSITION CKmDoc::GetFirstOkrOdPos()  { return m_odsList.GetHeadPosition(); }
//---------------------------------------------------------------
COkrOd* CKmDoc::GetNextOkrOd(POSITION& pos) { return (COkrOd*)m_odsList.GetNext(pos); }
//---------------------------------------------------------------
UINT CKmDoc::GetActiveOkrOdNo()    { return m_odsActive; }
//---------------------------------------------------------------
UINT CKmDoc::OkrOdNoToRecordIndex(UINT nOkrOdNo ) { return (nOkrOdNo-1); }
//---------------------------------------------------------------
UINT CKmDoc::RecordIndexToOkrOdNo(UINT nRecInd)  { return (nRecInd+1); }

//---------------------------------------------------------------
UINT CKmDoc::GetOkrCount(UINT NalNo)
{
  CNal* pNal;
  POSITION pos;
  for ( pos = GetFirstNalPos(); pos != NULL; GetNextNal(pos)) {
    pNal = (CNal*)m_nalList.GetAt(pos);
    if (pNal->m_nalNum == NalNo ) { return pNal->m_nalOkrCount; }
  }
  return (0);
}

//---------------------------------------------------------------
BOOL CKmDoc::CanOds()
{ CNal* pNal;
  POSITION pos;
  for ( pos = GetFirstNalPos(); pos != NULL; GetNextNal(pos)) {
    pNal = (CNal*)m_nalList.GetAt(pos);
    if (pNal->m_nalNum == m_nalActive )
     return	(pNal->m_nalTyp ==T_NALEZNOSC ||
							pNal->m_nalTyp ==T_NALEZ_SPLACONA ||
							pNal->m_nalTyp ==T_KWOTA_ZAB);
  }
  return (FALSE);
}

//---------------------------------------------------------------
void CKmDoc::UpdateActiveOkrOd(UINT NalOld,UINT NalNew)
{
  if (NalOld==NalNew) return;
  COkrOd* pOkrOd;
  POSITION pos;
  for ( pos = GetFirstOkrOdPos(); pos != NULL; GetNextOkrOd(pos)) {
    pOkrOd = (COkrOd*)m_odsList.GetAt(pos);
    if (pOkrOd->m_okrParent == NalNew) {
      m_odsActive = pOkrOd->m_okrNum;
      return;
    }
  }
  m_odsActive =0;
}

//---------------------------------------------------------------
void CKmDoc::RemoveOkrOd(UINT OkrOdNo)
{ COkrOd* pOkrOd;
  CNal* pNal;
  POSITION pos;
  for ( pos = GetFirstOkrOdPos(); pos != NULL; GetNextOkrOd(pos)) {
    pOkrOd = (COkrOd*)m_odsList.GetAt(pos);
    if (pOkrOd->m_okrNum == OkrOdNo && pOkrOd->m_okrParent == m_nalActive) {
      pNal = GetParentNal(pOkrOd);
      pNal->m_nalOkrCount--;
      m_odsList.RemoveAt(pos);
      delete pOkrOd;
      SetModifiedFlag();
      for ( pos = GetFirstOkrOdPos(); pos != NULL; GetNextOkrOd(pos)) {
        pOkrOd = (COkrOd*)m_odsList.GetAt(pos);
        if (pOkrOd->m_okrParent == m_nalActive && pOkrOd->m_okrNum > OkrOdNo)
          pOkrOd->m_okrNum--;
      }
       if ((int)m_odsActive > pNal->m_nalOkrCount)  {
        m_odsActive--;
        if (m_odsActive) UpdateAllViews(NULL,(LPARAM)(MAKELONG(m_odsActive,0)),pOkrOd);
        else            UpdateAllViews(NULL,NULL,NULL);
      }
      else              UpdateAllViews(NULL,NULL,NULL);
      return;
    }
  }
}

//---------------------------------------------------------------
COkrOd* CKmDoc::GetOkrOd( UINT OkrOdNo,UINT ParentNo,DWORD& dwStopa,DWORD& dwFactor,
                        int& nTypO,int& nTypW,CString& strDateP,CString& strDateK)
{ COkrOd* pOkrOd;
  POSITION pos;
  for ( pos = GetFirstOkrOdPos(); pos != NULL; GetNextOkrOd(pos)) {
    pOkrOd = (COkrOd*)m_odsList.GetAt(pos);
    if (pOkrOd->m_okrNum == OkrOdNo && pOkrOd->m_okrParent == ParentNo) {
      dwStopa = pOkrOd->m_okrStopa;
      dwFactor = pOkrOd->m_okrFactor;
      nTypW = (int)pOkrOd->m_okrStopaTyp;
      nTypO = (int)pOkrOd->m_okrTyp;
      strDateP = pOkrOd->m_okrDateP;
      strDateK = pOkrOd->m_okrDateK;
      return pOkrOd;
    }
  }
  dwStopa = 0L; nTypW = 1; nTypO = 0; strDateP = "";strDateK = "--------";
  return NULL;
}

//---------------------------------------------------------------
void CKmDoc::UpdateOkrOd(CView* pSrcView,UINT OkrOdNo,UINT ParentNo,
              DWORD& dwStopa, DWORD& dwFactor,int& nTypO,int& nTypW,CString& strDateP,CString& strDateK)
{ COkrOd* pOkrOd;
  POSITION pos;
  for ( pos = GetFirstOkrOdPos(); pos != NULL; GetNextOkrOd(pos)) {
    pOkrOd = (COkrOd*)m_odsList.GetAt(pos);
    if (pOkrOd->m_okrNum == OkrOdNo && pOkrOd->m_okrParent == ParentNo) {
	  pOkrOd->m_okrStopa = dwStopa; // R - 00 / 100;  //Marzena
	  pOkrOd->m_okrFactor= dwFactor; 

      pOkrOd->m_okrStopaTyp = (BYTE)nTypW;
      pOkrOd->m_okrTyp = (BYTE)nTypO;
      pOkrOd->m_okrDateP = strDateP;
      pOkrOd->m_okrDateK = strDateK;
      UpdateAllViews(NULL,(LPARAM)(MAKELONG(OkrOdNo,0)),pOkrOd);
      SetModifiedFlag();
      return;
    }
  }
}

//---------------------------------------------------------------
void CKmDoc::ChangeSelectionNextOkrOdNo(BOOL bNext)
{
  if (bNext) {     // Next
    if (m_odsActive < GetOkrCount(m_nalActive)) { ++m_odsActive; }
  }
  else  {           // Prev
    if (m_odsActive>1) { --m_odsActive; }
  }
  COkrOd* pOkr=(COkrOd*)m_odsList.GetHead();
  UpdateAllViews(NULL,(LPARAM)(MAKELONG(m_odsActive,0)),pOkr);
}

//---------------------------------------------------------------
void CKmDoc::ChangeSelectionToOkrOdNo(UINT nNewActOkrOdNo)
{ UINT OldAct;
  OldAct = m_odsActive;
  m_odsActive = OkrOdNoToRecordIndex( nNewActOkrOdNo );
  COkrOd* pOkr=(COkrOd*)m_odsList.GetHead();
  UpdateAllViews(NULL,(LPARAM)(MAKELONG(m_odsActive,0)),pOkr);
}

//---------------------------------------------------------------
void CKmDoc::DelOkrOd()
{  CString strD;
char buf[100];
  if (!CanOds() || GetOkrCount(m_nalActive)==0) return;
  strD.LoadString(IDS_P_DEL_OKROD);
  wsprintf(buf,(const char *)strD,m_odsActive);
    switch (AfxMessageBox(buf, MB_YESNO)) {
    case IDCANCEL:
    case IDNO:  return;
  }
  RemoveOkrOd(m_odsActive);
}

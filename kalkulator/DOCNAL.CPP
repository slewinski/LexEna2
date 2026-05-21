//--11-20-96 10:41pm---------------------------------------------
// Part of CKmDoc realization    -  Naleznosci

//---------------------------------------------------------------
CNal* CKmDoc::NewNal()
{ CNal* pNalItem = new CNal();
  CNal*pNal;
  pNalItem->m_nalNum = m_nalList.GetCount()+1;
  if (pNalItem->m_nalNum==1)
    pNalItem->m_nalDate = theApp.m_Date;
  else {     
    pNal = (CNal*)m_nalList.GetTail();
    pNalItem->m_nalDate = pNal->m_nalDate;
  }

  pNalItem->m_nalGroszy = 0L;
  pNalItem->m_nalTyp = 0;
  pNalItem->m_nalOkrCount=0;
  m_nalList.AddTail(pNalItem);
  SetModifiedFlag();   // Mark document as modified to confirm File Close
  return pNalItem;
}

//---------------------------------------------------------------
void CKmDoc::DNewNal()
{  CNal* pNal;
   pNal = NewNal();
   m_nalActive = pNal->m_nalNum;
   UpdateAllViews(NULL,(LPARAM)(MAKELONG(m_nalActive,0)),pNal);
   CDlgNal DNal;
   DNal.m_pDoc = this;
	 DNal.m_skrOds = 0;
   if (DNal.DoModal()!=IDOK)  RemoveNal(m_nalActive);
}

//---------------------------------------------------------------
POSITION CKmDoc::GetFirstNalPos()  { return m_nalList.GetHeadPosition(); }
//---------------------------------------------------------------
CNal* CKmDoc::GetNextNal(POSITION& pos) { return (CNal*)m_nalList.GetNext(pos); }
//---------------------------------------------------------------
UINT CKmDoc::GetActiveNalNo()    { return m_nalActive; }
//---------------------------------------------------------------
UINT CKmDoc::NalNoToRecordIndex(UINT nNalNo ) { return (nNalNo-1); }
//---------------------------------------------------------------
UINT CKmDoc::RecordIndexToNalNo(UINT nRecInd)  { return (nRecInd+1); }

//---------------------------------------------------------------
int CKmDoc::GetNal(UINT NalNo, DWORD& dwGroszy, int& nTypN, CString& strDate)
{
  CNal* pNal;
  POSITION pos;
  for ( pos = GetFirstNalPos(); pos != NULL; GetNextNal(pos)) {
    pNal = (CNal*)m_nalList.GetAt(pos);
    if (pNal->m_nalNum == NalNo ) {
      dwGroszy = pNal->m_nalGroszy;
      nTypN = (int)pNal->m_nalTyp;
      strDate = pNal->m_nalDate;
      return pNal->m_nalOkrCount;
    }
  }
  dwGroszy = 0L; nTypN = 0;  strDate = "--------";
  return 0;
}

//---------------------------------------------------------------
void CKmDoc::UpdateNal(CView* pSrcView,
              UINT NalNo,DWORD& dwGroszy,int& nTypN,CString& strDate,int n)
{
  CNal* pNal;
  POSITION pos,pos1;
  COkrOd* pOkrOd;
   for ( pos = GetFirstNalPos(); pos != NULL; GetNextNal(pos)) {
    pNal = (CNal*)m_nalList.GetAt(pos);
    if (pNal->m_nalNum == NalNo ) {
      pNal->m_nalGroszy = dwGroszy;
      pNal->m_nalTyp = (BYTE)nTypN;

      pNal->m_nalDate = strDate;
      if (pNal->m_nalOkrCount &&
        pNal->m_nalTyp!=T_NALEZNOSC &&
				pNal->m_nalTyp!=T_NALEZ_SPLACONA &&
				pNal->m_nalTyp!=T_KWOTA_ZAB) {
remo:
        for ( pos1 = GetFirstOkrOdPos(); pos1 != NULL; GetNextOkrOd(pos1)) {
          pOkrOd = (COkrOd*)m_odsList.GetAt(pos1);
          if (pOkrOd->m_okrParent == NalNo) {
            m_odsList.RemoveAt(pos1); delete pOkrOd; goto remo;
          }
        }
        pNal->m_nalOkrCount = 0;
      }
   //-----------------------------------------------------------------------
      UpdateAllViews(NULL,(LPARAM)(MAKELONG(NalNo,0)),pNal);
      SetModifiedFlag();   // Mark document as modified to confirm File Close
      return;
    }
  }
}

//---------------------------------------------------------------
void CKmDoc::RemoveNal(UINT NalNo)
{
  CNal* pNal;
  COkrOd* pOkrOd;
  POSITION pos,pos1;
  for ( pos = GetFirstNalPos(); pos != NULL; GetNextNal(pos)) {
    pNal = (CNal*)m_nalList.GetAt(pos);
    if (pNal->m_nalNum == NalNo ) {
      if (pNal->m_nalOkrCount) {
remo:
        for ( pos1 = GetFirstOkrOdPos(); pos1 != NULL; GetNextOkrOd(pos1)) {
          pOkrOd = (COkrOd*)m_odsList.GetAt(pos1);
          if (pOkrOd->m_okrParent == NalNo) {
            m_odsList.RemoveAt(pos1);
            delete pOkrOd;
            goto remo;
          }
        }
      }
      m_nalList.RemoveAt(pos);
      delete pNal;
      SetModifiedFlag();   // Mark document as modified to confirm File Close
      if (!m_nalList.IsEmpty()) {
        for ( pos = GetFirstNalPos(); pos != NULL; GetNextNal(pos)) {
          pNal = (CNal*)m_nalList.GetAt(pos);
          if (pNal->m_nalNum > NalNo ) pNal->m_nalNum--;
        }
        for ( pos1 = GetFirstOkrOdPos(); pos1 != NULL; GetNextOkrOd(pos1)) {
          pOkrOd = (COkrOd*)m_odsList.GetAt(pos1);
          if (pOkrOd->m_okrParent > NalNo) pOkrOd->m_okrParent--;
        }
        if ((int)m_nalActive > m_nalList.GetCount()) {
          m_nalActive--; UpdateActiveOkrOd(0xffff,m_nalActive);
          if (m_nalActive) {
            UpdateAllViews(NULL,(LPARAM)(MAKELONG(m_nalActive,0)),pNal);
            return;
          }
        }
        else UpdateActiveOkrOd(0xffff,m_nalActive);
      }
      else   m_nalActive = m_odsActive =0;
      UpdateAllViews(NULL,NULL,NULL);
      return;
    }
  }
}

//---------------------------------------------------------------
void CKmDoc::ChangeSelectionNextNalNo(BOOL bNext)
{
  if (bNext) {     // Next
    if ((int)m_nalActive < m_nalList.GetCount()) {
      ++m_nalActive;
			UpdateActiveOkrOd(m_nalActive-1,m_nalActive);
    }
  }
  else  {           // Prev
    if (m_nalActive>1) {
      --m_nalActive;
			UpdateActiveOkrOd(m_nalActive+1,m_nalActive);
    }
  }
  CNal* pNal=(CNal*)m_nalList.GetHead();
  UpdateAllViews(NULL,(LPARAM)(MAKELONG(m_nalActive,0)),pNal);
  if (GetOkrCount(m_nalActive)) {
    COkrOd* pOkr=(COkrOd*)m_odsList.GetHead();
    UpdateAllViews(NULL,(LPARAM)(MAKELONG(m_odsActive,0)),pOkr);
  }
}

//---------------------------------------------------------------
void CKmDoc::ChangeSelectionToNalNo(UINT nNewActNalNo)
{ UINT OldAct;
  OldAct = m_nalActive;
  m_nalActive = NalNoToRecordIndex( nNewActNalNo );
  UpdateActiveOkrOd(OldAct,m_nalActive);

  if (m_nalList.IsEmpty()) return; //HEJMUS
  CNal* pNal=(CNal*)m_nalList.GetHead();
  UpdateAllViews(NULL,(LPARAM)(MAKELONG(m_nalActive,0)),pNal);
  if (GetOkrCount(m_nalActive)) {
    COkrOd* pOkr=(COkrOd*)m_odsList.GetHead();
    UpdateAllViews(NULL,(LPARAM)(MAKELONG(m_odsActive,0)),pOkr);
  }
}

//---------------------------------------------------------------
void CKmDoc::DelNal()
{  CString strD;
char buf[100];
  if (m_nalList.GetCount()<1) return;
  strD.LoadString(IDS_P_DEL_NAL);
  wsprintf(buf,(const char *)strD,m_nalActive);
    switch (AfxMessageBox(buf, MB_YESNO)) {
    case IDCANCEL:
    case IDNO:  return;
  }
  RemoveNal(m_nalActive);
}

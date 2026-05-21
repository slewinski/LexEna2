// Part of CKmDoc realization    -  Wplaty
//  96.06.15 14:58
//---------------------------------------------------------------
void CKmDoc::DNewWpl()
{  CWpl* pWpl;
   pWpl = NewWpl();
   m_wplActive = pWpl->m_wplNum;
   UpdateAllViews(NULL,(LPARAM)(MAKELONG(m_wplActive,0)),pWpl);
   CDlgWpl DWpl;
   DWpl.m_pDoc = this;
   if (DWpl.DoModal()!=IDOK) RemoveWpl(m_wplActive);
}

//---------------------------------------------------------------
CWpl* CKmDoc::NewWpl()
{ CWpl* pWplItem = new CWpl();
  CWpl* pWpl;
  pWplItem->m_wplNum = m_wplList.GetCount()+1;
  if (pWplItem->m_wplNum ==1)
    pWplItem->m_wplDate = theApp.m_Date;
  else {
    pWpl = (CWpl*)m_wplList.GetTail();
    pWplItem->m_wplDate = pWpl->m_wplDate;
  }
  pWplItem->m_wplGroszy = 0L;
  pWplItem->m_wplTyp = 0;
  pWplItem->m_SQLTyp = 0;
  m_wplList.AddTail(pWplItem);
  SetModifiedFlag();   // Mark document as modified to confirm File Close
  return pWplItem;
}

//---------------------------------------------------------------
POSITION CKmDoc::GetFirstWplPos() { return m_wplList.GetHeadPosition(); }
//---------------------------------------------------------------
CWpl* CKmDoc::GetNextWpl(POSITION& pos) { return (CWpl*)m_wplList.GetNext(pos); }
//---------------------------------------------------------------
UINT CKmDoc::GetActiveWplNo()   { return m_wplActive; }
//---------------------------------------------------------------
UINT CKmDoc::WplNoToRecordIndex(UINT nWplNo ) { return (nWplNo-1); }
//---------------------------------------------------------------
UINT CKmDoc::RecordIndexToWplNo(UINT nRecInd)  { return (nRecInd+1); }

//---------------------------------------------------------------
void CKmDoc::GetWpl(UINT WplNo, DWORD& dwGroszy, int& nTypW, CString& strDate, int& nSQLTyp)
{
  CWpl* pWpl;
  POSITION pos;
  for ( pos = GetFirstWplPos(); pos != NULL; GetNextWpl(pos)) {
    pWpl = (CWpl*)m_wplList.GetAt(pos);
    if (pWpl->m_wplNum == WplNo ) {
      dwGroszy = pWpl->m_wplGroszy;
      nTypW = (int)pWpl->m_wplTyp;
      nSQLTyp = (int)pWpl->m_SQLTyp;      
	  strDate = pWpl->m_wplDate;
      return;
    }
  }
  dwGroszy = 0L; nTypW = 0;  strDate = "--------";
}

//---------------------------------------------------------------
void CKmDoc::UpdateWpl(CView* pSrcView,
              UINT WplNo,DWORD& dwGroszy,int& nTypW,CString& strDate,int& nTypSQL)
{
  CWpl* pWpl;
  POSITION pos;
  for ( pos = GetFirstWplPos(); pos != NULL; GetNextWpl(pos)) {
    pWpl = (CWpl*)m_wplList.GetAt(pos);
    if (pWpl->m_wplNum == WplNo ) {
      pWpl->m_wplGroszy = dwGroszy;
      pWpl->m_wplDate = strDate;
 	  pWpl->m_wplTyp = (BYTE)nTypW;
	  pWpl->m_SQLTyp = (BYTE)nTypSQL;
	  SortWpl(pos,strDate);
	  UpdateAllViews(pSrcView,m_wplActive,pWpl);
      SetModifiedFlag();   // Mark document as modified to confirm File Close
      return;
    }
  }
}

//---------------------------------------------------------------
void CKmDoc::SortWpl(POSITION pos1, CString& strDate)
{
  CWpl* pWpl;
  CWpl* pWpl1;
  POSITION pos;
  int i;
  for ( pos = GetFirstWplPos(); pos != NULL; GetNextWpl(pos)) {
    pWpl = (CWpl*)m_wplList.GetAt(pos);
    if (GetStrDate(pWpl->m_wplDate) > GetStrDate(strDate)) {
      pWpl1 = (CWpl*)m_wplList.GetAt(pos1);
      m_wplList.RemoveAt(pos1);
      m_wplList.InsertBefore(pos,pWpl1);
wych: i=1;
      for ( pos = GetFirstWplPos(); pos != NULL; GetNextWpl(pos)) {
        pWpl = (CWpl*)m_wplList.GetAt(pos);
        pWpl->m_wplNum = i++;
        if (GetStrDate(pWpl->m_wplDate) == GetStrDate(strDate)) m_wplActive = pWpl->m_wplNum;
      }
      return;
    }
  }
  if (pos==pos1) return;
  pWpl1 = (CWpl*)m_wplList.GetAt(pos1);
  m_wplList.RemoveAt(pos1);
  m_wplList.AddTail(pWpl1);
  goto wych;
}

//---------------------------------------------------------------
void CKmDoc::RemoveWpl(UINT WplNo)
{
  CWpl* pWpl;
  POSITION pos;
  for ( pos = GetFirstWplPos(); pos != NULL; GetNextWpl(pos)) {
    pWpl = (CWpl*)m_wplList.GetAt(pos);
    if (pWpl->m_wplNum == WplNo ) {
      m_wplList.RemoveAt(pos);
      delete pWpl;
      SetModifiedFlag();   // Mark document as modified to confirm File Close
      for ( pos = GetFirstWplPos(); pos != NULL; GetNextWpl(pos)) {
        pWpl = (CWpl*)m_wplList.GetAt(pos);
        if (pWpl->m_wplNum > WplNo ) pWpl->m_wplNum--;
      }
      if ((int)m_wplActive > m_wplList.GetCount())  {
        m_wplActive--;  
        if (m_wplActive) UpdateAllViews(NULL,(LPARAM)(MAKELONG(m_wplActive,0)),pWpl);
        else            UpdateAllViews(NULL,NULL,NULL);
      }
      else              UpdateAllViews(NULL,NULL,NULL);
      return;
    }
  }
}

//---------------------------------------------------------------
void CKmDoc::ChangeSelectionNextWplNo(BOOL bNext)
{
  if (bNext) {     // Next
    if ( (int)m_wplActive < m_wplList.GetCount()) ++m_wplActive; 
  }
  else  {           // Prev
    if (m_wplActive>1) --m_wplActive; 
  }
  if (m_wplList.IsEmpty()) return; //HEJMUS
  CWpl* pWpl=(CWpl*)m_wplList.GetHead();
  UpdateAllViews(NULL,(LPARAM)(MAKELONG(m_wplActive,0)),pWpl);
}

//---------------------------------------------------------------
void CKmDoc::ChangeSelectionToWplNo(UINT nNewActWplNo)
{ m_wplActive = WplNoToRecordIndex( nNewActWplNo );
  CWpl* pWpl=(CWpl*)m_wplList.GetHead();
  UpdateAllViews(NULL,(LPARAM)(MAKELONG(m_wplActive,0)),pWpl);
}

//---------------------------------------------------------------
void CKmDoc::DelWpl()
{ CString strD;
char buf[100];
  
  if (m_wplList.GetCount()<1) return;
  strD.LoadString(IDS_P_DEL_WPL);
  wsprintf(buf,(const char *)strD,m_wplActive);
    switch (AfxMessageBox(buf, MB_YESNO)) {
    case IDCANCEL:
    case IDNO:  return;
  }
  RemoveWpl(m_wplActive);
}  
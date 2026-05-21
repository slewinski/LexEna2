//-96.06.10 08:18------------------------------------------------
// Part of CKmDoc realization    -  K-Wplacie / Po_Wplacie

//---------------------------------------------------------------
CK_Wpl* CKmDoc::NewK_Wpl(int Typ,int TypNal,POSITION pos, double Kwota, DWORD LDate)
{ CK_Wpl* pWI = new CK_Wpl();
  pWI->m_Typ = Typ;
  pWI->m_TypNal = TypNal;
  pWI->m_Pos = pos;
  pWI->m_Kwota = ceil(Kwota);
  pWI->m_Procent = 0L;
  pWI->m_LDate0 = LDate;
  pWI->prizn = 0;
  m_k_Wpl.AddTail(pWI);
////////////////////////////////////////////////
   switch (Typ) {
   case 1: SummaOdsetek+=ceil(Kwota);break;
   case 2: SummaOdsetek+=ceil(Kwota);break;
   case 4: SummaOdsetek+=ceil(Kwota);break;
   }
////////////////////////////////////////////////
  return pWI;
}

//---------------------------------------------------------------
POSITION CKmDoc::GetFirstK_WplPos() { return m_k_Wpl.GetHeadPosition(); }
//---------------------------------------------------------------
CK_Wpl* CKmDoc::GetNextK_Wpl(POSITION& pos)
   { return (CK_Wpl*)m_k_Wpl.GetNext(pos); }

void CKmDoc::UpdateK_Wpl(POSITION pos, double Kwota)
{
CK_Wpl* pKW;
POSITION posK;
  for ( posK = GetFirstK_WplPos(); posK != NULL; GetNextK_Wpl(posK)) {
    pKW = (CK_Wpl*)m_k_Wpl.GetAt(posK);
    if (pKW->m_Pos == pos ) {
      pKW->m_Kwota += Kwota;
      pKW->m_Typ = 2;
      return;
    }
  }
}
//---------------------------------------------------------------
double CKmDoc::RemoveK_Wpl(POSITION pos)
{
CK_Wpl* pKW;
POSITION posK;
double Kw=0;
  for ( posK = GetFirstK_WplPos(); posK != NULL; GetNextK_Wpl(posK)) {
    pKW = (CK_Wpl*)m_k_Wpl.GetAt(posK);
    if (pKW->m_Pos == pos ) {
      Kw = pKW->m_Kwota;
      m_k_Wpl.RemoveAt(posK); delete pKW; break;
    }
  }
  return Kw;
}


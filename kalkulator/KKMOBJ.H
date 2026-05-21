////////////////////////////////////////////////////////////////////////
//Forward declaration of data structure class
// 96.05.4 11:54

class CNal : public CObject
{
public:
    CNal();
    DECLARE_SERIAL(CNal)
// Attributes
    UINT      m_nalNum;        // Numer Naleznosci
    CString   m_nalDate;       // Data  Naleznosci
    DWORD     m_nalGroszy;     // Kwota Naleznosci
    BYTE      m_nalTyp;        // Typ Naleznosci
    BYTE      m_nalOkrCount;   // Number of COkrOds
    CDate     m_LDate;
// Operations
public:
    virtual void Serialize(CArchive& ar);   // overridden for document i/o
};

////////////////////////////////////////////////////////////////////////////
class CWpl : public CObject
{
public:
    CWpl();
    DECLARE_SERIAL(CWpl)

//  Attributes
    UINT      m_wplNum;     // Numer Wplaty
    CString   m_wplDate;    // Data  Wplaty
    DWORD     m_wplGroszy;  // Kwota Wplaty
    BYTE      m_wplTyp;     // Typ Wplaty
    CDate     m_LDate;
    CObList   m_Po_Wpl;     // Each element is CPo_Wpl
    DWORD     m_ZostaloGroszy;  // Kwota po rozchodu Wplaty
    BYTE      m_SQLTyp;     // z SQL Ks.P. z,d,s,w, bw, bz
// Operations
public:
    CPo_Wpl* NewPo_Wpl(int Typ,POSITION pos,double Kwota,double KwotaDo);
    POSITION GetFirstPo_WplPos();
    CPo_Wpl* GetNextPo_Wpl(POSITION& pos);

     virtual void Serialize(CArchive& ar);   // overridden for document i/o
};

/////////////////////////////////////////////////////////////////////////
class COkrOd : public CObject
{
public:
    COkrOd();
    DECLARE_SERIAL(COkrOd)
// Attributes
    UINT      m_okrNum;      // Numer Okresu
    UINT      m_okrParent;   // Numer Naleznosci
    CString   m_okrDateP;    // Data okresy poczatkowa
    CString   m_okrDateK;    // Data okresy koncowa
    DWORD     m_okrStopa;    // Stopa procentowa
    DWORD     m_okrFactor;    // mnoznik
	BYTE      m_okrTyp;      // Typ Odsetok ('UST','UMN','SKR')
    BYTE      m_okrStopaTyp; // Typ Stopy ( 'M', 'R', 'D' )
    CDate     m_PLDate;
    CDate     m_KLDate;
// Operations
public:
    virtual void Serialize(CArchive& ar);   // overridden for document i/o
};

////////////////////////////////////////////////////////////////////////////
class CK_Wpl : public CObject
{
public:
    CK_Wpl();
    DECLARE_DYNCREATE(CK_Wpl)
// Attributes    
    int       m_Typ;      // Typ Wplaty : 0 - Naleznosc; 1- niekoneczny odsetki
                          // 2 - koneczny ods.; 8 - Naleznosc(Koszty)
    int       m_TypNal;
    POSITION  m_Pos;
    double    m_Kwota;   // Kwota Wplaty   
    double    m_Procent; // Skarbowe - Suma % odsetek do Wplaty
    DWORD     m_LDate0;  // Date of creation or last action
	int       prizn; 
};

////////////////////////////////////////////////////////////////////////////
class CPo_Wpl : public CObject
{
public:
    CPo_Wpl();
    DECLARE_DYNCREATE(CPo_Wpl)
// Attributes    
    int       m_Typ;      // Typ wplaconej kwoty (8=Kosz. 1=Ods. 0=Nal.)
    POSITION  m_Pos;      // Positision in CObList swoego typu
    double     m_Kwota;    // Kwota wplacona 
    double     m_KwotaDo;  // Kwota do Wplaty
};

////////////////////////////////////////////////////////////////////////////
class CStanD : public CObject
{
public:
    CStanD();
    DECLARE_SERIAL(CStanD)
// Attributes
    CString   m_stanD;       // Data obliczania stanu
// Operations
public:
    virtual void Serialize(CArchive& ar);   // overridden for document i/o
};


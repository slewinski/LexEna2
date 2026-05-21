#pragma once


// dokr dialog

class dokr : public CDialog
{
	DECLARE_DYNAMIC(dokr)

public:
	dokr(CWnd* pParent = NULL);   // standard constructor
	virtual ~dokr();

// Dialog Data
	enum { IDD = IDD_DOKR };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedRadioo0();
	afx_msg void OnBnClickedRadioo1();
	afx_msg void OnBnDoubleclickedRadioo0();
	afx_msg void OnBnDoubleclickedRadioo1();
};

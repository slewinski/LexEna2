// dokr.cpp : implementation file
//

#include "stdafx.h"
#include "km.h"
#include ".\dokr.h"


// dokr dialog

IMPLEMENT_DYNAMIC(dokr, CDialog)
dokr::dokr(CWnd* pParent /*=NULL*/)
	: CDialog(dokr::IDD, pParent)
{
}

dokr::~dokr()
{
}

void dokr::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(dokr, CDialog)
	ON_BN_CLICKED(IDC_RADIOO0, OnBnClickedRadioo0)
	ON_BN_CLICKED(IDC_RADIOO1, OnBnClickedRadioo1)
	ON_BN_DOUBLECLICKED(IDC_RADIOO0, OnBnDoubleclickedRadioo0)
	ON_BN_DOUBLECLICKED(IDC_RADIOO1, OnBnDoubleclickedRadioo1)
END_MESSAGE_MAP()


// dokr message handlers



void dokr::OnBnClickedRadioo0()
{
	// TODO: Add your control notification handler code here
		// TODO: Add your control notification handler code here
		CWnd* pWnd;
  pWnd = GetDlgItem(IDC_FACTOR);
  pWnd->ShowWindow(SW_SHOW); 
//  pWnd = GetDlgItem(IDC_STATICa);
//  pWnd->ShowWindow(SW_SHOW); 
}

void dokr::OnBnClickedRadioo1()
{
	// TODO: Add your control notification handler code here
		// TODO: Add your control notification handler code here
		CWnd* pWnd;
  pWnd = GetDlgItem(IDC_FACTOR);
  pWnd->ShowWindow(SW_HIDE); 
//  pWnd = GetDlgItem(IDC_STATICa);
//  pWnd->ShowWindow(SW_HIDE); 
}

void dokr::OnBnDoubleclickedRadioo0()
{
	// TODO: Add your control notification handler code here
			CWnd* pWnd;
  pWnd = GetDlgItem(IDC_FACTOR);
  pWnd->ShowWindow(SW_SHOW); 
}

void dokr::OnBnDoubleclickedRadioo1()
{
	// TODO: Add your control notification handler code here
			CWnd* pWnd;
  pWnd = GetDlgItem(IDC_FACTOR);
  pWnd->ShowWindow(SW_HIDE); 
}

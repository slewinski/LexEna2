; CLW file contains information for the MFC ClassWizard

[General Info]
Version=1
LastClass=CDlgOkr
LastTemplate=CDialog
NewFileInclude1=#include "stdafx.h"
NewFileInclude2=#include "km.h"
LastPage=0

ClassCount=26
Class1=CChildFrame
Class2=CPV
Class3=CDlgNal
Class4=CDlgWpl
Class5=CDlgOkr
Class6=CGetStan
Class7=CGetDate
Class8=CKmApp
Class9=CKmDoc
Class10=CKkmView
Class11=CPomoc
Class12=CAboutDlg
Class13=CMainFrame
Class14=CNagl
Class15=CStop
Class16=CNView
Class17=CNewOkr
Class18=COblView
Class19=COdsUst
Class20=COView
Class21=CRowView
Class22=CSplitterFrame
Class23=CTitleNal
Class24=CTitleWpl
Class25=CTitleOkr
Class26=CWView

ResourceCount=35
Resource1=IDR_RRRTYPE (Polish)
Resource2=IDD_DWPL (Polish)
Resource3=IDR_LIST (Polish)
Resource4=IDD_POMOC (Polish)
Resource5=IDD_NAG (Polish)
Resource6=IDD_ABOUTBOX (Polish)
Resource7=IDD_STOP (Polish)
Resource8=IDD_DATE_STAN (Polish)
Resource9=IDR_MAINFRAME (Polish)
Resource10=IDD_NEW_OKR
Resource11=IDD_DOKR (Polish)
Resource12=IDD_ODS_UST (Polish)
Resource13=IDR_TBIN (Polish)
Resource14=IDD_GET_STAN (Polish)
Resource15=IDD_DNAL (Polish)
Resource16=IDR_TBKALK (Polish)
Resource17=IDD_NEW_OKR (Polish)
Resource18=IDR_TBIN
Resource19=IDD_NAG
Resource20=IDD_DIALOG1
Resource21=IDD_DNAL
Resource22=IDD_DATE_STAN
Resource23=IDD_DOKR
Resource24=IDD_ODS_UST
Resource25=IDR_TBOUT (Polish)
Resource26=IDR_RRRTYPE
Resource27=IDD_ABOUTBOX
Resource28=IDD_STOP
Resource29=IDR_TBKALK
Resource30=IDD_POMOC
Resource31=IDD_GET_STAN
Resource32=IDR_LIST
Resource33=IDD_DWPL
Resource34=IDR_MAINFRAME
Resource35=IDR_TBOUT

[CLS:CChildFrame]
Type=0
BaseClass=CMDIChildWnd
HeaderFile=CHILDFRM.H
ImplementationFile=CHILDFRM.CPP

[CLS:CPV]
Type=0
BaseClass=CPreviewView
HeaderFile=CPV.H
ImplementationFile=CPV.CPP

[CLS:CDlgNal]
Type=0
BaseClass=CDialog
HeaderFile=DLG.H
ImplementationFile=DLG.CPP
LastObject=CDlgNal
Filter=D
VirtualFilter=dWC

[CLS:CDlgWpl]
Type=0
BaseClass=CDialog
HeaderFile=DLG.H
ImplementationFile=DLG.CPP
LastObject=IDC_DATE
Filter=D
VirtualFilter=dWC

[CLS:CDlgOkr]
Type=0
BaseClass=CDialog
HeaderFile=DLG.H
ImplementationFile=DLG.CPP
LastObject=IDC_STOPA
Filter=D
VirtualFilter=dWC

[CLS:CGetStan]
Type=0
BaseClass=CDialog
HeaderFile=GETSTAN.H
ImplementationFile=Getstan.cpp

[CLS:CGetDate]
Type=0
BaseClass=CDialog
HeaderFile=GETSTAN.H
ImplementationFile=Getstan.cpp
LastObject=CGetDate
Filter=D
VirtualFilter=dWC

[CLS:CKmApp]
Type=0
BaseClass=CWinApp
HeaderFile=KKM.H
ImplementationFile=Km.cpp

[CLS:CKmDoc]
Type=0
BaseClass=CDocument
HeaderFile=KKMDOC.H
ImplementationFile=Kmdoc.cpp
Filter=N
VirtualFilter=DC
LastObject=CKmDoc

[CLS:CKkmView]
Type=0
BaseClass=CView
HeaderFile=KKMVIEW.H
ImplementationFile=KKMVIEW.CPP

[CLS:CPomoc]
Type=0
BaseClass=CDialog
HeaderFile=Km.cpp
ImplementationFile=Km.cpp

[CLS:CAboutDlg]
Type=0
BaseClass=CDialog
HeaderFile=Km.cpp
ImplementationFile=Km.cpp
LastObject=CAboutDlg

[CLS:CMainFrame]
Type=0
BaseClass=CMDIFrameWnd
HeaderFile=MAINFRM.H
ImplementationFile=MAINFRM.CPP

[CLS:CNagl]
Type=0
BaseClass=CDialog
HeaderFile=NAGL.H
ImplementationFile=NAGL.CPP

[CLS:CStop]
Type=0
BaseClass=CDialog
HeaderFile=NAGL.H
ImplementationFile=NAGL.CPP

[CLS:CNView]
Type=0
BaseClass=CRowView
HeaderFile=NALVW.H
ImplementationFile=NALVW.CPP

[CLS:CNewOkr]
Type=0
BaseClass=CDialog
HeaderFile=NEWOKR.H
ImplementationFile=NEWOKR.CPP
Filter=D
VirtualFilter=dWC
LastObject=CNewOkr

[CLS:COblView]
Type=0
BaseClass=CRowView
HeaderFile=OBLVW.H
ImplementationFile=OBL.CPP

[CLS:COdsUst]
Type=0
BaseClass=CDialog
HeaderFile=ODSETKI.H
ImplementationFile=Odsetki.cpp
LastObject=COdsUst

[CLS:COView]
Type=0
BaseClass=CRowView
HeaderFile=ODSVW.H
ImplementationFile=ODSVW.CPP

[CLS:CRowView]
Type=0
BaseClass=CScrollView
HeaderFile=ROWVIEW.H
ImplementationFile=ROW.CPP

[CLS:CSplitterFrame]
Type=0
BaseClass=CMDIChildWnd
HeaderFile=SPLITTER.H
ImplementationFile=SPLITTER.CPP

[CLS:CTitleNal]
Type=0
BaseClass=CView
HeaderFile=TITLE.H
ImplementationFile=TITLE.CPP

[CLS:CTitleWpl]
Type=0
BaseClass=CView
HeaderFile=TITLE.H
ImplementationFile=TITLE.CPP

[CLS:CTitleOkr]
Type=0
BaseClass=CView
HeaderFile=TITLE.H
ImplementationFile=TITLE.CPP

[CLS:CWView]
Type=0
BaseClass=CRowView
HeaderFile=WPLVW.H
ImplementationFile=WPLVW.CPP

[DLG:IDD_DNAL]
Type=1
Class=CDlgNal
ControlCount=11
Control1=IDC_TYPN,combobox,1344339971
Control2=IDC_DATE,edit,1350639745
Control3=IDC_GROSZY,edit,1350639746
Control4=IDC_SKR_ODS,button,1342242819
Control5=IDOK,button,1342242817
Control6=IDCANCEL,button,1342242816
Control7=IDC_STATIC,static,1342308353
Control8=IDC_NALNO,edit,1350567936
Control9=IDC_STATIC,static,1342308353
Control10=IDC_STATIC,static,1342308353
Control11=IDC_STATIC,static,1342308353

[DLG:IDD_DWPL]
Type=1
Class=CDlgWpl
ControlCount=10
Control1=IDC_DATE,edit,1350639745
Control2=IDC_GROSZY,edit,1350639746
Control3=IDC_TYPW,combobox,1344339971
Control4=IDOK,button,1342242817
Control5=IDCANCEL,button,1342242816
Control6=IDC_STATIC,static,1342308353
Control7=IDC_STATIC,static,1342308353
Control8=IDC_STATIC,static,1342308353
Control9=IDC_WPLNO,edit,1350567936
Control10=IDC_STATIC_SP,static,1342308353

[DLG:IDD_DOKR]
Type=1
Class=CDlgOkr
ControlCount=22
Control1=IDC_DATEP,edit,1350639745
Control2=IDC_DATEK,edit,1350639745
Control3=IDC_STOPA,edit,1350639746
Control4=IDC_RADIOO0,button,1342373897
Control5=IDC_RADIOO1,button,1342177289
Control6=IDC_RADIOW0,button,1342373897
Control7=IDC_RADIOW1,button,1342177289
Control8=IDC_RADIOW2,button,1342177289
Control9=IDOK,button,1342373889
Control10=IDCANCEL,button,1342242816
Control11=IDC_STATIC,static,1342308354
Control12=IDC_OKRNO,edit,1350567936
Control13=IDC_STATIC,static,1342308353
Control14=IDC_STATIC,button,1342177287
Control15=IDC_STATIC,static,1342308353
Control16=IDC_STATIC,static,1342308353
Control17=IDC_STATIC,static,1342308353
Control18=IDC_STATIC,static,1342308352
Control19=IDC_STATIC,static,1342308352
Control20=IDC_STATIC,button,1342177287
Control21=IDC_FACTOR,edit,1350639746
Control22=IDC_STATIC,static,1342308353

[DLG:IDD_GET_STAN]
Type=1
Class=CGetStan
ControlCount=12
Control1=IDC_STATIC,button,1342177287
Control2=IDC_STAN_KAL,button,1342373897
Control3=IDC_STAN_BANK,button,1342177289
Control4=IDC_STATIC,static,1342308352
Control5=IDC_EDIT1,edit,1350631424
Control6=IDC_STATIC,button,1342308359
Control7=IDC_STAN_LIST,listbox,1352728833
Control8=ID_NEW_ELEM,button,1342242816
Control9=ID_EDIT_ELEM,button,1342242816
Control10=ID_DEL_ELEM,button,1342242816
Control11=IDOK,button,1342242817
Control12=IDCANCEL,button,1342242816

[DLG:IDD_DATE_STAN]
Type=1
Class=CGetDate
ControlCount=3
Control1=IDC_DATEP,edit,1350639745
Control2=IDOK,button,1342242817
Control3=IDCANCEL,button,1342242816

[DLG:IDD_POMOC]
Type=1
Class=CPomoc
ControlCount=19
Control1=IDOK,button,1342242817
Control2=IDC_STATIC,static,1342308352
Control3=IDC_STATIC,static,1342308352
Control4=IDC_STATIC,static,1342308352
Control5=IDC_STATIC,static,1342308352
Control6=IDC_STATIC,static,1342308352
Control7=IDC_STATIC,static,1342308352
Control8=IDC_STATIC,static,1342177287
Control9=IDC_STATIC,static,1342177287
Control10=IDC_STATIC,static,1342177287
Control11=IDC_STATIC,static,1342177287
Control12=IDC_STATIC,static,1342177287
Control13=IDC_STATIC,static,1342177287
Control14=IDC_STATIC,static,1342308353
Control15=IDC_STATIC,static,1342308353
Control16=IDC_STATIC,static,1342308353
Control17=IDC_STATIC,static,1342308353
Control18=IDC_STATIC,static,1342308353
Control19=IDC_STATIC,static,1342308353

[DLG:IDD_ABOUTBOX]
Type=1
Class=CAboutDlg
ControlCount=4
Control1=IDC_STATIC,static,1342177283
Control2=IDC_STATIC,static,1342308352
Control3=IDC_STATIC,static,1342308352
Control4=IDOK,button,1342373889

[DLG:IDD_NAG]
Type=1
Class=CNagl
ControlCount=4
Control1=IDOK,button,1342242817
Control2=IDCANCEL,button,1342242816
Control3=IDC_STATIC,static,1342308352
Control4=IDC_EDIT1,edit,1350631552

[DLG:IDD_STOP]
Type=1
Class=CStop
ControlCount=4
Control1=IDOK,button,1342242817
Control2=IDCANCEL,button,1342242816
Control3=IDC_STATIC,static,1342308352
Control4=IDC_EDIT1,edit,1350631552

[DLG:IDD_NEW_OKR]
Type=1
Class=CNewOkr
ControlCount=6
Control1=IDC_DATEP,edit,1350639745
Control2=IDC_STOPA,edit,1350639746
Control3=IDOK,button,1342242817
Control4=IDCANCEL,button,1342242816
Control5=IDC_STATIC,static,1342308353
Control6=IDC_STATIC,static,1342308353

[DLG:IDD_ODS_UST]
Type=1
Class=COdsUst
ControlCount=4
Control1=IDC_ODS_COMBO,listbox,1352728577
Control2=ID_NEW_ELEM,button,1342242816
Control3=IDOK,button,1342242817
Control4=IDCANCEL,button,1342242816

[MNU:IDR_LIST (Polish)]
Type=1
Class=?
Command1=ID_FILE_NEW
Command2=ID_FILE_OPEN
Command3=ID_KONWERT
Command4=ID_FILE_CLOSE
Command5=ID_FILE_SAVE
Command6=ID_FILE_SAVE_AS
Command7=ID_APP_EXIT
Command8=ID_NEW_ELEM
Command9=ID_EDIT_ELEM
Command10=ID_DEL_ELEM
Command11=ID_FILE_PRINT
Command12=ID_FILE_PRINT_PREVIEW
Command13=ID_NAL
Command14=ID_WPL
Command15=ID_OKROD
Command16=ID_NEXT
Command17=ID_PREV
Command18=ID_VIEW_TOOLBAR
Command19=ID_VIEW_STATUS_BAR
Command20=ID_OBL_STANDATE
Command21=ID_ODSETKI
Command22=IDM_DO_WYDR
Command23=IDM_DO_WYDR_R
Command24=IDM_NAG
Command25=IDM_STOP
Command26=ID_WINDOW_ARRANGE
Command27=ID_HELP_INDEX
Command28=ID_HELP_USING
Command29=ID_APP_ABOUT
Command30=IDM_POMOC
CommandCount=30

[MNU:IDR_MAINFRAME (Polish)]
Type=1
Class=?
Command1=ID_FILE_NEW
Command2=ID_FILE_OPEN
Command3=ID_FILE_MRU_FILE1
Command4=ID_APP_EXIT
Command5=ID_VIEW_TOOLBAR
Command6=ID_VIEW_STATUS_BAR
Command7=ID_HELP_INDEX
Command8=ID_HELP_USING
Command9=ID_APP_ABOUT
CommandCount=9

[MNU:IDR_RRRTYPE (Polish)]
Type=1
Class=?
Command1=ID_FILE_NEW
Command2=ID_FILE_OPEN
Command3=ID_FILE_CLOSE
Command4=ID_FILE_SAVE
Command5=ID_FILE_SAVE_AS
Command6=ID_APP_EXIT
Command7=ID_NEW_ELEM
Command8=ID_EDIT_ELEM
Command9=ID_DEL_ELEM
Command10=ID_FILE_PRINT
Command11=ID_FILE_PRINT_PREVIEW
Command12=ID_NAL
Command13=ID_WPL
Command14=ID_OKROD
Command15=ID_NEXT
Command16=ID_PREV
Command17=ID_VIEW_TOOLBAR
Command18=ID_VIEW_STATUS_BAR
Command19=ID_OBL_STANDATE
Command20=ID_ODSETKI
Command21=IDM_DO_WYDR
Command22=IDM_DO_WYDR_R
Command23=IDM_NAG
Command24=IDM_STOP
Command25=ID_WINDOW_ARRANGE
Command26=ID_HELP_INDEX
Command27=ID_HELP_USING
Command28=ID_APP_ABOUT
Command29=IDM_POMOC
CommandCount=29

[ACL:IDR_MAINFRAME (Polish)]
Type=1
Class=?
Command1=ID_FILE_PRINT_PREVIEW
Command2=ID_OBL_STANDATE
Command3=ID_FILE_NEW
Command4=ID_FILE_OPEN
Command5=ID_FILE_PRINT
Command6=ID_FILE_SAVE
Command7=ID_FILE_CLOSE
CommandCount=7

[DLG:IDD_ABOUTBOX (Polish)]
Type=1
Class=?
ControlCount=4
Control1=IDC_STATIC,static,1342177283
Control2=IDC_STATIC,static,1342308352
Control3=IDC_STATIC,static,1342308352
Control4=IDOK,button,1342373889

[DLG:IDD_DWPL (Polish)]
Type=1
Class=CDlgWpl
ControlCount=10
Control1=IDC_DATE,edit,1350639745
Control2=IDC_GROSZY,edit,1350639746
Control3=IDC_TYPW,combobox,1344339971
Control4=IDOK,button,1342242817
Control5=IDCANCEL,button,1342242816
Control6=IDC_STATIC,static,1342308353
Control7=IDC_STATIC,static,1342308353
Control8=IDC_STATIC,static,1342308353
Control9=IDC_WPLNO,edit,1350567936
Control10=IDC_STATIC_SP,static,1342308353

[DLG:IDD_DNAL (Polish)]
Type=1
Class=CDlgNal
ControlCount=11
Control1=IDC_DATE,edit,1350639745
Control2=IDC_GROSZY,edit,1350639746
Control3=IDC_TYPN,combobox,1344339971
Control4=IDC_SKR_ODS,button,1342242819
Control5=IDOK,button,1342242817
Control6=IDCANCEL,button,1342242816
Control7=IDC_STATIC,static,1342308353
Control8=IDC_NALNO,edit,1350567936
Control9=IDC_STATIC,static,1342308353
Control10=IDC_STATIC,static,1342308353
Control11=IDC_STATIC,static,1342308353

[DLG:IDD_GET_STAN (Polish)]
Type=1
Class=?
ControlCount=12
Control1=IDC_STATIC,button,1342177287
Control2=IDC_STAN_KAL,button,1342373897
Control3=IDC_STAN_BANK,button,1342177289
Control4=IDC_STATIC,static,1342308352
Control5=IDC_EDIT1,edit,1350631424
Control6=IDC_STATIC,button,1342308359
Control7=IDC_STAN_LIST,listbox,1352728833
Control8=ID_NEW_ELEM,button,1342242816
Control9=ID_EDIT_ELEM,button,1342242816
Control10=ID_DEL_ELEM,button,1342242816
Control11=IDOK,button,1342242817
Control12=IDCANCEL,button,1342242816

[DLG:IDD_ODS_UST (Polish)]
Type=1
Class=COdsUst
ControlCount=4
Control1=IDC_ODS_COMBO,listbox,1352728577
Control2=ID_NEW_ELEM,button,1342242816
Control3=IDOK,button,1342242817
Control4=IDCANCEL,button,1342242816

[DLG:IDD_NEW_OKR (Polish)]
Type=1
Class=CNewOkr
ControlCount=6
Control1=IDC_DATEP,edit,1350639745
Control2=IDC_STOPA,edit,1350639746
Control3=IDOK,button,1342242817
Control4=IDCANCEL,button,1342242816
Control5=IDC_STATIC,static,1342308353
Control6=IDC_STATIC,static,1342308353

[DLG:IDD_DATE_STAN (Polish)]
Type=1
Class=CGetDate
ControlCount=3
Control1=IDC_DATEP,edit,1350639745
Control2=IDOK,button,1342242817
Control3=IDCANCEL,button,1342242816

[DLG:IDD_DOKR (Polish)]
Type=1
Class=CDlgOkr
ControlCount=20
Control1=IDC_DATEP,edit,1350639745
Control2=IDC_DATEK,edit,1350639745
Control3=IDC_STOPA,edit,1350639746
Control4=IDC_RADIOO0,button,1342373897
Control5=IDC_RADIOO1,button,1342177289
Control6=IDC_RADIOW0,button,1342373897
Control7=IDC_RADIOW1,button,1342177289
Control8=IDC_RADIOW2,button,1342177289
Control9=IDOK,button,1342373889
Control10=IDCANCEL,button,1342242816
Control11=IDC_STATIC,static,1342308354
Control12=IDC_OKRNO,edit,1350567936
Control13=IDC_STATIC,static,1342308353
Control14=IDC_STATIC,button,1342177287
Control15=IDC_STATIC,static,1342308353
Control16=IDC_STATIC,static,1342308353
Control17=IDC_STATIC,static,1342308353
Control18=IDC_STATIC,static,1342308352
Control19=IDC_STATIC,static,1342308352
Control20=IDC_STATIC,button,1342177287

[DLG:IDD_POMOC (Polish)]
Type=1
Class=?
ControlCount=19
Control1=IDOK,button,1342242817
Control2=IDC_STATIC,static,1342308352
Control3=IDC_STATIC,static,1342308352
Control4=IDC_STATIC,static,1342308352
Control5=IDC_STATIC,static,1342308352
Control6=IDC_STATIC,static,1342308352
Control7=IDC_STATIC,static,1342308352
Control8=IDC_STATIC,static,1342177287
Control9=IDC_STATIC,static,1342177287
Control10=IDC_STATIC,static,1342177287
Control11=IDC_STATIC,static,1342177287
Control12=IDC_STATIC,static,1342177287
Control13=IDC_STATIC,static,1342177287
Control14=IDC_STATIC,static,1342308353
Control15=IDC_STATIC,static,1342308353
Control16=IDC_STATIC,static,1342308353
Control17=IDC_STATIC,static,1342308353
Control18=IDC_STATIC,static,1342308353
Control19=IDC_STATIC,static,1342308353

[DLG:IDD_NAG (Polish)]
Type=1
Class=?
ControlCount=4
Control1=IDOK,button,1342242817
Control2=IDCANCEL,button,1342242816
Control3=IDC_STATIC,static,1342308352
Control4=IDC_EDIT1,edit,1350631552

[DLG:IDD_STOP (Polish)]
Type=1
Class=?
ControlCount=4
Control1=IDOK,button,1342242817
Control2=IDCANCEL,button,1342242816
Control3=IDC_STATIC,static,1342308352
Control4=IDC_EDIT1,edit,1350631552

[TB:IDR_TBIN (Polish)]
Type=1
Class=?
Command1=ID_FILE_CLOSE
Command2=ID_FILE_SAVE
Command3=ID_NEW_ELEM
Command4=ID_EDIT_ELEM
Command5=ID_DEL_ELEM
Command6=ID_PREV
Command7=ID_NEXT
Command8=ID_OBL_STANDATE
Command9=IDM_POMOC
Command10=ID_HELP_USING
CommandCount=10

[TB:IDR_TBKALK (Polish)]
Type=1
Class=?
Command1=ID_FILE_CLOSE
Command2=ID_FILE_SAVE
Command3=ID_FILE_PRINT_PREVIEW
Command4=ID_FILE_PRINT
Command5=ID_OBL_STANDATE
Command6=IDM_KALK
Command7=IDM_POMOC
Command8=ID_HELP_USING
CommandCount=8

[TB:IDR_TBOUT (Polish)]
Type=1
Class=?
Command1=ID_FILE_NEW
Command2=ID_FILE_OPEN
Command3=IDM_POMOC
Command4=ID_HELP_USING
CommandCount=4

[MNU:IDR_MAINFRAME]
Type=1
Class=?
Command1=ID_FILE_NEW
Command2=ID_FILE_OPEN
Command3=ID_FILE_MRU_FILE1
Command4=ID_APP_EXIT
Command5=ID_VIEW_TOOLBAR
Command6=ID_VIEW_STATUS_BAR
Command7=ID_HELP_INDEX
Command8=ID_HELP_USING
Command9=ID_APP_ABOUT
CommandCount=9

[MNU:IDR_RRRTYPE]
Type=1
Class=?
Command1=ID_FILE_NEW
Command2=ID_FILE_OPEN
Command3=ID_FILE_CLOSE
Command4=ID_FILE_SAVE
Command5=ID_FILE_SAVE_AS
Command6=ID_APP_EXIT
Command7=ID_NEW_ELEM
Command8=ID_EDIT_ELEM
Command9=ID_DEL_ELEM
Command10=ID_FILE_PRINT
Command11=ID_FILE_PRINT_PREVIEW
Command12=ID_NAL
Command13=ID_WPL
Command14=ID_OKROD
Command15=ID_NEXT
Command16=ID_PREV
Command17=ID_VIEW_TOOLBAR
Command18=ID_VIEW_STATUS_BAR
Command19=ID_OBL_STANDATE
Command20=ID_ODSETKI
Command21=IDM_DO_WYDR
Command22=IDM_DO_WYDR_R
Command23=IDM_NAG
Command24=IDM_STOP
Command25=ID_WINDOW_ARRANGE
Command26=ID_HELP_INDEX
Command27=ID_HELP_USING
Command28=ID_APP_ABOUT
Command29=IDM_POMOC
CommandCount=29

[MNU:IDR_LIST]
Type=1
Class=?
Command1=ID_FILE_NEW
Command2=ID_FILE_OPEN
Command3=ID_KONWERT
Command4=ID_FILE_CLOSE
Command5=ID_FILE_SAVE
Command6=ID_FILE_SAVE_AS
Command7=ID_APP_EXIT
Command8=ID_NEW_ELEM
Command9=ID_EDIT_ELEM
Command10=ID_DEL_ELEM
Command11=ID_FILE_PRINT
Command12=ID_FILE_PRINT_PREVIEW
Command13=ID_NAL
Command14=ID_WPL
Command15=ID_OKROD
Command16=ID_NEXT
Command17=ID_PREV
Command18=ID_VIEW_TOOLBAR
Command19=ID_VIEW_STATUS_BAR
Command20=ID_OBL_STANDATE
Command21=ID_ODSETKI
Command22=IDM_DO_WYDR
Command23=IDM_DO_WYDR_R
Command24=IDM_NAG
Command25=IDM_STOP
Command26=ID_WINDOW_ARRANGE
Command27=ID_HELP_INDEX
Command28=ID_HELP_USING
Command29=ID_APP_ABOUT
Command30=IDM_POMOC
CommandCount=30

[ACL:IDR_MAINFRAME]
Type=1
Class=?
Command1=ID_FILE_PRINT_PREVIEW
Command2=ID_OBL_STANDATE
Command3=ID_FILE_NEW
Command4=ID_FILE_OPEN
Command5=ID_FILE_PRINT
Command6=ID_FILE_SAVE
Command7=ID_FILE_CLOSE
CommandCount=7

[TB:IDR_TBIN]
Type=1
Class=?
Command1=ID_FILE_CLOSE
Command2=ID_FILE_SAVE
Command3=ID_NEW_ELEM
Command4=ID_EDIT_ELEM
Command5=ID_DEL_ELEM
Command6=ID_PREV
Command7=ID_NEXT
Command8=ID_OBL_STANDATE
Command9=IDM_POMOC
Command10=ID_HELP_USING
CommandCount=10

[TB:IDR_TBKALK]
Type=1
Class=?
Command1=ID_FILE_CLOSE
Command2=ID_FILE_SAVE
Command3=ID_FILE_PRINT_PREVIEW
Command4=ID_FILE_PRINT
Command5=ID_OBL_STANDATE
Command6=IDM_KALK
Command7=IDM_POMOC
Command8=ID_HELP_USING
CommandCount=8

[TB:IDR_TBOUT]
Type=1
Class=?
Command1=ID_FILE_NEW
Command2=ID_FILE_OPEN
Command3=IDM_POMOC
Command4=ID_HELP_USING
CommandCount=4

[DLG:IDD_DIALOG1]
Type=1
Class=?
ControlCount=2
Control1=IDOK,button,1342242817
Control2=IDCANCEL,button,1342242816


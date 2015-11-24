; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "SCCO Accounting System"
#define MyAppVersion "2.1.2.5"
#define MyAppPublisher "Sta. Cruz Savings and Credit Cooperative"
#define MyAppURL "https://www.facebook.com/pages/Sta-Cruz-Savings-Credit-Cooperative/141420875904329/"
#define MyAppExeName "SCCO.WPF.MVC.CS.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{68AB7485-69CD-45E3-BEF9-6AE9481F0F88}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
OutputBaseFilename=SCCO Accounting System 2.1.2.6 Setup
SetupIconFile=F:\github\scco\SCCO.WPF.MVC.CSHARP\chart.ico
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 0,6.1

[Files]
; Accounting System
Source: "F:\github\scco\SCCO.WPF.MVC.CSHARP\bin\Debug\SCCO.WPF.MVC.CS.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "F:\github\scco\SCCO.WPF.MVC.CSHARP\bin\Debug\SCCO.WPF.MVC.CS.exe.config"; DestDir: "{app}"; Flags: ignoreversion

; Crystal Reports
Source: "F:\github\scco\SCCO.WPF.MVC.CSHARP\bin\Debug\CrystalDecisions.CrystalReports.Engine.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "F:\github\scco\SCCO.WPF.MVC.CSHARP\bin\Debug\CrystalDecisions.Shared.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "F:\github\scco\SCCO.WPF.MVC.CSHARP\bin\Debug\SAPBusinessObjects.WPF.Viewer.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "F:\github\scco\SCCO.WPF.MVC.CSHARP\bin\Debug\ReportFiles\*.*"; DestDir: "{app}\ReportFiles"; Flags:replacesameversion

; MySQL
Source: "F:\github\scco\SCCO.WPF.MVC.CSHARP\bin\Debug\mysql.data.dll"; DestDir: "{app}"; Flags: ignoreversion

; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent


#define MyAppName "Interweb Searcher"
#define MyAppVersion "1.1.0"
#define MyAppPublisher "AGG Productions"
#define MyAppExeName "Interweb Searcher.exe"

[Setup]
AppId={{Interweb-Searcher}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName=C:\Users\Public\{#MyAppName}
DisableDirPage=yes
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
PrivilegesRequired=lowest
OutputDir=./
OutputBaseFilename=Interweb.Searcher.Installer
SetupIconFile=.\Images\Icon.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: checkedonce

[Files]
Source: ".\bin\Release\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\bin\Release\Interweb Searcher.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\bin\Release\Interweb Searcher.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\bin\Release\Interweb Searcher.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\bin\Release\Wpf.Ui.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\bin\Release\Wpf.Ui.pdb"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

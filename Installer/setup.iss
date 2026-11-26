; Water Refilling Station Management System - Inno Setup script
; 1) Build the solution in Release mode.  2) Open this file in Inno Setup and press Ctrl+F9.
; Put this file in the Installer folder at the root of the App repo.

#define MyAppName "Water Refilling Station"
#define MyAppVersion "1.0.0"
#define MyAppExeName "WaterRefillingStation.exe"
#define BuildDir "..\WaterRefillingStation\WaterRefillingStation\bin\Release"

[Setup]
AppId={{08264E46-215B-4789-A58A-40FB47EBA4F6}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher=Water Refilling Station Team
DefaultDirName={autopf}\{#MyAppName}
DisableProgramGroupPage=yes
OutputDir=Output
OutputBaseFilename=WaterRefillingStation-Setup-{#MyAppVersion}
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
; Installs for the current user without an admin prompt. The wizard offers "all users" as an option.
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
UninstallDisplayIcon={app}\{#MyAppExeName}

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "Create a desktop shortcut"; GroupDescription: "Additional shortcuts:"

[Files]
Source: "{#BuildDir}\*"; DestDir: "{app}"; Excludes: "*.pdb,*.xml,*.vshost.*"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
; Creates the database (%LocalAppData%\WaterRefillingStation\station.db) during setup, with no manual steps.
Filename: "{app}\{#MyAppExeName}"; Parameters: "/initdb"; StatusMsg: "Setting up the database..."; Flags: runhidden runasoriginaluser waituntilterminated
Filename: "{app}\{#MyAppExeName}"; Description: "Launch {#MyAppName}"; Flags: nowait postinstall skipifsilent

; The database is NOT deleted on uninstall, so the station's records are kept.

[Code]
function IsDotNet48Installed: Boolean;
var
  Release: Cardinal;
begin
  Result := RegQueryDWordValue(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full', 'Release', Release)
            and (Release >= 528040);
end;

function InitializeSetup: Boolean;
begin
  Result := True;
  if not IsDotNet48Installed then
  begin
    MsgBox('{#MyAppName} needs Microsoft .NET Framework 4.8. Please install it from microsoft.com, then run this setup again.', mbCriticalError, MB_OK);
    Result := False;
  end;
end;

[Setup]
AppName=Machina
AppVersion=1.0
DefaultDirName={autopf}\Machina
DefaultGroupName=Machina
OutputBaseFilename=Machina_Setup
Compression=lzma
SolidCompression=yes

[Files]
; 빌드 결과물이 있는 폴더의 모든 파일을 포함
Source: "Builds\Windows\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\Machina"; Filename: "{app}\Machina.exe"
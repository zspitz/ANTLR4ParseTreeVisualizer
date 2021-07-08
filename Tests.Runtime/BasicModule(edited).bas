Attribute VB_Name = "BasicModule"
'***************************************************************************
'*                          Woobind Network Meter                          *
'***************************************************************************
'*   Copyright (C) 2007 by Roman Gemini                                    *
'*   networkmeter@ukr.net                                                  *
'*                                                                         *
'*   This program is free software; you can redistribute it and/or modify  *
'*   it under the terms of the GNU General Public License as published by  *
'*   the Free Software Foundation; either version 2 of the License, or     *
'*   (at your option) any later version.                                   *
'*                                                                         *
'*   This program is distributed in the hope that it will be useful,       *
'*   but WITHOUT ANY WARRANTY; without even the implied warranty of        *
'*   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the         *
'*   GNU General Public License for more details.                          *
'*                                                                         *
'*   You should have received a copy of the GNU General Public License     *
'*   along with this program; if not, write to the                         *
'*   Free Software Foundation, Inc.,                                       *
'*   59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.             *
'***************************************************************************

Public Declare Function CreateEllipticRgn Lib "gdi32" (ByVal X1 As Long, ByVal Y1 As Long, ByVal X2 As Long, ByVal Y2 As Long) As Long
Public Declare Function CreateRoundRectRgn Lib "gdi32" (ByVal X1 As Long, ByVal Y1 As Long, ByVal X2 As Long, ByVal Y2 As Long, ByVal X3 As Long, ByVal Y3 As Long) As Long
Public Declare Function StretchBlt Lib "gdi32" (ByVal hdc As Long, ByVal x As Long, ByVal y As Long, ByVal nWidth As Long, ByVal nHeight As Long, ByVal hSrcDC As Long, ByVal xSrc As Long, ByVal ySrc As Long, ByVal nSrcWidth As Long, ByVal nSrcHeight As Long, ByVal dwRop As Long) As Long

Public Const WM_CLOSE = &H10
Public Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Long
Public Declare Sub InitCommonControls Lib "comctl32.dll" ()
Public Declare Sub InitCommonControlsX Lib "mscomctl.dll" ()
Public Declare Function GetWindowText Lib "user32" Alias "GetWindowTextA" (ByVal hWnd As Long, ByVal lpString As String, ByVal cch As Long) As Long
Public Declare Function FindWindowEx Lib "user32" Alias "FindWindowExA" (ByVal hWnd1 As Long, ByVal hWnd2 As Long, ByVal lpsz1 As String, ByVal lpsz2 As String) As Long
Public Declare Function GetParent Lib "user32" (ByVal hWnd As Long) As Long
Public Declare Function CreateWindowEx Lib "user32" Alias "CreateWindowExA" (ByVal dwExStyle As Long, ByVal lpClassName As String, ByVal lpWindowName As String, ByVal dwStyle As Long, ByVal x As Long, ByVal y As Long, ByVal nWidth As Long, ByVal nHeight As Long, ByVal hWndParent As Long, ByVal hMenu As Long, ByVal hInstance As Long, lpParam As Any) As Long
Public Declare Function Putfocus Lib "user32" Alias "SetFocus" (ByVal hWnd As Long) As Long
Public Declare Function GetForegroundWindow Lib "user32" () As Long
Public Declare Function CreateRectRgn Lib "gdi32" (ByVal X1 As Long, ByVal Y1 As Long, ByVal X2 As Long, ByVal Y2 As Long) As Long

Private Const SND_NOSTOP = &H10
Public Const SPI_GETWORKAREA = 48
Public Const SPI_SETWORKAREA = 47


Public Const HWND_TOPMOST = -1
Public Const HWND_NOTOPMOST = -2
Public Const SWP_NOACTIVATE = &H10
Public Const SWP_SHOWWINDOW = &H40

Public Declare Function ExtractAssociatedIcon Lib "shell32.dll" Alias "ExtractAssociatedIconA" (ByVal hInst As Long, ByVal lpIconPath As String, lpiIcon As Long) As Long
Public Declare Function CallWindowProc Lib "user32" Alias "CallWindowProcA" (ByVal lpPrevWndFunc As Long, ByVal hWnd As Long, ByVal Msg As Long, ByVal wParam As Long, ByVal lParam As Long) As Long
Public Declare Function PostMessage Lib "user32" Alias "PostMessageA" (ByVal hWnd As Long, ByVal wMsg As Long, ByVal wParam As Long, ByVal lParam As Long) As Long
Public Declare Function VarPtrArray Lib "msvbvm60.dll" Alias "VarPtr" (Ptr() As Any) As Long
Public Declare Function PeekMessage Lib "user32" Alias "PeekMessageA" (lpMsg As Msg, ByVal hWnd As Long, ByVal wMsgFilterMin As Long, ByVal wMsgFilterMax As Long, ByVal wRemoveMsg As Long) As Long
Public Declare Function lstrcpyn1 Lib "kernel32" Alias "lstrcpynA" (lpString1 As Byte, ByVal lpString2 As String, ByVal cNumber As Long) As Long
Public Declare Function lstrcpyn2 Lib "kernel32" Alias "lstrcpynA" (ByVal lpString1 As String, lpString2 As Byte, ByVal cNumber As Long) As Long
Public Declare Function SystemTimer Lib "winmm.dll" Alias "timeGetTime" () As Long
Public Declare Function GetTempPath Lib "kernel32" Alias "GetTempPathA" (ByVal nBufferLength As Long, ByVal lpBuffer As String) As Long
Public Declare Function SetWindowPos Lib "user32" (ByVal hWnd As Long, ByVal hWndInsertAfter As Long, ByVal x As Long, ByVal y As Long, ByVal cx As Long, ByVal cy As Long, ByVal wFlags As Long) As Long
Public Declare Function ShellExecute Lib "shell32.dll" Alias "ShellExecuteA" (ByVal hWnd As Long, ByVal lpOperation As String, ByVal lpFile As String, ByVal lpParameters As String, ByVal lpDirectory As String, ByVal nShowCmd As Long) As Long
Public Declare Function GetWindowsVersion Lib "kernel32" Alias "GetVersion" () As Long
Public Declare Function RegisterServiceProcess Lib "kernel32.dll" (ByVal dwProcessId As Long, ByVal dwType As Long) As Long
Public Declare Function GetCurrentProcessId Lib "kernel32.dll" () As Long
Public Declare Function SetWindowLong Lib "user32" Alias "SetWindowLongA" (ByVal hWnd As Long, ByVal nIndex As Long, ByVal dwNewLong As Long) As Long
Public Declare Function GetWindowLong Lib "user32" Alias "GetWindowLongA" (ByVal hWnd As Long, ByVal nIndex As Long) As Long
Public Declare Function RegQueryValueEx Lib "advapi32" Alias "RegQueryValueExA" (ByVal hKey As Long, ByVal lpValueName As String, lpReserved As Long, lpType As Long, lpData As Any, lpcbData As Long) As Long
Public Declare Function RegOpenKeyEx Lib "advapi32" Alias "RegOpenKeyExA" (ByVal hKey&, ByVal lpSubKey$, ByVal dwReserved&, ByVal samDesired As Long, phkResult As Long) As Long
Public Declare Function RegCreateKey Lib "advapi32.dll" Alias "RegCreateKeyA" (ByVal hKey As Long, ByVal lpSubKey As String, phkResult As Long) As Long
Public Declare Function RegSetValueEx Lib "advapi32.dll" Alias "RegSetValueExA" (ByVal hKey As Long, ByVal lpValueName As String, ByVal Reserved As Long, ByVal dwType As Long, lpData As Any, ByVal cbData As Long) As Long
Public Declare Function RegCloseKey Lib "advapi32.dll" (ByVal hKey As Long) As Long
Public Declare Function ExtFloodFill Lib "gdi32" (ByVal hdc As Long, ByVal x As Long, ByVal y As Long, ByVal crColor As Long, ByVal wFillType As Long) As Long
Public Declare Function Beep Lib "kernel32" (ByVal dwFreq As Long, ByVal dwDuration As Long) As Long
Public Declare Sub Sleep Lib "kernel32" (ByVal milliseconds As Long)
Public Declare Function SetParent Lib "user32" (ByVal hWndChild As Long, ByVal hWndNewParent As Long) As Long
Public Declare Function GetDesktopWindow Lib "user32" () As Long
Public Declare Function GetSystemDirectory Lib "kernel32" Alias "GetSystemDirectoryA" (ByVal lpBuffer As String, ByVal nSize As Long) As Long
Public Declare Function sndPlaySound Lib "winmm.dll" Alias "sndPlaySoundA" (ByVal lpszSoundName As String, ByVal uFlags As Long) As Long
Public Declare Function sndStopSound Lib "winmm.dll" Alias "sndPlaySoundA" (ByVal lpszNull As Long, ByVal uFlags As Long) As Long
Public Declare Function GetDiskFreeSpaceEx Lib "kernel32" Alias "GetDiskFreeSpaceExA" (ByVal lpDirectoryName As String, lpFreeBytesAvailableToCaller As Currency, lpTotalNumberOfBytes As Currency, lpTotalNumberOfFreeBytes As Currency) As Long
Public Declare Function SetWindowRgn Lib "user32" (ByVal hWnd As Long, ByVal hRgn As Long, ByVal bRedraw As Boolean) As Long
Public Declare Function CreatePolygonRgn Lib "gdi32" (lpPoint As POINTAPI, ByVal nCount As Long, ByVal nPolyFillMode As Long) As Long
Public Declare Function GetFileAttributes Lib "kernel32" Alias "GetFileAttributesA" (ByVal lpFileName As String) As Long
Public Declare Function GetShortPathName Lib "kernel32" Alias "GetShortPathNameA" (ByVal lpszLongPath As String, ByVal lpszShortPath As String, ByVal cchBuffer As Long) As Long
Public Declare Function GetPixel Lib "gdi32" (ByVal hdc As Long, ByVal x As Long, ByVal y As Long) As Long
Public Declare Function SetPixelV Lib "gdi32" (ByVal hdc As Long, ByVal x As Long, ByVal y As Long, ByVal crColor As Long) As Long
Public Declare Function PtInRegion Lib "gdi32" (ByVal hRgn As Long, ByVal x As Long, ByVal y As Long) As Long
Public Declare Function GetWindowRgn Lib "user32" (ByVal hWnd As Long, ByVal hRgn As Long) As Long
Public Declare Function LineTo Lib "gdi32" (ByVal hdc As Long, ByVal x As Long, ByVal y As Long) As Long
Public Declare Function MoveToEx Lib "gdi32" (ByVal hdc As Long, ByVal x As Long, ByVal y As Long, lpPoint As POINTAPI) As Long
Public Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef Destination As Any, ByRef Source As Any, ByVal Length As Long)
Public Declare Function BitBlt Lib "gdi32" (ByVal hDestDC As Long, ByVal x As Long, ByVal y As Long, ByVal nWidth As Long, ByVal nHeight As Long, ByVal hSrcDC As Long, ByVal xSrc As Long, ByVal ySrc As Long, ByVal dwRop As Long) As Long

Public Declare Function TransparentBlt Lib "gdi32" Alias "GdiTransparentBlt" (ByVal hDestDC As Long, ByVal x As Long, ByVal y As Long, ByVal nWidthDest As Long, ByVal nHeightDest As Long, ByVal hSrcDC As Long, ByVal xSrc As Long, ByVal ySrc As Long, ByVal nWidthSrc As Long, ByVal nHeightSrc As Long, ByVal crTransparent As Long) As Long
Public Declare Function AlphaBlend Lib "gdi32" Alias "GdiAlphaBlend" (ByVal hDestDC As Long, ByVal x As Long, ByVal y As Long, ByVal nWidthDest As Long, ByVal nHeightDest As Long, ByVal hSrcDC As Long, ByVal xSrc As Long, ByVal ySrc As Long, ByVal nWidthSrc As Long, ByVal nHeightSrc As Long, pblendFunction As blendFunction) As Long
Public Declare Function AlphaBlendA Lib "gdi32" Alias "GdiAlphaBlend" (ByVal hDestDC As Long, ByVal x As Long, ByVal y As Long, ByVal nWidthDest As Long, ByVal nHeightDest As Long, ByVal hSrcDC As Long, ByVal xSrc As Long, ByVal ySrc As Long, ByVal nWidthSrc As Long, ByVal nHeightSrc As Long, pblendFunction As Any) As Long



Public Declare Function GetKeyState Lib "user32" (ByVal nVirtKey As Long) As Integer
Public Declare Function SHGetFileInfo Lib "shell32" Alias "SHGetFileInfoA" (ByVal pszPath As Any, ByVal dwFileAttributes As Long, psfi As SHFILEINFO, ByVal cbFileInfo As Long, ByVal uFlags As Long) As Long
Public Declare Function DrawIconEx Lib "user32" (ByVal hdc As Long, ByVal xLeft As Long, ByVal yTop As Long, ByVal hIcon As Long, ByVal cxWidth As Long, ByVal cyWidth As Long, ByVal istepIfAniCur As Long, ByVal hbrFlickerFreeDraw As Long, ByVal diFlags As Long) As Boolean
Public Declare Function DestroyIcon Lib "user32" (ByVal hIcon As Long) As Long
Public Declare Function ImageList_GetIconSize Lib "comctl32" (ByVal himl As Long, cx As Long, cy As Long) As Boolean
Public Declare Function OpenProcess Lib "kernel32" (ByVal dwDesiredAccess As Long, ByVal bInheritHandle As Long, ByVal dwProcessId As Long) As Long
Public Declare Function TerminateProcess Lib "kernel32" (ByVal hProcess As Long, ByVal uExitCode As Long) As Long
Public Declare Function GetWindowThreadProcessId Lib "user32" (ByVal hWnd As Long, lpdwProcessId As Long) As Long
Public Declare Function CreateSolidBrush Lib "gdi32" (ByVal crColor As Long) As Long
Public Declare Function FillRect Lib "user32" (ByVal hdc As Long, lpRect As RECT, ByVal hBrush As Long) As Long
Public Declare Function GetWindowRect Lib "user32" (ByVal hWnd As Long, lpRect As RECT) As Long
Public Declare Function ClientToScreen Lib "user32" (ByVal hWnd As Long, lpPoint As POINTAPI) As Long
Public Declare Sub FillMemory Lib "kernel32" Alias "RtlFillMemory" (Destination As Any, ByVal Length As Long, ByVal FillData As Byte)
Public Declare Function OleCreatePictureIndirect Lib "olepro32.dll" (lpPictDesc As PictDesc, riid As Guid, ByVal fOwn As Long, iPic As IPicture) As Long
Public Declare Function CopyIcon Lib "user32" (ByVal hIcon As Long) As Long
'Public Declare Function DrawIconEx Lib "user32" (ByVal hdc As Long, ByVal xLeft As Long, ByVal yTop As Long, ByVal hIcon As Long, ByVal cxWidth As Long, ByVal cyHeight As Long, ByVal istepIfAniCur As Long, ByVal hbrFlickerFreeDraw As Long, ByVal diFlags As Long) As Long
Public Declare Function DrawIcon Lib "user32.dll" (ByVal hdc As Long, ByVal x As Long, ByVal y As Long, ByVal hIcon As Long)
Public Declare Function timeGetTime Lib "winmm.dll" () As Long
Public Declare Function SetPriorityClass Lib "kernel32" (ByVal hProcess As Long, ByVal dwPriorityClass As Long) As Long
Public Declare Function SetThreadPriority Lib "kernel32" (ByVal hThread As Long, ByVal nPriority As Long) As Long
Public Declare Function GetFullPathName Lib "kernel32" Alias "GetFullPathNameA" (ByVal lpFileName As String, ByVal nBufferLength As Long, ByVal lpBuffer As String, ByVal lpFilePart As String) As Long
Public Declare Function GetCursorPos Lib "user32" (cPoint As POINTAPI) As Long
Public Declare Function WindowFromPoint Lib "user32" (ByVal xPoint As Long, ByVal yPoint As Long) As Long
Public Declare Function Shell_NotifyIcon Lib "shell32.dll" Alias "Shell_NotifyIconA" (ByVal dwMessage As Long, lpData As NOTIFYICONDATA) As Long
Public Declare Function GetObjectAPI Lib "gdi32" Alias "GetObjectA" (ByVal hObject As Long, ByVal nCount As Long, lpObject As Any) As Long
Public Declare Function GetObjectA Lib "gdi32" (ByVal hObject As Long, ByVal nCount As Long, lpObject As Any) As Long
Public Declare Function GetUserName Lib "advapi32.dll" Alias "GetUserNameA" (ByVal lpBuffer As String, nSize As Long) As Long
Public Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hWnd As Long, ByVal wMsg As Long, ByVal wParam As Long, lParam As Any) As Long
Public Declare Function DefWindowProc Lib "user32" Alias "DefWindowProcA" (ByVal hWnd As Long, ByVal wMsg As Long, ByVal wParam As Long, ByVal lParam As Long) As Long
Public Declare Function GetClientRect Lib "user32" (ByVal hWnd As Long, lpRect As RECT) As Long
Public Declare Function GetIconInfo Lib "user32" (ByVal hIcon As Long, piconinfo As ICONINFO) As Long
Public Declare Function CreateBitmap Lib "gdi32" (ByVal nWidth As Long, ByVal nHeight As Long, ByVal nPlanes As Long, ByVal nBitCount As Long, lpBits As Any) As Long
Public Declare Function CopyImage Lib "user32" (ByVal handle As Long, ByVal uType As Long, ByVal cxDesired As Long, ByVal cyDesired As Long, ByVal fuFlags As Long) As Long
Public Declare Function CreateCompatibleDC Lib "gdi32" (ByVal hdc As Long) As Long
Public Declare Function SelectObject Lib "gdi32" (ByVal hdc As Long, ByVal hObject As Long) As Long
Public Declare Function SetBkColor Lib "gdi32" (ByVal hdc As Long, ByVal crColor As Long) As Long
Public Declare Function SetTextColor Lib "gdi32" (ByVal hdc As Long, ByVal crColor As Long) As Long
Public Declare Function GetBkColor Lib "gdi32" (ByVal hdc As Long) As Long
Public Declare Function GetTextColor Lib "gdi32" (ByVal hdc As Long) As Long
Public Declare Function DeleteDC Lib "gdi32" (ByVal hdc As Long) As Long
Public Declare Function DeleteObject Lib "gdi32" (ByVal hObject As Long) As Long
Public Declare Function OleTranslateColor Lib "oleaut32.dll" (ByVal lOleColor As Long, ByVal lHPalette As Long, lColorRef As Long) As Long
Public Declare Function CreateHalftonePalette Lib "gdi32" (ByVal hdc As Long) As Long
Public Declare Function GetDC Lib "user32" (ByVal hWnd As Long) As Long
Public Declare Function ReleaseDC Lib "user32" (ByVal hWnd As Long, ByVal hdc As Long) As Long
Public Declare Function CreateDIBSection Lib "gdi32" (ByVal hdc As Long, pBitmapInfo As BITMAPINFO, ByVal un As Long, lplpVoid As Long, ByVal handle As Long, ByVal dw As Long) As Long
Public Declare Function DispatchMessage Lib "user32" Alias "DispatchMessageA" (lpMsg As Msg) As Long
Public Declare Function TranslateMessage Lib "user32" (lpMsg As Msg) As Long
Public Declare Function GetMessage Lib "user32" Alias "GetMessageA" (lpMsg As Msg, ByVal hWnd As Long, ByVal wMsgFilterMin As Long, ByVal wMsgFilterMax As Long) As Long
Public Declare Function DefineDosDevice Lib "kernel32" Alias "DefineDosDeviceA" (ByVal dwFlags As Long, ByVal lpDeviceName As String, ByVal lpTargetPath As String) As Long
Public Declare Function SetVolumeLabel Lib "kernel32" Alias "SetVolumeLabelA" (ByVal lpRootPathName As String, ByVal lpVolumeName As String) As Long
Public Declare Function TextOut Lib "gdi32" Alias "TextOutA" (ByVal hdc As Long, ByVal x As Long, ByVal y As Long, ByVal lpString As String, ByVal nCount As Long) As Long
Public Declare Function CreateFile Lib "kernel32" Alias "CreateFileA" (ByVal lpFileName As String, ByVal dwDesiredAccess As Long, ByVal dwShareMode As Long, ByVal ZERO As Long, ByVal dwCreationDisposition As Long, ByVal dwFlagsAndAttributes As Long, ByVal hTemplateFile As Long) As Long
Public Declare Function WriteFile Lib "kernel32" (ByVal hFile As Long, lpBuffer As Any, ByVal nNumberOfBytesToWrite As Long, lpNumberOfBytesWritten As Long, ByVal ZERO As Long) As Long
Public Declare Function ReadFile Lib "kernel32" (ByVal hFile As Long, lpBuffer As Any, ByVal nNumberOfBytesToRead As Long, lpNumberOfBytesRead As Long, ByVal ZERO As Long) As Long
Public Declare Function CloseHandle Lib "kernel32" (ByVal hObject As Long) As Long
Public Declare Function mciSendString Lib "winmm.dll" Alias "mciSendStringA" (ByVal lpstrCommand As String, ByVal lpstrReturnString As String, ByVal uReturnLength As Long, ByVal hwndCallback As Long) As Long
Public Declare Function AlphaBlending Lib "msimg32.dll" Alias "AlphaBlend" (ByVal hDCDest As Long, ByVal nXOriginDest As Long, ByVal nYOriginDest As Long, ByVal nWidthDest As Long, ByVal nHeightDest As Long, ByVal hdcSrc As Long, ByVal nXOriginSrc As Long, ByVal nYOriginSrc As Long, ByVal nWidthSrc As Long, ByVal nHeightSrc As Long, ByVal BF As Long) As Long
Public Declare Function ShowWindow Lib "user32" (ByVal hWnd As Long, ByVal nCmdShow As Long) As Long
Public Declare Function GetWindowDC Lib "user32" (ByVal hWnd As Long) As Long
Public Declare Function CreateCompatibleBitmap Lib "gdi32" (ByVal hdc As Long, ByVal nWidth As Long, ByVal nHeight As Long) As Long
Public Declare Function GetDeviceCaps Lib "gdi32" (ByVal hdc As Long, ByVal nIndex As Long) As Long
Public Declare Function GetWindowsDirectory Lib "kernel32" Alias "GetWindowsDirectoryA" (ByVal lpBuffer As String, ByVal nSize As Long) As Long
Public Declare Function GetSysColor Lib "user32" (ByVal nIndex As Long) As Long
Public Declare Function SystemParametersInfo Lib "user32" Alias "SystemParametersInfoA" (ByVal uAction As Long, ByVal uParam As Long, ByRef lpvParam As Any, ByVal fuWinIni As Long) As Long
Public Declare Function GetWindow Lib "user32" (ByVal hWnd As Long, ByVal wCmd As Long) As Long
Public Declare Function EnableWindow Lib "user32" (ByVal hWnd As Long, ByVal fEnable As Long) As Long
Public Declare Function GetEnvironmentVariable Lib "kernel32" Alias "GetEnvironmentVariableA" (ByVal lpName As String, ByVal lpBuffer As String, ByVal nSize As Long) As Long
Public Declare Function GetClassName Lib "user32" Alias "GetClassNameA" (ByVal hWnd As Long, ByVal lpClassName As String, ByVal nMaxCount As Long) As Long
Public Declare Function IsWindowVisible Lib "user32" (ByVal hWnd As Long) As Long
Public Declare Function GetDriveType Lib "kernel32" Alias "GetDriveTypeA" (ByVal nDrive As String) As Long
Public Declare Function FindWindowS Lib "user32" Alias "FindWindowA" (ByVal lpClassName As Long, ByVal lpWindowName As String) As Long
Public Declare Function GetVersionEx Lib "kernel32" Alias "GetVersionExA" (lpVersionInformation As OSVERSIONINFO) As Long
Public Declare Function ExtractIcon Lib "shell32.dll" Alias "ExtractIconA" (ByVal hInst As Long, ByVal lpszExeFileName As String, ByVal nIconIndex As Long) As Long

Public Declare Function SetLayeredWindowAttributes Lib "user32.dll" (ByVal handle As Long, ByVal param1 As Long, ByVal param2 As Byte, ByVal Param3 As Long) As Long
Public Declare Function RedrawWindow Lib "user32" (ByVal hWnd As Long, ByVal ZERO As Long, ByVal hrgnUpdate As Long, ByVal fuRedraw As Long) As Long

Public Declare Function GetDIBits Lib "gdi32" (ByVal ahDc As Long, ByVal hBitmap As Long, ByVal nStartScan As Long, ByVal nNumScans As Long, lpBits As Any, lpBI As BITMAPINFO, ByVal wUsage As Long) As Long
Public Declare Function SetDIBits Lib "gdi32" (ByVal hdc As Long, ByVal hBitmap As Long, ByVal nStartScan As Long, ByVal nNumScans As Long, lpBits As Any, lpBI As BITMAPINFO, ByVal wUsage As Long) As Long
Public Declare Function GetPixelRGB Lib "gdi32" Alias "GetPixel" (ByVal hdc As Long, ByVal x As Long, ByVal y As Long) As pRGB
Public Declare Function SetPixelRGB Lib "gdi32" Alias "SetPixelV" (ByVal hdc As Long, ByVal x As Long, ByVal y As Long, ByVal crColor As pRGB) As Long
Public Declare Function BitBlt32to24C Lib "Grender.DLL" Alias "_Render_BitBlt32to24@32" (ByRef vbDestGSprite As Sprite24, ByVal x As Long, ByVal y As Long, ByVal W As Long, ByVal h As Long, ByRef vbSource As Sprite32, ByVal SrcX As Long, ByVal SrcY As Long) As Long
Public Declare Function BitBlt24to24C Lib "Grender.DLL" Alias "_Render_BitBlt24to24@32" (ByRef vbDestGSprite As Sprite24, ByVal x As Long, ByVal y As Long, ByVal W As Long, ByVal h As Long, ByRef vbSource As Sprite24, ByVal SrcX As Long, ByVal SrcY As Long) As Long
Public Declare Function InvertAlpha32C Lib "Grender.DLL" Alias "_Render_InvertAlpha32@20" (ByRef vbDestGSprite As Sprite32, ByVal x As Long, ByVal y As Long, ByVal W As Long, ByVal h As Long) As Long
Public Declare Function AlphaAdd32C Lib "Grender.DLL" Alias "_Render_AlphaAdd32@24" (ByRef vbDestGSprite As Sprite32, ByVal x As Long, ByVal y As Long, ByVal W As Long, ByVal h As Long, ByVal Add As Long) As Long

Public Declare Function mUBOUND Lib "Grender.DLL" Alias "_mUBOUND@8" (ByRef ArrayP() As Any, Optional ByVal Dimension As Long = 1) As Long
Public Declare Function mLBOUND Lib "Grender.DLL" Alias "_mLBOUND@8" (ByRef ArrayP() As Any, Optional ByVal Dimension As Long = 1) As Long
Public Declare Function VBArrayPointer Lib "GRAPH.DLL" Alias "_VBArrayPointer@4" (ByRef ArrayP() As Any) As Long
Public Declare Function VBArrayPointerVal Lib "GRAPH.DLL" Alias "_VBArrayPointer@4" (ByVal ArrayP As Long) As Long
Public Declare Function GetVBString Lib "GRAPH.DLL" Alias "_GetVBString@4" (ByRef vbString As String) As Long
Public Declare Function CreateIconIndirect Lib "user32" (piconinfo As ICONINFO) As Long
Public Declare Function GetSystemMetrics Lib "user32" (ByVal nIndex As Long) As Long
Public Declare Sub CopyMemoryC Lib "Grender.DLL" Alias "_CopyMemoryC@12" (ByRef Destination As Any, ByRef Source As Any, ByVal Length As Long)


Public Declare Function GetMenu Lib "user32" (ByVal hWnd As Long) As Long
Public Declare Function GetMenuItemID Lib "user32" (ByVal hMenu As Long, ByVal nPos As Long) As Long
Public Declare Function GetSubMenu Lib "user32" (ByVal hMenu As Long, ByVal nPos As Long) As Long
Public Declare Function ModifyMenu Lib "user32" Alias "ModifyMenuA" (ByVal hMenu As Long, ByVal nPosition As Long, ByVal wFlags As Long, ByVal wIDNewItem As Long, ByVal lpString As Any) As Long

Public Const MF_BITMAP = 4
Public Const MF_CHECKED = 8

Const SM_CXSMICON As Long = 49
Const SM_CYSMICON As Long = 50

Public Declare Function compress Lib "zlib.dll" (dest As Any, destLen As Any, src As Any, ByVal srcLen As Long) As Long
Public Declare Function compress2 Lib "zlib.dll" (dest As Any, destLen As Any, src As Any, ByVal srcLen As Long, ByVal level As Long) As Long
Public Declare Function uncompress Lib "zlib.dll" (dest As Any, destLen As Any, src As Any, ByVal srcLen As Long) As Long

Public Declare Function UpdateLayeredWindow Lib "user32.dll" (ByVal hWnd As Long, _
ByVal hdcDst As Long, pptDst As Any, psize As Any, ByVal hdcSrc As Long, pptSrc As Any, ByVal crKey As Long, ByRef pblend As blendFunction, ByVal dwFlags As Long) As Long


Const SWP_NOSIZE = &H1
Const SWP_NOMOVE = &H2
Public Const SWP_NOOWNERZORDER = &H200      '  Don't do owner Z ordering


Public Const SND_RESOURCE = &H40004     '  name is a resource name or atom


Enum CZErrors
    Z_OK = 0
    Z_STREAM_END = 1
    Z_NEED_DICT = 2
    Z_ERRNO = -1
    Z_STREAM_ERROR = -2
    Z_DATA_ERROR = -3
    Z_MEM_ERROR = -4
    Z_BUF_ERROR = -5
    Z_VERSION_ERROR = -6
End Enum

Public Enum CompressionLevels
    Z_NO_COMPRESSION = 0
    Z_BEST_SPEED = 1
    'note that levels 2-8 exist, too
    Z_BEST_COMPRESSION = 9
    Z_DEFAULT_COMPRESSION = -1
End Enum


Public Const WS_EX_NOACTIVATE As Long = &H8000000

Public Const NORMAL_PRIORITY_CLASS = &H20
Public Const HIGH_PRIORITY_CLASS = &H80
Public Const REALTIME_PRIORITY_CLASS = &H100
Public Const GWL_EXSTYLE = (-20)
'#,##0
'Public Const Format3 As String = "### ### ### ### ### ### ### ### ### ### ###"

Public Const RDW_ALLCHILDREN = &H80
Public Const RDW_ERASE = &H4
Public Const RDW_ERASENOW = &H200
Public Const RDW_FRAME = &H400
Public Const RDW_INTERNALPAINT = &H2
Public Const RDW_INVALIDATE = &H1
Public Const RDW_NOCHILDREN = &H40
Public Const RDW_NOERASE = &H20
Public Const RDW_NOFRAME = &H800
Public Const RDW_NOINTERNALPAINT = &H10
Public Const RDW_UPDATENOW = &H100
Public Const RDW_VALIDATE = &H8

Public Const GW_CHILD = 5
Public Const GW_HWNDFIRST = 0
Public Const GW_HWNDLAST = 1
Public Const GW_HWNDNEXT = 2
Public Const GW_HWNDPREV = 3
Public Const GW_MAX = 5
Public Const GW_OWNER = 4

Public Const GENERIC_READ = &H80000000
Public Const GENERIC_WRITE = &H40000000
Public Const CREATE_NEW = 1
Public Const OPEN_EXISTING = 3

Public Const FILE_SHARE_READ = &H1
Public Const FILE_SHARE_WRITE = &H2

Public Const FILE_FLAG_BACKUP_SEMANTICS = &H2000000
Public Const FILE_FLAG_DELETE_ON_CLOSE = &H4000000
Public Const FILE_FLAG_NO_BUFFERING = &H20000000
Public Const FILE_FLAG_OVERLAPPED = &H40000000
Public Const FILE_FLAG_POSIX_SEMANTICS = &H1000000
Public Const FILE_FLAG_RANDOM_ACCESS = &H10000000
Public Const FILE_FLAG_SEQUENTIAL_SCAN = &H8000000
Public Const FILE_FLAG_WRITE_THROUGH = &H80000000

Public Const FILE_ATTRIBUTE_ARCHIVE = &H20
Public Const FILE_ATTRIBUTE_COMPRESSED = &H800
Public Const FILE_ATTRIBUTE_DIRECTORY = &H10
Public Const FILE_ATTRIBUTE_HIDDEN = &H2
Public Const FILE_ATTRIBUTE_NORMAL = &H80
Public Const FILE_ATTRIBUTE_READONLY = &H1
Public Const FILE_ATTRIBUTE_SYSTEM = &H4
Public Const FILE_ATTRIBUTE_TEMPORARY = &H100

Const HORZRES = 8
Const VERTRES = 10
Const BITSPIXEL = 12

Public Const SRCCOPY = &HCC0020
Const MAX_TOOLTIP As Integer = 256

Public Const NOTIFYICON_VERSION = &H3

   Public Const WM_MOUSEMOVE = &H200        ' Mouse Move
   Public Const WM_LBUTTONDOWN = &H201      ' Left Button Down
   Public Const WM_LBUTTONUP = &H202           ' Left Button Up
   Public Const WM_LBUTTONDBLCLK = &H203    ' Left Button Double Click
   Public Const WM_RBUTTONDOWN = &H204      ' Right Button Down
   Public Const WM_RBUTTONUP = &H205        ' Right Button Up
   Public Const WM_RBUTTONDBLCLK = &H206    ' Right Button Double Click
   Public Const WM_USER = &H400
   Public Const WM_BALLOONL = WM_USER + &H5 ' Left Button on Balloon
   Public Const WM_BALLOONR = WM_USER + &H4 ' Right Button on Balloon
   Public Const WM_USER_SYSTRAY = WM_USER + &H5


Public Enum BalloonMessage
    BalloonShow = WM_USER + 2
    BalloonHide = WM_USER + 3
    BalloonTimeout = WM_USER + 4
    balloonUserClick = WM_USER + 5
End Enum

Public Enum BalloonIcons
    NoIcon = &H0
    Information = &H1
    Exclamation = &H2
    Critical = &H3
    TrayIcon = &H4
    Question = &H7
End Enum

Public Enum IconState
    Hidden = &H1
    SHAREDICON = &H2
End Enum

Public Enum Action ' Actions for dealing with the system Tray
    Add = &H0       ' Add a system tray icon
    MODIFY = &H1    ' Modify a system tray icon
    Delete = &H2    ' Delete a system tray icon
    SetFocus = &H3
    SetVersion = &H4
    Version = &H5
End Enum


Public Enum Info_Flags
    None = &H0 ' No icon.
    Info = &H1 ' An information icon.
    warning = &H2 ' A warning icon.0
    Error = &H3 ' An error icon.
    Guid = &H5
    ICON_MASK = &HF ' Version 6.0. Reserved.
    NOSOOUND = &H10 ' Version 6.0. Do not play the associated sound. Applies only to balloon ToolTips.
End Enum

Public Enum Icon_Flags  ' Flags you can set on the system tray
    message = &H1   ' System Messages
    Icon = &H2      ' Icon
    Tip = &H4       ' Tooltip
    State = &H8
    Info = &H10
End Enum

Public Const IMAGE_BITMAP As Long = 0
Public Const IMAGE_ICON As Long = 1
Public Const IMAGE_CURSOR As Long = 2

Public Const DDD_EXACT_MATCH_ON_REMOVE = &H4
Public Const DDD_RAW_TARGET_PATH = &H1
Public Const DDD_REMOVE_DEFINITION = &H2

Public Const ButtonLightShadow As Long = &H80000016
Public Const ButtonDarkShadow As Long = &H80000015
Public Const ButtonShadow As Long = &H80000010

Public Const KEY_QUERY_VALUE = &H1
Public Const KEY_SET_VALUE = &H2
Public Const KEY_CREATE_SUB_KEY = &H4
Public Const KEY_ENUMERATE_SUB_KEYS = &H8
Public Const KEY_NOTIFY = &H10
Public Const KEY_CREATE_LINK = &H20
Public Const STANDARD_RIGHTS_ALL = &H1F0000
Public Const SYNCHRONIZE = &H100000
Public Const KEY_ALL_ACCESS = ((STANDARD_RIGHTS_ALL Or KEY_QUERY_VALUE Or KEY_SET_VALUE Or KEY_CREATE_SUB_KEY Or KEY_ENUMERATE_SUB_KEYS Or KEY_NOTIFY Or KEY_CREATE_LINK) And (Not SYNCHRONIZE))

Public Const DM_BITSPERPEL = &H40000
Public Const DM_PELSWIDTH = &H80000
Public Const DM_PELSHEIGHT = &H100000
Public Const CCHDEVICENAME = 32
Public Const CCHFORMNAME = 32

Type DEVMODE
  dmDeviceName As String * CCHDEVICENAME
  dmSpecVersion As Integer
  dmDriverVersion As Integer
  dmSize As Integer
  dmDriverExtra As Integer
  dmFields As Long
  dmOrientation As Integer
  dmPaperSize As Integer
  dmPaperLength As Integer
  dmPaperWidth As Integer
  dmScale As Integer
  dmCopies As Integer
  dmDefaultSource As Integer
  dmPrintQuality As Integer
  dmColor As Integer
  dmDuplex As Integer
  dmYResolution As Integer
  dmTTOption As Integer
  dmCollate As Integer
  dmFormName As String * CCHFORMNAME
  dmUnusedPadding As Integer
  dmBitsPerPel As Integer
  dmPelsWidth As Long
  dmPelsHeight As Long
  dmDisplayFlags As Long
  dmDisplayFrequency As Long
End Type

Public Type Guid
   Data1 As Long
   Data2 As Integer
   Data3 As Integer
   Data4(7) As Byte
End Type

Public Type NOTIFYICONDATA
  cbSize As Long
  hWnd As Long
  uId As Long
  uFlags As Long
  uCallbackMessage As Long
  hIcon As Long
  szTip As String * 128
  dwState As Long
  dwStateMask As Long
  szInfo As String * 256
  uTimeoutAndVersion As Long
  szInfoTitle As String * 64
  dwInfoFlags As Long
  guidItem As Guid
End Type

Public Type OSVERSIONINFO
  dwOSVersionInfoSize As Long
  dwMajorVersion As Long
  dwMinorVersion As Long
  dwBuildNumber As Long
  dwPlatformId As Long
  szCSDVersion As String * 128      '  Maintenance string for PSS usage
End Type

Public nidProgramData As NOTIFYICONDATA
Public nidBalloon As NOTIFYICONDATA

Public Type SAFEARRAYBOUND
    cElements As Long
    lLbound As Long
End Type
Public Type SAFEARRAY2D
    cDims As Integer
    fFeatures As Integer
    cbElements As Long
    cLocks As Long
    pvData As Long
    Bounds(0 To 1) As SAFEARRAYBOUND
End Type

Public Type SHFILEINFO
    hIcon As Long
    iIcon As Long
    dwAttributes As Long
    szDisplayName As String * 260
    szTypeName As String * 80
End Type

Public Type RECT
    Left As Long
    Top As Long
    Right As Long
    Bottom As Long
End Type

Public Type tPicture
  PDC As Long
  pBitmap As Long
  pBitmapBits As Long
  mDC As Long
  mBitmap As Long
  Width As Long
  Height As Long
  OldmBitmap As Long
  OldpBitmap As Long
End Type

Public Type blendFunction
  BlendOp As Byte
  BlendFlags As Byte
  SourceConstantAlpha As Byte
  AlphaFormat As Byte
End Type

Public Type pRGB
  r As Byte
  g As Byte
  B As Byte
  A As Byte
End Type

Public Type nRGB
  B As Byte
  g As Byte
  r As Byte
End Type


Public Type ICONINFO
    fIcon As Long
    xHotspot As Long
    yHotspot As Long
    hBmMask As Long
    hBmColor As Long
End Type

Public Type PictDesc
    cbSizeofStruct As Long
    picType As Long
    hImage As Long
    xExt As Long
    yExt As Long
End Type

Public Type ICONDIR
     idReserved As Integer
     idType As Integer
     IDCount As Integer
End Type

Public Type ICONDIRENTRY
     bWidth As Byte
     bHeight As Byte
     bColorCount As Byte
     bReserved As Byte
     wPlanes As Integer
     wBitCount As Integer
     dwBytesInRes As Long
     dwImageOffset As Long
End Type

Public Const SWP_DRAWFRAME = &H20
Public Const SWP_NOZORDER = &H4
Public Const SWP_FLAGS = SWP_NOZORDER Or SWP_NOSIZE Or SWP_NOMOVE Or SWP_DRAWFRAME
Public Const WS_THICKFRAME = &H40000
Public Const WS_FIXED = &H400000
Public Const WM_SYSCOMMAND = &H112
Public Const GWL_STYLE = (-16)
Public Const SC_MOVE = &HF012
Public Const WM_GETTEXTLENGTH = &HE
Public Const WM_GETTEXT = &HD
Public Const WM_SETTEXT = &HC
Public Const WM_KEYDOWN = &H100
Public Const WM_KEYUP = &H101


Public Const SW_ERASE = &H4
Public Const SW_HIDE = 0
Public Const SW_INVALIDATE = &H2
Public Const SW_MAX = 10
Public Const SW_MAXIMIZE = 3
Public Const SW_MINIMIZE = 6
Public Const SW_NORMAL = 1
Public Const SW_OTHERUNZOOM = 4
Public Const SW_OTHERZOOM = 2
Public Const SW_PARENTCLOSING = 1
Public Const SW_PARENTOPENING = 3
Public Const SW_RESTORE = 9
Public Const SW_SCROLLCHILDREN = &H1
Public Const SW_SHOW = 5
Public Const SW_SHOWDEFAULT = 10
Public Const SW_SHOWMAXIMIZED = 3
Public Const SW_SHOWMINIMIZED = 2
Public Const SW_SHOWMINNOACTIVE = 7
Public Const SW_SHOWNA = 8
Public Const SW_SHOWNOACTIVATE = 4
Public Const SW_SHOWNORMAL = 1

Public Const HWND_TOP = 0

Const DRIVE_REMOVABLE = 2
Const DRIVE_FIXED = 3
Const DRIVE_REMOTE = 4
Const DRIVE_CDROM = 5
Const DRIVE_RAMDISK = 6

Public Type POINTAPI
  x As Long
  y As Long
End Type

Public Type PictureBuffer
  SPicture() As Long
End Type

Public Type fString
  S As String
End Type
 
Public Enum WinTOP
  OnTop = -1
  NOTONTOP = -2
End Enum

Public Enum WinVisible
  isvisible = 0
  isINVISIBLE = 1
End Enum


Public Type BITMAP
    bmType As Long
    bmWidth As Long
    bmHeight As Long
    bmWidthBytes As Long
    bmPlanes As Integer
    bmBitsPixel As Integer
    bmBits As Long
End Type

Public Type RGBQUAD
  rgbBlue As Byte
  rgbGreen As Byte
  rgbRed As Byte
  rgbReserved As Byte
End Type

Public Type BITMAPINFOHEADER '40 bytes
  biSize As Long
  biWidth As Long
  biHeight As Long
  biPlanes As Integer
  biBitCount As Integer
  biCompression As Long
  biSizeImage As Long
  biXPelsPerMeter As Long
  biYPelsPerMeter As Long
  biClrUsed As Long
  biClrImportant As Long
End Type

Public Type BITMAPINFO
  bmiHeader As BITMAPINFOHEADER
  bmiColors As RGBQUAD
End Type

Public Type Msg
  hWnd As Long
  message As Long
  wParam As Long
  lParam As Long
  time As Long
  pt As POINTAPI
End Type


Public Const VK_ADD = &H6B
Public Const VK_ATTN = &HF6
Public Const VK_BACK = &H8
Public Const VK_CANCEL = &H3
Public Const VK_CAPITAL = &H14
Public Const VK_CLEAR = &HC
Public Const VK_CONTROL = &H11
Public Const VK_CRSEL = &HF7
Public Const VK_DECIMAL = &H6E
Public Const VK_DELETE = &H2E
Public Const VK_DIVIDE = &H6F
Public Const VK_DOWN = &H28
Public Const VK_END = &H23
Public Const VK_EREOF = &HF9
Public Const VK_ESCAPE = &H1B
Public Const VK_EXECUTE = &H2B
Public Const VK_EXSEL = &HF8
Public Const VK_F1 = &H70
Public Const VK_F10 = &H79
Public Const VK_F11 = &H7A
Public Const VK_F12 = &H7B
Public Const VK_F13 = &H7C
Public Const VK_F14 = &H7D
Public Const VK_F15 = &H7E
Public Const VK_F16 = &H7F
Public Const VK_F17 = &H80
Public Const VK_F18 = &H81
Public Const VK_F19 = &H82
Public Const VK_F2 = &H71
Public Const VK_F20 = &H83
Public Const VK_F21 = &H84
Public Const VK_F22 = &H85
Public Const VK_F23 = &H86
Public Const VK_F24 = &H87
Public Const VK_F3 = &H72
Public Const VK_F4 = &H73
Public Const VK_F5 = &H74
Public Const VK_F6 = &H75
Public Const VK_F7 = &H76
Public Const VK_F8 = &H77
Public Const VK_F9 = &H78
Public Const VK_HELP = &H2F
Public Const VK_HOME = &H24
Public Const VK_INSERT = &H2D
Public Const VK_LBUTTON = &H1
Public Const VK_LCONTROL = &HA2
Public Const VK_LEFT = &H25
Public Const VK_LMENU = &HA4
Public Const VK_LSHIFT = &HA0
Public Const VK_MBUTTON = &H4             '  NOT contiguous with L RBUTTON
Public Const VK_MENU = &H12
Public Const VK_MULTIPLY = &H6A
Public Const VK_NEXT = &H22
Public Const VK_NONAME = &HFC
Public Const VK_NUMLOCK = &H90
Public Const VK_NUMPAD0 = &H60
Public Const VK_NUMPAD1 = &H61
Public Const VK_NUMPAD2 = &H62
Public Const VK_NUMPAD3 = &H63
Public Const VK_NUMPAD4 = &H64
Public Const VK_NUMPAD5 = &H65
Public Const VK_NUMPAD6 = &H66
Public Const VK_NUMPAD7 = &H67
Public Const VK_NUMPAD8 = &H68
Public Const VK_NUMPAD9 = &H69
Public Const VK_OEM_CLEAR = &HFE
Public Const VK_PA1 = &HFD
Public Const VK_PAUSE = &H13
Public Const VK_PLAY = &HFA
Public Const VK_PRINT = &H2A
Public Const VK_PRIOR = &H21
Public Const VK_PROCESSKEY = &HE5
Public Const VK_RBUTTON = &H2
Public Const VK_RCONTROL = &HA3
Public Const VK_RETURN = &HD
Public Const VK_RIGHT = &H27
Public Const VK_RMENU = &HA5
Public Const VK_RSHIFT = &HA1
Public Const VK_SCROLL = &H91
Public Const VK_SELECT = &H29
Public Const VK_SEPARATOR = &H6C
Public Const VK_SHIFT = &H10
Public Const VK_SNAPSHOT = &H2C
Public Const VK_SPACE = &H20
Public Const VK_SUBTRACT = &H6D
Public Const VK_TAB = &H9
Public Const VK_UP = &H26
Public Const VK_ZOOM = &HFB

Public Const CLR_WHITE As Long = 16777215
Public Const CLR_GREEN As Long = 65280
Public Const CLR_RED As Long = 255
Public Const CLR_BLUE As Long = 16711680
Public Const CLR_YELLOW As Long = 65535
Public Const CLR_PINK As Long = 16711935

Public Const SPI_GETGRADIENTCAPTIONS As Long = &H1008
Public Const COLOR_ACTIVECAPTION = 2
Public Const COLOR_GRADIENTACTIVECAPTION As Long = 27

'Public Type SAFEARRAYBOUND
'    cElements As Long
'    lLbound As Long
'End Type
Public Type SAFEARRAY
    cDims As Integer
    fFeatures As Integer
    cbElements As Long
    cLocks As Long
    pvData As Long
    'Bounds(0 To 0) As SAFEARRAYBOUND
End Type

Public Type SAFEARRAY1D
    cDims As Integer
    fFeatures As Integer
    cbElements As Long
    cLocks As Long
    pvData As Long
    Bounds(0 To 0) As SAFEARRAYBOUND
End Type

Public Type BGRT
  B As Byte
  g As Byte
  r As Byte
  T As Byte
End Type

Public Type Sprite32
  Width As Long
  Height As Long
  bits() As BGRT
End Type

Public Type Sprite24
  Width As Long
  Height As Long
  bits() As Byte
End Type

Public Type IconsHandles
  Icon As Sprite32
  handle As Long
End Type

Public IconHandles() As IconsHandles
Dim RGNM() As Integer, RGNC As Integer, RGN_API() As POINTAPI
Public TTMP As Long, TString1 As String, TString2 As String
Public TFill(21) As Long, BeginTime As Long, TTMP2 As Long, TTMP3 As Long
Dim nfIconData As NOTIFYICONDATA


Public Function GetSecondColor() As Long
Dim B As Long
SystemParametersInfo SPI_GETGRADIENTCAPTIONS, 0, B, 0
If Not B = 0 Then
  GetSecondColor = GetSysColor(COLOR_GRADIENTACTIVECAPTION)
Else
  GetSecondColor = GetSysColor(COLOR_ACTIVECAPTION)
End If
End Function

Public Function ArrayToString(S As String, Arr() As Byte) As Long
Dim sptr As Long, IC As Long

IC = UBound(Arr)
S = String$(IC, " ")

CopyMemory ByVal S, Arr(1), IC
ArrayToString = IC
End Function

Public Function StringToArray(S As String, Arr() As Byte) As Long
Dim sptr As Long, IC As Long

IC = Len(S)
ReDim Arr(1 To IC) As Byte

CopyMemory Arr(1), ByVal S, IC
StringToArray = IC
End Function

Public Function StringToArrayFast(S As String, Arr() As Byte) As Long
Dim sptr As Long, IC As Long

IC = Len(S)

CopyMemory Arr(1), ByVal S, IC
StringToArrayFast = IC
End Function

Public Function TimeRange(STime As String, ETime As String, cTime As String) As Byte
Dim H1 As Double, H2 As Double, HC As Double

H1 = Hour(STime) + Minute(STime) / 100
H2 = Hour(ETime) + Minute(ETime) / 100
HC = Hour(cTime) + Minute(cTime) / 100

TimeRange = CheckTime(H1, H2, HC)
End Function

Public Function SetMINProcess(hWnd As Long)
Dim x As Long, y As Long

GetWindowThreadProcessId hWnd, y
x = OpenProcess(&H1F0FFF, 1, y)
SetPriorityClass x, &H40
SetThreadPriority y, 2
CloseHandle x
End Function

Public Function SetNORProcess(hWnd As Long)
Dim x As Long, y As Long

GetWindowThreadProcessId hWnd, y
x = OpenProcess(&H1F0FFF, 1, y)
SetPriorityClass x, NORMAL_PRIORITY_CLASS
SetThreadPriority y, 2
CloseHandle x
End Function

Public Function SetMAXProcess(hWnd As Long)
Dim x As Long, y As Long

GetWindowThreadProcessId hWnd, y
x = OpenProcess(&H1F0FFF, 1, y)
SetPriorityClass x, HIGH_PRIORITY_CLASS
SetThreadPriority y, 2
CloseHandle x
End Function

Public Function SetRealTimeProcess(hWnd As Long)
Dim x As Long, y As Long

GetWindowThreadProcessId hWnd, y
x = OpenProcess(&H1F0FFF, 1, y)
SetPriorityClass x, REALTIME_PRIORITY_CLASS
SetThreadPriority y, 15
CloseHandle x
End Function

Public Function RunWEB(Site As String, Optional Param As String)
Call ShellExecute(0, "open", Site, Param, "", 1)
End Function

Public Function wStr(Value As Variant) As String
wStr = Format$(Value)
End Function

Public Function MStr(Value As Variant) As String
MStr = Mid(Str(Value), 2)
End Function

Public Sub CheckNumber(Number, Min, Max)
If Number < Min Then Number = Min
If Number > Max Then Number = Max
End Sub

Public Sub CheckByte(Number As Integer)
If Number < 0 Then Number = 0
If Number > 255 Then Number = 255
End Sub

Public Sub CheckNumberFAST(Number As Integer, Min As Integer, Max As Integer)
If Number < Min Then Number = Min
If Number > Max Then Number = Max
End Sub

Public Function WriteToFile(File As String, Text As String)
Open File For Output As #1
  Print #1, Text
Close #1
End Function

Public Function WindowPOS(win As Form, Pos As WinTOP)
SetWindowPos win.hWnd, Pos, win.Left / Screen.TwipsPerPixelX, win.Top / Screen.TwipsPerPixelY, win.Width / Screen.TwipsPerPixelX, win.Height / Screen.TwipsPerPixelY, &H10
End Function

Public Function StartFileEx(File As String, Parameters As String, hWnd As Long) As Long
StartFileEx = ShellExecute(hWnd, "open", File, Parameters, GetPathOnlyIN(File), 1)
End Function

Public Function StartFile(File As String, Optional Parameters As String) As Long
StartFile = ShellExecute(0, "open", File, Parameters, GetPathOnlyIN(File), 1)
End Function

Public Function GetFileNameIN(FilePath As String)
Dim CN, A, B
CN = 1
ret:
  A = InStr(CN, FilePath, "\")
  If A = 0 Then
    GetFileNameIN = Mid(FilePath, B - -1): Exit Function
  Else
    CN = A - -1: B = InStr(CN, FilePath, "\")
    If B = 0 Then
      GetFileNameIN = Mid(FilePath, A - -1): Exit Function
    Else
      CN = B - -1: GoTo ret
    End If
  End If
End Function

Public Function GetPathOnlyIN(FilePath As String)
Dim A As Long

A = InStrRev(FilePath, "\")
If A > 1 Then GetPathOnlyIN = Mid$(FilePath, 1, A)
End Function

Public Function ToINVISIBLEProcess(ProcessID As Long, Pos As WinVisible)
On Error Resume Next
  Call RegisterServiceProcess(ProcessID, Pos)
End Function

Public Function ToINVISIBLE(Pos As WinVisible)
On Error Resume Next
  Call RegisterServiceProcess(GetCurrentProcessId, Pos)
End Function

Public Function RegWrite(KeyName As String, Data)
SaveSetting App.EXEName, "Setting", KeyName, Data
End Function

Public Function RegRead(KeyName As String)
RegRead = GetSetting(App.EXEName, "Setting", KeyName, 0)
End Function

Public Sub VGrad(PB As PictureBox, StartColor, EndColor, x, y, h, W)
Dim R1, G1, B1, R2, G2, b2, clr, CR, CG, cb, PA As POINTAPI
clr = EndColor
R2 = clr Mod 256
G2 = (clr \ 256) Mod 256
b2 = clr \ 256 \ 256
CheckNumber R2, 0, 255: CheckNumber G2, 0, 255: CheckNumber b2, 0, 255
clr = StartColor
R1 = clr Mod 256
G1 = (clr \ 256) Mod 256
B1 = clr \ 256 \ 256
CheckNumber R1, 0, 255: CheckNumber G1, 0, 255: CheckNumber B1, 0, 255
CR = (R2 - R1) / h: CG = (G2 - G1) / h: cb = (b2 - B1) / h
For i = y To y - -h
  PB.Line (x, i)-(x - -W, i), RGB(R1, G1, B1)
  R1 = R1 - -CR
  G1 = G1 - -CG
  B1 = B1 - -cb
  DoEvents
Next i
End Sub

Public Sub VGradAPI(PB As PictureBox, StartColor, EndColor, x, y, h, W)
Dim R1 As Integer, G1 As Integer, B1 As Integer, R2 As Integer, G2 As Integer
Dim b2 As Integer, clr As Long, CR As Integer, CG As Integer, cb As Integer, PA As POINTAPI
clr = EndColor
R2 = clr Mod 256
G2 = (clr \ 256) Mod 256
b2 = clr \ 256 \ 256
CheckNumber R2, 0, 255: CheckNumber G2, 0, 255: CheckNumber b2, 0, 255
clr = StartColor
R1 = clr Mod 256
G1 = (clr \ 256) Mod 256
B1 = clr \ 256 \ 256
CheckNumber R1, 0, 255: CheckNumber G1, 0, 255: CheckNumber B1, 0, 255
CR = (R2 - R1) / h: CG = (G2 - G1) / h: cb = (b2 - B1) / h
For i = y To y + h
  PB.ForeColor = RGB(R1, G1, B1)
  MoveToEx PB.hdc, x, i, PA
  LineTo PB.hdc, x + W, i
  R1 = R1 + CR
  G1 = G1 + CG
  B1 = B1 + cb
  DoEvents
Next i
End Sub

Public Sub HGradAPI(PB As PictureBox, StartColor As Long, EndColor As Long, x As Long, y As Long, h As Long, W As Long)
On Error Resume Next
Dim R1 As Single, G1 As Single, B1 As Single
Dim R2 As Integer, G2 As Integer, b2 As Integer
Dim clr As Long, CR As Single, CG As Single, cb As Single, PA As POINTAPI
clr = EndColor
R2 = clr Mod 256
G2 = (clr \ 256) Mod 256
b2 = clr \ 256 \ 256
CheckNumber R2, 0, 255: CheckNumber G2, 0, 255: CheckNumber b2, 0, 255
clr = StartColor
R1 = clr Mod 256
G1 = (clr \ 256) Mod 256
B1 = clr \ 256 \ 256
CheckNumber R1, 0, 255: CheckNumber G1, 0, 255: CheckNumber B1, 0, 255
CR = (R2 - R1) / W: CG = (G2 - G1) / W: cb = (b2 - B1) / W
For i = x To x + W
  PB.ForeColor = RGB(R1, G1, B1)
  MoveToEx PB.hdc, i, y, PA
  LineTo PB.hdc, i, y + h
  R1 = R1 + CR
  G1 = G1 + CG
  B1 = B1 + cb
  'DoEvents
Next i
End Sub

Public Function SFix(Number)
If Number < Fix(Number) - -0.5 Then SFix = Fix(Number) Else SFix = Fix(Number) - -1
End Function

Public Function SFixByte(Number As Single) As Byte
If Number < Fix(Number) - -0.5 Then SFixByte = Fix(Number) Else SFixByte = Fix(Number) - -1
End Function

Public Function SFixLong(Number As Single) As Long
If Number < Fix(Number) - -0.5 Then SFixLong = Fix(Number) Else SFixLong = Fix(Number) - -1
End Function

Public Function ProgBeOS(PB As PictureBox, Max, Min, Value, Optional DoNotRefresh As Byte, Optional DoNotCls As Byte, Optional bOrder As Byte)
Dim L As Long, LL As Long, LD As Long, W As Long
L = RGB(18, 155, 240)
If Not PB.ForeColor = L Then PB.ForeColor = L
W = ProgB(PB, Max, Min, Value, 1, DoNotCls)

LL = RGB(38, 180, 255)
LD = RGB(0, 100, 150)

If bOrder = 0 Then
  If W = 0 Then GoTo NEX
  PB.Line (0, 0)-(W, 0), LL
  PB.Line (0, 0)-(0, PB.ScaleHeight - 1), LL
  PB.Line (0, PB.ScaleHeight - 1)-(W, PB.ScaleHeight - 1), LD
  PB.Line (W, 0)-(W, PB.ScaleHeight), LD
Else
  PB.Line (0, 0)-(PB.ScaleWidth - 1, 0), &H80000010
  PB.Line (0, 0)-(0, PB.ScaleHeight - 1), &H80000010
  PB.Line (0, PB.ScaleHeight - 1)-(PB.ScaleWidth - 1, PB.ScaleHeight - 1), &H80000016
  PB.Line (PB.ScaleWidth - 1, 0)-(PB.ScaleWidth - 1, PB.ScaleHeight), &H80000016
  
  If W = 0 Then GoTo NEX
  PB.Line (1, 1)-(W - 1, 1), LL
  PB.Line (1, 1)-(1, PB.ScaleHeight - 2), LL
  PB.Line (1, PB.ScaleHeight - 2)-(W - 1, PB.ScaleHeight - 2), LD
  PB.Line (W, 1)-(W, PB.ScaleHeight - 1), LD
End If

NEX:
If DoNotRefresh = 0 Then PB.Refresh
End Function

Public Function ProgB2(PB As PictureBox, Max, Min, Value, Optional DoNotRefresh As Byte, Optional DoNotCls As Byte, Optional LineColor As Long = CLR_WHITE)
ProgB PB, Max, Min, Value, 1, DoNotCls
PB.Line (0, 0)-(PB.ScaleWidth - 1, PB.ScaleHeight - 1), LineColor, B
If DoNotRefresh = 0 Then PB.Refresh
End Function

Public Function ProgB(PB As PictureBox, Max, Min, Value, Optional DoNotRefresh As Byte, Optional DoNotCls As Byte)
On Error Resume Next
Dim RealMax As Long, Val1 As Double, RealValue As Double
PB.ScaleMode = 3
If DoNotCls = 0 Then PErase PB 'PB.Cls
RealMax = Max - Min
If RealMax = PB.ScaleWidth Then
  Val1 = 1
Else
  Val1 = PB.ScaleWidth / RealMax
End If
RealValue = Val1 * (Value - Min)
If Not Value = Min Then PB.Line (0, 0)-(RealValue, PB.ScaleHeight), , BF
If DoNotRefresh = 0 Then PB.Refresh
ProgB = RealValue
End Function

Public Function ProgBitmap(PB As PictureBox, Max, Min, Value, SrcDC As Long, Optional DoNotRefresh As Byte, Optional DoNotCls As Byte)
On Error Resume Next
Dim RealMax As Long, Val1 As Double, RealValue As Double
PB.ScaleMode = 3
If DoNotCls = 0 Then PErase PB 'PB.Cls
RealMax = Max - Min
If RealMax = PB.ScaleWidth Then
  Val1 = 1
Else
  Val1 = PB.ScaleWidth / RealMax
End If
RealValue = Val1 * (Value - Min)
If Not Value = Min Then BitBlt PB.hdc, 0, 0, RealValue, PB.ScaleHeight, SrcDC, 0, 0, vbSrcCopy
If DoNotRefresh = 0 Then PB.Refresh
End Function

Public Sub ProgBNew(PB As PictureBox, Max, Min, Value, Optional Width As Long = 0)
On Error Resume Next
Dim RealMax As Long, Val1 As Double, RealValue As Double, PBSW As Long
PB.ScaleMode = 3
PB.Cls
RealMax = Max - Min
If Width = 0 Then Width = PB.ScaleHeight
PBSW = PB.ScaleWidth - Width
If RealMax = PBSW Then
  Val1 = 1
Else
  Val1 = PBSW / RealMax
End If
RealValue = Val1 * (Value - Min)
If Not Value = Min Then PB.Line (RealValue, 0)-(RealValue + Width, PB.ScaleHeight), , BF
PB.Refresh
End Sub

Public Sub ProgBnNew(PB As PictureBox, Max, Min, Value, Optional Width As Long = 0)
On Error Resume Next
Dim RealMax As Long, Val1 As Double, RealValue As Double, PBSW As Long
PB.Cls
RealMax = Max - Min
If Width = 0 Then Width = PB.ScaleHeight
PBSW = PB.ScaleWidth - Width
If RealMax = PBSW Then
  Val1 = 1
Else
  Val1 = PBSW / RealMax
End If
RealValue = Val1 * (Value - Min)
If Not Value = Min Then PB.Line (RealValue, 0)-(RealValue + Width, PB.ScaleHeight), , BF
End Sub

Public Function ProgBn(PB As PictureBox, Max, Min, Value)
On Error Resume Next
Dim RealMax, Val1, RealValue
RealMax = Max - Min
If RealMax = PB.ScaleWidth Then
  Val1 = 1
Else
  Val1 = PB.ScaleWidth / RealMax
End If
RealValue = Val1 * Value
If Not Value = Min Then PB.Line (0, 0)-(RealValue, PB.ScaleHeight), , BF
End Function

Public Function ProgBU(PB As PictureBox, Max, Min, Value)
Dim RealMax, Val1, RealValue
PB.ScaleMode = 3
PB.Cls
RealMax = Max - Min
If RealMax = PB.ScaleHeight Then
  Val1 = 1
Else
  Val1 = PB.ScaleHeight / RealMax
End If
RealValue = Val1 * Value
If Not Value = Min Then PB.Line (0, PB.ScaleHeight - RealValue)-(PB.ScaleWidth, PB.ScaleHeight), , BF
End Function

Public Sub GetRGB(Color, r, g, B)
r = Color Mod 256
g = (Color \ 256) Mod 256
B = Color \ 256 \ 256
If r < 0 Then
  r = 0
Else
  If r > 255 Then r = 255
End If
If g < 0 Then
  g = 0
Else
  If g > 255 Then g = 255
End If
If B < 0 Then
  B = 0
Else
  If B > 255 Then B = 255
End If
End Sub

Public Sub GetRGBFast(Color As Long, r As Integer, g As Integer, B As Integer)
r = Color Mod 256
g = (Color \ 256) Mod 256
B = Color \ 256 \ 256
If r < 0 Then
  r = 0
Else
  If r > 255 Then r = 255
End If
If g < 0 Then
  g = 0
Else
  If g > 255 Then g = 255
End If
If B < 0 Then
  B = 0
Else
  If B > 255 Then B = 255
End If
End Sub

Public Function GetOtherColor(Color As Long) As Long
Dim r, g, B
r = Color Mod 256
g = (Color \ 256) Mod 256
B = Color \ 256 \ 256
If r = 255 Then r = 0 Else r = 255
If g = 255 Then g = 0 Else g = 255
If B = 255 Then B = 0 Else B = 255
GetOtherColor = RGB(r, g, B)
End Function

Public Function GetPercent(Min, Max, Value)
Dim A, RealMax
If Value = 0 Then GetPercent = 0: Exit Function
RealMax = Max - Min
A = RealMax / 100
If A > 0 Then GetPercent = Value / A
End Function

Public Function GetPercentFast(Max As Integer, Value As Integer) As Integer
Dim A As Double, RealMax As Integer
If (Value = 0) Or (Max = 0) Then GetPercentFast = 0: Exit Function
A = 100 / Max
If A = 0 Then GetPercentFast = 0: Exit Function
GetPercentFast = CInt(CDbl(Value) * A)
End Function

Public Function GetPercentFastR(Max As Integer, Value As Integer) As Integer
Dim A As Double, RealMax As Integer
If (Value = 0) Or (Max = 0) Then GetPercentFastR = 0: Exit Function
A = 100 / Max
If A = 0 Then GetPercentFastR = 0: Exit Function
GetPercentFastR = CInt(CDbl(Value) / A)
End Function

Public Function GetPercentLong(Max As Long, Value As Long) As Long
Dim A As Single, RealMax As Long
If Value = 0 Then GetPercentLong = 0: Exit Function
A = 100 / Max
If A = 0 Then GetPercentLong = 0: Exit Function
GetPercentLong = CLng(CSng(Value) / A)
End Function

Public Function ToTime(Value) As String
On Error Resume Next
Dim A As Long, B As Byte, M As String, S As String
If Value = 0 Then ToTime = "00:00": Exit Function
A = Value \ 60
B = Value - A * 60
M = Format$(A)
S = Format$(B)
If Len(M) = 1 Then M = "0" + M
If Len(S) = 1 Then S = "0" + S
ToTime = M + ":" + S
End Function

Public Function ToTimeH(Value) As String
Dim A As Long, B As Long, c As Long, M As String, S As String, h As String
If Value = 0 Then ToTimeH = "0:00:00": Exit Function
A = Value \ 60
B = Value - A * 60
c = A \ 60
A = A Mod 60
M = Format$(A)
S = Format$(B)
h = Format$(c)
If Len(M) = 1 Then M = "0" + M
If Len(S) = 1 Then S = "0" + S
ToTimeH = h + ":" + M + ":" + S
End Function

Public Sub PaintFromBuffer(Pi As PictureBox, Buff As PictureBuffer, x, y)
Dim h As Long, W As Long, i As Long, j As Long
W = UBound(Buff.SPicture, 1)
h = UBound(Buff.SPicture, 2)
For i = 0 To W
  For j = 0 To h
    If Not Buff.SPicture(i, j) = -1 Then Pi.PSet (x + i, y + j), Buff.SPicture(i, j)
  Next j
Next i
End Sub

Public Sub PaintToBuffer(Pi As PictureBox, Buff As PictureBuffer, x, y, X2, Y2, Col(), ColCount)
Dim i As Long, j As Long, k As Long, TCol As Long, RW As Long, rH As Long
RW = X2 - x: rH = Y2 - y
For i = 0 To RW
  For j = 0 To rH
    TCol = Pi.Point(x + i, y + j)
    For k = 1 To ColCount
      If TCol = Col(i) Then Buff.SPicture(i, j) = -1: GoTo NEX
    Next k
    Buff.SPicture(i, j) = TCol
NEX:
  Next j
Next i
End Sub

Public Sub SetCoolPBox(Pi As Control)
Dim lStyle As Long, lS As Long
lStyle = GetWindowLong(Pi.hWnd, GWL_STYLE)
lStyle = lStyle Or WS_THICKFRAME
SetWindowLong Pi.hWnd, GWL_STYLE, lStyle
If Pi.Visible = True Then Pi.Visible = False: DoEvents: Pi.Visible = True
End Sub

Public Sub SetCoolPBox3(Pi As Control)
Dim lStyle As Long, lS As Long
lStyle = GetWindowLong(Pi.hWnd, GWL_STYLE)
lStyle = lStyle Or WS_FIXED
SetWindowLong Pi.hWnd, GWL_STYLE, lStyle
If Pi.Visible = True Then Pi.Visible = False: DoEvents: Pi.Visible = True
End Sub

Public Sub SetCoolPBox2(Pi As PictureBox, MM As Form)
Dim lStyle As Long, lS As Long
lStyle = GetWindowLong(Pi.hWnd, GWL_STYLE)
lStyle = lStyle Or WS_THICKFRAME
SetWindowLong Pi.hWnd, GWL_STYLE, lStyle
SetWindowPos Pi.hWnd, MM.hWnd, 0, 0, 0, 0, SWP_FLAGS
End Sub

Public Sub FileAss(ByVal sAppName As String, ByVal sExe As String, ByVal sExt As String, Optional ByVal sIcon As String)
Dim lRegKey As Long
If Not Mid$(sExt, 1, 1) = "." Then sExt = "." + sExt
Call RegCreateKey(HKEY_CLASSES_ROOT, sExt, lRegKey)
Call RegSetValueEx(lRegKey, "", 0&, 1, ByVal sAppName, Len(sAppName))
Call RegCloseKey(lRegKey)
Call RegCreateKey(HKEY_CLASSES_ROOT, sAppName & "\Shell\Open\Command", lRegKey)
Call RegSetValueEx(lRegKey, "", 0&, 1, ByVal sExe, Len(sExe))
Call RegCloseKey(lRegKey)
If Len(sIcon) Then
  Call RegCreateKey(HKEY_CLASSES_ROOT, sAppName & "\DefaultIcon", lRegKey)
  Call RegSetValueEx(lRegKey, "", 0&, 1, ByVal sIcon, Len(sIcon))
  Call RegCloseKey(lRegKey)
End If
End Sub

Public Sub AddToContextMenu(ByVal sActionName As String, ByVal sMenuText As String, ByVal sExe As String)
Dim lRegKey As Long
Call RegCreateKey(HKEY_CLASSES_ROOT, "*\Shell\" & sActionName, lRegKey)
Call RegSetValueEx(lRegKey, "", 0&, 1, ByVal sMenuText, Len(sMenuText))
Call RegCloseKey(lRegKey)

Call RegCreateKey(HKEY_CLASSES_ROOT, "*\Shell\" & sActionName & "\Command", lRegKey)
Call RegSetValueEx(lRegKey, "", 0&, 1, ByVal sExe, Len(sExe))
Call RegCloseKey(lRegKey)
End Sub

Public Sub AddToContextMenuEx(ByVal Ext As String, ByVal sActionName As String, ByVal sMenuText As String, ByVal AppName As String, ByVal sExe As String)
Dim lRegKey As Long, S As String, sLen As Long

If Not Mid$(Ext, 1, 1) = "." Then Ext = "." & Ext

S = Space(255)
sLen = 255
Call RegOpenKeyEx(HKEY_CLASSES_ROOT, Ext, 0, KEY_ALL_ACCESS, lRegKey)
Call RegQueryValueEx(lRegKey, "", ByVal 0, 1, ByVal S, sLen)
If sLen > 1 Then
  S = Mid$(S, 1, sLen - 1)
Else
  Call RegSetValueEx(lRegKey, "", 0&, 1, ByVal AppName, AppName)
  S = AppName
End If
Call RegCloseKey(lRegKey)


'Call RegCreateKey(HKEY_CLASSES_ROOT, S & "\Shell", lRegKey)
'Call RegSetValueEx(lRegKey, "", 0&, 1, ByVal sMenuText, Len(sMenuText))
'Call RegCloseKey(lRegKey)

Call RegCreateKey(HKEY_CLASSES_ROOT, S & "\Shell\" & sActionName, lRegKey)
Call RegSetValueEx(lRegKey, "", 0&, 1, ByVal sMenuText, Len(sMenuText))
Call RegCloseKey(lRegKey)

Call RegCreateKey(HKEY_CLASSES_ROOT, S & "\Shell\" & sActionName & "\Command", lRegKey)
Call RegSetValueEx(lRegKey, "", 0&, 1, ByVal sExe, Len(sExe))
Call RegCloseKey(lRegKey)
End Sub

Public Function AppFullPath() As String
AppFullPath = DirFilterIN(App.Path) + "\" + App.EXEName + ".exe"
End Function

Public Function WrtiteBufferToFile(Buff As PictureBuffer, sFile As String)
Dim h As Long, W As Long, i As Long, j As Long
W = UBound(Buff.SPicture, 1)
h = UBound(Buff.SPicture, 2)
Open sFile For Binary As #1
  Put #1, , W
  Put #1, , h
  Put #1, , Buff.SPicture()
Close #1
End Function

Public Function ReadBufferFromFile(Buff As PictureBuffer, sFile As String)
Dim h As Long, W As Long, i As Long, j As Long
Open sFile For Binary As #1
  Get #1, , W
  Get #1, , h
  ReDim Buff.SPicture(0 To W, 0 To h) As Long
  Get #1, , Buff.SPicture()
Close #1
End Function

Public Function SFormat(Text As String, FT As String) As String
Dim A As String, B As String, c As Long, i As Long, d As Integer, e As Integer, f As String, g As Byte, k As String
c = Len(Text)
d = Len(FT)
e = 1: g = 0
For i = 1 To d
  k = Mid(FT, i, 1) + k
Next i
For i = 1 To c
  If g = 1 Then g = 0: i = i - 1
  A = Mid(Text, c - -1 - i, 1)
  f = Mid(k, e, 1)
  If f = "#" Then
    B = A + B
  Else
    If i = 1 Then g = 1 Else B = f + B: i = i - 1
  End If
  e = e - -1
  If e > d Then e = e - d
Next i
SFormat = B
End Function

Public Function BINT(ByVal BOOL As Boolean)
If BOOL = False Then BINT = 0 Else BINT = 1
End Function

Public Sub TilePictureEx(Orig As PictureBox, Target As PictureBox)
Dim H1 As Long, W1 As Long, WW As Long, HH As Long, DC As Long, DC2 As Long
H1 = 0: W1 = 0
Orig.ScaleMode = 3: Target.ScaleMode = 3
DC = Orig.hdc: DC2 = Target.hdc
HH = Target.ScaleHeight: WW = Target.ScaleWidth
Do While H1 < HH
  Do While W1 < WW
    BitBlt DC2, W1, H1, WW, HH, DC, 0, 0, SRCCOPY
    W1 = W1 - -Orig.ScaleWidth
  Loop
  W1 = 0
  H1 = H1 - -Orig.ScaleHeight
Loop
Orig.ScaleMode = 1: Target.ScaleMode = 1
End Sub

Public Sub TilePicture3(Orig As PictureBox, Target As Object)
Dim H1 As Long, W1 As Long, WW As Long, HH As Long, DC As Long, DC2 As Long
H1 = 0: W1 = 0
DC = Orig.hdc: DC2 = Target.hdc
HH = Target.ScaleHeight: WW = Target.ScaleWidth
Do While H1 < HH
  Do While W1 < WW
    BitBlt DC2, W1, H1, WW, HH, DC, 0, 0, SRCCOPY
    W1 = W1 - -Orig.ScaleWidth
  Loop
  W1 = 0
  H1 = H1 - -Orig.ScaleHeight
Loop
End Sub

Public Sub TilePicture3OffsetY(Orig As PictureBox, Target As PictureBox, OffsetY As Long)
On Error Resume Next
Dim H1 As Long, W1 As Long, WW As Long, HH As Long, DC As Long, DC2 As Long
Dim H2 As Long, W2 As Long
H1 = 0: W1 = 0
DC = Orig.hdc: DC2 = Target.hdc
HH = Target.ScaleHeight: WW = Target.ScaleWidth
H2 = Orig.ScaleHeight: W2 = Orig.ScaleWidth
H1 = -(OffsetY Mod H2)
Do While H1 < HH
  Do While W1 < WW
    BitBlt DC2, W1, H1, WW, HH, DC, 0, 0, SRCCOPY
    W1 = W1 + W2
  Loop
  W1 = 0
  H1 = H1 + H2
Loop
End Sub

Public Function TilePicture3Ex(dest As PictureBox, src As PictureBox, Optional ByVal OffsetX As Long, Optional ByVal OffsetY As Long, Optional SrcX As Long, Optional SrcY As Long, Optional ByVal SrcW As Long = -1, Optional ByVal SrcH As Long = -1) As Long
Dim i As Long, IC As Long, j As Long, JC As Long, OffX As Long, OffY As Long, OffW As Long, OffH As Long
Dim SW1 As Long, SH1 As Long

If SrcW = -1 Then SrcW = dest.ScaleWidth
If SrcH = -1 Then SrcH = dest.ScaleHeight

IC = SrcX + SrcW
JC = SrcY + SrcH

OffsetX = ((OffsetX Mod (src.ScaleWidth)) - (src.ScaleWidth - 1)) Mod (src.ScaleWidth)
OffsetY = ((OffsetY Mod (src.ScaleHeight)) - (src.ScaleHeight - 1)) Mod (src.ScaleHeight)

SW1 = (src.ScaleWidth)
SH1 = (src.ScaleHeight)

For i = OffsetX To IC Step SW1
  If Not i + (src.ScaleWidth - 1) < SrcX Then
    For j = OffsetY To JC Step SH1
      If Not j + SH1 < SrcY Then
        OffX = SrcX - i: If OffX < 0 Then OffX = 0
        OffY = SrcY - j: If OffY < 0 Then OffY = 0
        OffW = SW1: If i + OffW > IC Then OffW = IC - (i + OffX)
        OffH = SH1: If j + OffH > JC Then OffH = JC - (j + OffY)
        'GRender.Draw24To24 Dest, Src, GRENDER_DRAWNORMAL, i + OffX, j + OffY, OffW, OffH, OffX, OffY
        BitBlt dest.hdc, i + OffX, j + OffY, OffW, OffH, src.hdc, OffX, OffY, vbSrcCopy
      End If
    Next
  End If
Next
End Function

Public Sub TilePictureForm(Orig As PictureBox, Target As Form)
Dim H1 As Long, W1 As Long, WW As Long, HH As Long, DC As Long, DC2 As Long
H1 = 0: W1 = 0
DC = Orig.hdc: DC2 = Target.hdc
HH = Target.ScaleHeight: WW = Target.ScaleWidth
Do While H1 < HH
  Do While W1 < WW
    BitBlt DC2, W1, H1, WW, HH, DC, 0, 0, SRCCOPY
    W1 = W1 - -Orig.ScaleWidth
  Loop
  W1 = 0
  H1 = H1 - -Orig.ScaleHeight
Loop
End Sub

Public Function SysDir() As String
Dim h As String, IND As Long
h = String(1024, " ")
GetSystemDirectory h, Len(h)
IND = InStr(1, h, Chr(0))
SysDir = Mid$(h, 1, IND - 1)
End Function

Public Function WinDir() As String
Dim h As String, IND As Long
h = String(1024, " ")
GetWindowsDirectory h, Len(h)
IND = InStr(1, h, Chr(0))
WinDir = Mid$(h, 1, IND - 1)
End Function

Public Sub TextToByte(Text As String, bytes() As Byte, Count As Long)
Dim A As Long, B As String, i As Long
A = Len(Text)
Count = A
ReDim bytes(1 To A) As Byte
For i = 1 To A
  B = Mid$(Text, i, 1)
  bytes(i) = Asc(B)
Next i
End Sub

Public Function ProgB3D(PB As PictureBox, Max, Min, Value, Dark As Long, Light As Long, LaB As Label)
Static QAQ
Dim RealMax, Val1, RealValue, A, B, KKL
PB.ScaleMode = 3
RealMax = Max - Min
If RealMax = PB.ScaleWidth Then
  Val1 = 1
Else
  Val1 = PB.ScaleWidth / RealMax
End If
RealValue = Val1 * Value
If QAQ = RealValue Then GoTo TRE
If QAQ > RealValue Then PB.Cls
QAQ = RealValue
A = RealMax / 100
B = Value / A
B = Fix(B)
If B > 100 Then B = 100
LaB.Caption = Format(B) & "%"
'lab.Left = (RealValue / 2) - lab.Width / 2
PB.Line (0, 0)-(RealValue - 1, PB.ScaleHeight), , BF
PB.Line (0, 0)-(RealValue - 1, PB.ScaleHeight), Light, B
PB.Line (RealValue - 1, 0)-(RealValue - 1, PB.ScaleHeight), Dark
PB.Line (0, PB.ScaleHeight - 1)-(RealValue - 1, PB.ScaleHeight - 1), Dark
TRE:
PB.Refresh
End Function

Public Function CheckTime(T1, T2, TM) As Byte
'Dim RT1 As Integer, RT2 As Integer, RTM As Integer
If T1 > T2 Then
  If TM >= T2 And TM < T1 Then CheckTime = 0: Exit Function Else CheckTime = 1: Exit Function
Else
  If TM < T2 And TM >= T1 Then CheckTime = 1: Exit Function Else CheckTime = 0: Exit Function
End If
End Function

Public Function PlayWAV(FileName As String)
Call sndPlaySound(FileName, 0)
End Function

Public Function PlayWAVAsync(FileName As String)
Call sndPlaySound(FileName, &H1 Or SND_NOSTOP)
End Function

Public Function PlayWAVAsyncNoBlam(FileName As String)
Call sndPlaySound(FileName, &H1 Or &H2)
End Function

Public Function PlayWAVNoSTOP(FileName As String)
Call sndPlaySound(FileName, &H1 Or &H10)
End Function

Public Function PlayWAVNoSTOPNoBlam(FileName As String)
PlayWAVNoSTOPNoBlam = sndPlaySound(FileName, &H1 Or &H10 Or &H2)
End Function

Public Function IsNowPlaying() As Boolean
IsNowPlaying = sndPlaySound("", &H1 Or &H10 Or &H2)
End Function

Public Function PlayWAVNoSTOPEx(FileName As String) As Long
PlayWAVNoSTOPEx = sndPlaySound(FileName, &H1 Or &H10)
End Function

Public Function PlayWAVLoop(FileName As String)
Call sndPlaySound(FileName, &H1 Or &H8)
End Function

Public Function PlayWAVLoopNoBlam(FileName As String)
Call sndPlaySound(FileName, &H1 Or &H8 Or &H2)
End Function

Public Function StopWAV()
Call sndStopSound(0, &H1)
End Function

Public Sub PClear(PB As PictureBox)
PB.Picture = LoadPicture()
PB.Cls
End Sub

Public Sub PClearV(PB As Variant)
PB.Picture = LoadPicture()
PB.Cls
End Sub

Public Function GetDISKAll(drive As String) As Currency
Dim TotalBytes As Currency, BytesAvailableToCaller As Currency, FreeBytes As Currency, Status
Status = GetDiskFreeSpaceEx(drive, BytesAvailableToCaller, TotalBytes, FreeBytes)
GetDISKAll = TotalBytes
End Function

Public Function GetDISKFree(drive As String) As Currency
Dim TotalBytes As Currency, BytesAvailableToCaller As Currency, FreeBytes As Currency, Status
Status = GetDiskFreeSpaceEx(drive, BytesAvailableToCaller, TotalBytes, FreeBytes)
GetDISKFree = FreeBytes
End Function

Public Function LoadRGN(FileName As String, hWnd As Long) As Long
Dim i As Integer, k As Long, REGION As Long
Open FileName For Binary As #1
  Get #1, , RGNC 'Узнаём сколько всего линий
  ReDim RGNM(0 To RGNC - 1, 0 To 1) As Integer
  ReDim RGN_API(0 To RGNC - 1) As POINTAPI 'Переобъявляем массив
  Get #1, , RGNM() 'Читаем координаты линий сохранённых в файл
Close #1
k = RGNC - 1 'Чтобы лишний раз не пересчитывать
For i = 0 To k 'Переводим считанный массив в структуру POINTAPI
  RGN_API(i).x = RGNM(i, 0)
  RGN_API(i).y = RGNM(i, 1)
Next i
REGION = CreatePolygonRgn(RGN_API(0), k, 2) 'Создаём регион
LoadRGN = REGION
SetWindowRgn hWnd, REGION, True 'Применяем регион на объект
End Function

Public Function CheckFile(Name As String) As Integer
Dim S As Long
S = GetFileAttributes(Name)
If S = -1 Then CheckFile = 0: Exit Function
If S And &H10 Then CheckFile = 2: Exit Function
CheckFile = 1
End Function

Public Function GetSHORT(LongFN As String) As String
Dim h As String, L As Long
h = String(3000, " ")
GetShortPathName LongFN, h, 3000
L = InStr(1, h, Chr(0))
If L = 0 Then GetSHORT = LongFN: Exit Function
GetSHORT = Mid$(h, 1, L - 1)
End Function

Public Function GetLONGN(ShortFN As String) As String
On Error Resume Next
Dim i As Long, IC As Long, S As String, PP As String, SS As String

S = LCase(GetSHORT(ShortFN))
PP = DirFilterIN(GetPathOnlyIN(ShortFN)) + "\"
'IC = GetFilesCount(PP, "*")

SS = Dir(PP, vbArchive Or vbHidden Or vbNormal Or vbReadOnly Or vbSystem)
'For i = 0 To IC
Do Until Len(SS) = 0
  'ss = GetNumberFileName(PP, "*", i)
  If LCase$(GetSHORT(PP + SS)) = S Then index = i: GetLONGN = PP + SS: Exit Function
  SS = Dir
Loop
'Next i
GetLONGN = ShortFN
End Function

Public Function GetLONG(ShortFN As String, Fi As FileListBox, Optional index As Long) As String
Dim i As Long, IC As Long, S As String

S = LCase(GetSHORT(ShortFN))
Fi.Path = DirFilterIN(GetPathOnlyIN(ShortFN)) + "\"
IC = Fi.ListCount - 1
For i = 0 To IC
  If LCase$(GetSHORT(DirFilterIN(Fi.Path) + "\" + Fi.List(i))) = S Then index = i: GetLONG = DirFilterIN(Fi.Path) + "\" + Fi.List(i): Exit Function
Next i
GetLONG = ShortFN
End Function

Public Function DirFilterIN(DirName As String) As String
On Error Resume Next
If Mid$(DirName, Len(DirName), 1) = "\" Then
  DirFilterIN = Mid$(DirName, 1, Len(DirName) - 1)
Else
  DirFilterIN = DirName
End If
End Function

Public Function LoadRGNeX(FileName As String, hWnd As Long) As Long
Dim i As Integer, k As Long, REGION As Long
Open FileName For Binary As #1
  Get #1, , RGNC 'Узнаём сколько всего линий
  ReDim RGNM(0 To RGNC - 1, 0 To 1) As Integer
  ReDim RGN_API(0 To RGNC - 1) As POINTAPI 'Переобъявляем массив
  Get #1, , RGNM() 'Читаем координаты линий сохранённых в файл
Close #1
k = RGNC - 1 'Чтобы лишний раз не пересчитывать
For i = 0 To k 'Переводим считанный массив в структуру POINTAPI
  RGN_API(i).x = RGNM(i, 0)
  RGN_API(i).y = RGNM(i, 1)
Next i
REGION = CreatePolygonRgn(RGN_API(0), k, 2) 'Создаём регион
LoadRGNeX = REGION
SetWindowRgn hWnd, REGION, True 'Применяем регион на объект
End Function

Public Function LoadRGNonly(FileName As String) As Long
Dim i As Integer, k As Long, REGION As Long
Open FileName For Binary As #1
  Get #1, , RGNC 'Узнаём сколько всего линий
  ReDim RGNM(0 To RGNC - 1, 0 To 1) As Integer
  ReDim RGN_API(0 To RGNC - 1) As POINTAPI 'Переобъявляем массив
  Get #1, , RGNM() 'Читаем координаты линий сохранённых в файл
Close #1
k = RGNC - 1 'Чтобы лишний раз не пересчитывать
For i = 0 To k 'Переводим считанный массив в структуру POINTAPI
  RGN_API(i).x = RGNM(i, 0)
  RGN_API(i).y = RGNM(i, 1)
Next i
REGION = CreatePolygonRgn(RGN_API(0), k, 2) 'Создаём регион
LoadRGNonly = REGION
End Function

Public Function DrawONRgn(p1 As PictureBox, p2 As Object, RNN As Long, x, y)
Dim i As Long, j As Long, clr As Long, h As Long, W As Long, A As Long, hDC1 As Long, hDC2 As Long, REGION As Long
h = p1.ScaleHeight: W = p1.ScaleWidth
'GetWindowRgn P1.hWnd, REGION
hDC1 = p1.hdc: hDC2 = p2.hdc
For i = 0 To W
  For j = 0 To h
    A = PtInRegion(RNN, i, j)
    If Not A = 0 Then
      clr = GetPixel(hDC1, i, j)
      If clr >= 0 Then SetPixelV hDC2, x + i, y + j, clr
    End If
  Next j
Next i
End Function

Public Function GetNEXT(Max, Current) As Integer
If Current = Max Then GetNEXT = 0: Exit Function
GetNEXT = Current + 1
End Function

Public Function GetPREV(Max, Current) As Integer
If Current = 0 Then GetPREV = Max: Exit Function
GetPREV = Current - 1
End Function

Public Function OnPerCent(Max, PER)
Dim k As Single
k = Max / 100
OnPerCent = PER * k
End Function

Public Function OnPerCentFAST(Max As Integer, PER As Integer) As Integer
On Error Resume Next
Dim k As Single
k = Max / 100
OnPerCentFAST = PER * k
End Function

Public Function ToBYTES(ST As String, ms() As Byte) As Long
Dim i As Long, sLen As Long, k As String
sLen = Len(ST)
ReDim ms(1 To sLen) As Byte
CopyMemory ms(1), ByVal ST, sLen
ToBYTES = sLen
End Function

Public Function ToSTRING(ST As String, ms() As Byte) As Long
Dim i As Long, sLen As Long
sLen = UBound(ms)
ST = Space$(sLen)
CopyMemory ByVal ST, ms(1), sLen
ToSTRING = sLen
End Function

Public Function ToSTringNZ(ST As String, ms() As Byte) As Long
Dim i As Long, sLen As Long
sLen = UBound(ms)
For i = 1 To sLen
  If Not Chr(ms(i)) = 0 Then ST = ST + Chr(ms(i))
Next i
ToSTringNZ = sLen
End Function

Public Function VBCopyFile(fSource As String, fDest As String, buffer As Long, PB As PictureBox, Optional LaB As Label)
On Error Resume Next
Dim ms() As Byte, lG As Long, FSize As Long, T As Long, NL As Byte
Dim F1 As Long, F2 As Long
ReDim ms(1 To buffer) As Byte
lG = 1
F1 = FreeFile
Open fSource For Binary Access Read As #F1
  F2 = FreeFile
  Open fDest For Binary Access Write As #F2
    FSize = LOF(1)
    Seek #F2, FSize
    Seek #F2, 1
    Do
      If lG + buffer < FSize Then
        Get #F1, lG, ms()
        Put #F2, lG, ms()
        lG = lG + buffer
      Else
        T = FSize - (lG - 1)
        ReDim ms(1 To T) As Byte
        Get #F1, lG, ms()
        Put #F2, lG, ms()
        lG = FSize
        Exit Do
      End If
      If lG >= FSize Then Exit Do
      ProgB PB, FSize, 1, lG
      DoEvents
    Loop
  Close #F2
Close #F1
End Function

Public Function EncodePassword(pW As String, code As Byte, ToBYTES() As Integer) As Integer
Dim i As Integer, sLen As Integer
sLen = Len(pW)
ReDim ToBYTES(1 To sLen) As Integer
For i = 1 To sLen
  ToBYTES(i) = (255 - Asc(Mid$(pW, i, 1))) * code
Next i
EncodePassword = sLen
End Function

Public Function DecodePassword(code As Byte, FBYTES() As Integer) As String
Dim i As Integer, sLen As Integer, k As String
sLen = UBound(FBYTES)
For i = 1 To sLen
  k = k & Chr(255 - (FBYTES(i) / code))
Next i
DecodePassword = k
End Function

Public Function GetKey(Key As Long) As Long
Dim h As Long
h = GetKeyState(Key)
If h < -2 Then GetKey = 1 Else GetKey = 0
End Function

Public Function GetKeyUP(Key As Long) As Long
Dim h As Long
h = GetKeyState(Key)
If h < -2 Then
  Do
    h = GetKeyState(Key)
    DoEvents
  Loop Until h > -2
  GetKeyUP = 1
Else
  GetKeyUP = 0
End If
End Function

Public Function GetTheMax(ByVal V1 As Integer, ByVal V2 As Integer) As Integer
If V1 > V2 Then GetTheMax = V1 Else GetTheMax = V2
End Function

Public Function GetTheMaxV(V1, V2)
If V1 > V2 Then GetTheMaxV = V1 Else GetTheMaxV = V2
End Function

Public Function GetTheMaxL(ByVal V1 As Long, ByVal V2 As Long) As Long
If V1 > V2 Then GetTheMaxL = V1 Else GetTheMaxL = V2
End Function

Public Function GetTheMin(ByVal V1 As Integer, ByVal V2 As Integer) As Integer
If V1 < V2 Then GetTheMin = V1 Else GetTheMin = V2
End Function

Public Function GetTheMinV(V1, V2)
If V1 < V2 Then GetTheMinV = V1 Else GetTheMinV = V2
End Function

Public Function GetTheMinL(ByVal V1 As Long, ByVal V2 As Long) As Long
If V1 < V2 Then GetTheMinL = V1 Else GetTheMinL = V2
End Function

Public Sub FileIconEx(sFilePath As String, uFlags As Long, objPB As PictureBox, x As Long, y As Long)
On Error Resume Next
Dim shfi As SHFILEINFO
objPB.Cls
SHGetFileInfo ByVal sFilePath, 0&, shfi, Len(shfi), &H100& Or uFlags
If uFlags And 1 Then
  DrawIconEx objPB.hdc, x, y, shfi.hIcon, 16, 16, 0, 0, &H3
Else
  DrawIconEx objPB.hdc, x, y, shfi.hIcon, 32, 32, 0, 0, &H3
End If
objPB.Refresh
DestroyIcon shfi.hIcon
End Sub

Public Function FileIconH(sFilePath As String, uFlags As Long) As Long
On Error Resume Next
Dim shfi As SHFILEINFO
SHGetFileInfo ByVal sFilePath, 0&, shfi, Len(shfi), &H100& Or uFlags

FileIconH = shfi.hIcon
End Function

Public Function GetFileType(FN As String) As String
Dim shfi As SHFILEINFO, d As Long, k(1 To 4) As Byte, P(1 To 2) As Integer
Dim S As String, i As Byte
d = SHGetFileInfo(ByVal FN, 0, shfi, Len(shfi), &H2000)

GetFileType = "ERROR"
If d = 0 Then Exit Function

CopyMemory k(1), d, 4

For i = 1 To 2
  S = S + Chr(k(i))
Next
If S = "MZ" Then GetFileType = "EXE_DOS": Exit Function
CopyMemory P(1), k(1), 4
If S = "PE" And k(2) = 0 Then GetFileType = "EXE_CONSOLE": Exit Function
If (S = "PE" Or S = "NE") Then GetFileType = "EXE_WIN": Exit Function
End Function

Public Sub FileIconPP(sFilePath As String, PB As Object, TP As PictureBox, Optional x As Long, Optional y As Long)
On Error Resume Next
Dim shfi As SHFILEINFO
objPB.Cls
SHGetFileInfo ByVal sFilePath, 0&, shfi, Len(shfi), &H100& Or uFlags

DrawIconEx TP.hdc, 0, 0, shfi.hIcon, 32, 32, 0, 0, &H3

PB.PaintPicture TP.Image, x, y, 16, 16, 0, 0, 32, 32

PB.Refresh
DestroyIcon shfi.hIcon
End Sub

Public Function FileIconPic(sFilePath As String) As IPicture
On Error Resume Next
Dim shfi As SHFILEINFO, g As Guid, PD As PictDesc
SHGetFileInfo ByVal sFilePath, 0&, shfi, Len(shfi), &H100& Or &H0

With g
  .Data1 = &H20400
  .Data4(0) = &HC0
  .Data4(7) = &H46
End With

With PD
  .cbSizeofStruct = Len(PDesc)
  .picType = 3
  .hImage = shfi.hIcon
End With

OleCreatePictureIndirect PD, g, 1, FileIconPic

DestroyIcon shfi.hIcon
End Function

Public Function FileIconHandle(sFilePath As String) As Long
On Error Resume Next
Dim shfi As SHFILEINFO, NewIcon As Long, ret As Long
ret = SHGetFileInfo(ByVal sFilePath, ByVal 0, shfi, Len(shfi), &H100& Or &H0)
NewIcon = CopyIcon(shfi.hIcon)
DestroyIcon shfi.hIcon
FileIconHandle = NewIcon
End Function

Public Sub EndTask(hWnd As Long)
Dim x As Long, y As Long
GetWindowThreadProcessId hWnd, y
x = OpenProcess(&H1, 1, y)
TerminateProcess x, 0
End Sub

Public Sub EndTask2(hWnd As Long)
Dim x As Long, y As Long
GetWindowThreadProcessId hWnd, y
x = OpenProcess(&H1, 1, y)
TerminateProcess x, 0
End Sub

Public Sub KillTask(ProcessID As Long)
Dim x As Long
x = OpenProcess(&H1, 1, ProcessID)
TerminateProcess x, 0
End Sub

Public Sub FillDC(hWnd As Long, hdc As Long, Color As Long)
Dim hBr As Long, r As RECT, P As POINTAPI
GetWindowRect hWnd, r
ClientToScreen hWnd, P
r.Left = P.x
r.Top = P.y
hBr = CreateSolidBrush(Color)
FillRect hdc, r, hBr
End Sub

Public Sub DrawLineAPI(DC As Long, X1 As Long, Y1 As Long, X2 As Long, Y2 As Long)
Dim A As POINTAPI
MoveToEx DC, X1, Y1, A
LineTo DC, X2, Y2
End Sub

Public Function InvN(Value)
InvN = Abs(CLng(Value) - 1)
End Function

Public Function GetSign(Value As Long) As Long
If Value < 0 Then GetSign = -1 Else GetSign = 1
End Function

Public Function GetSign2(Value As Long) As Long
If Value < 0 Then
  GetSign2 = -1
ElseIf Value > 0 Then
  GetSign2 = 1
Else
  GetSign2 = 0
End If
End Function

Public Function PasswordEncode(Pass As String) As String
Dim IC As Long, i As Long, S As String
IC = Len(Pass)
For i = 1 To IC
  S = S + Chr(255 - Asc(Mid$(Pass, i, 1))) + Chr(Rnd * 255)
Next i
PasswordEncode = S
End Function

Public Function PasswordDecode(Pass As String) As String
Dim IC As Long, i As Long, S As String
IC = Len(Pass)
For i = 1 To IC Step 2
  S = S + Chr(255 - Asc(Mid$(Pass, i, 1)))
Next i
PasswordDecode = S
End Function

Public Function FillGet(INTERVAL As Long, Optional i As Long = 0) As Byte
If timeGetTime - TFill(i) >= INTERVAL Then TFill(i) = timeGetTime: FillGet = 1 Else FillGet = 0
If timeGetTime < TFill(i) Then TFill(i) = timeGetTime
End Function

Public Sub wait(INTERVAL As Long)
Dim L As Long
L = timeGetTime
Do
  DoEvents
Loop Until timeGetTime - L > INTERVAL
End Sub

Public Sub mKill(sFile As String)
On Error Resume Next
SetAttr sFile, vbNormal
Kill sFile
End Sub

Public Function GetTempDir() As String
Dim S As String, A As Long
S = String(2000, " ")
GetTempPath 2000, S
A = InStr(1, S, Chr(0))
If A = 0 Then GetTempDir = DirFilterIN(S) & "\" Else GetTempDir = DirFilterIN(Mid$(S, 1, A - 1)) & "\"
End Function

Public Function WindowUnderCursor() As Long
Dim P As POINTAPI
GetCursorPos P
WindowUnderCursor = WindowFromPoint(P.x, P.y)
End Function

Public Sub ArrayToArrayByte(Arr1() As Byte, Arr2() As Byte)
Dim LB As Long, UB As Long, aLen As Long
UB = UBound(Arr1)
LB = LBound(Arr1)
aLen = (UB - LB) + 1

ReDim Arr2(LB To UB) As Byte
CopyMemory Arr2(LB), Arr1(LB), aLen
End Sub

Public Function IfMoreThenZero(Value)
If Value > 0 Then IfMoreThenZero = Value Else IfMoreThenZero = 0
End Function

Public Function IsPointInRect(x As Long, y As Long, RX1 As Long, RY1 As Long, RX2 As Long, RY2 As Long) As Byte
If x > RX1 Then
  If x < RX2 Then
    If y > RY1 Then
      If y < RY2 Then
        IsPointInRect = 1
      End If
    End If
  End If
End If
End Function

Public Function IsPointInRectWH(x As Long, y As Long, RX1 As Long, RY1 As Long, WWW As Long, HHH As Long) As Byte
Dim RY2 As Long, RX2 As Long

RX2 = RX1 + WWW
RY2 = RY1 + HHH

If x > RX1 Then
  If x < RX2 Then
    If y > RY1 Then
      If y < RY2 Then
        IsPointInRectWH = 1
      End If
    End If
  End If
End If
End Function

Public Sub BeginMeter()
BeginTime = timeGetTime
End Sub

Public Function EndMeter() As Long
EndMeter = timeGetTime - BeginTime
End Function

Public Function GetFilesCount(ByVal folder As String, Ext As String) As Long
Dim S As String
folder = DirFilterIN(folder) + "\*." + Ext
S = Dir(folder, vbArchive Or vbHidden Or vbNormal Or vbReadOnly Or vbSystem)
Do Until S = ""
  GetFilesCount = GetFilesCount + 1
  S = Dir
Loop
End Function

Public Function GetNumberFileName(ByVal folder As String, Ext As String, Num As Long) As String
Dim S As String, i As Long
folder = DirFilterIN(folder) + "\*." + Ext
S = Dir(folder, vbArchive Or vbHidden Or vbNormal Or vbReadOnly Or vbSystem)
Do Until S = ""
  i = i + 1
  If i = Num Then GetNumberFileName = S
  S = Dir
Loop
End Function

Public Function TrayAdd(TrayObj As PictureBox, Optional ToolTip As String, Optional Icon As Long) As Long
With nfIconData
    .hWnd = TrayObj.hWnd
    .uId = 0
    .uFlags = Icon_Flags.Icon Or Icon_Flags.message Or Icon_Flags.Tip
    .uCallbackMessage = WM_MOUSEMOVE
    .hIcon = TrayObj.Picture.handle
    If Not Icon = 0 Then .hIcon = Icon
    ' .hIcon = TrayObj.Image.Handle
    .szTip = ToolTip & vbNullChar
    .cbSize = Len(nfIconData)
End With
TrayAdd = Shell_NotifyIcon(NIM_ADD, nfIconData)
End Function

Public Sub TrayModify(TrayObj As PictureBox, Optional ToolTip As String, Optional Icon As Long)
With nfIconData
    .hWnd = TrayObj.hWnd
    .uFlags = Icon_Flags.Icon Or Icon_Flags.message Or Icon_Flags.Tip
    .uCallbackMessage = WM_MOUSEMOVE
    .hIcon = TrayObj.Picture.handle
    If Not Icon = 0 Then .hIcon = Icon
    ' .hIcon = TrayObj.Image.Handle
    .szTip = ToolTip & vbNullChar
    .cbSize = Len(nfIconData)
End With
If Shell_NotifyIcon(Action.MODIFY, nfIconData) = 0 Then
  Shell_NotifyIcon Action.Add, nfIconData
End If
End Sub

Public Sub TrayBalloon(TrayObj As PictureBox, strTitle As String, IconType As Long, strMessage As String)

   With nidBalloon
      .cbSize = Len(nidBalloon)
      .hWnd = TrayObj.hWnd
  '    .uId = App.hInstance
      .uFlags = Icon_Flags.Info
      .dwInfoFlags = IconType
      .szInfoTitle = strTitle + Chr(0)
      .szInfo = strMessage + Chr(0)
   End With

   Call Shell_NotifyIcon(Action.MODIFY, nidBalloon)
   
End Sub

Public Sub TrayRemove()
Call Shell_NotifyIcon(Action.Delete, nfIconData)
End Sub

Public Sub DeleteBalloon(TrayObj As PictureBox)
   Call Shell_NotifyIcon(Action.Delete, nidBalloon)
End Sub

Public Function mRND(ToNumber As Long) As Long
mRND = timeGetTime Mod ToNumber
End Function

Public Function GetPAD(N, To1 As String, To4 As String, To0 As String) As String
Dim M As String, k As Long, d As Long
M = Mid$(Format$(N), Len(Format$(N)), 1)
If Len(Format$(N)) > 1 Then
  d = Val(Mid$(Format$(N), Len(Format$(N)) - 1, 2))
Else
  d = N
End If
k = Val(M)
If d > 10 And d < 20 Then k = 0
Select Case k
  Case 1: GetPAD = To1
  Case 2, 3, 4: GetPAD = To4
  Case Else: GetPAD = To0
End Select
End Function

Public Function ToPoint(ByVal S As String) As String
Dim i As Long, IC As String
IC = Len(S)
For i = 1 To IC
  If Mid$(S, i, 1) = "," Then Mid$(S, i, 1) = "."
Next
ToPoint = S
End Function

Public Sub GetPictureWH(handle As Long, Width As Long, Height As Long, Optional bits As Long)
Dim BM As BITMAP
GetObjectAPI handle, Len(BM), BM
Width = BM.bmWidth
Height = BM.bmHeight
bits = BM.bmBitsPixel
End Sub

Public Function GetPictureBitCount(handle As Long) As Long
Dim BM As BITMAP
GetObjectAPI handle, Len(BM), BM
GetPictureBitCount = BM.bmBitsPixel
End Function

Public Function LoadTextFile(FileName As String) As String
Dim f As Long
f = FreeFile
Open FileName For Input As f
  LoadTextFile = Input$(LOF(f), f)
Close f
End Function

Public Function LoadFile(FileName As String) As String
Dim f As Long, B() As Byte, IC As Long
f = FreeFile
If CheckFile(FileName) <> 1 Then Exit Function
Open FileName For Binary As f
  IC = LOF(f)
  If Not IC = 0 Then
    ReDim B(1 To IC) As Byte
    Get #f, 1, B()
    LoadFile = String(IC, " ")
    CopyMemory ByVal LoadFile, B(1), IC
  End If
Close f
End Function

Public Function LoadFileB(FileName As String, ToArr() As Byte, Optional Max As Long = -1) As Long
Dim f As Long, IC As Long
f = FreeFile
Open FileName For Binary As f
  IC = LOF(f)
  If IC > Max And Max >= 0 Then IC = Max
  If Not IC = 0 Then
    ReDim ToArr(1 To IC) As Byte
    Get #f, 1, ToArr()
    LoadFileB = IC
  End If
Close f
End Function

Public Sub SaveFile(FileName As String, Text As String)
Dim f As Long, ms() As Byte, IC As Long
f = FreeFile
Open FileName For Output As f
Close f
Open FileName For Binary Access Write As f
  If f = FreeFile Then Exit Sub
  IC = Len(Text)
  If Not IC = 0 Then
    ReDim ms(1 To IC) As Byte
    CopyMemory ms(1), ByVal Text, IC
    Put #f, 1, ms()
  End If
Close f
End Sub

Public Sub SaveFileB(FileName As String, Arr() As Byte)
On Error Resume Next
Dim f As Long, IC As Long
f = FreeFile
Open FileName For Output As f
Close f
Open FileName For Binary Access Write As f
  If f = FreeFile Then Exit Sub
  IC = UBound(Arr)
  If Not IC = 0 Then
    Put #f, 1, Arr()
  End If
Close f
End Sub

Public Sub SaveControl(ctrl As Control, Optional Section As String = "SavedC")
On Error Resume Next
Dim i As Long
i = ctrl.index

SaveSetting App.EXEName, Section, ctrl.Name & i & " value", ctrl.Value
SaveSetting App.EXEName, Section, ctrl.Name & i & " caption", ctrl.Caption
SaveSetting App.EXEName, Section, ctrl.Name & i & " text", ctrl.Text
SaveSetting App.EXEName, Section, ctrl.Name & i & " backcolor", ctrl.BackColor
SaveSetting App.EXEName, Section, ctrl.Name & i & " forecolor", ctrl.ForeColor
SaveSetting App.EXEName, Section, ctrl.Name & i & " listindex", ctrl.ListIndex
SaveSetting App.EXEName, Section, ctrl.Name & i & " checked", ctrl.Checked
End Sub

Public Sub LoadControl(ctrl As Control, Optional Section As String = "SavedC")
On Error Resume Next
Dim i As Long
i = ctrl.index

ctrl.Value = Val(GetSetting(App.EXEName, Section, ctrl.Name & i & " value", ctrl.Value))
ctrl.ListIndex = Val(GetSetting(App.EXEName, Section, ctrl.Name & i & " listindex", ctrl.ListIndex))
ctrl.Caption = GetSetting(App.EXEName, Section, ctrl.Name & i & " caption", ctrl.Caption)
ctrl.Text = GetSetting(App.EXEName, Section, ctrl.Name & i & " text", ctrl.Text)
ctrl.BackColor = Val(GetSetting(App.EXEName, Section, ctrl.Name & i & " backcolor", ctrl.BackColor))
ctrl.ForeColor = Val(GetSetting(App.EXEName, Section, ctrl.Name & i & " forecolor", ctrl.ForeColor))
ctrl.Checked = CBool(GetSetting(App.EXEName, Section, ctrl.Name & i & " checked", ctrl.Checked))
    Exit Sub
End Sub

Public Sub LoadControlR(ctrl As Control, Optional Section As String = "SavedC", Optional NoText As Boolean)
On Error Resume Next
Dim i As Long
i = ctrl.index

ctrl.Value = CBool(GetSetting(App.EXEName, Section, ctrl.Name & i & " value", ctrl.Value))
ctrl.ListIndex = Val(GetSetting(App.EXEName, Section, ctrl.Name & i & " listindex", ctrl.ListIndex))
If Not NoText Then ctrl.Caption = GetSetting(App.EXEName, Section, ctrl.Name & i & " caption", ctrl.Caption)
If Not NoText Then ctrl.Text = GetSetting(App.EXEName, Section, ctrl.Name & i & " text", ctrl.Text)
ctrl.BackColor = Val(GetSetting(App.EXEName, Section, ctrl.Name & i & " backcolor", ctrl.BackColor))
ctrl.ForeColor = Val(GetSetting(App.EXEName, Section, ctrl.Name & i & " forecolor", ctrl.ForeColor))
End Sub


Public Function PictureFromImage(picImage As IPictureDisp) As StdPicture
Dim TP As String
TP = DirFilterIN(GetTempDir) + "\tmp0456.bmp"
SavePicture picImage, TP
Set PictureFromImage = LoadPicture(TP)
mKill TP
End Function

Public Function RGBWidth(Width As Long) As Long
RGBWidth = (Width * 3 + 3) And &HFFFFFFFC
End Function

Public Function MaskWidth(W As Long) As Long
MaskWidth = ((((W + 7) \ 8) + 3) And &HFFFFFFFC)
End Function

Public Function Username() As String
On Error Resume Next
Dim S As String * 1000, A As Long
GetUserName S, Len(S)
A = InStr(1, S, Chr(0))
If Not A = 0 Then Username = Mid$(S, 1, A - 1) Else Username = S
End Function

Public Sub ShowErrorMsg(Msg As String)
MsgBox Msg, vbCritical, "Ошибка"
End Sub

Public Function fDiv(ByVal V1, ByVal V2)
Dim A As Long, B As Long

A = 389 \ 17
A = CLng(V1) \ CLng(V2)
B = CLng(V1) Mod CLng(V2)
If B > 0 Then A = A + 1
fDiv = A
End Function

Public Function FPSMS(FPS As Long) As Long
FPSMS = 1000 / FPS
End Function

Public Function DownUp(OldV, NewV, MinS As String, MaxS As String, RavS As String) As String
If OldV > NewV Then
  DownUp = MinS
ElseIf OldV < NewV Then
  DownUp = MaxS
Else
  DownUp = RavS
End If
End Function

Public Sub DrawRNDLineH(DC As Long, y As Long, Width As Long, Optional MaxRND As Long = -1)
Dim i As Long, Py As Long

Py = y

For i = 0 To Width
  SetPixelV DC, i, Py, 0
  Py = Py + (Rnd * 2) - 1
  If Py > y + MaxRND And Not MaxRND = -1 Then
    Py = Py - 1
  ElseIf Py < y - MaxRND And Not MaxRND = -1 Then
    Py = Py + 1
  End If
Next
End Sub

Public Sub DrawRNDLineV(DC As Long, x As Long, Height As Long, Optional MaxRND As Long = -1)
Dim i As Long, Px As Long

Px = x

For i = 0 To Height
  SetPixelV DC, Px, i, 0
  Px = Px + (Rnd * 2) - 1
  If Px > x + MaxRND And Not MaxRND = -1 Then
    Px = Px - 1
  ElseIf Px < x - MaxRND And Not MaxRND = -1 Then
    Px = Px + 1
  End If
Next
End Sub

Public Sub FillPB(PB As PictureBox, x As Long, y As Long, Color As Long)
Dim OldFillStyle As Long, OldFillColor As Long

OldFillStyle = PB.FillStyle
OldFillColor = PB.FillColor
PB.FillStyle = 0
PB.FillColor = Color

ExtFloodFill PB.hdc, x, y, PB.Point(x, y), 1

PB.FillStyle = OldFillStyle
PB.FillColor = OldFillColor
End Sub

Public Function CheckByValue(V1, V2, Optional Value = 1) As Boolean
CheckByValue = False
If V1 + Value >= V2 Then
  If V1 - Value <= V2 Then
    CheckByValue = True
  End If
End If
End Function

Public Sub NeedSleep(ms As Long)
Dim L As Long
L = timeGetTime - BeginTime
If ms > L And L > 0 Then 'Sleep MS - L
  Do Until (EndMeter >= ms) Or (EndMeter < 0)
  Loop
End If
End Sub

Public Function GetWindowIcon(hWnd As Long, Optional iType As Long = 1) As Long
GetWindowIcon = DefWindowProc(hWnd, &H7F, iType, 0)
End Function

Public Function SetWindowIcon(hWnd As Long, hIcon As Long, Optional iType As Long = 0) As Long
SetWindowIcon = DefWindowProc(hWnd, &H80, iType, hIcon)
End Function

Public Function NoZeroS(Text As String) As String
Dim A As Long
A = InStr(1, Text, Chr$(0))
If A = 0 Then
  NoZeroS = Text
ElseIf A = 1 Then
  NoZeroS = ""
Else
  NoZeroS = Mid$(Text, 1, A - 1)
End If
End Function

Public Function NoLastS(Text As String) As String
Dim i As Long, IC As Long

IC = Len(Text)
For i = IC To 1 Step -1
  If Not Mid$(Text, i, 1) = " " Then NoLastS = Mid$(Text, 1, i): Exit Function
Next

NoLastS = ""
Exit Function
End Function

Public Function IsNothing(Obj As Object) As Boolean
Dim A As Long
CopyMemory A, Obj, 4
If A = 0 Then IsNothing = True Else IsNothing = False
End Function

Public Function ObjectPtr(Obj As Object) As Long
Dim A As Long
CopyMemory A, Obj, 4
ObjectPtr = A
End Function

Public Sub mRefresh(Obj As Object)
Dim DC As Long, r As RECT

DC = GetDC(Obj.hWnd)
GetClientRect hWnd, r
BitBlt DC, 0, 0, r.Right, r.Bottom, Obj.hdc, 0, 0, vbSrcCopy
End Sub

Public Sub mDoEvents2(hWnd As Long)
Dim M As Msg

If GetMessage(M, hWnd, 0, 0) Then
  TranslateMessage M
  DispatchMessage M
End If
End Sub

'Public Sub DoEvents2(hwnd As Long)
'Dim M As Msg
'
'If GetMessage(M, hwnd, 0, 0) Then
'  TranslateMessage M
'  DispatchMessage M
'End If

'End Sub
Public Sub mDoEvents(hWnd As Long)
Dim M As Msg
If Not PeekMessage(M, hWnd, 0, &H500, &H0) = 0 Then DoEvents
End Sub

Public Sub Inc(ByRef Value, ByValue)
Value = Value + ByValue
End Sub

Public Sub mGet(vbFile As Long, ms() As Byte, ByVal Count As Long, Optional BytesRead As Long)
Dim M() As Byte, FR As Long, TP As Long
FR = (LOF(vbFile) - Seek(vbFile)) - 1
If Count > FR Then Count = FR
TP = Seek(vbFile)
ReDim M(1 To Count) As Byte
Get vbFile, , M()
Seek vbFile, TP + Count
CopyMemory ms(LBound(ms)), M(1), Count
BytesRead = Count
End Sub

Public Sub mPut(vbFile As Long, ms() As Byte, Count As Long)
Dim M() As Byte, TP As Long
ReDim M(1 To Count) As Byte
CopyMemory M(1), ms(LBound(ms)), Count
TP = Seek(vbFile)
Put vbFile, , M()
Seek vbFile, TP + Count
End Sub

Public Sub PErase(PB As PictureBox, Optional WithoutPicture As Byte)
PB.Line (0, 0)-(PB.ScaleWidth, PB.ScaleHeight), PB.BackColor, BF
PB.CurrentX = 0
PB.CurrentY = 0
If Not PB.Picture.handle = 0 And WithoutPicture = 0 Then PB.PaintPicture PB.Picture, 0, 0
End Sub

Public Sub PEraseObj(PB As Object, Optional WithoutPicture As Byte)
PB.Line (0, 0)-(PB.ScaleWidth, PB.ScaleHeight), PB.BackColor, BF
PB.CurrentX = 0
PB.CurrentY = 0
If Not PB.Picture.handle = 0 And WithoutPicture = 0 Then PB.PaintPicture PB.Picture, 0, 0
End Sub

Public Function ToLong(RGB As pRGB) As Long
CopyMemory ToLong, RGB, 4
End Function

Public Sub FixFloat(S As String)
S = Replace(S, ",", ".", , , vbTextCompare)
End Sub

Public Function FixFloatF(S As String) As String
FixFloatF = Replace(S, ",", ".", , , vbTextCompare)
End Function

Public Sub ReDimStr(S As String, Size As Long)
If Size <= 1 Then Exit Sub
S = String$(Size, " ")
End Sub

Public Function FileInStr(vbFile As Long, Text As String, Optional ByVal StartPos As Long = 1, Optional noCase As Byte = 0, Optional BufferSize As Long = 512) As Long
Dim M() As Byte, S As String, TL As Long, r As Long
Dim i As Long, IC As Long, sz As Long, BST As Long

If StartPos < 1 Then StartPos = 1

TL = Len(Text)
IC = LOF(vbFile)
BST = BufferSize - TL
For i = StartPos To IC Step BST
  sz = IC - (i - 1)
  If sz > BufferSize Then sz = BufferSize
  If Not Len(S) = sz Then
    ReDim M(1 To sz) As Byte
    ReDimStr S, sz
  End If
  
  Get vbFile, i, M()
  CopyMemory ByVal S, M(1), sz
  
  If noCase = 0 Then
    r = InStr(1, S, Text, vbBinaryCompare)
  Else
    r = InStr(1, S, Text, vbTextCompare)
  End If
  If Not r = 0 Then FileInStr = (i - 1) + r: Exit Function
Next
FileInStr = -1
End Function



Public Sub GradV2(PB As PictureBox, x As Long, y As Long, W As Long, h As Long, Color1 As Long, Color2 As Long)
Dim R1 As Single, G1 As Single, B1 As Single
Dim R2 As Single, G2 As Single, b2 As Single

Dim i As Long, IC As Long, RStep As Single, GStep As Single, BStep As Single

GetRGB Color1, R1, G1, B1
GetRGB Color2, R2, G2, b2

IC = h \ 2
RStep = (R2 - R1) / CSng(IC)
GStep = (G2 - G1) / CSng(IC)
BStep = (b2 - B1) / CSng(IC)
For i = 0 To IC
  PB.Line (x, y + i)-(x + W, y + i), RGB(R1, G1, B1)
  R1 = R1 + RStep
  G1 = G1 + GStep
  B1 = B1 + BStep
Next


GetRGB Color2, R1, G1, B1
GetRGB Color1, R2, G2, b2

RStep = (R2 - R1) / CSng(IC)
GStep = (G2 - G1) / CSng(IC)
BStep = (b2 - B1) / CSng(IC)
For i = 0 To IC
  PB.Line (x, y + i + IC)-(x + W, y + i + IC), RGB(R1, G1, B1)
  R1 = R1 + RStep
  G1 = G1 + GStep
  B1 = B1 + BStep
Next
End Sub

Public Function PBBitmapInfo(PB As PictureBox) As BITMAPINFO
PBBitmapInfo.bmiHeader.biSize = 40
PBBitmapInfo.bmiHeader.biWidth = PB.ScaleWidth
PBBitmapInfo.bmiHeader.biHeight = PB.ScaleHeight
PBBitmapInfo.bmiHeader.biPlanes = 1
PBBitmapInfo.bmiHeader.biBitCount = 24
PBBitmapInfo.bmiHeader.biCompression = 0
PBBitmapInfo.bmiHeader.biSizeImage = 0
End Function

Public Function ArrayDataPointer(ArrayPointer As Long) As Long
Dim U2 As Long, U3 As Long, tSA As SAFEARRAY

U2 = VarPtr(tSA)
CopyMemory U3, ByVal ArrayPointer, 4
CopyMemory ByVal U2, ByVal U3, Len(tSA)

ArrayDataPointer = tSA.pvData
End Function


Public Sub vbBitBltTransparent(DestPB As PictureBox, DestX As Long, DestY As Long, Width As Long, Height As Long, SrcPB As PictureBox, SrcX As Long, SrcY As Long, ByVal alpha As Single)
Dim i As Long, j As Long, CR As nRGB, CD As nRGB
Dim r As Integer, g As Integer, B As Integer, HA As Single, clr As Long

If Not DestPB.ScaleMode = 3 Then DestPB.ScaleMode = 3
If Not SrcPB.ScaleMode = 3 Then SrcPB.ScaleMode = 3

Dim DestDC As Long, DestBitmap As Long, SrcDC As Long, SrcBitmap As Long
Dim DestBI As BITMAPINFO, SrcBI As BITMAPINFO, dH As Long, Sh As Long, dw As Long
Dim sArr() As nRGB, dArr() As nRGB, T1 As Long, T2 As Long

ReDim sArr(0 To SrcPB.ScaleWidth) As nRGB, dArr(0 To DestPB.ScaleWidth) As nRGB

dH = DestPB.ScaleHeight - 1
dw = DestPB.ScaleWidth
Sh = SrcPB.ScaleHeight - 1
DestDC = DestPB.hdc
DestBitmap = DestPB.Image.handle
SrcDC = SrcPB.hdc
SrcBitmap = SrcPB.Image.handle
DestBI = PBBitmapInfo(DestPB)
SrcBI = PBBitmapInfo(SrcPB)

If alpha < 0 Then alpha = 0
If alpha > 1 Then alpha = 1
HA = 1 - alpha

For j = 0 To Height
  GetDIBits SrcDC, SrcBitmap, Sh - (j + SrcY), 1, ByVal ArrayDataPointer(VarPtrArray(sArr())), SrcBI, 0
  GetDIBits DestDC, DestBitmap, dH - (j + DestY), 1, ByVal ArrayDataPointer(VarPtrArray(dArr())), DestBI, 0
  For i = 0 To Width
    T1 = DestX + i
    If Not T1 > dw And Not T1 < 0 Then
      T2 = SrcX + i
      dArr(T1).r = sArr(T2).r * HA + dArr(T1).r * alpha
      dArr(T1).g = sArr(T2).g * HA + dArr(T1).g * alpha
      dArr(T1).B = sArr(T2).B * HA + dArr(T1).B * alpha
    End If
  Next
  SetDIBits DestDC, DestBitmap, dH - (j + DestY), 1, ByVal ArrayDataPointer(VarPtrArray(dArr())), DestBI, 0
Next
End Sub

Public Sub vbBitBltTransparentColor(DestPB As PictureBox, DestX As Long, DestY As Long, Width As Long, Height As Long, SrcPB As PictureBox, SrcX As Long, SrcY As Long, ByVal TColor As Long)
Dim i As Long, j As Long, CR As nRGB, CD As nRGB
Dim r As Integer, g As Integer, B As Integer, HA As Single, clr As Long

If Not DestPB.ScaleMode = 3 Then DestPB.ScaleMode = 3
If Not SrcPB.ScaleMode = 3 Then SrcPB.ScaleMode = 3

Dim DestDC As Long, DestBitmap As Long, SrcDC As Long, SrcBitmap As Long
Dim DestBI As BITMAPINFO, SrcBI As BITMAPINFO, dH As Long, Sh As Long, dw As Long
Dim sArr() As nRGB, dArr() As nRGB, T1 As Long, T2 As Long

ReDim sArr(0 To SrcPB.ScaleWidth) As nRGB, dArr(0 To DestPB.ScaleWidth) As nRGB

dH = DestPB.ScaleHeight - 1
dw = DestPB.ScaleWidth
Sh = SrcPB.ScaleHeight - 1
DestDC = DestPB.hdc
DestBitmap = DestPB.Image.handle
SrcDC = SrcPB.hdc
SrcBitmap = SrcPB.Image.handle
DestBI = PBBitmapInfo(DestPB)
SrcBI = PBBitmapInfo(SrcPB)

GetRGB TColor, r, g, B

For j = 0 To Height
  GetDIBits SrcDC, SrcBitmap, Sh - (j + SrcY), 1, ByVal ArrayDataPointer(VarPtrArray(sArr())), SrcBI, 0
  GetDIBits DestDC, DestBitmap, dH - (j + DestY), 1, ByVal ArrayDataPointer(VarPtrArray(dArr())), DestBI, 0
  For i = 0 To Width
    T1 = DestX + i
    If Not T1 > dw And Not T1 < 0 Then
      T2 = SrcX + i
      If (Not sArr(T2).r = r) Or (Not sArr(T2).g = g) Or (Not sArr(T2).B = B) Then
        dArr(T1) = sArr(T2)
      End If
    End If
  Next
  SetDIBits DestDC, DestBitmap, dH - (j + DestY), 1, ByVal ArrayDataPointer(VarPtrArray(dArr())), DestBI, 0
Next
End Sub

Public Sub StringPut(f As Long, Optional Pos As Long = -1, Optional PutString As String)
Dim L As Long, B() As Byte

L = Len(PutString)
If Not L = 0 Then ReDim B(1 To L) As Byte: CopyMemory B(1), ByVal PutString, L

If Pos > 0 Then
  Put f, , L
  If Not L = 0 Then Put f, , B()
Else
  Put f, Pos, L
  If Not L = 0 Then Put f, Pos + 4, B()
End If
End Sub

Public Sub StringGet(f As Long, Optional Pos As Long = -1, Optional GetString As String)
Dim L As Long, B() As Byte

GetString = ""

If Pos > 0 Then
  Get f, , L
  If Not L = 0 Then
    ReDim B(1 To L) As Byte
    Get f, , B()
    GetString = String(L, " ")
    CopyMemory ByVal GetString, B(1), L
  End If
Else
  Get f, Pos, L
  If Not L = 0 Then
    ReDim B(1 To L) As Byte
    Get f, Pos + 4, B()
    GetString = String(L, " ")
    CopyMemory ByVal GetString, B(1), L
  End If
End If
End Sub

Public Function cWidth(hWnd As Long) As Long
Dim r As RECT
GetClientRect hWnd, r
cWidth = r.Right
End Function

Public Function cHeight(hWnd As Long) As Long
Dim r As RECT
GetClientRect hWnd, r
cHeight = r.Bottom
End Function

Public Function GetFileText(FileName As String) As String
Dim f As Long
f = FreeFile
Open FileName For Input As f
  
  GetFileText = Input$(LOF(f), #f)
Close f
End Function

Public Sub SetFileText(FileName As String, Text As String)
Dim f As Long
f = FreeFile
Open FileName For Output As #f
  Print #f, Text
Close #f
End Sub

Public Sub GetPictureWHEx(Pic As StdPicture, Optional Width As Long, Optional Height As Long)
Dim BM As BITMAP, Icon As ICONINFO, hWnd As Long
If Pic.Type = 1 Then
  hWnd = Pic.handle
ElseIf Pic.Type = 3 Then
  GetIconInfo Pic.handle, Icon
  hWnd = Icon.hBmColor
End If

If hWnd = 0 Then Exit Sub

GetObjectAPI hWnd, Len(BM), BM
Width = BM.bmWidth
Height = BM.bmHeight

If Pic.Type = 3 Then
  DeleteObject Icon.hBmColor
  DeleteObject Icon.hBmMask
End If
End Sub

Public Function CreateDIB(ByVal lHDC As Long, ByVal lWidth As Long, ByVal lheight As Long, BitsPointer As Long, Optional BitsPerPixel As Long = 24) As Long
Dim m_tBI As BITMAPINFO
With m_tBI.bmiHeader
  .biSize = Len(m_tBI.bmiHeader)
  .biWidth = lWidth
  .biHeight = lheight
  .biPlanes = 1
  .biBitCount = BitsPerPixel
  .biCompression = BI_RGB
  If Not bits = 1 Then
    .biSizeImage = RGBWidth(lWidth) * .biHeight
  Else
    .biSizeImage = MaskWidth(lWidth) * .biHeight
  End If
End With
CreateDIB = CreateDIBSection(lHDC, m_tBI, DIB_RGB_COLORS, BitsPointer, 0, 0)
End Function

Public Function CreatetPictureFromFile(FileName As String, Optional TColor As Long = -1) As tPicture
Dim Pic As StdPicture
Set Pic = LoadPicture(FileName)

CreatetPictureFromFile = CreatetPicture(Pic, TColor)
End Function

Public Function CopyhBmp(hBMP As Long) As Long
CopyhBmp = CopyImage(hBMP, IMAGE_BITMAP, 0, 0, &H4)
End Function

Public Function CreateEmptytPicture(Width As Long, Height As Long, Optional Dib24Bit As Byte) As tPicture
Dim MaskColor As Long, hPal As Long
Dim cBk As Long, cText As Long
Dim tDC As Long, tBitmap As Long, tBO As Long, BPS As Long

hDcScreen = CreateCompatibleDC(0) 'GetDC(0&)
hPal = CreateHalftonePalette(hDcScreen)
OleTranslateColor TColor, hPal, MaskColor

CreateEmptytPicture.Width = Width
CreateEmptytPicture.Height = Height
CreateEmptytPicture.PDC = CreateCompatibleDC(0)
If Dib24Bit = 0 Then
  BPS = GetDeviceCaps(CreateEmptytPicture.PDC, BITSPIXEL)
  CreateEmptytPicture.pBitmap = CreateBitmap(CreateEmptytPicture.Width, CreateEmptytPicture.Height, 1, BPS, ByVal 0&)
  'CreateEmptytPicture.pBitmap = CreateCompatibleBitmap(CreateEmptytPicture.PDC, CreateEmptytPicture.Width, CreateEmptytPicture.height)
  CreateEmptytPicture.OldpBitmap = SelectObject(CreateEmptytPicture.PDC, CreateEmptytPicture.pBitmap)
Else
  CreateEmptytPicture.pBitmap = CreateDIB(CreateEmptytPicture.PDC, CreateEmptytPicture.Width, CreateEmptytPicture.Height, CreateEmptytPicture.pBitmapBits, 24)
  CreateEmptytPicture.OldpBitmap = SelectObject(CreateEmptytPicture.PDC, CreateEmptytPicture.pBitmap)
End If
End Function

Public Function CreatetPictureFromIcon(hIcon As Long) As tPicture
Dim ICN As ICONINFO

GetIconInfo hIcon, ICN

CreatetPictureFromIcon.pBitmap = ICN.hBmColor
CreatetPictureFromIcon.PDC = CreateCompatibleDC(0)
CreatetPictureFromIcon.OldpBitmap = SelectObject(CreatetPictureFromIcon.PDC, CreatetPictureFromIcon.pBitmap)

CreatetPictureFromIcon.mBitmap = ICN.hBmMask
CreatetPictureFromIcon.mDC = CreateCompatibleDC(0)
CreatetPictureFromIcon.OldmBitmap = SelectObject(CreatetPictureFromIcon.mDC, CreatetPictureFromIcon.mBitmap)
End Function

Public Function CreatetPicture(Pic As StdPicture, TColor As Long, Optional Dib24Bit As Byte, Optional NoSelectToDC As Boolean) As tPicture
Dim MaskColor As Long, hPal As Long
Dim cBk As Long, cText As Long
Dim tDC As Long, tBitmap As Long, tBO As Long

hDcScreen = CreateCompatibleDC(0) 'GetDC(0&)
hPal = CreateHalftonePalette(hDcScreen)
OleTranslateColor TColor, hPal, MaskColor

'Color
GetPictureWH Pic.handle, CreatetPicture.Width, CreatetPicture.Height
CreatetPicture.PDC = CreateCompatibleDC(0)
If Dib24Bit = 0 Then
  CreatetPicture.pBitmap = CopyImage(Pic.handle, IMAGE_BITMAP, 0, 0, &H4)
  CreatetPicture.OldpBitmap = SelectObject(CreatetPicture.PDC, CreatetPicture.pBitmap)
Else
  tDC = CreateCompatibleDC(0)
  tBitmap = CopyImage(Pic.handle, IMAGE_BITMAP, 0, 0, &H4)
  tBO = SelectObject(tDC, tBitmap)
  
  CreatetPicture.pBitmap = CreateDIB(CreatetPicture.PDC, CreatetPicture.Width, CreatetPicture.Height, CreatetPicture.pBitmapBits, 24)
  CreatetPicture.OldpBitmap = SelectObject(CreatetPicture.PDC, CreatetPicture.pBitmap)
  BitBlt CreatetPicture.PDC, 0, 0, CreatetPicture.Width, CreatetPicture.Height, tDC, 0, 0, vbSrcCopy
  
  DeleteObject SelectObject(tDC, tBitmap)
  DeleteObject tBitmap
  DeleteDC tDC
End If

If NoSelectToDC Then
  DeleteObject SelectObject(CreatetPicture.PDC, CreatetPicture.OldpBitmap)
End If

'SetBkColor CreatetPicture.PDC, cBk
'SetTextColor CreatetPicture.PDC, cText
'BitBlt CreatetPicture.PDC, 0, 0, CreatetPicture.Width, CreatetPicture.height, CreatetPicture.mDC, 0, 0, &H220326

If TColor = -1 Then Exit Function
'Mask
CreatetPicture.mDC = CreateCompatibleDC(0)
CreatetPicture.mBitmap = CreateBitmap(CreatetPicture.Width, CreatetPicture.Height, 1, 1, ByVal 0&)
CreatetPicture.OldmBitmap = SelectObject(CreatetPicture.mDC, CreatetPicture.mBitmap)

cBk = GetBkColor(CreatetPicture.PDC)
cText = GetTextColor(CreatetPicture.PDC)

SetBkColor CreatetPicture.mDC, TColor
SetTextColor CreatetPicture.mDC, vbWhite
SetBkColor CreatetPicture.PDC, TColor
SetTextColor CreatetPicture.PDC, vbWhite

BitBlt CreatetPicture.mDC, 0, 0, CreatetPicture.Width, CreatetPicture.Height, CreatetPicture.PDC, 0, 0, vbSrcCopy
'

SetBkColor CreatetPicture.PDC, vbWhite
SetTextColor CreatetPicture.PDC, vbBlack
BitBlt CreatetPicture.PDC, 0, 0, CreatetPicture.Width, CreatetPicture.Height, CreatetPicture.mDC, 0, 0, &H220326

'BitBlt Form1.hdc, 100, 100, CreatetPicture.Width, CreatetPicture.Height, CreatetPicture.mDC, 0, 0, vbSrcCopy
End Function

Public Sub DrawtPicture(tPic As tPicture, ToDC As Long, x As Long, y As Long, Optional Width As Long = -1, Optional Height As Long = -1, Optional SrcX As Long, Optional SrcY As Long)
Dim cBk As Long, cText As Long
If Width = -1 Then Width = tPic.Width
If Height = -1 Then Height = tPic.Height

If tPic.mDC = 0 Then
  BitBlt ToDC, x, y, Width, Height, tPic.PDC, SrcX, SrcY, vbSrcCopy
Else
  cBk = GetBkColor(ToDC)
  cText = GetTextColor(ToDC)
  
  SetTextColor ToDC, 0 'RGB(100, 100, 100)
  SetBkColor ToDC, vbWhite
  
  BitBlt ToDC, x, y, Width, Height, tPic.mDC, SrcX, SrcY, vbSrcAnd
  BitBlt ToDC, x, y, Width, Height, tPic.PDC, SrcX, SrcY, vbSrcPaint
  
  SetBkColor ToDC, cBk
  SetTextColor ToDC, cText
End If
End Sub


Public Sub DeletetPicture(Pic As tPicture)
DeleteObject SelectObject(Pic.mDC, Pic.OldmBitmap)
'DeleteObject Pic.OldmBitmap
DeleteObject Pic.mBitmap
DeleteDC Pic.mDC

DeleteObject SelectObject(Pic.PDC, Pic.OldpBitmap)
'DeleteObject Pic.OldpBitmap
DeleteObject Pic.pBitmap
DeleteDC Pic.PDC

Pic.mDC = 0
Pic.PDC = 0
Pic.OldmBitmap = 0
Pic.OldpBitmap = 0
Pic.mBitmap = 0
Pic.pBitmap = 0
Pic.pBitmapBits = 0
Pic.Height = 0
Pic.Width = 0
End Sub

Public Function GetSubFolderFilesCount(ByVal folder As String) As Long
Dim i As Long, ArrayF() As String, ArrayC As Long, S2 As String, cDir As Long, cSize As Long, S As String
ReDim ArrayF(1 To 1) As String
ArrayC = 1: cDir = 1
ArrayF(ArrayC) = DirFilterIN(folder) + "\"

GETFOLDERS:
folder = DirFilterIN(folder) + "\"
S = Dir(folder, vbDirectory Or vbArchive Or vbHidden Or vbNormal Or vbReadOnly Or vbSystem)
Do Until S = ""
  If Not StrComp(S, ".") = 0 And Not StrComp(S, "..") = 0 Then
    S2 = folder + S
    If (GetAttr(S2) And vbDirectory) = vbDirectory Then
      ArrayC = ArrayC + 1
      ReDim Preserve ArrayF(1 To ArrayC) As String
      ArrayF(ArrayC) = S2
    Else
      GetSubFolderFilesCount = GetSubFolderFilesCount + 1
    End If
  End If
  S = Dir
Loop

If cDir < ArrayC Then
  cDir = cDir + 1
  folder = ArrayF(cDir)
  GoTo GETFOLDERS
End If
End Function

Public Function GetSubFolderFiles(ByVal folder As String, FilesArray() As String) As Long
On Error Resume Next
Dim i As Long, ArrayF() As String, ArrayC As Long, S2 As String, cDir As Long, cSize As Long, S As String
ReDim ArrayF(1 To 1) As String
ArrayC = 1: cDir = 1
ArrayF(ArrayC) = DirFilterIN(folder) + "\"

GETFOLDERS:
folder = DirFilterIN(folder) + "\"
S = Dir(folder, vbDirectory Or vbArchive Or vbHidden Or vbNormal Or vbReadOnly Or vbSystem)
Do Until S = ""
  If Not StrComp(S, ".") = 0 And Not StrComp(S, "..") = 0 Then
    S2 = folder + S
    If (GetAttr(S2) And vbDirectory) = vbDirectory Then
      ArrayC = ArrayC + 1
      ReDim Preserve ArrayF(1 To ArrayC) As String
      ArrayF(ArrayC) = S2
    Else
      GetSubFolderFiles = GetSubFolderFiles + 1
      ReDim Preserve FilesArray(1 To GetSubFolderFiles) As String
      FilesArray(GetSubFolderFiles) = S2
    End If
  End If
  S = Dir
Loop

If cDir < ArrayC Then
  cDir = cDir + 1
  folder = ArrayF(cDir)
  GoTo GETFOLDERS
End If
End Function

Public Function GetFolderFiles(ByVal folder As String, FilesArray() As String) As Long
On Error Resume Next
Dim i As Long, S2 As String, cDir As Long, cSize As Long, S As String
cDir = 1

GETFOLDERS:
folder = DirFilterIN(folder) + "\"
S = Dir(folder, vbDirectory Or vbArchive Or vbHidden Or vbNormal Or vbReadOnly Or vbSystem)
Do Until S = ""
  If Not StrComp(S, ".") = 0 And Not StrComp(S, "..") = 0 Then
    S2 = folder + S
    If (GetAttr(S2) And vbDirectory) = vbDirectory Then
    Else
      GetFolderFiles = GetFolderFiles + 1
      ReDim Preserve FilesArray(1 To GetFolderFiles) As String
      FilesArray(GetFolderFiles) = S2
    End If
  End If
  S = Dir
Loop
End Function

Public Function GetSubFolderSize(ByVal folder As String) As Long
Dim i As Long, ArrayF() As String, ArrayC As Long, S2 As String, cDir As Long, cSize As Long, S As String
ReDim ArrayF(1 To 1) As String
ArrayC = 1: cDir = 1
ArrayF(ArrayC) = DirFilterIN(folder) + "\"

GETFOLDERS:
folder = DirFilterIN(folder) + "\"
S = Dir(folder, vbDirectory Or vbArchive Or vbHidden Or vbNormal Or vbReadOnly Or vbSystem)
Do Until S = ""
  If Not StrComp(S, ".") = 0 And Not StrComp(S, "..") = 0 Then
    S2 = folder + S
    If (GetAttr(S2) And vbDirectory) = vbDirectory Then
      ArrayC = ArrayC + 1
      ReDim Preserve ArrayF(1 To ArrayC) As String
      ArrayF(ArrayC) = S2
    Else
      cSize = cSize + FileLen(S2)
    End If
  End If
  S = Dir
Loop

If cDir < ArrayC Then
  cDir = cDir + 1
  folder = ArrayF(cDir)
  GoTo GETFOLDERS
End If
GetSubFolderSize = cSize
End Function

Public Sub CoolBorderPB(PB As PictureBox)
Dim Light As Long, Shadow As Long
Light = &H80000014
Shadow = &H80000010

PB.Line (0, 0)-(PB.ScaleWidth - 1, 0), Shadow
PB.Line (0, 1)-(PB.ScaleWidth - 1, 1), Light

PB.Line (0, PB.ScaleHeight - 2)-(PB.ScaleWidth - 1, PB.ScaleHeight - 2), Shadow
PB.Line (0, PB.ScaleHeight - 1)-(PB.ScaleWidth - 1, PB.ScaleHeight - 1), Light

PB.Line (1, 3)-(1, PB.ScaleHeight - 4), Light
PB.Line (2, 3)-(2, PB.ScaleHeight - 4), Shadow
PB.Line (3, 3)-(3, PB.ScaleHeight - 4), Light
PB.Line (4, 3)-(4, PB.ScaleHeight - 4), Shadow
End Sub

Public Sub StringToArr(Arr() As Byte, S As String)
Dim L As Long
L = Len(S)
ReDim Arr(0 To L) As Byte
CopyMemory Arr(0), ByVal S, L + 1
End Sub

Public Function ArrToString(Arr() As Byte, Optional NullEnd As Byte) As String
Dim L As Long
L = UBound(Arr)
ArrToString = String(L, " ")
CopyMemory ByVal ArrToString, Arr(0), L
If Not NullEnd = 0 Then
  L = InStr(1, ArrToString, Chr$(0), vbBinaryCompare)
  If L = 0 Then Exit Function
  ArrToString = Mid$(ArrToString, 1, L - 1)
End If
End Function

Public Sub SetObjectAddr(ByRef Obj As Object, nObj As Long)
CopyMemory Obj, nObj, 4
End Sub

Public Function GetObjectAddr(Obj As Object) As Long
Dim L As Long
CopyMemory L, Obj, 4
GetObjectAddr = L
End Function

Public Sub UCaseFirst(S As String)
On Error Resume Next
Mid$(S, 1, 1) = UCase(Mid$(S, 1, 1))
End Sub

Public Sub PrintMultiLineW(PB As Object, S As String, W As Long)
On Error Resume Next
Dim ST As String, A As Long, i As Long, x As Long
x = PB.CurrentX
i = 1
A = InStr(i, S, vbCrLf)
Do Until A = 0
  ST = ""
  ST = Mid$(S, i, A - i)
  PB.CurrentX = x
  
  
  If PB.TextWidth(ST) > W And W > 0 Then
REB:
    If Len(ST) > 1 Then ST = Mid$(ST, 1, Len(ST) - 1) Else ST = ""
    If Len(S) = 0 Then GoTo PRNT
    If PB.TextWidth(ST + "...") > W Then GoTo REB
    ST = ST + "..."
  End If
PRNT:
  PB.Print ST
  
  i = A + 2
  A = InStr(i, S, vbCrLf)
Loop

ST = ""
ST = Mid$(S, i)
PB.CurrentX = x

If PB.TextWidth(ST) > W And W > 0 Then
REB2:
  If Len(ST) > 1 Then ST = Mid$(ST, 1, Len(ST) - 1) Else ST = ""
  If Len(S) = 0 Then GoTo PRNT2
  If PB.TextWidth(ST + "...") > W Then GoTo REB2
  ST = ST + "..."
End If
PRNT2:

PB.Print ST
End Sub

Public Sub PrintMultiLine(PB As Object, S As String)
On Error Resume Next
Dim ST As String, A As Long, i As Long, x As Long
x = PB.CurrentX
i = 1
A = InStr(i, S, vbCrLf)
Do Until A = 0
  ST = ""
  ST = Mid$(S, i, A - i)
  PB.CurrentX = x
  PB.Print ST
  
  i = A + 2
  A = InStr(i, S, vbCrLf)
Loop

ST = ""
ST = Mid$(S, i)
PB.CurrentX = x
PB.Print ST
End Sub

Public Sub SaveWindowXY(frm As Form, vName As String)
SaveSetting App.EXEName, vName, "X", frm.Left
SaveSetting App.EXEName, vName, "Y", frm.Top
'SaveSetting "mpTRIAMP", vName, "X", frm.Left
'SaveSetting "mpTRIAMP", vName, "Y", frm.Top
End Sub

Public Sub LoadWindowXY(frm As Form, vName As String)
frm.Left = GetSetting(App.EXEName, vName, "X", frm.Left)
frm.Top = GetSetting(App.EXEName, vName, "Y", frm.Top)
End Sub

Public Function SeekZ(vbFile As Long, Optional NewPos As Long = -1) As Long
If NewPos = -1 Then SeekZ = Seek(vbFile) - 1 Else Seek vbFile, NewPos + 1
End Function

Public Sub FileCpy(vbFileSource As Long, vbFileDest As Long, Optional BufSize As Long = 32767, Optional List As ListBox, Optional FMTSTR As String, Optional DelayInt As Long)
On Error Resume Next
Dim buf() As Byte, NM As Long, lf As Long
ReDim buf(1 To BufSize) As Byte
lf = LOF(vbFileSource)
Do
  If Seek(vbFileSource) + BufSize <= lf Then
    Get vbFileSource, , buf()
    Put vbFileDest, , buf()
    If DelayInt > 0 Then
      If FillGet(DelayInt) = 1 Then
        List.List(1) = Replace$(FMTSTR, "%", Format$(GetPercent(1, lf, Seek(vbFileSource)), "0") + "%")
        List.Refresh
      End If
    End If
  Else
    NM = LOF(vbFileSource) - Seek(vbFileSource) + 1
    If NM = 0 Then Exit Do
    ReDim buf(1 To NM) As Byte
    Get vbFileSource, , buf()
    Put vbFileDest, , buf()
    Exit Do
  End If
Loop
If DelayInt > 0 Then
  List.List(1) = Replace$(FMTSTR, "%", "100%")
  List.Refresh
End If
End Sub

Public Function eUBound(ms) As Long
On Error Resume Next
eUBound = UBound(ms)
If eUBound < 0 Then eUBound = 0
End Function

Public Function MountDrive(DriveLetter As String, FolderPath As String, Optional Label As String) As Boolean
MountDrive = DefineDosDevice(0, DriveLetter & ":", FolderPath)
If Not Len(Label) = 0 Then SetVolumeLabel DriveLetter & ":\", Label
End Function

Public Function UnMountDrive(DriveLetter As String) As Boolean
UnMountDrive = DefineDosDevice(DDD_REMOVE_DEFINITION, DriveLetter & ":", "")
End Function

Public Function GetByteArrayPointer(Arr() As Byte) As Long
Dim u As Long, U2 As Long, U3 As Long, tSA As SAFEARRAY1D

u = VarPtrArray(Arr())
U2 = VarPtr(tSA)
CopyMemory U3, ByVal u, 4
CopyMemory ByVal U2, ByVal U3, Len(tSA)

GetByteArrayPointer = tSA.pvData
End Function

Public Function CPP(Text As String, Optional NONT As Boolean) As String
CPP = CPPFormat(Text, NONT)
End Function

Public Function CPPFormat(ByVal Text As String, Optional NONT As Boolean) As String
If Not NONT Then Text = Replace$(Text, "\n", vbCrLf, , , vbTextCompare)
If Not NONT Then Text = Replace$(Text, "\t", Chr$(9), , , vbTextCompare)
Text = Replace$(Text, "\042", Chr$(34), , , vbTextCompare)
Text = Replace$(Text, "\000", Chr$(0), , , vbTextCompare)
Text = Replace$(Text, "''", Chr$(34), , , vbTextCompare)
Text = Replace$(Text, "\\", "\", , , vbTextCompare)
CPPFormat = Text
End Function

Public Sub PrintTextCentered(Obj As Object, Text As String)
Obj.CurrentX = (Obj.ScaleWidth / 2) - (Obj.TextWidth(Text) / 2)
'Obj.Print Text
TextOut Obj.hdc, Obj.CurrentX, Obj.CurrentY, Text, Len(Text)
End Sub

Public Sub PrintTextFullCentered(Obj As Object, Text As String)
Obj.CurrentX = (Obj.ScaleWidth / 2) - (Obj.TextWidth(Text) / 2)
Obj.CurrentY = (Obj.ScaleHeight / 2) - (Obj.TextHeight(Text) / 2)
'Obj.Print Text
TextOut Obj.hdc, Obj.CurrentX, Obj.CurrentY, Text, Len(Text)
End Sub

Public Function IsMouseInWindow(hWnd As Long, Optional x As Long, Optional y As Long) As Boolean
Dim r As RECT, P As POINTAPI

GetWindowRect hWnd, r
GetCursorPos P
If P.x >= r.Left And P.x <= r.Right And P.y >= r.Top And P.y <= r.Bottom Then
  IsMouseInWindow = True
  x = P.x - r.Left
  y = P.y - r.Top
Else
  IsMouseInWindow = False
End If
End Function

Public Function IsMouseInControl(xform As Form, xctrl As Control, Optional x As Long, Optional y As Long) As Boolean
Dim r As RECT, P As POINTAPI

r.Top = (xform.Top / 15 + xctrl.Top)
r.Bottom = (xform.Top / 15 + (xctrl.Top + xctrl.Height))
r.Left = (xform.Left / 15 + xctrl.Left)
r.Right = (xform.Left / 15 + (xctrl.Left + xctrl.Width))

GetCursorPos P

If P.x >= r.Left And P.x <= r.Right And P.y >= r.Top And P.y <= r.Bottom Then
  IsMouseInControl = True
  x = P.x - r.Left
  y = P.y - r.Top
Else
  IsMouseInControl = False
End If
End Function

Function IsWindowOnScreen(inhandle As Long) As Boolean

Dim r As RECT, P As POINTAPI, handle As Long, nCurrent As Currency, Current As Currency

GetWindowRect inhandle, r


    For P.x = r.Left To r.Right Step 15
      For P.y = r.Top To r.Bottom Step 15
        handle = WindowFromPoint(P.x, P.y)
        If handle = inhandle Then IsWindowOnScreen = True:  Exit Function
      Next
    Next


IsWindowOnScreen = False

End Function

Public Function ObjectRef(Obj As Object) As Long
CopyMemory ObjectRef, Obj, 4
End Function

Public Sub CopyFileNoCache(SrcFileName As String, DestFileName As String, Optional Block As Long = 22, Optional ProgressB As Object, Optional UpdateInterval As Long = 50, Optional DoEventsHWND As Long)
Dim buffer() As Byte, BufSize As Long, i As Long, IC As Long, FileSrc As Long, FileDest As Long, UsePB As Byte

If Not GetObjectAddr(ProgressB) = 0 Then UsePB = 1

BufSize = 2 ^ Block
mKill DestFileName
IC = FileLen(SrcFileName)
FileSrc = CreateFile(SrcFileName, GENERIC_READ, FILE_SHARE_READ, 0, OPEN_EXISTING, FILE_FLAG_NO_BUFFERING, 0)
FileDest = CreateFile(DestFileName, GENERIC_WRITE, FILE_SHARE_WRITE, 0, CREATE_NEW, FILE_FLAG_NO_BUFFERING, 0)
'FileDest = CreateFile(DestFileName, GENERIC_WRITE, FILE_SHARE_WRITE Or FILE_SHARE_READ, 0, OPEN_EXISTING, FILE_FLAG_OVERLAPPED, 0)
If Not UsePB = 0 Then ProgressB.Max = IC

ReDim buffer(1 To BufSize) As Byte
Do Until i + BufSize >= IC
  ReadFile FileSrc, buffer(1), BufSize, 0, 0
  WriteFile FileDest, buffer(1), BufSize, 0, 0
  i = i + BufSize
  If Not UsePB = 0 Then If FillGet(UpdateInterval, 21) = 1 Then ProgressB.Value = i: ProgressB.RePaint: If Not DoEventsHWND = 0 Then mDoEvents DoEventsHWND
Loop

CloseHandle FileSrc
CloseHandle FileDest

If i = IC Then Exit Sub

FileSrc = FreeFile
Open SrcFileName For Binary Access Read As FileSrc
  FileDest = FreeFile
  Open DestFileName For Binary Access Write As FileDest
    BufSize = IC - i
    ReDim buffer(1 To BufSize) As Byte
    Get FileSrc, i, buffer()
    Put FileDest, i, buffer()
    i = IC
  Close FileDest
Close FileSrc
If Not UsePB = 0 Then ProgressB.Value = i: ProgressB.RePaint
End Sub

Public Function RangeRnd(Range As Double) As Double
RangeRnd = Rnd * (Range * 2) - Range
End Function

Public Sub PlayAVI(hWnd As Long, FileName As String)
Dim i As Long, RS As String, cb As Long, A$, x As Long, y As Long

RS = Space$(128)
A$ = GetSHORT(FileName)
i = mciSendString("open AVIvideo!" & A$ & " alias movie parent " & hWnd & " style child", RS, 128, cb)
'i = mciSendString("open AVIvideo!" & A$ & " alias movie parent " & GetDesktopWindow & " style child", RS, 128, cb)
'i = mciSendString("put movie window at " & 0 & " " & 0 & " " & 1024 & " " & 768, 0&, 0&, 0&)
'i = mciSendString("put movie window client at 200 0 0 0", RS, 128, cb)
'If i Then MsgBox "Error! Probably file not found. Modify the code to point to an .AVI file on your system."
i = mciSendString("play movie", RS, 128, cb)
End Sub

Public Sub StopAVI()
Dim i As Long, RS As String, cb As Long

RS = Space$(128)
i = mciSendString("stop movie", RS, 128, cb)
i = mciSendString("close movie", RS, 128, cb)
End Sub

Public Sub RePlayAVI()
Dim i As Long, RS As String, cb As Long

RS = Space$(128)
i = mciSendString("play movie from 0", RS, 128, cb)
End Sub

Public Function GetPosAVI() As Single
Dim i As Long, RS As String, cb As Long, S As Single, S2 As Single

RS = Space$(128)
i = mciSendString("status movie length", RS, 128, cb)

If Val(RS) Then
  S = Val(RS)
  i = mciSendString("status movie position", RS, 128, cb)
  GetPosAVI = (Val(RS) / S) * 100
Else
  GetPosAVI = 100
End If
End Function

'Public Function AlphaBlend(ByVal destHDC As Long, ByVal XDest As Long, ByVal YDest As Long, ByVal destWidth As Long, ByVal destHeight As Long, ByVal srcHDC As Long, ByVal XSrc As Long, ByVal YSrc As Long, ByVal srcWidth As Long, ByVal srcHeight As Long, ByVal AlphaSource As Long) As Long
'  Dim lngBlend As Long
'  lngBlend = Val("&h" & Hex(AlphaSource) & "00" & "00")
'  AlphaBlending destHDC, XDest, YDest, destWidth, destHeight, srcHDC, XSrc, ycrc, srcWidth, srcHeight, lngBlend
'End Function

Public Sub SmoothUnload(hWnd As Long)
Dim BG As tPicture, FR As tPicture, BFR As tPicture, r As RECT, DC As Long, i As Long

GetWindowRect hWnd, r
r.Bottom = r.Bottom - r.Top
r.Right = r.Right - r.Left

BG = CreateEmptytPicture(r.Right, r.Bottom)
FR = CreateEmptytPicture(r.Right, r.Bottom)
BFR = CreateEmptytPicture(r.Right, r.Bottom)

DC = GetWindowDC(GetDesktopWindow)
BitBlt FR.PDC, 0, 0, r.Right, r.Bottom, DC, r.Left, r.Top, vbSrcCopy

ShowWindow hWnd, 0
DoEvents
Sleep 100
BitBlt BG.PDC, 0, 0, r.Right, r.Bottom, DC, r.Left, r.Top, vbSrcCopy
ShowWindow hWnd, 4

'DC = GetWindowDC(hwnd)
DC = GetWindowDC(GetDesktopWindow)

For i = 0 To 255 Step 50
  BeginMeter
  BitBlt BFR.PDC, 0, 0, r.Right, r.Bottom, BG.PDC, 0, 0, vbSrcCopy
  'AlphaBlend BFR.PDC, 0, 0, R.Right, R.Bottom, FR.PDC, 0, 0, R.Right, R.Bottom, i
  BitBlt DC, r.Left, r.Top, r.Right, r.Bottom, BFR.PDC, 0, 0, vbSrcCopy
  NeedSleep 20
Next

For i = 255 To 0 Step -8
  BeginMeter
  BitBlt BFR.PDC, 0, 0, r.Right, r.Bottom, BG.PDC, 0, 0, vbSrcCopy
  'AlphaBlend BFR.PDC, 0, 0, R.Right, R.Bottom, FR.PDC, 0, 0, R.Right, R.Bottom, i
  BitBlt DC, r.Left, r.Top, r.Right, r.Bottom, BFR.PDC, 0, 0, vbSrcCopy
  NeedSleep 20
Next

ShowWindow hWnd, 0

DeletetPicture BFR
DeletetPicture FR
DeletetPicture BG
End Sub

Public Function FilterCommandFN(CFN As String) As String
If Len(CFN) > 0 Then
  If Mid$(CFN, 1, 1) = Chr$(34) Then
    FilterCommandFN = Mid$(CFN, 2, Len(CFN) - 2)
  Else
    FilterCommandFN = GetLONGN(CFN)
  End If
End If
End Function

Public Function WindowText(hWnd As Long) As String
Dim L As Long
L = SendMessage(hWnd, WM_GETTEXTLENGTH, 0, CLng(0))
If L <= 0 Then Exit Function

WindowText = String(L + 1, " ")
SendMessage hWnd, WM_GETTEXT, L + 1, ByVal WindowText
WindowText = NoZeroS(WindowText)
End Function

Public Function GetWText(PB As Object, W As Long, Text As String) As String
On Error Resume Next
Dim i As Long, IC As Long
If PB.TextWidth(Text) <= W Then GetWText = Text: Exit Function

IC = Len(Text)
For i = 1 To IC
  If PB.TextWidth(Mid$(Text, 1, i) + "...") > W Then
    GetWText = Mid$(Text, 1, i - 1) + "..."
    Exit Function
  End If
Next
End Function

Public Function Format3(Value) As String
If Not Value = 0 Then
  Format3 = Format$(Value, "#,##0")
Else
  Format3 = Format$(Value)
End If
End Function

Public Function PrintF(ByVal S As String, ParamArray params() As Variant) As String
On Error Resume Next
Dim i As Long, IC As Long, A As Long, B As Long, c As Long, SS As String
Dim S1 As String, S2 As String

S = CPPFormat(S)
IC = UBound(params)
For i = 0 To IC
  If VarType(params(i)) = vbString Then SS = params(i) Else SS = Format$(params(i))
  A = InStr(1, S, "%s", vbTextCompare)
  
  If A > 1 Then S1 = Mid$(S, 1, A - 1) Else S1 = ""
  If Not A = Len(S) - 1 Then S2 = Mid$(S, A + 2) Else S2 = ""
  
  S = S1 + SS + S2
  'S = Replace$(S, "%s", SS, A, 2, vbTextCompare)
Next
PrintF = S
End Function

Public Sub SetChildsEnabled(hWnd As Long, Enable As Long)
Dim A As Long

A = GetWindow(hWnd, GW_CHILD)
If A = 0 Then Exit Sub
A = GetWindow(A, GW_HWNDFIRST)
If A = 0 Then Exit Sub

Do
  SetChildsEnabled A, Enable
  EnableWindow A, Enable
  A = GetWindow(A, GW_HWNDNEXT)
Loop Until A = 0
End Sub

Public Function GetEnv(VarName As String) As String
GetEnv = String(4096, " ")
GetEnvironmentVariable VarName, GetEnv, 4095
VarName = NoZeroS(VarName)
End Function

Public Sub BlendXPIn(frm As Object, Optional Delay As Long = 1000, Optional pv As Long = 2)
On Error Resume Next
Dim h As Long, i As Long
h = GetWindowLong(frm.hWnd, GWL_EXSTYLE)
SetWindowLong frm.hWnd, GWL_EXSTYLE, h Or 524288

BeginMeter
Do Until EndMeter > Delay
  i = EndMeter / (Delay / CSng(255))
  SetLayeredWindowAttributes frm.hWnd, 0, CByte(i), pv
  If i = 0 Then frm.Show
  RedrawWindow frm.hWnd, 0, 0, RDW_ERASE Or RDW_INVALIDATE Or RDW_FRAME Or RDW_ALLCHILDREN
  DoEvents
Loop

SetWindowLong frm.hWnd, GWL_EXSTYLE, h
RedrawWindow frm.hWnd, 0, 0, RDW_ERASE Or RDW_INVALIDATE Or RDW_FRAME Or RDW_ALLCHILDREN
End Sub

Public Sub SetFormLayered(ByRef frm As Object)
On Error Resume Next
Dim h As Long
h = GetWindowLong(frm.hWnd, GWL_EXSTYLE)
SetWindowLong frm.hWnd, GWL_EXSTYLE, h Or 524288
End Sub

Public Sub SetFormTColorXP(frm As Object, TColor As Long, Optional Transparency As Long = 255)
On Error Resume Next
Dim h As Long
h = GetWindowLong(frm.hWnd, GWL_EXSTYLE)
SetWindowLong frm.hWnd, GWL_EXSTYLE, h Or 524288

SetLayeredWindowAttributes frm.hWnd, TColor, Transparency, IIf(Transparency = 255, 1, 3)
RedrawWindow frm.hWnd, 0, 0, RDW_ERASE Or RDW_INVALIDATE Or RDW_FRAME Or RDW_ALLCHILDREN
End Sub

Public Sub SetWindowTColorXP(frmHandle As Long, TColor As Long, Optional Transparency As Long = 255)
On Error Resume Next
Dim h As Long
h = GetWindowLong(frmHandle, GWL_EXSTYLE)
SetWindowLong frmHandle, GWL_EXSTYLE, h Or 524288

SetLayeredWindowAttributes frmHandle, TColor, Transparency, IIf(Transparency = 255, 1, 3)
RedrawWindow frmHandle, 0, 0, RDW_ERASE Or RDW_INVALIDATE Or RDW_FRAME Or RDW_ALLCHILDREN
End Sub

Public Sub SetFormAlphaXP(frm As Object, Optional Transparency As Long = 255)
On Error Resume Next
Dim h As Long
h = GetWindowLong(frm.hWnd, GWL_EXSTYLE)
SetWindowLong frm.hWnd, GWL_EXSTYLE, h Or 524288

SetLayeredWindowAttributes frm.hWnd, 0, Transparency, 2
RedrawWindow frm.hWnd, 0, 0, RDW_ERASE Or RDW_INVALIDATE Or RDW_FRAME Or RDW_ALLCHILDREN
End Sub

Public Sub RemoveFormTColorXP(frm As Object, Optional Transparency As Long = 255)
On Error Resume Next
Dim h As Long
h = GetWindowLong(frm.hWnd, GWL_EXSTYLE)

If Transparency = 255 Then
  SetWindowLong frm.hWnd, GWL_EXSTYLE, IIf(h And 524288, h Xor 524288, h)
Else
  SetWindowLong frm.hWnd, GWL_EXSTYLE, h Or 524288
  SetLayeredWindowAttributes frm.hWnd, 0, Transparency, 2
End If
End Sub

Public Sub BlendXPOut(frm As Object, Optional Delay As Long = 1000, Optional pv As Long = 2)
On Error Resume Next
Dim h As Long, i As Long
h = GetWindowLong(frm.hWnd, GWL_EXSTYLE)
SetWindowLong frm.hWnd, GWL_EXSTYLE, h Or 524288

BeginMeter
Do Until EndMeter > Delay
  i = EndMeter / (Delay / CSng(255))
  i = 255 - i
  If i < 0 Then i = 0
  SetLayeredWindowAttributes frm.hWnd, 0, CByte(i), pv
  RedrawWindow frm.hWnd, 0, 0, RDW_ERASE Or RDW_INVALIDATE Or RDW_FRAME Or RDW_ALLCHILDREN
  DoEvents
Loop

frm.Hide
SetWindowLong frm.hWnd, GWL_EXSTYLE, h
RedrawWindow frm.hWnd, 0, 0, RDW_ERASE Or RDW_INVALIDATE Or RDW_FRAME Or RDW_ALLCHILDREN
End Sub

Public Function GetParameters(ByVal cLine As String, ParamArr() As String) As Long
Dim i As Long, IC As Long, c As String * 1, InKav As Long

cLine = Trim$(cLine)
IC = Len(cLine)
If Not IC = 0 Then GetParameters = 1: ReDim ParamArr(1 To GetParameters) As String
For i = 1 To IC
  c = Mid$(cLine, i, 1)
  If Asc(c) = 32 And InKav = 0 Then
    If Not Len(ParamArr(GetParameters)) = 0 Then
      GetParameters = GetParameters + 1
      ReDim Preserve ParamArr(1 To GetParameters) As String
    End If
  ElseIf Asc(c) = 34 Then
    InKav = Abs(InKav - 1)
  Else
    ParamArr(GetParameters) = ParamArr(GetParameters) + c
  End If
Next
End Function

Public Function IsInList(List() As String, Text As String, Optional Compare As VbCompareMethod = vbBinaryCompare) As Long
On Error Resume Next
Dim i As Long, IC As Long, IB As Long
IB = 1
IC = UBound(List)
IB = LBound(List)
For i = IB To IC
  If StrComp(List(i), Text, Compare) = 0 Then
    IsInList = i: Exit Function
  End If
Next
IsInList = -1
End Function

Public Sub AssToMe(Ext As String, Section As String)
FileAss Section, Chr$(34) + DirFilterIN(App.Path) + "\" + App.EXEName + ".exe" + Chr$(34) + " " + Chr$(34) + "%1" + Chr$(34), Ext, DirFilterIN(App.Path) + "\" + App.EXEName + ".exe,0"
End Sub

Public Function InVB() As Boolean
If InStr(1, App.Path, "vb98", vbTextCompare) = 0 Then InVB = False Else InVB = True
End Function

Public Function ApplyMenuBitmap(hMenu As Long, Item As Long, PictureHandle As Long)
Dim lngID As Long
lngID = GetMenuItemID(hMenu, Item)
Call ModifyMenu(hMenu, lngID, MF_BITMAP, lngID, PictureHandle)
End Function


Public Sub CopyComboBox(Source As Object, dest As Object)
Dim i As Long, IC As Long

dest.Clear
IC = Source.ListCount - 1
For i = 0 To IC
  dest.AddItem Source.List(i)
  dest.ItemData(dest.NewIndex) = Source.ItemData(i)
Next
If Not Source.ListIndex < 0 Then dest.ListIndex = Source.ListIndex
End Sub

Public Function FindComboBoxItem(Co As ComboBox, Item As String, Optional Compare As VbCompareMethod = vbBinaryCompare) As Long
Dim i As Long, IC As Long

IC = Co.ListCount - 1
For i = 0 To IC
  If StrComp(Co.List(i), Item, Compare) = 0 Then
    FindComboBoxItem = i
    Exit Function
  End If
Next
FindComboBoxItem = -1
End Function

Public Function Is2KXP() As Boolean
Dim B(1 To 4) As Byte
CopyMemory B(1), GetWindowsVersion(), 4
If B(1) >= 5 Then Is2KXP = True Else Is2KXP = False
End Function

Public Function Is2K() As Boolean
Dim B(1 To 4) As Byte
CopyMemory B(1), GetWindowsVersion(), 4
If B(1) = 5 And B(2) = 0 Then Is2K = True
End Function

Public Function WindowClass(hWnd As Long) As String
WindowClass = String(256, " ")
GetClassName hWnd, ByVal WindowClass, 256
WindowClass = NoZeroS(WindowClass)
End Function

Public Function IsTotalCmD(Optional RetPath As String) As Boolean
On Error Resume Next
Dim i, drv As Long, d As String
For i = 0 To 25
  d = Chr$(i + 65) & ":\"
  drv = GetDriveType(d)
  If drv = DRIVE_FIXED Then
    If CheckFile(d & "Totalcmd\TOTALCMD.EXE") = 1 Then RetPath = d & "Totalcmd\TOTALCMD.EXE": IsTotalCmD = True: Exit Function
    If CheckFile(d & "Program Files\totalcmd\TOTALCMD.EXE") = 1 Then RetPath = d & "Program Files\totalcmd\TOTALCMD.EXE": IsTotalCmD = True: Exit Function
  End If
NoDrive:
Next i
IsTotalCmD = False
End Function

Public Function OpenInTotalCmD(Path As String) As Long
On Error Resume Next
Dim A As Long, S As String, B As Long
A = GetWindow(GetDesktopWindow, GW_CHILD)
If A = 0 Then GoTo PARTTWO
A = GetWindow(A, GW_HWNDFIRST)

Do
  If InStr(1, WindowText(A), "Total Commander", vbTextCompare) = 1 Then
    B = A
    ShowWindow A, 1
    SetWindowPos A, HWND_TOP, 0, 0, 0, 0, SWP_NOMOVE Or SWP_NOSIZE
    A = GetWindow(A, 5)
    If A = 0 Then A = B: GoTo NEX
    A = GetWindow(A, GW_HWNDFIRST)
    Do
      If WindowClass(A) = "TMyPanel" Then
        A = GetWindow(A, GW_CHILD)
        If A = 0 Then A = B: GoTo NEX
        A = GetWindow(A, GW_HWNDFIRST)
        Do
          If WindowClass(A) = "TComboBox" Then
            SendMessage A, WM_SETTEXT, 0, ByVal ("cd " & Path)
            SendMessage A, WM_KEYDOWN, 13, 0
            SendMessage A, WM_KEYUP, 13, 0
            Exit Function
          End If
          A = GetWindow(A, GW_HWNDNEXT)
        Loop Until A = 0
      End If
      A = GetWindow(A, GW_HWNDNEXT)
    Loop Until A = 0
    A = B: GoTo NEX
  End If
NEX:
  A = GetWindow(A, GW_HWNDNEXT)
Loop Until A = 0


PARTTWO:
If IsTotalCmD(S) = True Then
  Call Shell(S & " " & Path, vbNormalFocus)
End If
End Function

Public Function SendTompTRIAMP(Text As String) As Long
On Error Resume Next
Dim A As Long, S As String, B As Long
A = FindWindowS(0, "mpTRIAMP PlayList (Waiting...)")
If A = 0 Then Exit Function

ShowWindow A, 1
SetWindowPos A, HWND_TOP, 0, 0, 0, 0, SWP_NOMOVE Or SWP_NOSIZE
A = GetWindow(A, 5)
If A = 0 Then Exit Function
SendMessage A, WM_SETTEXT, 0, ByVal Text
SendTompTRIAMP = 1
End Function

Public Function UnHex2(S As String) As Byte
Dim B1 As Byte, b2 As Byte, B As Byte

B = Asc(Mid$(S, 1, 1))
B1 = IIf(B > 64, B - 55, B - 48)
B = Asc(Mid$(S, 2, 1))
b2 = IIf(B > 64, B - 55, B - 48)

UnHex2 = B1 * 16 + b2
End Function

Public Function ConvertHexColor(ByVal HexColor As String) As Long
If Not Len(HexColor) = 6 Then Exit Function

HexColor = UCase$(HexColor)
ConvertHexColor = RGB(UnHex2(Mid$(HexColor, 1, 2)), UnHex2(Mid$(HexColor, 3, 2)), UnHex2(Mid$(HexColor, 5, 2)))
End Function

Public Function DrawBitmapToDC(hBMP As Long, x As Long, y As Long, W As Long, h As Long, ToDC As Long, SrcX As Long, SrcY As Long, Optional Rop As Long = vbSrcCopy)
Dim hdc As Long, hBmpOld As Long

hdc = CreateCompatibleDC(0)
hBmpOld = SelectObject(hdc, hBMP)

BitBlt ToDC, x, y, W, h, hdc, SrcX, SrcY, Rop

SelectObject hdc, hBmpOld
DeleteDC hdc
End Function

Public Function FilterFileName(FN As String) As String
FilterFileName = FN
FilterFileName = Replace$(FilterFileName, ";", "_")
FilterFileName = Replace$(FilterFileName, "\", "_")
FilterFileName = Replace$(FilterFileName, "|", "_")
FilterFileName = Replace$(FilterFileName, "/", "_")
FilterFileName = Replace$(FilterFileName, ":", "_")
FilterFileName = Replace$(FilterFileName, "*", "_")
FilterFileName = Replace$(FilterFileName, "<", "_")
FilterFileName = Replace$(FilterFileName, ">", "_")
FilterFileName = Replace$(FilterFileName, "?", "_")
FilterFileName = Replace$(FilterFileName, Chr$(34), "_")
End Function

Public Function FormatFF(Expression, Optional formats, Optional FirstDayOfWeek As VbDayOfWeek = vbMonday, Optional FirstWeekOfYear As VbFirstWeekOfYear = vbFirstJan1) As String
FormatFF = FixFloatF(Format$(Expression, formats, FirstDayOfWeek, FirstWeekOfYear))
End Function

Public Function uBOUNDL(LongArray() As Long) As Long
On Error Resume Next
uBOUNDL = UBound(LongArray())
End Function

Public Function uBOUNDB(LongArray() As Byte) As Long
On Error Resume Next
uBOUNDB = UBound(LongArray())
End Function

Public Function CompressString(Text As String, Optional level As Long = 9) As String
Dim Arr() As Byte
StringToArray Text, Arr()
CompressArray Arr(), level
ArrayToString CompressString, Arr()
End Function

Public Function DecompressString(Text As String) As String
Dim Arr() As Byte
StringToArray Text, Arr()
DecompressArray Arr()
ArrayToString DecompressString, Arr()
End Function

Public Function CompressArray(Arr() As Byte, Optional level As Long = 9) As Long
Dim IB As Long, IC As Long, DestIC As Long
Dim DestArr() As Byte, DL As Long, r As Long

CompressArray = 4
IC = uBOUNDB(Arr())
If IC = -1 Then ReDim Arr(1 To 4) As Byte: CopyMemory Arr(1), CLng(4), 4: Exit Function
IB = LBound(Arr())
DestIC = IC * 1.5
ReDim DestArr(IB To DestIC) As Byte

DL = (DestIC - IB) + 1
r = compress2(DestArr(IB), DL, Arr(IB), (IC - IB) + 1, level)
If r <> 0 Then Erase Arr(): CompressArray = 0: Exit Function

ReDim Arr(IB To (DL + IB) + 3) As Byte
CopyMemory Arr(IB), CLng((IC - IB) + 1), 4
CopyMemory Arr(IB + 4), DestArr(IB), DL
CompressArray = DL + 4
End Function

Public Function DecompressArray(Arr() As Byte, Optional ArrlBound As Long = 1) As Long
Dim IB As Long, IC As Long, DestIC As Long
Dim DestArr() As Byte, DL As Long

IC = -1
IC = uBOUNDB(Arr())
IB = ArrlBound
If IC = -1 Then Erase Arr: Exit Function
CopyMemory DL, Arr(IB), 4
If DL <= 0 Or DL > 100000000 Then Erase Arr: Exit Function

DestIC = DL
ReDim DestArr(IB To DestIC) As Byte

'DL = (DestIC - IB) + 1
If uncompress(DestArr(IB), DL, Arr(IB + 4), (IC - IB) - 3) <> 0 Then Erase Arr(): DecompressArray = 0: Exit Function

ReDim Arr(IB To (DL + IB) - 1) As Byte
CopyMemory Arr(IB), DestArr(IB), DL
DecompressArray = DL
End Function

Public Function ByteArrayPointer(Arr() As Byte) As Long
Dim u As Long, U2 As Long, U3 As Long, tSA As SAFEARRAY2D

u = VarPtrArray(Arr())
U2 = VarPtr(tSA)
CopyMemory U3, ByVal u, 4
CopyMemory ByVal U2, ByVal U3, Len(tSA)

ByteArrayPointer = tSA.pvData
'MsgBox GetUnitDataPointer & vbCrLf & VBArrayPointer(Arr())
End Function

Public Function DateTimeFN() As String
DateTimeFN = Format$(Year(Date)) & Format$(Month(Date), "00") & Format$(Day(Date), "00") & Format$(Hour(time), "00") & Format$(Minute(time), "00") & Format$(Second(time), "00") & Format$(SystemTimer, "0000000000")
End Function

Public Sub AddToString(Text As String, Add As String, Optional Char As String = vbCrLf)
If Len(Text) = 0 Then Text = Add Else Text = Text & Char & Add
End Sub

Public Function FillGetC(INTERVAL As Long, Optional LastTime As Long = 0) As Byte
If timeGetTime - LastTime >= INTERVAL Then FillGetC = 1: Exit Function
If timeGetTime < LastTime Then FillGetC = 1 'TFill(i) = timeGetTime
End Function

Public Function IsNT() As Boolean
Dim V As OSVERSIONINFO
V.dwOSVersionInfoSize = Len(V)
If GetVersionEx(V) <> 0 Then
  If V.dwPlatformId = 2 Then IsNT = True
End If
End Function

Public Sub GetVideoMode(ByRef scWidth As Long, ByRef scHeight As Long, ByRef scDepth As Long)
    Dim hdc As Long
    hdc = GetDC(GetDesktopWindow())
    scWidth = GetDeviceCaps(hdc, HORZRES)
    scHeight = GetDeviceCaps(hdc, VERTRES)
    scDepth = GetDeviceCaps(hdc, BITSPIXEL)
    ReleaseDC GetDesktopWindow(), hdc
End Sub

Public Function ScreenWidth() As Long
Dim L As Long
GetVideoMode L, 0, 0
ScreenWidth = L
End Function

Public Function ScreenHeight() As Long
Dim L As Long
GetVideoMode 0, L, 0
ScreenHeight = L
End Function

Public Function ScreenDepth() As Long
Dim L As Long
GetVideoMode 0, 0, L
ScreenDepth = L
End Function

Public Function MySplit(ToArray() As String, Expression As String, Optional Delimiter As String = vbCrLf, Optional Limit As Long, Optional Compare As VbCompareMethod = vbBinaryCompare) As Long
Dim TAC As Long, i As Long, L As Long
Erase ToArray()

If Len(Expression) = 0 Then ReDim ToArray(1 To 1) As String: TAC = 1

L = 1
Do
  If Compare = vbBinaryCompare Then
    i = MyInStr(L, Expression, Delimiter)
  Else
    i = InStr(L, Expression, Delimiter, Compare)
  End If
  If i = 0 Then Exit Do
  TAC = TAC + 1
  ReDim Preserve ToArray(1 To TAC) As String
  ToArray(TAC) = Mid$(Expression, L, (i - L))
  
  If TAC >= Limit And Limit > 0 Then ToArray(TAC) = ToArray(TAC) & Mid$(Expression, i): GoTo NEX
  L = i + Len(Delimiter)
Loop
If Len(Expression) >= L Then
  TAC = TAC + 1
  ReDim Preserve ToArray(1 To TAC) As String
  ToArray(TAC) = Mid$(Expression, L)
End If

NEX:
MySplit = TAC
End Function

Public Function IsPathRelative(Path As String) As Boolean
If Len(Path) < 3 Then IsPathRelative = True: Exit Function
If Mid$(Path, 1, 2) = "//" Then Exit Function
If Not Mid$(Path, 2, 1) = ":" Then IsPathRelative = True: Exit Function
End Function

Public Function SortStringArray(Arr() As String, Optional Compare As VbCompareMethod = vbTextCompare) As Long
On Error Resume Next
Dim i As Long, IC As Long, j As Long, k As Long
Dim index As Long, Index2 As Long, M_index As Long, M_index2 As Long
Dim firstItem As Long, distance As Long, Value As String
Dim m_Value As Long, numEls As Long

IC = UBound(Arr)
If IC = 0 Then Exit Function

'ShellSort
numEls = IC
firstItem = 1
Do
  distance = distance * 3 + 1
Loop Until distance > numEls
Do
  distance = distance \ 3
  For index = distance + 1 To numEls
    Value = Arr(index)
    Index2 = index
    Do While StrComp(Arr(Index2 - distance), Value, Compare) > 0
      Arr(Index2) = Arr(Index2 - distance)
      Index2 = Index2 - distance
      If Index2 <= distance Then Exit Do
    Loop
    Arr(Index2) = Value
  Next
Loop Until distance = 1
SortStringArray = IC
End Function

Public Function SortStringArrayFN(Arr() As String, Optional Compare As VbCompareMethod = vbTextCompare) As Long
On Error Resume Next
Dim i As Long, IC As Long, j As Long, k As Long
Dim index As Long, Index2 As Long, M_index As Long, M_index2 As Long
Dim firstItem As Long, distance As Long, Value As String
Dim m_Value As Long, numEls As Long

IC = UBound(Arr)
If IC = 0 Then Exit Function

'ShellSort
numEls = IC
firstItem = 1
Do
  distance = distance * 3 + 1
Loop Until distance > numEls
Do
  distance = distance \ 3
  For index = distance + 1 To numEls
    Value = Arr(index)
    Index2 = index
    Do While StrComp(GetFileNameIN(Arr(Index2 - distance)), GetFileNameIN(Value), Compare) > 0
      Arr(Index2) = Arr(Index2 - distance)
      Index2 = Index2 - distance
      If Index2 <= distance Then Exit Do
    Loop
    Arr(Index2) = Value
  Next
Loop Until distance = 1
SortStringArrayFN = IC
End Function

Public Function LoadIcon32(FN As String) As Sprite32
On Error Resume Next
Dim f As Long
Dim id As ICONDIR, IDE As ICONDIRENTRY
Dim bih As BITMAPINFOHEADER

f = FreeFile
Open FN For Binary Access Read As #f
If f = FreeFile Then Exit Function
  Get #f, 1, id
  Get #f, , IDE
  Get #f, , bih
  LoadIcon32.Width = bih.biWidth
  LoadIcon32.Height = bih.biHeight / 2
  ReDim LoadIcon32.bits(1 To LoadIcon32.Width, 1 To LoadIcon32.Height) As BGRT
  Get #f, , LoadIcon32.bits()
Close f
End Function

Public Function LoadTGA32(FN As String, Optional ByVal Pos As Long = 1) As Sprite32
On Error Resume Next
Dim f As Long, W As Integer, h As Integer, T As Long, B As Byte

f = FreeFile
Open FN For Binary Access Read As #f
If f = FreeFile Then Exit Function
  Get #f, Pos, T
  If Not T = 131072 Then Exit Function 'Not TGA
  Get #f, Pos + 12, W
  Get #f, Pos + 14, h
  Get #f, Pos + 16, B
  If Not B = 32 Then Exit Function 'Not 32 Bit
  
  LoadTGA32.Width = W
  LoadTGA32.Height = h
  ReDim LoadTGA32.bits(1 To LoadTGA32.Width, 1 To LoadTGA32.Height) As BGRT
  Get #f, Pos + 18, LoadTGA32.bits()
Close f
End Function

Public Function GetBGRTPointer(Arr() As BGRT) As Long
Dim u As Long, U2 As Long, U3 As Long, tSA As SAFEARRAY1D

u = VarPtrArray(Arr())
U2 = VarPtr(tSA)
CopyMemory U3, ByVal u, 4
CopyMemory ByVal U2, ByVal U3, Len(tSA)

GetBGRTPointer = tSA.pvData
End Function

Public Function LoadTGA32Mem(Data() As Byte) As Sprite32
On Error Resume Next
Dim f As Long, W As Integer, h As Integer, T As Long, B As Byte ', i As Long, j As Long, L As Long
'Dim MS() As BGRT
'Dim BT() As BGRT

CopyMemory T, Data(1), 4
If Not T = 131072 Then Exit Function 'Not TGA
CopyMemory W, Data(13), 2
CopyMemory h, Data(15), 2
CopyMemory B, Data(17), 1
If Not B = 32 Then Exit Function 'Not 32 Bit

LoadTGA32Mem.Width = W
LoadTGA32Mem.Height = h
ReDim LoadTGA32Mem.bits(1 To LoadTGA32Mem.Width, 1 To LoadTGA32Mem.Height) As BGRT
'ReDim MS(1 To LoadTGA32Mem.Width, 1 To LoadTGA32Mem.Height) As BGRT
'ReDim BT(1 To LoadTGA32Mem.Width, 1 To LoadTGA32Mem.Height) As BGRT
'For i = 1 To W
'  For j = 1 To H
'    LoadTGA32Mem.Bits(i, j).B = Data(19 + L): L = L + 1
'    LoadTGA32Mem.Bits(i, j).G = Data(19 + L): L = L + 1
'    LoadTGA32Mem.Bits(i, j).R = Data(19 + L): L = L + 1
'    LoadTGA32Mem.Bits(i, j).T = Data(19 + L): L = L + 1
'  Next
'Next

'MsgBox VarPtr(MS(1, 1)) & " - " & GetBGRTPointer(MS())

CopyMemory ByVal GetBGRTPointer(LoadTGA32Mem.bits()), Data(19), CLng(W) * CLng(h) * 4#
'MsgBox Data(19) & " - " & MS(1, 1).B & " - " & CLng(W) * CLng(H) * 4#
'LoadTGA32Mem.Bits = MS
End Function

Public Function CreateSprite32(ByVal Width As Long, ByVal Height As Long) As Sprite32
CreateSprite32.Width = Width
CreateSprite32.Height = Height
ReDim CreateSprite32.bits(1 To Width, 1 To Height) As BGRT
End Function

Public Function InvertAlpha32(SPR As Sprite32, Optional ByVal x As Long, Optional ByVal y As Long, Optional ByVal W As Long = -1, Optional ByVal h As Long = -1) As Long
If W < 0 Then W = SPR.Width
If h < 0 Then h = SPR.Height
InvertAlpha32C SPR, x, y, W, h
End Function

Public Function LoadRes(id As Long, ms() As Byte) As Long
On Error Resume Next
Dim Arr() As Byte, IC As Long
Arr = LoadResData(id, "Custom")
IC = UBound(Arr) + 1
ReDim ms(1 To IC) As Byte
CopyMemory ms(1), Arr(0), IC
LoadRes = IC
End Function

Public Function LoadResS(id As Long) As String
On Error Resume Next
Dim Arr() As Byte, IC As Long
Arr = LoadResData(id, "Custom")
IC = UBound(Arr) + 1
LoadResS = String(IC, " ")
CopyMemory ByVal LoadResS, Arr(0), IC
End Function

Public Function MenuResHandle(id As Long) As Long
Dim ms() As Byte
LoadRes id, ms(): MenuResHandle = GetIcon32Handle(LoadTGA32Mem(ms()))
End Function

Public Function ResIcon32(id As Long) As Sprite32
Dim ms() As Byte
LoadRes id, ms()
ResIcon32 = LoadIcon32Mem(ms())
End Function

Public Function LoadIcon32Mem(Data() As Byte) As Sprite32
On Error Resume Next
Dim f As Long
Dim id As ICONDIR, IDE As ICONDIRENTRY
Dim bih As BITMAPINFOHEADER

f = FreeFile
'Open FN For Binary Access Read As #F
'If F = FreeFile Then Exit Function
  'Get #F, 1, id
  'Get #F, , IDE
  'Get #F, , bih
  CopyMemory id, Data(1), Len(id)
  CopyMemory IDE, Data(1 + Len(id)), Len(IDE)
  CopyMemory bih, Data(1 + Len(id) + Len(IDE)), Len(bih)
  LoadIcon32Mem.Width = bih.biWidth
  LoadIcon32Mem.Height = bih.biHeight / 2
  ReDim LoadIcon32Mem.bits(1 To LoadIcon32Mem.Width, 1 To LoadIcon32Mem.Height) As BGRT
  'Get #F, , LoadIcon32Mem.Bits()
  CopyMemory LoadIcon32Mem.bits(1, 1), Data(1 + Len(id) + Len(IDE) + Len(bih)), LoadIcon32Mem.Width * LoadIcon32Mem.Height * 4
'Close F
End Function

Public Function GetIcon32Handle(SPR As Sprite32) As Long
On Error Resume Next
Dim i As Long, L As Long
i = 1
Do
  If FindIcon32Handle(i) = 0 Then Exit Do
  i = i + 1
Loop

L = UBound(IconHandles)
L = L + 1
ReDim Preserve IconHandles(1 To L) As IconsHandles
IconHandles(L).handle = i
IconHandles(L).Icon = SPR
GetIcon32Handle = i
End Function

Public Function FindIcon32Handle(handle As Long) As Long
On Error Resume Next
Dim i As Long, IC As Long

IC = UBound(IconHandles)
For i = 1 To IC
  If IconHandles(i).handle = handle Then FindIcon32Handle = i: Exit Function
Next
End Function

Public Function DrawSprite32(SPR As Sprite32, hdc As Long, Optional x As Long, Optional y As Long) As Long
Dim tDC As Long, tBitmap As Long, tBO As Long, BitsPtr As Long, SA As SAFEARRAY2D
Dim SPR24 As Sprite24

tDC = CreateCompatibleDC(0)
tBitmap = CreateDIB(tDC, SPR.Width, SPR.Height, BitsPtr, 24)
tBO = SelectObject(tDC, tBitmap)
BitBlt tDC, 0, 0, SPR.Width, SPR.Height, hdc, x, y, vbSrcCopy

'ReDim SPR24.Bits(1 To RGBWidth(SPR.Width), 1 To SPR.Height) As Byte
SPR24.Width = SPR.Width
SPR24.Height = SPR.Height

With SA
  .cbElements = 1
  .cDims = 2
  .Bounds(0).lLbound = 1
  .Bounds(0).cElements = SPR.Height
  .Bounds(1).lLbound = 1
  .Bounds(1).cElements = RGBWidth(SPR.Width)
  .pvData = BitsPtr
End With

CopyMemory ByVal VarPtrArray(SPR24.bits()), VarPtr(SA), 4
BitBlt32to24C SPR24, 0, 0, SPR.Width, SPR.Height, SPR, 0, 0
BitBlt hdc, x, y, SPR.Width, SPR.Height, tDC, 0, 0, vbSrcCopy


CopyMemory ByVal VarPtrArray(SPR24.bits()), CLng(0), 4
DeleteObject SelectObject(tDC, tBO)
DeleteObject tBitmap
DeleteDC tDC
End Function

Public Function DrawSprite24(SPR As Sprite24, hdc As Long, Optional x As Long, Optional y As Long) As Long
Dim tDC As Long, tBitmap As Long, tBO As Long, BitsPtr As Long, SA As SAFEARRAY2D
Dim SPR24 As Sprite24

tDC = CreateCompatibleDC(0)
tBitmap = CreateDIB(tDC, SPR.Width, SPR.Height, BitsPtr, 24)
tBO = SelectObject(tDC, tBitmap)
BitBlt tDC, 0, 0, SPR.Width, SPR.Height, hdc, x, y, vbSrcCopy

'ReDim SPR24.Bits(1 To RGBWidth(SPR.Width), 1 To SPR.Height) As Byte
SPR24.Width = SPR.Width
SPR24.Height = SPR.Height

With SA
  .cbElements = 1
  .cDims = 2
  .Bounds(0).lLbound = 1
  .Bounds(0).cElements = SPR.Height
  .Bounds(1).lLbound = 1
  .Bounds(1).cElements = RGBWidth(SPR.Width)
  .pvData = BitsPtr
End With

CopyMemory ByVal VarPtrArray(SPR24.bits()), VarPtr(SA), 4
BitBlt24to24C SPR24, 0, 0, SPR.Width, SPR.Height, SPR, 0, 0
BitBlt hdc, x, y, SPR.Width, SPR.Height, tDC, 0, 0, vbSrcCopy


CopyMemory ByVal VarPtrArray(SPR24.bits()), CLng(0), 4
DeleteObject SelectObject(tDC, tBO)
DeleteObject tBitmap
DeleteDC tDC
End Function

Public Sub VertivalFlipSprite24(dest As Sprite24)
Dim W As Long, ms() As Byte, i As Long, IC As Long, MC As Long, k As Long

W = UBound(dest.bits, 1)
ReDim ms(1 To W) As Byte

MC = UBound(dest.bits, 2)
IC = (MC \ 2) - 1
k = W
For i = 0 To IC
  CopyMemoryC ms(1), dest.bits(1, i + 1), k
  CopyMemoryC dest.bits(1, i + 1), dest.bits(1, MC - i), k
  CopyMemoryC dest.bits(1, MC - i), ms(1), k
Next
End Sub

Public Function Split2(ByVal Text As String, Delimiter As String, Optional S1 As String, Optional S2 As String, Optional Compare As VbCompareMethod = vbBinaryCompare) As Long
Dim i As Long
S1 = "": S2 = ""
i = InStr(1, Text, Delimiter, Compare)
If i = 0 Then S1 = Text: Exit Function
If i = 1 And Len(Delimiter) = Len(Text) Then Exit Function
If i = 1 Then S2 = Mid$(Text, Len(Delimiter) + 1): Exit Function
If i + Len(Delimiter) = Len(Text) + 1 Then S1 = Mid$(Text, 1, i - 1): Exit Function
S1 = Mid$(Text, 1, i - 1): S2 = Mid$(Text, i + Len(Delimiter))
End Function

Public Function AddAlpha32(SPR As Sprite32, ByVal Add As Long, Optional ByVal x As Long, Optional ByVal y As Long, Optional ByVal W As Long = -1, Optional ByVal h As Long = -1) As Long
If W = -1 Then W = SPR.Width
If h = -1 Then h = SPR.Height
AlphaAdd32C SPR, x, y, W, h, Add
End Function

Public Function CreateEmptytPicture32(ByVal Width As Long, ByVal Height As Long, ByVal Ptr As Long, Optional AlwaysMem As Boolean) As tPicture
Dim MaskColor As Long, hPal As Long
Dim cBk As Long, cText As Long
Dim tDC As Long, tBitmap As Long, tBO As Long, BPS As Long

hDcScreen = CreateCompatibleDC(0) 'GetDC(0&)
hPal = CreateHalftonePalette(hDcScreen)
'OleTranslateColor TColor, hPal, MaskColor

CreateEmptytPicture32.Width = Width
CreateEmptytPicture32.Height = Height
CreateEmptytPicture32.PDC = CreateCompatibleDC(0)
BPS = GetDeviceCaps(CreateEmptytPicture32.PDC, BITSPIXEL)

If BPS = 32 And Not AlwaysMem Then
  VertivalFlipPtr32 Ptr, Width, Height
  CreateEmptytPicture32.pBitmap = CreateBitmap(CreateEmptytPicture32.Width, CreateEmptytPicture32.Height, 1, BPS, ByVal Ptr)
  CreateEmptytPicture32.OldpBitmap = SelectObject(CreateEmptytPicture32.PDC, CreateEmptytPicture32.pBitmap)
Else
  CreateEmptytPicture32.pBitmap = CreateDIB(CreateEmptytPicture32.PDC, CreateEmptytPicture32.Width, CreateEmptytPicture32.Height, CreateEmptytPicture32.pBitmapBits, 32)
  CreateEmptytPicture32.OldpBitmap = SelectObject(CreateEmptytPicture32.PDC, CreateEmptytPicture32.pBitmap)
  If Ptr <> 0 Then CopyMemory ByVal CreateEmptytPicture32.pBitmapBits, ByVal Ptr, Width * Height * 4
End If
End Function

Public Sub VertivalFlipPtr32(Ptr As Long, W As Long, h As Long)
Dim ms() As Long, i As Long, IC As Long, MC As Long, k As Long

ReDim ms(1 To W) As Long

MC = h
IC = (MC \ 2) - 1
k = W * 4
For i = 0 To IC
  CopyMemoryC ms(1), ByVal (Ptr + i * k), k
  CopyMemoryC ByVal (Ptr + i * k), ByVal (Ptr + (MC - (i + 1)) * k), k
  CopyMemoryC ByVal (Ptr + (MC - (i + 1)) * k), ms(1), k
Next
End Sub

Public Sub VertivalFlipSprite32(dest As Sprite32)
Dim W As Long, ms() As Long, i As Long, IC As Long, MC As Long, k As Long

W = UBound(dest.bits, 1)
ReDim ms(1 To W) As Long

MC = UBound(dest.bits, 2)
IC = (MC \ 2) - 1
k = W * 4
For i = 0 To IC
  CopyMemoryC ms(1), dest.bits(1, i + 1), k
  CopyMemoryC dest.bits(1, i + 1), dest.bits(1, MC - i), k
  CopyMemoryC dest.bits(1, MC - i), ms(1), k
Next
End Sub

Public Function FillF(Ptr As Long, W As Long, h As Long) As Long
Dim i As Long, IC As Long, j As Long, JC As Long, B As Byte, ms() As Long

IC = W: JC = h
ReDim ms(1 To IC) As Long
For j = 1 To JC
  For i = 1 To IC
    'B = S.Bits(i, j).B
    'S.Bits(i, j).B = S.Bits(i, j).T
    'S.Bits(i, j).T = B
    ms(i) = &H2F888888
  Next
  'CopyMemory ByVal (Ptr + (j - 1) * IC * 4), MS(1), IC * 4
Next
End Function

Public Function CreatetPicture32FromTGA(FN As String, Optional Pos As Long = 1) As tPicture
Dim S As Sprite32, i As Long, j As Long
S = LoadTGA32(FN, Pos)

For i = 1 To S.Width
  For j = 1 To S.Height
    If S.bits(i, j).T < 255 Then
      S.bits(i, j).B = (CLng(S.bits(i, j).T) * CLng(S.bits(i, j).B)) \ 255
      S.bits(i, j).g = (CLng(S.bits(i, j).T) * CLng(S.bits(i, j).g)) \ 255
      S.bits(i, j).r = (CLng(S.bits(i, j).T) * CLng(S.bits(i, j).r)) \ 255
    End If
  Next
Next

FillF VarPtr(S.bits(1, 1)), S.Width, S.Height

CreatetPicture32FromTGA = CreateEmptytPicture32(S.Width, S.Height, VarPtr(S.bits(1, 1)))
'CreatetPicture32FromTGA = CreateEmptytPicture32(320, 240, 0)

Exit Function


Dim tDC As Long, tBitmap As Long, tBO As Long, BitsPtr As Long ', SA As SAFEARRAY2D
'Dim SPR As Sprite32

tDC = CreateCompatibleDC(0)
tBitmap = CreateDIB(tDC, S.Width, S.Height, BitsPtr, 32)
tBO = SelectObject(tDC, tBitmap)
'BitBlt tDC, 0, 0, SPR.Width, SPR.Height, hdc, X, Y, vbSrcCopy

'SPR32.Width = SPR.Width
'SPR32.Height = SPR.Height

'With SA
'  .cbElements = 1
'  .cDims = 2
'  .Bounds(0).lLbound = 1
'  .Bounds(0).cElements = S.Height
'  .Bounds(1).lLbound = 1
'  .Bounds(1).cElements = S.Width * 4
'  .pvData = BitsPtr
'End With

'CopyMemory ByVal VarPtrArray(SPR24.Bits()), VarPtr(SA), 4
CopyMemory ByVal BitsPtr, S.bits(1, 1), S.Width * S.Height * 4

BitBltA CreatetPicture32FromTGA.PDC, x, y, S.Width, S.Height, tDC, 0, 0, S.Width, S.Height, 255


'CopyMemory ByVal VarPtrArray(SPR24.Bits()), CLng(0), 4
DeleteObject SelectObject(tDC, tBO)
DeleteObject tBitmap
DeleteDC tDC
End Function

Public Sub ApplytPicture32ToFormXP(tPic As tPicture, ByVal hWnd As Long)
Dim h As Long, BF As blendFunction, P As POINTAPI, p1 As POINTAPI

P.x = tPic.Width
P.y = tPic.Height

SetWindowPos hWnd, 0, 0, 0, tPic.Width, tPic.Height, &H10 Or &H2 Or &H200 Or &H4

BF.AlphaFormat = 1
BF.BlendOp = 0
BF.SourceConstantAlpha = 255
'Call UpdateLayeredWindow(hwnd, 0, VarPtr(P1), VarPtr(P), tPic.PDC, VarPtr(P1), 0, BF, 2)
Call UpdateLayeredWindow(hWnd, 0, 0, VarPtr(P), tPic.PDC, VarPtr(p1), 0, BF, 2)
RedrawWindow hWnd, 0, 0, RDW_ERASE Or RDW_INVALIDATE Or RDW_FRAME Or RDW_ALLCHILDREN

End Sub

Public Function BitBltA(ByVal hDestDC As Long, ByVal x As Long, ByVal y As Long, ByVal nWidthDest As Long, ByVal nHeightDest As Long, ByVal hSrcDC As Long, ByVal xSrc As Long, ByVal ySrc As Long, ByVal nWidthSrc As Long, ByVal nHeightSrc As Long, alpha As Byte) As Long
Dim AF As blendFunction, L As Long

'AC_SRC_OVER=0
'AC_SRC_ALPHA=1
AF.AlphaFormat = 1
AF.SourceConstantAlpha = alpha
AF.BlendOp = 0
AF.BlendFlags = 0
CopyMemory L, AF, 4
Call AlphaBlendA(hDestDC, x, y, nWidthDest, nHeightDest, hSrcDC, xSrc, ySrc, nWidthSrc, nHeightSrc, ByVal L)
End Function

Public Function GetComment(FN As String) As String
On Error Resume Next
Dim S As String, IC As Long, Arr() As String, i As Long, S2 As String
Dim sFN As String, L As Long

sFN = GetFileNameIN(FN)
S = LoadFile(GetPathOnlyIN(FN) & "descript.ion")
IC = MySplit(Arr(), S)
For i = 1 To IC
  If Len(Trim$(Arr(i))) > 0 Then
    If Mid$(Arr(i), 1, 1) = Chr$(34) Then
      L = InStr(2, Arr(i), Chr$(34))
      S = "": S2 = ""
      If L > 2 Then
        S = Mid$(Arr(i), 2, L - 2)
        S2 = Mid$(Arr(i), L + 2)
      End If
    Else
      Split2 Arr(i), " ", S, S2
    End If
    If StrComp(sFN, S, vbTextCompare) = 0 Then GetComment = S2: Exit Function
  End If
Next
End Function

Public Function SetComment(FN As String, Comment As String) As Long
On Error Resume Next
Dim S As String, IC As Long, Arr() As String, i As Long, S2 As String
Dim sFN As String, L As Long, f As String, AD As Boolean

sFN = GetFileNameIN(FN)
S = LoadFile(GetPathOnlyIN(FN) & "descript.ion")
f = FreeFile
Open GetPathOnlyIN(FN) & "descript.ion" For Output As #f
IC = MySplit(Arr(), S)
For i = 1 To IC
  If Len(Trim$(Arr(i))) > 0 Then
    If Mid$(Arr(i), 1, 1) = Chr$(34) Then
      L = InStr(2, Arr(i), Chr$(34))
      S = "": S2 = ""
      If L > 2 Then
        S = Mid$(Arr(i), 2, L - 2)
        S2 = Mid$(Arr(i), L + 2)
      End If
    Else
      Split2 Arr(i), " ", S, S2
    End If
    If StrComp(sFN, S, vbTextCompare) = 0 Then
      Arr(i) = Chr$(34) & S & Chr$(34) & " " & Comment
      AD = True
    End If
    Print #f, Arr(i)
  End If
Next
If Not AD Then
  Print #f, Chr$(34) & GetFileNameIN(FN) & Chr$(34) & " " & Comment
End If
Close #f
End Function

Public Function TTrim(Text As String) As String
Dim B1 As Long, b2 As Long, i As Long, IC As Long, c As Long

IC = Len(Text)
For B1 = 1 To IC
  c = Asc(Mid$(Text, B1, 1))
  If Not ((c = 32) Or (c = 9)) Then GoTo FOR2
Next
FOR2:

For b2 = IC To 1 Step -1
  c = Asc(Mid$(Text, b2, 1))
  If Not ((c = 32) Or (c = 9)) Then Exit For
Next
If (B1 > b2) Or (B1 = 0) Or (b2 = 0) Then Exit Function
TTrim = Mid$(Text, B1, (b2 - B1) + 1)
End Function

Public Function InWork() As Boolean
If CheckFile("C:\.work") = 1 Then InWork = True
End Function

Public Function MyMkDir(Path As String) As Long
On Error Resume Next
Dim i As Long, IC As Long, Arr() As String, CP As String

IC = MySplit(Arr(), Path, "\")
For i = 1 To IC
  AddToString CP, Arr(i), "\"
  MkDir CP
Next
End Function

Public Function Hex2(ByVal Value As Byte) As String
Hex2 = Hex$(Value)
If Len(Hex2) < 2 Then Hex2 = "0" & Hex2
End Function

Public Function LongHex(Text As String) As String
Dim i As Long, IC As Long

IC = Len(Text)
LongHex = String(IC * 2, " ")
For i = 1 To IC
  Mid$(LongHex, i * 2 - 1, 2) = Hex2(Asc(Mid$(Text, i, 1)))
Next
End Function

Public Function LongUnHex4(Text As String) As String
On Error Resume Next
Dim i As Long, IC As Long, N As Integer, S As String * 2

IC = Len(Text) \ 4
LongUnHex4 = String(IC * 2, " ")
For i = 1 To IC
  CopyMemory ByVal S, CInt(Val("&h" & Mid$(Text, i * 4 - 1, 4))), 2
  Mid$(LongUnHex4, i * 2 - 1, 2) = S
Next
End Function

Public Function LongUnHex(Text As String) As String
Dim i As Long, IC As Long

IC = Len(Text) \ 2
LongUnHex = String(IC, " ")
For i = 1 To IC
  Mid$(LongUnHex, i, 1) = Chr$(Val(UnHex2(Mid$(Text, i * 2 - 1, 2))))
Next
End Function

Public Function MyInStr(Pos As Long, String1 As String, String2 As String) As Long
On Error Resume Next
Dim i As Long, IC As Long, JC As Long

If Pos < 0 Then Exit Function

IC = Len(String1)
JC = Len(String2)
If Pos > IC Then Exit Function

IC = (IC - (JC - 1)) - (Pos - 1)
For i = 1 To IC
  If BinaryCompare(Mid$(String1, (Pos - 1) + i, JC), String2) Then MyInStr = (Pos - 1) + i: Exit Function
Next
End Function

Public Function BinaryCompare(S1 As String, S2 As String) As Boolean
Dim i As Long, IC As Long

IC = Len(S1)
For i = 1 To IC
  If Not Asc(Mid$(S1, i, 1)) = Asc(Mid$(S2, i, 1)) Then Exit Function
Next
BinaryCompare = True
End Function

Public Function CreateIconFromPBDC(PB As PictureBox) As Long
Dim ICN As Long, INF As ICONINFO, hMask As Long, ms() As Byte

ReDim ms(0 To PB.ScaleWidth * PB.ScaleHeight / 4) As Byte
hMask = CreateBitmap(PB.ScaleWidth, PB.ScaleHeight, 1, 1, ms(0))

INF.fIcon = 1
INF.hBmMask = hMask
INF.hBmColor = PB.Image.handle

ICN = CreateIconIndirect(INF)

DeleteObject hMask
CreateIconFromPBDC = ICN
'If Modify = 0 Then
  'TrayAdd TrayP, Label1.Caption, ICN
'Else
'  TrayModify TrayP, Label1.Caption, ICN
'End If

'DestroyIcon ICN
End Function

Public Function GetTrayIconW() As Long
GetTrayIconW = GetSystemMetrics(SM_CXSMICON)
End Function

Public Function GetTrayIconH() As Long
GetTrayIconH = GetSystemMetrics(SM_CYSMICON)
End Function


Public Function SetWindowRect(hWnd As Long, wRect As RECT) As Long

    SetWindowRect = SetWindowPos(hWnd, &H0, wRect.Left, wRect.Top, wRect.Right - wRect.Left, wRect.Bottom - wRect.Top, &H10)

End Function

Function GetScreen() As RECT

Dim UZ As RECT, B As Long
B = SystemParametersInfo(SPI_GETWORKAREA, 0, UZ, WM_SETTINGCHANGE)
GetScreen = UZ

End Function
Public Function IsFullScreen() As Boolean

Dim g As Long, h As RECT, i As RECT

g = GetForegroundWindow()
h = GetScreen
Call GetWindowRect(g, i)

If h.Right - h.Left <= i.Right - i.Left And h.Bottom - h.Top <= i.Bottom - i.Top Then IsFullScreen = True


End Function

Public Sub ResSound(ByVal ResourceId As Integer)
    Dim ret As Variant, SoundBuffer As String
    'возвращаемая строка конвертируется к Уникоду
    SoundBuffer = StrConv(LoadResData(ResourceId, "CUSTOM"), vbUnicode)
    ret = sndPlaySound(SoundBuffer, 1 Or 2 Or 4)
    DoEvents 'Важно: эта функция необходима для асинхронно играющего звука
End Sub


Public Function GetResFile(ByVal ResourceId As Integer)

Dim i As Integer
Dim temppath As String
Dim slength As Long
temppath = Space(255)
slength = GetTempPath(255, temppath)
temppath = Left(temppath, slength)

i = FreeFile
SaveFile DirFilterIN(temppath) & "\tmpfile", StrConv(LoadResData(ResourceId, "CUSTOM"), vbUnicode)

GetResFile = DirFilterIN(temppath) & "\tmpfile"

End Function


Function GetWorkArea() As RECT
Dim r As RECT
Call SystemParametersInfo(SPI_GETWORKAREA, 0, r, 0)
GetWorkArea = r
End Function

Function SetWorkArea(inArea As RECT)

    Call SystemParametersInfo(SPI_SETWORKAREA, 0, inArea, 0)

End Function


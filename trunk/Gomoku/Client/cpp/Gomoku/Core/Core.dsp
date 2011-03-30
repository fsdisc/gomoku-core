# Microsoft Developer Studio Project File - Name="Core" - Package Owner=<4>
# Microsoft Developer Studio Generated Build File, Format Version 6.00
# ** DO NOT EDIT **

# TARGTYPE "Win32 (x86) Static Library" 0x0104

CFG=Core - Win32 Debug
!MESSAGE This is not a valid makefile. To build this project using NMAKE,
!MESSAGE use the Export Makefile command and run
!MESSAGE 
!MESSAGE NMAKE /f "Core.mak".
!MESSAGE 
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE 
!MESSAGE NMAKE /f "Core.mak" CFG="Core - Win32 Debug"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "Core - Win32 Release" (based on "Win32 (x86) Static Library")
!MESSAGE "Core - Win32 Debug" (based on "Win32 (x86) Static Library")
!MESSAGE 

# Begin Project
# PROP AllowPerConfigDependencies 0
# PROP Scc_ProjName ""
# PROP Scc_LocalPath ""
CPP=cl.exe
RSC=rc.exe

!IF  "$(CFG)" == "Core - Win32 Release"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 0
# PROP BASE Output_Dir "Release"
# PROP BASE Intermediate_Dir "Release"
# PROP BASE Target_Dir ""
# PROP Use_MFC 0
# PROP Use_Debug_Libraries 0
# PROP Output_Dir "Release"
# PROP Intermediate_Dir "Release"
# PROP Target_Dir ""
# ADD BASE CPP /nologo /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_MBCS" /D "_LIB" /YX /FD /c
# ADD CPP /nologo /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_MBCS" /D "_LIB" /YX /FD /c
# ADD BASE RSC /l 0x409 /d "NDEBUG"
# ADD RSC /l 0x409 /d "NDEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo /o"Release/gomoku-core.bsc"
LIB32=link.exe -lib
# ADD BASE LIB32 /nologo
# ADD LIB32 /nologo /out:"Release\gomoku-core.lib"

!ELSEIF  "$(CFG)" == "Core - Win32 Debug"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 1
# PROP BASE Output_Dir "Debug"
# PROP BASE Intermediate_Dir "Debug"
# PROP BASE Target_Dir ""
# PROP Use_MFC 0
# PROP Use_Debug_Libraries 1
# PROP Output_Dir "Debug"
# PROP Intermediate_Dir "Debug"
# PROP Target_Dir ""
# ADD BASE CPP /nologo /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_MBCS" /D "_LIB" /YX /FD /GZ /c
# ADD CPP /nologo /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_MBCS" /D "_LIB" /YX /FD /GZ /c
# ADD BASE RSC /l 0x409 /d "_DEBUG"
# ADD RSC /l 0x409 /d "_DEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo /o"Debug/gomoku-core.bsc"
LIB32=link.exe -lib
# ADD BASE LIB32 /nologo
# ADD LIB32 /nologo /out:"Debug\gomoku-core.lib"

!ENDIF 

# Begin Target

# Name "Core - Win32 Release"
# Name "Core - Win32 Debug"
# Begin Group "Source Files"

# PROP Default_Filter "cpp;c;cxx;rc;def;r;odl;idl;hpj;bat"
# Begin Source File

SOURCE=.\Source\Board.cpp
# End Source File
# Begin Source File

SOURCE=.\Source\BoardUI.cpp
# End Source File
# Begin Source File

SOURCE=.\Source\Cell.cpp
# End Source File
# Begin Source File

SOURCE=.\Source\Client.cpp
# End Source File
# Begin Source File

SOURCE=.\Source\ComputerPlayer.cpp
# End Source File
# Begin Source File

SOURCE=.\Source\Config.cpp
# End Source File
# Begin Source File

SOURCE=.\Source\Game.cpp
# End Source File
# Begin Source File

SOURCE=.\Source\GameState.cpp
# End Source File
# Begin Source File

SOURCE=.\Source\HumanPlayer.cpp
# End Source File
# Begin Source File

SOURCE=.\Source\Move.cpp
# End Source File
# Begin Source File

SOURCE=.\Source\MoveListener.cpp
# End Source File
# Begin Source File

SOURCE=.\Source\Player.cpp
# End Source File
# Begin Source File

SOURCE=.\Source\PlayerRunner.cpp
# End Source File
# Begin Source File

SOURCE=.\Source\RemotePlayer.cpp
# End Source File
# Begin Source File

SOURCE=.\Source\Runner.cpp
# End Source File
# Begin Source File

SOURCE=.\Source\WebClient.cpp
# End Source File
# End Group
# Begin Group "Header Files"

# PROP Default_Filter "h;hpp;hxx;hm;inl"
# Begin Source File

SOURCE=.\Header\Board.h
# End Source File
# Begin Source File

SOURCE=.\Header\BoardUI.h
# End Source File
# Begin Source File

SOURCE=.\Header\Cell.h
# End Source File
# Begin Source File

SOURCE=.\Header\Client.h
# End Source File
# Begin Source File

SOURCE=.\Header\ComputerPlayer.h
# End Source File
# Begin Source File

SOURCE=.\Header\Config.h
# End Source File
# Begin Source File

SOURCE=.\Header\Game.h
# End Source File
# Begin Source File

SOURCE=.\Header\GameState.h
# End Source File
# Begin Source File

SOURCE=.\Header\HumanPlayer.h
# End Source File
# Begin Source File

SOURCE=.\Header\Move.h
# End Source File
# Begin Source File

SOURCE=.\Header\MoveListener.h
# End Source File
# Begin Source File

SOURCE=.\Header\Player.h
# End Source File
# Begin Source File

SOURCE=.\Header\PlayerRunner.h
# End Source File
# Begin Source File

SOURCE=.\Header\RemotePlayer.h
# End Source File
# Begin Source File

SOURCE=.\Header\Runner.h
# End Source File
# Begin Source File

SOURCE=.\Header\WebClient.h
# End Source File
# End Group
# Begin Group "Private Files"

# PROP Default_Filter ""
# Begin Source File

SOURCE=.\Private\Socket.cpp
# End Source File
# Begin Source File

SOURCE=.\Private\Socket.h
# End Source File
# End Group
# Begin Source File

SOURCE=.\ReadMe.txt
# End Source File
# End Target
# End Project

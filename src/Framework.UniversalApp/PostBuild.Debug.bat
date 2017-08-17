ECHO OFF
REM Usage: Call "$(ProjectDir)PostBuild.$(ConfigurationName).bat" "$(ProjectDir)" "$(OutDir)" "$(TargetPath)" "$(RootNameSpace)" "$(ConfigurationName)"
REM Vars:  $(TargetPath) = output file, $(TargetDir) = full bin path , $(OutDir) = bin\debug, $(ConfigurationName) = "Debug"

REM Locals
SET LibFolder=\lib\GenesysFramework
SET FullPath=%1%2
SET FullPath=%FullPath:"=%
SET FullPath="%FullPath%"
SET RootNamespace=%4
SET RootNamespace=%RootNamespace:"=%
SET Configuration=%5
if "%Configuration%"=="" SET Configuration="Debug"
ECHO Configuration: %Configuration%

REM Copying project output to build location
Echo FullPath: %FullPath% 
Echo 3: %3
Echo RootNamespace: %RootNamespace%
Echo   to %LibFolder%

MD %LibFolder%
%WINDIR%\system32\attrib.exe %LibFolder%\*.* -r /s
%WINDIR%\system32\xcopy.exe %FullPath%.* %LibFolder%\*.* /f/s/e/r/c/y
%WINDIR%\system32\xcopy.exe %1Properties\*.rd.xml %LibFolder%\%RootNamespace%\Properties\*.* /s/r/y
%WINDIR%\system32\xcopy.exe %FullPath%*.png %LibFolder%\%RootNamespace%\*.* /s/r/y
%WINDIR%\system32\xcopy.exe %FullPath%*.xbf %LibFolder%\%RootNamespace%\*.* /s/r/y
%WINDIR%\system32\xcopy.exe %FullPath%*.xml %LibFolder%\%RootNamespace%\*.* /s/r/y
%WINDIR%\system32\xcopy.exe %3 %LibFolder%\*.* /s/r/y
%WINDIR%\system32\xcopy.exe %FullPath%*.pri %LibFolder%\*.* /s/r/y

REM Enable if want to copy all related Genesys dependencies to \lib (even those not of this Solution) 
REM %WINDIR%\system32\xcopy.exe "%PartialPath%Genesys.*" "%LibFolder%\*.*" /f/s/e/r/c/y

Exit 0
SET PATH=%ProgramFiles(x86)%\MSBuild\12.0\Bin;%PATH%
msbuild.exe SystemMock\SystemMock.csproj /p:Configuration="Release 4.0" /t:Build
msbuild.exe SystemMock\SystemMock.csproj /p:Configuration="Release 4.5" /t:Build;Package;Publish

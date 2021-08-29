Steps to install this plugin to Resharper:

1. Add this this entry to Extension Manager section in Resharper Options Window.

Name:   MemberOrderInspection
Source: E:\Alem\Program\Yazýlým\ReSharper\MemberOrderInspection\Atesh.MemberOrderInspection\bin

2. Build this project.

2.a. Make sure that bin\net471\Atesh.MemberOrderInspection.dll is included in this project. Otherwise "Pack" command won't include the compiled dll into the package for an unknown reason.

2.b. Including the dll into this project must be done manually in the csproj text file.

  <ItemGroup>
    <Content Include="bin\net471\Atesh.MemberOrderInspection.dll" PackagePath="dotFiles" Pack="true" />
  </ItemGroup>

2.c. If the project's target framework is upgraded (ex: net461 to net471) the dll include line in csproj must be updated with the correct folder name manually.

3. "Pack" this project by right clicking on the project in Visual Studio.

3.a. If the package version isn't increased manually, Resharper won't recognize the package as updateable so the plugin needs to be uninstalled and re-installed.
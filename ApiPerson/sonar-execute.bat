lembrar de rodar isso no developer prompt
dotnet tool install --global dotnet-sonarscanner --version 5.0.4
dotnet sonarscanner begin /k:"ApiPerson" /d:sonar.cs.opencover.reportsPaths="%CD%\opencover.xml"
dotnet build ApiPerson.sln
%USERPROFILE%\.nuget\packages\opencover\4.6.519\tools\Opencover.Console.exe -output:"%CD%\opencover.xml" -register:user -target:"C:\Program Files\dotnet\dotnet.exe" -targetargs:"test C:\Users\julia\Desktop\Treinamento\api-person\ApiPerson.Test\ApiPerson.Test.csproj" -oldstyle
dotnet sonarscanner end

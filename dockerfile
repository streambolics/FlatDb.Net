FROM microsoft/aspnetcore:2.0
COPY ./bin/amd64/ ./
ENTRYPOINT ["dotnet", "flatdatabase.dll"]


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish QuizSvc/QuizSvc.csproj -c Release -o /app/publish /p:UseAppHost=false
#Sửa 'Services' thành project name, file .csproj phải nằm trong folder chính 

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
# EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "QuizSvc.dll"]
#Sửa 'Services' thành project name
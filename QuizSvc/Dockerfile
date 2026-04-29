#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build8
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["QuizSvc/QuizSvc.csproj", "QuizSvc/"]
COPY ["QuizSvc.Application/QuizSvc.Application.csproj", "QuizSvc.Application/"]
COPY ["QuizSvc.Domain/QuizSvc.Domain.csproj", "QuizSvc.Domain/"]
COPY ["QuizSvc.Infrastructure/QuizSvc.Infrastructure.csproj", "QuizSvc.Infrastructure/"]
COPY ["QuizSvc.Protos/QuizSvc.Protos.csproj", "QuizSvc.Protos/"]
COPY ["QuizSvc.Share/QuizSvc.Share.csproj", "QuizSvc.Share/"]
RUN dotnet restore "./QuizSvc/QuizSvc.csproj"
COPY . .
WORKDIR "/src/QuizSvc"
RUN dotnet build "./QuizSvc.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build8 AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./QuizSvc.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuizSvc.dll"]
FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY src/Message.Integration.Api/Message.Integration.Api.csproj src/Message.Integration.Api/
COPY src/Message.Integration.Aliyun/Message.Integration.Aliyun.csproj src/Message.Integration.Aliyun/
RUN dotnet restore src/Message.Integration.Api/Message.Integration.Api.csproj
COPY . .
WORKDIR /src/src/Message.Integration.Api
RUN dotnet build Message.Integration.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Message.Integration.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Message.Integration.Api.dll"]

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Totolist.Backend/Totolist.Backend.csproj", "Totolist.Backend/"]
RUN dotnet restore "Totolist.Backend/Totolist.Backend.csproj"
COPY . .
WORKDIR "/src/Totolist.Backend"
RUN dotnet build "Totolist.Backend.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Totolist.Backend.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Totolist.Backend.dll"]
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /Hainz

COPY ./src ./src
COPY ./Hainz.sln .

RUN dotnet restore
RUN dotnet publish ./src/Hainz/Hainz.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /Hainz
COPY --from=build /Hainz/out .
ENTRYPOINT ["dotnet", "Hainz.dll"]
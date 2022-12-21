FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /Hainz

COPY ./src .

RUN dotnet restore ./Hainz/Hainz.csproj
RUN dotnet publish ./Hainz/Hainz.csproj -c Debug -o out

FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /Hainz
COPY --from=build /Hainz/out .
ENTRYPOINT ["dotnet", "Hainz.dll"]
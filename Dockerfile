FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /Hainz

COPY ./src/Hainz/Hainz.csproj .
RUN dotnet restore

COPY .src/Hainz .
RUN dotnet publish -c Debug -o out

FROM mcr.microsoft.com/dotnet/runtime:7.0
WORKDIR /Hainz
COPY --from=build /Hainz/out .
ENTRYPOINT ["dotnet", "Hainz.dll"]
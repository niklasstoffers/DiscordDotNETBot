FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /Hainz

COPY ./src ./src
COPY ./Hainz.sln .

RUN dotnet restore
RUN dotnet publish ./src/Hainz/Hainz.csproj -c Release -o out


FROM mcr.microsoft.com/dotnet/runtime:6.0-alpine

RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

WORKDIR /Hainz
COPY --from=build /Hainz/out .
ENTRYPOINT ["dotnet", "Hainz.dll"]
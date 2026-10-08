FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY SmartBook/SmartBook.csproj SmartBook/
RUN dotnet restore SmartBook/SmartBook.csproj
COPY . .
RUN dotnet publish SmartBook/SmartBook.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:9.0-bookworm-slim
WORKDIR /app
COPY --from=build /app/publish .
RUN apt-get update && apt-get install -y wget \
  && wget -O /usr/local/bin/ttyd https://github.com/tsl0922/ttyd/releases/download/1.7.7/ttyd.x86_64 \
  && chmod +x /usr/local/bin/ttyd \
  && apt-get clean && rm -rf /var/lib/apt/lists/*
EXPOSE 7681
CMD sh -c "ttyd -W -p ${PORT:-7681} -t fontSize=20 -t titleFixed=SmartBook dotnet SmartBook.dll"

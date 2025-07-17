# Använd en liten Alpine-bas med .NET 9 runtime
FROM mcr.microsoft.com/dotnet/runtime:9.0-alpine AS runtime

# Installera ttyd (statiskt byggd binär för Alpine)
RUN apk add --no-cache ttyd

# Arbetskatalog
WORKDIR /app

# Kopiera hela publish-mappen
COPY publish .

# ttyd startar din app och exponerar port 7681
EXPOSE 7681
ENTRYPOINT ["ttyd", "-W", "-p", "7681", "./SmartBook"]
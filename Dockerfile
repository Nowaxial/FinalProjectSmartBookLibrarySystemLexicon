# 1. Liten Alpine-runtime
FROM mcr.microsoft.com/dotnet/runtime:9.0-alpine AS runtime
WORKDIR /app

# 2. Kopiera hela publish-mappen
COPY bin/publish/ .

# 3. Gör filen körbar (behövs på Linux)
RUN chmod +x SmartBook

# 4. ttyd ger webb-terminal
RUN apk add --no-cache ttyd

# 5. Exponera port 7681 och starta
EXPOSE 7681
ENTRYPOINT ["ttyd", "-W", "-p", "7681", "./SmartBook"]
FROM mcr.microsoft.com/dotnet/runtime:9.0-alpine AS runtime
WORKDIR /app

# Kopiera innehållet från din publish-mapp
# COPY FinalProjectSmartBookLibrarySystemLexicon/publish/ .
COPY /publish .

# Gör filen körbar
RUN chmod +x SmartBook.exe

# Lägg till ttyd för webb-terminal
RUN apk add --no-cache ttyd

EXPOSE 7681
ENTRYPOINT ["ttyd", "-W", "-p", "7681", "./SmartBook.exe"]
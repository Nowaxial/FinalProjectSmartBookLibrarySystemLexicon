FROM mcr.microsoft.com/dotnet/runtime:9.0-alpine
WORKDIR /app
COPY publish/ .
RUN chmod +x SmartBook
RUN apk add --no-cache ttyd
EXPOSE 7681
ENTRYPOINT ["ttyd", "-W", "-p", "7681", "./SmartBook"]
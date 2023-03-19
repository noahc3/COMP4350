# create API docker image from artifacts
FROM mcr.microsoft.com/dotnet/runtime-deps:7.0-alpine-amd64
LABEL org.opencontainers.image.source="https://github.com/noahc3/comp4350"
WORKDIR /app
COPY ./.docker/api/app-linux-musl-x64 ./
RUN chmod +x ./ThreaditAPI
CMD ["./ThreaditAPI"]
EXPOSE 80

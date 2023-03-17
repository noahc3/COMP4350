# create web docker image from artifacts
FROM steebchen/nginx-spa:stable
LABEL org.opencontainers.image.source="https://github.com/noahc3/comp4350"
COPY ./.docker/web/static /app
EXPOSE 80
CMD ["nginx"]
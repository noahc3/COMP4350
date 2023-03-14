# create web docker image from artifacts
FROM nginx
LABEL org.opencontainers.image.source="https://github.com/noahc3/comp4350"
COPY ./.docker/web/static /usr/share/nginx/html
EXPOSE 80
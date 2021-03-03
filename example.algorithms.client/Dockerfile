FROM registry.casperinc.net/node:14.15.5-alpine3.13 AS builder

WORKDIR /app

COPY .npmrc ./
COPY package.json ./
COPY tsconfig.json ./

RUN npm install

COPY ./src ./src
COPY ./public ./public
COPY .env.production ./

ARG deploy_env
RUN if [ "$deploy_env" = "dev" ]; then npm run build-dev; else if [ "$deploy_env" = "local"]; then npm run build-local; else npm run build ; fi ; fi

FROM registry.casperinc.net/nginx:alpine

WORKDIR /usr/share/nginx/html

RUN rm -rf ./*

COPY --from=builder /app/build .

ENTRYPOINT ["nginx", "-g", "daemon off;"]
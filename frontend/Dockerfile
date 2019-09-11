# Build image
FROM node:10 AS builder

WORKDIR /usr/src/app

COPY package*.json ./
RUN npm ci
COPY . .

RUN $(npm bin)/ng build --prod=true

# App image
FROM nginx:1.17

WORKDIR /usr/share/nginx/html

COPY --from=builder /usr/src/app/dist/awesome-bookstore .
COPY nginx.conf /etc/nginx/nginx.conf

# Use an official Node.js runtime as the base image
FROM node:latest
WORKDIR app
ARG APPDATA
COPY package*.json ./
COPY . .
RUN npm i
EXPOSE 3000
CMD ["npm", "run", "dev", "--", "--host"]

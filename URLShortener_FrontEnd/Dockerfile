# Use Node.js image to build the frontend
FROM node:18 AS build
WORKDIR /app

# Copy package.json and install dependencies
COPY package.json package-lock.json ./
RUN npm install

# Copy source files and build the Vue.js app
COPY . ./
RUN npm run build

# Use Nginx to serve the built frontend
FROM nginx:latest
COPY --from=build /app/dist /usr/share/nginx/html

# Expose port 80
EXPOSE 80

# Start Nginx server
CMD ["nginx", "-g", "daemon off;"]

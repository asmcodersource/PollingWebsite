import { fileURLToPath, URL } from 'node:url';
import mkcert from 'vite-plugin-mkcert'
import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import { env } from 'process';


const target = env.ASPNETCORE_HTTPS_PORT ? `https://mssql-server:${env.ASPNETCORE_HTTPS_PORT}` :
    env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:5001';

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [plugin(), mkcert()],
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        }
    },
    server: {
        proxy: {
            '^/weatherforecast': {
                target,
                secure: false
            }
        },
        port: 3000,
    },
})

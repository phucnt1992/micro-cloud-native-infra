
/** @type {import('next').NextConfig} */
const nextConfig = {
  swcMinify: true,
  compiler:{
    reactRemoveProperties: true,
    removeConsole: true,
  },
  experimental: {
    appDir: true,
  },
}

module.exports = nextConfig

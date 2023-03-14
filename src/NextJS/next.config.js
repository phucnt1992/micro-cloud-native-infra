
/** @type {import('next').NextConfig} */
const nextConfig = {
  swcMinify: true,
  // Disable strict mode for now, as it causes issues with some libraries (e.g. fluentui)
  reactStrictMode: false,
  compiler:{
    reactRemoveProperties: true,
    removeConsole: true,
  },
  experimental: {
    appDir: true,
  },
}

module.exports = nextConfig

import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  /* config options here */
  reactCompiler: true,
  images: {
    remotePatterns: [
      { hostname: "images.unsplash.com" },
      { hostname: "cdn-icons-png.flaticon.com" },
    ],
  },
  async rewrites() {
    return [
      {
        source: "/spring-server/:path*",
        destination: "http://localhost:8080/:path*",
      },
      {
        source: "/payment-service/:path*",
        destination: "http://localhost:5182/:path*",
      },
    ];
  },
};

export default nextConfig;

const { createProxyMiddleware } = require('http-proxy-middleware');

const apiProxy = createProxyMiddleware(["/api", "/img"], {
    target: 'https://localhost:7113',
    secure: false
});

const authProxy = createProxyMiddleware("/auth/api", {
    target: 'https://localhost:23422',
    secure: false,
    pathRewrite: {
        '^/auth/api': '/api' // Optional: rewrite path if needed
    }
});

module.exports = function (app) {
    app.use(apiProxy);
    app.use(authProxy);
};
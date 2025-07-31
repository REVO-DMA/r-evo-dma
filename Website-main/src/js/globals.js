const globals = {
    stripe: {
        publishableKey: "pk_live_51N7RRCIKeJsElTL1qgnJjZhl0hIYk24pOuUigL3S5YlwnBJWhzdiZkUqaOXvO5FAHSNpaqNzSmIOaVGWYJgPbDjJ00wNdEOIpU",
    },
    apiURL: "http://127.0.0.1:8081",
};

if (window.location.host === "evodma.com") {
    globals.apiURL = "https://api.evodma.com";
    globals.stripe.publishableKey = "pk_live_51N7RRCIKeJsElTL1qgnJjZhl0hIYk24pOuUigL3S5YlwnBJWhzdiZkUqaOXvO5FAHSNpaqNzSmIOaVGWYJgPbDjJ00wNdEOIpU";
} else if (window.location.host === "dev-web.evodma.com") {
    globals.apiURL = "https://dev-api.evodma.com";
}

export default globals;

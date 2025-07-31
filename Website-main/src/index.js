import "./css/index.sass";

import { initialize as initializeRouter } from "./js/router";
import { get as getSessionToken } from "./js/sessionManager";

(async () => {
    await getSessionToken(true);
    initializeRouter();
})();

// Lazy load Font Awesome
import("@fortawesome/fontawesome-pro/js/fontawesome").then(() => {
    import("@fortawesome/fontawesome-pro/js/solid");
    import("@fortawesome/fontawesome-pro/js/duotone");
    import("@fortawesome/fontawesome-pro/js/brands");
});

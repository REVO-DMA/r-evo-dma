import { routeManifest } from "./routes/routeManifest";
import { show as show404 } from "./routes/404/template";
import { show as showLoader, hide as hideLoader } from "./loader";

export function initialize() {
    handleRoute();

    window.addEventListener('popstate', handleRoute);
}

/**
 * @param {string} route
 */
export function navigate(route) {
    showLoader("Loading");
    history.pushState(null, "", route);
    handleRoute();
}

export async function handleRoute() {
    // Get the current URL and parse any query parameters
    const url = new URL(window.location.href);
    const currentUrl = url.pathname;
    const queryParams = Object.fromEntries(url.searchParams.entries());

    // Get the function corresponding to the current route
    const routeHandler = routeManifest[currentUrl];

    if (routeHandler) {
        await routeHandler(queryParams);
    } else {
        show404();
    }

    hideLoader();
}

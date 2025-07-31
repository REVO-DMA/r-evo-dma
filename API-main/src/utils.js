export function safeJSONParse(data) {
    try {
        return JSON.parse(data);
    } catch (error) {
        return false;
    }
}

/**
 * Determine whether or not a given route is in an array of routes.
 * @param {string?} route
 * @param {string[]} routes
 */
export function isRouteInRoutes(route, routes) {
    let result = false;

    if (route == null) return false;

    for (let i = 0; i < routes.length; i++) {
        const thisRoute = routes[i];

        if (route === thisRoute) {
            result = true;
            break;
        }
    }

    return result;
}

/**
 * @param {number} min
 * @param {number} max
 */
export function randomNumberInRange(min, max) {
    min = Math.ceil(min);
    max = Math.floor(max);
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

/**
 * @param {string} str
 */
export function extractVersion(str) {
    const regex = /(0|[1-9]\d*)\.(0|[1-9]\d*)(?:\.(0|[1-9]\d*))?(?:-((?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+([0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$/;
    const match = str.match(regex);
    if (match != null && match.length > 0) return match[0];
    else return null;
}

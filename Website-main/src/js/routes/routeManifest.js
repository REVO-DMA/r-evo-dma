import { show as showAccount } from "./account/template";
import { show as showAuth } from "./auth/template";
import { show as showCheckout } from "./checkout/template";
import { show as showDownloads } from "./downloads/template";
import { show as showLegal } from "./legal/template";
import { show as showStore } from "./store/template";

export const routeManifest = {
    "/auth": showAuth,
    "/account": showAccount,
    "/store": showStore,
    "/checkout": showCheckout,
    "/downloads": showDownloads,
    "/legal": showLegal
};

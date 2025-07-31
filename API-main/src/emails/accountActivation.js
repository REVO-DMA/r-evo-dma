import globals from "../globals.js";
import { account, getTemplate, transport } from "./emails.js";

/**
 * Compose and send an account activation email to the specified email.
 * @param {import("../database/tables/User/User.js").User} User The User instance.
 * @param {string} token The account activation token.
 */
export async function accountActivation(User, token) {
    console.log(`Sending account activation email to "${User.Email}"...`);

    const substitutedEmail = getTemplate("accountActivation", {
        username: User.Username,
        email: User.Email,
        link: `${globals.ui.url}/auth?action=account-activation&token=${token}&email=${User.Email}`,
    });

    try {
        transport
            .sendMail({
                from: `"EVO DMA" <${account.from}>`,
                to: User.Email,
                subject: "EVO DMA Account Activation",
                text: substitutedEmail,
            })
            .then(() => {
                console.log(`Successfully sent account activation email to "${User.Email}"!`);
            });
    } catch (error) {
        console.log(`Unable to send account activation email to "${User.Email}": ${error}`);
    }
}

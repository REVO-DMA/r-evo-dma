import globals from "../globals.js";
import { account, getTemplate, transport } from "./emails.js";

/**
 * Compose and send a password reset email to the specified email.
 * @param {import("../database/tables/User/User.js").User} User The User instance.
 * @param {string} token The password reset token.
 */
export async function passwordReset(User, token) {
    console.log(`Sending password reset email to "${User.Email}"...`);

    const substitutedEmail = getTemplate("forgotPassword", {
        username: User.Username,
        email: User.Email,
        link: `${globals.ui.url}/auth?action=password-reset&token=${token}&email=${User.Email}`,
    });

    try {
        transport
            .sendMail({
                from: `"EVO DMA" <${account.from}>`,
                to: User.Email,
                subject: "EVO DMA Password Reset",
                text: substitutedEmail,
            })
            .then(() => {
                console.log(`Successfully sent password reset email to "${User.Email}"!`);
            });
    } catch (error) {
        console.log(`Unable to send password reset email to "${User.Email}": ${error}`);
    }
}

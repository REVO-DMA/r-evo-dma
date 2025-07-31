import fs from "fs";
import { createTransport } from "nodemailer";
import path from "path";
import { fileURLToPath } from "url";
import globals from "../globals.js";

const __dirname = path.dirname(fileURLToPath(import.meta.url));

export const account = globals.email.noreply;

/** @type {import("nodemailer").Transporter} */
export let transport = null;

const templates = {};

export function initialize() {
    transport = createTransport({
        host: account.host,
        port: account.port,
        secure: account.secure,
        auth: {
            user: account.auth.user,
            pass: account.auth.pass,
        },
    });

    loadTemplates();
}

export function shutdown() {
    if (transport === null) return;

    transport.close();
}

/**
 * Loads all email templates into memory.
 */
function loadTemplates() {
    console.log("Loading email templates...");
    const files = fs.readdirSync(path.join(__dirname, "templates"));

    files.forEach((file) => {
        const parsedPath = path.parse(file);

        if (parsedPath.ext === ".tmpl") {
            const template = fs.readFileSync(path.join(__dirname, "templates", file)).toString();

            templates[parsedPath.name] = template;
        }
    });
}

/**
 * @param {string} templateName
 * @param {object} substitutions
 */
export function getTemplate(templateName, substitutions) {
    /** @type {string} */
    const template = templates[templateName];

    const regex = /{([^}]+)}/g;

    return template.replace(regex, (match, key) => substitutions[key.toLowerCase().trim()] || "");
}

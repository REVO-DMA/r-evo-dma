import { getCurrentWindow, clipboard } from "@electron/remote";

import { Terminal } from "xterm";
import { FitAddon } from "xterm-addon-fit";
import { SearchAddon } from "xterm-addon-search";
import { WebglAddon } from "xterm-addon-webgl";
import consoleColor from "ansi-colors";
import { DateTime } from "luxon";
import tippy from "tippy.js";
import { DEV_MODE } from "./env";
import { showPage } from "./pageHelper";

let terminal: Terminal;
let searchAddon: SearchAddon;

export enum LogStyle {
    dim,
    bold,
    hidden,
    italic,
    underline,
    inverse,
    strikethrough
};

export enum LogType {
    success,
    error,
    info,
    warn,
    debug,
    plain,
    total
};

const eLogStyleMap = new Map<LogStyle, consoleColor.StyleFunction>([
    [LogStyle.dim, consoleColor.dim],
    [LogStyle.bold, consoleColor.bold],
    [LogStyle.hidden, consoleColor.hidden],
    [LogStyle.italic, consoleColor.italic],
    [LogStyle.underline, consoleColor.underline],
    [LogStyle.inverse, consoleColor.inverse],
    [LogStyle.strikethrough, consoleColor.strikethrough],
]);

const eLogTypeMap = new Map([
    [LogType.success, consoleColor.greenBright("[SUCCESS]")],
    [LogType.error, consoleColor.redBright("[ERROR]")],
    [LogType.info, consoleColor.blueBright("[INFO]")],
    [LogType.warn, consoleColor.yellowBright("[WARN]")],
    [LogType.debug, DEV_MODE === true ? consoleColor.bgCyan("[DEBUG]") : null], // only output debug logs in dev env
    [LogType.plain, consoleColor.whiteBright("[LOG]")],
]);

const visibleConsoleLogCounts = new Set([
    LogType.total
]);

const consoleLogCount = new Map([
    [LogType.success, { val: 0 }],
    [LogType.error, { val: 0 }],
    [LogType.info, { val: 0 }],
    [LogType.warn, { val: 0 }],
    [LogType.debug, { val: 0 }],
    [LogType.plain, { val: 0 }],
    [LogType.total, { val: 0 }],
]);

const LogCountNumberEls = new Map([
    [LogType.success, document.getElementById("consoleLogCountNumber_success") as HTMLDivElement],
    [LogType.error, document.getElementById("consoleLogCountNumber_error") as HTMLDivElement],
    [LogType.info, document.getElementById("consoleLogCountNumber_info") as HTMLDivElement],
    [LogType.warn, document.getElementById("consoleLogCountNumber_warning") as HTMLDivElement],
    [LogType.debug, document.getElementById("consoleLogCountNumber_debug") as HTMLDivElement],
    [LogType.plain, document.getElementById("consoleLogCountNumber_plain") as HTMLDivElement],
    [LogType.total, document.getElementById("consoleLogCountNumber_total") as HTMLDivElement],
]);

const LogCountEls = new Map([
    [LogType.success, document.getElementById("consoleLogCount_success") as HTMLDivElement],
    [LogType.error, document.getElementById("consoleLogCount_error") as HTMLDivElement],
    [LogType.info, document.getElementById("consoleLogCount_info") as HTMLDivElement],
    [LogType.warn, document.getElementById("consoleLogCount_warning") as HTMLDivElement],
    [LogType.debug, document.getElementById("consoleLogCount_debug") as HTMLDivElement],
    [LogType.plain, document.getElementById("consoleLogCount_plain") as HTMLDivElement],
    [LogType.total, document.getElementById("consoleLogCount_total") as HTMLDivElement],
]);

export function initialize() {
    const consoleContainer = document.getElementById("consoleContainer");

    showPage(consoleContainer);

    const showHideTooltip = tippy("[data-tippy-content-console]")[0];
    showHideTooltip.setContent("Show");

    const consoleElement = document.getElementById("consoleElement");

    const cssDecl = getComputedStyle(document.documentElement);
    const fontFamily = cssDecl.getPropertyValue("--mono-font-family");
    const fontSize = Number(cssDecl.getPropertyValue("--text-size-sm").replace("px", ""));
    const textColor = cssDecl.getPropertyValue("--text-color");

    terminal = new Terminal({
        fontFamily: fontFamily,
        fontSize: fontSize,
        fontWeight: 400,
        letterSpacing: 1,
        lineHeight: 1,
        cursorStyle: "underline",
        disableStdin: true,
        scrollback: 2000,
        theme: {
            background: cssDecl.getPropertyValue("--primary-background-color"),
            foreground: textColor,
            selectionBackground: "#077bff",
            selectionForeground: textColor,
            cursor: textColor,
            cursorAccent: "#eb1c22",
        },
    });

    // terminal.options.theme.

    const fitAddon = new FitAddon();
    searchAddon = new SearchAddon();
    
    terminal.loadAddon(searchAddon);
    terminal.loadAddon(fitAddon);
    
    terminal.open(consoleElement);
    terminal.loadAddon(new WebglAddon());

    // Scroll to bottom
    const console_jumpToPresent = document.getElementById("console_jumpToPresent");
    console_jumpToPresent.addEventListener("click", () => {
        terminal.scrollToBottom();
    });

    // Clear console
    const clearConsoleEl = document.getElementById("console_clear");
    clearConsoleEl.addEventListener("click", () => {
        terminal.clear();

        clearLogCounts();

        clearLogCountEls();

        hideLogCountEls([LogType.total]);
    });

    // Toggle visibility
    const toggleVisibilityEl = document.getElementById("console_toggleVisibility");
    let consoleShown: boolean = false;
    toggleVisibilityEl.addEventListener("click", () => {
        if (consoleShown) {
            consoleShown = false;
            console_jumpToPresent.style.display = "none";
            clearConsoleEl.style.display = "none";
            toggleVisibilityEl.innerHTML = `<i class="fa-solid fa-eye consoleActionIcon"></i>`;
            showHideTooltip.setContent("Show");
            consoleContainer.style.height = "32px";
        } else {
            consoleShown = true;
            console_jumpToPresent.style.display = "";
            clearConsoleEl.style.display = "";
            toggleVisibilityEl.innerHTML = `<i class="fa-solid fa-eye-slash consoleActionIcon"></i>`;
            showHideTooltip.setContent("Hide");
            consoleContainer.style.height = "";
        }

        fitAddon.fit();
        terminal.scrollToBottom();
    });

    // Detect events that require a console resize
    const browserWindow = getCurrentWindow();
    browserWindow.on("resized", () => { fitAddon.fit() });
    browserWindow.on("maximize", () => { fitAddon.fit() });
    browserWindow.on("unmaximize", () => { fitAddon.fit() });

    // Copy on right click
    consoleElement.addEventListener("contextmenu", () => {
        if (terminal.getSelection() === "") {
            return;
        }

        clipboard.writeText(terminal.getSelection());
        terminal.clearSelection();
    });
}

export function Log(text: string, type: LogType = LogType.plain, style: LogStyle[] = []) {
    const prefix = eLogTypeMap.get(type);
    if (prefix === null) return;

    // Apply styles
    style.forEach(logStyle => {
        text = eLogStyleMap.get(logStyle)(text);
    });

    terminal.writeln(`${DateTime.now().toFormat("HH:mm:ss:u")} > ${prefix} ${text}`);

    if (!visibleConsoleLogCounts.has(type)) showLogCountEls([type]);

    consoleLogCount.get(type).val++;
    LogCountNumberEls.get(type).innerText = String(consoleLogCount.get(type).val);

    // Update the total
    consoleLogCount.get(LogType.total).val++;
    LogCountNumberEls.get(LogType.total).innerText = String(consoleLogCount.get(LogType.total).val);
}

function clearLogCounts() {
    consoleLogCount.forEach(item => {
        item.val = 0;
    });
}

function clearLogCountEls(exclude: LogType[] = []) {
    LogCountNumberEls.forEach((item, key) => {
        if (exclude.includes(key)) return;

        item.innerText = "0";
    });
}

function hideLogCountEls(exclude: LogType[] = []) {
    LogCountEls.forEach((item, key) => {
        if (exclude.includes(key)) return;

        item.style.display = "none";
        visibleConsoleLogCounts.delete(key);
    });
}

function showLogCountEls(include: LogType[] = []) {
    LogCountEls.forEach((item, key) => {
        if (!include.includes(key)) return;

        item.style.display = "";
        visibleConsoleLogCounts.add(key);
    });
}

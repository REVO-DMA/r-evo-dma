function minimizeTemplate() {
    return /*html*/ `
    <div class="titlebar_windowMinimizeOuter" id="titlebar_windowMinimize">
        <img class="titlebar_windowMinimizeIcon" src="${new URL("./img/minimize.svg", import.meta.url)}">
    </div>
    `;
}

export function maximizeTemplate() {
    return /*html*/ `
    <img class="titlebar_windowMaximizeRestoreIcon" src="${new URL("./img/maximize.svg", import.meta.url)}">
    `;
}

export function restoreTemplate() {
    return /*html*/ `
    <img class="titlebar_windowMaximizeRestoreIcon" src="${new URL("./img/restore.svg", import.meta.url)}">
    `;
}

function maximizeRestoreTemplate() {
    return /*html*/ `
    <div class="titlebar_windowMaximizeRestoreOuter" id="titlebar_windowMaximizeRestore">
        <img class="titlebar_windowMaximizeRestoreIcon" src="${new URL("./img/maximize.svg", import.meta.url)}">
    </div>
    `;
}

export function closeSpinnerTemplate() {
    return /*html*/ `
    <img class="titlebar_windowCloseSpinnerIcon" src="${new URL("./img/spinner.svg", import.meta.url)}">
    `;
}

function closeTemplate() {
    return /*html*/ `
    <div class="titlebar_windowCloseOuter" id="titlebar_windowClose">
        <img class="titlebar_windowCloseIcon" src="${new URL("./img/close.svg", import.meta.url)}">
    </div>
    `;
}

export function HTML(windowTitle = "", minimize = true, maximizeRestore = true, close = true) {
    const injectedButtons = [];

    const buttonWidth = 46;
    let controlsContainerWidth = 0;

    if (minimize) {
        injectedButtons.push(minimizeTemplate());
        controlsContainerWidth += buttonWidth;
    }
    
    if (maximizeRestore) {
        injectedButtons.push(maximizeRestoreTemplate());
        controlsContainerWidth += buttonWidth;
    }

    if (close) {
        injectedButtons.push(closeTemplate());
        controlsContainerWidth += buttonWidth;
    }

    return /*html*/ `
    <div class="titlebar_outer" id="electron_titlebar_outer">
        <div class="titlebar_applicationDrag"></div>
        <div class="titlebar_manufacturer">dzltest</div>
        <div class="titlebar_windowTitle" id="titlebar_windowTitle">${windowTitle}</div>
        <div class="titlebar_controlsContainer" style="width: ${controlsContainerWidth}">
            ${injectedButtons.join("")}
        </div>
        <div class="titlebar_applicationResizer"></div>
    </div>
    `;
}

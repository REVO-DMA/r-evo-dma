/**
 * Injects a custom titlebar into the current page and returns an EventEmitter that emits events when the titlebar buttons are clicked.
 * @returns {Promise<import("events").EventEmitter>} An EventEmitter that emits events when the titlebar buttons are clicked.
 * @event minimize Emitted when minimize button is clicked.
 * @event blur Emitted when the window is blurred.
 * @event focus Emitted when the window is focused.
 * @event maximize Emitted when maximize button is clicked.
 * @event restore Emitted when restore button is clicked.
 * @event beforeClose Emitted when close button is clicked.
 */
export function injectTitlebar(): Promise<import("events").EventEmitter>;

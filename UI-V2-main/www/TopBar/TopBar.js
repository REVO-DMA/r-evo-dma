const titlebarOuter = document.getElementById("titlebar_outer");
const windowMinimize = document.getElementById("titlebar_windowMinimize");
const windowMaximizeRestore = document.getElementById("titlebar_windowMaximizeRestore");
const windowClose = document.getElementById("titlebar_windowClose");

windowMinimize.addEventListener("click", () => {
	window.chrome.webview.postMessage("minimize");
	restart();
});

windowMaximizeRestore.addEventListener("click", () => {
	window.chrome.webview.postMessage("maximizeRestore");
});

windowClose.addEventListener("click", () => {
	window.chrome.webview.postMessage("close");
});

// This fixes a bug that causes the last clicked button on the top bar to stay in a hovered state.
// Quick and dirty but it works.
// This just tells the c# side to reload the page.
function restart() {
	setTimeout(() => {
		window.chrome.webview.postMessage("restart");
	}, 50)
}

window.chrome.webview.addEventListener("message", (message) => {
	const msg = message.data;

	if (msg === "maximize") {
		windowMaximizeRestore.children[0].src = "{rl}www/TopBar/img/restore.svg{/rl}";
	} else if (msg === "restore") {
		windowMaximizeRestore.children[0].src = "{rl}www/TopBar/img/maximize.svg{/rl}";
	} else if (msg === "focus") {
		titlebarOuter.classList.remove("titlebar_outerBlurred");
	} else if (msg === "blur") {
		titlebarOuter.classList.add("titlebar_outerBlurred");
	}
});

window.addEventListener("DOMContentLoaded", () => {
	window.chrome.webview.postMessage("DOMContentLoaded");
});
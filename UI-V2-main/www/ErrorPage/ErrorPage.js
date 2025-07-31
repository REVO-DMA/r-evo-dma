const retryButton = document.getElementById("retryButton");

retryButton.addEventListener("click", () => {
	window.chrome.webview.postMessage("reload");
});

window.chrome.webview.addEventListener("message", (message) => {
	console.log(message.data);
});
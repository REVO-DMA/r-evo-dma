import delay from "delay";

export async function showPage(page: HTMLElement) {
    page.style.display = "";
    await delay(100);
    page.classList.add("shown");
    await delay(900);
}

export async function hidePage(page: HTMLElement) {
    page.classList.remove("shown");
    await delay(1000);
    page.style.display = "none";
}

export async function fadePages(hide: HTMLElement, show: HTMLElement) {
    hide.classList.remove("shown");
    await delay(600);

    show.style.display = "";
    await delay(100);
    show.classList.add("shown");

    await delay(300);
    hide.style.display = "none";
}

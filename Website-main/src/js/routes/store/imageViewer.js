/**
 * @param {string[]} images An array of image URIs.
 * @param {number} startIndex
 */
export function showImageViewer(images, startIndex) {
    document.body.insertAdjacentHTML("beforeend", generateHTML(images, startIndex));

    attachEvents(startIndex);
}

function hideImageViewer() {
    Array.from(document.getElementsByClassName("imageViewerContainer")).forEach((imageViewer) => {
        imageViewer.outerHTML = "";
    });
}

/**
 * @param {string[]} images An array of image URIs.
 * @param {number} startIndex
 */
function generateHTML(images, startIndex) {
    const imagesHTML = [];
    images.forEach((image, i) => {
        if (i === startIndex) {
            if (image.endsWith(".mp4")) {
                imagesHTML.push(/*html*/ `
                    <div class="col-auto p-0 imageViewerImageContainer">
                        <video autoplay="true" loop="true" src="${image}" class="p-0 imageViewerImage" type="video/mp4" />
                    </div>
                `);
            } else {
                imagesHTML.push(/*html*/ `
                    <div class="col-auto p-0 imageViewerImageContainer">
                        <img src="${image}" class="p-0 imageViewerImage" />
                    </div>
                `);
            }
        } else {
            if (image.endsWith(".mp4")) {
                imagesHTML.push(/*html*/ `
                    <div class="col-auto p-0 imageViewerImageContainer" style="display: none;">
                        <video autoplay="true" loop="true" src="${image}" class="p-0 imageViewerImage" type="video/mp4" />
                    </div>
                `);
            } else {
                imagesHTML.push(/*html*/ `
                    <div class="col-auto p-0 imageViewerImageContainer" style="display: none;">
                        <img src="${image}" class="p-0 imageViewerImage" />
                    </div>
                `);
            }
        }
    });

    return /*html*/ `
        <div class="row m-0 justify-content-center align-items-center imageViewerContainer prevent-select">
            <div class="col-auto p-0 imageViewerCloseButton" id="imageViewerCloseButton">
                <i class="fa-solid fa-xmark"></i>
            </div>
            <div class="row col-12 m-0 p-0 imageViewerContainerInner justify-content-between align-items-center">
                <div class="col-auto imageViewerChevron" id="imageViewerLeftChevron"><i class="fa-solid fa-chevron-left"></i></div>
                ${imagesHTML.join("")}
                <div class="col-auto imageViewerChevron" id="imageViewerRightChevron"><i class="fa-solid fa-chevron-right"></i></div>
            </div>
            <div class="row col-12 m-0 p-0 justify-content-center align-items-center">
                <div class="col-auto imageViewerIndex" id="imageViewerIndex">${startIndex + 1} of ${images.length}</div>
            </div>
        </div>
    `;
}

/**
 * Attach events to the image viewer.
 * @param {number} startIndex
 */
function attachEvents(startIndex) {
    /** @type {HTMLDivElement[]} */
    const images = Array.from(document.getElementsByClassName("imageViewerImageContainer"));

    const imageViewerIndexEl = document.getElementById("imageViewerIndex");

    let imageIndex = startIndex;

    // Handle decrement
    document.getElementById("imageViewerLeftChevron").addEventListener("click", () => {
        if (imageIndex === 0) return;

        images[imageIndex].style.display = "none";
        imageIndex--;
        images[imageIndex].style.display = "";

        imageViewerIndexEl.innerText = `${imageIndex + 1} of ${images.length}`;
    });

    // Handle increment
    document.getElementById("imageViewerRightChevron").addEventListener("click", () => {
        if (imageIndex + 1 === images.length) return;

        images[imageIndex].style.display = "none";
        imageIndex++;
        images[imageIndex].style.display = "";

        imageViewerIndexEl.innerText = `${imageIndex + 1} of ${images.length}`;
    });

    document.getElementById("imageViewerCloseButton").addEventListener("click", hideImageViewer);
}

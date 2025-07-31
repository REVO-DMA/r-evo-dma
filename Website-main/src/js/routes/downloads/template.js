import defaultIcon from "../../../img/defaultIcon.svg";
import { httpPost } from "../../http";
import { attachEvents as attachHeaderEvents, header as getHeader } from "../../templates/header";
import prettyBytes from "pretty-bytes";
import Showdown from "showdown";

const converter = new Showdown.Converter();

/**
 * Represents a download.
 * @typedef {Object} Download
 * @property {string} name The friendly name of this download.
 * @property {string} id The internal ID of this download.
 * @property {string} version The version of the download.
 * @property {number} size The size of this download.
 * @property {string} updatedAt The relative time this download was uploaded at.
 * @property {string} changelog The download's changelog.
 */

/** @type {Download[]?} */
let downloads = null;

/** @type {string} */
let selectedDownloadID = null;

export async function show(queryParams) {
    const result = await httpPost("get-downloads-list", {}, true);
    const response = result.response;
    if (response.success) {
        downloads = response.message;
    } else {
        // Show message - unable to retrieve products
        return;
    }

    document.getElementById("baseContainer").innerHTML = getTemplate();
    attachHeaderEvents();
    attachDownloadableEvents();
}

/**
 * Get a download by it's ID.
 * @param {string} ID
 */
export function getDownloadByID(ID) {
    let output = null;

    for (let i = 0; i < downloads.length; i++) {
        const download = downloads[i];

        if (download.id === ID) {
            output = download;
            break;
        }
    }

    return output;
}

/**
 * @param {Download} downloadInfo
 */
function createDownload(downloadInfo) {
    return /*html*/ `
        <div class="row m-0 justify-content-center align-items-center storeItem" id="${downloadInfo.id}">
            <!-- Icon -->
            <div class="col-auto">
                <img src="${defaultIcon}" class="p-0 storeItemIcon" />
            </div>
            <!-- Name/Blurb -->
            <div class="col">
                <!-- Name -->
                <div class="row">
                    <div class="col-auto storeItemName">${downloadInfo.name}</div>
                    <div class="col-auto">${downloadInfo.version !== null ? downloadInfo.version : ""}</div>
                </div>
                <!-- Blurb -->
                <div class="row mt-2">
                    <div class="col-auto storeItemDescriptionBlurb">Updated: ${downloadInfo.updatedAt}</div>
                    <div class="col-auto storeItemDescriptionBlurb">Size: ${prettyBytes(downloadInfo.size)}</div>
                </div>
            </div>
            <!-- Chevron -->
            <div class="col-auto">
                <i class="fa-solid fa-chevron-right"></i>
            </div>
        </div>
    `;
}

function getTemplate() {
    const storeDownloads = [];
    downloads.forEach((download) => {
        storeDownloads.push(createDownload(download));
    });

    return /*html*/ `
        ${getHeader("Downloads")}

        <div class="row m-0 p-0 justify-content-center align-items-center downloadsContainer">
            ${storeDownloads.join("")}
        </div>
    `;
}

/**
 * @param {Download} downloadInfo
 */
function viewDownload(downloadInfo) {
    return /*html*/ `
        <div class="row m-0 justify-content-center align-items-center storeViewItemContainer">
            <!-- Main Image -->
            <div class="col-4">
                <img src="${defaultIcon}" class="p-0 downloadsMainImage" />
            </div>
            <div class="row m-0 col-8">
                <!-- Main -->
                <div class="col-12 storeViewItemMain">
                    <!-- Name -->
                    <div class="row m-0">
                        <div class="col-auto storeItemName">${downloadInfo.name}</div>
                    </div>
                    <div class="row m-0 mt-3">
                        <div class="col-auto">Updated: ${downloadInfo.updatedAt}</div>
                    </div>
                    ${downloadInfo.version !== null ? /*html*/`
                        <div class="row m-0 mt-3">
                            <div class="col-auto">Version: ${downloadInfo.version}</div>
                        </div>
                    ` : ""}
                    <!-- Changelog Area -->
                    <div class="row m-0 mt-3 storeItemTabsContainer">
                        <!-- Changelog Toggle -->
                        <div class="row m-0 p-0 storeItemTabSelectorOuter">
                            <div class="col-auto p-0 storeItemTabSelector storeItemTabSelectorActive"><i class="fa-solid fa-list-ul me-2"></i>Changelog</div>
                        </div>
                        <div class="row m-0 p-3 storeItemSelectedTabContent">
                            <div class="col-auto p-0 storeItemDescription">${downloadInfo.changelog != null ? converter.makeHtml(downloadInfo.changelog) : "No changelog available."}</div>
                        </div>
                    </div>
                </div>
                <!-- Purchase -->
                <div class="col-12 mt-3 storeViewItemMain">
                    <div class="row m-0 align-items-center justify-content-end">
                        <div class="col-auto pe-0">Size: ${prettyBytes(downloadInfo.size)}</div>
                        <div class="col-auto pe-0">
                        ${downloadInfo.version !== null ? /*html*/`
                            <button class="btn btn-primary" id="DownloadFileButton">Download ver. ${downloadInfo.version}</button>
                        ` : /*html*/`
                            <button class="btn btn-primary" id="DownloadFileButton">Download</button>
                        `}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    `;
}

function attachDownloadableEvents() {
    /** @type {HTMLDivElement[]} */
    const storeItems = document.getElementsByClassName("storeItem");
    Array.from(storeItems).forEach((downloadItem) => {
        downloadItem.addEventListener("click", async () => {
            selectedDownloadID = downloadItem.id;
            const downloadItemData = getDownloadByID(downloadItem.id);

            const HTML = /*html*/ `
                ${getHeader("Downloads")}
                ${viewDownload(downloadItemData)}
            `;

            document.getElementById("baseContainer").innerHTML = HTML;
            attachHeaderEvents();
            attachFileDownloadEvent();
        });
    });
}

function attachFileDownloadEvent() {
    const button = document.getElementById("DownloadFileButton");
    button.addEventListener("click", async () => {
        const result = await httpPost("get-download", { id: selectedDownloadID }, true);
        const response = result.response;
        /** @type {string} */
        let url = null;
        if (response.success) {
            url = response.message;
        } else {
            // Show message - unable to retrieve download
            return;
        }

        // Download the file
        const anchor = document.createElement("a");
        anchor.href = url;
        anchor.target = "_blank";
        document.body.appendChild(anchor);
        anchor.click();
    });
}
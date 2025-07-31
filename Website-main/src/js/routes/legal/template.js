import { attachEvents as attachHeaderEvents, header as getHeader } from "../../templates/header";
import { getText as getEulaText } from "./eula";
import { getStyles as getLegalStyles } from "./legalStyles";
import { getText as getPrivacyPolicyText } from "./privacyPolicy";
import { getText as getReturnsPolicyText } from "./returnsPolicy";
import { getText as getTermsOfServiceText } from "./termsOfService";

export async function show(queryParams) {
    document.getElementById("baseContainer").innerHTML = getTemplate();
    attachEvents();
    attachHeaderEvents();
}

function attachEvents() {
    const read_privacyPolicyEl = document.getElementById("read_privacyPolicy");
    const read_tosEl = document.getElementById("read_tos");
    const read_eulaEl = document.getElementById("read_eula");
    const read_returnsPolicyEl = document.getElementById("read_returnsPolicy");
    
    read_privacyPolicyEl.addEventListener("click", () => {
        const text = getPrivacyPolicyText();
        showLegalText(text);
    });

    read_tosEl.addEventListener("click", () => {
        const text = getTermsOfServiceText();
        showLegalText(text);
    });

    read_eulaEl.addEventListener("click", () => {
        const text = getEulaText();
        showLegalText(text);
    });

    read_returnsPolicyEl.addEventListener("click", () => {
        const text = getReturnsPolicyText();
        showLegalText(text);
    });
}

function showLegalText(html) {
    
    const legalContainerEl = document.getElementById("legalContainer");
    const legalTextContainerEl = document.getElementById("legalTextContainer");
    
    const styles = getLegalStyles();
    legalTextContainerEl.innerHTML = styles + html;

    legalContainerEl.style.display = "none";
    legalTextContainerEl.style.display = "";
}

function getTemplate() {
    return /*html*/ `
        ${getHeader("Legal")}

        <div class="row m-0 p-0 justify-content-center align-items-center legalTextContainer" id="legalTextContainer" style="display: none;"></div>

        <div class="row m-0 p-0 justify-content-center align-items-center legalContainer" id="legalContainer">
            <div class="row m-0 mb-4 p-0 justify-content-center align-items-center">
                <div class="col legalSectionCard me-3">
                    <!-- Title -->
                    <div class="row m-0 p-0 justify-content-center align-items-center">
                        <div class="col-auto">
                            <h1>Privacy Policy</h1>
                        </div>
                    </div>

                    <!-- Blurb -->
                    <div class="row m-0 mb-2 p-0 justify-content-center align-items-center legalCardBlurb">
                        <div class="col-auto">Our Privacy Policy outlines how we collect, use, and protect your personal information. We are committed to ensuring your privacy is safeguarded and comply with all relevant data protection regulations. By using our services, you consent to our collection and use of your information as described in this policy.</div>
                    </div>

                    <!-- Button -->
                    <div class="row m-0 p-0 justify-content-center align-items-center">
                        <div class="col-auto">
                            <button class="btn btn-primary" id="read_privacyPolicy">
                                Read<i class="fa-solid fa-chevron-right ms-2 buttonIcon"></i>
                            </button>
                        </div>
                    </div>
                </div>

                <div class="col legalSectionCard">
                    <!-- Title -->
                    <div class="row m-0 p-0 justify-content-center align-items-center">
                        <div class="col-auto">
                            <h1>TOS</h1>
                        </div>
                    </div>

                    <!-- Blurb -->
                    <div class="row m-0 mb-2 p-0 justify-content-center align-items-center legalCardBlurb">
                        <div class="col-auto">Our Terms of Service govern your use of our website and services. By accessing or using our platform, you agree to abide by these terms. Please read them carefully to understand your rights and obligations while using our services.</div>
                    </div>

                    <!-- Button -->
                    <div class="row m-0 p-0 justify-content-center align-items-center">
                        <div class="col-auto">
                            <button class="btn btn-primary" id="read_tos">
                                Read<i class="fa-solid fa-chevron-right ms-2 buttonIcon"></i>
                            </button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row m-0 p-0 justify-content-center align-items-center">
                <div class="col legalSectionCard me-3">
                    <!-- Title -->
                    <div class="row m-0 p-0 justify-content-center align-items-center">
                        <div class="col-auto">
                            <h1>EULA</h1>
                        </div>
                    </div>
                    
                    <!-- Blurb -->
                    <div class="row m-0 mb-2 p-0 justify-content-center align-items-center legalCardBlurb">
                        <div class="col-auto">The End User License Agreement (EULA) is a legal contract between you and our company, granting you the right to use our software under specific conditions. By installing or using our software, you accept and agree to comply with the terms outlined in this agreement.</div>
                    </div>

                    <!-- Button -->
                    <div class="row m-0 p-0 justify-content-center align-items-center">
                        <div class="col-auto">
                            <button class="btn btn-primary" id="read_eula">
                                Read<i class="fa-solid fa-chevron-right ms-2 buttonIcon"></i>
                            </button>
                        </div>
                    </div>
                </div>

                <div class="col legalSectionCard">
                    <!-- Title -->
                    <div class="row m-0 p-0 justify-content-center align-items-center">
                        <div class="col-auto">
                            <h1>Returns Policy</h1>
                        </div>
                    </div>

                    <!-- Blurb -->
                    <div class="row m-0 mb-2 p-0 justify-content-center align-items-center legalCardBlurb">
                        <div class="col-auto">Our Returns Policy outlines the conditions under which returns may be considered. Please review this policy carefully to understand the criteria under which returns may be considered.</div>
                    </div>

                    <!-- Button -->
                    <div class="row m-0 p-0 justify-content-center align-items-center">
                        <div class="col-auto">
                            <button class="btn btn-primary" id="read_returnsPolicy">
                                Read<i class="fa-solid fa-chevron-right ms-2 buttonIcon"></i>
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    `;
}
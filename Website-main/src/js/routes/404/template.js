import { initialize as initializeLogic } from "./logic";

export function show(queryParams) {
    document.getElementById("baseContainer").innerHTML = getTemplate();
    initializeLogic();
}

function getTemplate() {
    return /*html*/ `
         <!-- 404 -->
        <div class="col-auto mt-5 mb-5 error404Container">
            <div class="row m-0 mb-4 justify-content-center align-items-center">
                <div class="col-auto">
                    <h1 id="authFormTitle">Page Not Found</h1>
                </div>
            </div>

            <div class="row m-0 mb-4 justify-content-center align-items-center">
                <div class="col-auto">
                    <button class="btn btn-primary" id="goHomeButton">Go Home</button>
                </div>
            </div>
        </div>
    `;
}
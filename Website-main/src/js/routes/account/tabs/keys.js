import { PurchaseData } from "../../auth/accountDataManager";

export function getTemplate() {
    return /*html*/ `
        <div class="row m-0 justify-content-center align-items-center">
            <table class="table table-dark table-striped text-center accountTable">
                <thead>
                    <tr>
                    <th scope="col">Product</th>
                    <th scope="col">Date</th>
                    <th scope="col">Key</th>
                    <th scope="col">Term</th>
                    </tr>
                </thead>
                <tbody>${generateSubscriptionsMarkup()}</tbody>
            </table>
        </div>
    `;
}

function generateSubscriptionsMarkup() {
    const keys = PurchaseData.keys;

    const HTML = [];

    keys.forEach((key) => {
        HTML.push(/*html*/ `
            <tr>
                <td>${key.Product}</td>
                <td>${key.Date}</td>
                <td>${key.Key}</td>
                <td>${key.Term > 1 ? `${key.Term} Days` : `${key.Term} Day`}</td>
            </tr>
        `);
    });

    return HTML.join("");
}

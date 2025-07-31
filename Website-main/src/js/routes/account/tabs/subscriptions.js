import { PurchaseData } from "../../auth/accountDataManager";

export function getTemplate() {
    return /*html*/ `
        <div class="row m-0 justify-content-center align-items-center">
            <table class="table table-dark table-striped text-center accountTable">
                <thead>
                    <tr>
                    <th scope="col">Product</th>
                    <th scope="col">Expiration</th>
                    <th scope="col">Active</th>
                    </tr>
                </thead>
                <tbody>${generateSubscriptionsMarkup()}</tbody>
            </table>
        </div>
    `;
}

function generateSubscriptionsMarkup() {
    const subscriptions = PurchaseData.subscriptions;

    const HTML = [];

    subscriptions.forEach((subscription) => {
        let expirationText = "N/A";
        if (subscription.Expiration != null) {
            expirationText = subscription.Expiration.toUpperCase();
        }

        let activeText = "NO";
        if (subscription.Active) {
            activeText = "YES";
        }

        HTML.push(/*html*/ `
            <tr>
                <td>${subscription.Product}</td>
                <td>${expirationText}</td>
                <td>${activeText}</td>
            </tr>
        `);
    });

    return HTML.join("");
}

import { PurchaseData } from "../../auth/accountDataManager";

export function getTemplate() {
    return /*html*/ `
        <div class="row m-0 justify-content-center align-items-center">
            <table class="table table-dark table-striped text-center accountTable">
                <thead>
                    <tr>
                    <th scope="col">ID</th>
                    <th scope="col">Date</th>
                    <th scope="col">Term</th>
                    <th scope="col">Price</th>
                    <th scope="col">Status</th>
                    </tr>
                </thead>
                <tbody>${generateOrdersMarkup()}</tbody>
            </table>
        </div>
    `;
}

function generateOrdersMarkup() {
    const orders = PurchaseData.orders;

    const HTML = [];

    orders.forEach((order) => {
        HTML.push(/*html*/ `
            <tr>
                <td>${order.ID}</td>
                <td>${order.Date.toUpperCase()}</td>
                <td>${order.Term} Days</td>
                <td>$${order.Price}</td>
                <td>${order.Status.toUpperCase()}</td>
            </tr>
        `);
    });

    return HTML.join("");
}

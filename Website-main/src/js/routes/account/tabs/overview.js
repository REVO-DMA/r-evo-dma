import { AccountData, PurchaseData } from "../../auth/accountDataManager";

export function getTemplate() {
    return /*html*/ `
        <!-- Account Stats -->
        <div class="row m-0 p-3 justify-content-center align-items-center">
            <!-- Orders -->
            <div class="col-auto p-0 me-3 accountStatsCard">
                <div class="row m-0 justify-content-center align-items-center accountStatsCardTitle">
                    <div class="col-auto">Orders</div>
                </div>
                <div class="row m-0 mt-2 mb-2 justify-content-center align-items-center accountStatsCardValue">
                    <div class="col-auto">${PurchaseData.orders.length}</div>
                </div>
            </div>
            <!-- Subscriptions -->
            <div class="col-auto p-0 me-3 accountStatsCard">
                <div class="row m-0 justify-content-center align-items-center accountStatsCardTitle">
                    <div class="col-auto">Subscriptions</div>
                </div>
                <div class="row m-0 mt-2 mb-2 justify-content-center align-items-center accountStatsCardValue">
                    <div class="col-auto">${PurchaseData.activeSubscriptions}</div>
                </div>
            </div>
        </div>
    `;

    // If user is not a member of the referral program show the CTA
    // ${AccountData.account.canInvite === false ? getReferralProgramCTA() : ""}

    // If user is a member of the referral program show stats
    // ${AccountData.account.canInvite === true ? getReferralProgramCards() : ""}
}

function getReferralProgramCards() {
    return /*html*/ `
        <!-- Referrals -->
        <div class="col-auto p-0 me-3 accountStatsCard">
            <div class="row m-0 justify-content-center align-items-center accountStatsCardTitle">
                <div class="col-auto">Referrals</div>
            </div>
            <div class="row m-0 mt-2 mb-2 justify-content-center align-items-center accountStatsCardValue">
                <div class="col-auto">0</div>
            </div>
        </div>
        <!-- Payouts -->
        <div class="col-auto p-0 accountStatsCard">
            <div class="row m-0 justify-content-center align-items-center accountStatsCardTitle">
                <div class="col-auto">Pending Payout</div>
            </div>
            <div class="row m-0 mt-2 mb-2 justify-content-center align-items-center accountStatsCardValue">
                <div class="col-auto">$0</div>
            </div>
        </div>  
    `;
}

function getReferralProgramCTA() {
    return /*html*/ `
        <!-- Referrals CTA -->
        <div class="row m-0 mb-5 justify-content-center align-items-center">
            <h3 class="text-center">Looking to make some extra cash?</h3>
            <div class="referralProgramDescription mb-2">
                Our referral program is a no-brainer way to get your buddies in on the fun and score some extra cash, too!
            </div>
            <div class="col-auto">
                <span class="ctaLearnMore" id="referralProgramLearnMore">Learn More<i class="fa-solid fa-arrow-right-long inlineTextIcon ms-2"></i></span>
            </div>
        </div>
    `;
}

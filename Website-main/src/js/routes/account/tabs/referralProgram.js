import { AccountData } from "../../auth/accountDataManager";

export function getTemplate() {
    return /*html*/ `
        <!-- If user is not a member of the referral program show the CTA -->
        ${AccountData.account.canInvite === false ? getReferralProgramCTA() : ""}
    `;
}

function getReferralProgramCTA() {
    return /*html*/ `
        <!-- Referrals CTA -->
        <div class="row m-0 justify-content-center align-items-center">
            <h3 class="text-center">Referral Program Details</h3>
            <div class="referralProgramDescription mb-2">
                Once someone registers with your invite code and makes their first purchase you will get a 10% commission.
                But the benefits don't stop there!
                You'll continuously earn an impressive 8% commission on all their future subscriptions and renewals.
                Your earnings can conveniently be credited to your account or, if you prefer, you can request a payout upon the completion of their subscription term.
                So, what are you waiting for? Get started today!
            </div>
            <div class="col-auto">
                <span class="ctaLearnMore" id="referralProgramLearnMore">Request Access</span>
            </div>
        </div>
    `;
}

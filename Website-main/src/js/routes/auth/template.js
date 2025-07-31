import authLogo from "../../../img/logo.svg";
import { initialize as initializeLogic } from "./logic";

export async function show(queryParams) {
    document.getElementById("baseContainer").innerHTML = getTemplate();
    initializeLogic(queryParams);
}

function getTemplate() {
    return /*html*/ `
        <!-- Logo -->
         <div class="col-auto">
            <div class="row m-0 pe-5 justify-content-center align-items-center">
                <div class="col-auto">
                    <img src="${authLogo}" class="authLogo p-0" />
                </div>
            </div>
         </div>

         <!-- Auth Form -->
        <div class="col-auto mt-5 mb-5 loginContainer">
            <!-- Form Title -->
            <div class="row m-0 mb-4 justify-content-center align-items-center">
                <div class="col-auto">
                    <h1 id="authFormTitle">Loading</h1>
                </div>
            </div>

            <!-- Alerts -->
            <div class="row m-0 justify-content-center align-items-center" id="authAlerts" style="display: none;"></div>

            <!-- Username -->
            <div class="row m-0 mb-3 justify-content-center align-items-center authUsername" style="display: none">
                <div class="col">
                    <label for="authUsername" class="form-label">Username</label>
                    <input type="text" class="form-control" id="authUsername" />
                </div>
            </div>

            <!-- Password Reset Token -->
            <div class="row m-0 mb-3 justify-content-center align-items-center authPasswordResetToken" style="display: none">
                <div class="col">
                    <label for="authPasswordResetToken" class="form-label">Token</label>
                    <input type="text" class="form-control" id="authPasswordResetToken" />
                </div>
            </div>

            <!-- Account Activation Token -->
            <div class="row m-0 mb-3 justify-content-center align-items-center authAccountActivationToken" style="display: none">
                <div class="col">
                    <label for="authAccountActivationToken" class="form-label">Token</label>
                    <input type="text" class="form-control" id="authAccountActivationToken" />
                </div>
            </div>

            <!-- Email -->
            <div class="row m-0 mb-3 justify-content-center align-items-center authEmail" style="display: none">
                <div class="col">
                    <label for="authEmail" class="form-label">Email</label>
                    <input type="email" class="form-control" id="authEmail" />
                </div>
            </div>

            <!-- Password -->
            <div class="row m-0 mb-2 justify-content-center align-items-center authPassword" style="display: none">
                <div class="col">
                    <label for="authPassword" class="form-label">Password</label>
                    <div class="row m-0">
                        <div class="col p-0">
                            <input type="password" class="form-control passwordInput" id="authPassword" />
                        </div>
                        <div class="col-auto p-0">
                            <div class="form-control passwordVisibilityButton" id="authPassword_visibilityToggle"><i class="fa-duotone fa-eye-slash"></i></div>
                        </div>
                    </div>
                    <div class="form-text authShowForgotPassword" id="authShowForgotPassword">Forgot password?</div>
                </div>
            </div>

            <!-- New Password -->
            <div class="row m-0 mb-2 justify-content-center align-items-center authNewPassword" style="display: none">
                <div class="col">
                    <label for="authNewPassword" class="form-label">New Password</label>
                    <div class="row m-0">
                        <div class="col p-0">
                            <input type="password" class="form-control passwordInput" id="authNewPassword" />
                        </div>
                        <div class="col-auto p-0">
                            <div class="form-control passwordVisibilityButton" id="authNewPassword_visibilityToggle"><i class="fa-duotone fa-eye-slash"></i></div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Password Strength -->
            <div class="row m-0 mb-3 justify-content-center align-items-center authPasswordRelated" style="display: none">
                <div class="row m-0">
                    <label class="form-label p-0">Strength: <span class="passwordStrengthText_0" id="passwordStrengthText">Too weak</span></label>
                </div>
                
                <!-- Lowercase -->
                <div class="row m-0 mb-1">
                    <div class="col passwordStrengthCriteria passwordStrengthCriteria_notMet" id="passwordStrengthCriteria_lowercaseContainer">
                        <span id="passwordStrengthCriteria_lowercaseIcon"><i class="fa-duotone fa-circle-xmark inlineTextIcon me-1"></i></span>
                        <span>Lowercase</span>
                    </div>
                </div>
                
                <!-- Uppercase -->
                <div class="row m-0 mb-1">
                    <div class="col passwordStrengthCriteria passwordStrengthCriteria_notMet" id="passwordStrengthCriteria_uppercaseContainer">
                        <span id="passwordStrengthCriteria_uppercaseIcon"><i class="fa-duotone fa-circle-xmark inlineTextIcon me-1"></i></span>
                        <span>Uppercase</span>
                    </div>
                </div>

                <!-- Number -->
                <div class="row m-0 mb-1">
                    <div class="col passwordStrengthCriteria passwordStrengthCriteria_notMet" id="passwordStrengthCriteria_numberContainer">
                        <span id="passwordStrengthCriteria_numberIcon"><i class="fa-duotone fa-circle-xmark inlineTextIcon me-1"></i></span>
                        <span>Number</span>
                    </div>
                </div>

                <!-- Symbol -->
                <div class="row m-0">
                    <div class="col passwordStrengthCriteria passwordStrengthCriteria_notMet" id="passwordStrengthCriteria_symbolContainer">
                        <span id="passwordStrengthCriteria_symbolIcon"><i class="fa-duotone fa-circle-xmark inlineTextIcon me-1"></i></span>
                        <span>Symbol</span>
                    </div>
                </div>
            </div>

            <!-- Invite Code -->
            <div class="row m-0 mb-3 justify-content-center align-items-center authInviteCode" style="display: none">
                <div class="col">
                    <label for="authInviteCode" class="form-label">Private Invite Code</label>
                    <input type="text" class="form-control" id="authInviteCode" />
                </div>
            </div>

            <div class="row m-0 mb-3 justify-content-center align-items-center">
                <div class="col-auto">
                    <button class="btn btn-primary authActivateAccount" id="authActivateAccount" style="display: none">Activate Account</button>
                    <button class="btn btn-primary authResetPassword" id="authResetPassword" style="display: none">Reset Password</button>
                    <button class="btn btn-primary authSendForgotPasswordEmail" id="authSendForgotPasswordEmail" style="display: none">Send Email</button>
                    <button class="btn btn-primary authLogin" id="authLogin" style="display: none">Login</button>
                    <button class="btn btn-primary authShowRegister" id="authShowRegistration" style="display: none">Show Registration</button>
                    <button class="btn btn-primary authRegister" id="authRegister" style="display: none">Register</button>
                    <button class="btn btn-primary authShowLogin" id="authShowLogin" style="display: none">Show Login</button>
                </div>
            </div>
        </div>
    `;
}
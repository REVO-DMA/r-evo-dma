import Swal from "sweetalert2";

/**
 * @param {("warning"|"error"|"success"|"info"|"question")} type
 * @param {string} title Main heading of the SweetAlert2 modal
 * @param {string} text Textual content of the SweetAlert2 modal
 * @param {boolean} showCancelButton Whether or not to show the cancel button
 * @param {string} confirmButtonText The textual content of the confirm button
 * @param {string} cancelButtonText The textual content of the cancel button
 * @param {string} callback A callback
 */
export function showAlert(type, title, text, showCancelButton, showDenyButton, confirmButtonText, denyButtonText, cancelButtonText, callback = null, preConfirm = null) {
    return new Promise((resolve) => {
        // Show the alert with the specified options
        Swal.fire({
            icon: type, // "warning", "error", "success", "info", or "question"
            title: title, // Main heading of the SweetAlert2 modal
            html: text, // Textual content of the SweetAlert2 modal
            showCancelButton: showCancelButton, // Whether or not to show the cancel button
            showDenyButton: showDenyButton, // Whether or not to show the deny button
            confirmButtonText: confirmButtonText, // The textual content of the confirm button
            cancelButtonText: cancelButtonText, // The textual content of the cancel button
            denyButtonText: denyButtonText, // The textual content of the deny button
            allowOutsideClick: false,
            allowEnterKey: false,
            preConfirm: preConfirm,
            heightAuto: false,
            showClass: {
                backdrop: "swal2-noanimation",
                popup: "",
                icon: "",
            },
            hideClass: {
                popup: "",
            },
        }).then((result) => {
            if (preConfirm) {
                resolve(result);
            } else {
                if (showDenyButton) {
                    if (result.isConfirmed) {
                        callback("yes");
                    } else if (result.isDenied) {
                        callback("no");
                    } else if (result.dismiss === Swal.DismissReason.cancel) {
                        if (callback) {
                            callback("cancel");
                        }
                    }
                } else {
                    if (result.value) {
                        if (callback) {
                            callback(true);
                        }
                    } else if (result.dismiss === Swal.DismissReason.cancel) {
                        if (callback) {
                            callback(false);
                        }
                    }
                }
            }
        });

        const confirmBtn = Swal.getConfirmButton();
        confirmBtn.classList.remove("swal2-confirm", "swal2-styled");
        confirmBtn.classList.add("btn", "btn-primary");

        if (showDenyButton) {
            const denyBtn = Swal.getDenyButton();
            denyBtn.classList.remove("swal2-deny", "swal2-styled");
            denyBtn.classList.add("btn", "btn-primary", "me-2");

            confirmBtn.classList.add("me-2");
        }

        if (showCancelButton) {
            const cancelBtn = Swal.getCancelButton();
            cancelBtn.classList.remove("swal2-cancel", "swal2-styled");
            cancelBtn.classList.add("btn", "btn-primary");

            confirmBtn.classList.add("me-2");
        }
    });
}

/**
 * Closes the currently open SweetAlert2 popup programmatically.
 */
export function closeAlert() {
    Swal.close();
}

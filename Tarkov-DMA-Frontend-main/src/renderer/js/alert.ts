import Swal, { SweetAlertIcon } from "sweetalert2";

export function showAlert(
    type: SweetAlertIcon,
    title: string | HTMLElement,
    text: string | HTMLElement,
    showCancelButton: boolean,
    showDenyButton: boolean,
    confirmButtonText: string,
    denyButtonText: string,
    cancelButtonText: string,
    callback?: (data: any) => void,
    preConfirm?: (inputValue: any) => void)
    {
    return new Promise((resolve) => {
        Swal.fire({
            icon: type,
            title: title,
            html: text,
            showCancelButton: showCancelButton,
            showDenyButton: showDenyButton,
            confirmButtonText: confirmButtonText,
            cancelButtonText: cancelButtonText,
            denyButtonText: denyButtonText,
            allowOutsideClick: false,
            allowEnterKey: false,
            preConfirm: preConfirm,
            showClass: {
                backdrop: "swal2-noanimation",
                popup: "",
                icon: "",
            },
            hideClass: {
                popup: "",
            },
        }).then((result: any) => {
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

export function closeAlert()
{
    Swal.close();
}

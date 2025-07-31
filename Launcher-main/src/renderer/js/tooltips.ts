import tippy from "tippy.js";

export function initialize() {
    tippy.setDefaultProps({
        arrow: false,
        allowHTML: true,
        hideOnClick: false,
        delay: [250, 0],
    });

    tippy("[data-tippy-content]");
}

import tippy, { Instance, Props } from "tippy.js";

let instance: Instance<Props>[] = null;

export function initialize()
{
    // Clean up if already instantiated
    if (instance != null)
    {
        instance.forEach(item => {
            item.destroy();
        });
    }

    tippy.setDefaultProps({
        arrow: false,
        allowHTML: true,
        hideOnClick: false,
        delay: [250, 0],
    });

    instance = tippy("[data-tippy-content]");
}

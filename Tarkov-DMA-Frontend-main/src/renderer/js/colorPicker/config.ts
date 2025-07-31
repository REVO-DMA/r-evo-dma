import Pickr from "@simonwep/pickr";

const pickrBaseConfig: Pickr.Options = {
    el: null,
    theme: "nano",
    swatches: null,
    comparison: false,
    adjustableNumbers: false,
    components: {
        preview: false,
        opacity: false,
        hue: true,
        interaction: {
            hex: false,
            rgba: false,
            hsla: false,
            hsva: false,
            cmyk: false,
            input: true,
            cancel: false,
            clear: false,
            save: false,
        },
    },
};

export function getPickrBaseConfig(el: string | HTMLElement)
{
    const config = pickrBaseConfig;
    config.el = el;

    return config;
}
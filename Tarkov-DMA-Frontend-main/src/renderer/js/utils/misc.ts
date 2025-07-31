export function hexToRGBA(hex: string, alpha: number) {
    // Convert the hex string to a 4-digit RGBA array
    return `rgba(${parseInt(hex.substring(1, 3), 16)}, ${parseInt(hex.substring(3, 5), 16)}, ${parseInt(hex.substring(5, 7), 16)}, ${alpha * 0.01})`;
}

export function hexToRGB(hex: string) {
    // Convert the hex string to a 3-digit RGBA array
    return `rgb(${parseInt(hex.substring(1, 3), 16)}, ${parseInt(hex.substring(3, 5), 16)}, ${parseInt(hex.substring(5, 7), 16)})`;
}

export function changeCSSRule(selectorText: string, newStyle: string, replace: boolean = false) {
    for (var i = 0; i < document.styleSheets.length; i++) {
        var styleSheet = document.styleSheets[i];
        var rules = styleSheet.cssRules;

        for (var ii = 0; ii < rules.length; ii++) {
            const rule = (rules[ii] as CSSStyleRule);

            if (rule.selectorText === selectorText) {
                if (!replace) {
                    const originalCSS = rule.style.cssText;

                    rule.style.cssText += newStyle;
    
                    return originalCSS;
                } else {
                    rule.style.cssText = newStyle;
    
                    return;
                }
            }
        }
    }
}
export function changeCSSRule(selectorText: string, newStyle: string, replace = false): string {
    for (var i = 0; i < document.styleSheets.length; i++) {
        var styleSheet = document.styleSheets[i];
        var rules = styleSheet.cssRules;

        for (var ii = 0; ii < rules.length; ii++) {
            const rule = rules[ii];
            const selector = rule.cssText.split("{")[0].trim();

            if (selector === selectorText) {
                if (!replace) {
                    const originalCSS = rule.cssText;

                    rule.cssText += newStyle;
    
                    return originalCSS;
                } else {
                    rule.cssText = newStyle;
    
                    return;
                }
            }
        }

        return "";
    }
}
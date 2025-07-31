/**
 * Darken a hex, rgb, or rgba color by the specified percentage.
 * @param {string} color
 * @param {number} percent
 * @returns {string} The darkened color.
 */
export function darkenColor(color, percent) {
    let r, g, b, a;
  
    // Check if the color is in hex format
    if (color[0] === "#") {
        color = color.substring(1);

        r = parseInt(color.substring(0, 2), 16);
        g = parseInt(color.substring(2, 4), 16);
        b = parseInt(color.substring(4), 16);

        r = Math.round(r * (1 - percent / 100));
        g = Math.round(g * (1 - percent / 100));
        b = Math.round(b * (1 - percent / 100));

        r = r.toString(16).padStart(2, "0");
        g = g.toString(16).padStart(2, "0");
        b = b.toString(16).padStart(2, "0");

        return `#${r}${g}${b}`;
    }
  
    // Check if the color is in RGB or RGBA format
    if (color.includes("rgb")) {
        color = color.substring(color.indexOf("(") + 1, color.indexOf(")")).split(",");

        r = parseInt(color[0]);
        g = parseInt(color[1]);
        b = parseInt(color[2]);
        a = color.length === 4 ? color[3] : 1;
        r = Math.round(r * (1 - percent / 100));
        g = Math.round(g * (1 - percent / 100));
        b = Math.round(b * (1 - percent / 100));
        return `rgba(${r},${g},${b},${a})`;
    }
  
    return "#000000";
}

/**
 * Extracts significant characters from a username.
 * @param {string} username
 * @returns {string}
 */
export function extractSignificant(username) {
    const pascalCase = username.match(/[A-Z]/g);

    if (username.includes(" ")) {
        // Handle name with spaces
        const split = username.split(" ");
        return (split[0][0] + split[split.length - 1][0]).toUpperCase();
    } else if (pascalCase != null && pascalCase.length >= 1 && username[0] === username[0].toLowerCase()) {
        // Handle camel case name
        return (username[0] + pascalCase[pascalCase.length - 1]).toUpperCase();
    } else if (pascalCase != null && pascalCase.length >= 2) {
        // Handle pascal case name
        return pascalCase[0] + pascalCase[pascalCase.length - 1];
    } else {
        // Handle unmatched name
        return (username[0] + username[1]).toUpperCase();
    }
}

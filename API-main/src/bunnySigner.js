import queryString from "querystring";
import crypto from "crypto";
import globals from "./globals.js";

/**
 * @param {string} url
 * @param {string?} allowedCountries
 * @param {string?} blockedCountries
 */
function addCountries(url, allowedCountries, blockedCountries) {
	if (allowedCountries != null) {
		const parsedUrl = new URL(url);
		url += ((parsedUrl.search == "") ? "?" : "&") + "token_countries=" + allowedCountries;
	}
    
	if (blockedCountries != null) {
		const parsedUrl = new URL(url);
		url += ((parsedUrl.search == "") ? "?" : "&") + "token_countries_blocked=" + blockedCountries;
	}

	return url;
}

/**
 * @param {number} minutes
 */
export function minutesToSeconds(minutes) {
    return minutes * 60;
}

/**
 * @param {string} url CDN URL w/o the trailing '/' - ex. http://test.b-cdn.net/file.png
 * @param {string} securityKey Security token found in your pull zone.
 * @param {number} expirationTimeSeconds Authentication validity in seconds.
 * @param {string?} userIp Optional parameter if you have the User IP feature enabled.
 * @param {boolean} isDirectory Optional parameter. `true` returns a URL separated by forward slashes (ex. (domain)/bcdn_token=...).
 * @param {string?} pathAllowed Directory to authenticate (ex. /path/to/images).
 * @param {string?} countriesAllowed List of countries allowed (ex. CA, US, TH).
 * @param {string?} countriesBlocked List of countries blocked (ex. CA, US, TH).
 */
export function signUrl(url, expirationTimeSeconds = 3600, userIp = null, isDirectory = false, pathAllowed = null, countriesAllowed = null, countriesBlocked = null) {
	url = addCountries(url, countriesAllowed, countriesBlocked);
	
    const parsedUrl = new URL(url);
	const parameters = parsedUrl.searchParams;
	
    let signaturePath;
    if (pathAllowed != null) {
		signaturePath = pathAllowed;
		parameters.set("token_path", signaturePath);
	} else {
		signaturePath = decodeURIComponent(parsedUrl.pathname);
	}
	parameters.sort();
	
    let parameterData = "", parameterDataUrl = "";
    if (Array.from(parameters).length > 0) {
		parameters.forEach(function(value, key) {
			if (value == "") {
				return;
			}
			if (parameterData.length > 0) {
				parameterData += "&";
			}
			parameterData += key + "=" + value;
			parameterDataUrl += "&" + key + "=" + queryString.escape(value);
			
		});
	}

    const expires = Math.floor(new Date() / 1000) + expirationTimeSeconds;
    const hashableBase = globals.bunny.urlTokenKey + signaturePath + expires + ((userIp != null) ? userIp : "") + parameterData;
	let token = Buffer.from(crypto.createHash("sha256").update(hashableBase).digest()).toString("base64");
	token = token.replace(/\n/g, "").replace(/\+/g, "-").replace(/\//g, "_").replace(/=/g, "");
	
    if (isDirectory) {
		return parsedUrl.protocol+ "//" + parsedUrl.host + "/bcdn_token=" + token + parameterDataUrl + "&expires=" + expires + parsedUrl.pathname;
	} else {
		return parsedUrl.protocol + "//" + parsedUrl.host + parsedUrl.pathname + "?token=" + token + parameterDataUrl + "&expires=" + expires;
	}
}
const fs = require("fs");
const path = require("path");

const semver = require("semver");
const beautify = require("js-beautify");

const packageJSON = path.join(__dirname, "../package.json");

const package = JSON.parse(fs.readFileSync(packageJSON).toString());
let packageVersion = String(package.version);

const command = process.argv[2];
const version = process.argv[3];

let sV;

/**
 * Split the version string at the "-".
 */
const splitVersion = () => {
    if (packageVersion.includes("-")) {
        const split = packageVersion.split("-");

        return { left: split[0], right: split[1] };
    } else {
        return null;
    }
};

if (command !== "get-version" && command !== "set-version") {
    console.log("Invalid command. Available commands:\n\tget-version\n\tset-version");
    process.exit(1);
}

if (command === "set-version") {
    if (version == null || version === "") {
        console.log("Invalid semver bump type.");
        process.exit(1);
    }

    // Clean the version string
    sV = splitVersion();
    if (sV !== null) {
        if (sV.right.includes("a")) {
            // Reassemble the version string, remove the "a"
            packageVersion = `${sV.left}-${sV.right.replace("a", "")}`;
        }
    }

    packageVersion = semver.inc(packageVersion, version);

    // Exit on bump fail
    if (packageVersion === null) {
        console.log("Unable to bump semver.");
        process.exit(1);
    }

    // Add "a" before - for prerelease (alpha)
    if (version === "prerelease") {
        sV = splitVersion();
        if (sV !== null) {
            packageVersion = `${sV.left}-a${sV.right}`;
        }
    }

    package.version = packageVersion;

    const formattedPackage = beautify(JSON.stringify(package), {
        indent_with_tabs: true,
        end_with_newline: true,
        preserve_newlines: false,
        brace_style: "collapse",
    });

    fs.writeFileSync(packageJSON, formattedPackage);
} else if (command === "get-version") {
    console.log(package.version);
}
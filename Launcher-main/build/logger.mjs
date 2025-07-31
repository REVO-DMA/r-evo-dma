import ansiColors from "ansi-colors";

const logStyle = {
    info: ansiColors.magenta,
    warning: ansiColors.yellow,
    error: ansiColors.red,
    success: ansiColors.green,
};

const log = {
    info: (msg, out = true) => {
        if (out) {
            console.log(logStyle.info(msg));
        } else {
            return logStyle.info(msg);
        }
    },
    warning: (msg, out = true) => {
        if (out) {
            console.log(logStyle.warning(msg));
        } else {
            return logStyle.warning(msg);
        }
    },
    error: (msg, out = true) => {
        if (out) {
            console.log(logStyle.error(msg));
        } else {
            return logStyle.error(msg);
        }
    },
    success: (msg, out = true) => {
        if (out) {
            console.log(logStyle.success(msg));
        } else {
            return logStyle.success(msg);
        }
    },
};

export default log;

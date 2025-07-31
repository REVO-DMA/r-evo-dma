import { validate as uuidValidate } from "uuid";

export function isSessionDataValid(Session_ID) {
    // Ensure this session is using a proper format
    if (!uuidValidate(Session_ID)) {
        return { success: false, message: ["Session malformed."] };
    }

    return { success: true, message: [] };
}

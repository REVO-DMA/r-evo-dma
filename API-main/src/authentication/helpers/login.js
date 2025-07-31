export async function checkLoginInfo(email, password) {
    const errors = [];

    // Ensure all data is non-null
    if (email == null || email === "") errors.push("Email is empty.");
    if (password == null || password === "") errors.push("Password is empty.");

    // Terminate early if there is empty data
    if (errors.length > 0) return { success: false, message: errors };

    return { success: true, message: [] };
}

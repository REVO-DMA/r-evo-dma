export async function POST(uri: string, body: string): Promise<string>
{
    return new Promise((resolve, reject) => {
        fetch(uri, {
            method: "POST",
            redirect: "follow",
            body: body,
            headers: {
                "Content-Type": "application/json",
            },
        })
        .then((response) => response.text())
        .then((result) => resolve(result))
        .catch((error) => console.log(`[HTTP] Error: ${error}`));
    });
}

export async function GET(uri: string): Promise<string>
{
    return new Promise((resolve, reject) => {
        fetch(uri, {
            method: "GET",
            redirect: "follow",
        })
        .then((response) => response.text())
        .then((result) => resolve(result))
        .catch((error) => console.log(`[HTTP] Error: ${error}`));
    });
}

export async function GET_Raw(uri: string): Promise<ArrayBuffer>
{
    return new Promise((resolve, reject) => {
        fetch(uri, {
            method: "GET",
            redirect: "follow",
        })
        .then((response) => response.arrayBuffer())
        .then((result) => resolve(result))
        .catch((error) => console.log(`[HTTP] Error: ${error}`));
    });
}
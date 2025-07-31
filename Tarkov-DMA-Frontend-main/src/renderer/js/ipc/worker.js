import delay from "delay";
import asyncFS from "fs/promises";
import { createConnection } from "node:net";
import { initialize as initializeENV } from "../../env";
import { Maps } from "../rendering/maps/mapManager";

const IPC_RETRY_DELAY_MS = 1000;

const PIPE_NAME = "57617fb3-3424-4ac7-8598-e2206398acda"; // TODO: This should be gotten from the server
const IPC_DELIMITER = Buffer.from([0x5c, 0x72, 0x5c, 0x6e]); // \r\n

/** @type {import("node:net").Socket} */
let IPC_CLIENT;

let IPC_CONNECTED = false;

onmessage = (e) => {
    send(e.data[0], e.data[1]);
};

(async () => {
    await initializeENV(true);

    console.log("Attempting to connect to the IPC Server...");

    while (true) {
        const connectStatus = await tryConnect();
        await delay(IPC_RETRY_DELAY_MS);
        if (connectStatus) break;
    }

    console.log("Connected to IPC Server!");
})();

/**
 * @param {number} type
 * @param {any} data
 */
function send(type, data) {
    if (!IPC_CONNECTED) return;

    if (type === 69420) {
        sendMapConfigs();
    }

    const payload = {
        MessageType: type,
        Data: JSON.stringify(data),
    };

    try {
        IPC_CLIENT.write(JSON.stringify(payload) + "\n");
    } catch (err) {
        console.error(`IPC Client ERROR: Thrown while sending: ${err}`);
    }
}

/** @type {Buffer} */
let IPC_DATA_PARTS = null;

/**
 * @returns {Promise<boolean>}
 */
function tryConnect() {
    return new Promise((resolve) => {
        IPC_CLIENT = createConnection(`\\\\.\\pipe\\${PIPE_NAME}`, () => {
            IPC_CLIENT.on("ready", async () => {
                send(1, {});

                // Tell main thread IPC is ready
                const ipcReady = Buffer.alloc(1, 200);
                postMessage(ipcReady.buffer, [ipcReady.buffer]);
            });
        });

        IPC_CLIENT.on("data", (data) => {
            // Check the last 4 bytes of the buffer for the delimiter.
            const MESSAGE_CONTAINS_DELIMITER = data.subarray(data.length - 4).equals(IPC_DELIMITER);

            if (MESSAGE_CONTAINS_DELIMITER) {
                // This is the end of a message
                if (IPC_DATA_PARTS !== null) {
                    // Add last part to buffer after removing the delimiter.
                    IPC_DATA_PARTS = Buffer.concat([IPC_DATA_PARTS, data.subarray(0, data.length - 4)]);
                    postMessage(IPC_DATA_PARTS.buffer, [IPC_DATA_PARTS.buffer]);

                    IPC_DATA_PARTS = null;
                } else {
                    // Whole message came in one chunk
                    const buffer = data.buffer.slice(0, data.length - 4);
                    postMessage(buffer, [buffer]);
                }
            } else {
                // This is a chunked message
                if (IPC_DATA_PARTS === null) {
                    IPC_DATA_PARTS = data;
                } else {
                    // Add this part to the parts buffer
                    IPC_DATA_PARTS = Buffer.concat([IPC_DATA_PARTS, data]);
                }
            }
        });

        IPC_CLIENT.on("connect", () => {
            IPC_CONNECTED = true;
            resolve(true);
        });

        IPC_CLIENT.on("error", () => {
            console.log(`Failed to connect, trying again in ${IPC_RETRY_DELAY_MS} ms...`);
            resolve(false);
        });

        IPC_CLIENT.on("end", () => {
            IPC_CONNECTED = false;
            console.log("Disconnected from IPC Server.");
        });
    });
}

function sendMapConfigs() {
    const mapConfigs = [];

    Maps.forEach((value, key) => {
        mapConfigs.push({
            mapID: key,
            x: value.Properties.X,
            y: value.Properties.Y,
            scale: value.Properties.Scale,
            rotation: value.Properties.Rotation,
            pawnRotation: value.Properties.PawnRotation,
        });
    });

    send(24, mapConfigs);
}

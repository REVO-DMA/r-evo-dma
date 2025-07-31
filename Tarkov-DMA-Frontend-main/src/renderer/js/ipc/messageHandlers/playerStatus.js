import { destroyPlayer, setPlayerAlive, setPlayerDead, setPlayerExfil } from "../../rendering/player/playerManager";

/**
 * @param {import("../utils/BufferReader.js").default} bufferReader
 */
export async function deserialize(bufferReader) {
    const UniqueID = bufferReader.readUint16LE();
    const Status = bufferReader.readUInt8();

    if (Status === 1) {
        // Alive
        setPlayerAlive(UniqueID);
    } else if (Status === 2) {
        // Die
        setPlayerDead(UniqueID);
    } else if (Status === 3) {
        // Exfil
        setPlayerExfil(UniqueID);
    } else if (Status === 4) {
        // Destroy
        destroyPlayer(UniqueID);
    }
}

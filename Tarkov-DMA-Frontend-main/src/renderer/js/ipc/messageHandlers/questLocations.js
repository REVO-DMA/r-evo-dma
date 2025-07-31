import { update as updateQuestLocations } from "../../rendering/questLocation/questManager";

/**
 * @param {import("../utils/BufferReader.js").default} bufferReader
 */
export function deserialize(bufferReader) {
    // Loot update
    const lootCount = bufferReader.readInt32LE();

    /** @type {import("../../rendering/loot/lootManager").LootItem[]} */
    const lootItems = [];
    for (let i = 0; i < lootCount; i++) {
        const ID = bufferReader.readUInt64();
        const Shown = bufferReader.readBoolean();
        // Get Item name
        const BSG_ID_TextLength = bufferReader.readUInt8();
        let BSG_ID = null;
        if (BSG_ID_TextLength !== 0) BSG_ID = bufferReader.readString(BSG_ID_TextLength);

        const Type = bufferReader.readUInt8();

        // Get Item name
        const NameTextLength = bufferReader.readUInt8();
        let Name = null;
        if (NameTextLength !== 0) Name = bufferReader.readString(NameTextLength);

        /** @type {import("../../rendering/loot/lootManager.js").LootPosition} */
        const Position = [bufferReader.readFloatLE(), bufferReader.readFloatLE(), bufferReader.readInt16LE()];

        lootItems.push([ID, Shown, BSG_ID, Type, Name, Position]);
    }

    updateQuestLocations(lootItems);
}

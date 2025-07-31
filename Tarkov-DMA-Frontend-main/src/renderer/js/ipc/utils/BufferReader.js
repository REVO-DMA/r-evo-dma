export default class BufferReader {
    /**
     * @param {Buffer} buffer
     */
    constructor(buffer) {
        this.buffer = buffer;
        this.cursorPosition = 0;
    }

    readBoolean() {
        const value = this.buffer.readUInt8(this.cursorPosition);
        this.cursorPosition += 1;

        if (value === 1) return true;
        else return false;
    }

    readUInt8() {
        const value = this.buffer.readUInt8(this.cursorPosition);
        this.cursorPosition += 1;
        return value;
    }

    readUint16LE() {
        const value = this.buffer.readUint16LE(this.cursorPosition);
        this.cursorPosition += 2;
        return value;
    }

    readInt16LE() {
        const value = this.buffer.readInt16LE(this.cursorPosition);
        this.cursorPosition += 2;
        return value;
    }

    readInt32LE() {
        const value = this.buffer.readInt32LE(this.cursorPosition);
        this.cursorPosition += 4;
        return value;
    }

    readUInt64() {
        const value = this.buffer.readBigUint64LE(this.cursorPosition);
        this.cursorPosition += 8;
        return value;
    }

    readFloatLE() {
        const value = this.buffer.readFloatLE(this.cursorPosition);
        this.cursorPosition += 4;
        return value;
    }

    /**
     * @param {number} length
     */
    readString(length) {
        const value = this.buffer.toString("utf-8", this.cursorPosition, this.cursorPosition + length);
        this.cursorPosition += length;
        return value;
    }
}

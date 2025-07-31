const cameraBase = 0x480;
const cameraP1 = 0x10;

const offsets = [
    {
        offset: 0x20,
        size: 12,
        name: "Location",
        type: "Vector3",
    },
    {
        offset: 0x3C,
        size: 4,
        name: "FOV",
        type: "float",
    },
    {
        offset: 0x8,
        size: 12,
        name: "Rotation",
        type: "UE_Math.FRotator",
    },
];

const baseOffset = cameraBase + cameraP1;

// Sort by offset
offsets.sort((a, b) => a.offset - b.offset);

// Calculate padding for each offset
for (let i = 0; i < offsets.length; i++) {
    const current = offsets[i];
    const next = offsets[i + 1];

    if (next) {
        current.padding = next.offset - (current.offset + current.size);
    } else {
        current.padding = 0;
    }
}

// Create structs

console.log(`
public readonly struct CameraManager
{
    public const uint InfoBase = 0x${(baseOffset + offsets[0].offset).toString(16)}; // Same as ${offsets[0].name} - Check struct from dump to ensure it's not changed
}
`);

const cameraInfoStruct = [];

cameraInfoStruct.push("public unsafe struct CameraInfo");
cameraInfoStruct.push("{");

let paddingIndex = 1;
offsets.forEach(offset => {
    cameraInfoStruct.push(`\tpublic ${offset.type} ${offset.name};`);
    if (offset.padding > 0) {
        cameraInfoStruct.push(`\tprivate fixed byte _p${paddingIndex}[0x${offset.padding.toString(16).toUpperCase()}];`);
        paddingIndex++;
    }
});

cameraInfoStruct.push("}");

console.log(cameraInfoStruct.join("\n"));

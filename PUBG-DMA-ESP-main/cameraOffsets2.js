const offsets = [
    {
        offset: 0x1610,
        size: 12,
        name: "Location",
        type: "Vector3",
    },
    {
        offset: 0x1644,
        size: 4,
        name: "FOV",
        type: "float",
    },
    {
        offset: 0x1628,
        size: 12,
        name: "Rotation",
        type: "UE_Math.FRotator",
    },
];

// Sort by offset
offsets.sort((a, b) => a.offset - b.offset);

// Calculate padding for each offset
for (let i = 0; i < offsets.length; i++) {
    const first = offsets[0];
    const current = offsets[i];

    current.fieldOffset = current.offset - first.offset;
}

// Create structs

console.log(`
public readonly struct CameraManager
{
    public const uint InfoBase = 0x${offsets[0].offset.toString(16)}; // Same as ${offsets[0].name} - Check struct from dump to ensure it's not changed
}
`);

const cameraInfoStruct = [];

cameraInfoStruct.push("[StructLayout(LayoutKind.Explicit, Pack = 1)]");
cameraInfoStruct.push("public unsafe struct CameraInfo");
cameraInfoStruct.push("{");

offsets.forEach(offset => {
    cameraInfoStruct.push(`\t[FieldOffset(0x${offset.fieldOffset.toString(16).toUpperCase()})]`);

    cameraInfoStruct.push(`\tpublic ${offset.type} ${offset.name};`);
});

cameraInfoStruct.push("}");

console.log(cameraInfoStruct.join("\n"));
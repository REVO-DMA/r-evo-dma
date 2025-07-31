using MemoryPack;
using SkiaSharp;

[MemoryPackable]
public readonly partial struct ESP_RenderPacket
{
    public readonly PlayerESP_Data[] RenderData;

    public ESP_RenderPacket(PlayerESP_Data[] renderData)
    {
        RenderData = renderData;
    }
}

[MemoryPackable]
public readonly partial struct BoneBounds
{
    public readonly float MinX;
    public readonly float MinY;
    public readonly float MaxX;
    public readonly float MaxY;

    public BoneBounds(float minX, float minY, float maxX, float maxY)
    {
        MinX = minX;
        MinY = minY;
        MaxX = maxX;
        MaxY = maxY;
    }
}

[MemoryPackable]
public readonly partial struct PlayerESP_Data
{
    public readonly SKPoint[] BoneScreenPositions;
    public readonly SKPoint HeadPosition;
    public readonly BoneBounds BoneBounds;
    public readonly string Name;
    public readonly bool Visible;
    public readonly int Health;

    public PlayerESP_Data(SKPoint[] boneScreenPositions, SKPoint headPosition, BoneBounds boneBounds, string name, bool visible, int health)
    {
        BoneScreenPositions = boneScreenPositions;
        HeadPosition = headPosition;
        BoneBounds = boneBounds;
        Name = name;
        Visible = visible;
        Health = health;
    }
}
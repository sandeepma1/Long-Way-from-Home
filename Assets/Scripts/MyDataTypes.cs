[System.Serializable]
public class Vector2Byte
{
    public byte x;
    public byte y;
    public Vector2Byte() { }
    public Vector2Byte(byte _x, byte _y)
    {
        x = _x;
        y = _y;
    }
    public Vector2Byte(byte _x)
    {
        x = _x;
        y = 0;
    }
}

[System.Serializable]
public class Vector2Short
{
    public short x;
    public short y;
    public Vector2Short() { }
    public Vector2Short(short _x, short _y)
    {
        x = _x;
        y = _y;
    }
    public Vector2Short(short _x)
    {
        x = _x;
        y = 0;
    }
}
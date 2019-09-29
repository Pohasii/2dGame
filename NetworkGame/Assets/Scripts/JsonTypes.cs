using UnityEngine;

public struct NickName
{
    public string name;

    public static NickName FromJson(string msg)
    {
        return JsonUtility.FromJson<NickName>(msg);
    }
}

public struct Coord
{
    public int x;
    public int y;

    public Coord(Vector3 coord)
    {
        x = (int)coord.x;
        y = (int)coord.y;
    }

    public static Coord FromJson(string msg)
    {
        return JsonUtility.FromJson<Coord>(msg);
    }

    public static implicit operator Coord(Vector2 coord)
    {
        return new Coord { x = (int)coord.x, y = (int)coord.y };
    }

    public static implicit operator Vector2(Coord coord)
    {
        return new Vector2(coord.x, coord.y);
    }
}

public struct OtherCoord
{
    public string id;

    public Vector2Int coord;

    public static PlayerInfo FromJson(string msg)
    {
        return JsonUtility.FromJson<PlayerInfo>(msg);
    }
}

public struct OtherSize
{
    public string id;
    public int size;

    public static OtherSize FromJson(string msg)
    {
        return JsonUtility.FromJson<OtherSize>(msg);
    }
}

public struct Size
{
    public int size;

    public static Size FromJson(string msg)
    {
        return JsonUtility.FromJson<Size>(msg);
    }
}

public struct Id
{
    public string id;
}

public struct PlayerInfo
{
    public string id;

    public int name;
    public Vector2Int coord;
    public int size;

    public static PlayerInfo FromJson(string msg)
    {
        return JsonUtility.FromJson<PlayerInfo>(msg);
    }
}
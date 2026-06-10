using UnityEngine;

namespace RwbyAP.Models;

public struct Breakable
{
    public long ID;
    public string Name;
    public Vector3 Position;

    public Breakable(long id, string name, Vector3 position)
    {
        ID = id;
        Name = name;
        Position = position;
    }
}

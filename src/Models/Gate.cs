namespace RwbyAP.Models;

public struct Gate {
    public long ID;
    public string Name;
    public string Encounter;

    public Gate(long id, string name, string encounter)
    {
        ID = id;
        Name = name;
        Encounter = encounter;
    }
}

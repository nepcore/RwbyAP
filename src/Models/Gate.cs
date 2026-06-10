namespace RwbyAP.Models;

public struct Gate {
    public long ID;
    public string Name;
    public int Encounter;

    public Gate(long id, string name, int encounter)
    {
        ID = id;
        Name = name;
        Encounter = encounter;
    }
}

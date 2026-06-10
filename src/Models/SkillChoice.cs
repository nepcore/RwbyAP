namespace RwbyAP.Models;

public struct SkillChoice
{
    private string[] choices = [];
    public SkillChoice(params string[] ids)
    {
        choices = ids;
    }

    public string Selected {
        get
        {
            if (choices.Length == 0) return null;
            else if (choices.Length == 1) return choices[0];
            else if (RWBYAP.Connection == null) return null;
            else
            {
                // reconstructing random so order of items received doesn't
                // impact randomization, making one seed always use the same
                // choices, but allowing different choices for different seeds
                var random = new System.Random(RWBYAP.Connection.Seed.GetHashCode());
                return choices[random.Next(choices.Length)];
            }
        }
    }
}

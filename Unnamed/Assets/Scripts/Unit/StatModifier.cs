// A class that represents a temporary or permanent modification to a stat.
// This is not a MonoBehaviour. It's a plain C# object.
public class StatModifier
{
    public readonly StatType TargetStat;
    public readonly OperationType Type;
    public readonly float Value;
    // We can add duration later if needed, e.g., public int DurationInTurns;

    // The 'source' can be used to track where the modifier came from (e.g., an item, a skill).
    public readonly object Source;

    public StatModifier(StatType targetStat, OperationType type, float value, object source = null)
    {
        TargetStat = targetStat;
        Type = type;
        Value = value;
        Source = source;
    }
}
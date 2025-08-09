using System.Collections.Generic;
using System.Linq; // We need LINQ for easy filtering
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField]
    private CharacterStatSheet statSheet;
    public CharacterStatSheet StatSheet => statSheet;

    // _baseStats stores the character's unmodified, "sheet" values.
    private Dictionary<StatType, int> _baseStats;

    // A list to hold all active modifiers.
    private readonly List<StatModifier> _statModifiers = new List<StatModifier>();

    // No longer need _currentStats. The value is now calculated on the fly.
    // We keep the public property for read-only access from outside.
    public IReadOnlyDictionary<StatType, int> BaseStats => _baseStats;

    private void Awake()
    {
        InitializeStatsFromSheet();
    }

    private void InitializeStatsFromSheet()
    {
        _baseStats = new Dictionary<StatType, int>();
        if (statSheet == null)
        {
            Debug.LogError($"Stat Sheet is not assigned on {gameObject.name}!", this);
            return;
        }

        // This part is similar to before, but populates _baseStats
        _baseStats.Add(StatType.MaxHP, statSheet.maxHP);
        _baseStats.Add(StatType.MaxMP, statSheet.maxMP);
        // ... (Add all other stats from the sheet here, just like before)
        _baseStats.Add(StatType.PhysicalAttack, statSheet.physicalAttack);
        _baseStats.Add(StatType.PhysicalDefense, statSheet.physicalDefense);
        _baseStats.Add(StatType.InternalAttack, statSheet.internalAttack);
        _baseStats.Add(StatType.InternalDefense, statSheet.internalDefense);
        _baseStats.Add(StatType.Speed, statSheet.speed);
        _baseStats.Add(StatType.Accuracy, statSheet.accuracy);
        _baseStats.Add(StatType.Evasion, statSheet.evasion);
        _baseStats.Add(StatType.Strength, statSheet.strength);
        _baseStats.Add(StatType.Constitution, statSheet.constitution);
        _baseStats.Add(StatType.Willpower, statSheet.willpower);
        _baseStats.Add(StatType.Intelligence, statSheet.intelligence);
        _baseStats.Add(StatType.Dexterity, statSheet.dexterity);
        _baseStats.Add(StatType.MoveRange, statSheet.moveRange);
        _baseStats.Add(StatType.JumpHeight, statSheet.jumpHeight);

        // Current HP/MP are special. They are not 'base' stats but runtime values.
        // We'll manage them separately. For now, we can add them here for initialization.
        _baseStats.Add(StatType.HP, GetStat(StatType.MaxHP));
        _baseStats.Add(StatType.MP, GetStat(StatType.MaxMP));
    }

    /// <summary>
    /// Gets the final calculated value of a stat after all modifiers are applied.
    /// </summary>
    public int GetStat(StatType type)
    {
        // For current HP and MP, we just return the value directly to avoid applying modifiers like +STR to current health.
        if (type == StatType.HP || type == StatType.MP)
        {
            _baseStats.TryGetValue(type, out int specialValue);
            return specialValue;
        }

        // Start with the base value from the sheet.
        if (!_baseStats.TryGetValue(type, out int baseValue))
        {
            Debug.LogWarning($"Stat of type '{type}' not found in base stats for {gameObject.name}.");
            return 0;
        }

        float finalValue = baseValue;

        // Apply all ADDITIVE modifiers first.
        var additiveMods = _statModifiers.Where(m => m.TargetStat == type && m.Type == OperationType.Additive);
        foreach (var mod in additiveMods)
        {
            finalValue += mod.Value;
        }

        // Apply all MULTIPLICATIVE modifiers next.
        float multiplier = 1.0f;
        var multiplicativeMods = _statModifiers.Where(m => m.TargetStat == type && m.Type == OperationType.Multiplicative);
        foreach (var mod in multiplicativeMods)
        {
            multiplier += mod.Value; // e.g., a +20% mod would have Value = 0.2
        }

        finalValue *= multiplier;

        return Mathf.RoundToInt(finalValue);
    }

    /// <summary>
    /// For directly changing stats that are not affected by modifiers, like current HP.
    /// </summary>
    public void SetRawStat(StatType type, int value)
    {
        if (_baseStats.ContainsKey(type))
        {
            _baseStats[type] = value;
        }
    }

    /// <summary>
    /// For modifying stats like current HP or MP.
    /// </summary>
    public void ModifyRawStat(StatType type, int amount)
    {
        if (_baseStats.ContainsKey(type))
        {
            _baseStats[type] += amount;
        }
    }

    /// <summary>
    /// Adds a new modifier to this character.
    /// </summary>
    public void AddModifier(StatModifier modifier)
    {
        _statModifiers.Add(modifier);
    }

    /// <summary>
    /// Removes a modifier from this character.
    /// </summary>
    public void RemoveModifier(StatModifier modifier)
    {
        _statModifiers.Remove(modifier);
    }
}
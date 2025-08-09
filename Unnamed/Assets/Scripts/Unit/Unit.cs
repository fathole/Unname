using UnityEngine;

// Unit class remains the central "brain" of a character.
// It delegates data management to CharacterStats and orchestrates actions.
[RequireComponent(typeof(CharacterStats))]
public class Unit : MonoBehaviour
{
    public CharacterStats Stats { get; private set; }

    public string CharacterName => (Stats != null && Stats.StatSheet != null) ? Stats.StatSheet.characterName : gameObject.name;

    // A property to check if the unit is alive.
    // It now gets this info directly from the HP stat.
    public bool IsAlive => Stats != null && Stats.GetStat(StatType.HP) > 0;

    // We can add more component references here later.
    // public Animator Animator { get; private set; }
    // public SkillManager Skills { get; private set; }

    private void Awake()
    {
        Stats = GetComponent<CharacterStats>();
        // Future components would be initialized here.
        // Animator = GetComponent<Animator>();
    }

    // --- Public API for Actions ---

    /// <summary>
    /// Makes the unit take damage. The actual damage calculation should happen BEFORE calling this.
    /// This method is responsible for applying the damage and triggering subsequent effects.
    /// </summary>
    /// <param name="damageAmount">The final calculated amount of damage to take.</param>
    public void TakeDamage(int damageAmount)
    {
        // If already dead, do nothing.
        if (!IsAlive)
        {
            return;
        }

        // We use ModifyRawStat for HP, as it's a direct state change.
        Stats.ModifyRawStat(StatType.HP, -damageAmount);

        // Clamp HP to not go below zero.
        if (Stats.GetStat(StatType.HP) < 0)
        {
            Stats.SetRawStat(StatType.HP, 0);
        }

        Debug.Log($"{CharacterName} takes {damageAmount} damage. Current HP: {Stats.GetStat(StatType.HP)} / {Stats.GetStat(StatType.MaxHP)}");

        // --- Event-driven hooks for other systems ---
        // TODO: Broadcast an event like "OnUnitTookDamage" so UI and Animators can react.
        // Example: EventManager.Broadcast(Events.OnUnitTookDamage, this);

        // Check for death.
        if (!IsAlive)
        {
            Die();
        }
    }

    /// <summary>
    /// Makes the unit receive healing.
    /// </summary>
    /// <param name="healAmount">The amount of HP to restore.</param>
    public void Heal(int healAmount)
    {
        if (!IsAlive)
        {
            return;
        }

        Stats.ModifyRawStat(StatType.HP, healAmount);

        // Clamp HP to not exceed MaxHP.
        int maxHP = Stats.GetStat(StatType.MaxHP);
        if (Stats.GetStat(StatType.HP) > maxHP)
        {
            Stats.SetRawStat(StatType.HP, maxHP);
        }

        Debug.Log($"{CharacterName} heals for {healAmount}. Current HP: {Stats.GetStat(StatType.HP)} / {Stats.GetStat(StatType.MaxHP)}");

        // TODO: Broadcast an event like "OnUnitHealed" for UI and particle effects.
    }

    /// <summary>
    /// Handles the unit's death logic.
    /// This method acts as a trigger for other systems.
    /// </summary>
    private void Die()
    {
        // It's good practice to ensure HP is exactly 0 on death.
        Stats.SetRawStat(StatType.HP, 0);

        Debug.Log($"{CharacterName} has been defeated.");

        // --- Event-driven hooks ---
        // TODO: Broadcast an event "OnUnitDied".
        // The TurnManager can listen for this to remove the unit from the turn order.
        // The Animator can listen to play the death animation.
        // The visual effects system can listen to make the character's model fade out.

        // For now, as a simple placeholder, we can disable the unit.
        // This is not ideal for the long term but works for testing.
        // gameObject.SetActive(false);
    }
}
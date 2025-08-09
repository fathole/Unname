// Defines how a StatModifier affects a stat's value.
public enum OperationType
{
    // Additive modifiers are applied first.
    // e.g., +10 Strength
    Additive,

    // Multiplicative modifiers are applied after all additives.
    // e.g., +20% Strength
    Multiplicative
}
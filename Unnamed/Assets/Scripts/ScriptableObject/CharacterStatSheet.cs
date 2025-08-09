using UnityEngine;

// This ScriptableObject now uses individual variables for clarity in the Inspector.
// It still acts as a template for a character's base stats.
[CreateAssetMenu(fileName = "NewCharacterStatSheet", menuName = "Wuxia Tactics/Character Stat Sheet")]
public class CharacterStatSheet : ScriptableObject
{
    [Header("Character Information")]
    public string characterName = "New Character";

    [Header("Core Combat Stats")]
    public int maxHP = 100;
    public int maxMP = 50;
    public int physicalAttack = 10;
    public int physicalDefense = 5;
    public int internalAttack = 10;
    public int internalDefense = 5;
    public int speed = 8;
    public int accuracy = 95;
    public int evasion = 5;

    [Header("Fundamental Potential Stats")]
    public int strength = 10;
    public int constitution = 10;
    public int willpower = 10;
    public int intelligence = 10;
    public int dexterity = 10;

    [Header("Tactical Movement Stats")]
    public int moveRange = 4;
    public int jumpHeight = 2;
}
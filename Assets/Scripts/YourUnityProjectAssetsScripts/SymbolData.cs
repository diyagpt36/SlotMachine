using UnityEngine;

/// <summary>
/// ScriptableObject that defines a single slot symbol — its sprite and payout multiplier.
/// Create via: Assets > Create > SlotGame > Symbol Data
/// </summary>
[CreateAssetMenu(fileName = "NewSymbol", menuName = "SlotGame/Symbol Data")]
public class SymbolData : ScriptableObject
{
    [Header("Visuals")]
    [Tooltip("The sprite displayed on the reel for this symbol.")]
    public Sprite sprite;

    [Header("Identity")]
    [Tooltip("Unique name used for matching (e.g. Cherry, Bar, Seven).")]
    public string symbolName;

    [Header("Payout")]
    [Tooltip("Multiplier applied to the base bet when three of these land.")]
    public int payoutMultiplier = 1;

    [Header("Bonus")]
    [Tooltip("If true, landing 3 of this symbol triggers the bonus feature.")]
    public bool isBonusSymbol = false;
}

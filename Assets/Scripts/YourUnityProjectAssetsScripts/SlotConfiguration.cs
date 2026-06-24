using UnityEngine;

/// <summary>
/// ScriptableObject holding global game settings.
/// Create via: Assets > Create > SlotGame > Slot Configuration
/// </summary>
[CreateAssetMenu(fileName = "SlotConfig", menuName = "SlotGame/Slot Configuration")]
public class SlotConfiguration : ScriptableObject
{
    [Header("Economy")]
    [Tooltip("Starting balance for the player.")]
    public int startingBalance = 1000;

    [Tooltip("Cost per spin.")]
    public int betAmount = 10;

    [Header("Reels")]
    [Tooltip("How long (seconds) the reel spins before stopping.")]
    public float baseSpinDuration = 1.5f;

    [Tooltip("Extra delay added per reel so they stop one after another.")]
    public float reelStaggerDelay = 0.4f;

    [Tooltip("Total symbols visible on each reel (usually 3).")]
    public int visibleSymbolCount = 3;

    [Header("Animation")]
    [Tooltip("Pixels per second the symbols scroll during a spin.")]
    public float spinSpeed = 800f;

    [Tooltip("Overshoot (in pixels) for the bounce landing effect.")]
    public float bounceOvershoot = 30f;

    [Tooltip("Duration of the bounce settle animation.")]
    public float bounceDuration = 0.15f;
}

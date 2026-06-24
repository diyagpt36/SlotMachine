using UnityEngine;

/// <summary>
/// Evaluates the result of a spin across all reels and determines
/// the payout. Extend this class to add more paylines or bonus logic.
/// </summary>
public class PaylineManager : MonoBehaviour
{
    // ── Public API ─────────────────────────────────────────────────────────

    /// <summary>
    /// Holds the outcome of a single spin evaluation.
    /// </summary>
    public struct SpinResult
    {
        public bool isWin;
        public bool isBonusTrigger;
        public int payout;
        public string winDescription;
    }

    /// <summary>
    /// Evaluates the centre payline across all reels.
    /// </summary>
    /// <param name="reels">All ReelController components, left to right.</param>
    /// <param name="betAmount">The wager for this spin.</param>
    public SpinResult Evaluate(ReelController[] reels, int betAmount)
    {
        var result = new SpinResult();

        if (reels == null || reels.Length == 0)
            return result;

        // ── Check for three-of-a-kind on the main payline ──────────────────
        SymbolData first = reels[0].CurrentSymbol;
        bool allMatch = true;

        for (int i = 1; i < reels.Length; i++)
        {
            if (reels[i].CurrentSymbol.symbolName != first.symbolName)
            {
                allMatch = false;
                break;
            }
        }

        if (allMatch)
        {
            result.isWin = true;
            result.payout = betAmount * first.payoutMultiplier;
            result.winDescription = $"3x {first.symbolName}! ×{first.payoutMultiplier}";

            // Bonus trigger (e.g. landing three Scatter/Wild symbols)
            if (first.isBonusSymbol)
            {
                result.isBonusTrigger = true;
                result.winDescription += " — BONUS!";
            }
        }

        return result;
    }
}

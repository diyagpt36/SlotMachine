using UnityEngine;

/// <summary>
/// Central game-state controller. Manages the player's balance and
/// acts as the bridge between SlotMachine results and the UI.
/// Singleton — one instance lives for the entire session.
/// </summary>
public class GameManager : MonoBehaviour
{
    // ── Inspector ──────────────────────────────────────────────────────────
    [SerializeField] private SlotConfiguration config;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private SlotMachine slotMachine;

    // ── State ──────────────────────────────────────────────────────────────
    private int _balance;

    // ── Unity lifecycle ────────────────────────────────────────────────────

    private void Start()
    {
        _balance = config.startingBalance;
        uiManager.UpdateBalance(_balance);
        uiManager.SetSpinInteractable(true);
        uiManager.ShowMessage("Good luck!");
    }

    // ── Public API (called by SlotMachine) ─────────────────────────────────

    /// <summary>
    /// Attempts to deduct the bet from the balance.
    /// Returns false (and shows a message) if insufficient funds.
    /// </summary>
    public bool TryDeductBet(int amount)
    {
        if (_balance < amount)
        {
            uiManager.ShowMessage("Insufficient balance!");
            return false;
        }

        _balance -= amount;
        uiManager.UpdateBalance(_balance);
        return true;
    }

    /// <summary>Called as soon as the spin begins.</summary>
    public void OnSpinStarted()
    {
        uiManager.SetSpinInteractable(false);
        uiManager.ShowMessage("Spinning…");
        uiManager.HideWinPanel();
    }

    /// <summary>Called when all reels have settled.</summary>
    public void OnSpinComplete(PaylineManager.SpinResult result)
    {
        if (result.isWin)
        {
            _balance += result.payout;
            uiManager.UpdateBalance(_balance);
            uiManager.ShowWin(result.winDescription, result.payout);

            if (result.isBonusTrigger)
                TriggerBonusRound();
        }
        else
        {
            uiManager.ShowMessage("No win — try again!");
        }

        uiManager.SetSpinInteractable(true);

        // Game-over check.
        if (_balance <= 0)
        {
            uiManager.SetSpinInteractable(false);
            uiManager.ShowMessage("Out of coins! Refresh to play again.");
        }
    }

    // ── Bonus feature ──────────────────────────────────────────────────────

    /// <summary>
    /// Simple bonus: award 5× the bet as a free prize.
    /// Extend this for free spins, pick-a-box, etc.
    /// </summary>
    private void TriggerBonusRound()
    {
        int bonusPrize = config.betAmount * 5;
        _balance += bonusPrize;
        uiManager.UpdateBalance(_balance);
        uiManager.ShowMessage($"BONUS! +{bonusPrize} coins awarded!");
    }
}

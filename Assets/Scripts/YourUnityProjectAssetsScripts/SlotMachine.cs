using System.Collections;
using UnityEngine;

public class SlotMachine : MonoBehaviour
{
    [Header("Reels (left to right)")]
    [SerializeField] private ReelController[] reels;

    [Header("Dependencies")]
    [SerializeField] private SlotConfiguration config;
    [SerializeField] private PaylineManager paylineManager;
    [SerializeField] private GameManager gameManager;

    private bool _isSpinning;

    public bool IsSpinning => _isSpinning;

    /// <summary>Called by old spin button — deducts bet then spins.</summary>
    public void RequestSpin()
    {
        if (_isSpinning) return;
        if (!gameManager.TryDeductBet(config.betAmount)) return;
        StartCoroutine(SpinAllReels());
    }

    /// <summary>Called by BetPanel — bet already deducted, just spin.</summary>
    public void StartSpin()
    {
        if (_isSpinning) return;
        StartCoroutine(SpinAllReels());
    }

    private IEnumerator SpinAllReels()
    {
        _isSpinning = true;
        gameManager.OnSpinStarted();

        int reelsCompleted = 0;
        int totalReels = reels.Length;

        for (int i = 0; i < totalReels; i++)
        {
            float spinDuration = config.baseSpinDuration + i * config.reelStaggerDelay;
            int capturedIndex = i;

            reels[capturedIndex].Spin(spinDuration, () =>
            {
                reelsCompleted++;
                if (reelsCompleted == totalReels)
                    HandleSpinComplete();
            });

            yield return new WaitForSeconds(config.reelStaggerDelay * 0.5f);
        }
    }

    private void HandleSpinComplete()
    {
        var result = paylineManager.Evaluate(reels, config.betAmount);
        gameManager.OnSpinComplete(result);
        _isSpinning = false;
    }
}
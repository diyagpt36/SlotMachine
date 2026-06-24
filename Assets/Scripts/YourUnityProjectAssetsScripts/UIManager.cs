using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages all UI elements: balance display, win panel, message text,
/// and the spin button's interactable state.
/// </summary>
public class UIManager : MonoBehaviour
{
    // ── Inspector ──────────────────────────────────────────────────────────
    [Header("HUD")]
    [SerializeField] private TMP_Text balanceText;
    [SerializeField] private TMP_Text messageText;

    [Header("Spin Button")]
    [SerializeField] private Button spinButton;

    [Header("Win Panel")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text winAmountText;
    [SerializeField] private TMP_Text winDescriptionText;

    [Header("Win Panel Animation")]
    [Tooltip("How long the win panel stays visible before fading.")]
    [SerializeField] private float winDisplayDuration = 2.5f;

    // ── Coroutine handle ───────────────────────────────────────────────────
    private Coroutine _winPanelCoroutine;

    // ── Public API ─────────────────────────────────────────────────────────

    public void UpdateBalance(int balance)
    {
        if (balanceText != null)
            balanceText.text = $"Balance: {balance}";
    }

    public void ShowMessage(string message)
    {
        if (messageText != null)
            messageText.text = message;
    }

    public void SetSpinInteractable(bool interactable)
    {
        if (spinButton != null)
            spinButton.interactable = interactable;
    }

    public void ShowWin(string description, int amount)
    {
        if (winPanel == null) return;

        if (_winPanelCoroutine != null)
            StopCoroutine(_winPanelCoroutine);

        if (winDescriptionText != null) winDescriptionText.text = description;
        if (winAmountText != null)      winAmountText.text = $"+{amount}";

        winPanel.SetActive(true);
        _winPanelCoroutine = StartCoroutine(HideWinPanelAfterDelay());
    }

    public void HideWinPanel()
    {
        if (_winPanelCoroutine != null)
        {
            StopCoroutine(_winPanelCoroutine);
            _winPanelCoroutine = null;
        }

        if (winPanel != null)
            winPanel.SetActive(false);
    }

    // ── Private helpers ────────────────────────────────────────────────────

    private IEnumerator HideWinPanelAfterDelay()
    {
        yield return new WaitForSeconds(winDisplayDuration);
        HideWinPanel();
    }
}

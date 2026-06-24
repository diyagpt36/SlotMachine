using UnityEngine;
using UnityEngine.UI;

public class BetPanel : MonoBehaviour
{
    [Header("Bet Panel UI")]
    [SerializeField] private GameObject betPanel;
    [SerializeField] private Button bet10Button;
    [SerializeField] private Button bet50Button;
    [SerializeField] private Button bet100Button;
    [SerializeField] private Button exitButton;

    [Header("Dependencies")]
    [SerializeField] private SlotMachine slotMachine;
    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        bet10Button.onClick.AddListener(() => PlaceBet(10));
        bet50Button.onClick.AddListener(() => PlaceBet(50));
        bet100Button.onClick.AddListener(() => PlaceBet(100));

        if (exitButton != null)
            exitButton.onClick.AddListener(HidePanel);

        betPanel.SetActive(false);
    }

    public void ShowPanel()
    {
        betPanel.SetActive(true);
        SetButtonsInteractable(true);
    }

    public void HidePanel()
    {
        betPanel.SetActive(false);
    }

    private void PlaceBet(int amount)
    {
        // Disable buttons while spinning
        SetButtonsInteractable(false);

        if (gameManager.TryDeductBet(amount))
        {
            slotMachine.StartSpin();
            // Show panel again after spin duration
            Invoke(nameof(ShowPanel), 3f);
        }
        else
        {
            // Not enough balance — re-enable buttons
            SetButtonsInteractable(true);
        }
    }

    private void SetButtonsInteractable(bool state)
    {
        bet10Button.interactable = state;
        bet50Button.interactable = state;
        bet100Button.interactable = state;
    }
}
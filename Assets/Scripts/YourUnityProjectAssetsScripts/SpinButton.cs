using UnityEngine;
using UnityEngine.UI;

public class SpinButton : MonoBehaviour
{
    [SerializeField] private SlotMachine slotMachine;

    private void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
            btn.onClick.AddListener(() => slotMachine.RequestSpin());
    }
}
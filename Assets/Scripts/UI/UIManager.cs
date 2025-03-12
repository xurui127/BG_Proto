using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text turnText;
    [SerializeField] TMP_Text goldText;
    [SerializeField] public GameObject movementPanel;
    [SerializeField] public GameObject cardPanel;

    private void OnEnable()
    {
        GameManager.OnTurnChangedEvent += UpdateTurnText;
        GameManager.OnGoldChangedEvent += UpdateGoldText;
        GameManager.ClosePanelsEvent += ClosePanels;

    }
    private void OnDisable()
    {
        GameManager.OnTurnChangedEvent -= UpdateTurnText;
        GameManager.OnGoldChangedEvent -= UpdateGoldText;
        GameManager.ClosePanelsEvent -= ClosePanels;
    }

    public void UpdateTurnText(int turnNumber) => turnText.text = $"Turn: {turnNumber}";

    public void UpdateGoldText(int amount) => goldText.text = $"Gold: {amount}";   
    
    public void TigglePanel()
    {
        cardPanel.SetActive(!cardPanel.activeSelf);
        movementPanel.SetActive(!movementPanel.activeSelf);
    }

    private void ClosePanels()
    {
        movementPanel.SetActive(false);
        cardPanel.SetActive(false);
    }

}

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
        GameManager.OnTurnChanged += UpdateTurnText;
        GameManager.OnGoldChanged += UpdateGoldText;
    }
    private void OnDisable()
    {
        GameManager.OnTurnChanged -= UpdateTurnText;
        GameManager.OnGoldChanged -= UpdateGoldText;
    }

    public void UpdateTurnText(int turnNumber) => turnText.text = $"Turn: {turnNumber}";

    public void UpdateGoldText(int amount) => goldText.text = $"Gold: {amount}";   
    
    public void TiggleCardPanel()
    {
        cardPanel.SetActive(!cardPanel.activeSelf);
        movementPanel.SetActive(!movementPanel.activeSelf);
    }
}

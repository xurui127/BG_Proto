using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text turnText;

    private void OnEnable()
    {
        GameManager.OnTurnChanged += UpdateTurnText;
    }
    private void OnDisable()
    {
        GameManager.OnTurnChanged -= UpdateTurnText;
    }

    public void UpdateTurnText(int turnNumber) => turnText.text = $"Turn: {turnNumber}";
}

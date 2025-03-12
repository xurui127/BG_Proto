using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    public Button cardButton;
    public TMP_Text cardText;
    private UnityAction onClickAction;

    public void SetupCard(string name, UnityAction action)
    {
        cardText.text = name;
        onClickAction = action;
        cardButton.onClick.AddListener(onClickAction);
        if (!GameManager.Instance.IsPlayer())
        {
            cardButton.interactable = false;
        }
    }

    private void OnDestroy()
    {
        cardButton.onClick.RemoveAllListeners();
    }
}

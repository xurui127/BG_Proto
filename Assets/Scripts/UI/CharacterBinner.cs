using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBinner : MonoBehaviour
{
    [SerializeField] TMP_Text characterNameText;
    [SerializeField] TMP_Text fruitText;
    [SerializeField] TMP_Text goalText;
    [SerializeField] RawImage iconImage;

    CharacterData characterData;

    private void OnDisable()
    {
        if (characterData != null)
        {
            characterData.OnFruitCountChangedEvent -= UpdateFruitText;
            characterData.OnGoalCountChangedEvent -= UpdateGoalText;
        }
    }

    internal void Init(CharacterData data)
    {
        characterData = data;
    }

    internal void BindCharacterTextEvents()
    {
        characterData.OnFruitCountChangedEvent += UpdateFruitText;
        characterData.OnGoalCountChangedEvent += UpdateGoalText;
    }

    internal void UpdateFruitText(int amount)
    {
        fruitText.text = $"Fruits: {amount}";
    }

    internal void UpdateGoalText(int amount)
    {
        goalText.text = $"Goal: {amount}";
    }

    internal void UpdateNameText(string nameText, string indexText)
    {
        characterNameText.text = $"{nameText} {indexText}";
    }
}

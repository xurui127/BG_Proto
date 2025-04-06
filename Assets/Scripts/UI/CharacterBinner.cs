using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBinner : MonoBehaviour
{
    [SerializeField] TMP_Text characterNameText;
    [SerializeField] TMP_Text fruitText;
    [SerializeField] TMP_Text goalText;
    [SerializeField] RawImage iconImage;

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

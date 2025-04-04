using TMPro;
using UnityEngine;

public class CharacterBinner : MonoBehaviour
{
    [SerializeField] TMP_Text characterNameText;
    [SerializeField] TMP_Text fruitText;
    int fruitCount;

    internal void UpdateFruitText(int amount)
    {
        fruitText.text = $"Fruits: {amount}";

    }

    internal void UpdateNameText(string nameText, string indexText)
    {
        characterNameText.text = $"{nameText} {indexText}";
    }

}

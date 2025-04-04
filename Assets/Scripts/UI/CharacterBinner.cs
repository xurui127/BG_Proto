using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBinner : MonoBehaviour
{
    [SerializeField] TMP_Text characterNameText;
    [SerializeField] TMP_Text fruitText;
    [SerializeField] RawImage iconImage;

    internal void UpdateFruitText(int amount)
    {
        fruitText.text = $"Fruits: {amount}";

    }

    internal void UpdateNameText(string nameText, string indexText)
    {
        characterNameText.text = $"{nameText} {indexText}";
    }

    //internal void SetIconImage(Camera iconCam)
    //{
    //    iconCam.targetTexture = iconImage;
    //}
}

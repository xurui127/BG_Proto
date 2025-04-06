using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] TMP_Text turnText;
    [SerializeField] TMP_Text goldText;
    [SerializeField] public GameObject movementPanel;
    [SerializeField] public GameObject cardPanel;
    [SerializeField] public GameObject noCardPanel;
    [SerializeField] public GameObject backButton;
    [SerializeField] CharacterBinner[] characterBinners;
    [SerializeField] RenderTexture[] iconCamTextures;

    private void OnEnable()
    {
        GameManager.OnTurnChangedEvent += UpdateTurnText;
        GameManager.OnFruitChangeEvent += UpdateCharacterFruitCount;
        GameManager.ClosePanelsEvent += ClosePanels;

        FruitBehaviour.OnInteractEvent += UpdateCharacterFruitCount;
        PotBehaviour.OnInteractEvent += UpdateCharcterGoalCount;
    }

    private void OnDisable()
    {
        GameManager.OnTurnChangedEvent -= UpdateTurnText;
        GameManager.OnFruitChangeEvent -= UpdateCharacterFruitCount;
        GameManager.ClosePanelsEvent -= ClosePanels;

        FruitBehaviour.OnInteractEvent -= UpdateCharacterFruitCount;
        PotBehaviour.OnInteractEvent -= UpdateCharcterGoalCount;
    }

    public void UpdateTurnText(int turnNumber) => turnText.text = $"Turn: {turnNumber}";

   // public void UpdateGoldText(int amount) => goldText.text = $"Gold: {amount}";   
    
    //internal void UpdateFruitText(int amout, int index) => 
    public void TigglePanel()
    {
        cardPanel.SetActive(!cardPanel.activeSelf);
        movementPanel.SetActive(!movementPanel.activeSelf);
    }

    public void OpenNoCardsPanel(bool isOpen) => noCardPanel.SetActive(isOpen);
  

    public void IsHideBackButton(bool isHide)
    {
        backButton.SetActive(isHide);
    }
    private void ClosePanels()
    {
        movementPanel.SetActive(false);
    }

    internal void SetupCharacterBinners(int count, List<CharacterData> allData,List<CharacterBehaviour> allBehaviours)
    {
        for (int i = 0; i <= count; i++)
        {
            characterBinners[i].gameObject.SetActive(true);

            bool isPlayer = i == 0;
            string nameText = isPlayer ? "Player" : "NPC";

            characterBinners[i].UpdateFruitText(allData[i].fruitCount);
            characterBinners[i].UpdateNameText(nameText,i.ToString());
            allBehaviours[i].SetupIConCam(iconCamTextures[i]);
        }
    }

    internal void UpdateCharacterFruitCount(int index, int amount)
    {
        characterBinners[index].UpdateFruitText(amount);
    }

    internal void UpdateCharcterGoalCount(int index, int amount)
    {
        characterBinners[index].UpdateGoalText(amount);
    }
}

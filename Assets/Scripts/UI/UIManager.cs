using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] TMP_Text turnText;
    [SerializeField] TMP_Text goldText;
    [SerializeField] public GameObject movementPanel;
    [SerializeField] public GameObject cardPanel;
    [SerializeField] public GameObject noCardPanel;
    [SerializeField] GameObject[] characterIconprefabs;
    [SerializeField] Transform[] iconSpawnPoints;
    [SerializeField] CharacterBinner[] characterBinners;
    [SerializeField] RenderTexture[] iconCamTextures;
    [SerializeField] GameObject endPanel;
    [SerializeField] GameObject winText;
    [SerializeField] GameObject loseText;

    private void OnEnable()
    {
        GameManager.OnTurnChangedEvent += UpdateTurnText;
        GameManager.OnFruitChangeEvent += UpdateCharacterFruitCount;
        GameManager.ClosePanelsEvent += ClosePanels;

        PotBehaviour.OpenEndPanelEvent += OpenEndPanel;
    }

    private void OnDisable()
    {
        GameManager.OnTurnChangedEvent -= UpdateTurnText;
        GameManager.OnFruitChangeEvent -= UpdateCharacterFruitCount;
        GameManager.ClosePanelsEvent -= ClosePanels;
        PotBehaviour.OpenEndPanelEvent -= OpenEndPanel;
    }

    public void UpdateTurnText(int turnNumber) => turnText.text = $"Turn: {turnNumber}";

    public void TigglePanel()
    {
        cardPanel.SetActive(!cardPanel.activeSelf);
        movementPanel.SetActive(!movementPanel.activeSelf);
    }

    public void OpenNoCardsPanel(bool isOpen) => noCardPanel.SetActive(isOpen);

    private void ClosePanels()
    {
        movementPanel.SetActive(false);
    }

    internal void SetupCharacterBinners(int count, List<CharacterData> allData, List<CharacterBehaviour> allBehaviours)
    {
        for (int i = 0; i <= count; i++)
        {
            characterBinners[i].gameObject.SetActive(true);

            bool isPlayer = i == 0;
            string nameText = isPlayer ? "Player" : "NPC";
            int index = isPlayer ? i + 1 : i;

            characterBinners[i].UpdateNameText(nameText, index.ToString());
            characterBinners[i].UpdateFruitText(allData[i].FruitCount);
            characterBinners[i].UpdateGoalText(allData[i].GoalCount);

            characterBinners[i].Init(allData[i]);
            characterBinners[i].BindCharacterTextEvents();

            var prefab = isPlayer ? characterIconprefabs[0]: characterIconprefabs[1];
            var icon = Instantiate(prefab, iconSpawnPoints[i].position,Quaternion.identity).GetComponent<IconProxy>();
            icon.gameObject.transform.parent = iconSpawnPoints[i];
            icon.SetupIConCam(iconCamTextures[i]);
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

    internal void OpenEndPanel(bool isWin)
    {
        endPanel.SetActive(true);
        winText.SetActive(isWin);
        loseText.SetActive(!isWin);
    }

    public void BackToTitleSceen()
    {
        SceneManager.LoadScene("Title");
    }

}

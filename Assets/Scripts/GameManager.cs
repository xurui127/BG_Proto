using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] UIManager uiManager;
    [SerializeField] BoardManager boardManager;
    [SerializeField] CardSystem cardSystem;
    [SerializeField] GameObject PlayerPrefab;
    [SerializeField] GameObject EnemyPrefab;
    [SerializeField] GameObject dicePrefab;
    [SerializeField] Transform[] diceSpawnPoint;

    [SerializeField] int TextStep = 0;

    [HideInInspector] public List<Transform> pathTile;

    const int maxFruitCount = 3;
    int currentFruitCount = 0;

    int characterCount = 0;
    int characterIndex = 0;
    int turnNumber = 0;
    int diceNumber = 0;

    CharacterBehaviour currentCharacterBehaviour;
    CharacterData currentCharacterData;
    List<CharacterBehaviour> allCharacterBehaviours = new();
    List<CharacterData> allCharacterData = new();

    public readonly FSMController stateMachine = new();
    public static UnityAction<int> OnTurnChangedEvent;
    public static UnityAction<int, int> OnFruitChangeEvent;
    public static UnityAction ClosePanelsEvent;

    public FSMController GetStateController() => stateMachine;

    public bool IsCharacterMovingDone() => currentCharacterBehaviour.isDoneMoving;

    public bool IsPlayer() => currentCharacterBehaviour.isPlayer;

    public void SetMovementPanel(bool isOpen) => uiManager.movementPanel.SetActive(isOpen);

    public bool IsEmptyCard() => currentCharacterData.hand.Count == 0;

    protected override void Awake()
    {
        base.Awake();
        allCharacterBehaviours = new();
        currentFruitCount = maxFruitCount;
    }

    // Start is called before the first frame update
    void Start()
    {
        stateMachine.RegisterState(new DrawCardState(this));
        stateMachine.RegisterState(new DecisionState(this));
        stateMachine.RegisterState(new MoveState(this));
        stateMachine.RegisterState(new WaitForDiceResultState(this));
        stateMachine.RegisterState(new RetrieveCardState(this));
        stateMachine.RegisterState(new EndTurnState(this));


        characterCount = GameSettings.enemyCount;
        UpdateTurnNumber();
        pathTile = boardManager.tiles;
        InitCharacters();
        SetCharacterBinner();
        AddCardsToCharacter();
        UpdateCameraTarget(currentCharacterBehaviour.gameObject);
        InitFruits();
        InitPot();
        stateMachine.SetState<DrawCardState>();
    }

    private void Update()
    {
        stateMachine.OnUpdate();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            RollSpecificDice(TextStep);
        }
    }

    private void InitCharacters()
    {
        for (int i = 0; i <= characterCount; i++)
        {
            int tileIndex = boardManager.GetAvailiableTiles()[i];
            // For Character in same tile Test
            //int tileIndex = 1;
            int capturedIndex = tileIndex;

            Quaternion rotation = boardManager.GetDirction(capturedIndex);
            Vector3 spawnPosition = boardManager.tiles[capturedIndex].position + new Vector3(0, 0.4f, 0);

            bool isPlayer = i == 0;
            var prefab = isPlayer ? PlayerPrefab : EnemyPrefab;
            var character = Instantiate(prefab, spawnPosition, rotation)
                                       .GetComponent<CharacterBehaviour>();
            var data = character.gameObject.GetComponent<CharacterData>();
            cardSystem.GenerateDeck(data);
            character.currentTileIndex = capturedIndex;
            allCharacterBehaviours.Add(character);
            allCharacterData.Add(data);

            if (isPlayer)
            {
                currentCharacterBehaviour = character;
                currentCharacterData = data;
            }
            data.index = i;
            boardManager.tileBehaviours[tileIndex].PlacedCharacter();
            boardManager.tileBehaviours[tileIndex].RegisterCharacter(character);
        }
    }

    private void InitFruits() => boardManager.InitFruits(maxFruitCount);

    private void InitPot() => boardManager.InitPot();

    private void AddCardsToCharacter()
    {
        foreach (var data in allCharacterData)
        {
            foreach (var card in cardSystem.cards)
            {
                data.currentCards.Add(card.id, card);
            }
        }
    }

    public void RollDice() => RollDiceInternal(1, null);

    public void RollSpecificDice(int step) => RollDiceInternal(1, step);

    public void RollTwoDices() => RollDiceInternal(2, null);

    private void RollDiceInternal(int diceCount, int? step)
    {
        ClosePanelsEvent?.Invoke();
        diceNumber = 0;

        for (int i = 0; i < diceCount; i++)
        {
            var spawnPosition = diceSpawnPoint[i + (diceCount - 1)];
            var dice = Instantiate(dicePrefab,
                                   spawnPosition.position,
                                   Quaternion.identity);
            var rolledValue = dice.GetComponent<Dice>().Roll(step);
            diceNumber += rolledValue;
        }

        if (step != null)
        {
            RemoveCards("20002");
        }
        if (diceCount == 2)
        {
            RemoveCards("20003");
        }

        stateMachine.SetState<WaitForDiceResultState>();
    }

    public void MoveCharacter()
    {
        if (currentCharacterBehaviour != null)
        {
            currentCharacterBehaviour.MovePath(diceNumber);
        }
    }

    internal void ResetCurrentTile()
    {
        var tileIndex = currentCharacterBehaviour.GetCurrentTileIndex();
        foreach (var character in allCharacterBehaviours)
        {
            if (character == currentCharacterBehaviour)
            {
                continue;
            }
            if (character.currentTileIndex == tileIndex)
            {
                return;
            }
        }

        boardManager.ResetPlacedCharacter(tileIndex);
    }

    internal void RegesterCurrentTile()
    {
        var tileIndex = currentCharacterBehaviour.GetCurrentTileIndex();
        boardManager.RegesterPlacedCharacter(tileIndex);
    }

    public void SetNextCharacterTurn()
    {
        currentCharacterData.DiscardHand();
        characterIndex = (characterIndex + 1) % allCharacterBehaviours.Count;
        currentCharacterBehaviour = allCharacterBehaviours[characterIndex];
        currentCharacterData = allCharacterData[characterIndex];
        UpdateCameraTarget(currentCharacterBehaviour.gameObject);
        if (characterIndex == 0)
        {
            UpdateTurnNumber();
        }
        cardSystem.ResetCardsDate();
    }

    private void UpdateCameraTarget(GameObject target)
    {
        if (virtualCamera != null)
        {
            virtualCamera.Follow = target.transform;
            virtualCamera.LookAt = target.transform;
        }
    }

    private void UpdateTurnNumber()
    {
        turnNumber++;
        OnTurnChangedEvent?.Invoke(turnNumber);
    }

    public void InitCards()
    {
        cardSystem.DrawCards(currentCharacterData);
    }

    internal void OpenPanels()
    {
        uiManager.cardPanel.SetActive(true);
        uiManager.movementPanel.SetActive(true);
    }

    public void AddFruits(int amount)
    {
        var index = allCharacterData.IndexOf(currentCharacterData);
        OnFruitChangeEvent?.Invoke(index, currentCharacterData.UpdateFruits(amount));
        ClosePanelsEvent?.Invoke();
        RemoveCards("20001");
    }

    public void UseRandomCard()
    {
        cardSystem.AIPlayCard();
    }

    private void RemoveCards(string id)
    {
        currentCharacterData.currentCards.Remove(id);
    }

    internal void PlayWorldCardFlyoutAnimation() => cardSystem.PlayWorldCardFlyoutAnimation();

    private void SetCharacterBinner()
    {
        uiManager.SetupCharacterBinners(characterCount, allCharacterData, allCharacterBehaviours);
    }

    internal void CheckIteractItems()
    {
        var tileIndex = currentCharacterBehaviour.currentTileIndex;
        var currentTile = boardManager.GetCurrentTile(tileIndex);
        var currentItem = currentTile.GetCurrentItemBehaviour();

        if (currentTile.isPlacedFruit)
        {
            currentFruitCount--;
        }

        if (currentTile != null &&
            currentItem != null &&
            (currentTile.isPlacedFruit ||
            currentTile.isPlacedPot))
        {
            currentItem.OnInteract(currentCharacterData);
        }

        currentTile.ResetTilePlacedFruit(currentTile.isPlacedFruit);
    }

    internal void PlaceFruit()
    {
        if (currentFruitCount != maxFruitCount)
        {
            currentFruitCount++;
            UpdateCameraTarget(boardManager.InitPlacedFruit());
        }
    }

    internal void RegisterCurrentCharacterOnTile()
    {
        boardManager.RegesterCurrentCharacterOnTile(currentCharacterBehaviour.GetCurrentTileIndex(),
                                                    currentCharacterBehaviour);
    }
    internal void UnregisterCurrentCharacterOnTile()
    {
        boardManager.UnregesterCurrentChracterOntile(currentCharacterBehaviour.GetCurrentTileIndex(),
                                                    currentCharacterBehaviour);
    }

}

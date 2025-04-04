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

    [HideInInspector] public List<Transform> pathTile;

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
    public static UnityAction<int> OnGoldChangedEvent;
    public static UnityAction<int, int> OnFruitChangeEvent;
    public static UnityAction ClosePanelsEvent;

    public FSMController GetStateController() => stateMachine;

    public bool IsCharacterMovingDone() => currentCharacterBehaviour.isDoneMoving;

    public bool IsPlayer() => currentCharacterBehaviour.isPlayer;

    public void SetMovementPanel(bool isOpen) => uiManager.movementPanel.SetActive(isOpen);

    public bool IsEmptyCard() => currentCharacterData.currentCards.Count == 0;

    protected override void Awake()
    {
        base.Awake();
        allCharacterBehaviours = new();
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
        UpdateCameraTarget();
        stateMachine.SetState<DrawCardState>();
    }

    private void Update()
    {
        stateMachine.OnUpdate();
    }

    private void InitCharacters()
    {
        for (int i = 0; i <= characterCount; i++)
        {
            int tileIndex = boardManager.GetRandomTileIndex();
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
        }
    }

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

    public void SetNextCharacterTurn()
    {
        currentCharacterData.DiscardHand();
        characterIndex = (characterIndex + 1) % allCharacterBehaviours.Count;
        currentCharacterBehaviour = allCharacterBehaviours[characterIndex];
        currentCharacterData = allCharacterData[characterIndex];
        UpdateCameraTarget();
        if (characterIndex == 0)
        {
            UpdateTurnNumber();
        }
        OnGoldChangedEvent?.Invoke(currentCharacterData.fruitCount);
        cardSystem.ResetCardsDate();
    }

    private void UpdateCameraTarget()
    {
        if (virtualCamera != null)
        {
            virtualCamera.Follow = currentCharacterBehaviour.transform;
            virtualCamera.LookAt = currentCharacterBehaviour.transform;
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
        OnFruitChangeEvent?.Invoke(index, currentCharacterData.AddFruits(amount));
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
        uiManager.SetupCharacterBinners(characterCount,allCharacterData, allCharacterBehaviours);
    }
}

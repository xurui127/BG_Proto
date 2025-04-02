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
    List<CharacterBehaviour> characterBehaviours = new();
    List<CharacterData> allCharacterData = new();

    public readonly FSMController stateMachine = new();
    public static UnityAction<int> OnTurnChangedEvent;
    public static UnityAction<int> OnGoldChangedEvent;
    public static UnityAction ClosePanelsEvent;

    public FSMController GetStateController() => stateMachine;

    public bool IsCharacterMovingDone() => currentCharacterBehaviour.isDoneMoving;

    public bool IsPlayer() => currentCharacterBehaviour.isPlayer;

    public void SetMovementPanel(bool isOpen) => uiManager.movementPanel.SetActive(isOpen);

    public bool IsEmptyCard() => currentCharacterData.currentCards.Count == 0;

    protected override void Awake()
    {
        base.Awake();
        characterBehaviours = new();
    }

    // Start is called before the first frame update
    void Start()
    {
        stateMachine.RegisterState(new DrawCardState(this));
        stateMachine.RegisterState(new DecisionState(this));
        stateMachine.RegisterState(new MoveState(this));
        stateMachine.RegisterState(new WaitForDiceResultState(this));
        stateMachine.RegisterState(new EndTurnState(this));


        characterCount = GameSettings.enemyCount;
        UpdateTurnNumber();
        pathTile = boardManager.tiles;
        InitCharacters();
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
        for (int i = 0; i < characterCount + 1; i++)
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
            characterBehaviours.Add(character);
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
        characterIndex = (characterIndex + 1) % characterBehaviours.Count;
        currentCharacterBehaviour = characterBehaviours[characterIndex];
        currentCharacterData = allCharacterData[characterIndex];
        UpdateCameraTarget();
        if (characterIndex == 0)
        {
            UpdateTurnNumber();
        }
        OnGoldChangedEvent?.Invoke(currentCharacterData.gold);
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


    public void AddGold(int amount)
    {
        OnGoldChangedEvent?.Invoke(currentCharacterData.AddGold(amount));
        ClosePanelsEvent?.Invoke();
        RemoveCards("20001");
    }

    public void UseRandomCard()
    {
        var cardList = cardSystem.GetCurrentCards();

        if (cardList.Length != 0)
        {
            var index = Random.Range(0, cardList.Length);
            cardList[index].cardButton.onClick?.Invoke();
        }
    }

    private void RemoveCards(string id)
    {
        currentCharacterData.currentCards.Remove(id);
    }
}
// Enemy turn 
//1. roll dice 
//2. move enemy 
//3. endTurn

// Character turn 
//1. roll dice 
//2. move Path
//3. end Turn 

//GameState 
//1. player move 
//2. enemy move

#region Game Loop Couroutine
// [SerializeField] PlayerBehaviour player;
//readonly float rollForce = 5f;
//readonly float torqueForce = 10f;
//var rb = dice.GetComponent<Rigidbody>();
//rb.AddForce(Vector3.up * rollForce, ForceMode.Impulse);
//rb.AddTorque(Random.insideUnitSphere * torqueForce, ForceMode.Impulse);
//private void GameLoop()
//{
//    StartCoroutine(GameLoopCoroutine());
//    IEnumerator GameLoopCoroutine()
//    {
//        while (true)
//        {
//            switch (state)
//            {
//                case GameState.Roll:
//                    yield return RollPhaseCoroutine();
//                    break;
//                case GameState.WaittingDice:
//                    yield return WaitForDiceResultCoroutine();
//                    break;
//                case GameState.Move:
//                    yield return MovePhaseCoroutine();
//                    break;
//                case GameState.EndTurn:
//                    yield return EndTurnPhaseCoroutine();
//                    break;
//            }
//        }
//    }
//}

//private IEnumerator RollPhaseCoroutine()
//{
//    currentCharacter = characters[characterIndex];
//    if (currentCharacter.isPlayer)
//    {
//        rollPanel.SetActive(true);
//    }
//    else
//    {
//        yield return new WaitForSeconds(1f);
//        RollDice();
//    }
//}

//private IEnumerator WaitForDiceResultCoroutine()
//{
//    yield return new WaitForSeconds(2f);
//    var dice = currentDice.GetComponent<Dice>();
//    yield return new WaitUntil(() => dice.isResultFound);
//    state = GameState.Move;
//}

//private IEnumerator MovePhaseCoroutine()
//{
//    yield return WaitForCharacterMoveCoroutine();
//    yield return new WaitUntil(() => currentCharacter.isDoneMoving);
//    state = GameState.EndTurn;
//}

//private IEnumerator WaitForCharacterMoveCoroutine()
//{
//    yield return new WaitForSeconds(1f);
//    Destroy(currentDice);
//    MoveCharacter();
//}

//private IEnumerator EndTurnPhaseCoroutine()
//{
//    characterIndex = (characterIndex + 1) % characters.Count;
//    currentCharacter = characters[characterIndex];
//    UpdateCameraTarget();
//    if (characterIndex == 0)
//    {
//        turnNumber++;
//        UpdateTurnText();
//    }
//    yield return new WaitForSeconds(0.5f);
//    state = GameState.Roll;
//    yield return null;
//}
#endregion
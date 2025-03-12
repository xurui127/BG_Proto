using Cinemachine;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<GameManager>();
            }
            if (instance == null)
            {
                GameObject obj = new("GameManager");
                instance = obj.AddComponent<GameManager>();
            }
            return instance;
        }
    }

    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] GameObject dicePrefab;
    [SerializeField] Transform diceSpawnPoint;
    [SerializeField] BoardManager boardManager;
    [SerializeField] PlayerBehaviour player;
    [SerializeField] GameObject PlayerPrefab;
    [SerializeField] GameObject EnemyPrefab;
    [SerializeField] TMP_Text turnText;
    [SerializeField] DeckSystem deckSystem;
    [SerializeField] UIManager UIManager;

    public readonly FSMController stateMachine = new();
    readonly float rollForce = 5f;
    readonly float torqueForce = 10f;
    int characterCount = 0;
    int characterIndex = 0;
    int turnNumber = 0;

    GameObject currentDice;
    CharacterBehaviour currentCharacter;
    CharacterData currentData;
    List<CharacterBehaviour> characters = new();
    List<CharacterData> characterDatas = new();

    public List<Transform> pathTile;
    public int diceNumber = 0;

    public static UnityAction<int> OnTurnChangedEvent;
    public static UnityAction<int> OnGoldChangedEvent;
    public static UnityAction ClosePanelsEvent;

    public Dice GetCurrentDice() => currentDice.GetComponent<Dice>();

    public FSMController GetStateController() => stateMachine;

    public void DestroyDice() => Destroy(currentDice);

    public bool IsCharacterMovingDone() => currentCharacter.isDoneMoving;

    public bool IsPlayer() => currentCharacter.isPlayer;

    public void SetMovementPanel(bool isOpen) => UIManager.movementPanel.SetActive(isOpen);


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        characters = new();
    }

    // Start is called before the first frame update
    void Start()
    {
        stateMachine.RegisterState(new RollState(this));
        stateMachine.RegisterState(new MoveState(this));
        stateMachine.RegisterState(new WaitForDiceResultState(this));
        stateMachine.RegisterState(new EndTurnState(this));

        characterCount = GameSettings.enemyCount;
        UpdateTurnNumber();
        pathTile = boardManager.tiles;
        InitCharacters();
        AddCardsToCharacter();
        UpdateCameraTarget();
        stateMachine.SetState<RollState>();
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

            var prefab = (i == 0) ? PlayerPrefab : EnemyPrefab;
            var character = Instantiate(prefab, spawnPosition, rotation)
                                       .GetComponent<CharacterBehaviour>();
            var data = character.gameObject.GetComponent<CharacterData>();
            character.currentTileIndex = capturedIndex;
            characters.Add(character);
            characterDatas.Add(data);

            if (i == 0)
            {
                currentCharacter = character;
                currentData = data;
            }
        }
    }

    private void AddCardsToCharacter()
    {
        foreach (var data in characterDatas)
        {
            foreach (var card in deckSystem.cards)
            {
                data.currentCards.Add(card);
            }
        }
    }

    public void RollDice()
    {
        SetMovementPanel(false);
        var dice = Instantiate(dicePrefab,
                               diceSpawnPoint.position,
                               Quaternion.identity);
        //var rb = dice.GetComponent<Rigidbody>();
        //rb.AddForce(Vector3.up * rollForce, ForceMode.Impulse);
        //rb.AddTorque(Random.insideUnitSphere * torqueForce, ForceMode.Impulse);

        currentDice = dice;
        diceNumber = dice.GetComponent<Dice>().RollDice();
        stateMachine.SetState<WaitForDiceResultState>();
    }

    public void MoveCharacter()
    {
        if (currentCharacter != null)
        {
            currentCharacter.MovePath(diceNumber);
        }
    }

    public void SetNextCharacterTurn()
    {
        characterIndex = (characterIndex + 1) % characters.Count;
        currentCharacter = characters[characterIndex];
        currentData = characterDatas[characterIndex];
        UpdateCameraTarget();
        if (characterIndex == 0)
        {
            UpdateTurnNumber();
        }
    }

    private void UpdateCameraTarget()
    {
        if (virtualCamera != null)
        {
            virtualCamera.Follow = currentCharacter.transform;
            virtualCamera.LookAt = currentCharacter.transform;
        }
    }

    private void UpdateTurnNumber()
    {
        turnNumber++;
        OnTurnChangedEvent?.Invoke(turnNumber);
    }

    public void InitCards()
    {
        UIManager.cardPanel.SetActive(true);
        deckSystem.GenerateCards(currentData);
        UIManager.movementPanel.SetActive(false);
    }

    public void AddGold(int amount)
    {
        OnGoldChangedEvent?.Invoke(currentData.AddGold(amount));
        ClosePanelsEvent?.Invoke();
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
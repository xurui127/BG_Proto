using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GameState
{
    Roll,
    WaittingDice,
    Move,
    EndTurn,
}

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

    FSMController stateMachine = new();

    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] GameObject dicePrefab;
    [SerializeField] Transform diceSpawnPoint;
    [SerializeField] GameObject rollPanel;
    [SerializeField] BoardManager boardManager;
    [SerializeField] PlayerBehaviour player;
    [SerializeField] GameObject PlayerPrefab;
    [SerializeField] GameObject EnemyPrefab;
    [SerializeField] TMP_Text turnText;

    readonly float rollForce = 5f;
    readonly float torqueForce = 10f;
    int characterCount = 0;
    int characterIndex = 0;
    int turnNumber = 1;
    GameObject currentDice;
    CharacterBehaviour currentCharacter;
    List<CharacterBehaviour> characters;

    public List<Transform> pathTile;
    public GameState state;
    public int diceNumber = 0;

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
        UpdateTurnText();
        state = GameState.Roll;
        rollPanel.SetActive(true);
        pathTile = boardManager.tiles;
        InitCharacters();
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
            Vector3 spawnPosition = boardManager.tiles[capturedIndex].position;

            if (i == 0)
            {
                currentCharacter = Instantiate(PlayerPrefab, spawnPosition, rotation)
                                              .GetComponent<CharacterBehaviour>();
                currentCharacter.currentTileIndex = capturedIndex;
                characters.Add(currentCharacter);
            }
            else
            {
                var enemy = Instantiate(EnemyPrefab, spawnPosition, rotation)
                                       .GetComponent<CharacterBehaviour>();
                enemy.currentTileIndex = capturedIndex;
                characters.Add(enemy);
            }
        }
    }

    public List<CharacterBehaviour> GetCharacterList() => characters;

    public Dice GetCurrentDice() => currentDice.GetComponent<Dice>();

    public FSMController GetStateController() => stateMachine;

    public void DestroyDice() => Destroy(currentDice);

    public bool IsCharacterMovingDone() => currentCharacter.isDoneMoving;

    public bool IsPlayer() => currentCharacter.isPlayer;

    public void SetRollPanel(bool isOpen) => rollPanel.SetActive(isOpen);

    public void RollDice()
    {
        rollPanel.SetActive(false);
        var dice = Instantiate(dicePrefab,
                               diceSpawnPoint.position,
                               Quaternion.identity);
        var rb = dice.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * rollForce, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * torqueForce, ForceMode.Impulse);
        currentDice = dice;
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
        UpdateCameraTarget();
        if (characterIndex == 0)
        {
            turnNumber++;
            UpdateTurnText();
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

    private void UpdateTurnText() => turnText.text = $"Turn: {turnNumber}";
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
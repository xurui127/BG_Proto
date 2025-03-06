using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    PlayerRoll,
    EnemyRoll,
    PlayerMove,
    EnemyMove,
    WaitingForDice,
    ResetSettings,
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

    [SerializeField] GameObject dicePrefab;
    [SerializeField] Transform diceSpawnPoint;
    [SerializeField] GameObject rollPanel;
    [SerializeField] BoardManager boardManager;
    [SerializeField] PlayerBehaviour player;
    [SerializeField] List<EnemyBehaviour> enemy;
    [SerializeField] List<CharacterBehaviour> characters;
    [SerializeField] GameObject PlayerPrefab;
    [SerializeField] GameObject EnemyPrefab;


    GameObject currentDice;
    CharacterBehaviour currentCharacter;
    readonly float rollForce = 5f;
    readonly float torqueForce = 10f;
    int characterIndex = 2;
    int diceTopNumber = 0;
    bool isEnemyRoll = false;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        state = GameState.PlayerRoll;
        rollPanel.SetActive(true);
        pathTile = boardManager.tiles;
        InitCharacters();
    }

   
    // Update is called once per frame
    void Update()
    {
        if (state == GameState.PlayerRoll)
        {
            rollPanel.SetActive(true);
        }
        if (state == GameState.EnemyRoll)
        {
            if (isEnemyRoll) return;
            isEnemyRoll = true;
            RollDice();
        }
        if (state == GameState.ResetSettings)
        {
            diceNumber = 0;
            isEnemyRoll = false;
            rollPanel.SetActive(true);
            state = GameState.PlayerRoll;
        }
    }

    private void InitCharacters()
    {
        for (int i = 0; i < characterIndex; i++)
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

    public void RollDice()
    {
        rollPanel.SetActive(false);
        var dice = Instantiate(dicePrefab, diceSpawnPoint.position, Quaternion.identity);
        var rb = dice.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * rollForce, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * torqueForce, ForceMode.Impulse);
        currentDice = dice;
        if (state == GameState.PlayerRoll)
        {
            state = GameState.PlayerMove;
        }
        else if (state == GameState.EnemyRoll)
        {
            state = GameState.EnemyMove;
        }
    }

    private void MovePlayer()
    {
        if (player != null)
        {
            player.MovePath(diceNumber);
        }
    }
    private void MoveEnemy()
    {
        if (enemy != null)
        {
            enemy[0].MovePath(diceNumber);
        }
    }
    internal void PlayerWaitForDiceResult()
    {
        StartCoroutine(WaitForDiceResultCoroutine());
        IEnumerator WaitForDiceResultCoroutine()
        {
            rollPanel.SetActive(false);
            yield return new WaitForSeconds(1f);
            Destroy(currentDice);
            MovePlayer();
        }
    }
    internal void EnemyWaitForDiceResult()
    {
        StartCoroutine(WaitForDiceResultCoroutine());
        IEnumerator WaitForDiceResultCoroutine()
        {
            yield return new WaitForSeconds(2f);
            Destroy(currentDice);
            MoveEnemy();
        }
    }
    internal void SetMoveStep(int n)
    {
        diceNumber = n;

        switch (state)
        {
            case GameState.PlayerMove:
                state = GameState.WaitingForDice;
                PlayerWaitForDiceResult();
                break;
            case GameState.EnemyMove:
                state = GameState.WaitingForDice;
                rollPanel.SetActive(false);
                EnemyWaitForDiceResult();
                break;
            default:
                Debug.LogWarning("Not In Correct State");
                break;
        }
    }

    // Enemy turn 
    //1. roll dice 
    //2. move enemy 
    //3. endTurn

    // Character behaviour 
    //1. roll dice 
    //2. move Path
    //3. end Turn 

    //GameState 
    //1. player move 
    //2. enemy move


}

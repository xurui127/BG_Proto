using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Roll,
    PlayerMove,
    WaitingForDice
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
    public List<Transform> pathTile;

    GameObject currentDice;
    readonly float rollForce = 5f;
    readonly float torqueForce = 10f;

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
        state = GameState.Roll;
        pathTile = boardManager.tiles;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == GameState.Roll)
        {
            rollPanel.SetActive(true);
        }

        if (state == GameState.PlayerMove)
        {
            diceNumber = 0;
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
        state = GameState.WaitingForDice;
        rollPanel.SetActive(false);
    }

    private void MovePlayer()
    {
        if (player != null)
        {
            player.MovePath(diceNumber);
        }
    }

    internal void WaitForDiceResult()
    {
        StartCoroutine(WaitForDiceResultCoroutine());
        IEnumerator WaitForDiceResultCoroutine()
        {
            yield return new WaitForSeconds(1f);
            Destroy(currentDice);
            MovePlayer();
        }
    }
}

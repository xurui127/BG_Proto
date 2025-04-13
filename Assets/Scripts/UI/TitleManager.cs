using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject errorPanel;
    [SerializeField] TMP_Text enemyCount;
    [SerializeField] TMP_InputField randomSeedField;

    const int maxCount = 3;
    private int currentCount = 0;
    private int setSeed;

    public void OpenStartPanel()
    {
        setSeed = Random.Range(1000, 10000);
        randomSeedField.text = setSeed.ToString();
        startPanel.SetActive(true);
        UpdateEnemyCountText();
    }

    public void CloseStartPanel()
    {
        startPanel.SetActive(false);
    }

    public void StartGame()
    {
        if (currentCount == 0)
        {
            errorPanel.SetActive(true);
            return;
        }
        GameSettings.enemyCount = currentCount;

        int.TryParse(randomSeedField.text, out setSeed);
        Debug.Log("Set seed to " + setSeed);
        UnityEngine.Random.InitState(setSeed);
        SceneManager.LoadScene("Main");
    }

    public void IncreaseValue()
    {
        if (currentCount < maxCount)
        {
            currentCount++;
            UpdateEnemyCountText();
        }
    }

    public void DecreaseValue()
    {
        if (currentCount > 0)
        {
            currentCount--;
            UpdateEnemyCountText();
        }
    }

    public void UpdateEnemyCountText() => enemyCount.text = currentCount.ToString();
}

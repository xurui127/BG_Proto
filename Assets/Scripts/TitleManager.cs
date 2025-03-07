using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GameObject startPanel;
    [SerializeField] TMP_Text enemyCount;
    [SerializeField] GameObject errorPanel;

    const int maxCount = 2;
    private int currentCount = 0;

    public void OpenStartPanel()
    {
        startPanel.SetActive(true);
    }

    public void StartGame()
    {
        if (currentCount == 0)
        {
            errorPanel.SetActive(true);
            return;
        }
        GameSettings.enemyCount = currentCount; 
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

    public void UpdateEnemyCountText()
    {
        enemyCount.text = currentCount.ToString();
    }
}

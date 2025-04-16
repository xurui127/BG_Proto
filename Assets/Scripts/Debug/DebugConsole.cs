using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DebugConsole : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TMP_Text log;
    [SerializeField] GameObject defaultLog;
    internal static bool IsOpen { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            IsOpen = !IsOpen;
            panel.SetActive(IsOpen);
            if (IsOpen)
            {
                log.text = "Open";
                defaultLog = EventSystem.current.currentSelectedGameObject;
                EventSystem.current.SetSelectedGameObject(inputField.gameObject);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(defaultLog);
                defaultLog = null;
            }
        }
    }

    public void OnEndEdit()
    {
        string input = inputField.text;
        if (string.IsNullOrEmpty(input))
        {
            return;
        }

        AddLog(input);
        inputField.text = "";
        inputField.ActivateInputField();
    }

    private void AddLog(string input)
    {
        if (log.text.Length > 0)
        {
            log.text += "\n";
        }
        log.text += input;
    }
}

using System;
using System.Collections.Generic;
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

    Dictionary<string, DebugCommand> commands = new();


    private void Awake()
    {
        var debugCommandTypes = Util.GetTypesWith<DebugCommandAttribute>();
        foreach (var debugCommandType in debugCommandTypes)
        {
            var debugCommand = (DebugCommand)Activator.CreateInstance(debugCommandType);

            var attrs = Attribute.GetCustomAttributes(debugCommand.GetType());

            foreach (System.Attribute attr in attrs)
            {
                if (attr is DebugCommandAttribute a)
                {
                    commands.Add(a.commandName, debugCommand);
                    break;
                }
            }
        }
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
                log.text = "";
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

        ProcessInput(input.ToLowerInvariant());
    }

    private void AddLog(string input)
    {
        if (log.text.Length > 0)
        {
            log.text += "\n";
        }
        log.text += input;
    }

    private void ProcessInput(string input)
    {
        var words = input.Split(' ');
        if (commands.ContainsKey(words[0]))
        {
            string log = commands[words[0]].OnCommand(words);
            AddLog(log);
        }
        else
        {
            AddLog($"Cannot find command {words[0]}.");
        }
    }
}

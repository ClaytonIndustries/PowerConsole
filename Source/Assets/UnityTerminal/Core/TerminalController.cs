using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CI.UnityTerminal.Core
{
    public class TerminalController : MonoBehaviour, IDragHandler
    {
        private const int _maxCommandHistory = 50;
        private const int _defaultMaxBufferSize = 150;
        private const string _clearCommand = "clear";
        private const string _helpCommand = "help";

        private bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; UpdateVisibility(); }
        }

        public string Title
        {
            get => _title.text;
            set => _title.text = value;
        }

        public bool IsEnabled { get; set; }
        public LogLevel LogLevel { get; set; } = LogLevel.Trace;
        public TerminalColours Colours { get; set; } = new TerminalColours();
        public int MaxBufferSize { get; set; } = _defaultMaxBufferSize;
        public List<KeyCode> OpenCloseHotkeys { get; set; } = new List<KeyCode>() { KeyCode.LeftControl, KeyCode.I };

        public event EventHandler<CommandEnteredEventArgs> CommandEntered;

        private TMP_InputField _text;
        private TMP_InputField _input;
        private TextMeshProUGUI _title;
        private Button _closeButton;
        private Scrollbar _scrollbar;

        private bool _isFollowingTail = true;
        private int _commandHistoryIndex = -1;

        private readonly Queue<string> _buffer = new Queue<string>(_defaultMaxBufferSize);
        private readonly List<string> _commandHistory = new List<string>(_maxCommandHistory);
        private Dictionary<string, CustomCommand> _commands = new Dictionary<string, CustomCommand>();

        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _text = GameObject.Find("TerminalFeed").GetComponent<TMP_InputField>();
            _input = GameObject.Find("TerminalInput").GetComponent<TMP_InputField>();
            _title = GameObject.Find("TerminalTitle").GetComponent<TextMeshProUGUI>();
            _closeButton = GameObject.Find("CloseButton").GetComponent<Button>();
            _scrollbar = GameObject.Find("Scrollbar Vertical").GetComponent<Scrollbar>();

            _closeButton.onClick.AddListener(() =>
            {
                IsVisible = false;
                UpdateVisibility();
            });

            _scrollbar.onValueChanged.AddListener(x =>
            {
                _isFollowingTail = x > 0.9;
            });

            RegisterCommand(new CustomCommand()
            {
                Command = "clear",
                Description = "Clears the terminal",
                Callback = e => ClearDisplay()
            });
            RegisterCommand(new CustomCommand()
            {
                Command = "help",
                Description = "Displays help content",
                Callback = e => ShowHelpContent()
            });

            UpdateVisibility();
        }

        public void Update()
        {
            if (OpenCloseHotkeys.Any() &&
                OpenCloseHotkeys.All(x => Input.GetKey(x)) && 
                Input.GetKeyDown(OpenCloseHotkeys.Last()))
            {
                IsVisible = !IsVisible;
                UpdateVisibility();
            }

            if (IsVisible)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    OnEnterPressed();
                }
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    IncrementCommandHistory(-1);
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    IncrementCommandHistory(1);
                }
            }
        }

        public void Log(LogLevel logLevel, string message, bool forceScroll)
        {
            if (_buffer.Count >= MaxBufferSize)
            {
                _buffer.Dequeue();
            }

            var colour = GetColour(logLevel);

            if (logLevel == LogLevel.None) 
            {
                _buffer.Enqueue($"<color=#{colour}>{DateTime.Now:HH:mm:ss} > {message}</color>");
            }
            else
            {
                _buffer.Enqueue($"<color=#{colour}>{DateTime.Now:HH:mm:ss} [{logLevel}] {message}</color>");
            }

            if (IsVisible)
            {
                RefreshDisplay();

                if (forceScroll || _isFollowingTail)
                {
                    ScrollToEnd();
                }
            }
        }

        public void ClearDisplay()
        {
            _text.text = string.Empty;
            _buffer.Clear();
        }

        public void RegisterCommand(CustomCommand command)
        {
            _commands[command.Command] = command;
        }

        public void UnregisterCommand(string command)
        {
            if (_commands.ContainsKey(command))
            {
                _commands.Remove(command);
            }
        }

        private void UpdateVisibility()
        {
            gameObject.transform.localScale = IsVisible ? Vector3.one : Vector3.zero;

            if (IsVisible)
            {
                RefreshDisplay();
                ScrollToEnd();
                FocusInput();
            }
        }

        private void OnEnterPressed()
        {
            Log(LogLevel.None, _input.text, true);

            HandleCommands();

            UpdateCommandHistory();

            CommandEntered?.Invoke(this, new CommandEnteredEventArgs() { Command = _input.text });
            ClearInput();
            FocusInput();
        }

        private void ClearInput()
        {
            _input.text = string.Empty;
        }

        private void FocusInput()
        {
            _input.ActivateInputField();
        }

        private void ShowHelpContent()
        {
            foreach (var command in _commands)
            {
                var descriptionString = string.IsNullOrWhiteSpace(command.Value.Description) ? string.Empty : $" - {command.Value.Description}";
                var argString = command.Value.Args != null && command.Value.Args.Any() ? $" | {string.Join(" ", command.Value.Args.Select(x => $"[{x.Name}]{(string.IsNullOrWhiteSpace(x.Description) ? string.Empty : $" {x.Description}")}"))}" : string.Empty;

                Log(LogLevel.None, $"{command.Key}{descriptionString}{argString}", true);
            }
        }

        private string GetColour(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    return ColorUtility.ToHtmlStringRGB(Colours.TraceColour);
                case LogLevel.Debug:
                    return ColorUtility.ToHtmlStringRGB(Colours.DebugColor);
                case LogLevel.Information:
                    return ColorUtility.ToHtmlStringRGB(Colours.InformationColor);
                case LogLevel.Warning:
                    return ColorUtility.ToHtmlStringRGB(Colours.WarningColor);
                case LogLevel.Error:
                    return ColorUtility.ToHtmlStringRGB(Colours.ErrorColor);
                case LogLevel.Critical:
                    return ColorUtility.ToHtmlStringRGB(Colours.CriticalColor);
                default:
                    return ColorUtility.ToHtmlStringRGB(Colours.NoneColour);
            }
        }

        private void RefreshDisplay()
        {
            _text.text = string.Join(Environment.NewLine, _buffer);
        }

        private void ScrollToEnd()
        {
            _scrollbar.value = 1;
        }

        private void UpdateCommandHistory()
        {
            if (_commandHistory.Count >= _maxCommandHistory)
            {
                _commandHistory.RemoveAt(0);
                _commandHistory.Add(_input.text);
            }
            else
            {
                _commandHistory.Add(_input.text);
            }
        }

        private void IncrementCommandHistory(int value)
        {
            _commandHistoryIndex += value;

            if (_commandHistoryIndex < -1)
            {
                _commandHistoryIndex = _commandHistory.Count - 1;
            }
            else if (_commandHistoryIndex > _commandHistory.Count -1)
            {
                _commandHistoryIndex = -1;
            }

            if (_commandHistoryIndex >= 0)
            {
                _input.text = _commandHistory[_commandHistoryIndex];
            }
            else
            {
                _input.text = string.Empty;
            }
        }

        private void HandleCommands()
        {
            string command;
            var args = new Dictionary<string, string>();
            var indexOfFirstArg = _input.text.IndexOf(" -");

            if (indexOfFirstArg >= 0)
            {
                command = _input.text.Substring(0, indexOfFirstArg).Trim();

                var argString = _input.text.Substring(indexOfFirstArg + 1).Trim();
                var argsList = new List<string>();

                var index = 0;
                var ignoreSpaces = false;
                var builder = new StringBuilder();
                while (index < argString.Length)
                {
                    if (!ignoreSpaces && argString[index] == '\"')
                    {
                        ignoreSpaces = true;
                    }
                    else if (ignoreSpaces && argString[index] == '\"')
                    {
                        ignoreSpaces = false;
                    }

                    if ((argString[index] != ' ' || (argString[index] == ' ' && ignoreSpaces)) && argString[index] != '\"')
                    {
                        builder.Append(argString[index]);
                    }
                    if ((argString[index] == ' ' && !ignoreSpaces) || index == argString.Length - 1)
                    {
                        argsList.Add(builder.ToString());
                        builder.Clear();
                    }
                    index++;
                }

                index = 0;
                while (index < argsList.Count)
                {
                    var noArg = false;
                    var option = argsList[index];
                    var arg = index + 1 < argsList.Count ? argsList[index + 1]: string.Empty;

                    if (arg.StartsWith("-"))
                    {
                        arg = string.Empty;
                        noArg = true;
                    }

                    args[option] = arg;

                    index += noArg ? 1 : 2;
                }
            }
            else
            {
                command = _input.text.Trim();
            }

            if (_commands.ContainsKey(command))
            {
                _commands[command].Callback?.Invoke(new CommandCallback()
                {
                    Command = command,
                    Args = args
                });
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }
    }
}
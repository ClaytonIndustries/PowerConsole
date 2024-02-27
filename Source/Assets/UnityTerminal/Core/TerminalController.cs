using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CI.UnityTerminal.Core
{
    public class TerminalController : MonoBehaviour, IDragHandler
    {
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
        public int MaxBufferSize { get; set; } = 150;
        public List<KeyCode> OpenCloseHotkeys { get; set; } = new List<KeyCode>() { KeyCode.LeftControl, KeyCode.I };

        public event EventHandler<CommandEnteredEventArgs> CommandEntered;

        private ScrollRect _scroll;
        private TextMeshProUGUI _text;
        private TMP_InputField _input;
        private TextMeshProUGUI _title;
        private Button _closeButton;
        private Scrollbar _scrollbar;

        private bool _isFollowingTail = true;

        private readonly Queue<string> _buffer = new Queue<string>();

        public void Awake()
        {
            DontDestroyOnLoad(gameObject);

            _scroll = GameObject.Find("Scroll View").GetComponent<ScrollRect>();
            _text = GameObject.Find("TerminalFeed").GetComponent<TextMeshProUGUI>();
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
                _isFollowingTail = x < 0.1;
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

            if (IsVisible && Input.GetKeyDown(KeyCode.Return))
            {
                OnEnterPressed();
            }
        }

        public void Log(LogLevel logLevel, string message, bool forceScroll)
        {
            if (logLevel < LogLevel)
            {
                return;
            }

            if (_buffer.Count >= MaxBufferSize)
            {
                _buffer.Dequeue();
            }

            var colour = GetColour(logLevel);

            if (logLevel == LogLevel.None) 
            {
                _buffer.Enqueue($"<color=#{colour}>{DateTime.Now:dd-MM-yy hh:mm:ss} > {message}</color>");
            }
            else
            {
                _buffer.Enqueue($"<color=#{colour}>{DateTime.Now:dd-MM-yy hh:mm:ss} [{logLevel}] {message}</color>");
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

        private void UpdateVisibility()
        {
            gameObject.transform.localScale = IsVisible ? Vector3.one : Vector3.zero;

            if (IsVisible)
            {
                RefreshDisplay();
                FocusInput();
                ScrollToEnd();
            }
        }

        private void OnEnterPressed()
        {
            if (_input.text == "clear")
            {
                ClearDisplay();
            }
            else
            {
                Log(LogLevel.None, _input.text, true);
            }

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
            StartCoroutine(ApplyScrollToEnd());
        }

        IEnumerator ApplyScrollToEnd()
        {
            yield return new WaitForEndOfFrame();
            _scroll.normalizedPosition = new Vector2(0, 0);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }
    }
}
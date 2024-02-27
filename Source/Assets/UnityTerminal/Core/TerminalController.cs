using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CI.UnityTerminal.Core
{
    public class TerminalController : MonoBehaviour, IDragHandler
    {
        private bool isVisible;
        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; SetVisibility(); }
        }

        public bool IsEnabled { get; set; }
        public int MaxBufferSize { get; set; } = 1000;

        public event EventHandler<CommandEnteredEventArgs> CommandEntered;

        private ScrollRect _scroll;
        private TextMeshProUGUI _text;
        private TMP_InputField _input;
        private Button _closeButton;
        private readonly Queue<string> _buffer = new Queue<string>();

        public void Awake()
        {
            DontDestroyOnLoad(gameObject);

            _scroll = GameObject.Find("Scroll View").GetComponent<ScrollRect>();
            _text = GameObject.Find("TerminalFeed").GetComponent<TextMeshProUGUI>();
            _input = GameObject.Find("TerminalInput").GetComponent<TMP_InputField>();
            _closeButton = GameObject.Find("CloseButton").GetComponent<Button>();

            _closeButton.onClick.AddListener(() =>
            {
                IsVisible = false;
                SetVisibility();
            });

            //_text.color = Color.green;

            SetVisibility();
        }

        public void Update()
        {
            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.I)) 
            {
                IsVisible = !IsVisible;
                SetVisibility();
            }

            if (IsVisible && Input.GetKeyDown(KeyCode.Return))
            {
                OnEnterPressed();
            }
        }

        public void Log(LogLevel logLevel, string message)
        {
            if (_buffer.Count >= MaxBufferSize)
            {
                _buffer.Dequeue();
            }

            var colour = GetColour(logLevel);

            if (logLevel == LogLevel.None) 
            {
                _buffer.Enqueue($"<color=\"{colour}\">{DateTime.Now:dd-MM-yyyy hh:mm:ss} {message}</color>");
            }
            else
            {
                _buffer.Enqueue($"<color=\"{colour}\">{DateTime.Now:dd-MM-yyyy hh:mm:ss} [{logLevel}] {message}</color>");
            }

            _text.text = string.Join(Environment.NewLine, _buffer);
            ScrollToEnd();
        }

        public void ClearDisplay()
        {
            _text.text = string.Empty;
            _buffer.Clear();
        }

        private void SetVisibility()
        {
            gameObject.transform.localScale = IsVisible ? Vector3.one : Vector3.zero;

            if (IsVisible)
            {
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
                Log(LogLevel.None, _input.text);
            }

            if (!string.IsNullOrEmpty(_input.text))
            {
                CommandEntered?.Invoke(this, new CommandEnteredEventArgs() { Command = _input.text });
                ClearInput();
                FocusInput();
            }
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
                case LogLevel.Error:
                    return "red";
                case LogLevel.Trace:
                    return "white";
                default:
                    return "green";
            }
        }

        private void ScrollToEnd()
        {
            if (IsVisible)
            {
                StartCoroutine(ApplyScrollToEnd());
            }
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
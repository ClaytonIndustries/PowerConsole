using UnityEngine;

namespace CI.UnityTerminal.Core
{
    public class CanvasController : MonoBehaviour
    {
        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
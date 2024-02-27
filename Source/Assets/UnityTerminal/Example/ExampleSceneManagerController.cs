using CI.UnityTerminal;
using UnityEngine;

public class ExampleSceneManagerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UnityTerminal.Initialise();
        UnityTerminal.IsEnabled = true;

        for (int i = 0; i < 500; i++)
        {
            UnityTerminal.Log(LogLevel.Error, "Hello World");
            UnityTerminal.Log(LogLevel.Trace, "Yo World");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
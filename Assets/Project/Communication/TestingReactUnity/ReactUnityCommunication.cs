using UnityEngine;
using System.Runtime.InteropServices;
using TMPro;

public class ReactUnityCommunication : Singleton<ReactUnityCommunication>
{
    public TextMeshProUGUI _latestMessage;

    [DllImport("__Internal")]
    private static extern void ReactMessage(string message);

    public void SendAldenAnjing()
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
    ReactMessage("Testing Alden Anjing");
#endif
    }

    public void SpawnNiggers(string message)
    {
        _latestMessage.text = message;
    }
}

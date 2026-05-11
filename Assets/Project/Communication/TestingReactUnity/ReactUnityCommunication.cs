using UnityEngine;
using System.Runtime.InteropServices;
using TMPro;

public class ReactUnityCommunication : Singleton<ReactUnityCommunication>
{
    public TextMeshProUGUI _latestMessage;

    [DllImport("__Internal")]
    private static extern void ReactMessage(string message, int number);

    public void SendAldenAnjing()
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
    ReactMessage("Testing Alden Anjing", 10);
#endif
    }

    // ini yang lu panggil
    public void SpawnNiggers(string message)
    {
        _latestMessage.text = message;
    }
}

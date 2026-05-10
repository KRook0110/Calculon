using UnityEngine;
using System.Runtime.InteropServices;

public class ReactUnityCommunication : Singleton<ReactUnityCommunication>
{
    [DllImport("__Internal")]
    private static extern void ReactMessage(string message);

    public void SendAldenAnjing()
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
    ReactMessage("Testing Alden Anjing");
#endif
    }
}

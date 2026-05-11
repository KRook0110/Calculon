using UnityEngine;

public class LevelSelectionReactHandler : Singleton<LevelSelectionReactHandler>
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ReactUnityCommunication.Instance.SendInit();
    }

}

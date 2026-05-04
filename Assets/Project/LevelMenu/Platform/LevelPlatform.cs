using UnityEngine;
using UnityEngine.EventSystems;

/*
* Handles the level platform, when clicked it will set the level selector to the preset level for the platform
*/
public class LevelPlatform : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private LevelData level;

    public void OnPointerClick(PointerEventData eventData)
    {
        LevelSelector.Instance.selectedLevel = level;
    }
}

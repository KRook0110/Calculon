using UnityEngine;
using NaughtyAttributes;
using System.IO;

public class SaveDataViewer : MonoBehaviour
{
    [ReadOnly]
    [SerializeField]
    private string _savePath;

    [Header("Current Saved Data")]
    [SerializeField]
    private GameSaveData _data;

    private void OnValidate()
    {
        _savePath = SaveSystem.SavePath;
    }

    private void Awake()
    {
        _savePath = SaveSystem.SavePath;
        RefreshData();
    }

    [Button("Refresh from Disk")]
    public void RefreshData()
    {
        _data = SaveSystem.Load();
        Debug.Log("Save data refreshed from disk.");
    }

    [Button("Save to Disk")]
    public void SaveToDisk()
    {
        SaveSystem.Save(_data);
        Debug.Log("Save data written to disk.");
    }

    [Button("Open Save Folder")]
    public void OpenSaveFolder()
    {
        string folder = Path.GetDirectoryName(SaveSystem.SavePath);
        if (Directory.Exists(folder))
        {
            Application.OpenURL("file://" + folder);
        }
        else
        {
            Debug.LogError($"Save folder does not exist: {folder}");
        }
    }

    [Button("Clear Save Data")]
    public void ClearSaveData()
    {
        if (File.Exists(SaveSystem.SavePath))
        {
            File.Delete(SaveSystem.SavePath);
            _data = new GameSaveData();
            Debug.Log("Save file deleted and local data reset.");
        }
    }
}

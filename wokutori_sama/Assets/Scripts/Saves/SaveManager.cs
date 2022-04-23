using System;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : SingletonMonoBehaviour<SaveManager>
{
    public SavePoint[] SavePoints;
    [ReadOnly] [SerializeField] private IntReactiveProperty currentSaveIndex = new IntReactiveProperty(-1);
    public IReadOnlyReactiveProperty<int> CurrentSaveIndex => currentSaveIndex;

    protected override void Awake()
    {
        base.Awake();

        var sceneName = SceneManager.GetActiveScene().name;
        if (PlayerPrefs.HasKey(sceneName))
        {
            currentSaveIndex.Value = PlayerPrefs.GetInt(sceneName);
        }
        else
        {
            currentSaveIndex.Value = -1;
        }
    }

    public bool CheckSavable(SavePoint savePoint)
    {
        if (!SavePoints.Contains(savePoint))
        {
            return false;
        }

        var index = Array.IndexOf(SavePoints, savePoint);
        if (index <= currentSaveIndex.Value)
        {
            return false;
        }

        return true;
    }

    public bool IsAlreadySaved(SavePoint savePoint)
    {
        if (!SavePoints.Contains(savePoint))
        {
            return false;
        }

        var index = Array.IndexOf(SavePoints, savePoint);
        if (index <= currentSaveIndex.Value)
        {
            return true;
        }

        return false;
    }

    public bool Save(SavePoint savePoint)
    {
        if (!SavePoints.Contains(savePoint))
        {
            return false;
        }

        var index = Array.IndexOf(SavePoints, savePoint);
        if (index <= currentSaveIndex.Value)
        {
            return false;
        }

        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, index);
        PlayerPrefs.Save();

        currentSaveIndex.Value = index;

        return true;
    }
}
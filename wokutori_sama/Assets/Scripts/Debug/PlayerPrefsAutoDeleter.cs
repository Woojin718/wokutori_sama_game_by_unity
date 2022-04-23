using UnityEngine;

public class PlayerPrefsAutoDeleter : MonoBehaviour
{
    private void Awake()
    {
        PlayerPrefs.DeleteAll();
    }
}

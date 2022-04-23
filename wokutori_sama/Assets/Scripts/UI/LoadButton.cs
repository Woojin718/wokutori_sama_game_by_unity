using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LoadButton : MonoBehaviour
{
    [SerializeField] private string nextScene;

    private void Start()
    {
        GetComponent<Button>()
            .OnClickAsObservable()
            .Take(1)
            .Subscribe(_ => SceneManager.LoadScene(nextScene))
            .AddTo(this);
    }
}
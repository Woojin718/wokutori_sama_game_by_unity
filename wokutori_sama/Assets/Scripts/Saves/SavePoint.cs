using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] private float saveDistance = 0.814f;
    [SerializeField] private Transform fireRoot;

    public Vector2 SavePosition => transform.position;

    private PlayerCore playerCore;

    private void Start()
    {
        // this.OnTriggerEnter2DAsObservable()
        //     .Where(col => col.CompareTag("PlayerCollider"))
        //     .Subscribe(_ => Save());

        playerCore = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCore>();

        Observable.EveryUpdate()
            .Where(_ => (playerCore.transform.position - transform.position).magnitude - saveDistance < 0.01f)
            .Take(1)
            .Subscribe(_ => Save())
            .AddTo(this);

        fireRoot.gameObject.SetActive(SaveManager.Instance.IsAlreadySaved(this));
    }

    private void Save()
    {
        if (SaveManager.Instance.CheckSavable(this))
        {
            playerCore.StartSaveFlow(StartFireAnimation).Forget();
        }
    }

    private void StartFireAnimation()
    {
        if (fireRoot == null)
        {
            return;
        }

        fireRoot.gameObject.SetActive(true);
        fireRoot.localScale = Vector3.zero;
        fireRoot.DOScale(Vector3.one, 1f);
        SaveManager.Instance.Save(this);
    }
}
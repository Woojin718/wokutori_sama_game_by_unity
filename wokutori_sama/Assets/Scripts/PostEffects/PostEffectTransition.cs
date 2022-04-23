using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PostEffectTransition : MonoBehaviour
{
    [SerializeField] private Material postEffectMaterial;
    [SerializeField] private float transitionTime = 2f;

    private readonly int _progressID = Shader.PropertyToID("_Progress");

    private void Start()
    {
        if (postEffectMaterial != null)
        {
            DOTween.To(
                () => 0.01f,
                value => postEffectMaterial.SetFloat(_progressID, value),
                0.99f,
                transitionTime
            );
        }
    }
}
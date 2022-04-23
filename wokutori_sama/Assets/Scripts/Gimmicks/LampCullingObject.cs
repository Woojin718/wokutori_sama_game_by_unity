using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Collider2D))]
public class LampCullingObject : MonoBehaviour
{
    [SerializeField] private string lampTag = "Lamp";
    [SerializeField] private bool insideVisible = false;

    private Light2D lamp;
    private float lampRadius;
    private Collider2D collider2D;

    private CharacterMarker _marker;
    private bool containsCharacter = false;

    private bool isInsideLamp = false;

    public bool IsCulling => isInsideLamp && lamp.intensity > 0.5f != insideVisible;

    private void Start()
    {
        lamp = GameObject.FindGameObjectWithTag(lampTag).GetComponent<Light2D>();
        lampRadius = lamp.pointLightOuterRadius;

        collider2D = GetComponent<Collider2D>();

        this.OnTriggerEnter2DAsObservable()
            .Select(col => col.gameObject.GetComponent<CharacterMarker>())
            .Where(marker => marker != null)
            .Subscribe(marker =>
            {
                _marker = marker;
                containsCharacter = true;
            });

        this.OnTriggerExit2DAsObservable()
            .Select(col => col.gameObject.GetComponent<CharacterMarker>())
            .Where(marker => marker != null)
            .Subscribe(marker =>
            {
                _marker = null;
                containsCharacter = false;
            });
    }

    private void Update()
    {
        isInsideLamp = (lamp.transform.position - transform.position).magnitude < lampRadius;
        if (containsCharacter)
        {
            // collider2D.isTrigger = true;
            collider2D.enabled = false;
        }
        else
        {
            // collider2D.isTrigger = isInsideLamp && lamp.intensity > 0.5f != insideVisible;
            collider2D.enabled = !(isInsideLamp && lamp.intensity > 0.5f != insideVisible);
        }
    }
}
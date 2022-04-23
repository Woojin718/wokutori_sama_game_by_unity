using System.Linq;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class PlayerCamera : MonoBehaviour
{
    public float NormalSize = 4.5f;
    public float ZoomSize = 2f;

    private void Awake()
    {
        var playerRenderer = GameObject.FindGameObjectWithTag("Player")
            .GetComponentsInChildren<SpriteRenderer>()
            .FirstOrDefault(renderer => renderer.gameObject.name == "PlayerRenderer");

        if (playerRenderer != null)
        {
            GetComponent<CinemachineVirtualCamera>().m_Follow = playerRenderer.transform;
        }
    }
}
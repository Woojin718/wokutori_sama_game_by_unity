using UniRx;
using UniRx.Triggers;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CarriableBox : MonoBehaviour
{
    private static int ground;
    private static int field;

    public bool IsFalling = false;

    private void Start()
    {
        ground = LayerMask.NameToLayer("Ground");
        field = LayerMask.NameToLayer("Field");

        var rb = GetComponent<Rigidbody2D>();

        this.OnCollisionStay2DAsObservable()
            .Where(_ => IsFalling)
            .Where(_ => Mathf.Abs(transform.eulerAngles.z % 90) < 0.01f)
            .Select(col => col.gameObject.layer)
            .Where(layer => layer == ground || layer == field)
            .SubscribeWithState(rb, (layer, rb) =>
            {
                IsFalling = false;
                rb.bodyType = RigidbodyType2D.Static;
            });
    }
}
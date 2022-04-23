using UnityEngine;

public interface IDieable
{
    Transform transform { get; }
    bool TryKill(DeadReason deadReason);
}
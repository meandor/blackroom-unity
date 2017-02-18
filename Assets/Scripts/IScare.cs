using UnityEngine;

public abstract class IScare : MonoBehaviour
{
    abstract public void Reset();
    abstract public void ToggleVisibility(bool active);
}

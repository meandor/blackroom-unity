using UnityEngine;
using System.Collections;

public class ScoreBoard : IScare
{
    private Vector3 startingPosition;
    private Quaternion startingRotation;
    public bool isVisible;

    void Start()
    {
        startingPosition = transform.localPosition;
        startingRotation = transform.localRotation;
        this.ToggleVisibility(isVisible);
    }

    public override void Reset()
    {
        transform.localPosition = startingPosition;
        transform.localRotation = startingRotation;
        this.ToggleVisibility(true);
    }

    public override void ToggleVisibility(bool active)
    {
        this.gameObject.SetActive(active);
    }
}

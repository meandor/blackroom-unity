using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreBoard : IScare
{
    public bool isVisible;
    public float centerXOffset;
    public float centerYOffset;
    public float centerZOffset;
    public float deltaYOffset;
    public Text scoreTextField;

    void Start()
    {
        this.scoreTextField.text = "1000";
        this.ToggleVisibility(isVisible);
    }

    public override void Reset()
    {
        this.ToggleVisibility(true);
        //this.scoreTextField.text = "0";
    }

    public override void ToggleVisibility(bool active)
    {
        this.gameObject.SetActive(active);
        if (active)
        {
            this.Reposition();
        }
    }

    private Vector3 CenterScreenPosition()
    {
        return Camera.main.ViewportToWorldPoint(new Vector3(this.centerXOffset, this.centerYOffset, this.centerZOffset));
    }

    private void Reposition()
    {
        transform.localPosition = this.CenterScreenPosition();
        float deltaY = Camera.main.transform.localEulerAngles.y + deltaYOffset;
        transform.localEulerAngles = new Vector3(0.0f, deltaY, 0.0f);
    }
}

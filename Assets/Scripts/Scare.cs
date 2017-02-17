using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(GvrAudioSource))]
[RequireComponent(typeof(Renderer))]
public class Scare : MonoBehaviour
{
    private Vector3 startingPosition;
    private GvrAudioSource audio;
    private AudioClip jumpScareSound;
    public AudioClip scareSound;
    public bool isVisible;

    void Awake()
    {
        jumpScareSound = Resources.Load<AudioClip>("Audio/162472__kastenfrosch__scary");
        audio = GetComponent<GvrAudioSource>();
    }

    void Start()
    {
        startingPosition = transform.localPosition;
        this.ToggleVisibility(isVisible);
    }

    void LateUpdate()
    {
        GvrViewer.Instance.UpdateState();
        if (GvrViewer.Instance.BackButtonPressed)
        {
            Application.Quit();
        }
    }

    public void ScareSequence()
    {
        StartCoroutine(StartTeleport());
    }

    public void Reset()
    {
        transform.localPosition = startingPosition;
    }

    private void ToggleVisibility(bool active)
    {
        this.gameObject.SetActive(active);
        if (active)
        {
            audio.PlayOneShot(scareSound, 0.8f);
        }
    }

    private IEnumerator StartTeleport()
    {
        // hide
        Debug.Log("hide");

        this.Hide();
        yield return new WaitForSecondsRealtime(3);
        Debug.Log("in your face");
        // show in center camera
        this.InYourFace();
        yield return new WaitForSecondsRealtime(1.5f);
        this.Hide();
        yield return new WaitForSecondsRealtime(4);
        // TODO Show next scare
    }

    private Vector3 CenterScreenPosition()
    {
        return Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0, 1f));
    }

    private void InYourFace()
    {
        Vector3 pos = this.CenterScreenPosition();
        Debug.Log("in your face pos: " + pos.x + ", " + pos.y + ", " + pos.z);
        transform.localPosition = this.CenterScreenPosition();
        audio.PlayOneShot(jumpScareSound, 0.4f);
    }

    private void Hide()
    {
        audio.Pause();
        transform.localPosition = new Vector3(30, 30, 30);
    }
}

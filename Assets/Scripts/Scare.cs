using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GvrAudioSource))]
public class Scare : MonoBehaviour
{
    private Vector3 startingPosition;
    private GvrAudioSource audio;
    private AudioClip jumpScareSound;
    public AudioClip scareSound;
    public bool isVisible;
    public Camera camera;

    void Awake()
    {
        jumpScareSound = Resources.Load<AudioClip>("Audio/jump_scare");
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
            audio.PlayOneShot(scareSound);
        }
    }

    private IEnumerator StartTeleport()
    {
        Debug.Log("hide");
        this.Hide();
        yield return new WaitForSecondsRealtime(3);
        Debug.Log("in your face");
        this.InYourFace();
        yield return new WaitForSecondsRealtime(1.5f);
        this.Hide();
        yield return new WaitForSecondsRealtime(4);
        // TODO Show next scare
    }

    private void InYourFace()
    {
        audio.Stop();
        Vector3 pos = this.CenterScreenPosition();
        Debug.Log("in your face pos: " + pos.x + ", " + pos.y + ", " + pos.z);
        transform.localPosition = this.CenterScreenPosition();
        float deltaY = camera.transform.localEulerAngles.y - 90.0f;
        transform.localEulerAngles = new Vector3(0.0f, deltaY, 0.0f);
        audio.PlayOneShot(jumpScareSound);
    }

    private Vector3 CenterScreenPosition()
    {
        return Camera.main.ViewportToWorldPoint(new Vector3(0.5f, -0.5f, 0.8f));
    }

    private void Hide()
    {
        audio.Pause();
        transform.localPosition = new Vector3(30, 30, 30);
    }
}

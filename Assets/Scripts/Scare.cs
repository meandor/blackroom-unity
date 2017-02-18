using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GvrAudioSource))]
public class Scare : IScare
{
    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private GvrAudioSource audio;
    private AudioClip jumpScareSound;
    public AudioClip scareSound;
    public bool isVisible;
    public Camera camera;
    public float deltaYOffset;
    public float centerXOffset;
    public float centerYOffset;
    public float centerZOffset;
    public IScare nextScare;

    void Awake()
    {
        jumpScareSound = Resources.Load<AudioClip>("Audio/jump_scare_2");
        audio = GetComponent<GvrAudioSource>();
    }

    void Start()
    {
        startingPosition = transform.localPosition;
        startingRotation = transform.localRotation;
        audio.loop = true;
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

    public override void Reset()
    {
        transform.localPosition = startingPosition;
        transform.localRotation = startingRotation;
        this.ToggleVisibility(true);
    }

    public override void ToggleVisibility(bool active)
    {
        this.gameObject.SetActive(active);
        if (active)
        {
            audio.clip = scareSound;
            audio.Play();
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
        this.SwitchToNextScare();
    }

    private void InYourFace()
    {
        audio.Stop();
        Vector3 pos = this.CenterScreenPosition();
        Debug.Log("in your face pos: " + pos.x + ", " + pos.y + ", " + pos.z);
        transform.localPosition = this.CenterScreenPosition();
        float deltaY = camera.transform.localEulerAngles.y + deltaYOffset;
        transform.localEulerAngles = new Vector3(0.0f, deltaY, 0.0f);
        audio.PlayOneShot(jumpScareSound);
    }

    private Vector3 CenterScreenPosition()
    {
        return Camera.main.ViewportToWorldPoint(new Vector3(this.centerXOffset, this.centerYOffset, this.centerZOffset));
    }

    private void Hide()
    {
        audio.Pause();
        transform.localPosition = new Vector3(30, 30, 30);
    }

    private void SwitchToNextScare()
    {
        if (this.nextScare != null)
        {
            this.nextScare.Reset(true);
        }
        this.ToggleVisibility(false);
    }
}

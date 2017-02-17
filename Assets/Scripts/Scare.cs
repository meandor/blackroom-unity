using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(GvrAudioSource))]
public class Scare : MonoBehaviour
{
    private Vector3 startingPosition;
    private GvrAudioSource audio;
    private AudioClip jumpScareSound;
    private AudioClip scare1;
    private AudioClip scare2;
    private AudioClip scare3;

    void Awake()
    {
        jumpScareSound = Resources.Load<AudioClip>("Audio/162472__kastenfrosch__scary");
        scare1 = Resources.Load<AudioClip>("Audio/178072__bsperan__scary-demon-haunting-sound-adam-webb-remix-3");
        audio = GetComponent<GvrAudioSource>();
        Debug.Log(jumpScareSound);
    }

    void Start()
    {
        startingPosition = transform.localPosition;
        audio.PlayOneShot(scare1, 0.8f);
    }

    void LateUpdate()
    {
        GvrViewer.Instance.UpdateState();
        if (GvrViewer.Instance.BackButtonPressed)
        {
            Application.Quit();
        }
    }

    IEnumerator StartTeleport()
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
        Debug.Log("transport");
        // transport randomely
        this.Reposition();
        
    }

    public void Reset()
    {
        transform.localPosition = startingPosition;
    }

    private Vector3 CenterScreenPosition()
    {
        //Camera camera = GetComponent<Camera>();
        return Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0, 1f));
    }

    public void TeleportRandomly()
    {
        StartCoroutine(StartTeleport());
    }

    private void InYourFace()
    {
        Vector3 pos = this.CenterScreenPosition();
        audio.Stop();
        Debug.Log("in your face pos: " + pos.x + ", " + pos.y + ", " + pos.z);
        transform.localPosition = this.CenterScreenPosition();
        audio.PlayOneShot(jumpScareSound, 0.4f);
    }

    private void Reposition()
    {
        Vector3 direction = Random.onUnitSphere;
        direction.y = Mathf.Clamp(direction.y, 0.5f, 1f);
        float distance = 2 * Random.value + 3.5f;
        Vector3 tempPosition = direction * distance;
        Debug.Log("Transport from" + transform.localPosition.x + ", " + transform.localPosition.y + ", " + transform.localPosition.z);
        Debug.Log("Transport to" + tempPosition.x + ", " + tempPosition.y + ", " + tempPosition.z);
        audio.Stop();
        transform.localPosition = tempPosition;
        audio.PlayOneShot(scare1, 0.8f);
    }

    private void Hide()
    {
        transform.localPosition = new Vector3(30, 30, 30);
    }
}

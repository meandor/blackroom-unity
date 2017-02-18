using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
[RequireComponent(typeof(AudioSource))]
public class GvrMicLevel : MonoBehaviour
{
    private const string DISPLAY_TEXT_FORMAT = "{0} Mic";
    private Text textField;
    private Camera playerCamera;
    private float loudness = 0;
    public float sensitivity = 100;
    public AudioSource audio;

    void Awake()
    {
        textField = GetComponent<Text>();
        audio = GetComponent<AudioSource>();
    }

    void Start()
    {
        this.playerCamera = Camera.main;
        /*transform.localPosition = this.playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1));
        //float deltaY = this.playerCamera.transform.localEulerAngles.y - 90;
        Vector3 cameraRotation = this.playerCamera.transform.localEulerAngles;
        Vector3 rotateView = new Vector3(cameraRotation.x, (cameraRotation.y + 10), (cameraRotation.z + 10));
        transform.localEulerAngles = rotateView;*/
        transform.SetParent(playerCamera.GetComponent<Transform>(), true);

        //add the rest of the code like this
        audio.clip = Microphone.Start("Built-in Microphone", true, 10, 44100);
        audio.loop = true;
        while (!(Microphone.GetPosition(null) > 0)) { }
        audio.Play();

        //Do not Mute the audio Source.
    }

    float GetAveragedVolume()
    {
        float[] data = new float[256];
        float a = 0;
        audio.GetOutputData(data, 0);
        foreach (float s in data)
        {
            a += Mathf.Abs(s);
        }
        return a / 256;
    }

    void LateUpdate()
    {
        loudness = GetAveragedVolume() * sensitivity;
        textField.text = string.Format(DISPLAY_TEXT_FORMAT, Mathf.RoundToInt(loudness));
    }

    public float GetLoudness()
    {
        return this.loudness;
    }
}

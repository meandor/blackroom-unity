using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class MicrophoneLevel : MonoBehaviour
{
    private Camera playerCamera;
    private float loudness = 0;
    private int score;
    private AudioSource audio;
    public float sensitivity = 100;
    public ScoreBoard scoreBoard;
    public GameObject scoreBoardGameObject;
    public int scoreThreshold;

    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    void Start()
    {
        this.playerCamera = Camera.main;
        transform.SetParent(playerCamera.GetComponent<Transform>(), true);
        audio.clip = Microphone.Start("Built-in Microphone", true, 10, 44100);
        audio.loop = true;
        while (!(Microphone.GetPosition(null) > 0)) { }
        audio.Play();
        this.score = 0;
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

    void Update()
    {
        this.loudness = GetAveragedVolume() * sensitivity;

        if (this.scoreBoardGameObject.activeSelf)
        {
            this.scoreBoard.scoreTextField.text = this.score.ToString();
        }
        else if (!this.scoreBoardGameObject.activeSelf && this.loudness > this.scoreThreshold)
        {
            this.score += 10;
        }
    }

    public void Reset()
    {
        this.score = 0;
    }
}

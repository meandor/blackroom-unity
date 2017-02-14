using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Teleport : MonoBehaviour
{
    private Vector3 startingPosition;

    public Material inactiveMaterial;
    public Material gazedAtMaterial;

    void Start()
    {
        startingPosition = transform.localPosition;
    }

    void LateUpdate()
    {
        GvrViewer.Instance.UpdateState();
        if (GvrViewer.Instance.BackButtonPressed)
        {
            Application.Quit();
        }
    }

    public void SetGazedAt(bool gazedAt)
    {
        if (inactiveMaterial != null && gazedAtMaterial != null)
        {
            GetComponent<Renderer>().material = gazedAt ? gazedAtMaterial : inactiveMaterial;
            return;
        }
        GetComponent<Renderer>().material.color = gazedAt ? Color.green : Color.red;
    }

    IEnumerator StartTeleport()
    {
        // hide
        Debug.Log("hide");
        transform.localPosition = new Vector3(30, 30, 30);
        yield return new WaitForSecondsRealtime(3);
        Debug.Log("in your face");
        // show in center camera
        Vector3 pos = this.CenterScreenPosition();
        Debug.Log("in your face pos: " + pos.x + ", " + pos.y + ", " + pos.z);
        transform.localPosition = this.CenterScreenPosition();
        yield return new WaitForSecondsRealtime(1.5f);
        Debug.Log("transport");
        // transport randomely
        Vector3 direction = Random.onUnitSphere;
        direction.y = Mathf.Clamp(direction.y, 0.5f, 1f);
        float distance = 2 * Random.value + 3.5f;
        Vector3 tempPosition = direction * distance;
        Debug.Log("Transport from" + transform.localPosition.x + ", " + transform.localPosition.y + ", " + transform.localPosition.z);
        Debug.Log("Transport to" + tempPosition.x + ", " + tempPosition.y + ", " + tempPosition.z);
        transform.localPosition = tempPosition;
    }

    public void Reset()
    {
        transform.localPosition = startingPosition;
    }

    private Vector3 CenterScreenPosition()
    {
        //Camera camera = GetComponent<Camera>();
        return Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1f));
    }

    public void TeleportRandomly()
    {
        StartCoroutine(StartTeleport());
    }
}

using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public float smoothing = 5f;

    public Vector3 offset = new Vector3(1f, 15f, -22f);

    void FixedUpdate()
    {
        Vector3 targetCameraPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, 
            targetCameraPos, smoothing * Time.deltaTime);
    }

}
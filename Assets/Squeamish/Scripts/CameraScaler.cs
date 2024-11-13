using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    public float baseHeight = 15f;  // The height of the camera for a standard aspect ratio

    void Update()
    {
        // Get the current aspect ratio
        float aspectRatio = (float)Screen.width / Screen.height;

        // Calculate the new orthographic size based on the aspect ratio
        Camera.main.orthographicSize = baseHeight / aspectRatio;
    }
}
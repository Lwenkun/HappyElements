using UnityEngine;
using System.Collections;

public class CameraSize : MonoBehaviour {

    void Awake() {
        float aspectRatio = 1080.0f / 1920.0f;
        float cameraHeight = Camera.main.orthographicSize * 2f * 1.0f;
        float width = aspectRatio * cameraHeight;
        float realRatio = (Screen.width) * 1.0f / Screen.height;
        float shouldHeight = width / realRatio;
        Camera.main.orthographicSize = shouldHeight / 2.0f;
    }

}

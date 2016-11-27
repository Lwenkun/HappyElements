using UnityEngine;
using System.Collections;

public class BaseUIBehaviour : MonoBehaviour {

    public void Show() {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }

    public void Hide() {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
}

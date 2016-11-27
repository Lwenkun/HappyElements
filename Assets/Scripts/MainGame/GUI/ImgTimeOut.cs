using UnityEngine;
using System.Collections;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
public class ImgTimeOut : BaseUIBehaviour {

    private Image imgTimeOut;

    public delegate void AnimOver();

	// Use this for initialization
	void Awake () {
        imgTimeOut = GetComponent<Image>();
	}

    public IEnumerator ShowTimeOut(AnimOver ao) {
        Show();
        float passTime = 0f;
        while((passTime += Time.deltaTime) < 0.2f) {
            imgTimeOut.transform.localScale = new Vector3(0.2f + passTime * 4 , 0.2f + passTime * 4, 0.2f + passTime * 4);
            yield return null;
        }
        passTime = 0f;
        while ((passTime += Time.deltaTime) < 0.15f)
        {
            imgTimeOut.transform.localScale = new Vector3(1f - passTime, 1f - passTime, 1f - passTime);
            yield return null;
        }

        passTime = 0f;
        while ((passTime += Time.deltaTime) < 0.15f)
        {
            imgTimeOut.transform.localScale = new Vector3(0.85f + passTime, 0.85f + passTime, 0.85f + passTime);
            yield return null;
        }
        yield return new WaitForSeconds(1.0f);
        ao();
    }

}
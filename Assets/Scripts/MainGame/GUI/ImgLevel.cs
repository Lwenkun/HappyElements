using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditorInternal.VR;
#endif
using UnityEngine.UI;

public class ImgLevel : BaseUIBehaviour {

    public Sprite[] levels;
    private Image imgLevel;

	// Use this for initialization
	void Awake () {
        imgLevel = GetComponent<Image>();
        transform.localScale = new Vector3(0f, 0f);
	}

    public void ShowLevel(int level) {
        if (level < 2 || level > 9)
            return;
        StartCoroutine(LevelAnimation(imgLevel, level));
    }

    private IEnumerator LevelAnimation(Image imgLevel, int currLevel) {
        imgLevel.sprite = levels[currLevel];
        float timeLeft = 0.5f;
        float passTime = 0f;
        while ((passTime += Time.deltaTime) < timeLeft)
        {
            imgLevel.transform.localScale = new Vector3(1 + 2 * passTime, 1 +  2 * passTime, 1 + 2 * passTime);
            yield return null;
        }
        transform.localScale = new Vector3(0f, 0f, 0f);
    }

}
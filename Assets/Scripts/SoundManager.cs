using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public AudioClip game;
    public AudioClip other;

    //游戏音效
    public AudioSource efxSource;
    //背景音效
    public AudioSource musicSource;
    public static SoundManager instance = null;
    //音调范围
    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

	// Use this for initialization
	void Awake () {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        PlayBGM(other);

        DontDestroyOnLoad(gameObject);
	}
    //播放单个音频资源文件
    public void playSingle(AudioClip clip) {
        if (!Setting.SFXState)
        {
            return;
        }
        efxSource.clip = clip;
        efxSource.Play();
    }

    //随机播放某个音音频文件
    public void RandomizeSfx(params AudioClip[] clips) {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randomIndex];
        efxSource.Play();
    }

    public void PlayBGM(AudioClip clip) {
        if (! Setting.BGMState)
        {
            return;
        }
        StopBGM();
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopBGM() {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }

	
}

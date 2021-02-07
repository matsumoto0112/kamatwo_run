using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource bgmSource = null;
    [SerializeField]
    private AudioSource seSource = null;

    private const float MAX_BGM_VOLUME = 0.4f;
    private const float MAX_SE_VOLUME = 0.7f;

    //BGM,SEのリスト
    private Dictionary<string, AudioClip> bgm;
    private Dictionary<string, AudioClip> se;

    private bool fadeFlag = false;
    private string nextBGMName = "";

    public float SEVolume
    {
        get
        {
            return seSource.volume;
        }
        set
        {
            seSource.volume = value;
        }
    }

    public float BGMVolume
    {
        get
        {
            return bgmSource.volume;
        }
        set
        {
            bgmSource.volume = value;
        }
    }

    private void Awake()
    {
        bgm = new Dictionary<string, AudioClip>();
        se = new Dictionary<string, AudioClip>();
        Object[] _bgm = Resources.LoadAll("Audio/BGM");
        Object[] _se = Resources.LoadAll("Audio/SE");

        foreach (AudioClip c in _bgm)
            bgm[c.name] = c;
        foreach (AudioClip c in _se)
            se[c.name] = c;

        Debug.Log("読み込んだBGMの数:" + bgm.Count);
        Debug.Log("読み込んだSEの数:" + se.Count);
    }

    private void Start()
    {
        bgmSource.volume =MAX_BGM_VOLUME;
        seSource.volume = MAX_SE_VOLUME;
        fadeFlag = false;
        nextBGMName = "";
    }

    private void Update()
    {
        //if (!fadeFlag)
        //    return;
        //bgmSource.volume -= Time.deltaTime * 2.0f;
        //if (bgmSource.volume <= 0)
        //{
        //    FadeEndBGM();
        //}
    }

    public void PlayBGM(string bgmName)
    {
        //指定したBGMが存在していないのなら
        if (!bgm.ContainsKey(bgmName))
            return;

        //BGMが鳴っていないのなら
        if (!bgmSource.isPlaying)
        {
            BGMStart(bgmName);
        }
        //フェード中だった時
        else if (fadeFlag)
        {
            FadeEndBGM();
            BGMStart(bgmName);
        }
    }

    private void FadeEndBGM()
    {
        bgmSource.Stop();
        fadeFlag = false;
        bgmSource.volume = MAX_BGM_VOLUME;
    }

    private void BGMStart(string bgmName)
    {
        bgmSource.clip = bgm[bgmName];
        bgmSource.Play();
        nextBGMName = "";
    }

    public void FadeOutBGM()
    {
        fadeFlag = true;
    }

    /// <summary>
    /// SEを鳴らす（のち改良）
    /// </summary>
    /// <param name="seName"></param>
    public void PlaySE(string seName)
    {
        seSource.PlayOneShot(se[seName]);
    }

    /// <summary>
    /// SEの再生時間を取得
    /// </summary>
    /// <param name="seName"></param>
    /// <returns></returns>
    public float GetSELength(string seName)
    {
        return se[seName].length;
    }
}

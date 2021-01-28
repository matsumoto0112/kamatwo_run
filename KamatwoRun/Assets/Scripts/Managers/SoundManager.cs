using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const string BGM_PATH = "Sounds/BGM";//BGM�p�X
    private const string SE_PATH = "Sounds/SE";//se�p�X
    private const string SOUND_OBJECT_NAME = "SoundManager";
    private const int BGM_SOURCE_NUM = 1;
    private const int SE_SOURCE_NUM = 5;
    private const float FADE_OUT_SECONDE = 0.5f;//�V�[���؂�ւ���������0,5�b��
    private const float BGM_VLOUE = 0.3f;//BGM�̃{�������[��
    private const float SE_VOLUME = 0.1f;//SE�̃{�����[��

    private bool isFadeOut = false;
    private float fadeDeltaTime = 0f;
    private int nextSESourceNum = 0;
    private BGMLabel currentBGM = BGMLabel.None;
    private BGMLabel nextBGM = BGMLabel.None;

    //BGM�͈�Â邪�ASE�͕��������ɂȂ邱�Ƃ�����
    private AudioSource bgmSource;
    private List<AudioSource> seSourceList;
    private Dictionary<string, AudioClip> seClipDic;
    private Dictionary<string, AudioClip> bgmClipDic;
    private static SoundManager singletonInstance = null;

    public static SoundManager SingletonInstance
    {
        get
        {
            if(!singletonInstance)
            {
                GameObject obj = new GameObject(SOUND_OBJECT_NAME);
                singletonInstance = obj.AddComponent<SoundManager>();
                DontDestroyOnLoad(obj);
            }
            return singletonInstance;
        }
    }
    private void Awake()
    {
        for(int i=0;i<SE_SOURCE_NUM+BGM_SOURCE_NUM;i++)
        {
            gameObject.AddComponent<AudioSource>();
           
        IEnumerable<AudioSource> audioSources = GetComponents<AudioSource>().Select(a => { a.playOnAwake = false; a.volume = BGM_VLOUE; a.loop = true; return a; });
            bgmSource = audioSources.First();
            seSourceList = audioSources.Skip(BGM_SOURCE_NUM).ToList();
            seSourceList.ForEach(a => { a.volume = SE_VOLUME; a.loop = false; });

            bgmClipDic = (Resources.LoadAll(BGM_PATH) as Object[]).ToDictionary(bgm => bgm.name, bgm => (AudioClip)bgm);
            seClipDic = (Resources.LoadAll(SE_PATH) as Object[]).ToDictionary(se => se.name, se => (AudioClip)se);
            Debug.Log(BGM_PATH);
          
        }

    }
    /// <summary>
    /// �w�肵���t�@�C������SE�𗬂�B��������delay�Ɏw�肵�����Ԃ����Đ��܂ł̊Ԋu���󂯂�
    /// </summary>
    /// 
    public void PlaySE(SELabel seLabel, float delay = 0.0f) => StartCoroutine(DelayPlaySE(seLabel, delay));

 
    private IEnumerator DelayPlaySE(SELabel seLabel, float delay)
    {
        yield return new WaitForSeconds(delay);
        AudioSource se = seSourceList[nextSESourceNum];
        se.PlayOneShot(seClipDic[seLabel.ToString()]);
        nextSESourceNum = (++nextSESourceNum < SE_SOURCE_NUM) ? nextSESourceNum : 0;
    }
    /// <summary>
    /// �w�肵��BGM�𗬂��B���łɗ���Ă���ꍇ��next�ɗ\�񂵁A����Ă���BGM���t�F�[�h�A�E�g������
    /// 
    /// </summary>
    public void PlayBGM(BGMLabel bgmLabel)
    {
        if (!bgmSource.isPlaying)
        {
           
            currentBGM = bgmLabel;
            nextBGM = BGMLabel.None;
            if (bgmClipDic.ContainsKey(bgmLabel.ToString()))
            {
                bgmSource.clip = bgmClipDic[bgmLabel.ToString()];
            }
            else
            {
                Debug.LogError($"bgmClipDic��{bgmLabel.ToString()}�Ƃ���Key�͂���܂���");
            }
            bgmSource.Play();
        }
        else if (currentBGM != bgmLabel)
        {
            isFadeOut = true;
            nextBGM = bgmLabel;
            fadeDeltaTime = 0f;
        }
    }
    /// <summary>
    /// BGN���~�߂�
    /// 
    /// </summary>
    /// 
�@�@public void StopSound()
    {
        bgmSource.Stop();
        seSourceList.ForEach(a => { a.Stop(); });
    }
    public enum BGMLabel
    {
        None,
        Home,
        Game,
        Library
    }

    public enum SELabel
    {
        Start,
        PlayGame,
        TapButton,
        Spell,
        Win,
        Lose,
        ItemGet
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
  private  void Update()
    {
        if (isFadeOut)
        {
            fadeDeltaTime += Time.deltaTime;
            bgmSource.volume = (1.0f - fadeDeltaTime / FADE_OUT_SECONDE) * BGM_VLOUE;

            if (fadeDeltaTime >= FADE_OUT_SECONDE)
            {
                isFadeOut = false;
                bgmSource.Stop();
            }
        }
        else if (nextBGM != BGMLabel.None)
        {
            bgmSource.volume = BGM_VLOUE;
            PlayBGM(nextBGM);
        }
    }
}


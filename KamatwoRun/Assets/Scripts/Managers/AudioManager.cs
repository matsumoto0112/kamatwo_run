using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExLib;

public class AudioManager : MonoBehaviour
{
  
    protected static AudioManager instance;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (AudioManager)FindObjectOfType(typeof(AudioManager));

                if (instance == null)
                {
                    Debug.LogError("SoundManager Instance Error");
                }
            }

            return instance;
        }
    }

    // ����
    public SoundVolume volume = new SoundVolume();

    // === AudioSource ===
    // BGM
    private AudioSource BGMsource;
    // SE
    private AudioSource[] SEsources = new AudioSource[16];
    // ����
    private AudioSource[] VoiceSources = new AudioSource[16];

    // === AudioClip ===
    // BGM
    public AudioClip[] BGM;
    // SE
    public AudioClip[] SE;
    // ����
    public AudioClip[] Voice;

    void Awake()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("SoundManager");
        if (obj.Length > 1)
        {
            // ���ɑ��݂��Ă���Ȃ�폜
            Destroy(gameObject);
        }
        else
        {
            // ���Ǘ��̓V�[���J�ڂł͔j�������Ȃ�
            DontDestroyOnLoad(gameObject);
        }

        // �S�Ă�AudioSource�R���|�[�l���g��ǉ�����

        // BGM AudioSource
        BGMsource = gameObject.AddComponent<AudioSource>();
        // BGM�̓��[�v��L���ɂ���
        BGMsource.loop = true;

        // SE AudioSource
        for (int i = 0; i < SEsources.Length; i++)
        {
            SEsources[i] = gameObject.AddComponent<AudioSource>();
        }

        // ���� AudioSource
        for (int i = 0; i < VoiceSources.Length; i++)
        {
            VoiceSources[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // �~���[�g�ݒ�
        BGMsource.mute = volume.Mute;
        foreach (AudioSource source in SEsources)
        {
            source.mute = volume.Mute;
        }
        foreach (AudioSource source in VoiceSources)
        {
            source.mute = volume.Mute;
        }

        // �{�����[���ݒ�
        BGMsource.volume = volume.BGM;
        foreach (AudioSource source in SEsources)
        {
            source.volume = volume.SE;
        }
        foreach (AudioSource source in VoiceSources)
        {
            source.volume = volume.Voice;
        }
    }



    // ***** BGM�Đ� *****
    // BGM�Đ�
    public void PlayBGM(int index)
    {
        if (0 > index || BGM.Length <= index)
        {
            return;
        }
        // ����BGM�̏ꍇ�͉������Ȃ�
        if (BGMsource.clip == BGM[index])
        {
            return;
        }
        BGMsource.Stop();
        BGMsource.clip = BGM[index];
        BGMsource.Play();
    }

    // BGM��~
    public void StopBGM()
    {
        BGMsource.Stop();
        BGMsource.clip = null;
    }


    // ***** SE�Đ� *****
    // SE�Đ�
    public void PlaySE(int index)
    {
        if (0 > index || SE.Length <= index)
        {
            return;
        }

        // �Đ����Ŗ���AudioSouce�Ŗ炷
        foreach (AudioSource source in SEsources)
        {
            if (false == source.isPlaying)
            {
                source.clip = SE[index];
                source.Play();
                return;
            }
        }
    }

    // SE��~
    public void StopSE()
    {
        // �S�Ă�SE�p��AudioSouce���~����
        foreach (AudioSource source in SEsources)
        {
            source.Stop();
            source.clip = null;
        }
    }


    // ***** �����Đ� *****
    // �����Đ�
    public void PlayVoice(int index)
    {
        if (0 > index || Voice.Length <= index)
        {
            return;
        }
        // �Đ����Ŗ���AudioSouce�Ŗ炷
        foreach (AudioSource source in VoiceSources)
        {
            if (false == source.isPlaying)
            {
                source.clip = Voice[index];
                source.Play();
                return;
            }
        }
    }

    // ������~
    public void StopVoice()
    {
        // �S�Ẳ����p��AudioSouce���~����
        foreach (AudioSource source in VoiceSources)
        {
            source.Stop();
            source.clip = null;
        }
    }

}

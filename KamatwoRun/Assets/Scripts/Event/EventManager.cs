using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExLib;

public class EventManager:SingletonMonoBehaviour<EventManager>
{
    [SerializeField]
    private GameSpeed gameSpeed = null;
    private IEnumerator coroutine = null;
    private GameObject playerModelObject = null;
    private Timer eventTimer;

    //�C�x���g�t���O
    public bool EventFlag
    {
        get
        {
            return coroutine != null;
        }
    }

    protected override void Awake()
    {
        coroutine = null;
        base.Awake();
        //�v���C���[���f���擾
        playerModelObject = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).gameObject;
        eventTimer = new Timer(2.0f);
    }

    /// <summary>
    /// �Q�[���X�^�[�g���̃C�x���g����
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameStartEvent()
    {
        yield return null;
    }

    /// <summary>
    /// �J�[�u�C�x���g
    /// </summary>
    /// <param name="subStageObject"></param>
    public void CurveEvent(GameObject subStageObject)
    {
        coroutine = CurveEventCoroutine(subStageObject);
        StartCoroutine(coroutine);
    }

    private IEnumerator CurveEventCoroutine(GameObject subStageObject)
    {
        //�J�����̐e�I�u�W�F�N�g��ۑ�
        Transform cameraParent = Camera.main.transform.parent;
        Vector3 localPosition = Camera.main.transform.localPosition;
        Vector3 localEulerAngle = Camera.main.transform.localEulerAngles;

        //�J�������T�u�X�e�[�W�̎q�I�u�W�F�N�g�ɂ���
        Camera.main.transform.parent = subStageObject.transform;
        Camera.main.transform.position = subStageObject.GetComponent<CurveCameraEvent>().EventCameraPosition;

        while (true)
        {
            //�J�������v���C���[�Ɍ�����
            Camera.main.transform.LookAt(playerModelObject.transform);
            eventTimer.UpdateTimer();
            if(eventTimer.IsTime() == true)
            {
                break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }

        Camera.main.transform.parent = cameraParent;
        Camera.main.transform.localPosition = localPosition;
        Camera.main.transform.localEulerAngles = localEulerAngle;

        coroutine = null;
        eventTimer.Initialize();
    }
}

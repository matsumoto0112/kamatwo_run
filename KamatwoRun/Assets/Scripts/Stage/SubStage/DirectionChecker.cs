using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �T�u�X�e�[�W�̐i�s�����𒲂ׂ�R���|�[�l���g
/// </summary>
public abstract class DirectionChecker : MonoBehaviour
{
    protected GatewayType entrance;
    protected GatewayType exit;

    /// <summary>
    /// �T�u�X�e�[�W�̓����A�o����ݒ肷��
    /// </summary>
    /// <param name="entrance"></param>
    /// <param name="exit"></param>
    public void Init(GatewayType entrance, GatewayType exit)
    {
        this.entrance = entrance;
        this.exit = exit;
    }

    /// <summary>
    /// �w����W����i�s�������擾����
    /// </summary>
    /// <param name="checkPosition"></param>
    /// <returns></returns>
    public abstract Vector3 Directon(Vector3 checkPosition);
}

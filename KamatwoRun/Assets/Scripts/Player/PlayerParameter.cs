using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�̃p�����[�^�Ǘ��N���X
/// </summary>
public class PlayerParameter : CharacterComponent
{
    [SerializeField]
    private PlayerParameterDataTable dataTable = null;

    public PlayerParameterData parameter { get; private set; }

    public override void OnCreate()
    {
        //�X�e�[�^�X�擾
        parameter = new PlayerParameterData(dataTable.GetParameter());
    }
}

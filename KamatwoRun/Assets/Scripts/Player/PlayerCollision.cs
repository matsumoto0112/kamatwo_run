using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�Փ˔���N���X
/// </summary>
public class PlayerCollision : CharacterComponent
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentToNullCheck(out ScoreObject scoreObject) == true)
        {
            Debug.Log($"Hit to ScoreObject!!");
        }
        else if (other.gameObject.GetComponentToNullCheck(out Obstacle obstacle) == true)
        {
            Debug.Log($"Hit to Obstacle...");
        }
        else if (other.gameObject.GetComponentToNullCheck(out WrappableObject wrappableObject) == true)
        {
            Debug.Log("$Hit to WrappableObject");
        }
    }
}

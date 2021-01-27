using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_ShapedLane : Lane
{
    [SerializeField, Tooltip("スポーン可能な地点の開始地点")]
    private GameObject lineStartPoint;
    [SerializeField, Tooltip("スポーン可能な地点の中継地点")]
    private GameObject lineMiddlePoint;
    [SerializeField, Tooltip("スポーン可能な地点の終了地点")]
    private GameObject lineEndPoint;

    [SerializeField, Tooltip("生成地点を一定間隔にするためのステップ値")]
    private int step = 5;

    /// <summary>
    /// ランダムなスポーン地点を返す
    /// </summary>
    /// <returns></returns>
    public override Vector3 GetRandomSpawnPoint()
    {
        if (!(lineStartPoint && lineEndPoint)) { return Vector3.zero; }
        float t = (Random.Range(0, step + 1) * 1.0f / step) * 2.0f;
        if (t <= 1.0f)
        {
            return Vector3.Lerp(lineStartPoint.transform.position, lineMiddlePoint.transform.position, t);
        }
        else
        {
            return Vector3.Lerp(lineMiddlePoint.transform.position, lineEndPoint.transform.position, t - 1.0f);
        }
    }

    private void OnDrawGizmos()
    {
        if (!(lineStartPoint && lineEndPoint && lineMiddlePoint)) { return; }

        //スポーン可能な地点を示す線分の表示
        {
            Vector3 from = lineStartPoint.transform.position;
            Vector3 to = lineMiddlePoint.transform.position;
            Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            Gizmos.DrawLine(from, to);
        }
        {
            Vector3 from = lineMiddlePoint.transform.position;
            Vector3 to = lineEndPoint.transform.position;
            Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            Gizmos.DrawLine(from, to);
        }
    }
}

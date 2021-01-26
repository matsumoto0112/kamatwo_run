using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1レーンを管理するクラス
/// </summary>
public class Lane : MonoBehaviour
{
    [SerializeField, Tooltip("スポーン可能な地点の開始地点")]
    private GameObject lineStartPoint;
    [SerializeField, Tooltip("スポーン可能な地点の終了地点")]
    private GameObject lineEndPoint;

    [SerializeField, Tooltip("生成地点を一定間隔にするためのステップ値")]
    private int step = 5;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// ランダムなスポーン地点を返す
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomSpawnPoint()
    {
        if (!(lineStartPoint && lineEndPoint)) { return Vector3.zero; }
        float t = Random.Range(0, step + 1) * 1.0f / step;
        return Vector3.Lerp(lineStartPoint.transform.position, lineEndPoint.transform.position, t);
    }

    private void OnDrawGizmos()
    {
        if (!(lineStartPoint && lineEndPoint)) { return; }

        //スポーン可能な地点を示す線分の表示
        Vector3 from = lineStartPoint.transform.position;
        Vector3 to = lineEndPoint.transform.position;
        Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        Gizmos.DrawLine(from, to);
    }
}

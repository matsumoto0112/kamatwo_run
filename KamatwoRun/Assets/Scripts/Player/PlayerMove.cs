using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class PlayerMove : CharacterComponent
{
    public LaneLocationType LocationType { get; private set; }

    private LanePositions lanePositions = null;

    public override void OnCreate()
    {
        LocationType = LaneLocationType.MIDDLE;
        lanePositions = transform.parent.GetComponentInChildren<LanePositions>();
        lanePositions.Initialize();
    }

    /// <summary>
    /// 左レーンへの状態を変更する
    /// </summary>
    public void LeftSideMoveTypeChange()
    {
        LocationType = (LaneLocationType)Mathf.Clamp((int)LocationType - 1, 0, Enum.GetValues(typeof(LaneLocationType)).Length - 1);
        Debug.Log($"num = {(int)LocationType} : type = {LocationType}");
    }

    public void RightSideMoveTypeChange()
    {
        LocationType = (LaneLocationType)Mathf.Clamp((int)LocationType + 1, 0, Enum.GetValues(typeof(LaneLocationType)).Length - 1);
        Debug.Log($"num = {(int)LocationType} : type = {LocationType}");
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    /// <param name="currentPosition"></param>
    /// <param name="t"></param>
    public void Move(Vector3 currentPosition, float t)
    {
        transform.position = Vector3.Lerp(currentPosition, NextMovePosition(), t);
    }

    /// <summary>
    /// ジャンプ時の最高到達点までにかかる時間を計算
    /// </summary>
    /// <param name="a">加速度</param>
    /// <param name="height">高さ</param>
    /// <returns></returns>
    public float CulcMaxArrivalTime(float a, float height)
    {
        //速度(m/s)² - 初速度(m/s)² = 2 * 加速度 * 変位
        float c = 2 * a * height;
        float v0 = Mathf.Sqrt(c * -1);

        float hightTime = v0 / (a * -1);
        //Debug.Log($"最高到達点に行くまでの時間->{hightTime}");
        return hightTime;
    }

    /// <summary>
    /// ジャンプ処理
    /// </summary>
    /// <param name="a">加速度</param>
    /// <param name="height">高さ</param>
    /// <param name="t">時間</param>
    /// <returns></returns>
    public float Jump(float a, float height, float t)
    {
        //速度(m/s)² - 初速度(m/s)² = 2 * 加速度 * 変位(最高到達点height)
        float c = 2 * a * height;
        float v0 = Mathf.Sqrt(c * -1);
        //Debug.Log($"初速->{v0}");

        //v = v0 + at;
        float v = v0 + (a * t);
        v *= Time.deltaTime;
        //Debug.Log($"現在の速度->{v}");
        transform.position += new Vector3(0, v, 0);
        return v;
    }

    /// <summary>
    /// 目的地までの距離
    /// </summary>
    /// <returns></returns>
    public float DistanceToDestination()
    {
        return Vector3.Distance(transform.position, NextMovePosition());
    }

    /// <summary>
    /// 次のレーン移動先の位置を返す
    /// </summary>
    /// <returns></returns>
    public Vector3 NextMovePosition()
    {
        return lanePositions.LanePositionList[(int)LocationType].position;
    }
}

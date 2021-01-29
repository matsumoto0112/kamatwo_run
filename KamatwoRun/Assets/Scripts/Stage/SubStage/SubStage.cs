using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// 出入口の種類
/// 出口のタイプと一致した入口を持つサブステージが連結可能である
/// </summary>
public enum GatewayType
{
    North,
    South,
    East,
    West,
}

public static class GatewayTypeExtend
{
    public static GatewayType ChainableType(GatewayType type)
    {
        switch (type)
        {
            case GatewayType.North:
                return GatewayType.South;
            case GatewayType.South:
                return GatewayType.North;
            case GatewayType.East:
                return GatewayType.West;
            default:
                return GatewayType.East;
        }
    }
}

/// <summary>
/// 分割された一ステージを管理する
/// </summary>
public class SubStage : MonoBehaviour
{
    [SerializeField, Tooltip("配置可能なオブジェクトリスト")]
    private SpawnObjectsParameter stageObjects;

    //生成済みオブジェクト
    private List<GameObject> spawnedObjects;

    [SerializeField, Tooltip("入口の種類")]
    private GatewayType entranceType;
    [SerializeField, Tooltip("出口の種類")]
    private GatewayType exitType;

    public GatewayType EntranceType { get { return entranceType; } }
    public GatewayType ExitType { get { return exitType; } }

    //コリジョン
    private BoxCollider boxCollider;
    //レーン配列
    private List<Lane> lanes;

    //進行方向を調べるコンポーネント
    private DirectionChecker directionChecker;
    private StageObjectSpawner spawner;

    /// <summary>
    /// カメラの範囲内かどうか
    /// </summary>
    public bool IsInsideCamera
    {
        get
        {
            //自身の中心から十字に伸びる点をもとに、その点がカメラの範囲内かどうか調べる
            Vector3 halfSize = boxCollider.size / 4 * 3;
            Vector3[] offsets = new Vector3[] {
                new Vector3(-halfSize.x, 0.0f, 0.0f),
                new Vector3(halfSize.x, 0.0f, 0.0f),
                new Vector3(0.0f, 0.0f, halfSize.z),
                new Vector3(0.0f, 0.0f, -halfSize.x), };

            foreach (var offset in offsets)
            {
                if (CheckInScreen(transform.position + offset)) { return true; }
            }
            return false;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        directionChecker = GetComponent<DirectionChecker>();
        directionChecker.Init(EntranceType, ExitType);
        spawner = FindObjectOfType<StageObjectSpawner>();
        spawnedObjects = new List<GameObject>();
        lanes = new List<Lane>();
        foreach (var lane in GetComponentsInChildren<Lane>())
        {
            lanes.Add(lane);
        };

        Assert.IsNotNull(spawner, "Spawnerが取得できませんでした");
        Assert.IsNotNull(boxCollider, "BoxColliderがアタッチされていません");
    }



    /// <summary>
    /// オブジェクトをスポーンする
    /// </summary>
    /// <param name="spawnNum"></param>
    public void SpawnObjects(int spawnNum)
    {
        for (int i = 0; i < spawnNum; i++)
        {
            //ランダムなレーンのランダムなスポーン地点を取り出す
            int laneNum = Random.Range(0, lanes.Count);
            Vector3? pointOrNull = lanes[laneNum].GetRandomSpawnPoint();
            //有効な値ならそれを使用してスポーン処理をする
            if (pointOrNull.HasValue)
            {
                //スポナーがスポーンに成功したら値が返ってくる
                GameObject spawned = spawner.SpawnIfSucceed(stageObjects.GetRandomObject(), pointOrNull.Value, laneNum);
                if (spawned)
                {
                    spawned.transform.SetParent(this.transform, true);
                    spawnedObjects.Add(spawned);
                }
            }
        }
    }

    /// <summary>
    /// 方向ベクトルからオブジェクトの回転方向を取得する
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    private Quaternion RotationFromDirectionVector(Vector3 dir)
    {
        if (dir.x == 0 && dir.z == -1)
        {
            return Quaternion.Euler(0, 0, 0);
        }
        else if (dir.x == 0 && dir.z == 1)
        {
            return Quaternion.Euler(0, 180, 0);
        }
        else if (dir.x == -1 && dir.z == 0)
        {
            return Quaternion.Euler(0, 90, 0);
        }
        else
        {
            return Quaternion.Euler(0, 270, 0);
        }
    }

    /// <summary>
    /// サブステージオブジェクトの移動処理
    /// </summary>
    /// <param name="direction">移動方向</param>
    /// <param name="speed">移動速度(差分時間考慮済み)</param>
    public void Move(Vector3 direction, float speed)
    {
        Vector3 moveAmount = direction * speed;
        this.transform.position += moveAmount;

        foreach (var obj in spawnedObjects)
        {
            obj.transform.rotation = RotationFromDirectionVector(direction);
        }
    }

    /// <summary>
    /// 手前の方向を取得する
    /// ステージの形状によって手前の方向が変わるため
    /// </summary>
    /// <param name="checkPosition">調べたい地点の座標</param>
    /// <returns></returns>
    public virtual Vector3 GetForegroundDirection(Vector3 checkPosition)
    {
        return -directionChecker.Directon(checkPosition);
    }

    /// <summary>
    /// サブステージの範囲内かどうか調べる
    /// </summary>
    /// <param name="checkPosition">調べたい座標</param>
    /// <returns>範囲内であればtrueを返す</returns>
    public bool IsInArea(Vector3 checkPosition)
    {
        return boxCollider.bounds.Contains(checkPosition);
    }

    /// <summary>
    /// 指定座標がカメラの範囲内に収まっているか
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    private bool CheckInScreen(Vector3 p)
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(p);
        if (viewPos.x < 0.0f || viewPos.x > 1.0f || viewPos.y < 0.0f || viewPos.y > 1.0f)
        {
            return false;
        }
        return true;
    }
}

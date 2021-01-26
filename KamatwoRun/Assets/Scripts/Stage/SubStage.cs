using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// 分割された一ステージを管理する
/// </summary>
public class SubStage : MonoBehaviour
{
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

    [SerializeField, Tooltip("配置可能なオブジェクトリスト")]
    private List<GameObject> stageObjects;

    //生成済みオブジェクト
    private List<GameObject> spawnedObjects;

    [SerializeField]
    private StageParameter stageParameter;
    [SerializeField, Tooltip("入口の種類")]
    private GatewayType entranceType;
    [SerializeField, Tooltip("入口の種類")]
    private GatewayType exitType;

    public GatewayType EntranceType { get { return entranceType; } }
    public GatewayType ExitType { get { return exitType; } }

    //コリジョン
    private BoxCollider boxCollider;
    //レーン配列
    private List<Lane> lanes;


    /// <summary>
    /// カメラの範囲内かどうか
    /// </summary>
    public bool IsInsideCamera
    {
        get
        {
            //自身の中心から十字に伸びる点をもとに、その点がカメラの範囲内かどうか調べる
            Vector3 halfSize = boxCollider.size / 2;
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
        spawnedObjects = new List<GameObject>();
        lanes = new List<Lane>();
        foreach (var lane in GetComponentsInChildren<Lane>())
        {
            lanes.Add(lane);
        };

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
            //ランダムなレーン番号を取得する
            int laneNum = Random.Range(0, lanes.Count);

            //そのレーンで生成可能な座標を取得する
            Vector3 spawnPosition = lanes[laneNum].GetRandomSpawnPoint();
            //敵が立つことを考えて少し上にずらす
            Vector3 enemyStandPosition = spawnPosition + Vector3.up;

            //同じ座標に生成しないようにテストする
            bool spawnTestSucceeded = true;
            foreach (var spawned in spawnedObjects)
            {
                //同じ座標値なら失敗とする
                if (Vector3.Distance(spawned.transform.position, enemyStandPosition) < Mathf.Epsilon) { spawnTestSucceeded = false; }
            }
            if (spawnTestSucceeded)
            {
                int spawnType = Random.Range(0, stageObjects.Count);
                GameObject obj = Instantiate(stageObjects[spawnType], enemyStandPosition, Quaternion.identity, this.transform);
                spawnedObjects.Add(obj);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

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
    }

    /// <summary>
    /// 手前の方向を取得する
    /// ステージの形状によって手前の方向が変わるため
    /// </summary>
    /// <param name="checkPosition">調べたい地点の座標</param>
    /// <returns></returns>
    public virtual Vector3 GetForegroundDirection(Vector3 checkPosition)
    {
        return Vector3.back;
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

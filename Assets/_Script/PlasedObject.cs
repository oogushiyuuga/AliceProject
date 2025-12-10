using UnityEngine;
using UnityEngine.EventSystems; // UI判定に必要

public class PlacedObject : MonoBehaviour
{
    private DragDropSpawner mySpawner; // 産みの親
    private Vector3 originalPos;
    private bool isDragging = false;
    
    // スマホでの誤タップ防止用の閾値
    private float dragThreshold = 0.5f; 

    public void Setup(DragDropSpawner spawner)
    {
        mySpawner = spawner;
    }

    // ドラッグ開始
    void OnMouseDrag()
    {
        // 設置フェーズ以外なら触れない
        if (GameManager.Instance.CurrentState != GameManager.GameState.Placement) return;
        if (mySpawner == null) return;

        isDragging = true;

        // ドラッグ中は、自分自身の当たり判定を消して、レイキャストが突き抜けるようにする
        // （これをしないと、自分が邪魔で地面の座標が取れない）
        SetColliderEnabled(false);

        // --- 移動処理 ---
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 地面(Ground)にレイが当たっているか？
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mySpawner.groundLayer))
        {
            // グリッドに合わせて移動
            Vector3 targetPos = hit.point;
            float gSize = mySpawner.gridSize;

            float x = Mathf.Round(targetPos.x / gSize) * gSize;
            float z = Mathf.Round(targetPos.z / gSize) * gSize;
            
            // 高さは固定（Spawnerの設定値があればそれを使う、なければ今の高さ）
            // 前回 Spawner に placeHeight を追加した場合は mySpawner.placeHeight を使ってください
            transform.position = new Vector3(x, transform.position.y, z);
        }
    }

    // 指を離した時（ドロップまたは削除判定）
    void OnMouseUp()
    {
        if (!isDragging) return;
        isDragging = false;
        
        // 当たり判定を復活させる
        SetColliderEnabled(true);

        if (mySpawner == null) return;

        // ★スマホ対応の最重要ポイント★
        // 指を離した場所が「UI（インベントリなど）」の上だった場合 → 削除（返却）
        if (IsPointerOverUI())
        {
            Debug.Log("UIの上で離したので削除します");
            mySpawner.ReturnItem(); // インベントリに戻す
            Destroy(gameObject);    // 自分を消す
            return;
        }

        // --- UIの上じゃなければ、配置判定 ---
        if (!IsPositionValid())
        {
            // 置けない場所なら元の位置に戻す
            transform.position = originalPos;
            Debug.Log("そこには置けません（障害物あり）");
        }
        else
        {
            // 配置確定（新しい場所を記憶）
            originalPos = transform.position;
            
            // ちょこっとエフェクト（音やパーティクル）を入れると気持ちいいです
        }
    }

    void Start()
    {
        originalPos = transform.position;
    }

    // コライダのオンオフ切り替え（子要素も含めて）
    private void SetColliderEnabled(bool isEnabled)
    {
        Collider[] cols = GetComponentsInChildren<Collider>();
        foreach (var c in cols) c.enabled = isEnabled;
    }

    // 配置可能かチェック（障害物判定）
    private bool IsPositionValid()
    {
        float gSize = mySpawner.gridSize;
        // 少し小さめの判定ボックスを作る（隣接マスへの誤反応防止）
        Vector3 halfExtents = Vector3.one * (gSize * 0.45f);
        Vector3 center = transform.position + Vector3.up * 0.5f; // 中心を少し上げる
        
        // 障害物レイヤーと重なっているかチェック
        Collider[] hitColliders = Physics.OverlapBox(center, halfExtents, Quaternion.identity, mySpawner.obstacleLayer);
        
        foreach (var c in hitColliders)
        {
            // 自分自身（と自分の子供）以外に当たっていたらNG
            if (c.transform.root != transform.root)
            {
                return false; 
            }
        }
        return true;
    }

    // ★スマホ・PC両対応の「UIに触れているか」判定メソッド
    private bool IsPointerOverUI()
    {
        // PC（マウス）の場合
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }

        // スマホ（タッチ）の場合
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            // 指IDを指定して判定する必要がある
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return true;
            }
        }

        return false;
    }
}
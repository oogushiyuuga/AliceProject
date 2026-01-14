using UnityEngine;
using UnityEngine.EventSystems;

public class PlacedObject : MonoBehaviour
{
    private DragDropSpawner mySpawner;
    private Vector3 originalPos;
    private bool isDragging = false;
    
    // ダブルタップ判定用
    private float lastTapTime = 0f;
    private const float doubleTapInterval = 0.3f;

    // Spawnerをセットする関数
    public void Setup(DragDropSpawner spawner)
    {
        mySpawner = spawner;
    }

    void Start()
    {
        originalPos = transform.position;
    }

    // ドラッグ中
    void OnMouseDrag()
    {
        // アクションフェーズ中は触れないように
        if (GameManager.Instance.CurrentState != GameManager.GameState.Placement) return;
        
        // Spawnerの設定がないとグリッド計算できないので中断
        if (mySpawner == null) return;

        isDragging = true;

        // 自分の当たり判定を一時的に切る（レイキャストが自分に当たらないように）
        SetColliderEnabled(false);

        // スマホのタッチ位置、またはマウス位置へレイを飛ばす
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Groundレイヤーに当たったら、その位置へ移動
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mySpawner.groundLayer))
        {
            // グリッドスナップ計算（親の設定値を使用）
            float gSize = mySpawner.gridSize;
            Vector3 targetPos = hit.point;

            float x = Mathf.Round(targetPos.x / gSize) * gSize;
            float z = Mathf.Round(targetPos.z / gSize) * gSize;
            
            // 高さは固定
            transform.position = new Vector3(x, transform.position.y, z);
        }
    }

    // 指を離した時（ドロップ、タップ判定）
    void OnMouseUp()
    {
        if (GameManager.Instance.CurrentState != GameManager.GameState.Placement) return;
        
        // 当たり判定を復活させる
        SetColliderEnabled(true);

        if (mySpawner == null) return;

        // ドラッグしていた場合
        if (isDragging)
        {
            isDragging = false;

            // インベントリの上で離したか判定
            if (IsPointerOverUI())
            {
                Debug.Log("UIへ戻したので削除");
                ReturnAndDestroy();
                return;
            }

            // 置ける場所か判定
            if (!IsPositionValid())
            {
                // 置けないなら、元の位置に戻す
                transform.position = originalPos;
                Debug.Log("そこには置けません");
            }
            else
            {
                // 置けるなら、ここを新しい「定位置」として記憶
                originalPos = transform.position;
            }
        }
        else
        {
            // ドラッグせずにその場で指を離した（＝タップ）
            
            // ダブルタップか判定
            float timeSinceLastTap = Time.time - lastTapTime;
            
            if (timeSinceLastTap < doubleTapInterval)
            {
                Debug.Log("ダブルタップで削除");
                ReturnAndDestroy();
                return;
            }
            
            // タップ時間を更新
            lastTapTime = Time.time;
        }
    }

    // アイテムをインベントリに返して消滅する処理
    private void ReturnAndDestroy()
    {
        if (mySpawner != null)
        {
            mySpawner.ReturnItem();
        }

        Destroy(gameObject);
    }

    // コライダのオンオフ切り替え
    private void SetColliderEnabled(bool isEnabled)
    {
        Collider[] cols = GetComponentsInChildren<Collider>();
        foreach (var c in cols) c.enabled = isEnabled;
    }

    // 配置可能かチェック（障害物判定）
    private bool IsPositionValid()
    {
        float gSize = mySpawner.gridSize;
        // 少し小さめの判定ボックス（隣接マスへの誤反応防止）
        Vector3 halfExtents = Vector3.one * (gSize * 0.45f);
        Vector3 center = transform.position + Vector3.up * 0.5f; 
        
        // 障害物レイヤーと重なっているかチェック
        Collider[] hitColliders = Physics.OverlapBox(center, halfExtents, Quaternion.identity, mySpawner.obstacleLayer);
        
        foreach (var c in hitColliders)
        {
            // 自分自身以外に当たっていたら置けない
            if (c.transform.root != transform.root)
            {
                return false; 
            }
        }
        return true;
    }

    // UIに触れているか判定メソッド
    private bool IsPointerOverUI()
    {
        // マウスの場合
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }

        // タッチの場合
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return true;
            }
        }

        return false;
    }
}
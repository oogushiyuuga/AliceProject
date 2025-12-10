using UnityEngine;

public class Turn : MonoBehaviour
{
    [Header("回転設定")]
    [Tooltip("90:右折, -90:左折, 180:Uターン")]
    [SerializeField] private float turnAngle = 90f;

    [Tooltip("通過後にアイテムを消すか？")]
    [SerializeField] private bool destroyOnUse = false;

    // 重複反応を防ぐためのクールタイム管理
    private bool isActivated = false;

    void OnTriggerEnter(Collider other)
    {
        // 連続で反応しないようにガード
        if (isActivated) return;

        if (other.CompareTag("Player"))
        {
            isActivated = true;
            
            // 1. プレイヤーを回転させる
            other.transform.Rotate(0, turnAngle, 0);

            // 回転後の角度を90度単位に補正する
            Vector3 currentEuler = other.transform.eulerAngles;
            float roundedY = Mathf.Round(currentEuler.y / 90f) * 90f;
            other.transform.rotation = Quaternion.Euler(0, roundedY, 0);

            // 曲がった後に「通路の端っこ」を歩かないようにする
            //SnapToGrid(other.transform); 

            Debug.Log("方向転換しました: " + turnAngle);

            if (destroyOnUse)
            {
                Destroy(gameObject);
            }
            else
            {
                // 少し待ってから再反応できるようにする（無限ループ防止）
                Invoke(nameof(ResetActivation), 0.5f);
            }
        }
    }

    private void ResetActivation()
    {
        isActivated = false;
    }
    
    // 位置補正用
    private void SnapToGrid(Transform target)
    {
        // グリッドサイズが2.0の場合の例
        float gridSize = 2.0f;
        Vector3 pos = target.position;
        pos.x = Mathf.Round(pos.x / gridSize) * gridSize;
        pos.z = Mathf.Round(pos.z / gridSize) * gridSize;
        target.position = pos;
    }
}
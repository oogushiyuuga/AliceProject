using UnityEngine;

public class WindSwitch : MonoBehaviour
{
    [Header("操作する風")]
    [SerializeField] private WindMove targetWind;

    [Header("スイッチの設定")]
    [SerializeField] private bool oneTimeOnly = false;
    
    [Tooltip("触れた時に色を変える対象")]
    [SerializeField] private Renderer switchRenderer;
    [SerializeField] private Material onMaterial;
    [SerializeField] private Material offMaterial;

    private bool hasUsed = false;

    // プレイヤーが触れたら切り替え
    void OnTriggerEnter(Collider other)
    {
        if (oneTimeOnly && hasUsed) return;

        if (other.CompareTag("Player"))
        {
            if (targetWind != null)
            {
                // 風を切り替える
                targetWind.ToggleWind();
                
                // 1回限定なら使用済みにする
                if (oneTimeOnly) hasUsed = true;

                // 押した感触（色変えなど）
                ChangeColor();
            }
        }
    }

    private void ChangeColor()
    {
        if (switchRenderer != null && onMaterial != null && offMaterial != null)
        {
            // 今のマテリアルを見て交互に入れ替える簡易処理
            if (switchRenderer.sharedMaterial == offMaterial)
            {
                switchRenderer.material = onMaterial;
            }
            else
            {
                switchRenderer.material = offMaterial;
            }
        }
    }
}
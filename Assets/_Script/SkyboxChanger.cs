using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    [Header("交換するSkyboxを設定")]
    [SerializeField] private Material newSkyboxMaterial; 

    private Material originalSkybox;
    private bool isChanged = false;

    void Start()
    {
        // 元のSkyboxを覚えておく（ゲーム終了時に戻すため）
        originalSkybox = RenderSettings.skybox;
    }

    void OnTriggerEnter(Collider other)
    {
        // プレイヤーが通過したら発動
        if (other.CompareTag("Player") && !isChanged)
        {
            SwapSkybox();
            isChanged = true;
        }
    }

    void SwapSkybox()
    {
        if (newSkyboxMaterial != null)
        {
            // Skyboxを入れ替える
            RenderSettings.skybox = newSkyboxMaterial;

            // 環境光（ライティング）を更新する
            DynamicGI.UpdateEnvironment();
        }
    }

    // ゲーム終了時などに元に戻す
    void OnDestroy()
    {
        RenderSettings.skybox = originalSkybox;
        DynamicGI.UpdateEnvironment();
    }

    void OnApplicationQuit()
    {
        RenderSettings.skybox = originalSkybox;
    }
}
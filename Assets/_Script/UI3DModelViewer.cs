using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class UI3DModelViewer : MonoBehaviour
{
    [Header("映したい3Dモデル")]
    [SerializeField] private GameObject modelPrefab;
    
    [Header("表示設定")]
    [SerializeField] private Vector3 modelOffset = Vector3.zero; // 位置微調整
    [SerializeField] private Vector3 modelRotation = new Vector3(0, 45, 0); // 角度
    [SerializeField] private float modelScale = 1.0f; // 大きさ
    [SerializeField] private Color backgroundColor = new Color(0, 0, 0, 0); // 背景色（透明）

    // 内部で使う変数
    private GameObject studioObject; // 生成したスタジオ
    private Camera studioCamera;
    private RenderTexture renderTexture;
    private RawImage targetImage;

    // スタジオが重ならないように座標をずらすためのカウンター
    private static int instanceCount = 0;

    void Start()
    {
        SetupStudio();
    }

    void SetupStudio()
    {
        targetImage = GetComponent<RawImage>();

        // 専用のRenderTextureをメモリ上に生成（256x256）
        renderTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        renderTexture.Create();

        // UI（RawImage）にセット
        targetImage.texture = renderTexture;

        // 撮影スタジオの場所を決める（個数ごとにY座標をずらして重なり防止）
        instanceCount++;
        Vector3 studioPos = new Vector3(0, -1000 - (instanceCount * 50), 0);

        // スタジオを作成
        studioObject = new GameObject("IconStudio_" + gameObject.name);
        studioObject.transform.position = studioPos;

        // カメラを作成してスタジオに入れる
        GameObject camObj = new GameObject("StudioCamera");
        camObj.transform.SetParent(studioObject.transform);
        camObj.transform.localPosition = new Vector3(0, 0, -5); // モデルの手前に配置

        studioCamera = camObj.AddComponent<Camera>();
        studioCamera.clearFlags = CameraClearFlags.SolidColor;
        studioCamera.backgroundColor = backgroundColor;
        studioCamera.targetTexture = renderTexture; // 映像の送り先

        // モデルを生成してスタジオに入れる
        if (modelPrefab != null)
        {
            GameObject model = Instantiate(modelPrefab, studioObject.transform);
            model.transform.localPosition = modelOffset;
            model.transform.localEulerAngles = modelRotation;
            model.transform.localScale = Vector3.one * modelScale;
        }
    }

    // オブジェクトが消える時に、作ったものを片付ける
    void OnDestroy()
    {
        if (renderTexture != null)
        {
            renderTexture.Release(); // メモリ開放
        }
        
        if (studioObject != null)
        {
            Destroy(studioObject); // スタジオ削除
        }
    }
}
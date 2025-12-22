using UnityEngine;
using UnityEngine.UI;

public class RainbowTextAnim : MonoBehaviour
{
    [Header("動きの設定")]
    [SerializeField] private float speed = 5.0f; // 動く速さ
    [SerializeField] private float scaleAmount = 0.2f; // 伸び縮みの激しさ
    [SerializeField] private float rotateAmount = 5.0f; // 回転の激しさ

    [Header("色の設定")]
    [SerializeField] private bool useRainbow = true; // 虹色にするか？
    [SerializeField] private float colorSpeed = 0.5f; // 色が変わる速さ

    private Text myText;
    private Vector3 originalScale;

    void Start()
    {
        myText = GetComponent<Text>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        // 時間の経過を作る
        float time = Time.time * speed;

        // モチモチ動く
        float scaleX = 1.0f + Mathf.Sin(time) * scaleAmount;
        float scaleY = 1.0f + Mathf.Cos(time) * scaleAmount;
        transform.localScale = new Vector3(originalScale.x * scaleX, originalScale.y * scaleY, 1);

        // ゆらゆら揺れる
        float zRot = Mathf.Sin(time * 0.5f) * rotateAmount;
        transform.localRotation = Quaternion.Euler(0, 0, zRot);

        // ゲーミング虹色発光
        if (useRainbow && myText != null)
        {
            float hue = Mathf.Repeat(Time.time * colorSpeed, 1.0f);
            myText.color = Color.HSVToRGB(hue, 1.0f, 1.0f);
        }
    }
}
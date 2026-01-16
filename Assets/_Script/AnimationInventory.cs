using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Animetioninventory : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private Button tabButton; // 開閉ボタン
    [SerializeField] private float animDuration = 0.4f; // アニメーションにかかる時間

    [Header("位置設定（Y座標）")]
    [SerializeField] private float closedY = 150f; // 閉じている時のY（画面外）
    [SerializeField] private float openedY = -150f;  // 開いている時のY（画面内）

    [Header("動きのカーブ")]
    [SerializeField] private AnimationCurve motionCurve = AnimationCurve.Linear(0, 0, 1, 1);

    private RectTransform rectTransform;
    private bool isOpen = false;
    private Coroutine currentCoroutine;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        
        // ボタンにクリック機能を登録
        if (tabButton != null)
        {
            tabButton.onClick.AddListener(OnTabClicked);
        }

        // 最初は閉じた位置に強制移動
        SetPosition(closedY);
        isOpen = false;
    }

    // ボタンが押されたら呼ばれる
    void OnTabClicked()
    {
        // 動作中なら前の動きをキャンセルして新しく動かす
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);

        isOpen = !isOpen; // フラグ反転
        
        float targetY = isOpen ? openedY : closedY;
        currentCoroutine = StartCoroutine(MoveTo(targetY));
    }

    // アニメーション処理
    private IEnumerator MoveTo(float targetY)
    {
        float startY = rectTransform.anchoredPosition.y;
        float timer = 0f;

        while (timer < animDuration)
        {
            timer += Time.deltaTime;
            float percent = Mathf.Clamp01(timer / animDuration);
            
            // カーブを使って動きに強弱をつける（「落ちてくる」感）
            float curveValue = motionCurve.Evaluate(percent);

            // 現在位置を計算
            float newY = Mathf.Lerp(startY, targetY, curveValue);
            
            SetPosition(newY);
            yield return null;
        }

        // 最後にズレがないように目標位置へピタッと合わせる
        SetPosition(targetY);
        currentCoroutine = null;
    }

    private void SetPosition(float y)
    {
        Vector2 pos = rectTransform.anchoredPosition;
        pos.y = y;
        rectTransform.anchoredPosition = pos;
    }
}
using UnityEngine;

public class Tip : MonoBehaviour
{
    [SerializeField] private GameObject tipButton; 
    [SerializeField] private GameObject tipWindow;
    [SerializeField] private int showTipCount = 3;
    private static int consecutiveGameOverCount = 0;

    void Start()
    {
        consecutiveGameOverCount++;
        Debug.Log("現在の連続ゲームオーバー回数: " + consecutiveGameOverCount);

        // 指定した回数以上に達していたら、ヒントボタンを表示する
        if (consecutiveGameOverCount >= showTipCount)
        {
            if (tipButton != null) tipButton.SetActive(true);
        }
        else
        {
            if (tipButton != null) tipButton.SetActive(false);
        }

        // 最初はヒントウィンドウ自体は閉じておく
        if (tipWindow != null) tipWindow.SetActive(false);
    }

    // ヒントボタンを押した時に呼ばれる
    public void OnTapOpen()
    {
        AudioManager.Instance.PlaySE("Button"); 
        if (tipWindow != null) tipWindow.SetActive(true);
    }

    // ヒントウィンドウを閉じる用
    public void OnTapClose()
    {
        AudioManager.Instance.PlaySE("Button");
        if (tipWindow != null) tipWindow.SetActive(false);
    }

    // 外部からカウントを0に戻すための処理
    public static void ResetGameOverCount()
    {
        consecutiveGameOverCount = 0;
    }
}
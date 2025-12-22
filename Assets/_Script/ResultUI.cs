using UnityEngine;

public class ResultUI : MonoBehaviour
{
    // ボタンが押されたら実行される関数
    public void OnClickNextButton()
    {
        // GameManagerのインスタンスを探して、次のステージへ進む処理を呼ぶ
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnNextStageClicked();
        }
        else
        {
            Debug.LogError("GameManagerが見つかりません！");
        }
    }
}
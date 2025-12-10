using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    // リトライボタン用
    public void OnRetryClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RetryStage();
        }
    }

    // ステージ選択へ戻るボタン用
    public void OnStageSelectClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GoToStageSelect();
        }
    }
}
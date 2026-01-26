using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlaySE("GameOver");
    }
    // リトライボタン用
    public void OnRetryClicked()
    {
        AudioManager.Instance.PlaySE("Button");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.RetryStage();
        }
    }

    // ステージ選択へ戻るボタン用
    public void OnStageSelectClicked()
    {
        AudioManager.Instance.PlaySE("Button");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GoToStageSelect();
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private Text Result;
    [SerializeField] private GameObject pauseCanvas;

    public void Open_Explain()
    {
        if (pauseCanvas != null && pauseCanvas.activeSelf)
        {
            pauseCanvas.SetActive(false);
        }

        var options = new ConfirmDialogOptions
        {
            PrefabPath = "Canvas_Explain", // どのプレハブを使うか指定
            OnOK = () => 
            { 
                Result.text = "";
                if (pauseCanvas != null) pauseCanvas.SetActive(true);
            }
        };
        ConfirmDialogController.Show(options);
    }

    public void Open_Credit()
    {
        var options = new ConfirmDialogOptions
        {
            PrefabPath = "Canvas_Credit", 
            OnOK = () => { Result.text = "合格通知を送信しました。"; }
        };
        ConfirmDialogController.Show(options);
    }

 public void Open_Pause()
    {
        var options = new ConfirmDialogOptions
        {
            PrefabPath = "Canvas_Pause", 
            OnOK = () => { Result.text = "ポーズを開きました"; }
        };
        ConfirmDialogController.Show(options);
    }

    public void OnTapOpen_Quit()
    {
        var options = new ConfirmDialogOptions
        {
            PrefabPath = "Canvas_Quit", 
            Title = "終了画面", // タイトルの設定
            OnOK = () => { Result.text = "ゲームを終了します"; },
            OnCancel = () => { Result.text = "ゲームを再開します"; }
        };
        ConfirmDialogController.Show(options);
    }
}
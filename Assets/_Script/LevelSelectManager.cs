using UnityEngine;
using UnityEngine.SceneManagement; // シーン移動に必須

public class LevelSelectManager : MonoBehaviour
{
    // ボタンからこの関数を呼び出します
    // 引数の stageNum に 1 や 2 を入れて使います
    public void OnStageButtonClicked(int stageNum)
    {
        AudioManager.Instance.PlaySE("Select");
        
        // シーンの名前を作成（例: "Stage" + 1 → "Stage1"）
        string sceneName = "Stage" + stageNum;

        // シーンを読み込む
        SceneManager.LoadScene(sceneName);
    }
}
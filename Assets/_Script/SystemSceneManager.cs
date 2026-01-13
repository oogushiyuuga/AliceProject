using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemSceneManager : MonoBehaviour
{
    // スタートボタンを押した時
    public void OnStartClicked()
    {
        SceneManager.LoadScene("StageSelectScene");
    }

    // 終了ボタンを押した時
    public void OnExitClicked()
    {
        // エディタで実行中はプレイモードを止める（デバッグ用）
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // 本番のアプリではゲームを閉じる
            Application.Quit();
        #endif
    }
}
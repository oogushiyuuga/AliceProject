using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemSceneManager : MonoBehaviour
{
    // スタートボタンを押した時
    public void OnStartClicked()
    {
        // 前回作った「ステージ選択画面」のシーン名を正確に入れる
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
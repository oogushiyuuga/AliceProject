using UnityEngine;
using UnityEngine.UI;

public class StageCrownController : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private string targetSceneName;
    [SerializeField] private GameObject crownIcon;

    void Start()
    {
        // 設定漏れ防止チェック
        if (string.IsNullOrEmpty(targetSceneName) || crownIcon == null)
        {
            Debug.LogWarning(gameObject.name + " : StageCrownControllerの設定が足りません！");
            return;
        }

        // 保存されているキー名を再現する (例: "Clear_Stage1")
        string saveKey = "Clear_" + targetSceneName;

        // PlayerPrefsを見て、クリア情報があるか確認する
        // GetIntはキーが存在しない場合、デフォルト値（ここでは0）を返す
        bool isCleared = PlayerPrefs.GetInt(saveKey, 0) == 1;

        // クリア済みなら王冠を表示する
        if (isCleared)
        {
            crownIcon.SetActive(true);
        }
    }
}
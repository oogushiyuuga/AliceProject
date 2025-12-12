using UnityEngine;

public class UnityChanDummy : MonoBehaviour
{
    // アニメーションから呼ばれるイベントを受け止めるための空っぽの関数
    // 引数（string str）がないとエラーになる場合があるのでつけておく
    public void OnCallChangeFace(string str)
    {
        // 何もしない（エラー回避用）
    }
}
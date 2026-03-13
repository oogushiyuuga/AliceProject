using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public GameObject[] cameraArray; // カメラを取得する用の配列を作成する
    bool judgment;  // カメラをオンにするかどうかを判定する変数

    public void OnButtonClick(int activeCamera)
    {
        // カメラの配列の数を繰り返す処理
        for (int i = 0; i < cameraArray.Length; i++)
        {
            if (i == activeCamera)
            {
                judgment = true;    // ボタンの引数とカメラの配列の要素番号が一緒ならtrue
            }
            else if (i != activeCamera)
            {
                judgment = false;   // ボタンの引数とカメラの配列の要素番号が違うならfalse
            }
            cameraArray[i].SetActive(judgment); // それぞれのカメラのオンオフを行う処理
        }
    }
}
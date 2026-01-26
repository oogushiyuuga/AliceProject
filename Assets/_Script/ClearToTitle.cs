using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearToTitle : MonoBehaviour
{
    public void OnTitleScene()
    {
        AudioManager.Instance.PlaySE("Button");
        AudioManager.Instance.StopBGM();
        SceneManager.LoadScene("StartScene");
    }

    public void OnSelectScene()
    {
        AudioManager.Instance.PlaySE("Button");
        AudioManager.Instance.StopBGM();
        SceneManager.LoadScene("StageSelectScene");
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearToTitle : MonoBehaviour
{
    public void OnTitleScene()
    {
        AudioManager.Instance.StopBGM();
        SceneManager.LoadScene("StartScene");
    }

    public void OnSelectScene()
    {
        AudioManager.Instance.StopBGM();
        SceneManager.LoadScene("StageSelectScene");
    }
}

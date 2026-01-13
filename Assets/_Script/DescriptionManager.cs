using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DescriptionManager : MonoBehaviour
{
    public static DescriptionManager Instance;

    [Header("UIパーツ")]
    [SerializeField] private GameObject panelObj;
    //[SerializeField] private Text descriptionText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    void Awake()
    {
        Instance = this;
        HideDescription();
    }

    // 説明を表示する関数
    public void ShowDescription(string text)
    {
        if (panelObj != null) panelObj.SetActive(true);
        if (descriptionText != null) descriptionText.text = text;
    }

    // 説明を隠す関数
    public void HideDescription()
    {
        if (panelObj != null) panelObj.SetActive(false);
        if (descriptionText != null) descriptionText.text = "";
    }
}
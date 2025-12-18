using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea(3, 5)]
    public string myDescription = "説明文";
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(DescriptionManager.Instance != null)
        {
            DescriptionManager.Instance.ShowDescription(myDescription);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(DescriptionManager.Instance != null)
        {
            DescriptionManager.Instance.HideDescription();
        }
    }
}

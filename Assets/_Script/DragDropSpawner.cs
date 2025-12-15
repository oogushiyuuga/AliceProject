using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class DragDropSpawner : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("アイテム設定")]
    [SerializeField] private int itemCount = 2; // 所持数
    [SerializeField] private Text countText; // 個数を表示するUIテキスト

    [Header("オブジェクト設定")]
    [SerializeField] private GameObject objectPrefab;

    [Header("見た目")]
    [SerializeField] private Material previewMaterialValid;
    [SerializeField] private Material previewMaterialInvalid;

    [Header("グリッド設定")]
    [SerializeField] private float placeHeight = 0.5f;
    public float gridSize = 2.0f;
    public LayerMask groundLayer;
    public LayerMask obstacleLayer;

    private Transform originalParent;
    private Vector3 startPosition;
    private CanvasGroup canvasGroup;
    private Transform canvasTransform;
    private GameObject currentPreview;
    private bool canPlace = false;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Canvas parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas != null)
        {
            canvasTransform = parentCanvas.transform;
        }
        else
        {
            Debug.LogError("DragDropSpawner: 親Canvasが見つかりません");
        }
    }

    void Start()
    {
        // ゲーム開始時に個数表示を更新
        UpdateCountText();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // アイテムがなければドラッグできないようにする（念のため）
        if (itemCount <= 0) return;

        originalParent = transform.parent;
        startPosition = transform.position;

        transform.SetParent(canvasTransform);
        canvasGroup.blocksRaycasts = false;
        
        canPlace = false;
        CreatePreviewObject();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemCount <= 0) return;

        transform.position = eventData.position;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Groundのみを対象にする設定済み前提
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            if (currentPreview != null)
            {
                currentPreview.SetActive(true);
                
                // 床の高さ(0f)に固定して計算
                Vector3 snappedPos = GetSnappedPosition(hit.point); 
                currentPreview.transform.position = snappedPos;

                canPlace = CheckPosition(snappedPos);
                UpdatePreviewColor(canPlace);
            }
        }
        else
        {
            if (currentPreview != null) currentPreview.SetActive(false);
            canPlace = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (itemCount <= 0) return;

        canvasGroup.blocksRaycasts = true;
        transform.SetParent(originalParent);
        transform.position = startPosition;

        // 配置可能フラグが立っていて、かつプレビューが表示されている時のみ
        if (canPlace && currentPreview != null && currentPreview.activeSelf)
        {
            SpawnObject(currentPreview.transform.position);
        }

        if (currentPreview != null) Destroy(currentPreview);
    }

    private void SpawnObject(Vector3 position)
    {
        if (objectPrefab != null)
        {
            GameObject obj = Instantiate(objectPrefab, position, objectPrefab.transform.rotation);
        
            PlacedObject placedObj = obj.GetComponent<PlacedObject>();
            if (placedObj != null)
            {
                placedObj.Setup(this);
            }

            ReduceItemCount();
        }
    }

    private void ReduceItemCount()
    {
        itemCount--;
        UpdateCountText();

        Debug.Log("残りのアイテム数: " + itemCount);

        if (itemCount <= 0)
        {
            canvasGroup.alpha = 0.5f; // 半透明
            canvasGroup.blocksRaycasts = false; // 触れない
        }
    }

    public void ReturnItem()
    {
        itemCount++;
        UpdateCountText();

        if (itemCount > 0)
        {
            canvasGroup.alpha = 1.0f;
            canvasGroup.blocksRaycasts = true;
        }
    }

    private void UpdateCountText()
    {
        if (countText != null)
        {
            countText.text = "x" + itemCount.ToString();
        }
    }

    private bool CheckPosition(Vector3 position)
    {
        Vector3 halfExtents = Vector3.one * (gridSize * 0.45f);
        Vector3 center = position + Vector3.up * (gridSize * 0.5f);
        bool hasHit = Physics.CheckBox(center, halfExtents, Quaternion.identity, obstacleLayer);
        return !hasHit;
    }

    private void UpdatePreviewColor(bool isValid)
    {
        if (currentPreview == null) return;
        Material targetMat = isValid ? previewMaterialValid : previewMaterialInvalid;
        
        Renderer[] renderers = currentPreview.GetComponentsInChildren<Renderer>();
        foreach (var rend in renderers)
        {
            Material[] mats = new Material[rend.sharedMaterials.Length];
            for (int i = 0; i < mats.Length; i++) mats[i] = targetMat;
            rend.sharedMaterials = mats;
        }
    }

    private Vector3 GetSnappedPosition(Vector3 targetPosition)
    {
        float x = Mathf.Round(targetPosition.x / gridSize) * gridSize;
        float z = Mathf.Round(targetPosition.z / gridSize) * gridSize;
        // 床の高さ（0f）に固定
        return new Vector3(x, placeHeight, z); 
    }

    private void CreatePreviewObject()
    {
        if (objectPrefab == null) return;
        currentPreview = Instantiate(objectPrefab);
        
        Collider[] colliders = currentPreview.GetComponentsInChildren<Collider>();
        foreach (var col in colliders) col.enabled = false;

        currentPreview.SetActive(false);
    }
}
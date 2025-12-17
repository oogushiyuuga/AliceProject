using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject placementCamera;
    [SerializeField] private GameObject playerCamera;

    [Header("UI設定")]
    [SerializeField] private GameObject resultCanvasPrefab;
    [SerializeField] private GameObject gameOverCanvasPrefab;
    [SerializeField] private MonoBehaviour playerMovementScript; 
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject iconStudioPrefab;
    [SerializeField] private GameObject LargeiconStudioPrefab;
    public enum GameState { Placement, Action, Result, GameOver }
    public GameState CurrentState { get; private set; }
    private GameObject currentResultCanvas;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    
        Instance = this;
    }

    void Start()
    {
        if (iconStudioPrefab != null)
        {
            Instantiate(iconStudioPrefab);
        }

        if (LargeiconStudioPrefab != null)
        {
            Instantiate(LargeiconStudioPrefab);
        }
        
        SwitchToPlacementPhase();
    }

    // 設置フェーズへ切り替え
    public void SwitchToPlacementPhase()
    {
        CurrentState = GameState.Placement;
        if (placementCamera != null) placementCamera.SetActive(true);

        if (playerCamera != null) playerCamera.SetActive(false);

        if (playerMovementScript != null) playerMovementScript.enabled = false;

        if (inventoryUI != null) inventoryUI.SetActive(true);

        if (startButton != null) startButton.SetActive(true);

        //マウスカーソルを表示・ロック解除
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // アクションフェーズへ切り替え（ボタンから呼ぶ）
    public void OnStartActionButtonClicked()
    {
        CurrentState = GameState.Action;
        if (placementCamera != null) placementCamera.SetActive(false);
        
        if (playerCamera != null) playerCamera.SetActive(true);

        if (playerMovementScript != null) playerMovementScript.enabled = true;

        if (inventoryUI != null) inventoryUI.SetActive(false);

        if (startButton != null) startButton.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void GameClear()
    {
        if (CurrentState == GameState.Result || CurrentState == GameState.GameOver) return;

        Debug.Log("ゲームクリア！");
        CurrentState = GameState.Result;

        if (resultCanvasPrefab != null)
        {
            // すでに表示されていたら消す（二重防止）
            if (currentResultCanvas != null) Destroy(currentResultCanvas);

            // プレハブを生成（親指定なし＝シーンのルートに生成）
            currentResultCanvas = Instantiate(resultCanvasPrefab);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (inventoryUI != null) inventoryUI.SetActive(false);
    }

    public void OnNextStageClicked()
    {
        // Build Settingsに登録されているシーン順序を見て、次のシーン番号を取得
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // 次のシーンが存在するか確認して移動
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("全ステージクリア！タイトルに戻ります");
            SceneManager.LoadScene("StartScene");
        }
    }

    public void TriggerGameOver()
    {
        if (CurrentState == GameState.Result || CurrentState == GameState.GameOver) return;

        Debug.Log("ゲームオーバー...");
        CurrentState = GameState.GameOver;

        if (gameOverCanvasPrefab != null)
        {
            Instantiate(gameOverCanvasPrefab);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        if (inventoryUI != null) inventoryUI.SetActive(false);
    }

    // リトライ（今のシーンを読み込み直す）
    public void RetryStage()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    // ステージ選択へ戻る
    public void GoToStageSelect()
    {
        SceneManager.LoadScene("StageSelectScene");
    }
}
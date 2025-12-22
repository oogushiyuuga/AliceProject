using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject placementCamera;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject resultCamera;

    [Header("UI設定")]
    [SerializeField] private GameObject resultCanvasPrefab;
    [SerializeField] private GameObject gameOverCanvasPrefab;
    [SerializeField] private MonoBehaviour playerMovementScript; 
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject startButton;
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
        SwitchToPlacementPhase();
        AudioManager.Instance.PlayBGM("BGM");
    }

    // 設置フェーズへ切り替え
    public void SwitchToPlacementPhase()
    {
        CurrentState = GameState.Placement;
        if (placementCamera != null) placementCamera.SetActive(true);

        if (playerCamera != null) playerCamera.SetActive(false);

        if(resultCamera != null) resultCamera.SetActive(false);

        if (playerMovementScript != null) playerMovementScript.enabled = false;

        if (inventoryUI != null) inventoryUI.SetActive(true);

        if (startButton != null) startButton.SetActive(true);

        #if UNITY_STANDALONE || UNITY_EDITOR
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        #endif
    }

    // アクションフェーズへ切り替え（ボタンから呼ぶ）
    public void OnStartActionButtonClicked()
    {
        CurrentState = GameState.Action;

        if (placementCamera != null) placementCamera.SetActive(false);
        
        if (playerCamera != null) playerCamera.SetActive(true);
        
        if(resultCamera != null) resultCamera.SetActive(false);

        if (playerMovementScript != null) playerMovementScript.enabled = true;

        if (inventoryUI != null) inventoryUI.SetActive(false);

        if (startButton != null) startButton.SetActive(false);

        // ロックすると、スマホでタッチ座標が取れなくなるケースがあるため避ける
        #if UNITY_STANDALONE || UNITY_EDITOR
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        #endif
    }

    public void GameClear()
    {
        if (CurrentState == GameState.Result || CurrentState == GameState.GameOver) return;

        Debug.Log("ゲームクリア！");
        CurrentState = GameState.Result;

        AudioManager.Instance.PlaySE("Gameclear_cute");

        if (placementCamera != null) placementCamera.SetActive(false);
        
        if (playerCamera != null) playerCamera.SetActive(false);
        
        if(resultCamera != null) resultCamera.SetActive(true);

        if (playerMovementScript != null)
        {
            if (playerMovementScript is PlayerMove player)
            {
                player.OnVictory();
            }
        }

        if (resultCanvasPrefab != null)
        {
            // すでに表示されていたら消す（二重防止）
            if (currentResultCanvas != null) Destroy(currentResultCanvas);

            // プレハブを生成（親指定なし＝シーンのルートに生成）
            currentResultCanvas = Instantiate(resultCanvasPrefab);
        }

        #if UNITY_STANDALONE || UNITY_EDITOR
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        #endif

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
            Debug.Log("全ステージクリア！");
            SceneManager.LoadScene("ClearScene");
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
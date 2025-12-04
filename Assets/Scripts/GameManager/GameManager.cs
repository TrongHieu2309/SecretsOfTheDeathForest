using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private Animator animAddCoin;
    [SerializeField] private GameObject pauseGameUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject winningUI;
    [SerializeField] private GameObject goalPoint;
    [SerializeField] private GameObject limitPoint;
    [SerializeField] private AudioClip takeCoin;

    public bool isPauseGame;
    public bool isGameOver;
    public bool isWinGame;
    private float currentCoin;

    void Awake()
    {
        instance = this;
        pauseGameUI.SetActive(false);
        gameOverUI.SetActive(false);
        goalPoint.SetActive(false);
        limitPoint.SetActive(true);
        winningUI.SetActive(false);
        isWinGame = false;
    }

    void Update()
    {
        if (isGameOver)
        {
            ToggleGameOver();
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        NextLevel();
    }

    /*-----COIN MANAGEMENT-----*/
    #region Coin Management
    public void AddCoins(float coinValue)
    {
        currentCoin += coinValue;
        animAddCoin.SetTrigger("addCoin");
        SoundManager.instance.PlaySound(takeCoin);
        UpdateCoins();
    }

    private void UpdateCoins()
    {
        // coinText.text = currentCoin.ToString() + " x";
        coinText.text = $"{currentCoin} x";
    }
    #endregion

    /*-----ENERGY MANAGEMENT-----*/
    #region Energy Management
    public void AddEnergy(float energyValue)
    {
        PlayerController.instance.currentSkill = Mathf.Min(PlayerController.instance.currentSkill + energyValue, PlayerController.instance.maxSkill);
        SoundManager.instance.PlaySound(takeCoin);
        UpdateEnergy();
    }

    private void UpdateEnergy()
    {
        PlayerController.instance.UpdateSkillBar();
    }
    #endregion

    /*-----USER INTERFACE MANAGEMENT-----*/
    #region UI Management
    private void TogglePause()
    {
        isPauseGame = !isPauseGame;
        pauseGameUI.SetActive(isPauseGame);
        Time.timeScale = isPauseGame ? 0f : 1f;
    }


    private void ToggleGameOver()
    {
        gameOverUI.SetActive(isGameOver);
        Time.timeScale = isGameOver == true ? 0f : 1f;
    }

    private void ToggleWinning()
    {
        winningUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void WinGame()
    {
        ToggleWinning();
        isWinGame = true;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
    #endregion

    private void NextLevel()
    {
        if (coinText.text == "10 x")
        {
            goalPoint.SetActive(true);
            limitPoint.SetActive(false);
        }
    }

    /*-----EVENT ANIMATION-----*/   
    #region Event Aniamtion
    public void HandleRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Resume()
    {
        TogglePause();
    }
    public void Quit()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void GameOverManagement()
    {
        isGameOver = true;
    }
    #endregion
}

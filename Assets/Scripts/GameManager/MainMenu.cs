using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour 
{
    [SerializeField] private GameObject tutorialUI;
    [SerializeField] private GameObject mainmenu;
    [SerializeField] private GameObject vine;
    [SerializeField] private GameObject Enemy;
    [SerializeField] private GameObject EnemyDead;
    private bool isTutorial;

    void Awake()
    {
        isTutorial = false;
    }

    void Update()
    {
        if (isTutorial)
        {
            vine.SetActive(false);
            Enemy.SetActive(false);
            EnemyDead.SetActive(false);
        }
        else
        {
            vine.SetActive(true);
            Enemy.SetActive(true);
            EnemyDead.SetActive(true);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void TutorialUI()
    {
        isTutorial = !isTutorial;
        tutorialUI.SetActive(isTutorial);
        mainmenu.SetActive(!isTutorial);
    }

    public void QuitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
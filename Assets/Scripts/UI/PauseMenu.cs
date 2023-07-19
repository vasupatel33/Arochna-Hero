using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [Header("States")]
    private bool paused;
    public bool Paused { get { return paused; } }
    private float textSize;

    [Header("References")]
    private GameObject pauseMenu;
    private TextMeshProUGUI resume;
    private TextMeshProUGUI mainMenu;
    private TextMeshProUGUI quit;

    void Awake()
    {
        pauseMenu = GameObject.Find("Canvas").transform.Find("PauseMenu").gameObject;

        resume = pauseMenu.transform.Find("Resume").GetChild(0).GetComponent<TextMeshProUGUI>();
        mainMenu = pauseMenu.transform.Find("Main Menu").GetChild(0).GetComponent<TextMeshProUGUI>();
        quit = pauseMenu.transform.Find("Quit").GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        textSize = resume.fontSize;
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();
    }

    public void Pause()
    {
        paused = !paused;

        if (paused)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            resume.fontSize = textSize;
            mainMenu.fontSize = textSize;
            quit.fontSize = textSize;
        }

        pauseMenu.SetActive(paused);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

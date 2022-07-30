using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    private void Start()
    {
        GameCenter.MenuPause = false;
        pausePanel.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PausePanelToggle();
        }
    }

    public void PausePanelToggle()
    {
        GameCenter.MenuPause = !GameCenter.MenuPause;
        pausePanel.SetActive(GameCenter.MenuPause);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}

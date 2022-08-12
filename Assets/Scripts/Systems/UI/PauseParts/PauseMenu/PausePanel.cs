using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private GameObject bacground;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject easterEggsPanel;
    [SerializeField] private GameObject donatesPanel;

    private void Start()
    {
        GameCenter.MenuPause = false;
        bacground.SetActive(false);
        SetActiveForPanels(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PausePanelToggle();
        }
    }

    public void EasterEggsPanelToggle()
    {
        easterEggsPanel.SetActive(!easterEggsPanel.activeSelf);
        pausePanel.SetActive(!easterEggsPanel.activeSelf);

        if(easterEggsPanel.activeSelf)
        {
            GetComponent<EasterEggSystem>().ShowAllItemButtons();
        }
    }
    public void DonatesPanelToggle()
    {
        donatesPanel.SetActive(!donatesPanel.activeSelf);
        pausePanel.SetActive(!donatesPanel.activeSelf);

        if (donatesPanel.activeSelf)
        {
            GetComponent<DonateHolderSystem>().ShowAllItemButtons();
        }
    }

    public void PausePanelToggle()
    {
        GameCenter.MenuPause = !GameCenter.MenuPause;
        bacground.SetActive(GameCenter.MenuPause);
        pausePanel.SetActive(GameCenter.MenuPause);

        if(!GameCenter.MenuPause)
        {
            easterEggsPanel.SetActive(false);
            donatesPanel.SetActive(false);
        }
    }
    public void Restart()
    {
        LevelProggress.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ExitGame()
    {
        Application.Quit();
    }


    private void SetActiveForPanels(bool value)
    {
        pausePanel.SetActive(value);
        easterEggsPanel.SetActive(value);
        donatesPanel.SetActive(value);
    }
}

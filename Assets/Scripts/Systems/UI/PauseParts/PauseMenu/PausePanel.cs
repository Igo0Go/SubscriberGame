using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private GameObject bacground;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject easterEggsPanel;
    [SerializeField] private GameObject donatesPanel;
    [SerializeField] private GameObject settingsPanel;

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

    public void PausePanelToggle()
    {
        GameCenter.MenuPause = !GameCenter.MenuPause;
        bacground.SetActive(GameCenter.MenuPause);
        pausePanel.SetActive(GameCenter.MenuPause);

        if (!GameCenter.MenuPause)
        {
            easterEggsPanel.SetActive(false);
            donatesPanel.SetActive(false);
            settingsPanel.SetActive(false);
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

    public void EasterEggsPanelToggle()
    {
        easterEggsPanel.SetActive(!easterEggsPanel.activeSelf);
        pausePanel.SetActive(!easterEggsPanel.activeSelf);

        if (easterEggsPanel.activeSelf)
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
    public void SettingsPanelToggle()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
        pausePanel.SetActive(!settingsPanel.activeSelf);
    }

    private void SetActiveForPanels(bool value)
    {
        pausePanel.SetActive(value);
        easterEggsPanel.SetActive(value);
        donatesPanel.SetActive(value);
        settingsPanel.SetActive(value);
    }
}

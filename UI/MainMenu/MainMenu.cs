using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private bool _isClosing;
    [SerializeField] private GameObject _panel;

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void OnEnable()
    {
        if (_isClosing)
            CancelEventSystem.AddEvent(SwapPanelActive, true);
    }

    private void OnDisable()
    {
        CancelEventSystem.RemoveEvent(SwapPanelActive);
    }

    private void SwapPanelActive()
    {
        _panel.SetActive(!_panel.activeSelf);
    }

}

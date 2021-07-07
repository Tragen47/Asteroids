using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static int Score;
    Button Continue;
    public Text ScoreText;

    GameObject MainCanvas;
    public GameObject Level;

    public GameObject Player;

    void Start()
    {
        MainCanvas = transform.Find("MainMenu").gameObject;
        Continue = transform.GetChild(0).Find("Continue").GetComponent<Button>();
        SetPause(true);
    }

    void DisableLevel()
    {
        foreach (Transform child in Level.transform)
            child.gameObject.SetActive(false);
        Level.SetActive(false);
    }

    void ResetLevel()
    {
        Score = 0;
        DisableLevel();
        Level.SetActive(true);

        Player.SetActive(true);
    }

    public void SetPause(bool paused)
    {
        Time.timeScale = paused ? 0 : 1;
        MainCanvas.SetActive(paused);
    }

    public void OnNewGameClick()
    {
        Continue.interactable = true;
        ResetLevel();
        SetPause(false);
    }

    public void OnControlsClick()
        => MainCanvas.GetComponent<Toggle>().isOn = !GetComponent<Toggle>().isOn;

    public void OnExitClick() => Application.Quit();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SetPause(true);
        if (!Player.activeInHierarchy)
        {
            Continue.interactable = false;
            DisableLevel();
            SetPause(true);
        }
        ScoreText.text = Score.ToString();
    }
}
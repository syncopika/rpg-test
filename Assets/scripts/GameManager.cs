using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// TODO: rename? more like a UI manager atm
public class GameManager : MonoBehaviour
{
    public Button yesButton;
    public Button noButton;
    public Text statusText;
    public Canvas canvas;

    Image canvasBackground;

    static GameManager gmInstance;

    IEnumerator showStatusTemporarily(int timeToWait, string newStatus)
    {
        setStatus(newStatus);
        yield return new WaitForSeconds(timeToWait);
        clearStatus();
    }

    void hideButtons()
    {
        canvasBackground.color = new Color32(0, 0, 0, 0);
        yesButton.GetComponent<CanvasGroup>().alpha = 0;
        noButton.GetComponent<CanvasGroup>().alpha = 0;
    }

    void showButtons()
    {
        canvasBackground.color = new Color32(255, 255, 255, 128);
        yesButton.GetComponent<CanvasGroup>().alpha = 1;
        noButton.GetComponent<CanvasGroup>().alpha = 1;
    }

    // TODO: use an enum for state
    public void updateStatusWithButtons(string newStatus, int state)
    {
        // ideally to be used when switching scenes
        setStatus(newStatus);
        showButtons();

        // TODO: maybe pass another arg to indicate what to do if yes button is pressed?
        if (state == 0) 
            clickYesToEnterCottage();

        if (state == 1) 
            clickYesToEnterWorldMap();
    }

    public void updateStatusTemporarily(int expiryTime, string newStatus)
    {
        // update status for x num seconds - useful for showing a temporary message
        StartCoroutine(showStatusTemporarily(expiryTime, newStatus));
    }

    private void setStatus(string newStatus)
    {
        statusText.text = newStatus;
    }

    private void clearStatus()
    {
        statusText.text = "";
    }

    void clickNoButton()
    {
        statusText.text = "";
        hideButtons();
    }

    void clickYesToEnterCottage()
    {
        yesButton.onClick.AddListener(enterCottage);
    }

    void enterCottage()
    {
        hideButtons();
        clearStatus();
        SceneManager.LoadScene("cottage-interior");
        yesButton.onClick.RemoveAllListeners();
    }

    void enterWorld()
    {
        hideButtons();
        clearStatus();
        SceneManager.LoadScene("main");
        yesButton.onClick.RemoveAllListeners();
    }

    void clickYesToEnterWorldMap()
    {
        yesButton.onClick.AddListener(enterWorld);
    }

    // Start is called before the first frame update
    void Start()
    {
        canvasBackground = canvas.GetComponent<Image>();

        hideButtons();

        noButton.onClick.AddListener(clickNoButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        if (gmInstance == null)
        {
            gmInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(gmInstance != this)
        {
            Destroy(gameObject);
        }
    }
}

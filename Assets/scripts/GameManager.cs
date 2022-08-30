using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Button yesButton;
    public Button noButton;
    public Text statusText;

    IEnumerator showStatusTemporarily(int timeToWait, string newStatus)
    {
        setStatus(newStatus);
        yield return new WaitForSeconds(timeToWait);
        clearStatus();
    }

    void hideButtons()
    {
        yesButton.GetComponent<CanvasGroup>().alpha = 0;
        noButton.GetComponent<CanvasGroup>().alpha = 0;
    }

    void showButtons()
    {
        yesButton.GetComponent<CanvasGroup>().alpha = 1;
        noButton.GetComponent<CanvasGroup>().alpha = 1;
    }

    public void updateStatusWithButtons(string newStatus)
    {
        // ideally to be used when switching scenes
        setStatus(newStatus);
        showButtons();

        // TODO: maybe pass another arg to indicate what to do if yes button is pressed?
        clickYesToEnterCottage();
    }

    public void updateStatusTemporarily(int expiryTime, string newStatus)
    {
        // update status for x num seconds
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
        yesButton.onClick.AddListener(delegate
        {
            enterCottage();
        });
    }

    void enterCottage()
    {
        SceneManager.LoadScene("cottage-interior");
    }

    // Start is called before the first frame update
    void Start()
    {
        hideButtons();

        noButton.onClick.AddListener(delegate
        {
            clickNoButton();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

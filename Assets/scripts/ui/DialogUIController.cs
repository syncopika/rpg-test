using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// attach this controller to a canvas that should support handling prompts (e.g. yes/no button with question text)
public class DialogUIController : MonoBehaviour
{
    public Button yesButton;
    public Button noButton;
    public Text statusText;
    public GameManager gameManager;

    Image canvasBackground;

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
        // assuming first-person so unlock cursor so player can click on buttons
        Cursor.lockState = CursorLockMode.None;

        // ideally to be used to display buttons (e.g. yes/no) to allow player to traverse scenes
        setStatus(newStatus);
        showButtons();

        yesButton.onClick.RemoveAllListeners();

        // TODO: maybe pass another arg to indicate what to do if yes button is pressed?
        if (state == 0)
        {
            clickYesToEnterCottage();
        }
        else if (state == 1)
        {
            clickYesToEnterWorldMap();
        }
        else if (state == 2)
        {
            clickYesToEnterTavern();
        }
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

        // assuming first-person so lock cursor again after no is selected
        Cursor.lockState = CursorLockMode.Locked;
    }

    void clickYesToEnterCottage()
    {
        yesButton.onClick.AddListener(enterCottage);
    }

    void clickYesToEnterTavern()
    {
        yesButton.onClick.AddListener(enterTavern);
    }

    void clickYesToEnterWorldMap()
    {
        yesButton.onClick.AddListener(enterWorld);
    }

    void enterCottage()
    {
        hideButtons();
        clearStatus();

        // save player position in the world
        GameObject player = GameObject.Find("low-poly-human-edit-rig2-edit");
        gameManager.setLastPlayerPos(player.transform.position);

        gameManager.crosshairs.enabled = false; // turn off crosshairs before changing scene b/c atm, the player defaults to 3rd person on scene load
        SceneManager.LoadScene("cottage-interior");
    }

    void enterTavern()
    {
        hideButtons();
        clearStatus();

        // save player position in the world
        GameObject player = GameObject.Find("low-poly-human-edit-rig2-edit");
        gameManager.setLastPlayerPos(player.transform.position);

        gameManager.crosshairs.enabled = false; // turn off crosshairs before changing scene b/c atm, the player defaults to 3rd person on scene load
        SceneManager.LoadScene("tavern-interior");
    }

    void enterWorld()
    {
        hideButtons();
        clearStatus();
        gameManager.crosshairs.enabled = false; // turn off crosshairs before changing scene b/c atm, the player defaults to 3rd person on scene load
        SceneManager.LoadScene("main");
    }

    // Start is called before the first frame update
    void Start()
    {
        canvasBackground = transform.GetComponent<Canvas>().GetComponent<Image>();

        hideButtons();

        noButton.onClick.AddListener(clickNoButton);

        //Debug.Log(transform.GetComponent<Canvas>());
        //Debug.Log(canvas.transform.Find("yes_button").transform.GetComponent<Button>());
    }

}

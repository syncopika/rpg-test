using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public Button yesButton;
    public Button noButton;
    public Text theText;
    public Image background;

    void hideCanvas()
    {
        yesButton.GetComponent<CanvasGroup>().alpha = 0;
        noButton.GetComponent<CanvasGroup>().alpha = 0;
        theText.enabled = false;
        background.color = new Color32(0, 0, 0, 0);
    }

    void hideButtons()
    {
        yesButton.GetComponent<CanvasGroup>().alpha = 0;
        noButton.GetComponent<CanvasGroup>().alpha = 0;
    }

    void showCanvas()
    {
        yesButton.GetComponent<CanvasGroup>().alpha = 1;
        noButton.GetComponent<CanvasGroup>().alpha = 1;
        background.color = new Color32(255, 255, 255, 255);
        theText.enabled = true;
    }

    public void updateDialog(string dialog, bool hideButtons=false)
    {
        theText.text = dialog;

        // TODO: set up yes and no buttons' callbacks
        showCanvas();

        if (hideButtons)
            this.hideButtons();
    }

    public void hideDialog()
    {
        hideCanvas();
    }

    // Start is called before the first frame update
    void Start()
    {
        hideCanvas();
    }

    // Update is called once per frame
    void Update()
    {
    }
}

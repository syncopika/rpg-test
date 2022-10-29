using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Canvas dialogUI; // show buttons fr entering/exiting scenes
    public Canvas inventoryUI;
    public Canvas crosshairs;
    public Canvas controlUI;

    private DialogUIController dialogUiController;
    private InventoryUIController inventoryUiController;

    static GameManager gmInstance;

    public void enterCottage()
    {
        dialogUI.enabled = true;
        Cursor.lockState = CursorLockMode.None;
        dialogUiController.updateStatusWithButtons("enter cottage?", 0);
    }

    public void exitCottage()
    {
        dialogUI.enabled = true;
        Cursor.lockState = CursorLockMode.None;
        dialogUiController.updateStatusWithButtons("exit cottage?", 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        dialogUiController = dialogUI.GetComponent<DialogUIController>();
        inventoryUiController = inventoryUI.GetComponent<InventoryUIController>();

        dialogUI.enabled = false;
        crosshairs.enabled = false;
        controlUI.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            inventoryUiController.toggleInventoryMenu();
        }

        if (Input.GetKeyDown("m"))
        {
            controlUI.enabled = !controlUI.enabled;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            crosshairs.enabled = !crosshairs.enabled;
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (crosshairs.enabled) crosshairs.enabled = false;
        }
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

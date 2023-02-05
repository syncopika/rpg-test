using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FarmerController : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject farm;

    private Quaternion initialRot;
    private Animator animator;

    private UnityAction yesAction;
    private UnityAction noAction;

    private bool questCompleted = false;
    private bool questAccepted = false;

    IEnumerator completedQuestWait()
    {
        yield return new WaitForSeconds(3);
        animator.SetBool("isIdle", false);
        transform.rotation = initialRot;
        gameManager.dialog.GetComponent<Dialog>().hideDialog();
    }

    IEnumerator waitAndHideDialog(int timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        gameManager.dialog.GetComponent<Dialog>().hideDialog();
    }

    public void completeQuest()
    {
        questCompleted = true;
        Debug.Log("quest completed!");
        gameManager.dialog.GetComponent<Dialog>().updateDialog("thanks, that's a great help!", true);
        StartCoroutine(waitAndHideDialog(2));
    }

    void acceptQuest()
    {
        questAccepted = true;
        Debug.Log("quest accepted!");
        gameManager.dialog.GetComponent<Dialog>().updateDialog("you're a lifesaver! can ya plant a tree right there if ya don't mind?", true);
        StartCoroutine(waitAndHideDialog(2));
    }

    void declineQuest()
    {
        gameManager.dialog.GetComponent<Dialog>().updateDialog("that's a darn shame...", true);
        StartCoroutine(waitAndHideDialog(2)); // wait 2 sec to close dialog
    }

    // Start is called before the first frame update
    void Start()
    {
        initialRot = transform.rotation;
        animator = transform.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name.Contains("low-poly-human") && !questAccepted && !questCompleted)
        {
            if (Camera.main.transform.GetComponent<PlayerCamera>().isInFirstPerson())
            {
                // first-person so unlock cursor so player can click on buttons
                Cursor.lockState = CursorLockMode.None;
            }

            gameManager.dialog.GetComponent<Dialog>().updateDialog("howdy there young feller, would you care to help me out with some farming?", false);

            yesAction = acceptQuest;
            noAction = declineQuest;
            gameManager.dialog.GetComponent<Dialog>().setYesButton(yesAction);
            gameManager.dialog.GetComponent<Dialog>().setNoButton(noAction);
        }
        else if(other.transform.name.Contains("low-poly-human") && questAccepted && questCompleted)
        {
            gameManager.dialog.GetComponent<Dialog>().updateDialog("howdy there young feller, thanks again for the help!", true);
            transform.LookAt(other.transform);
            animator.SetBool("isIdle", true);
            StartCoroutine(completedQuestWait());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.name.Contains("low-poly-human") && !questCompleted && !questAccepted)
        {
            transform.LookAt(other.transform);
            animator.SetBool("isIdle", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        animator.SetBool("isIdle", false);
        transform.rotation = initialRot;
        gameManager.dialog.GetComponent<Dialog>().hideDialog();

        if (Camera.main.transform.GetComponent<PlayerCamera>().isInFirstPerson())
        {
            // first-person so lock cursor again
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

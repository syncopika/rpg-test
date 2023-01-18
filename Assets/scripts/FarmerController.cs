using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerController : MonoBehaviour
{
    public GameManager gameManager;

    private Quaternion initialRot;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        initialRot = transform.rotation;
        animator = transform.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        gameManager.dialog.GetComponent<Dialog>().updateDialog("hey there", true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.name.Contains("low-poly-human"))
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

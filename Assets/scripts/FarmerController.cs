using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerController : MonoBehaviour
{
    private Quaternion initialRot;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        initialRot = transform.rotation;
        animator = transform.GetComponent<Animator>();
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

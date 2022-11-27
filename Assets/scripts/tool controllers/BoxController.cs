using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetComponent<Animator>();
        animator.SetBool("isOpen", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // close the box
        if (other.name.Equals("low-poly-human-edit-rig2-edit"))
        {
            animator.SetBool("isOpen", false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // open the box
        if (other.name.Equals("low-poly-human-edit-rig2-edit"))
        {
            animator.SetBool("isOpen", true);
        }
    }
}

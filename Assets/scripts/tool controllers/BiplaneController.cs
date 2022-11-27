using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiplaneController : MonoBehaviour
{
    bool playerInRange = false;

    public GameObject propeller;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name.Equals("low-poly-human-edit-rig2-edit"))
        {
            propeller.transform.Rotate(new Vector3(0, 0, 1) * 500f);
        }
    }
}

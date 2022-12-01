using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// helpful! https://www.youtube.com/watch?v=wPd61RvDDR0 (How to Get in and out of a Car in Unity (with car and character controllers))
public class BiplaneController : MonoBehaviour
{
    bool playerInRange = false;
    bool isFlying = false;

    public GameObject propeller;
    public GameObject player;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, getForward() * 20f, Color.blue);

        if (playerInRange && !isFlying)
        {
            if (Input.GetKeyUp("r"))
            {
                // get in plane
                // disable player controller
                Debug.Log("get in plane");
                player.GetComponent<Player>().enabled = false;
                isFlying = true;
                cam.GetComponent<BiplaneCameraController>().enabled = true;
                cam.GetComponent<PlayerCamera>().enabled = false;
            }
        }
        else if(isFlying)
        {
            if (Input.GetKeyUp("r"))
            {
                Debug.Log("get out of plane");
                // get out of plane 
                // enable player controller
                player.GetComponent<Player>().enabled = true;
                isFlying = false;

                // TODO: where to place player?
                cam.GetComponent<BiplaneCameraController>().enabled = false;
                cam.GetComponent<PlayerCamera>().enabled = true;
            }
            
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += getForward() * Time.deltaTime * 15f;
            }

            if (Input.GetKey(KeyCode.E))
            {
                // rotate right
                // TODO: need to check altitude before allowing
                transform.Rotate(new Vector3(-1, 0, 0) * 15 * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                // rotate left
                // TODO: need to check altitude before allowing
                transform.Rotate(new Vector3(1, 0, 0) * 15 * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.A))
            {
                // move left
                transform.Rotate(new Vector3(0, -1, 0) * 15 * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.D))
            {
                // move right
                transform.Rotate(new Vector3(0, 1, 0) * 15 * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                // move up
                transform.Rotate(new Vector3(0, 0, 1) * 25 * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                // move down
                // TODO: need to check altitude before allowing
                transform.Rotate(new Vector3(0, 0, -1) * 25 * Time.deltaTime);
            }

            propeller.transform.Rotate(new Vector3(0, 0, 1) * 500f);
        }
    }

    public Vector3 getForward()
    {
        //Vector3 forward = Vector3.Cross(Vector3.up, transform.right);
        //forward.Normalize(); // make unit vector

        // in this case our biplane's right vector is actually pointing left so we can rotate
        // check with:
        /*
        Debug.DrawRay(transform.position, getForward(), Color.red);
        Debug.DrawRay(transform.position, transform.up, Color.blue);
        Debug.DrawRay(transform.position, transform.right, Color.green);
        
        or let's just use the right vector since it's pointing 'forward' 
        */

        //return Quaternion.Euler(new Vector3(0, 90, 0))  * - forward;

        return Quaternion.Euler(new Vector3(-10, 0, 0)) * transform.right;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name.Equals("low-poly-human-edit-rig2-edit"))
        {
            propeller.transform.Rotate(new Vector3(0, 0, 1) * 500f);
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerInRange = false;
    }
}

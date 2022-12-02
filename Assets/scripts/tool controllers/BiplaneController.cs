using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// helpful! https://www.youtube.com/watch?v=wPd61RvDDR0 (How to Get in and out of a Car in Unity (with car and character controllers))
public class BiplaneController : MonoBehaviour
{
    bool playerInRange = false;
    bool isFlying = false;
    bool isLevel = false;

    Quaternion initialRotation = Quaternion.identity;
    Quaternion levelRotation = Quaternion.identity; // the biplane is leveled with the ground - should have this rotation when taking off and landing (it's pointing upwards slightly on the ground when static)

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
                //Debug.Log("get in plane");
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
                //Debug.Log("get out of plane");
                // get out of plane

                transform.GetComponent<Rigidbody>().useGravity = true;

                // enable player controller
                player.GetComponent<Player>().enabled = true;
                isFlying = false;

                // TODO: where to place player?
                cam.GetComponent<BiplaneCameraController>().enabled = false;
                cam.GetComponent<PlayerCamera>().enabled = true;
            }
            
            if (Input.GetKey(KeyCode.W))
            {
                //transform.GetComponent<Rigidbody>().AddForce(getForward() * 2000f);

                if (!isLevel)
                {
                    if(initialRotation == Quaternion.identity) initialRotation = transform.rotation;
                    if(levelRotation == Quaternion.identity) levelRotation = initialRotation * Quaternion.Euler(-15 * Vector3.forward);

                    transform.rotation = Quaternion.Lerp(transform.rotation, levelRotation, Time.deltaTime * 12f);
                    transform.position += getForward() * Time.deltaTime * 12f;
                }
                
                if (Quaternion.Angle(transform.rotation, levelRotation) <= 1.2f)
                {
                    //Debug.Log("level rotation reached");
                    isLevel = true;
                    initialRotation = Quaternion.identity;
                    levelRotation = Quaternion.identity;
                }

                if (isLevel)
                {
                    // we've reached the right rotation so we can be airborne
                    transform.GetComponent<Rigidbody>().useGravity = false;
                    transform.position += getForward() * Time.deltaTime * 15f;
                }
            }
            else
            {
                transform.GetComponent<Rigidbody>().useGravity = true;
                isLevel = false;
                initialRotation = Quaternion.identity;
                levelRotation = Quaternion.identity;
            }

            if (Input.GetKey(KeyCode.E))
            {
                // rotate right
                // TODO: need to check altitude before allowing
                transform.Rotate(new Vector3(-1, 0, 0) * 20 * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                // rotate left
                // TODO: need to check altitude before allowing
                transform.Rotate(new Vector3(1, 0, 0) * 20 * Time.deltaTime);
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

            propeller.transform.Rotate(new Vector3(0, 0, 1) * 1000f);
        }
    }

    public Vector3 getForward()
    {
        //return Quaternion.Euler(new Vector3(-10, 0, 0)) * transform.right;
        return transform.right;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera2 : MonoBehaviour
{
    public GameObject player;
    public GameObject gameManager;
    public GameObject rotationBone;

    private Vector3 lastPos;
    private bool inFirstPerson;

    // for mouse movement
    private float sensitivity = 100f;
    private float rotY = 0.0f;
    private float rotX = 0.0f;

    void shootRay()
    {
        RaycastHit hit;
        Ray r = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(r, out hit))
        {
            if (hit.collider != null)
            {
                Debug.Log("ray hit: " + hit.transform);
                if (hit.transform.name.Contains("cottage"))
                {
                    // enter cottage scene
                    gameManager.GetComponent<GameManager>().updateStatusWithButtons("enter cottage?");
                    //Debug.Log("enter cottage?");
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        inFirstPerson = false;
        Vector3 playerForward = player.GetComponent<Player>().getForward();
        Vector3 newVec = 9f * playerForward;
        newVec.y = -4f;
        transform.position = player.transform.position - newVec;

        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            inFirstPerson = !inFirstPerson;
        }

        if (Input.GetKey("q") || Input.GetKey("e"))
        {
            transform.rotation = player.transform.rotation;
            Vector3 rot = transform.localRotation.eulerAngles;
            rotY = rot.y;
            rotX = rot.x;
        }

        Vector3 playerForward = player.GetComponent<Player>().getForward();

        if (inFirstPerson)
        {
            Vector3 playerPos = player.transform.position;
            transform.position = new Vector3(playerPos.x, playerPos.y + 4.5f, playerPos.z);

            // allow look around with mouse when scope is on
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = -Input.GetAxis("Mouse Y"); // negative so no inversion of axis to mouse move

            if (mouseX != 0f || mouseY != 0f)
            {
                rotY += (mouseX * sensitivity * Time.deltaTime);
                rotX += (mouseY * sensitivity * Time.deltaTime);

                // restrict downward view to 15 degrees
                rotX = Mathf.Clamp(rotX, -90.0f, 90.0f);

                // also rotY - TODO: this doesn't seem to be working atm?
                rotY = Mathf.Clamp(rotY, rotY - 5f, rotY + 5f);

                // use quaternion to get new rotation
                Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);

                // also rotate neck bone so head is rotated in the same way
                transform.rotation = localRotation;

                rotationBone.transform.rotation = localRotation;

                player.transform.rotation = Quaternion.Euler(player.transform.rotation.x, rotY, 0.0f);
            }

            if (Input.GetMouseButtonDown(0))
            {
                shootRay();
            }
        }
        else
        {
            transform.rotation = player.transform.rotation;

            Vector3 newVec = 6f * playerForward;
            newVec.y = -3f;

            if (lastPos != null)
            {
                transform.position = Vector3.Lerp(player.transform.position - newVec, lastPos, 0.6f);
            }
            else
            {
                transform.position = player.transform.position - newVec;
            }

            lastPos = transform.position;
        }

    }
}

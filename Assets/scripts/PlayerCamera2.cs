using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera2 : MonoBehaviour
{
    public GameObject player;
    //public GameObject gameManager;
    public GameObject rotationBone;

    GameObject gameManager;

    private Vector3 lastPos;
    private bool inFirstPerson;

    // for mouse movement
    private float sensitivity = 100f;
    private float rotY = 0.0f;
    private float rotX = 0.0f;

    float distFromCamera = 8.2f;

    Vector3 cameraPosCorrection = Vector3.zero;
    string playerCharacterModelIdentifier = "human";

    bool rotationChanged = false;

    void shootRay()
    {
        RaycastHit hit;
        Ray r = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(r, out hit))
        {
            if (hit.collider != null)
            {
                Debug.Log("ray hit: " + hit.transform);
                if (hit.transform.name.ToLower().Contains("door"))
                {
                    // leave cottage scene
                    gameManager.GetComponent<GameManager>().updateStatusWithButtons("leave cottage?", 1);
                }
            }
        }
    }

    Vector3 getNewCameraPos()
    {
        Vector3 playerPos = new Vector3(
            player.transform.position.x, transform.position.y, player.transform.position.z
        );
        //playerPos -= player.GetComponent<Player>().getForward() * 0.5f; // add some buffer room behind the player

        Vector3 playerForward = player.GetComponent<Player>().getForward();
        Vector3 newVec = distFromCamera * playerForward;
        newVec.y = -5f;

        if(cameraPosCorrection != Vector3.zero) // if camera needs correction
        {
            // if we're trying to figure out the pos of camera due to obstruction
            RaycastHit hit;
            if (Physics.Linecast(transform.position, playerPos, out hit))
            {
                if (hit.transform && !hit.transform.name.Contains(playerCharacterModelIdentifier))
                {
                    //Debug.Log("trying to correct cam distance");
                    // keep correcting the camera pos until no more boundary obstruction
                    cameraPosCorrection += (playerForward * 0.1f);

                    return cameraPosCorrection;
                }
            }

            // no more obstruction! yay
            //Debug.Log("no more obstruction");
            cameraPosCorrection = Vector3.zero;

            return transform.position; // return current pos
            
        }
        else
        {
            // TODO: the camera still moves out of bounds sometimes
            // e.g. when rotating when player is at a boundary
            // sometimes when player is at a boundary and steps back far enough, the camera bounces to try to correct but never resolves

            // check if current cam position is OK or needs to be corrected
            // recalculate every time rotation changes? e.g. if rotation changes, set cameraPosCorrection to newDesiredPos
            Vector3 newDesiredPos = player.transform.position - newVec;

            if (rotationChanged)
            {
                cameraPosCorrection = newDesiredPos;
                rotationChanged = false;
                //Debug.Log("rotation changed");
                return cameraPosCorrection;
            }

            // if we can get to newDesiredPos, then let's do that
            RaycastHit hit;
            if (Physics.Linecast(newDesiredPos, playerPos, out hit) && hit.transform.name.Contains(playerCharacterModelIdentifier))
            {
                // no obstructions, desired pos OK
                //Debug.Log("no obstructions found");
                cameraPosCorrection = Vector3.zero;
                return newDesiredPos;
            }
            else if (Physics.Linecast(newDesiredPos, playerPos, out hit) && !hit.transform.name.Contains(playerCharacterModelIdentifier))
            {
                // use hit.transform.position as a starting point but make sure the y axis value matches the camera
                Vector3 startPos = new Vector3(hit.transform.position.x, transform.position.y, hit.transform.position.z);
                cameraPosCorrection = startPos; // start at obstruction pos and correct pos as needed
                return cameraPosCorrection;
            }
            else if (Physics.Linecast(transform.position, playerPos, out hit) && hit.transform.name.Contains(playerCharacterModelIdentifier))
            {
                // no obstruction from current cam pos to the player - we've reached a satisfactory distance.
                // stop correcting camera pos
                cameraPosCorrection = Vector3.zero;
                return transform.position;
            }
            else if (Physics.Linecast(transform.position, playerPos, out hit) && !hit.transform.name.Contains(playerCharacterModelIdentifier))
            {
                //Debug.Log("obstruction found");
                Vector3 startPos = new Vector3(hit.transform.position.x, transform.position.y, hit.transform.position.z);
                cameraPosCorrection = startPos;
                return cameraPosCorrection;
            }
            else
            {
                cameraPosCorrection = Vector3.zero;
                return transform.position;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        inFirstPerson = false;
        Vector3 playerForward = player.GetComponent<Player>().getForward();
        Vector3 newVec = distFromCamera * playerForward;
        newVec.y = -5f;
        transform.position = player.transform.position - newVec;

        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;

        // gamemanager is not native to this scene but comes from DontDestroyOnLoad so it should be available here
        gameManager = GameObject.Find("GameManager");
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
            rotationChanged = true;
        }

        Vector3 playerForward = player.GetComponent<Player>().getForward();

        if (inFirstPerson)
        {
            Vector3 playerPos = player.transform.position;
            transform.position = new Vector3(playerPos.x, playerPos.y + 5.5f, playerPos.z);

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
            Vector3 newPos = getNewCameraPos();

            if (lastPos != null)
                transform.position = Vector3.Lerp(newPos, lastPos, 0.8f);
            else
                transform.position = newPos;

            lastPos = transform.position;
        }

        //Debug.DrawLine(transform.position, player.transform.position, Color.blue);
        //Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
    }
}

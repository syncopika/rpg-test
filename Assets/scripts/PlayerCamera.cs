using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject player;
    public GameObject gameManager;
    public GameObject rotationBone; // torso
    public GameObject headBone; // head

    private Vector3 lastPos;
    private bool inFirstPerson;
    private bool inThirdPersonFront;

    // for mouse movement
    private float sensitivity = 100f;
    private float rotY = 0.0f;
    private float rotX = 0.0f;

    private Quaternion rotationBoneRotation; // use this to keep track of current rotation to set to (e.g. when aiming in a certain direction) since we manually change the rotation to override changes from animation

    //float distFromCamera = 8.2f;
    //Vector3 cameraPosCorrection = Vector3.zero;
    //string playerCharacterModelIdentifier = "human";
    //bool rotationChanged = false;

    public void toggleFirstPerson()
    {
        inFirstPerson = !inFirstPerson;

        if (inFirstPerson && inThirdPersonFront)
        {
            transform.rotation = player.transform.rotation; // if going from thirdpersonfront back to first person, correct the rotation
        }

        inThirdPersonFront = false;
    }

    public void toggleInThirdPersonFront()
    {
        inThirdPersonFront = !inThirdPersonFront;
        inFirstPerson = false;
    }

    public void setToPlayerRotation()
    {
        transform.rotation = player.transform.rotation;
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }

    void shootRay()
    {
        RaycastHit hit;
        Ray r = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(r, out hit))
        {
            if (hit.collider != null)
            {
                Debug.Log("ray hit: " + hit.transform);
                if (hit.transform.name.Contains("EnterCottage"))
                {
                    // enter cottage scene
                    gameManager.GetComponent<GameManager>().enterCottage();
                } 
                else if (hit.transform.name.Contains("ExitCottage"))
                {
                    gameManager.GetComponent<GameManager>().exitCottage();
                }
            }
        }
    }

    /* not actually needed anymore but leaving here just in case :)
    Vector3 getNewCameraPos()
    {
        Vector3 playerPos = new Vector3(
            player.transform.position.x, transform.position.y, player.transform.position.z
        );

        Vector3 playerForward = player.GetComponent<Player>().getForward();
        Vector3 newVec = distFromCamera * playerForward;
        newVec.y = -5f;

        if (cameraPosCorrection != Vector3.zero) // if camera needs correction
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
    */

    // Start is called before the first frame update
    void Start()
    {
        inFirstPerson = false;
        inThirdPersonFront = false;

        rotationBoneRotation = Quaternion.identity;

        Vector3 playerForward = player.GetComponent<Player>().getForward();
        Vector3 newVec = 9f * playerForward;
        newVec.y = -4f;
        transform.position = player.transform.position - newVec;

        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;

        // get the gamemanager when coming back to this scene.
        gameManager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerForward = player.GetComponent<Player>().getForward();

        if (inThirdPersonFront)
        {
            transform.rotation = player.transform.rotation * Quaternion.Euler(0, 180f, 0); // rotate 180 deg to face player

            Vector3 newVec = 9f * playerForward;
            newVec.y = 4f;

            if (lastPos != null)
                transform.position = Vector3.Lerp(player.transform.position + newVec, lastPos, 0.6f);
            else
                transform.position = player.transform.position + newVec;

            lastPos = transform.position;
        }
        else if (!inFirstPerson)
        {
            transform.rotation = player.transform.rotation;

            Vector3 newVec = 9f * playerForward;
            newVec.y = -4f;

            // by default, we'd like to place the camera at newPos
            Vector3 newPos = player.transform.position - newVec;

            // but there might be an obstacle in between the player and the camera so check first
            // note the raycast is from player position to camera. seems to work better than going from camera pos to player pos :/.
            Vector3 currPlayerPos = new Vector3(player.transform.position.x, newPos.y, player.transform.position.z);

            RaycastHit hit;
            if (lastPos != null && Physics.Raycast(currPlayerPos, -playerForward, out hit, Vector3.Distance(currPlayerPos, newPos)))
            {
                if (!hit.transform.name.Contains("human"))
                {
                    Vector3 correctedNewPos = hit.point + playerForward * 3f;
                    newPos = correctedNewPos;
                }
            }

            if (lastPos != null)
                transform.position = Vector3.Lerp(lastPos, newPos, 1f);
            else
                transform.position = newPos;

            lastPos = transform.position;
        }

    }

    private void LateUpdate()
    {
        // need to do this stuff in lateupdate because we're rotating a bone in the player's armature
        // https://forum.unity.com/threads/head-bone-wont-rotate-via-script.442351/
        if (inFirstPerson)
        {
            // TODO? in first person mode, make the head invisible - but this would require redoing the model to separate the head from the body so they're separate meshes

            // allow look around with mouse
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = -Input.GetAxis("Mouse Y"); // negative so no inversion of axis to mouse move

            if (mouseX != 0f || mouseY != 0f)
            {
                rotY += (mouseX * sensitivity * Time.deltaTime);
                rotX += (mouseY * sensitivity * Time.deltaTime);

                // restrict downward view to 15 degrees
                rotX = Mathf.Clamp(rotX, -90.0f, 90.0f);

                // use quaternion to get new rotation
                Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
                transform.rotation = localRotation; // camera rotation

                rotationBone.transform.rotation = localRotation; // set character torso rotation to match camera
                headBone.transform.rotation = localRotation;

                rotationBoneRotation = localRotation;

                player.transform.rotation = Quaternion.Euler(player.transform.rotation.x, rotY, 0.0f); // this rotates the whole player about the y axis (left/right)
            }
            else
            {
                if (rotationBoneRotation != Quaternion.identity)
                {
                    rotationBone.transform.rotation = rotationBoneRotation;
                    headBone.transform.rotation = rotationBoneRotation;
                }
            }

            Vector3 headPos = headBone.transform.position;
            Vector3 newCamPos = headPos + (headBone.transform.forward * 0.5f);
            newCamPos.y += 0.5f;

            transform.position = newCamPos; //new Vector3(headPos.x, headPos.y + .5f, headPos.z);

            if (Input.GetMouseButtonDown(0))
                shootRay();
        }

    }
}

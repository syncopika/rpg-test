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

    public void toggleFirstPerson()
    {
        inFirstPerson = !inFirstPerson;

        if (inFirstPerson && inThirdPersonFront)
        {
            transform.rotation = player.transform.rotation; // if going from thirdpersonfront back to first person, correct the rotation
        }

        if (inFirstPerson)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
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
        GameManager gm = gameManager.GetComponent<GameManager>();

        if (Physics.Raycast(r, out hit))
        {
            if (hit.collider != null)
            {
                Debug.Log("ray hit: " + hit.transform);
                if (hit.transform.name.Contains("EnterCottage"))
                {
                    gm.enterCottage();
                } 
                else if (hit.transform.name.Contains("ExitCottage"))
                {
                    gm.exitCottage();
                }
                else if (hit.transform.name.Contains("EnterTavern"))
                {
                    Debug.Log("TODO: enter tavern");
                }
                else if (hit.transform.name.Contains("book-animated"))
                {
                    // opening/closing a book (cottage interior scene)
                    Animator bookAnimator = hit.transform.GetComponent<Animator>();
                    bookAnimator.SetBool("isOpen", !bookAnimator.GetBool("isOpen"));
                }
                else if (hit.transform.name.Contains("topBox"))
                {
                    // opening/closing a box (cottage interior scene)
                    Animator boxAnimator = hit.transform.GetComponent<Animator>();
                    boxAnimator.SetBool("isOpen", !boxAnimator.GetBool("isOpen"));
                }
            }
        }
    }

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

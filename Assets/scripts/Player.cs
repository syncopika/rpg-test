using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// kinematic, non-physics
public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;

    private string areaInside = ""; // TODO: use an enum for specific areas?
    private GameObject areaInsideObj;

    // TODO: figure out a better way to do this but doing this now for testing
    public GameObject bulletEmitter;
    public GameObject bulletPrefab;

    // for detecting distance between player model and terrain/ground
    public GameObject baselineObj;

    private InventoryManager inventory;

    private bool isInFirstPerson;

    public Vector3 getForward()
    {
        Vector3 forward = Vector3.Cross(Vector3.up, transform.right);
        forward.Normalize(); // make unit vector
        return -forward;
    }
    private void OnTriggerEnter(Collider other)
    {
        // check if in a zone (e.g. garden or near water for fishing)
        Debug.Log(other);
        areaInside = other.name;
        areaInsideObj = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        areaInside = "";
        areaInsideObj = null;
    }

    private bool checkIfCanMove(Vector3 dir)
    {
        RaycastHit hit;
        if(rb.SweepTest(dir, out hit, 1.0f))
        {
            if (hit.transform.tag.Equals("obstacle"))
            {
                return false;
            }
        }

        return true;
    }

    private bool isInGarden()
    {
        return areaInside.Contains("garden-area");
    }

    private bool isNextToWater()
    {
        return areaInside.Equals("WaterProDaytime");
    }

    private void fireWeapon()
    {
        // https://forum.unity.com/threads/firing-a-bullet-from-a-gun.701327/
        Vector3 forward = Vector3.Cross(bulletEmitter.transform.up, bulletEmitter.transform.forward); // bulletEmitter.transform.forward isn't really the forward we want
        forward.Normalize();

        //Debug.DrawRay(bulletEmitter.transform.position, forward * 10, Color.green);

        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = bulletEmitter.transform.position;
        bullet.transform.rotation = bulletEmitter.transform.rotation;

        bullet.GetComponent<Rigidbody>().AddForce(forward * 20f, ForceMode.Impulse);
    }

    private void adjustVerticalHeightBasedOnTerrain()
    {
        RaycastHit hit;

        // avoid water, which is layer 4
        // https://docs.unity3d.com/Manual/use-layers.html
        int waterLayer = 1 << 4;

        waterLayer = ~waterLayer;

        if(Physics.Raycast(baselineObj.transform.position, -Vector3.up, out hit, 15.0f, waterLayer))
        {
            if (hit.transform.name.Equals("Terrain"))
            {
                //Debug.Log("distance to terrain: " + hit.distance);
                //Debug.Log("hit pos y: " + hit.point.y + ", curr pos y: " + transform.position.y);
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            }
        }

    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        inventory = GetComponent<InventoryManager>();

        isInFirstPerson = false;
    }

    void Update()
    {
        adjustVerticalHeightBasedOnTerrain();

        // all the key presses in this if/else if block
        // are associated with actions that shouldn't be activated
        // simultaneously with another
        if (Input.GetKeyDown("w"))
        {
            anim.SetBool("isIdle", false);
            anim.SetBool("isWalk", true);
            anim.SetFloat("direction", 1f);
        }
        else if (Input.GetKeyUp("w"))
        {
            anim.SetBool("isIdle", true);
            anim.SetBool("isWalk", false);
        }
        else if (Input.GetKeyDown("a"))
        {
            anim.SetBool("isIdle", false);
            anim.SetBool("isWalkSide", true);
            anim.SetFloat("direction", -1f);
        }
        else if (Input.GetKeyUp("a"))
        {
            anim.SetBool("isIdle", true);
            anim.SetBool("isWalkSide", false);
        }
        else if (Input.GetKeyDown("d"))
        {
            anim.SetBool("isIdle", false);
            anim.SetBool("isWalkSide", true);
            anim.SetFloat("direction", 1f);
        }
        else if (Input.GetKeyUp("d"))
        {
            anim.SetBool("isIdle", true);
            anim.SetBool("isWalkSide", false);
        }
        else if (Input.GetKeyDown("s"))
        {
            // walk backwards 
            // make sure the walk animation is set to loop (animation should have "Loop Time" and "Loop Pose" checked)
            // however, if we have a different animation for walking backwards, we will need to create a new state in the controller
            // https://gamedev.stackexchange.com/questions/116380/how-to-play-an-animation-backwards-in-unity
            anim.SetBool("isIdle", false);
            anim.SetBool("isWalk", true);
            anim.SetFloat("direction", -1f); // used as a multiplier to speed to make the animation run backwards
        }
        else if (Input.GetKeyUp("s"))
        {
            anim.SetBool("isIdle", true);
            anim.SetBool("isWalk", false);
        }

        // TODO: left/right lean for q and e when armed?
        // let player turn left and right
        if (!isInFirstPerson && Input.GetKey("q") && checkIfCanMove(transform.rotation * (-Vector3.up * Time.deltaTime * 300f)))
        {
            transform.Rotate(-Vector3.up * Time.deltaTime * 300f); // rotate counterclockwise about the Y axis
            Camera.main.transform.GetComponent<PlayerCamera>().setToPlayerRotation();
        }
        else if (!isInFirstPerson && Input.GetKey("e") && checkIfCanMove(transform.rotation * (Vector3.up * Time.deltaTime * 300f)))
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 300f);
            Camera.main.transform.GetComponent<PlayerCamera>().setToPlayerRotation();
        }

        // handle movement (no root motion)
        if (Input.GetKey("w") && checkIfCanMove(transform.forward))
        {
            if (anim.GetBool("isRun"))
                transform.position += transform.forward * Time.deltaTime * 9f;
            else
                transform.position += transform.forward * Time.deltaTime * 5f;
        }
        else if (Input.GetKey("s") && checkIfCanMove(-transform.forward))
        {
            if (anim.GetBool("isRun"))
                transform.position -= transform.forward * Time.deltaTime * 9f;
            else
                transform.position -= transform.forward * Time.deltaTime * 5f;
        }
        else if (Input.GetKey("a") && checkIfCanMove(-transform.right))
        {
            transform.position -= transform.right * Time.deltaTime * 5f;
        }
        else if (Input.GetKey("d") && checkIfCanMove(transform.right))
        {
            transform.position += transform.right * Time.deltaTime * 5f;
        }

        if (Input.GetKeyDown("j") && !anim.GetBool("isArmed"))
        {
            // allow root motion for jumping? https://answers.unity.com/questions/766225/turn-off-root-motion-for-a-specific-animation.html
            anim.SetTrigger("jump");
        }

        if (anim.GetBool("isFishing"))
        {
            // TODO: get fish
        }

        if (anim.GetBool("isArmed") && Input.GetMouseButtonDown(0))
        {
            // fire weapon
            fireWeapon();
        }

        if (Input.GetKeyUp("u") && anim.GetBool("isIdle"))
        {
            if (anim.GetBool("isArmed"))
                anim.SetBool("isArmed", false);
            else
                anim.SetBool("isArmed", true);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
            anim.SetBool("isRun", true);
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            anim.SetBool("isRun", false);

        // fishing
        if (isNextToWater() && inventory.currentlyEquippedObjName().Equals("fishingPole"))
        {
            if (Input.GetKeyDown("f") && anim.GetBool("isIdle"))
            {
                anim.SetBool("isFishing", true);
                inventory.currentlyEquippedObj().transform.GetComponent<FishingPoleController>().toggleFishingLine();
            }
            else if (Input.GetKeyUp("f"))
            {
                anim.SetBool("isFishing", false);

                // TODO? wait until animation is finished before disabling line renderer for fishing line
                // https://answers.unity.com/questions/426266/how-to-wait-until-an-animation-is-finished.html
                inventory.currentlyEquippedObj().transform.GetComponent<FishingPoleController>().toggleFishingLine();
            }
        }

        if (isInGarden() && !anim.GetBool("isArmed"))
        {
            // watering plants
            if (Input.GetKeyDown("c") && inventory.currentlyEquippedObjName().Equals("wateringCan"))
                anim.SetBool("isWateringPlants", true);
            else if (Input.GetKeyUp("c"))
                anim.SetBool("isWateringPlants", false);

            // planting seeds
            else if (Input.GetKeyDown("p"))
            {
                anim.SetBool("isPlantingSeeds", true);
                areaInsideObj.GetComponent<GardenManager>().plantTree(getForward());
            }
            else if (Input.GetKeyUp("p"))
            {
                anim.SetBool("isPlantingSeeds", false);
            }

            // shoveling - only allow if shovel equipped
            else if (Input.GetKeyDown("v") && inventory.currentlyEquippedObjName().Equals("shovel"))
            {
                anim.SetBool("isShoveling", true);
                areaInsideObj.GetComponent<GardenManager>().shovel();
            }
            else if (Input.GetKeyUp("v") && inventory.currentlyEquippedObjName().Equals("shovel"))
            {
                anim.SetBool("isShoveling", false);
            }

            // raking - only allow if rake equipped
            else if (Input.GetKeyDown("y") && inventory.currentlyEquippedObjName().Equals("rake"))
            {
                anim.SetBool("isRaking", true);
                areaInsideObj.GetComponent<GardenManager>().rake();
            }
            else if (Input.GetKeyUp("y") && inventory.currentlyEquippedObjName().Equals("rake"))
            {
                anim.SetBool("isRaking", false);
            }
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            isInFirstPerson = !isInFirstPerson;
            Camera.main.transform.GetComponent<PlayerCamera>().toggleFirstPerson();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            isInFirstPerson = false;
            Camera.main.transform.GetComponent<PlayerCamera>().toggleInThirdPersonFront();
        }

            // https://stackoverflow.com/questions/66644719/how-to-use-transform-forward-that-only-acknowledges-one-axis-of-rotation
            //Vector3 forward = Vector3.Cross(Vector3.up, transform.right) * 10;
            //Debug.DrawRay(transform.position, -forward, Color.green);
        }

}

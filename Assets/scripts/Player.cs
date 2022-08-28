﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameObject treePrefab;

    private Rigidbody rb;
    private Animator anim;

    private string areaInside = ""; // TODO: use an enum?

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
    }

    private void OnTriggerExit(Collider other)
    {
        areaInside = "";
    }

    private bool isInGarden()
    {
        return areaInside.Equals("garden-area");
    }

    private bool isNextToWater()
    {
        return areaInside.Equals("WaterProDaytime");
    }

    IEnumerator plantSeed()
    {
        // TODO: just putting a little tree to demonstrate but eventually have a little mound?
        yield return new WaitForSeconds(3);

        Vector3 treePos = transform.position + 1.2f*getForward();
        Instantiate(treePrefab, treePos, Quaternion.AngleAxis(90, Vector3.left));
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // let player turn left and right
        if (Input.GetKey("q"))
        {
            transform.Rotate(-Vector3.up * Time.deltaTime * 300f); // rotate counterclockwise about the Y axis
        }
        else if (Input.GetKey("e"))
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 300f);
        }

        // handle animation toggling
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
            // isWalkSide
            anim.SetBool("isIdle", false);
            anim.SetBool("isWalkSide", true);
            anim.SetFloat("direction", -1f);
        }
        else if (Input.GetKeyUp("a"))
        {
            // back to idle
            anim.SetBool("isIdle", true);
            anim.SetBool("isWalkSide", false);
        }
        else if (Input.GetKeyDown("d"))
        {
            // isWalkSide = true
            anim.SetBool("isIdle", false);
            anim.SetBool("isWalkSide", true);
            anim.SetFloat("direction", 1f);
        }
        else if (Input.GetKeyUp("d"))
        {
            // back to idle
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
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // use shift to toggle running
            anim.SetBool("isRun", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            anim.SetBool("isRun", false);
        }

        // fishing - TODO: only allow if fishing pole equipped
        if (isNextToWater())
        {
            if (Input.GetKeyDown("f"))
            {
                anim.SetBool("isFishing", true);
            }
            else if (Input.GetKeyUp("f"))
            {
                anim.SetBool("isFishing", false);
            }
        }

        if (isInGarden())
        {
            // watering plants - TODO: only allow if watering can equipped
            if (Input.GetKeyDown("c"))
            {
                anim.SetBool("isWateringPlants", true);
            }
            else if (Input.GetKeyUp("c"))
            {
                anim.SetBool("isWateringPlants", false);
            }

            // planting seeds
            else if (Input.GetKeyDown("p"))
            {
                anim.SetBool("isPlantingSeeds", true);
                StartCoroutine(plantSeed());
            }
            else if (Input.GetKeyUp("p"))
            {
                anim.SetBool("isPlantingSeeds", false);
            }

            // shoveling - TODO: only allow if shovel equipped
            else if (Input.GetKeyDown("v"))
            {
                anim.SetBool("isShoveling", true);
            }
            else if (Input.GetKeyUp("v"))
            {
                anim.SetBool("isShoveling", false);
            }

            // raking - TODO: only allow if rake equipped
            else if (Input.GetKeyDown("y"))
            {
                anim.SetBool("isRaking", true);
            }
            else if (Input.GetKeyUp("y"))
            {
                anim.SetBool("isRaking", false);
            }
        }

        // handle movement (no root motion)
        if (Input.GetKey("w"))
        {
            if (anim.GetBool("isRun"))
            {
                transform.position += transform.forward * Time.deltaTime * 9f;
            }
            else 
            {
                transform.position += transform.forward * Time.deltaTime * 5f;
            }
        }else if (Input.GetKey("s"))
        {
            if (anim.GetBool("isRun"))
            {
                transform.position -= transform.forward * Time.deltaTime * 9f;
            }
            else
            {
                transform.position -= transform.forward * Time.deltaTime * 5f;
            }
        }
        else if (Input.GetKey("a"))
        {
            transform.position -= transform.right * Time.deltaTime * 5f;
        }
        else if (Input.GetKey("d"))
        {
            transform.position += transform.right * Time.deltaTime * 5f;
        }

        if (anim.GetBool("isFishing"))
        {
            // TODO: get fish
        }

        //transform.Translate(Input.GetAxis("Horizontal") * 0.1f, 0, Input.GetAxis("Vertical") * 0.1f);

        // https://stackoverflow.com/questions/66644719/how-to-use-transform-forward-that-only-acknowledges-one-axis-of-rotation
        //Vector3 forward = Vector3.Cross(Vector3.up, transform.right) * 10;
        //Debug.DrawRay(transform.position, -forward, Color.green);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingPoleController : MonoBehaviour
{
    public Transform floater;

    bool hasBite;
    Vector3 floaterPositionOnBite;

    Transform hookedFish = null;

    LineRenderer fishingLine;

    Transform owner; // e.g. the player

    public void toggleFishingLine()
    {
        fishingLine.enabled = !fishingLine.enabled;
    }

    public void registerHookedFish(Transform fish)
    {
        hookedFish = fish;
    }

    public void resetFishingPole()
    {
        floater.position = floaterPositionOnBite;
        fishingLine.enabled = false;

        if(owner) owner.GetComponent<Player>().setText("");

        if (hasBite)
        {
            transform.GetComponent<RopeControllerRealisticNoSpring>().toggleHasBite();
            transform.GetComponent<RopeControllerRealisticNoSpring>().setFloaterPos(Vector3.zero);
            hasBite = false;
        }

        if (hookedFish)
        {
            hookedFish.GetComponent<FishController>().resetFish();
            hookedFish = null;
        }
    }

    public void bite()
    {
        transform.GetComponent<RopeControllerRealisticNoSpring>().toggleHasBite();
        hasBite = true;
        floaterPositionOnBite = floater.position;
    }

    public void setOwner(Transform owner)
    {
        this.owner = owner;
    }

    void Start()
    {
        fishingLine = transform.GetComponent<LineRenderer>();
        fishingLine.enabled = false;
        hasBite = false;
    }

    private void Update()
    {
        if (hasBite)
        {
            float oscillationSpeed = 8.0f;
            transform.GetComponent<RopeControllerRealisticNoSpring>().setFloaterPos(new Vector3(floater.position.x, floaterPositionOnBite.y + 2.0f*Mathf.Cos(oscillationSpeed * Time.time), floater.position.z));
            owner.GetComponent<Player>().setText("press space to catch fish");

            // catch on spacebar press
            // TODO: make a more complex fishing system?
            if (Input.GetKeyUp("space"))
            {
                Debug.Log("fish caught");
                hasBite = false;
                hookedFish.GetComponent<FishController>().isCaught();

                if (owner.name.Contains("low-poly-human-edit-rig2-edit"))
                {
                    owner.GetComponent<Player>().getInventory().addToInventory("fish", hookedFish.gameObject);
                    Debug.Log(owner.GetComponent<Player>().getInventory().getCurrentInventory());
                    owner.GetComponent<Player>().setText("");

                    // adjust line renderer so it's not oscillating anymore
                    transform.GetComponent<RopeControllerRealisticNoSpring>().setFloaterPos(new Vector3(floater.position.x, floater.position.y, floater.position.z));
                    transform.GetComponent<RopeControllerRealisticNoSpring>().toggleHasBite();
                }
            }
        }
    }

}

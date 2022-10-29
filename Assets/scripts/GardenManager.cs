using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenManager : MonoBehaviour
{
    private int requiredShovelings = 1;
    private int requiredRakings = 1;

    // TODO: more variables for planting seeds
    // growing stuff, collecting produce, etc?
    //public Material gardenIsPrepped; // material for when garden is prepared to be used

    public GameObject tilledSoil;

    public GameObject treePrefab;

    public void shovel()
    {
        requiredShovelings--;
        evaluateState();
    }

    public void rake()
    {
        requiredRakings--;
        evaluateState();
    }

    IEnumerator plantTreeCoroutine(Vector3 position)
    {
        {
            // TODO: just putting a little tree to demonstrate but eventually have a little mound?
            yield return new WaitForSeconds(3);

            // TODO: need to ensure plant will stay in bounds within the garden-area!
            Vector3 treePos = transform.position + 1.2f * position;
            GameObject tree = Instantiate(treePrefab, treePos, Quaternion.AngleAxis(90, Vector3.left));
            tree.tag = "obstacle";
            MeshCollider collider = tree.AddComponent<MeshCollider>();
            collider.convex = true;
        }
    }

    public void plantTree(Vector3 treeLocation)
    {
        StartCoroutine(plantTreeCoroutine(treeLocation));
    }

    private void evaluateState()
    {
        if(requiredShovelings <= 0 && requiredRakings <= 0)
        {
            // change the garden state
            Debug.Log("changing garden state");

            tilledSoil.transform.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(transform.name);
        tilledSoil.transform.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

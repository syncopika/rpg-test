using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerGardenManager : BaseGardenManager
{
    public GameObject treePrefab;
    public GameObject farmer;

    private bool questCompleted = false;
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

            // alert the farmer that tree has been planted (and therefore quest completed)
            if (!questCompleted)
            {
                questCompleted = true;
                farmer.GetComponent<FarmerController>().completeQuest();
            }
        }
    }

    override public void plantTree(Vector3 treeLocation)
    {
        StartCoroutine(plantTreeCoroutine(treeLocation));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

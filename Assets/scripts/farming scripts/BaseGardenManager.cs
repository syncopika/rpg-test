using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGardenManager : MonoBehaviour
{

    IEnumerator plantTreeCoroutine(Vector3 position)
    {
        {
            yield return new WaitForSeconds(3);
        }
    }

    public virtual void plantTree(Vector3 treeLocation)
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

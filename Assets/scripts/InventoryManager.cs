using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    // stuff
    public GameObject fishingPole;
    public GameObject rifle;

    // gardening equipment
    public GameObject shovel;
    public GameObject rake;
    public GameObject wateringCan;

    private GameObject currentlyEquipped = null;
    private string currentlyEquippedName = "";

    private Dictionary<string, GameObject> inventory = new Dictionary<string, GameObject>();

    public GameObject currentlyEquippedObj()
    {
        return currentlyEquipped;
    }

    public string currentlyEquippedObjName()
    {
        return currentlyEquippedName;
    }

    public List<string> getCurrentInventory()
    {
        return inventory.Keys.ToList();
    }

    public GameObject equip(string obj)
    {
        // if currentlyEquippedName == obj, de-equip it
        if (currentlyEquippedName.Equals(obj))
        {
            obj = "";
        }

        // deactivate current object
        if (currentlyEquipped != null) currentlyEquipped.SetActive(false);

        // TODO? play animation to withdraw item from inventory

        if (inventory.ContainsKey(obj))
        {
            currentlyEquippedName = obj;
            currentlyEquipped = inventory[obj];
            currentlyEquipped.SetActive(true);
            return inventory[obj];
        }
        
        currentlyEquippedName = "";

        return null;
    }

    void Start()
    {
        inventory["fishingPole"] = fishingPole;
        inventory["rifle"] = rifle;
        inventory["shovel"] = shovel;
        inventory["rake"] = rake;
        inventory["wateringCan"] = wateringCan;
    }

    void Update()
    {
        // use numpad for toggling between objects
        if (Input.GetKeyUp("1"))
            equip("rifle");
        else if (Input.GetKeyUp("2"))
            equip("shovel");
        else if (Input.GetKeyUp("3"))
            equip("rake");
        else if (Input.GetKeyUp("4"))
            equip("wateringCan");
        else if (Input.GetKeyUp("5"))
            equip("fishingPole");
    }
}

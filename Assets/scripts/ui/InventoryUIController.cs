using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour
{
    public Transform player; // need access to player's inventory
    public Text textPrefab;

    Image canvasBackground;

    bool menuOpen;

    public void toggleInventoryMenu()
    {
        menuOpen = !menuOpen;
        transform.GetComponent<Canvas>().gameObject.SetActive(menuOpen);
    }

    Text addTextToUi(string text, int xPos, int yPos)
    {
        Transform parent = transform.Find("items");
        Text newText = Instantiate(textPrefab, parent);
        newText.text = text;
        newText.rectTransform.anchoredPosition = new Vector3(xPos, yPos, 0);
        return newText;
    }

    void cleanUi()
    {
        Transform parent = transform.Find("items");
        foreach (Transform child in parent)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void updateMenu()
    {
        cleanUi(); // delete old text elements first

        InventoryManager im = player.GetComponent<Player>().getInventory();

        if (im)
        {
            int xPos = -250;
            int yPos = -80;
            foreach (string s in im.getCurrentInventory())
            {
                Text newText = addTextToUi(s, xPos, yPos);

                if (s.Equals(im.currentlyEquippedObjName()))
                {
                    newText.color = Color.blue;
                    newText.fontStyle = FontStyle.Normal;
                }

                yPos += 45;
            }
        }
    }

    void Start()
    {
        canvasBackground = transform.GetComponent<Canvas>().GetComponent<Image>();

        transform.GetComponent<Canvas>().gameObject.SetActive(false);
        menuOpen = false;
    }

    void Update()
    {
        if (player == null)
        {
            GameObject p = GameObject.Find("low-poly-human-edit-rig2-edit");
            if (p)
            {
                player = p.transform;
            }
        }

        if (player != null && menuOpen)
        {
            updateMenu();
        }
    }
}

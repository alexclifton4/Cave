using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject UIPrefab;
    public GameObject UIInventory;
    public int UISpacing;

    private List<EntityDescription> items;

    // Start is called before the first frame update
    void Start()
    {
        items = new List<EntityDescription>();
    }

    // Adds an item to the inventory
    public void Add(EntityDescription item)
    {
        items.Add(item);
        // Create the UI element
        GameObject obj = Instantiate(UIPrefab);
        obj.transform.SetParent(UIInventory.transform);
        obj.GetComponent<InventoryEntry>().Init(item, UISpacing * (items.Count - 1));
    }

    // Checks if the inventory contains an item
    public bool Contains(string name)
    {
        // Loop through the inventory
        foreach(EntityDescription item in items)
        {
            if (item.name.Equals(name))
            {
                return true;
            }
        }

        return false;
    }

    // Clears all the items
    public void Clear()
    {
        items.Clear();

        // Remove all UI entries
        foreach (Transform t in UIInventory.transform)
		{
            Destroy(t.gameObject);
		}
    }
}

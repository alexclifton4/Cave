using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public NPCDescription description;
    public GameObject baseEntity;

    // Called when the NPC is instantiated
    void Awake()
    {
        // Add to the list
        GameManager.Instance.npcs.Add(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set properties from description
        GetComponent<SpriteRenderer>().sprite = description.sprite;
    }

    // Destroys the NPC
    public void Remove()
    {
        // Remove from the list and destroy
        GameManager.Instance.npcs.Remove(gameObject);
        Destroy(gameObject);
    }
}

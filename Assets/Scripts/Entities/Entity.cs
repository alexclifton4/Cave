using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public EntityDescription description;

    private NPC npc;

    // Called when the entity is instantiated
    void Awake()
    {
        // Add to the list
        GameManager.Instance.entities.Add(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set properties from description
        GetComponent<SpriteRenderer>().sprite = description.sprite;

        // If this is a spawner, spawn an NPC
        if (description.npcSpawner) {
            GameObject npcGO = Instantiate(description.npc.prefab);
            npcGO.transform.position = transform.position;

            npc = npcGO.GetComponent<NPC>();
            npc.description = description.npc;
            npc.baseEntity = gameObject;
        }
    }

    // Destroys the entity and any attached NPCs
    public void Remove() {
        // Remove any attached NPCs
        if (npc) {
            npc.Remove();
        }

        // Remove from list
        GameManager.Instance.entities.Remove(gameObject);

        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public EntityDescription description;

    // Start is called before the first frame update
    void Start()
    {
        // Set properties from description
        GetComponent<SpriteRenderer>().sprite = description.sprite;

        // If this is a spawner, spawn an NPC
        if (description.npcSpawner) {
            GameObject npc = Instantiate(description.npc.prefab);
            npc.GetComponent<NPC>().description = description.npc;
            npc.transform.position = transform.position;
        }
    }
}

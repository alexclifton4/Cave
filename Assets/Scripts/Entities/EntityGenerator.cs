using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EntityGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public CaveGenerator cave;

    public static List<GameObject> entities;

    // Generates entities for a cave
    public void GenerateEntities(EntityDescription[] entitiesToAdd)
    {
        if (entities == null)
        {
            entities = new List<GameObject>();
        }

        foreach (EntityDescription entity in entitiesToAdd) {
            // Create the entity
            Vector3 position = tilemap.GetCellCenterWorld((Vector3Int)cave.AllocateRandomTile(caveTile.entity));
            GameObject entityObj = Instantiate(entity.prefab);

            // Set its properties
            entityObj.transform.position = (Vector2) position;
            entityObj.GetComponent<Entity>().description = entity;

            // Add to list
            entities.Add(entityObj);
        }
    }

    public void ResetEntities()
    {
        if (entities != null)
        {
            // Destroy all the entities
            foreach (GameObject entity in entities)
            {
                Destroy(entity);
            }

            entities.Clear();
        }
    }
}
 
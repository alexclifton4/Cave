using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EntityGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public CaveGenerator cave;

    // Generates entities for a cave
    public void GenerateEntities(EntityDescription[] entitiesToAdd)
    {
        if (GameManager.Instance.entities == null)
        {
            GameManager.Instance.entities = new List<GameObject>();
        }

        foreach (EntityDescription entity in entitiesToAdd) {
            // Create the entity
            Vector3 position = tilemap.GetCellCenterWorld((Vector3Int)cave.AllocateRandomTile(caveTile.entity));
            GameObject entityObj = Instantiate(entity.prefab);

            // Set its properties
            entityObj.transform.position = (Vector2) position;
            entityObj.GetComponent<Entity>().description = entity;
        }
    }

    public void ResetEntities()
    {
        if (GameManager.Instance.entities != null)
        {
            // Destroy all the entities
            while (GameManager.Instance.entities.Count != 0)
            {
                GameManager.Instance.entities[0].GetComponent<Entity>().Remove();
            }
        }
    }
}
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsVisualiser
{
    private static bool boundsShowing = false;

    public static void ToggleBounds() {
        // Toggle state
        if (boundsShowing = !boundsShowing)
        {
            // Show bounds
            // Loop through each npc
            foreach (GameObject npc in GameManager.Instance.npcs)
            {
                // Get or add line renderer
                LineRenderer lr = npc.GetComponent<LineRenderer>();
                if (lr == null)
                {
                    lr = npc.AddComponent<LineRenderer>();
                    lr.useWorldSpace = false;
                    lr.startWidth = 0.2f;
                    lr.endWidth = 0.2f;

                    // Draw the circle
                    DrawCircle(lr, npc.GetComponent<NPC>().description.detectionRadius);
                } else
                {
                    lr.enabled = true;
                }

                // Do the same for the base entity
                NPC npcObj = npc.GetComponent<NPC>();
                lr = npcObj.baseEntity.GetComponent<LineRenderer>();
                if (lr == null)
                {
                    lr = npcObj.baseEntity.AddComponent<LineRenderer>();
                    lr.useWorldSpace = false;
                    lr.startWidth = 0.2f;
                    lr.endWidth = 0.2f;

                    // Draw the circle
                    DrawCircle(lr, npc.GetComponent<NPC>().description.baseRadius);
                } else 
                {
                    lr.enabled = true;
                }
            }
        } else {
            // Hide bounds
            // Loop through each npc
            foreach(GameObject npc in GameManager.Instance.npcs)
            {
                npc.GetComponent<LineRenderer>().enabled = false;
                npc.GetComponent<NPC>().baseEntity.GetComponent<LineRenderer>().enabled = false;
            }
        }
    }

    private static void DrawCircle(LineRenderer lr, float size)
    {
        lr.positionCount = 31;
        float x, y, angle = 0;

        for (int i = 0; i < (31); i++)
        {
            x = Mathf.Sin(angle) * size;
            y = Mathf.Cos(angle) * size;

            lr.SetPosition(i, new Vector3(x, y, -1));

            angle += 2 * Mathf.PI / 30;
        }
    }
}

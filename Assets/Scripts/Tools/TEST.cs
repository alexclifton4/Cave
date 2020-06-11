using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    public bool redraw = false;
    public float size;
    public int segments;

    LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (redraw)
        {
            Draw();
            redraw = false;
        }
    }

    void Draw()
    {
        lineRenderer.positionCount = segments + 1;
        float x, y, angle = 0;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(angle) * size;
            y = Mathf.Cos(angle) * size;

            lineRenderer.SetPosition(i, new Vector3(x, y, -1));

            angle += 2 * Mathf.PI / segments;
        }
    }
}

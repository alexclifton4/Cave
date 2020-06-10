﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public NPCDescription description;

    // Start is called before the first frame update
    void Start()
    {
        // Set properties from description
        GetComponent<SpriteRenderer>().sprite = description.sprite;
    }
}

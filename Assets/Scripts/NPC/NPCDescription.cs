using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New NPC", menuName="NPC")]
public class NPCDescription : ScriptableObject
{
    public new string name;
    public Sprite sprite;
    public GameObject prefab;
}

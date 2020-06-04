using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Entity", menuName="Entity")]
public class EntityDescription : ScriptableObject
{
    public new string name;
    public string description;
    public Sprite sprite;
    public GameObject prefab;
    public bool collectable;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New GameMode", menuName="Game Mode")]
public class GameMode : ScriptableObject
{
    public new string name;
    public EntityDescription[] entities;
}

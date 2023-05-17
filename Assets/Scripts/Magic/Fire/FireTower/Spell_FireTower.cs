using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Khan_Shared.Magic;

[CreateAssetMenu(fileName = "FireTower", menuName = "Magic/Fire/FireTower/FireTower")]
public class Spell_FireTower : Spell
{
    public float size = 1;
    public Color color = Color.black;
    public float animationSpeed = 1;

    public override void Trigger()
    {
        Instantiate(prefab);
        Debug.Log($"made a fire tower with size {size} and color {color}");
    }
}

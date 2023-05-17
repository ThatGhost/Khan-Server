using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Khan_Shared.Magic;

public class PrefabBuilder_FFT : PrefabBuilder
{
    // Components

    // Modify the components based on spell params
    public override void build(Spell spell)
    {
        if (spell is not Spell_FireTower fireTower) return;

        gameObject.transform.position = new Vector3(0, 0, fireTower.size);
    }
}

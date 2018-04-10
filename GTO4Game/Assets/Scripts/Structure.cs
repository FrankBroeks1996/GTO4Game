using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum StructureType
{
    Base,
    Defense,
    UnitProduction
}

public class Structure : Unit {
    [Header("Screen that shows when structure is selected")]
    public GameObject UnitsScreen;

    [Header("The range that units can spawn in")]
    public int SpawnRange = 1; 

    [Header("Properties used in case of Game over accurs")]
    public StructureType StructureType;
    public UnityEvent DeathEvent = new UnityEvent();

    public override void Death()
    {
        if (StructureType == StructureType.Base)
        {
            DeathEvent.Invoke();
        }
        base.Death();
    }
}

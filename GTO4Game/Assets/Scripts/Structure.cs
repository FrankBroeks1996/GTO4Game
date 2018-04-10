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
    public GameObject UnitsScreen;
    public int BuildingTime;
    public int SpawnRange = 1; 
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

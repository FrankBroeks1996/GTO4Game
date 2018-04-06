using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StructureType
{
    Base,
    Defense,
    UnitProduction
}

public class Structure : ArmyEntity {
    public GameObject UnitsScreen;
    public int BuildingTime;
    public List<UnitFactory> Factorys;
    public int SpawnRage; 
    public StructureType StructureType;
}

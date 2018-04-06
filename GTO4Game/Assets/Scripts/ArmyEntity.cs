using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ArmyEntity : MonoBehaviour {
    public Player Player;
    public int Health;
    public int Damage;
    public int Price;
    public bool CanAttackInTurn = true;

    public UnityEvent OnHealthChanged = new UnityEvent();

    public void RemoveHealth(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Tile targetUnitTile = transform.parent.GetComponent<Tile>();
            targetUnitTile.Occupied = false;
            Destroy(gameObject);
        }
        OnHealthChanged.Invoke();
    }
}

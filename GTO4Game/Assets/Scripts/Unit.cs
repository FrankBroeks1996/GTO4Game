using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour {

    [Header("The player the unit belongs to")]
    public Player Player;

    [Header("Unit stats")]
    public int Health;
    public int Damage;
    public int Price;
    public int MovementRange;
    public int AttackRange;
    
    [Header("Booleans controlling if the player can move/attack")]
    public bool CanMoveInTurn = true;
    public bool CanAttackInTurn = true;

    [Header("Properties keeping track of highlighting")]
    public bool IsHighLight = false;
    private Color originalColor;

    public UnityEvent OnHealthChanged = new UnityEvent();

    void Awake()
    {
        originalColor = GetComponent<Renderer>().material.color;
    }

    public void RemoveHealth(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Death();
        }
        OnHealthChanged.Invoke();
    }

    public virtual void Death()
    {
        Tile targetUnitTile = transform.parent.GetComponent<Tile>();
        targetUnitTile.Occupied = false;
        Destroy(gameObject);
    }

    public void HighLight(bool highlightOn)
    {
        IsHighLight = !IsHighLight;
        if (highlightOn)
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
        else
        {
            GetComponent<Renderer>().material.color = originalColor;
        }
    }

    public void AttackEnemy(Unit target)
    {
        if (CanAttackInTurn)
        {
            target.RemoveHealth(Damage);
            CanAttackInTurn = false;
        }
    }

    public void Move(Tile tile)
    {
        if (CanMoveInTurn)
        {
            Tile fromTile = transform.parent.GetComponent<Tile>();
            fromTile.ArmyEntityOnTile = null;
            fromTile.Occupied = false;

            tile.ArmyEntityOnTile = this;
            tile.Occupied = true;
            transform.position = new Vector3(tile.transform.position.x, transform.position.y, tile.transform.position.z);
            transform.SetParent(tile.transform);
            CanMoveInTurn = false;
        }
    }
}

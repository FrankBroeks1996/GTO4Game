﻿using System.Collections;
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

    public bool IsHighLight = false;
    private Color originalColor;

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
            GetComponent<Renderer>().material.color = Color.white;
        }
        else
        {
            GetComponent<Renderer>().material.color = originalColor;
        }
    }
}

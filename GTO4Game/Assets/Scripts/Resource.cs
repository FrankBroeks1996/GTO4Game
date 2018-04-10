using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Resource : MonoBehaviour {
    [Header("Resource information")]
    public int CurrentOwned;
    public int StartAmount;

    public UnityEvent OnValueChanged = new UnityEvent();
    
	void Awake () {
        CurrentOwned = StartAmount;
	}

    public void AddResources(int amount)
    {
        CurrentOwned += amount;
        UpdateUI();
    }

    public bool RemoveResources(int amount)
    {
        if(CurrentOwned - amount >= 0)
        {
            CurrentOwned -= amount;
            UpdateUI();
            return true;
        }
        return false;
    }

    public void UpdateUI()
    {
        OnValueChanged.Invoke();
    }
}

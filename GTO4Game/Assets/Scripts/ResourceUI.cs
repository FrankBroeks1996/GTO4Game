﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Resource))]
public class ResourceUI : MonoBehaviour {

    public Text ResourceAmountText;

    private Resource resource;

    void Awake()
    {
        resource = GetComponent<Resource>();
        resource.OnValueChanged.AddListener(UpdateUI);
    }

    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        ResourceAmountText.text = resource.CurrentOwned.ToString();
    }
}

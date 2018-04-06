using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBarUI : MonoBehaviour {
    public ArmyEntity ArmyEntity;

    private Slider slider;

	void Awake()
    {
        slider = GetComponent<Slider>();
        ArmyEntity.OnHealthChanged.AddListener(UpdateUI);
    }

    void Start()
    {
        slider.maxValue = ArmyEntity.Health;
        UpdateUI();
    }

    public void UpdateUI()
    {
        slider.value = ArmyEntity.Health;
        if (slider.value != slider.maxValue)
        {
            transform.parent.GetComponent<CanvasGroup>().alpha = 1f;
        }
    }
}

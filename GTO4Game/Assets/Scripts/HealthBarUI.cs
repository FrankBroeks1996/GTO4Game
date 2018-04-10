using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBarUI : MonoBehaviour {

    [Header("The unit belonging to the healthbar")]
    public Unit Unit;

    private Slider slider;

	void Awake()
    {
        slider = GetComponent<Slider>();
        Unit.OnHealthChanged.AddListener(UpdateUI);
    }

    void Start()
    {
        slider.maxValue = Unit.Health;
        UpdateUI();
    }

    public void UpdateUI()
    {
        slider.value = Unit.Health;
        if (slider.value != slider.maxValue)
        {
            transform.parent.GetComponent<CanvasGroup>().alpha = 1f;
        }
    }
}

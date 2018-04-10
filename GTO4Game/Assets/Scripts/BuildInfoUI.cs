using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildInfoUI : MonoBehaviour {

    [Header("Unit stats text")]
    public Text HealthText;
    public Text DamageText;
    public Text PriceText;

    [Header("The unit that needs to be shown")]
    public Unit Unit;

    void Start()
    {
        HealthText.text = "Health: " + Unit.Health.ToString();
        if(Unit.Damage == 0)
        {
            DamageText.gameObject.SetActive(false);
        }
        else
        {
            DamageText.text = "Damage: " + Unit.Damage.ToString();
        }
        PriceText.text = "Cost: " + Unit.Price.ToString();
    }

}

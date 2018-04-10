using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildInfoUI : MonoBehaviour {

    public Text HealthText;
    public Text DamageText;
    public Text PriceText;
    public ArmyEntity ArmyEntity;

    void Start()
    {
        HealthText.text = "Health: " + ArmyEntity.Health.ToString();
        if(ArmyEntity.Damage == 0)
        {
            DamageText.gameObject.SetActive(false);
        }
        else
        {
            DamageText.text = "Damage: " + ArmyEntity.Damage.ToString();
        }
        PriceText.text = "Cost: " + ArmyEntity.Price.ToString();
    }

}

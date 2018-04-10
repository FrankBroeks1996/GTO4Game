using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionInfoUI : MonoBehaviour {

    public Text HealthText;
    public Text DamageText;

	public void SetText(int health = 0, int damage = 0)
    {
        ShowScreen();
        if(health != 0)
        {
            if (health < 0)
            {
                HealthText.transform.gameObject.SetActive(true);
            }
            HealthText.text = "Health: " + health;
        }
        else
        {
            HealthText.transform.gameObject.SetActive(false);
        }
        
        if(damage != 0)
        {
            DamageText.transform.gameObject.SetActive(true);
            DamageText.text = "Damage: " + damage;
        }
        else
        {
            DamageText.transform.gameObject.SetActive(false);
        }
    }

    public void HideScreen()
    {
        gameObject.SetActive(false);
    }

    public void ShowScreen()
    {
        gameObject.SetActive(true);
    }
}

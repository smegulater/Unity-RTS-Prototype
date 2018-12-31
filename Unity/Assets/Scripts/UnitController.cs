using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitController : MonoBehaviour
{
    // Start is called before the first frame update
    public UnitStats Stats;

    public GameObject UI;
    public Text uiHealth;
    public Text uiStamina;

    public bool Selected;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Selected)
        {
            if (!UI.activeSelf)
            {
                ShowUI();
            }

            UpdateUI();

            if (Stats.CurrentHealth <= 0)
            {
                Stats.CurrentHealth = Stats.MaxHealth;
            }
            else
            {
                Stats.CurrentHealth -= Random.value * 3.8f;
            }

            if (Stats.CurrentStamina <= 0)
            {
                Stats.CurrentStamina = Stats.MaxStamina;
            }
            else
            {
                Stats.CurrentStamina -= Random.value * 3.8f;
            }
        }
        else
        {
            if (UI.activeSelf)
            {
                HideUI();
            }
        }

    }

    private void UpdateUI()
    {
        uiHealth.text = (Stats.CurrentHealth / Stats.MaxHealth).ToString("P");
        uiStamina.text = (Stats.CurrentStamina / Stats.MaxStamina).ToString("P");
    }

    public void HideUI()
    {
        UI.SetActive(false);
    }
    public void ShowUI()
    {
        UI.SetActive(true);
    }
}

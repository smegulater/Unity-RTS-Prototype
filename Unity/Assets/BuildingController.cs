using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SelectionController))]
public class BuildingController : MonoBehaviour
{

    public BuildingStats Stats;

    public GameObject UI;
    public Text uiHealth;

    private SelectionController sc;

    void Start()
    {
        sc = gameObject.GetComponent<SelectionController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sc.Selected)
        {
            if (!UI.activeSelf)
            {
                ShowUI();
            }

            UpdateUI();

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

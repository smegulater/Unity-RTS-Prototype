using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityRTSCore;


public class Metric
{
    public string Name { get { return name; } }
    public object Data { get { return data; } set { data = value; } }

    private string name;
    private object data;

    public Metric(string Name, object Data)
    {
        name = Name;
        data = Data;
    }

}

public class DebugOverlayManager : MonoBehaviour
{
    public static DebugOverlayManager instance;

    private Queue<Metric> updateQueue;
    private List<Metric> metrics;
    private Text DebugText;

    // Start is called before the first frame update
    private void Start()
    {
        #region Singleton Check
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Debug.LogError("Attempted to create multiples instances of the DebugOverlayManager Script");
            Destroy(this);
        }
        #endregion

        updateQueue = new Queue<Metric>();
        metrics = new List<Metric>();



        DebugText = transform.GetComponentInChildren<Text>();


    }

    // Update is called once per frame
    private void Update()
    {
        UpdateDebugOverlay();
    }

    private void UpdateDebugOverlay()
    {
        string debugText = "";

        foreach(Metric metric in metrics)
        {
            debugText += RTSCore.MultiDebugLine(metric.Name,metric.Data);
        }

        DebugText.text = debugText;
        
    }

    public void AddMetric(string Name, object Data)
    {
        Metric newMetric = new Metric(Name, Data);

        bool unique = true;

        int index = 0;

        for (int i = 0; i < metrics.Count; i++)
        {
            if (metrics[i].Name == newMetric.Name)
            {
                unique = false;
                index = i;

            }
        }



        if (unique)
        {
            metrics.Add(newMetric);
            Debug.Log("Added a new metric with name ( "+ newMetric.Name +" ).");
        }
        else
        {
            metrics[index].Data = Data;
            return;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DailyObj : MonoBehaviour
{
    [SerializeField] private GameObject completedObject;
    [SerializeField] private GameObject notCompletedObject;
    [SerializeField] private TMP_Text day;
    public void Init(bool state,bool today)
    {
        completedObject.SetActive(state);
        notCompletedObject.SetActive(!state);
        if(today)
            day.color = new Color(day.color.r,day.color.g,day.color.b,.5f);
        else
            day.color = new Color(day.color.r, day.color.g, day.color.b, 1);
    }
}

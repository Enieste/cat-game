using UnityEngine;
using TMPro;
using System;

public class DaylightDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI daylightText;

    private void Update()
    {
        float daylight = InkStateHandler.GetDaylight();
        int displayValue = Mathf.RoundToInt(daylight / 10f);
        daylightText.text = $"{displayValue}/10";
    }
}


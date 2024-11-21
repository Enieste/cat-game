using UnityEngine;
using UnityEngine.UI;

public class HungerMeter : MonoBehaviour
{
    [SerializeField] private HungerSystem hungerSystem;
    [SerializeField] private Text debugText;

    private void Update()
    {
        HungerState state = hungerSystem.GetCurrentState();
        debugText.text = $"Saturation: {state.Saturation}, Plays: {state.Plays}, Pets: {state.Pets}, Daylight: {state.Daylight}, Day: {state.Day}";
    }
}
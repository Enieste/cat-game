using UnityEngine;

public class Fridge : MonoBehaviour
{
    [SerializeField] private HungerSystem hungerSystem;
    
    private void Awake()
    {
        
    }

    private void OnMouseDown()
    {
        DispenseFood();
    }

    public void DispenseFood()
    {
        var kibble = new Kibble();
        hungerSystem.Feed(kibble);
    }
}

public class Kibble : IFood
{
    public int SaturationValue => 70;
    public int ConsumptionTime => 3;
}

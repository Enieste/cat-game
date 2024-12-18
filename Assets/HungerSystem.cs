using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class HungerState
{
    public const int Min = 0;
    public const int Max = 100;
    public const int HungerThreshold = 20;

    public int Saturation { get; private set; }
    public int Plays { get; private set; }
    public int Pets { get; private set; }
    public int Daylight { get; private set; }
    public int Day { get; private set; }

    public HungerState(int saturation, int plays, int pets, int daylight, int day)
    {
        Saturation = saturation;
        Plays = plays;
        Pets = pets;
        Daylight = daylight;
        Day = day;
    }

}

public enum ActivityType
{
    Resting,
    Playing,
    Petting,
    Eating
}

public interface IFood
{
    int SaturationValue { get; }
    int ConsumptionTime { get; }
}

public interface IToy
{
    int Fun { get; }
    int PlayTime { get; }
}

public class HungerSystem : MonoBehaviour
{
    private float accumulatedTime;
    private HungerAction? lastNotifiedAction;
    [SerializeField] private InkDialogueManager dialogueManager;
    [SerializeField] private CatController catController;

    private static readonly Dictionary<ActivityType, int> ActivitySaturationPrice = new Dictionary<ActivityType, int>
    {
        { ActivityType.Resting, 1 },
        { ActivityType.Playing, 2 },
        { ActivityType.Petting, 2 },
        // don't make it positive, it's easier to change food value instead
        { ActivityType.Eating, 0 }
    };

    private static readonly Dictionary<ActivityType, int> ActivityPlaysPrice = new Dictionary<ActivityType, int>
    {
        { ActivityType.Resting, 1 },
        { ActivityType.Playing, -3 },
        { ActivityType.Petting, 1 },
        { ActivityType.Eating, 2 }
    };
    
    private static readonly Dictionary<ActivityType, int> ActivityPetsPrice = new Dictionary<ActivityType, int>
    {
        { ActivityType.Resting, 1 },
        { ActivityType.Playing, 2 },
        { ActivityType.Petting, -5 },
        { ActivityType.Eating, 1 }
    };

    private static readonly Dictionary<ActivityType, int> ActivityRestingPrice = new Dictionary<ActivityType, int>
    {
        { ActivityType.Resting, 1 },
        { ActivityType.Playing, 1 },
        { ActivityType.Petting, 1 },
        { ActivityType.Eating, 1 }
    };

    public event Action<HungerAction> OnActionOccurred;

    private void Awake()
    {
        accumulatedTime = 0f;
        BindInkMethods();
    }

    private void BindInkMethods()
    {
        UnbindInkMethods();
        dialogueManager.GetStory().BindExternalFunction("Pet", Pet);
        InkStateHandler.ReportExternalFunction("Pet");
        dialogueManager.GetStory().BindExternalFunction("Play", Play);
        InkStateHandler.ReportExternalFunction("Play");
        dialogueManager.GetStory().BindExternalFunction("Idle", Idle);
        InkStateHandler.ReportExternalFunction("Idle");

    }

    private void UnbindInkMethods()
    {
        if (InkStateHandler.IsExternalFunctionKnown("Pet"))
        {
            dialogueManager.GetStory().UnbindExternalFunction("Pet");
        }
        if (InkStateHandler.IsExternalFunctionKnown("Play"))
        {
            dialogueManager.GetStory().UnbindExternalFunction("Play");
        }
        if (InkStateHandler.IsExternalFunctionKnown("Idle"))
        {
            dialogueManager.GetStory().UnbindExternalFunction("Idle");
        }

    }

    public void FeedWith(IFood food)
    {
        float currentSaturation = InkStateHandler.GetFood();
        float newSaturation = Mathf.Min(HungerState.Max, currentSaturation + food.SaturationValue);
        InkStateHandler.SetFood(newSaturation);
        CheckAndNotifyHungerState(Mathf.RoundToInt(newSaturation));
        ProcessTime(food.ConsumptionTime, ActivityType.Eating);
    }

    public void PlayWith(IToy toy)
    {
        float currentPlay = InkStateHandler.GetPlay();
        float newPlay = Mathf.Min(HungerState.Max, currentPlay + toy.Fun);
        InkStateHandler.SetPlay(newPlay);
        ProcessTime(toy.PlayTime, ActivityType.Playing);
    }

    public void Pet()
    {
        ProcessTime(10, ActivityType.Petting);
    }

    public void Play()
    {
        ProcessTime(10, ActivityType.Playing);
    }

    public void Idle()
    {
        ProcessTime(10, ActivityType.Resting);
    }

    public void ProcessTime(float deltaTime, ActivityType activity)

    {
        if (InkStateHandler.IsNight())
        {
            Debug.LogError("No time can be spent at night, control should be in Ink right now");
            return;
        }
        
        accumulatedTime += deltaTime;
        
        while (accumulatedTime >= 1f)
        {
            var t = Mathf.RoundToInt(accumulatedTime);

            float currentSaturation = InkStateHandler.GetFood();
            float saturationLoss = ActivitySaturationPrice[activity] * t;
            float newSaturation = Mathf.Max(HungerState.Min, currentSaturation - saturationLoss);
            InkStateHandler.SetFood(newSaturation);
            
            CheckAndNotifyHungerState(Mathf.RoundToInt(newSaturation));

            float currentPlays = InkStateHandler.GetPlay();
            float playsLoss = ActivityPlaysPrice[activity] * t;
            float newPlays = Mathf.Max(HungerState.Min, currentPlays - playsLoss);
            InkStateHandler.SetPlay(newPlays);
            
            float currentPets = InkStateHandler.GetPets();
            float petsLoss = ActivityPetsPrice[activity] * t;
            float newPets = Mathf.Max(HungerState.Min, currentPets - petsLoss);
            InkStateHandler.SetPets(newPets);

            int daylight = InkStateHandler.GetDaylight();
            int newDaylight = daylight - t;
            var tickNight = newDaylight < 1;
            int newDaylightAdjusted = tickNight ? 0 : newDaylight;
            InkStateHandler.SetDaylight(newDaylightAdjusted);
            
            accumulatedTime = 0;
        }
    }

    private void CheckAndNotifyHungerState(int saturation)
    {
        HungerAction? currentAction = null;

        if (saturation <= HungerState.Min)
        {
            currentAction = HungerAction.Starving;
        }
        else if (saturation <= HungerState.HungerThreshold)
        {
            currentAction = HungerAction.Hungry;
        }
        else if (saturation > HungerState.Max)
        {
            currentAction = HungerAction.Oversaturated;
        }

        // Only notify if the action has changed
        if (currentAction != lastNotifiedAction)
        {
            if (currentAction.HasValue)
            {
                OnActionOccurred?.Invoke(currentAction.Value);
            }
            
            lastNotifiedAction = currentAction;
        }
    }

    public HungerState GetCurrentState()
    {
        return new HungerState(Mathf.RoundToInt(InkStateHandler.GetFood()),
            Mathf.RoundToInt(InkStateHandler.GetPlay()),
            Mathf.RoundToInt(InkStateHandler.GetPets()),
            InkStateHandler.GetDaylight(),
            InkStateHandler.GetDate());
    }
}

public enum HungerAction
{
    Eating,
    Hungry,
    Starving,
    Oversaturated
}
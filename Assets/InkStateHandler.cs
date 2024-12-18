using UnityEngine;
using Ink.Runtime;
using System;
using System;
using System.Collections.Generic;

public static class InkStateHandler
{
    private static Story _story;
    
    private static int DAYLIGHT = 100;

    public static Story InitializeOrGet(TextAsset inkJSON)
    {
        if (_story == null)
        {
            _story = new Story(inkJSON.text);
            _story.allowExternalFunctionFallbacks = true;
            BindExternalFunctions();

            _story.variablesState["food"] = 50f;
            _story.variablesState["play"] = 50f;
            _story.variablesState["pets"] = 50f;
            _story.variablesState["daylight"] = DAYLIGHT;

            Debug.Log($"Ink story initialized with values - Food: {GetFood()}, Play: {GetPlay()}, Pets: {GetPets()}");
        }

        return _story;
    }

    private static HashSet<string> externalFunctionsKnown = new HashSet<string>();

    public static void ReportExternalFunction(string name)
    {
        externalFunctionsKnown.Add(name);
    }

    public static bool IsExternalFunctionKnown(string name)
    {
        return externalFunctionsKnown.Contains(name);
    }

    private static void BindExternalFunctions()
    {
        _story.BindExternalFunction("GetFood", () => GetFood());
        _story.BindExternalFunction("SetFood", (float value) => SetFood(value));

        _story.BindExternalFunction("GetPlay", () => GetPlay());
        _story.BindExternalFunction("SetPlay", (float value) => SetPlay(value));

        _story.BindExternalFunction("GetPets", () => GetPets());
        _story.BindExternalFunction("SetPets", (float value) => SetPets(value));
        
        _story.BindExternalFunction("GetDate", () => GetDate());
        _story.BindExternalFunction("SetDate", (int value) => SetDate(value));
        
        _story.BindExternalFunction("isNight", () => IsNight());
        _story.BindExternalFunction("goDay", GoDay);
     
        //_story.BindExternalFunction("Pet", () => {
        //    throw new Exception("Pet function not redefined");
        //});
        //_story.BindExternalFunction("Feed", () => {
        //    throw new Exception("Feed function not redefined");
        //});


    }

    private static void ResetState()
    {
        // Save current variables
        float currentFood = (float)_story.variablesState["food"];
        float currentPlay = (float)_story.variablesState["play"];
        float currentPets = (float)_story.variablesState["pets"];

        // Reset state
        _story.ResetState();
        //BindExternalFunctions();

        _story.variablesState["food"] = currentFood;
        _story.variablesState["play"] = currentPlay;
        _story.variablesState["pets"] = currentPets;
    }

    public static float GetFood()
    {
        return (float)_story.variablesState["food"];
    }

    public static void SetFood(float value)
    {
        _story.variablesState["food"] = Mathf.Clamp(value, 0f, 100f);
    }

    public static float GetPlay()
    {
        return (float)_story.variablesState["play"];
    }

    public static void SetPlay(float value)
    {
        _story.variablesState["play"] = Mathf.Clamp(value, 0f, 100f);
    }

    public static float GetPets()
    {
        return (float)_story.variablesState["pets"];
    }

    public static void SetPets(float value)
    {
        _story.variablesState["pets"] = Mathf.Clamp(value, 0f, 100f);
    }

    public static int GetDate()
    {
        return (int)_story.variablesState["date"];
    }

    public static void SetDate(int value)
    {
        _story.variablesState["date"] = value;
    }

    public static int GetDaylight()
    {
        return (int)_story.variablesState["daylight"];
    }

    public static void SetDaylight(int value)
    {
        int min = 0;
        int max = 100;
        _story.variablesState["daylight"] = System.Math.Clamp(value, min, max);
    }

    public static void SetNewDay()
    {
        SetDate(GetDate() + 1);
        SetDaylight(100);
    }

    public static bool IsNight()
    {
        return GetDaylight() == 0;
    }

    public static void GoDay()
    {
        var date = GetDate();
        date++;
        SetDate(date);
        SetDaylight(DAYLIGHT);
    }

    public static Story GetStory(TextAsset inkJSON)
    {
        if (_story == null)
        {
            InitializeOrGet(inkJSON);
        }
        return _story;
    }
}
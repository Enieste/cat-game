EXTERNAL GetFood()
EXTERNAL SetFood(value)
EXTERNAL GetPlay()
EXTERNAL SetPlay(value)
EXTERNAL GetPets()
EXTERNAL SetPets(value)

EXTERNAL GetDaylight()
EXTERNAL GetDate()
EXTERNAL SetDate(value)


VAR food = 50.0
VAR play = 50.0
VAR pets = 50.0
VAR daylight = 100
VAR date = 1


=== cat_dialogue ===
~ temp currentFood = GetFood()
~ temp currentPlay = GetPlay()
~ temp currentPets = GetPets()
~ food = currentFood
~ play = currentPlay
~ pets = currentPets

{
    - currentPlay < 30 && currentPets < 30:
        The cat seems very unhappy, desperately trying to get your attention...
    - currentPets < 30:
        The cat looks at you longingly, wanting affection...
    - currentPlay < 30:
        The cat seems restless, looking for something to do...
    - currentFood < 30:
        The cat looks at you with hungry eyes and meows desperately...
    - else:
        The cat looks at you contentedly.
}

+ [Pet the cat]
    ~ pets = 100.0
    ~ SetPets(pets)
    {currentFood < 30:
        The cat purrs, but keeps looking at the food bowl...
    - else:
        The cat purrs happily, enjoying your attention.
    }
    -> END
+ [Play with cat]
    ~ play = 100.0
    ~ SetPlay(play)
    {currentFood < 30:
        The cat tries to play, but seems more interested in food right now.
    - else:
        The cat excitedly chases after the toy!
    }
    -> END
+ [Ignore]
    You left the cat alone.
    -> END

=== food_bowl ===
~ temp currentFood = GetFood()
The cat watches the food bowl intently.
+ [Feed the cat]
    ~ food = 100.0
    ~ SetFood(food)
    You pour some food into the bowl. The cat happily munches away.
    -> END
+ [Ignore]
    {currentFood < 30:
        The cat meows sadly and keeps staring at the bowl.
    - else:
        The cat loses interest and walks away.
    }
    -> END
    
=== night_1 ===
You go to bed...
+ [Leave the door closed]
    You close the door and try to sleep.
    Suddenly, you wake up because of sad squeaks behind the door...
        + +  [Let the kitten in]
                    The kitten happily enters your room. Jumps up your bed, purrs and curls up next to you.
                            -> END
        ++  [Ignore the squeaks]
            You try to ignore the meowing...
                +++  [Put a pillow over your head]
                            The squeaks continue, but become more distant as you drift off to sleep.
                            -> END
                +++  [Feel guilty]
                            After a few minutes of guilt, you get up and open the door.
                            The kitten triumphantly enters and claims its spot on your bed.
                            -> END
        ++ [Call out "Go to sleep!"]
           The meowing stops for a moment...
           Then continues even louder.
                +++  [Give up and open the door]
                            The cat struts in victoriously.
                            -> END
               +++  [Endure it stubbornly]
                            It takes a long time, but eventually you fall asleep while the cat continues to sit near the door.
                            -> END
+ [Leave the door open]
    The kitten follows you to bed immediately...
    He purrs and curls up next to your pillow.
    You both sleep peacefully.
        -> END
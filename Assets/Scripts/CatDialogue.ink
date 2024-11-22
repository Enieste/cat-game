=== function Pet ===
~ return

=== function Feed ===
~ return

=== function Play ===
~ return

EXTERNAL GetFood()
EXTERNAL SetFood(value)
EXTERNAL GetPlay()
EXTERNAL SetPlay(value)
EXTERNAL GetPets()
EXTERNAL SetPets(value)
EXTERNAL Pet()
EXTERNAL Feed()
EXTERNAL Play()

EXTERNAL GetDaylight()
EXTERNAL GetDate()
EXTERNAL SetDate(value)

EXTERNAL isNight()
EXTERNAL goDay()


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
    ~ Pet()
    {currentFood < 30:
        The cat purrs, but keeps looking at the food bowl...
    - else:
        The cat purrs happily, enjoying your attention.
    }
    -> END
+ [Play with cat]
    ~ SetPlay(play + 10)
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
    ~ Feed()
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
    ~ SetPets(pets - 10)
    You close the door and try to sleep.
    Suddenly, you wake up because of sad squeaks behind the door...
        + +  [Let the kitten in]
                    The kitten happily enters your room. Jumps up your bed, purrs and curls up next to you.
                    ~ SetPets(pets + 30)
                            -> END
        ++  [Ignore the squeaks]
            You try to ignore the meowing...
                +++  [Put a pillow over your head]
                            ~ SetPets(pets - 20)
                            The squeaks continue, but become more distant as you drift off to sleep.
                            -> END
                +++  [Feel guilty]
                            After a few minutes of guilt, you get up and open the door.
                            The kitten triumphantly enters and claims its spot on your bed.
                            ~ SetPets(pets + 30)
                            -> END
        ++ [Call out "Go to sleep!"]
           The meowing stops for a moment...
           Then continues even louder.
                +++  [Give up and open the door]
                            The cat struts in victoriously.
                            ~ SetPets(pets + 30)
                            -> END
               +++  [Endure it stubbornly]
                            It takes a long time, but eventually you fall asleep while the cat continues to sit near the door.
                            ~ SetPets(pets - 20)
                            -> END
+ [Leave the door open]
    ~ SetPets(pets - 30)
    The kitten follows you to bed immediately...
    He purrs and curls up next to your pillow.
    You both sleep peacefully.
        -> END
        
        
=== cat_couch === 
The cat is comfortably lying on a surface.
    + [Pet the cat]
        The cat purrs contentedly, enjoying the attention.
        ~ Pet()
        -> END
    + [Shoo away]
        The cat gives you an annoyed look but jumps down from the couch.
        ~ SetPets(pets - 20)
        -> END
        
=== beginning ==== 
It was a long day. You're tired from work and your best friend couldn't join you for the evening. 
You're heading back to your lonely house, thinking about...
* [New TV series] Maybe you can check out that new show everyone's talking about.
* [A bottle of beer] Even from the street how you can hear that cold beer in the fridge is calling your name.
* [Playing a video game] You still need to beat that annoying boss. Blade of... what was his name again? 
- Yes. The evening probably won't be that bad after all.
Here's your house. And your porch. And your door. And your box.
    Wait a minute.... What box? You didn't order anything.
It's not Christmas and it's not your birthday. Maybe you should call the police?
Suddenly the box cries out in a thin voice. And then you hear some scratching.
A cat! Somebody left a box with a cat on your porch. 
-> box_action


=== box_action ===
What do you want to do?
* [Look for a person who left the box] You look around — the street is empty. Probably that somebody is already far away. -> box_action
* [Left the box outside and proceed with the initial plan] Well... It's not really an option here. We still have a game to play, are we? -> box_action
* [Take the box home] You have a cat now. -> END        
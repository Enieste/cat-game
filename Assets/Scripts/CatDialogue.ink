=== function Pet ===
~ return

=== function Feed ===
~ return

=== function Play ===
~ return

=== function PutOnFloor ===
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
EXTERNAL PutOnFloor()

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
        The cat seems very unhappy, desperately trying to get your attention... # cat_0
    - currentPets < 30:
        The cat looks at you longingly, wanting affection... # cat_1
    - currentPlay < 30:
        The cat seems restless, looking for something to do... # cat_0
    - currentFood < 30:
        The cat looks at you with hungry eyes and meows desperately... # cat_0
    - else:
        The cat looks at you contentedly. # cat_1
}

+ [Pet the cat]
    ~ Pet()
    {currentFood < 30:
        The cat purrs, but keeps looking at the food bowl... # cat_0
    - else:
        The cat purrs happily, enjoying your attention. # cat_1
    }
    -> END
+ [Play with cat]
    ~ Play()
    {currentFood < 30:
        The cat tries to play, but seems more interested in food right now. # cat_0
    - else:
        The cat excitedly chases after your fingers! # cat_thin
    }
    -> END
+ [Ignore]
    You left the cat alone.
    -> END

=== food_bowl ===
~ temp currentFood = GetFood()
~ PutOnFloor()
The cat watches the food bowl intently. # cat_0
+ [Feed the cat]
    ~ Feed()
    You pour some food into the bowl. The cat happily munches away.
    -> END
+ [Ignore]
    {currentFood < 30:
        The cat meows sadly and keeps staring at the bowl. # cat_1
    - else:
        The cat loses interest and walks away. # cat_0
    }
    -> END
    
=== night ===
~ PutOnFloor()
You go to bed... #hide_cat
+ [Leave the door closed]
    ~ SetPets(pets - 10)
    You close the door and try to sleep.
    Suddenly, you wake up because of sad squeaks behind the door...
        + +  [Let the kitten in]
                    The kitten happily enters your room. Jumps up your bed, purrs and curls up next to you. # cat_1
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
                            The kitten triumphantly enters and claims its spot on your bed. # cat_1
                            ~ SetPets(pets + 30)
                            -> END
        ++ [Call out "Go to sleep!"]
           The meowing stops for a moment...
           Then continues even louder.
                +++  [Give up and open the door]
                            The cat struts in victoriously. # cat_1
                            ~ SetPets(pets + 30)
                            -> END
               +++  [Endure it stubbornly]
                            It takes a long time, but eventually you fall asleep while the cat continues to sit near the door.
                            ~ SetPets(pets - 20)
                            -> END
+ [Leave the door open]
    ~ SetPets(pets - 30)
    The kitten follows you to bed immediately... # show_cat # cat_1
    He purrs and curls up next to your pillow.
    You both sleep peacefully.
        -> END
        
=== night_1 ===
~ temp currentFood = GetFood()
~ temp currentPlay = GetPlay()
~ temp currentPets = GetPets()
~ PutOnFloor()
The exhaustion had been building up since work. You've spent the whole evening trying to please your new furry neighbor, and now sleep is the only thing on your mind. #hide_cat
You enter the bedroom and hear the soft patter of tiny paws behind you - the kitten is following you.
What will you do??
    + [Close the door behind you. Let him sleep on the couch.]
       You close the door right before he can get through, then you lie down on your bed.
        The pitiful meowing from the living room makes it hard to fall asleep.
            ++ [Ignore him. He needs to get used to this.]
            You try hard not to hear him. It barely helps - the quiet whimpering follows you even in your dreams.
            You wake up to your alarm and find that the kitten spent the night behind the couch in the living room.
            ~ SetPets (currentPets - 20)
            He emerges covered in dust and greets you with a demanding "meow". # show_cat # cat_1
            -> Morning_1
            ++ [Feel sorry and let him in.]
            You open the door, and the kitten darts inside like lightning, then rubbing against your legs. # show_cat # cat_1
            You get back in bed while he carefully investigates the room. # cat_0
            In the darkness you try to keep an eye on him for a while but quickly fall asleep. # cat_thin
            -> Night_1_with_cat
    + [Leave the door open.]
    The kitten immediately runs in after you and starts exploring the room. # show_cat # cat_1
    You lie down and watch him in the darkness for a while, but quickly fall asleep. # cat_thin
    -> Night_1_with_cat
        
=== Night_1_with_cat ===
~ temp currentPlay = GetPlay()
You wake up to something... or someone trying to pull your blanket off. #hide_cat
You pull it back, but whoever it is, they're strong.
A chill runs down your spine. Suddenly, something bites your favorite toe.
Barely resisting the urge to scream, you reach for the lamp.
The kitten sits on the bed, looking at you with innocent eyes. His pupils are so dilated they almost cover the iris. # show_cat # cat_wide
Looks like he decided to play with your feet. # cat_0
~ SetPlay (currentPlay + 10)
You exhale, turn off the light, and try to go back to sleep. #hide_cat
On the edge between sleep and wakefulness, only one thought buzzes in your head...
"How can such a tiny thing be so strong?"
When annoying alarm sound finally wakes you, the kitten is sleeping sweetly at your feet.
-> Morning_1

=== Morning_1 ===
~ temp currentPlay = GetPlay()
~ temp currentPets = GetPets()
New day. 
You fill the bowl with extra food and head off to work.
In the evening, you hurry home to your little friend.
~ SetPlay (currentPlay - 50)
~ SetPets (currentPets - 50)
-> END
        
=== cat_couch === 
~ temp currentPlay = GetPlay()
The cat is comfortably lying on a surface.
    + [Pet the cat]
        The cat purrs contentedly, enjoying the attention. # cat_1
        ~ Pet()
        -> END
    + [Gently put him on the floor]
        ~ PutOnFloor()
        {currentPlay < 30:
            You managed to place him on the floor, but he tried to bite your fingers in a process. # cat_thin
            ~ SetPlay(play + 10)
        - else:
            The cat purrs while you're putting him on the floor. # cat_1
            ~ SetPets(pets + 10)
        }
        -> END
    + [He shouldn't be there. Shoo away]
        ~ PutOnFloor()
        ~ SetPets(pets - 10)
        The cat gives you an annoyed look but jumps down from the surface. # cat_0
        -> END
        
=== beginning ==== 
# hide_cat
It was a long day. You're tired from work and your best friend couldn't join you for the evening. 
You're heading back to your lonely house, thinking about...
* [New TV series] Maybe you can check out that new show everyone's talking about.
* [A bottle of beer] Even from the street how you can hear that cold beer in the fridge is calling your name.
* [Playing a video game] You still need to beat that annoying boss. Blade of... what was his name again? 
- Yes. The evening probably won't be that bad after all.
Here's your house. And your porch. And your door. And your box. #show_cat # cat_inbox
    Wait a minute.... What box? You didn't order anything.
It's not Christmas and it's not your birthday. Maybe you should call the police?
Suddenly the box cries out in a thin voice. And then you hear some scratching. Before you have a chance to react... It opens. 
A cat! Somebody left a box with a cat on your porch. # cat_box
-> box_action


=== box_action ===
What do you want to do?
* [Look for a person who left the box] You look around — the street is empty. Probably that somebody is already far away. -> box_action
* [Leave the box outside and proceed with the initial plan] Well... It's not really an option here. We still have a game to play, don't we? -> box_action
* [Take the box home] You have a cat now. -> END   


=== night_2 ===
~ temp currentFood = GetFood()
~ temp currentPlay = GetPlay()
~ temp currentPets = GetPets()
You're already in bed, ready for sleep. The house is quiet... perhaps too quiet. #hide_cat

{
    - currentFood < 30:
    Just as you're about to drift off, a long, haunting yowl echoes through the apartment.
    You look up to see the kitten sitting in your bedroom doorway, staring at you with an intensity. # show_cat # cat_wide
        + [Get up and check his bowl]
            You drag yourself out of the warm bed. # cat_1
            His bowl isn't completely empty, but you can see the bottom. # cat_0
            He lets out another soul-piercing meow. # cat_thin
            ++ [Add more food]
                ~ SetFood(currentFood + 30)
                You add a small scoop. He trills happily and trots off to eat.
                When you return to bed, he joins you, purring contentedly. # cat_1
                -> Bedtime_2
            ++ [Return to bed without feeding]
                ~ SetFood(currentFood - 10)
                You return to bed. Throughout the night, your dreams are haunted by increasingly creative vocalizations. # hide_cat
                -> Bedtime_2
                
    - currentPlay < 40:
    The silence is broken by sudden rapid thumping.
    You sit up in bed and see a dark blur racing around your room - from the dresser to the windowsill, then onto your desk.
    The kitten appears suddenly on your bed, eyes wide and glowing in the dark, his tail puffed up like a bottle brush. # show_cat # cat_wide
        + [Try to calm him down with pets]
            ~ SetPets(currentPets + 10)
            ~ SetPlay(currentPlay + 15)
            At least he's gentler now than during your first night together. # cat_1
            -> Bedtime_2
        + [Hide under the blanket]
            ~ SetPlay(currentPlay - 10)
            You pull the covers over your head. Do you think this is a good idea?..
            -> Night_2_chaos           
            
    - currentPets < 40:
    As you're trying to get comfortable, you feel... watched.
    You open your eyes to find the kitten sitting on your chest, completely still, just... staring. # show_cat # cat_wide
    His tail twitches occasionally. The silence is unnerving.
        + [Pet him]
            ~ SetPets(currentPets + 20)
            You reach up to scratch behind his ears. He immediately melts into a purring puddle.
            After a few minutes of attention, he settles down beside you.
            -> Bedtime_2
        + [Try to move him]
            ~ SetPets(currentPets - 10)
            You gently try to shift him off your chest. He becomes a lead weight.
            You try again. He somehow gets heavier.
            This continues until you give up and pet him instead.
            -> Bedtime_2        
                
    - else:
    The day was good - you played together, he ate well, and got plenty of attention.
    You drift off to sleep easily... #hide_cat
    
    ...only to wake up in the middle of the night to a strange sound.
    Something like... soft chirping?
    You reach for the lamp, squinting in the sudden light.
    
    The kitten sits at the foot of your bed, his back perfectly straight, tail raised in alert attention. # show_cat # cat_1
    His eyes are fixed intently on the corner of the room, head moving slightly as if tracking something. # cat_0
    You look at the corner. Nothing's there - just the meeting point of two walls and the ceiling.
    
    He makes that strange chirping sound again, still staring at the empty corner. # cat_thin 
    
    + [Ask "What do you see?"]
        Your whispered question breaks his concentration. # cat_1
        He turns to you, blinks slowly, then curls up next to your feet as if nothing happened.
        You look at the corner one more time before turning off the light. #hide_cat
        -> Morning_2_peaceful
    + [Pretend you didn't see anything]
        You turn off the light quickly, trying not to think about what cats might see that we can't. #hide_cat
        He stays still for a few more minutes before finally settling down to sleep.
        -> Morning_2_peaceful
}
=== Night_2_chaos ===
You wake up at 3 AM to chaos.
Your phone has somehow made it to the floor.
The curtains are swaying despite all windows being closed.
The kitten sits innocently in the center of your chest, as if he had nothing to do with any of it. # show_cat # cat_wide
~ SetPlay(play + 20)
-> Morning_2

=== Bedtime_2 ===
You finally manage to fall asleep...
Only to wake up at 3 AM to the sound of your phone hitting the carpet.
The kitten sits on your nightstand, making direct eye contact. # show_cat # cat_wide
"Don't you dare," you whisper.
He slowly, deliberately, pushes your glass of water closer to the edge. # cat_thin
-> Morning_2

=== Morning_2_peaceful ===
~ SetPlay(play - 20)
~ SetPets(pets - 20)
~ SetFood(food - 20)
You wake up naturally, just before your alarm, feeling well-rested. # hide_cat
The kitten is still curled up at your feet, only stirring when he hears you move.
He greets you with a tiny, sleepy "mrrp". # show_cat # cat_1
-> END

=== Morning_2 ===
~ SetPlay(play - 40)
~ SetPets(pets - 40)
~ SetFood(food - 30)
Morning comes, and with it, the realization that having a kitten is like living with a tiny adorable poltergeist. # hide_cat
But when he curls up purring in your lap at breakfast, somehow it all seems worth it. # show_cat # cat_1
-> END
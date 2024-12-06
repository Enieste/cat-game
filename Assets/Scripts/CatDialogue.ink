=== function Pet ===
~ return

=== function Feed ===
~ return

=== function Play ===
~ return

=== function Idle ===
~ return

=== function PutOnFloor ===
~ return

=== function CandyWrapperPlay ===
~ return

EXTERNAL GetFood()
EXTERNAL SetFood(value)
EXTERNAL GetPlay()
EXTERNAL SetPlay(value)
EXTERNAL GetPets()
EXTERNAL SetPets(value)
EXTERNAL Pet()
EXTERNAL Feed()
EXTERNAL Idle()
EXTERNAL Play()
EXTERNAL PutOnFloor()
EXTERNAL CandyWrapperPlay()

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
    - currentPlay < 60 && currentPets < 60:
        The cat seems very unhappy, desperately trying to get your attention... #show_cat # cat_0
    - currentPets < 60:
        The cat looks at you longingly, wanting affection... #show_cat # cat_1
    - currentPlay < 60:
        The cat seems restless, looking for something to do... #show_cat # cat_0
    - currentFood < 60:
        The cat looks at you with hungry eyes and meows desperately... #show_cat # cat_0
    - else:
        The cat looks at you, content. #show_cat # cat_1
}

+ [Pet the cat]
    ~ Pet()
    {currentFood < 60:
        The cat purrs, but keeps looking at the food bowl... # cat_0
    - else:
        The cat purrs happily, enjoying your attention. # cat_1
    }
    -> END
+ [Play with cat]
    ~ Play()
    {currentFood < 60:
        The cat tries to play, but seems more interested in food right now. # cat_0
    - else:
        The cat excitedly chases after your fingers! # cat_thin
    }
    -> END
+ [Ignore]
    You leave the cat alone.
    -> END

=== food_bowl ===
~ temp currentFood = GetFood()
~ PutOnFloor()
The cat watches the food bowl intently. # cat_0
+ [Feed the cat]
    ~ Feed()
    You put some food into the bowl. The cat happily munches away.
    -> END
+ [Ignore]
    {currentFood < 60:
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
    Suddenly, you wake up, hearing sad meowing behind the door...
        + +  [Let the kitten in]
                    The kitten happily enters your room. Jumping onto your bed, he purrs and curls up next to you. # cat_1
                    ~ SetPets(pets + 30)
                            -> END
        ++  [Ignore it]
            You try to ignore the meowing...
                +++  [Put a pillow over your head]
                            ~ SetPets(pets - 20)
                            The sound persists, but become more distant as you drift off to sleep.
                            -> END
                +++  [Feel guilty]
                            After a few minutes of guilt, you get up and open the door.
                            The kitten triumphantly enters and claims his spot on your bed. # cat_1
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
                            It takes a long time, but eventually you fall asleep while the cat stays near the door.
                            ~ SetPets(pets - 20)
                            -> END
+ [Leave the door open]
    ~ SetPets(pets + 30)
    The kitten follows you to bed immediately... # show_cat # cat_1
    He purrs and curls up next to your pillow.
    You both sleep peacefully.
        -> END
        
=== night_1 ===
~ temp currentFood = GetFood()
~ temp currentPlay = GetPlay()
~ temp currentPets = GetPets()
~ PutOnFloor()
You feel exhausted after work. You've spent the whole evening trying to please your new furry roommate, and now sleep is the only thing on your mind. #hide_cat
You enter the bedroom and hear the soft patter of tiny paws behind you - the kitten is following you.
What will you do??
    + [Close the door behind you. Let him sleep on the couch.]
       You close the door right before he can get through, and then lie down on your bed.
        The pitiful meowing from the living room makes it hard to fall asleep.
            ++ [Ignore him. He needs to get used to this.]
            You try hard not to block out the sound. It barely helps - the quiet whimpering follows you even in your dreams.
            You wake up to your alarm and find that the kitten spent the night behind the couch in the living room.
            ~ SetPets (currentPets - 20)
            He emerges covered in dust and greets you with a demanding "meow". # show_cat # cat_1
            -> Morning_1
            ++ [Feel sorry and let him in.]
            You open the door, and the kitten darts inside like lightning, then rubs against your legs. # show_cat # cat_1
            You get back in bed while he carefully investigates the room. # cat_0
            In the darkness, you try to keep an eye on him for a while but quickly fall asleep. # cat_thin
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
The kitten sits on the bed, looking at you with innocent eyes. His pupils are so dilated you can barely see the iris. # show_cat # cat_wide
Looks like he decided to play with your feet. # cat_0
~ SetPlay (currentPlay + 10)
You exhale, turn off the light, and try to go back to sleep. #hide_cat
On the edge between sleep and wakefulness, only one thought buzzes in your head...
"How can such a tiny thing be so strong?"
When the annoying alarm finally wakes you, the kitten is sleeping sweetly at your feet.
-> Morning_1

=== Morning_1 ===
~ temp currentPlay = GetPlay()
~ temp currentPets = GetPets()
New day. You fill the bowl with extra food and head off to work.
In the evening, you hurry home to your little friend.
~ SetPlay (currentPlay - 50)
~ SetPets (currentPets - 50)
-> END
        
=== cat_couch === 
~ temp currentPlay = GetPlay()
The cat is comfortably lying on a surface.
    + [Pet the cat]
        ~ Pet()
        The cat purrs contentedly, enjoying the attention. # cat_1
        -> END
    + [Gently put him on the floor]
        ~ PutOnFloor()
        {currentPlay < 60:
             ~ SetPlay(play + 20)
            You manage to place him on the floor, but he tries to bite your fingers in a process. # cat_thin
        - else:
             ~ SetPets(pets + 20)
            The cat purrs as you put him on the floor. # cat_1
        }
        -> END
    + [He shouldn't be there. Shoo him away]
        ~ PutOnFloor()
        ~ SetPets(pets - 20)
        The cat gives you an annoyed look but jumps down from the surface. # cat_0
        -> END
        
=== beginning ==== 
# hide_cat
It was a long day. You're tired from work and your best friend couldn't join you for the evening. 
You're heading back to your lonely house, thinking about...
* [New TV series] Maybe you can check out that new show everyone's talking about.
* [A bottle of beer] You can already hear that cold beer in the fridge calling your name.
* [Playing a video game] You still need to beat that annoying boss. Blade of... what was his name again? 
- Yes. The evening probably won't be that bad after all.
Here's your house. And your porch. And your door. And your box. #show_cat # cat_inbox
    Wait a minute.... A box? You didn't order anything.
It's not Christmas and it's not your birthday. Maybe you should call the police?
Suddenly the box cries out in a thin voice. And then you hear scratching. Before you have a chance to react... It opens. 
A cat! Somebody left a box with a cat on your porch. # cat_box
-> box_action


=== box_action ===
What do you want to do?
* [Look for a person who left the box] You look around — the street is empty. That person is probably already far away. -> box_action
* [Leave the box outside and proceed with the initial plan] Well... That's not really an option here. We still have a game to play, don't we? -> box_action
* [Take the box home] You have a cat now. -> END   


=== night_2 ===
~ temp currentFood = GetFood()
~ temp currentPlay = GetPlay()
~ temp currentPets = GetPets()
You're already in bed, ready for sleep. The house is quiet... perhaps too quiet. #hide_cat

{
    - currentFood < 30:
    Just as you're about to drift off, a long, haunting yowl echoes through the apartment.
    You look up to see the kitten sitting in your bedroom doorway, staring at you intensely. # show_cat # cat_wide
        + [Get up and check his bowl]
            You drag yourself out of the warm bed. # cat_1
            His bowl isn't completely empty, but you can see the bottom. # cat_0
            He lets out another soul-piercing meow. # cat_thin
            ++ [Add more food]
                ~ SetFood(currentFood + 40)
                You add a small scoop. He trills happily and trots off to eat.
                When you return to bed, he joins you, purring contentedly. # cat_1
                -> Bedtime_2
            ++ [Go back to bed]
                You return to bed. Throughout the night, your dreams are haunted by increasingly creative vocalizations. # hide_cat
                -> Bedtime_2
                
    - currentPlay < 40:
    Sudden rapid thumping breaks the silence.
    You sit up in bed and see a dark blur racing around your room - from the dresser to the windowsill, then onto your desk.
    The kitten suddenly jumps on your bed, eyes wide and glowing in the dark, his tail puffed up like a bottle brush. # show_cat # cat_wide
        + [Try to calm him down with pets]
            ~ SetPets(currentPets + 10)
            ~ SetPlay(currentPlay + 15)
            At least he's gentler now than during your first night together. # cat_1
            -> Bedtime_2
        + [Hide under the blanket]
            ~ SetPlay(currentPlay - 10)
            You pull the covers over your head. Is this a good idea?..
            -> Night_2_chaos           
            
    - currentPets < 40:
    Trying to get comfortable, you feel like... you're being watched.
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
    
    He makes that strange chirping sound again, not taking his eyes off the empty corner. # cat_thin 
    
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
You wake up at 3 AM to complete chaos.
Your phone has somehow made it to the floor.
The curtains are swaying despite all windows being closed.
The kitten sits innocently on the center of your chest, as if he had nothing to do with any of it. # show_cat # cat_wide
~ SetPlay(play + 20)
-> Morning_2

=== Bedtime_2 ===
You finally manage to fall asleep...
Only to wake up at 3 AM to the sound of your phone hitting the carpet.
The kitten sits on your nightstand, making direct eye contact. # show_cat # cat_wide
"Don't you dare," you whisper.
He slowly, deliberately, pushes your glass of water closer to the edge. # cat_thin
~ SetPets(pets + 20)
-> Morning_2

=== Morning_2_peaceful ===
You wake up naturally, just before your alarm, feeling well-rested. # hide_cat
The kitten is still curled up at your feet, only stirring when he hears you move.
He greets you with a tiny, sleepy "mrrp". # show_cat # cat_1
~ SetPlay(play - 20)
~ SetPets(pets - 20)
~ SetFood(food - 20)
-> demo_finish

=== Morning_2 ===
Morning comes, and with it, the realization that having a kitten is like living with a tiny adorable poltergeist. # hide_cat
But when he curls up purring in your lap at breakfast, somehow it all seems worth it. # show_cat # cat_1
~ SetPlay(play - 20)
~ SetPets(pets - 20)
~ SetFood(food - 20)
-> demo_finish

=== demo_finish ===
That's all for the game so far. Thank you for playing this demo! 
I'll be updating it soon with more days to play through, more events to discover, and of course, more of our furry friend! =)
-> END

=== toy_under_couch ===
You notice the kitten pawing desperately at something under the couch. #show_cat #cat_thin
+ [Ignore him]
    Cats will be cats. Nothing worth watching.
    -> END
+ [Check it]
    You peer under the couch and spot a candy wrapper. When you pull it out, the kitten's eyes light up like you've discovered buried treasure. #cat_wide
    ++ [Play with the wrapper]
        ~ CandyWrapperPlay()
        You toss it across the floor. He pounces on it with pure joy. #cat_thin
        The next few minutes are spent throwing the wrapper, watching him chase it down, and throwing it again.
        Eventually, he bats it back under the couch himself and wanders off. Mission accomplished. #cat_0
        -> END
    ++ [Throw it away]
        The kitten watches mournfully as his newfound shiny toy disappears into the kitchen trash. #cat_1
        Sorry little friend, but a clean house is more important than temporary entertainment. #cat_0
        -> END
        
LIST TVShows = (documentary_dogs), (favorite_show), (kids_cartoon), (old_movie), (bird_documentary), (soap_opera), (true_crime)

VAR currentShow = ""

=== function pickRandomShow() ===
~ return LIST_RANDOM(TVShows)

=== function getShowDescription(show) ===
{
    - show == documentary_dogs: ~ return "a documentary about dogs"
    - show == favorite_show: ~ return "a new episode of your favorite show"
    - show == kids_cartoon: ~ return "some bizarre, hypnotic children's cartoon"
    - show == old_movie: ~ return "an old movie that's somehow always on TV"
    - show == bird_documentary: ~ return "a nature documentary about birds that makes your kitten chirp"
    - show == soap_opera: ~ return "a dramatic soap opera that makes no sense"
    - show == true_crime: ~ return "a true crime documentary that's strangely satisfying"
    - else: ~ return "something on TV"
}

=== watch_tv ===
You look at your TV... Maybe watch something for a while? # hide_cat
    + [Turn it on]
        ~ Idle()
        ~ currentShow = pickRandomShow()
        You prepare some popcorn and watch {getShowDescription(currentShow)}.
         {
            - food < 60:
            You hear a long, demanding meow and reluctantly turn off the TV. 
            Someone needs dinner more than you need entertainment. # show_cat # cat_0
            - play < 60:
            The kitten keeps trying to catch everything moving on screen. # show_cat # cat_thin
            Better turn off the TV before he knocks it over in his excitement. 
            - pets < 60:
            The kitten jumps into your lap, purring and rubbing against your cheek. # show_cat # cat_1
            Hard to focus on the show when someone is so desperately seeking attention. You give up and turn the TV off.
            - else:
            It was a good one. Now, where did that cat go?..
        }
        -> END
    + [Leave it]
        You've got other things to do. Now, where did that cat go?..
        -> END
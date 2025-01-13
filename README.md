# Gambit VR
Play as X-Men Gambit with powerful card skills! Learn to toss card with accuracy, control your cards with homing abilities and even throw many cards at once! The fight with the sentinels continues!

[![Gambit VR for XR Challenge - Video Demonstration](https://github.com/user-attachments/assets/0af69a96-c6be-41ca-90e1-280995cb1ad4)](https://www.youtube.com/watch?v=S1Osihe4oT4&t=12s)
_(Click on the image to watch a video demonstration!)_

## Inspiration
Gambit in X-men is a cool character that is underrepresented in X-men series. He's throwing skill and fighting skill is easily replicated for hobby and combat experience. Hence, this game is designed to let players become Gambit and have cool card throwing skills and special abilities to fight the Sentinels

## What it does
Player is able to spawn a card with two fingers snap together just like real card players, and throw the card by flicking it out. Other skills include snapping the cards to float in air to store up a few card, and at once they can be directed to enemy by touching them. Last ability allows player to charge up the power of a single card in hand before throwing. A charge card can be controlled by the player after shooting to aim at different enemies.

## How we built it
This was built using the Unity game engine and OpenXR. The throwing was implemented by detecting the hand gestures and the speed of the hand while releasing the card. When a gesture is detected, it triggers an event that would inform the scripts controlling the cards. The card and hand gestures are controlled using a Finite State Machine (FSM), which uses transitions between states such as "Holding the Card" and "Releasing the Card" to decide what to do at each state. We also utilized programs such as Blender and Photoshop for the aesthetics side of the program. Various animations for the game was done manually on Blender, utilizing its Animation system and its Graph Editor to create and fine-tune each part of the animations clip, ensuring it is smooth and nice.

## Challenges we ran into
The main challenge we ran into was working with VR, this was the first time our team worked with a Meta Quest headset, and we had to do a lot of research to make this work. We also had a lot of issues with detecting the gestures, as we had to figure out how to ensure the actions feel fluid for the player despite difficulties with detecting the gesture while moving the hand at high speeds. Other challenges we ran into was related to the effects for the card. Since we utilized Unity's built-in Particle System, we were restricted to only the settings available in the component. It was difficult to create an effect that fit our vision for the game.

## Accomplishments that we're proud of
Similar to the challenges, one accomplishments we're proud of is the effects. While yes, it was difficult to work with the particle system, the end product of all of our efforts makes the gestures for the cards feel more alive, and adds on to the overall immersion of the game. Some other accomplishments we are proud of is getting the card to follow the rotation of the finger correctly, as well as using a delay to allow the headset to detect certain gestures more easily.

## What we learned
During our time doing this challenge, we've gained a better understanding on how a XR game is developed. From how to set up the proper environment to making Unity detect our hands while using the Meta headset.

## What's next for Gambit VR
We hope to develop this further by designing a short 5 - 10 minutes level game. This helps us gain better understanding on the reception of this game concept before further planning a large scale project.

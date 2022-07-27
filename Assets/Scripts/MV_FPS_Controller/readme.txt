# MV FPS Controller for Unity

### Initial setup 1 (with prefab)
1. Drag MV_FPS_Controller/Player prefab on your scene
2. Set preferable configuration
3. Note that for valid material depending sounds you should use your tags in audio configuration
4. If there are moving platforms, put MovingPlatformSupport script on them (this script only tells player platform motion 
and does not move platforms) and add trigger collider (MovingPlatformSupport processes motion when player is in trigger zone)
5. Player's layer should not be contained in Player/Config/Touch/LayerMask!
6. Enjoy controls!

### Initial setup 2 (with scripts)
1. Put MV_FPS_Controller/Scripts/Player/Player.cs script on your character
2. Drag your Camera into Player script
3. Set preferable player configuration
4. Player's layer should not be contained in Player/Config/Touch/LayerMask!

6. Put MV_FPS_Controller/Scripts/Inputs/PlayerInput.cs script on your character
7. Set preferable input configuration
8. Drag Player script into Input Receiver field of PlayerInput script
8. Or implement your own input controller: to pass controls to Player script you need to address 
it as MV_FPS_Controller/Scripts/Inputs/InputReceiver.cs script - Player extends this abstract class 
(gameObject.GetComponent<InputReceiver>() or via serializable field of InputReceiver type). 

9. Add MV_FPS_Controller/Scripts/Animation/PlayerAnimator.cs, MV_FPS_Controller/Scripts/Audio/PlayerAudio.cs scripts 
on your character and drag them into corresponding Player script fields. 
These scripts extend MV_FPS_Controller/Scripts/Player/PlayerActionsCallback.cs abstract class.
9. Or implement your own animator and audio scripts by extending PlayerActionsCallback abstract class.
10. Drag your Camera into PlayerAnimator script
11. Set up audio sources for PlayerAudio script (head audio source should be in child of your camera game object or in it)
12. Set tags for audio materials
13. Set preferable audio and animation configuration
14. Add your script that extends PlayerActionsCallback class into PlayerActionsCallback field of Player script to know about 
all player actions (optional).

15. If there are moving platforms, put MovingPlatformSupport script on them (this script only tells player platform motion 
and does not move platforms) and add trigger collider (MovingPlatformSupport processes motion when player is in trigger zone)
16. Enjoy controls!

See configuration setup in ConfigSetup.pdf
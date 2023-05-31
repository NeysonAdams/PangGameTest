#Pang Game

- This product was developed in Unity v.2021.3.26f1 according to the requirement

## Architecture
The architecture is an MVC model
Main 4 models:
- Player - stores information about the player and the game character, such as movement speed, points, number of available shots, etc.
- BallModel - stores information about bouncing balls (size, weight, rebound force, etc.)
- WeaponModel - stores information about the character's weapon (flying speed, player id)
- LevelModel - level information (number of balls and players)\

Views
Game Views inherit from the GameObjectView class. The main idea of this class is that it has 5 main delegates that are launched in the corresponding methods of the extensible MonoBehaviour (Start, Update, etc.)
     public Action<Models> OnUpdate; - at Update
     public Action<GameObjectView> OnStart; - at Start
     public Action<GameObjectView> OnDead; - When OnDestroy
     public Action<GameObjectView, GameObject> OnCollision; - at OnCollisionEnter2D
     public Action<GameObjectView> OnAwake; - with Awake

this approach allows you to control the View from classes that do not extend MonoBehaviour
game objects use RidgitBody2D to implement animation and physics

Controllers are not MonoBehaviour, which eliminates the overhead associated with MonoBehaviour and simplifies testing. Basically there are 3 main controllers
- GameplayController manages the gameplay and also provides interaction between the player and the game
- LevelCreatecontoller - Creates, modifies and optionally loads level information
- GameUiController - manages ui objects like score counter, main menu, game over screen


Also note that for this project I have developed two shaders.
SphereUI - shader for rendering balls
ZShape2D - wavy line for player weapon
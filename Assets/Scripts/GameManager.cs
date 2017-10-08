using UnityEngine;
using System.Collections;
using System.Diagnostics;

namespace Completed
{
	using System.Collections.Generic;		//Allows us to use Lists. 
	using UnityEngine.UI;					//Allows us to use UI.
	
	public class GameManager : MonoBehaviour
	{
		public float levelStartDelay = 2f;						//Time to wait before starting level, in seconds.
		public float turnDelay = 0.0f;							//Delay between each Player turn.
		public int playerHealthPoints = 100;					//Starting value for Player health points.
		public static GameManager instance = null;				//Static instance of GameManager which allows it to be accessed by any other script.
		[HideInInspector] public bool playersTurn = true;       //Boolean to check if it's players turn, hidden in inspector but public.
        private GameObject playerRef;
        private Stopwatch turnTimer = new Stopwatch();          // Move the enemies if the player is just standing still.
		
		private Text levelText;									//Text to display current level number.
		private GameObject levelImage;							//Image to block out level as levels are being set up, background for levelText.
		private BoardCreator boardScript;						//Store a reference to our BoardManager which will set up the level.
		private int level = 0;									//Current level number, expressed in game as "Level 1".
		private List<Enemy> enemies;							//List of all Enemy units, used to issue them move commands.
		private bool enemiesMoving;								//Boolean to check if enemies are moving.
		private bool doingSetup = true;							//Boolean to check if we're setting up board, prevent Player from moving during setup.
		
		
		
		//Awake is always called before any Start functions
		void Awake()
		{
            turnTimer.Start();

			//Check if instance already exists
			if (instance == null)
				
				//if not, set instance to this
				instance = this;
			
			//If instance already exists and it's not this:
			else if (instance != this)
				
				//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
				Destroy(gameObject);	
			
			//Sets this to not be destroyed when reloading scene
			DontDestroyOnLoad(gameObject);
			
			//Assign enemies to a new List of Enemy objects.
			enemies = new List<Enemy>();
			
			//Get a component reference to the attached BoardManager script
			boardScript = GetComponent<BoardCreator>();

            //Call the InitGame function to initialize the first level - Only needed to test the game straight from the main game scene. 
            //If loaded from the main menu screen this is no longer necessary.
			//InitGame();
		}
		
		//This is called each time a scene is loaded.
		public void OnLevelWasLoaded()
		{
			//Add one to our level number.
			level++;
			//Call InitGame to initialize our level.
			InitGame();
		}
		
		//Initializes the game for each level.
		public void InitGame()
		{
			//While doingSetup is true the player can't move, prevent player from moving while title card is up.
			doingSetup = true;
			
			//Get a reference to our image LevelImage by finding it by name.
			if (levelImage == null) levelImage = GameObject.Find("LevelImage");

            //Set levelImage to active blocking player's view of the game board during setup.
            levelImage.SetActive(true);

            //Get a reference to our text LevelText's text component by finding it by name and calling GetComponent.
            levelText = GameObject.Find("LevelText").GetComponent<Text>();
			
			//Set the text of levelText to the string "Day" and append the current level number.
			levelText.text = "Level " + level;
			
			//Set levelImage to active blocking player's view of the game board during setup.
			levelImage.SetActive(true);
			
			//Call the HideLevelImage function with a delay in seconds of levelStartDelay.
			Invoke("HideLevelImage", levelStartDelay);
			
			//Clear any Enemy objects in our List to prepare for next level.
			enemies.Clear();
			
			//Call the SetupScene function of the BoardManager script, pass it current level number.
			boardScript.Restart(level);
			
		}
		
		
		//Hides black image used between levels
		void HideLevelImage()
		{
			//Disable the levelImage gameObject.
			levelImage.SetActive(false);
			
			//Set doingSetup to false allowing player to move again.
			doingSetup = false;
		}
		
		//Update is called every frame.
		void Update()
		{
            if(turnTimer.Elapsed.TotalSeconds > 10f && doingSetup == false)
            {

            }

			//Check that playersTurn or enemiesMoving or doingSetup are not currently true.
			else if(playersTurn || enemiesMoving || doingSetup)
				
				//If any of these are true, return and do not start MoveEnemies.
				return;
			
			//Start moving enemies.
			StartCoroutine (MoveEnemies ());
		}
		
		//Call this to add the passed in Enemy to the List of Enemy objects.
		public void AddEnemyToList(Enemy script)
		{
			//Add Enemy to List enemies.
			enemies.Add(script);
		}

        public void RemoveEnemyFromList(Enemy script)
        {
            enemies.Remove(script);
        }


        //GameOver is called when the player reaches 0 health points
        public void GameOver()
		{
			//Set levelText to display number of levels passed and game over message
			levelText.text = "After " + level + " floors, you died.";
			
			//Enable black background image gameObject.
			levelImage.SetActive(true);
			
			//Disable this GameManager.
			enabled = false;
		}
		
		//Coroutine to move enemies in sequence.
		IEnumerator MoveEnemies()
		{
            turnTimer.Stop();
            turnTimer.Reset();
            playerRef = GameObject.Find("Player");
			//While enemiesMoving is true player is unable to move.
			enemiesMoving = true;
			
			//Wait for turnDelay seconds, defaults to .1 (100 ms).
			//yield return new WaitForSeconds(turnDelay);
			
			//If there are no enemies spawned (IE in first level):
			//if (enemies.Count == 0) 
			//{
			//	  Wait for turnDelay seconds between moves, replaces delay caused by enemies moving when there are none.
			//	  yield return new WaitForSeconds(turnDelay);
			//}
			
			//Loop through List of Enemy objects.
			for (int i = 0; i < enemies.Count; i++)
			{

                if (Vector3.Distance(playerRef.transform.position, enemies[i].gameObject.transform.position) < 20)
                {

                    //Call the MoveEnemy function of Enemy at index i in the enemies List.
                    enemies[i].MoveEnemy();
                }

                //Wait for Enemy's moveTime before moving next Enemy, 
                //yield return new WaitForSeconds(enemies[i].moveTime);
                yield return null;
			}
			//Once Enemies are done moving, set playersTurn to true so player can move.
			playersTurn = true;
			
			//Enemies are done moving, set enemiesMoving to false.
			enemiesMoving = false;
            turnTimer.Start();
		}
	}
}


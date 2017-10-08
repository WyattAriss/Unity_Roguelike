using UnityEngine;
using System.Collections;
using UnityEngine.UI;	//Allows us to use UI.
using Random = UnityEngine.Random;

namespace Completed
{
	//Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
	public class Player : MovingObject
	{
		public float restartLevelDelay = 1f;		//Delay time in seconds to restart level.
		public int pointsPerHealth = 5;			    //Number of points to add to player health points when picking up a health object.
		public int pointsPerSoda = 20;				//Number of points to add to player health points when picking up a soda object.
        [HideInInspector]public int playerMaxHealth = 100;           //Players max health.
        [HideInInspector]public int health;							//Used to store player health points total during level.
        [HideInInspector]public int mana;
        [HideInInspector]public int stamina;
        [HideInInspector]public int playerArmor = 0;                 //Players max armor.
		[HideInInspector]public int playerDamage = 1;                //How much damage a player does.
        [HideInInspector]public int playerMaxStamina = 10;              //How much stamina the player has. Used for skills.
        [HideInInspector]public int playerMaxMana = 10;                 //How much mana the player has. Used for spells.
        [HideInInspector]public int playerXP = 0;                    //The players current experience points. Used for determining levelup.
        [HideInInspector]public int playerLevel = 1;                 //The players level.
        [HideInInspector]public int xpRequired;                      //Amount of xp required before levelup occurs.
        [HideInInspector]private bool levelUp = false;               //To check for levelup.
        [HideInInspector]private bool isCasting = false;

        AbilityUse abilityUseRef;
        PlayerInfo playerInfoRef;

        private string playerName;
        [HideInInspector] public int playerClass;                    //1 = Warrior, 2 = Rogue, 3 = Sorcerer
        [HideInInspector] public int playerStrength;                 //Add Strength - 10 to damage
        [HideInInspector] public int playerDexterity;                // Skills cost dex-10 / 2 less stamina to use
        [HideInInspector] public int playerIntelligence;             //Some powerful magic items require some intelligence to use.
        [HideInInspector] public int playerVitality;                 //Amount of max health gained per level up
        [HideInInspector] public int playerWillpower;                //Levelup grants vitality + (1d4) + (Willpower - 10)/2, also grants flat stamina gain

        public AudioClip inventorySound1;           //1 of 2 clips to play when inventory is opened or closed.
        public AudioClip inventorySound2;           //2 of 2 clips to play when inventory is opened or closed.
        public AudioClip moveSound1;				//1 of 2 Audio clips to play when player moves.
		public AudioClip moveSound2;				//2 of 2 Audio clips to play when player moves.
		public AudioClip eatSound1;					//1 of 2 Audio clips to play when player collects a health object.
		public AudioClip eatSound2;					//2 of 2 Audio clips to play when player collects a health object.
		public AudioClip drinkSound1;				//1 of 2 Audio clips to play when player collects a soda object.
		public AudioClip drinkSound2;				//2 of 2 Audio clips to play when player collects a soda object.
		public AudioClip gameOverSound;				//Audio clip to play when player dies.
        private int levelCounter = 1;               //Level Counter.

        //Stat screen player character sheet texts.
        public Text nameText;
        public Text classText;
        public Text levelText;
        public Text attributesText;                 //Update after level up is completed.
        public Text statsText;
        public Text xpText;                         //Update after an enemy is slain.

        public Text healthText;                     //Display life above the health globe.
        private Text levelText2;					//Text to display the next level number.
        public GameObject gameManager;			    //GameManager prefab to instantiate.
        private GameObject levelImage2;				//Image to block out level as levels are being set up, background for levelText.
        private GameObject temp1;                   //Temporary game object holder.

        PlayerInventory PlayerInventoryReference;
        int Temp;                                   //Temporary int storage.
		
		private Animator animator;					//Used to store a reference to the Player's animator component.
		private Vector2 touchOrigin = -Vector2.one; //Used to store location of screen touch origin for mobile controls.

        //public static Player pInstance = null;               //Static instance of Inventory which allows it to be accessed by any other script.

        void SetBaseCharacter()                     
        {
            GameObject playerInfoGameObject = GameObject.FindGameObjectWithTag("Player Info");
            playerInfoRef = playerInfoGameObject.GetComponent<PlayerInfo>();
            playerName = playerInfoRef.charName;
            playerClass = playerInfoRef.charClass;                                              //1 = Warrior, 2 = Rogue, 3 = Sorcerer
            playerStrength = playerInfoRef.charStrength;
            playerDexterity = playerInfoRef.charDexterity;
            playerIntelligence = playerInfoRef.charIntelligence;
            playerVitality = playerInfoRef.charVitality;
            playerWillpower = playerInfoRef.charWillpower;

            playerMaxHealth = playerVitality + Random.Range(1, 5) + ((playerWillpower - 10) / 2);
            playerMaxMana = playerIntelligence + Random.Range(1, 5) + ((playerWillpower - 10) / 2);
            playerMaxStamina = playerWillpower + Random.Range(1, 5);

            nameText.text = playerName;
            if (playerClass == 1)
                classText.text = "Warrior";
            else if (playerClass == 2)
                classText.text = "Rogue";
            else if (playerClass == 3)
                classText.text = "Sorcerer";
            levelText.text = "Level: " + playerLevel;

            xpText.text = "" + playerXP;

            //Update attribues after levelup completes.
            attributesText.text = "" + playerStrength + System.Environment.NewLine + playerDexterity + System.Environment.NewLine + playerIntelligence + System.Environment.NewLine + playerVitality + System.Environment.NewLine  + playerWillpower;

            /// Dont destroy so that we can copy stats into player info on level up and acess them elsewhere.
            DestroyObject(playerInfoGameObject);
        }

        //Start overrides the Start function of MovingObject
        protected override void Start ()
		{

            SetBaseCharacter();

            /*
            //Check if pInstance already exists
            if (pInstance == null)

                //if not, set pInstance to this
                pInstance = this;

            //If pInstance already exists and it's not this:
            else if (pInstance != this)

                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of the player.
                Destroy(gameObject);

            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);
            */

            //Get a component reference to the Player's animator component
            animator = GetComponent<Animator>();
			
			health = playerMaxHealth;
            mana = playerMaxMana;
            stamina = playerMaxStamina;

            healthText.text = "Life: " + health;
			
			//Call the Start function of the MovingObject base class.
			base.Start ();
		}
		
		
		//This function is called when the behaviour becomes disabled or inactive.
		private void OnDisable ()
		{
			//When Player object is disabled, store the current local health total in the GameManager so it can be re-loaded in next level.
			GameManager.instance.playerHealthPoints = health;
		}
		
		
		private void Update ()
		{

            abilityUseRef = this.GetComponent<AbilityUse>();
            isCasting = abilityUseRef.isCasting;

            PlayerInventoryReference = this.GetComponent<PlayerInventory>();
            if (PlayerInventoryReference.healthGained != 0)
            {
                health += PlayerInventoryReference.healthGained;
                if (health > playerMaxHealth) health = playerMaxHealth;
                PlayerInventoryReference.healthGained = 0;
            }
            if (PlayerInventoryReference.manaGained != 0)
            {
                mana += PlayerInventoryReference.manaGained;
                if (mana > playerMaxMana) mana = playerMaxMana;
                PlayerInventoryReference.manaGained = 0;
            }


            //Play inventory sound when inventory or equipment panels are opened or closed. And update character sheet.
            if (Input.GetKeyDown("c") || Input.GetKeyDown("i"))
            {
                SoundManager.instance.RandomizeSfx(inventorySound1, inventorySound2);
            }

            playerArmor = PlayerInventoryReference.maxArmor;
            int tempMin = playerDamage + PlayerInventoryReference.minDamage;
            int tempMax = playerDamage + PlayerInventoryReference.maxDamage;

            statsText.text = "" + health + "/" + playerMaxHealth + System.Environment.NewLine + mana + "/" + playerMaxMana + System.Environment.NewLine + stamina + "/" + playerMaxStamina + System.Environment.NewLine + playerArmor + System.Environment.NewLine + tempMin + " - " + tempMax;

            healthText.text = "Life: " + health;

            //If it's not the player's turn, exit the function.
            if (!GameManager.instance.playersTurn) return;
            //Check to see if the player is trying to cast a spell.
            if (!isCasting)
            {
                int horizontal = 0;     //Used to store the horizontal move direction.
                int vertical = 0;       //Used to store the vertical move direction.


                //Check if we are running either in the Unity editor or in a standalone build.
                #if UNITY_STANDALONE || UNITY_WEBPLAYER

                //Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
                horizontal = (int)(Input.GetAxisRaw("Horizontal"));

                //Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
                vertical = (int)(Input.GetAxisRaw("Vertical"));

                //Check if moving horizontally, if so set vertical to zero.
                if (horizontal != 0)
                {
                    vertical = 0;
                }
                //Check if we are running on iOS, Android, Windows Phone 8 or Unity iPhone
                #elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
			
			//Check if Input has registered more than zero touches
			if (Input.touchCount > 0)
			{
				//Store the first touch detected.
				Touch myTouch = Input.touches[0];
				
				//Check if the phase of that touch equals Began
				if (myTouch.phase == TouchPhase.Began)
				{
					//If so, set touchOrigin to the position of that touch
					touchOrigin = myTouch.position;
				}
				
				//If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
				else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
				{
					//Set touchEnd to equal the position of this touch
					Vector2 touchEnd = myTouch.position;
					
					//Calculate the difference between the beginning and end of the touch on the x axis.
					float x = touchEnd.x - touchOrigin.x;
					
					//Calculate the difference between the beginning and end of the touch on the y axis.
					float y = touchEnd.y - touchOrigin.y;
					
					//Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
					touchOrigin.x = -1;
					
					//Check if the difference along the x axis is greater than the difference along the y axis.
					if (Mathf.Abs(x) > Mathf.Abs(y))
						//If x is greater than zero, set horizontal to 1, otherwise set it to -1
						horizontal = x > 0 ? 1 : -1;
					else
						//If y is greater than zero, set horizontal to 1, otherwise set it to -1
						vertical = y > 0 ? 1 : -1;
				}
			}
			
                #endif //End of mobile platform dependendent compilation section started above with #elif

                //Check if we have a non-zero value for horizontal or vertical
                if (horizontal != 0 || vertical != 0)
                {
                    //Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
                    //Pass in horizontal and vertical as parameters to specify the direction to move Player in.
                    bool b = AttemptMove<Wall>(horizontal, vertical);
                    if (!b) AttemptMove<Enemy>(horizontal, vertical);

                }
            }
		}
		
		//AttemptMove overrides the AttemptMove function in the base class MovingObject
		//AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
		protected override bool AttemptMove <T> (int xDir, int yDir)
		{
			
			
			//Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
			bool b = base.AttemptMove <T> (xDir, yDir);
			
			//Hit allows us to reference the result of the Linecast done in Move.
			RaycastHit2D hit;

            //If Move returns true, meaning Player was able to move into an empty space.
            if (Move(xDir, yDir, out hit)) 
			{
				//Call RandomizeSfx of SoundManager to play the move sound, passing in two audio clips to choose from.
				SoundManager.instance.RandomizeSfx (moveSound1, moveSound2);
			}
			
			//Since the player has moved and lost health points, check if the game has ended.
			CheckIfGameOver ();
			
			//Set the playersTurn boolean of GameManager to false now that players turn is over.
			GameManager.instance.playersTurn = false;

            return b;
		}


        //OnCantMove overrides the abstract function OnCantMove in MovingObject.
        //It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy.
        protected override void OnCantMove <T> (T component)
		{
            int tempDamage = 0;
            PlayerInventoryReference = this.GetComponent<PlayerInventory>();   //Allows us to use stats from items.

            tempDamage = playerDamage + Random.Range(PlayerInventoryReference.minDamage, PlayerInventoryReference.maxDamage + 1);

            if (component is Wall)
            {
                //Set hitWall to equal the component passed in as a parameter.
                Wall hitWall = component as Wall;

                //Call the DamageWall function of the Wall we are hitting.
                hitWall.DamageWall(tempDamage);

                //Set the attack trigger of the player's animation controller in order to play the player's attack animation.
                animator.SetTrigger("playerChop");
            }
            else
            {
                //Set hitEnemy to equal the component passed in as a parameter.
                Enemy hitEnemy = component as Enemy;

                //Call the DamageEnemy function of the enemy we are hitting.
                hitEnemy.DamageEnemy(tempDamage);

                //Set the attack trigger of the player's animation controller in order to play the player's attack animation.
                animator.SetTrigger("playerChop");

                //Update the character sheet XP text.
                xpText.text = "" + playerXP;
            }
          
		}


        //OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
        private void OnTriggerEnter2D (Collider2D other)
		{
			//Check if the tag of the trigger collided with is Exit.
			if(other.tag == "Exit")
			{

				//Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
				Invoke ("Restart", restartLevelDelay);
				
			}
			
			//Check if the tag of the trigger collided with is health.
			else if(other.tag == "Health")
			{
				//Add pointsPerHealth to the players current health total.
				health += pointsPerHealth;

                //Make sure the player cannot gain health over his maximum health.
                if (health > playerMaxHealth) health = playerMaxHealth;
				
				//Update healthText to represent current total and notify player that they gained points
				healthText.text = "+" + pointsPerHealth + " Life: " + health;
				
				//Call the RandomizeSfx function of SoundManager and pass in two eating sounds to choose between to play the eating sound effect.
				SoundManager.instance.RandomizeSfx (eatSound1, eatSound2);
				
				//Disable the health object the player collided with.
				other.gameObject.SetActive (false);
			}
			
			//Check if the tag of the trigger collided with is Soda.
			else if(other.tag == "Soda")
			{
				//Add pointsPerSoda to players health points total
				health += pointsPerSoda;

                //Make sure the player cannot gain health over his maximum health.
                if (health > playerMaxHealth) health = playerMaxHealth;

                //Update healthText to represent current total and notify player that they gained points
                healthText.text = "+" + pointsPerSoda + " Life: " + health;
				
				//Call the RandomizeSfx function of SoundManager and pass in two drinking sounds to choose between to play the drinking sound effect.
				SoundManager.instance.RandomizeSfx (drinkSound1, drinkSound2);
				
				//Disable the soda object the player collided with.
				other.gameObject.SetActive (false);
			}
		}
		
		
		//Restart reloads the scene when called.
		private void Restart ()
		{
            transform.position = new Vector3(0, 0, 0); //Move player back to start position

            /*
            //Load the last scene loaded, in this case Main, the only scene in the game.
            Application.LoadLevel (Application.loadedLevel);
            */

            temp1 = GameObject.Find("Board");
            UnityEngine.Object.Destroy(temp1);
            temp1 = GameObject.Find("Exit(Clone)");
            UnityEngine.Object.Destroy(temp1);
            temp1 = GameObject.Find("BoardHolder");
            UnityEngine.Object.Destroy(temp1);

            GameManager.instance.OnLevelWasLoaded ();

            /*
            levelCounter += 1;
            //Get a reference to our image LevelImage by finding it by name.
            levelImage2 = GameObject.Find("LevelImage");
            //Enable the levelImage gameObject.
            levelImage2.SetActive(true);
            //Get a reference to our text LevelText's text component by finding it by name and calling GetComponent.
            levelText2 = GameObject.Find("LevelText").GetComponent<Text>();
            //Set the text of levelText to the string "Day" and append the current level number.
            levelText2.text = "Level " + levelCounter;
            */

		}
		
		
		//Losehealth is called when an enemy attacks the player.
		//It takes a parameter loss which specifies how many points to lose.
		public void LoseHealth (int loss)
		{

            //PlayerInventoryReference = this.GetComponent<PlayerInventory>();   //Allows us to use stats from items.

            //playerArmor = PlayerInventoryReference.maxArmor;

            //Armor effect formula - make this a better formula.
            playerArmor /= 4;

            //Set the trigger for the player animator to transition to the playerHit animation.
            animator.SetTrigger ("playerHit");

            //Damage reduction calculation.
            int damageTaken;
            damageTaken = loss - playerArmor;
            if (damageTaken < 1) damageTaken = 1;

			//Subtract lost health points from the players total.
			health -= damageTaken;
			
			//Update the health display with the new total.
			//healthText.text = "-"+ loss + " health: " + health;
			
			//Check to see if game has ended.
			CheckIfGameOver ();
		}
		
		
		//CheckIfGameOver checks if the player is out of health points and if so, ends the game.
		private void CheckIfGameOver ()
		{
			//Check if health point total is less than or equal to zero.
			if (health <= 0) 
			{
				//Call the PlaySingle function of SoundManager and pass it the gameOverSound as the audio clip to play.
				SoundManager.instance.PlaySingle (gameOverSound);
				
				//Stop the background music.
				SoundManager.instance.musicSource.Stop();
				
				//Call the GameOver function of GameManager.
				GameManager.instance.GameOver ();
			}
		}
	}
}


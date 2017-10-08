using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

namespace Completed
{
	//Enemy inherits from MovingObject, our base class for objects that can move, Player also inherits from this.
	public class Enemy : MovingObject
	{
        public int enemyMinDamage; 							//The min amount of health points to subtract from the player when attacking.
        public int enemyMaxDamage; 							//The max amount of health points to subtract from the player when attacking.
        private int enemyDamage;                            //Actual damage the enemy deals after random calculations.
        public int enemyHealth;                             //The amount of health points the enemy has.
        public int enemyLevel;                              //Use to make sure weaker and stronger enemies spawn on appropriate levels.
        public int enemyXP;                                 //The amount of experience the player gains after killing this enemy.
		public AudioClip attackSound1;						//First of two audio clips to play when attacking the player.
		public AudioClip attackSound2;						//Second of two audio clips to play when attacking the player.
        public AudioClip chopSound1;                        //1 of 2 audio clips that play when the enemy is attacked by the player.
        public AudioClip chopSound2;                        //2 of 2 audio clips that play when the enemy is attacked by the player.
        public int percentChanceToDropItem = 100;           //Set on enemy prefab in inspector.


        private Animator animator;							//Variable of type Animator to store a reference to the enemy's Animator component.
		private Transform target;							//Transform to attempt to move toward each turn.
		private bool skipMove;                              //Boolean to determine whether or not enemy should skip a turn or move this turn.


        //DamageEnemy is called when the player attacks an enemy.
        public void DamageEnemy(int loss)
        {
            //Call the RandomizeSfx function of SoundManager to play one of two chop sounds.
            SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);


            //Subtract loss from hit point total.
            enemyHealth -= loss;


            if (enemyHealth <= 0)
            {
                Destroy(gameObject);
                GameManager.instance.RemoveEnemyFromList(this);

                int randChance = Random.Range(1, 101);

                if (randChance < percentChanceToDropItem)
                {
                    ItemDataBaseList inventoryItemList;


                    inventoryItemList = (ItemDataBaseList)Resources.Load("ItemDatabase");



                    int randomNumber = Random.Range(1, inventoryItemList.itemList.Count);
                    int raffle = Random.Range(6, 100);

                    if (raffle <= inventoryItemList.itemList[randomNumber].rarity)
                    {

                        if (inventoryItemList.itemList[randomNumber].itemModel == null)
                        {

                        }
                        else
                        {
                            //Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
                            Vector3 randomPositionLoot = gameObject.transform.position;

                            GameObject randomLootItem = (GameObject)Instantiate(inventoryItemList.itemList[randomNumber].itemModel, randomPositionLoot, Quaternion.identity);
                            PickUpItem item = randomLootItem.AddComponent<PickUpItem>();
                            item.item = inventoryItemList.itemList[randomNumber];
                            randomLootItem.transform.SetParent(GameObject.Find("BoardHolder").transform);

                        }
                    }
                }

            }
            
        }

        //Start overrides the virtual Start function of the base class.
        protected override void Start ()
		{
			//Register this enemy with our instance of GameManager by adding it to a list of Enemy objects. 
			//This allows the GameManager to issue movement commands.
			GameManager.instance.AddEnemyToList (this);
			
			//Get and store a reference to the attached Animator component.
			animator = GetComponent<Animator> ();
			
			//Find the Player GameObject using it's tag and store a reference to its transform component.
			target = GameObject.FindGameObjectWithTag ("Player").transform;
			
			//Call the start function of our base class MovingObject.
			base.Start ();
		}
		
		
		//Override the AttemptMove function of MovingObject to include functionality needed for Enemy to skip turns.
		//See comments in MovingObject for more on how base AttemptMove function works.
		protected override bool AttemptMove <T> (int xDir, int yDir)
		{
            if (Vector3.Distance(target.transform.position, this.gameObject.transform.position) > 20)
            {
                skipMove = true;
                return false;
            }
                //Check if skipMove is true, if so set it to false and skip this turn.
                if (skipMove)
			{
				skipMove = false;
				return false;
				
			}
			
			//Call the AttemptMove function from MovingObject.
			bool b = base.AttemptMove <T> (xDir, yDir);
			
			//Now that Enemy has moved, set skipMove to true to skip next move.
			skipMove = true;

            return b;
		}
		
		
		//MoveEnemy is called by the GameManger each turn to tell each Enemy to try to move towards the player.
		public void MoveEnemy ()
		{
			//Declare variables for X and Y axis move directions, these range from -1 to 1.
			//These values allow us to choose between the cardinal directions: up, down, left and right.
			int xDir = 0;
			int yDir = 0;
			
                //If the difference in positions is approximately zero (Epsilon) do the following:
                if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)

                    //If the y coordinate of the target's (player) position is greater than the y coordinate of this enemy's position set y direction 1 (to move up). If not, set it to -1 (to move down).
                    yDir = target.position.y > transform.position.y ? 1 : -1;

                //If the difference in positions is not approximately zero (Epsilon) do the following:
                else
                    //Check if target x position is greater than enemy's x position, if so set x direction to 1 (move right), if not set to -1 (move left).
                    xDir = target.position.x > transform.position.x ? 1 : -1;

                //Call the AttemptMove function and pass in the generic parameter Player, because Enemy is moving and expecting to potentially encounter a Player
                AttemptMove<Player>(xDir, yDir);

		}
		
		
		//OnCantMove is called if Enemy attempts to move into a space occupied by a Player, it overrides the OnCantMove function of MovingObject 
		//and takes a generic parameter T which we use to pass in the component we expect to encounter, in this case Player
		protected override void OnCantMove <T> (T component)
		{
			//Declare hitPlayer and set it to equal the encountered component.
			Player hitPlayer = component as Player;

            //Call the LoseHealth function of hitPlayer passing it enemyDamage, the amount of healthpoints to be subtracted.
            enemyDamage = Random.Range(enemyMinDamage, enemyMaxDamage);
			hitPlayer.LoseHealth (enemyDamage);
			
			//Set the attack trigger of animator to trigger Enemy attack animation.
			animator.SetTrigger ("enemyAttack");
			
			//Call the RandomizeSfx function of SoundManager passing in the two audio clips to choose randomly between.
			SoundManager.instance.RandomizeSfx (attackSound1, attackSound2);
		}
	}
}

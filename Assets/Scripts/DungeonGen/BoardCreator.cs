using System.Collections;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
    // The type of tile that will be laid in a specific position.
    public enum TileType
    {
        Wall, Floor,
    }


    public int columns = 100;                                 // The number of columns on the board (how wide it will be).
    public int rows = 100;                                    // The number of rows on the board (how tall it will be).
    public IntRange numRooms = new IntRange(15, 20);         // The range of the number of rooms there can be.
    public IntRange roomWidth = new IntRange(3, 10);         // The range of widths rooms can have.
    public IntRange roomHeight = new IntRange(3, 10);        // The range of heights rooms can have.
    public IntRange corridorLength = new IntRange(6, 10);    // The range of lengths corridors between rooms can have.
    public GameObject[] floorTiles;                           // An array of floor tile prefabs.
    public GameObject[] wallTiles;                            // An array of wall tile prefabs.
    public GameObject[] outerWallTiles;                       // An array of outer wall tile prefabs.
    public GameObject[] enemyTiles;                           // An array of enemies.
    public GameObject exit;                                   // Exit tile.
    private XY[] validPositions;                             /// Array to hold coordinates of valid positions.  - Not working     
    private GameObject player;

    public GameObject[] containerPrefab;                     

    private TileType[][] tiles;                               // A jagged array of tile types representing the board, like a grid.
    private Room[] rooms;                                     // All the rooms that are created for this board.
    private Corridor[] corridors;                             // All the corridors that connect the rooms.
    private Transform boardHolder;                           // GameObject that acts as a container for all other tiles.


    //private void Start()
    //{
    //    // Create the board holder.
    //    boardHolder = new GameObject("BoardHolder").transform;

    //    SetupTilesArray();

    //    CreateRoomsAndCorridors();

    //    SetTilesValuesForRooms();
    //    SetTilesValuesForCorridors();

    //    InstantiateTiles(1);
    //    InstantiateOuterWalls();
    //}

    public void Restart(int level)
    {
        // Create the board holder.
        boardHolder = new GameObject("BoardHolder").transform;

        SetupTilesArray();

        CreateRoomsAndCorridors();

        SetTilesValuesForRooms();
        SetTilesValuesForCorridors();

        InstantiateTiles(level);
        InstantiateOuterWalls();
    }

    void SetupTilesArray()
    {
        // Set the tiles jagged array to the correct width.
        tiles = new TileType[columns][];

        // Go through all the tile arrays...
        for (int i = 0; i < tiles.Length; i++)
        {
            // ... and set each tile array is the correct height.
            tiles[i] = new TileType[rows];
        }
    }


    void CreateRoomsAndCorridors()
    {
        // Create the rooms array with a random size.
        rooms = new Room[numRooms.Random];

        // There should be one less corridor than there is rooms.
        corridors = new Corridor[rooms.Length - 1];

        // Create the first room and corridor.
        rooms[0] = new Room();
        corridors[0] = new Corridor();

        // Setup the first room, there is no previous corridor so we do not use one.
        rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows);

        // Setup the first corridor using the first room.
        corridors[0].SetupCorridor(rooms[0], corridorLength, roomWidth, roomHeight, columns, rows, true);

        for (int i = 1; i < rooms.Length; i++)
        {
            // Create a room.
            rooms[i] = new Room();

            // Setup the room based on the previous corridor.
            rooms[i].SetupRoom(roomWidth, roomHeight, columns, rows, corridors[i - 1]);

            // If we haven't reached the end of the corridors array...
            if (i < corridors.Length)
            {
                // ... create a corridor.
                corridors[i] = new Corridor();

                // Setup the corridor based on the room that was just created.
                corridors[i].SetupCorridor(rooms[i], corridorLength, roomWidth, roomHeight, columns, rows, false);

            }


            // Move the player to the new start location on level loaded.
            if (i == (int)((rooms.Length - 1) * .5f))
            {
                GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
                player = playerObject;
                Vector3 playerPos = new Vector3(rooms[i].xPos, rooms[i].yPos, 0);
                player.gameObject.transform.position = playerPos;

            }
            if (i == rooms.Length - 1)
            {
                Vector3 exitPos = new Vector3(rooms[i].xPos, rooms[i].yPos, 0);
                Instantiate(exit, exitPos, Quaternion.identity);
            }
        }

    }


    void SetTilesValuesForRooms()
    {
        // Go through all the rooms...
        for (int i = 0; i < rooms.Length; i++)
        {
            Room currentRoom = rooms[i];

            // ... and for each room go through it's width.
            for (int j = 0; j < currentRoom.roomWidth; j++)
            {
                int xCoord = currentRoom.xPos + j;

                // For each horizontal tile, go up vertically through the room's height.
                for (int k = 0; k < currentRoom.roomHeight; k++)
                {
                    int yCoord = currentRoom.yPos + k;

                    // The coordinates in the jagged array are based on the room's position and it's width and height.
                    tiles[xCoord][yCoord] = TileType.Floor;
                }
            }
        }
    }


    void SetTilesValuesForCorridors()
    {
        // Go through every corridor...
        for (int i = 0; i < corridors.Length; i++)
        {
            Corridor currentCorridor = corridors[i];

            // and go through it's length.
            for (int j = 0; j < currentCorridor.corridorLength; j++)
            {
                // Start the coordinates at the start of the corridor.
                int xCoord = currentCorridor.startXPos;
                int yCoord = currentCorridor.startYPos;

                // Depending on the direction, add or subtract from the appropriate
                // coordinate based on how far through the length the loop is.
                switch (currentCorridor.direction)
                {
                    case Direction.North:
                        yCoord += j;
                        break;
                    case Direction.East:
                        xCoord += j;
                        break;
                    case Direction.South:
                        yCoord -= j;
                        break;
                    case Direction.West:
                        xCoord -= j;
                        break;
                }

                // Set the tile at these coordinates to Floor.
                tiles[xCoord][yCoord] = TileType.Floor;
            }
        }
    }


    void InstantiateTiles(int level)
    {
        //validPositions = new XY[200];
        //int positionCount = 0;
        // Go through all the tiles in the jagged array...
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                // ... and instantiate a floor tile for it.
                InstantiateFromArray(floorTiles, i, j);

                // If the tile type is Wall...
                if (tiles[i][j] == TileType.Wall)
                {
                    // ... instantiate a wall over the top.
                    InstantiateFromArray(wallTiles, i, j);
                }
                //else
                //{
                //    validPositions[positionCount].SetCoordinates(i, j);                                         // Store all valid positions to instatiate other objects.
                //    positionCount++;
                //}
            }
        }

        //Instantiate enemies.
        int numOfRooms = 0;                                                                 // int to store the number of rooms spawned on this level.
        foreach (Room element in rooms)
        {
            numOfRooms++;
        }
        int enemyCount = (int)Random.Range(Mathf.Log(level, 1.2f) + numOfRooms - 10,  Mathf.Log(level + 1, 1.1f) + numOfRooms);
        int cnt = -1;
        ///Why wont the storage array method work?
        //while (cnt < enemyCount)
        //{
        //    for (int validPos = 0; validPos < positionCount; validPos++)
        //    {
        //        int rand = Random.Range(1, 500);
        //        if (rand == 50)
        //        {
        //            InstantiateFromArray(enemyTiles, validPositions[validPos].x, validPositions[validPos].y);
        //            validPositions[validPos] = null;
        //        }
        //    }
        //}
        int chestCount = 0;
        int chestSpawnAmount = numOfRooms + Random.Range(-10, level);
        while (cnt < enemyCount || chestCount < chestSpawnAmount)
        {
            cnt++;                                                              // Everytime it checks every valid position but has to loop again decrease the amount of enemies.
                                                                                // helps reduce enemy count on smaller levels
            for (int y = 0; y < tiles.Length; y++)
            {
                for (int z = 0; z < tiles[y].Length; z++)
                {
                    if (tiles[y][z] != TileType.Wall)
                    {
                        int chestSpawnChance = Random.Range(1, 500);
                        int rand = Random.Range(1, 1000);
                        if (rand == 50 && cnt < enemyCount)
                        {
                            InstantiateFromArray(enemyTiles, y, z);
                            tiles[y][z] = TileType.Wall;
                            cnt++;
                        }
                        else if (chestSpawnChance == 1 && chestCount < chestSpawnAmount)
                        {
                            InstantiateFromArray(containerPrefab, y, z);
                            chestCount++;
                        }
                    }
                }
            }
        }


        /// Should be able to write this into the spawning enemies array.
        // while(cnt < enemyCount || cnt < amountOfLoot)
        // then add if statements that checks each condition again and determines whether or not to spawn the tile
        ItemDataBaseList inventoryItemList;

        int counter = 0;


        inventoryItemList = (ItemDataBaseList)Resources.Load("ItemDatabase");

        int amountOfLoot = (int)Random.Range(Mathf.Log(level, 1.2f) + 15, Mathf.Log(level + 1, 1.1f) + 35);

        while (counter < amountOfLoot)
        {

            for (int y = 0; y < tiles.Length; y++)
            {
                for (int z = 0; z < tiles[y].Length; z++)
                {
                    if (counter >= amountOfLoot) break;
                    if (tiles[y][z] != TileType.Wall)
                    {
                        int rand = Random.Range(1, 1000);
                        if (rand == 50)
                        {
                            int randomNumber = Random.Range(1, inventoryItemList.itemList.Count);
                            int raffle = Random.Range(1, 100);

                            if (raffle <= inventoryItemList.itemList[randomNumber].rarity)
                            {

                                if (inventoryItemList.itemList[randomNumber].itemModel == null)
                                    counter--;
                                else
                                {
                                    Vector3 randomPositionLoot = new Vector3(y, z, 0);

                                    GameObject randomLootItem = (GameObject)Instantiate(inventoryItemList.itemList[randomNumber].itemModel, randomPositionLoot, Quaternion.identity);
                                    PickUpItem item = randomLootItem.AddComponent<PickUpItem>();
                                    item.item = inventoryItemList.itemList[randomNumber];
                                    randomLootItem.transform.SetParent(boardHolder);
                                    counter++;

                                }
                            }
                        }
                    }
                }
            }
        }

    }


    void InstantiateOuterWalls()
    {
        // The outer walls are one unit left, right, up and down from the board.
        float leftEdgeX = -1f;
        float rightEdgeX = columns + 0f;
        float bottomEdgeY = -1f;
        float topEdgeY = rows + 0f;

        // Instantiate both vertical walls (one on each side).
        InstantiateVerticalOuterWall(leftEdgeX, bottomEdgeY, topEdgeY);
        InstantiateVerticalOuterWall(rightEdgeX, bottomEdgeY, topEdgeY);

        // Instantiate both horizontal walls, these are one in left and right from the outer walls.
        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, bottomEdgeY);
        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, topEdgeY);
    }


    void InstantiateVerticalOuterWall(float xCoord, float startingY, float endingY)
    {
        // Start the loop at the starting value for Y.
        float currentY = startingY;

        // While the value for Y is less than the end value...
        while (currentY <= endingY)
        {
            // ... instantiate an outer wall tile at the x coordinate and the current y coordinate.
            InstantiateFromArray(outerWallTiles, xCoord, currentY);

            currentY++;
        }
    }


    void InstantiateHorizontalOuterWall(float startingX, float endingX, float yCoord)
    {
        // Start the loop at the starting value for X.
        float currentX = startingX;

        // While the value for X is less than the end value...
        while (currentX <= endingX)
        {
            // ... instantiate an outer wall tile at the y coordinate and the current x coordinate.
            InstantiateFromArray(outerWallTiles, currentX, yCoord);

            currentX++;
        }
    }


    void InstantiateFromArray(GameObject[] prefabs, float xCoord, float yCoord)
    {
        // Create a random index for the array.
        int randomIndex = Random.Range(0, prefabs.Length);

        // The position to be instantiated at is based on the coordinates.
        Vector3 position = new Vector3(xCoord, yCoord, 0f);

        // Create an instance of the prefab from the random index of the array.
        GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;

        // Set the tile's parent to the board holder.
        tileInstance.transform.parent = boardHolder.transform;
    }
}
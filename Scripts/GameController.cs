using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpatialPartitionPattern
{
    public class GameController : MonoBehaviour
    {
        public GameObject friendlyObj;
        public GameObject enemyObj;

        //Material Change
        public Material enemyMaterial;
        public Material closestEnemyMaterial;

        //Parent Gameobjects
        public Transform enemyParent;
        public Transform friendlyParent;

        //List of Soldiers
        List<Soldier> enemySoldiers = new List<Soldier>();
        List<Soldier> friendlySoldiers = new List<Soldier>();

        //Closest enemies
        List<Soldier> closestEnemies = new List<Soldier>();

        //Grid data
        float mapWidth = 50f;
        int cellSize = 10;

        //Number of soldiers
        public int numberOfSoldiers = 300;

        //Grid
        Grid grid;

        //Switch between spatial partition and slow
        public bool SpatialPartition;
        //public Text buttonText;
        public Toggle toggle;

        //Time
        public Text msText;

        public void SpatialPartitionChanger()
        {
            if (SpatialPartition == true)
            {
                SpatialPartition = false;
            }
            else
            {
                SpatialPartition = true;
            }
        }

        void Start()
        {

            //Create new grid
            grid = new Grid((int)mapWidth, cellSize);

            //Random soldiers int list
            for (int i = 0; i < (numberOfSoldiers * 2); i++)
            {
                //Give the enemy random positions
                Vector3 randomPos = new Vector3(Random.Range(0f, mapWidth), Random.Range(0f, mapWidth), Random.Range(0f, mapWidth));

                //Create a new enemy
                GameObject newEnemy = Instantiate(enemyObj, randomPos, Quaternion.identity) as GameObject;

                //Add to enemy list
                enemySoldiers.Add(new Enemy(newEnemy, mapWidth, grid));

                //Parent it
                newEnemy.transform.parent = enemyParent;
            }

            for (int i = 0; i < numberOfSoldiers; i++)
            {
                //Give the friendly random positions
                Vector3 randomPos = new Vector3(Random.Range(0f, mapWidth), Random.Range(0f, mapWidth), Random.Range(0f, mapWidth));

                //Create a new friendly
                GameObject newFriendly = Instantiate(friendlyObj, randomPos, Quaternion.identity) as GameObject;

                //Add to friendly list
                friendlySoldiers.Add(new Friendly(newFriendly, mapWidth));

                //Parent it
                newFriendly.transform.parent = friendlyParent;
            }
        }

        void Update()
        {
            float startTime = Time.realtimeSinceStartup;

            //Move enemies
            for (int i = 0; i < enemySoldiers.Count; i++)
            {
                enemySoldiers[i].Move();
            }

            //Reset material of closest enemy
            for (int i = 0; i < closestEnemies.Count; i++)
            {
                closestEnemies[i].soldierMeshRenderer.material = enemyMaterial;
            }

            //Reset list of closest enemies
            closestEnemies.Clear();
            Soldier closestEnemy;

            //Find closest enemy and change color
            for (int i = 0; i < friendlySoldiers.Count; i++)
            {
                if (SpatialPartition != true)
                {
                    //Spatial Partition slow version
                    closestEnemy = FindClosestEnemySlow(friendlySoldiers[i]);

                }
                else
                {
                    //Spatial Partition fast version
                    closestEnemy = grid.FindClosestEnemy(friendlySoldiers[i]);

                }

                //If enemy found
                if (closestEnemy != null)
                {
                    //Change material
                    closestEnemy.soldierMeshRenderer.material = closestEnemyMaterial;

                    closestEnemies.Add(closestEnemy);

                    //Move friendly in enemy direction
                    friendlySoldiers[i].Move(closestEnemy);
                }

                float timeElapsed = (Time.realtimeSinceStartup - startTime) * 1000f;
                msText.text = timeElapsed + "ms";

            }
        }

        //Find closest enemy slow version
        Soldier FindClosestEnemySlow(Soldier soldier)
        {
            Soldier closestEnemy = null;

            float bestDistSqr = Mathf.Infinity;

            //Loop through enemies
            for (int i = 0; i < enemySoldiers.Count; i++)
            {
                //The distance
                float distSqr = (soldier.soldierTrans.position - enemySoldiers[i].soldierTrans.position).sqrMagnitude;

                //Distance test
                if (distSqr < bestDistSqr)
                {
                    bestDistSqr = distSqr;

                    closestEnemy = enemySoldiers[i];
                }
            }

            return closestEnemy;
        }

    }
}
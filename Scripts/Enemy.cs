using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialPartitionPattern
{
    public class Enemy : Soldier
    {
        //Enemy heading
        Vector3 currentTarget;
        //Previous position
        Vector3 oldPos;
        //Map width
        float mapWidth;
        //The grid
        Grid grid;

        //Init enemy
        public Enemy(GameObject soldierObj, float mapWidth, Grid grid)
        {
            //Save something
            this.soldierTrans = soldierObj.transform;

            this.soldierMeshRenderer = soldierObj.GetComponent<MeshRenderer>();

            this.mapWidth = mapWidth;

            this.grid = grid;

            //Add unit to grid
            grid.Add(this);

            //Init old pos
            oldPos = soldierTrans.position;

            this.walkSpeed = 5f;

            //Give new random coordinates
            GetNewTarget();
        }

        public override void Move()
        {
            //Move towards target
            soldierTrans.Translate(Vector3.forward * Time.deltaTime * walkSpeed);

            //Check for new cell
            grid.Move(this, oldPos);

            //Save old pos
            oldPos = soldierTrans.position;

            //If reached target, get new target
            if ((soldierTrans.position - currentTarget).magnitude < 1f)
            {
                GetNewTarget();
            }
        }

        void GetNewTarget()
        {
            currentTarget = new Vector3(Random.Range(0f, mapWidth), Random.Range(0f, mapWidth), Random.Range(0f, mapWidth));

            //Rotate
            soldierTrans.rotation = Quaternion.LookRotation(currentTarget - soldierTrans.position);
        }
    }
}

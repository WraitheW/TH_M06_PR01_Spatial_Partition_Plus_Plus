using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialPartitionPattern
{
    public class Friendly : Soldier
    {
        //Init friendly
        public Friendly(GameObject soldierObj, float mapWidth)
        {
            this.soldierTrans = soldierObj.transform;

            this.walkSpeed = 2f;
        }

        public override void Move(Soldier closestEnemy)
        {
            //Rotate towards closest enemy
            soldierTrans.rotation = Quaternion.LookRotation(closestEnemy.soldierTrans.position - soldierTrans.position);
            //Move toward enemy
            soldierTrans.Translate(Vector3.forward * Time.deltaTime * walkSpeed);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialPartitionPattern
{
    public class Soldier
    {
        //To change material
        public MeshRenderer soldierMeshRenderer;

        //To move soldier
        public Transform soldierTrans;

        //The speed of soldier
        protected float walkSpeed;

        public Soldier previousSoldier;
        public Soldier nextSoldier;

        public virtual void Move()
        { }

        public virtual void Move(Soldier soldier)
        { }

    }
}

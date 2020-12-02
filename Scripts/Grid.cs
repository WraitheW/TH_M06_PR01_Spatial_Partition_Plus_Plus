using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialPartitionPattern
{
    public class Grid
    {
        //Convert world coord to cell pos
        int cellSize;

        //Actual grid
        Soldier[,,] cells;

        //Init grid
        public Grid(int mapWidth, int cellSize)
        {
            this.cellSize = cellSize;

            int numberOfCells = mapWidth / cellSize;

            cells = new Soldier[numberOfCells, numberOfCells, numberOfCells];
        }

        //Add a unity to grid
        public void Add(Soldier soldier)
        {
            //Determine which grid cell soldier exists
            int cellX = (int)(soldier.soldierTrans.position.x / cellSize);
            int cellY = (int)(soldier.soldierTrans.position.y / cellSize);
            int cellZ = (int)(soldier.soldierTrans.position.z / cellSize);

            //Add soldier to front of list
            soldier.previousSoldier = null;
            soldier.nextSoldier = cells[cellX, cellY, cellZ];

            //Associate cell with soldier
            cells[cellX, cellY, cellZ] = soldier;

            if (soldier.nextSoldier != null)
            {
                //Linked list nonsense
                soldier.nextSoldier.previousSoldier = soldier;
            }
        }

        //Get closest enemy from gridd
        public Soldier FindClosestEnemy(Soldier friendlySoldier)
        {
            //Determine cell of friendly soldier
            int cellX = (int)(friendlySoldier.soldierTrans.position.x / cellSize);
            int cellY = (int)(friendlySoldier.soldierTrans.position.y / cellSize);
            int cellZ = (int)(friendlySoldier.soldierTrans.position.z / cellSize);

            //Get first enemy in grid
            Soldier enemy = cells[cellX, cellY, cellZ];

            //Find closest soldier of all in linked list
            Soldier closestSoldier = null;

            float bestDistSqr = Mathf.Infinity;

            //Loop through linked list
            while (enemy != null)
            {
                //Distance sqr b/w soldier and enemy
                float distSqr = (enemy.soldierTrans.position - friendlySoldier.soldierTrans.position).sqrMagnitude;

                //If statement
                if (distSqr < bestDistSqr)
                {
                    bestDistSqr = distSqr;

                    closestSoldier = enemy;
                }

                enemy = enemy.nextSoldier;
            }

            return closestSoldier;
        }

        public void Move(Soldier soldier, Vector3 oldPos)
        {
            //Find old cell
            int oldCellX = (int)(oldPos.x / cellSize);
            int oldCellZ = (int)(oldPos.z / cellSize);
            int oldCellY = (int)(oldPos.y / cellSize);

            //Get new cell
            int cellX = (int)(soldier.soldierTrans.position.x / cellSize);
            int cellY = (int)(soldier.soldierTrans.position.y / cellSize);
            int cellZ = (int)(soldier.soldierTrans.position.z / cellSize);

            //If it didn't change cells, done
            if (oldCellX == cellX && oldCellZ == cellZ && oldCellY == cellY)
            {
                return;
            }

            //Unlink from list of old cell
            if (soldier.previousSoldier != null)
            {
                soldier.previousSoldier.nextSoldier = soldier.nextSoldier;
            }

            if (soldier.nextSoldier != null)
            {
                soldier.nextSoldier.previousSoldier = soldier.previousSoldier;
            }

            //If head of list, remove
            if (cells[oldCellX, oldCellY, oldCellZ] == soldier)
            {
                cells[oldCellX, oldCellY, oldCellZ] = soldier.nextSoldier;
            }

            Add(soldier);
        }
    }
}

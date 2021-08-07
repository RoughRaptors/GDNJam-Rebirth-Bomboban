using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEMPJAMNAMEREPLACEME
{
    public class IceCubeOccupier : TileOccupier
    {
        void Start()
        {
            health = 2;
        }

        public override bool ReactToExplosion(int fromRow, int fromCol, DataManager.Direction explosionDirection)
        {
            /*
            // move in the collision direction until an occupier is hit
            // both occupiers take one damage upon hit
            int curRow = curTile.GetRow();
            int curCol = curTile.GetCol();
            bool shouldKeepMoving = true;

            // move in our direction
            if (collisionDirection == DataManager.Direction.Up)
            {
                for (int i = curRow; i >= 0; --i)
                {
                    shouldKeepMoving = HandleCollision(i, curCol);
                }
            }
            else if (collisionDirection == DataManager.Direction.Down)
            {

            }
            else if (collisionDirection == DataManager.Direction.Left)
            {

            }
            else if (collisionDirection == DataManager.Direction.Right)
            {

            }
            */

            return false;
        }

        public override bool ReactToCollision(TileOccupier collidingOccupier, DataManager.Direction collisionDirection)
        {
            // do nothing if we collided with player
            if(collidingOccupier is PlayerOccupier)
            {
                return false;
            }

            return true;
        }

        /*
        bool HandleCollision(int newRow, int newCol)
        {
            int curRow = curTile.GetRow();
            int curCol = curTile.GetCol();
            
            // get the occupier at our next tile in our movement direction
            TileOccupier tileOccupier = GameManager.Instance.GetOccupierAtLocation(newRow, newCol);
            if (tileOccupier)
            {
                // we hit something, take and deal damage
                SubtractHealth(1);
                tileOccupier.SubtractHealth(1);

                if (health <= 0)
                {
                    Destroy(this.gameObject);
                }

                if (tileOccupier.GetHealth() <= 0)
                {
                    Destroy(tileOccupier.gameObject);
                }

                return true;
            }

            return false;
        }

        bool HandleExplosion(int newRow, int newCol)
        {
            return false
        }
        */
    }
}
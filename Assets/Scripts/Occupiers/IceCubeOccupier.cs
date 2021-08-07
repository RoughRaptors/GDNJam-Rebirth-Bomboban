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
            SubtractHealth(1);
            bool didExplode = Push(fromRow, fromCol, explosionDirection);
            if(didExplode)
            {
                Vector3 newPos = new Vector3(curTile.GetPhysicalXPos(), curTile.GetPhysicalYPos());
                StartCoroutine(HandleMovePhysical(newPos));
            }

            return didExplode;
        }

        public override bool ReactToCollision(TileOccupier collidingOccupier, DataManager.Direction collisionDirection)
        {
            bool didCollide = false;
            if (collidingOccupier is PlayerOccupier)
            {
                didCollide = Push(collidingOccupier.GetCurTile().GetRow(), collidingOccupier.GetCurTile().GetCol(), collisionDirection);
                if (didCollide)
                {
                    Vector3 newPos = new Vector3(curTile.GetPhysicalXPos(), curTile.GetPhysicalYPos());
                    StartCoroutine(HandleMovePhysical(newPos));
                }
            }

            return didCollide;
        }

        private bool Push(int fromRow, int fromCol, DataManager.Direction direction)
        {
            // don't go out of bounds
            if ((curTile.GetRow() == 0 && direction == DataManager.Direction.Up)
                || (curTile.GetRow() == DataManager.NUM_ROWS - 1 && direction == DataManager.Direction.Down)
                || (curTile.GetCol() == 0 && direction == DataManager.Direction.Left)
                || (curTile.GetCol() > DataManager.NUM_COLS - 1) && direction == DataManager.Direction.Right)
            {
                return false;
            }

            // move in our direction until we hit something
            int newRow = curTile.GetRow();
            int newCol = curTile.GetCol();
            bool doneMoving = false;
            while (!doneMoving)
            {
                if (direction == DataManager.Direction.Up)
                {
                    newRow = newRow - 1;
                }
                else if (direction == DataManager.Direction.Down)
                {
                    newRow = newRow + 1;
                }
                else if (direction == DataManager.Direction.Left)
                {
                    newCol = newCol - 1;
                }
                else if (direction == DataManager.Direction.Right)
                {
                    newCol = newCol + 1;
                }

                // edge of level, stop
                if (!GameManager.Instance.IsValidTile(newRow, newCol))
                {
                    return true;
                }

                // we moved, nothing is here anymore
                curTile.SetTileOccupier(null);

                doneMoving = CheckForOccupierAndCollision(newRow, newCol, direction);
            }

            return true;
        }

        bool CheckForOccupierAndCollision(int occupierRow, int occupierCol, DataManager.Direction direction)
        {
            bool collided = false;
            bool didDestroyCollidingOccupier = false;

            int newRow = occupierRow;
            int newCol = occupierCol;            
            TileOccupier collisionTileOccupier = GameManager.Instance.GetOccupierAtLocation(occupierRow, occupierCol);
            if (collisionTileOccupier)
            {
                collided = true;
                SubtractHealth(1);

                // if we collided with a hole, destroy both
                if(collisionTileOccupier is HoleOccupier)
                {
                    Destroy(this.gameObject);
                    Destroy(collisionTileOccupier.gameObject);

                    return true;
                }

                // we want to keep moving if we destroyed the object, as we replace it by moving on top of it
                didDestroyCollidingOccupier = collisionTileOccupier.SubtractHealth(1);
                if (!didDestroyCollidingOccupier)
                {
                    // we need to stop moving, to do this we actually have to reverse calculate to get the new tile
                    // these directions are intentionally reversed to backtrack from our direction
                    if (direction == DataManager.Direction.Up)
                    {
                        newRow = newRow + 1;
                    }
                    else if (direction == DataManager.Direction.Down)
                    {
                        newRow = newRow - 1;
                    }
                    else if (direction == DataManager.Direction.Left)
                    {
                        newCol = newCol + 1;
                    }
                    else if (direction == DataManager.Direction.Right)
                    {
                        newCol = newCol - 1;
                    }
                }
            }

            Tile newTile = GameManager.Instance.GetTileAtLocation(newRow, newCol);
            if (newTile)
            {
                newTile.SetTileOccupier(this);
                SetCurTile(newTile);
            }

            // if we collided or the new occupier was destroyed, stop moving, otherwise keep going until we hit something
            if(collided || didDestroyCollidingOccupier)
            {
                return true;
            }

            return false;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEMPJAMNAMEREPLACEME
{
    public class TitaniumBoxOccupier : TileOccupier
    {
        public override bool ReactToExplosion(int fromRow, int fromCol, DataManager.Direction explosionDirection)
        {
            // is pushed by the player
            // don't go out of bounds
            if ((curTile.GetRow() == 0 && explosionDirection == DataManager.Direction.Up)
                || (curTile.GetRow() == DataManager.NUM_ROWS - 1 && explosionDirection == DataManager.Direction.Down)
                || (curTile.GetCol() == 0 && explosionDirection == DataManager.Direction.Left)
                || (curTile.GetCol() > DataManager.NUM_COLS - 1 && explosionDirection == DataManager.Direction.Right))
            {
                return false;
            }

            int newRow = curTile.GetRow();
            int newCol = curTile.GetCol();
            if (explosionDirection == DataManager.Direction.Up)
            {
                newRow = newRow - 1;
            }
            else if (explosionDirection == DataManager.Direction.Down)
            {
                newRow = newRow + 1;
            }
            else if (explosionDirection == DataManager.Direction.Left)
            {
                newCol = newCol - 1;
            }
            else if (explosionDirection == DataManager.Direction.Right)
            {
                newCol = newCol + 1;
            }

            // don't move if it were going to collide with another object that's not a hole
            TileOccupier newTileOccupierAfterMove = GameManager.Instance.GetOccupierAtLocation(newRow, newCol);
            if (newTileOccupierAfterMove)
            {
                // if we collide with a hole, destroy both
                if (newTileOccupierAfterMove is HoleOccupier)
                {
                    flaggedForDeath = true;
                    collisionObjectToDestroyAfterLERP = newTileOccupierAfterMove;
                }
                else
                {
                    // if we don't do anything from the explosion, stop
                    if(!newTileOccupierAfterMove.ReactToCollision(this, explosionDirection))
                    {
                        return true;
                    }
                }
            }

            // valid movement
            Tile newTileAfterMove = GameManager.Instance.GetTileAtLocation(newRow, newCol);
            if (newTileAfterMove)
            {
                // set our occupier's CURRENT tile occupier to null and the NEW tile's occupier to this
                curTile.SetTileOccupier(null);

                // our new tile occupier is this object and the occupier's tile is the new tile
                // i hate having intertwined references here but, ugh, no time
                newTileAfterMove.SetTileOccupier(this);
                SetCurTile(newTileAfterMove);

                Vector3 newPos = new Vector3(newTileAfterMove.GetPhysicalXPos(), newTileAfterMove.GetPhysicalYPos());
                StartCoroutine(HandleMovePhysical(newPos));
            }

            return true;
        }

        public override bool ReactToCollision(TileOccupier collidingOccupier, DataManager.Direction collisionDirection)
        {
            // do nothing
            return false;
        }
    }
}
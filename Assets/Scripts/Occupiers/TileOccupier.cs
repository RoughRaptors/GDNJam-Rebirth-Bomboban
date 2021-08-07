using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEMPJAMNAMEREPLACEME
{
    public abstract class TileOccupier : MonoBehaviour
    {
        protected int health;
        protected bool flaggedForDeath = false;
        protected TileOccupier collisionObjectToDestroyAfterLERP = null;
        public int GetHealth() { return health; }

        protected Tile curTile;
        public Tile GetCurTile() { return curTile; }
        public void SetCurTile(Tile tile) { curTile = tile; }

        // the direction is not the direction it came from, but the direction the occupier that initiated the direction was coming from
        // ie if a player is at (0,0) and moves right to (0,1) and collides with something, collisionDirection is right
        // even though it came from the other side relative to the object being collided with
        public abstract bool ReactToExplosion(int fromRow, int fromCol, DataManager.Direction explosionDirection);
        public abstract bool ReactToCollision(TileOccupier collidingOccupier, DataManager.Direction collisionDirection);

        public bool SubtractHealth(int amount, bool fromExplosion = false)
        {
            health -= amount;
            if (health <= 0)
            {
                curTile.SetTileOccupier(null);

                // if it's not from an explosion, don't immediately destroy it
                // wait for the lerp to complete
                if (!fromExplosion)
                {
                    flaggedForDeath = true;
                }
                else
                {
                    Destroy(this.gameObject);
                }

                return true;
            }

            return false;
        }

        protected IEnumerator HandleMovePhysical(Vector3 newPos, int numTilesMoved = 1)
        {
            float elapsedTime = 0;
            float waitTime = DataManager.LERP_COROUTINE_WAIT_TIME * numTilesMoved;

            while (elapsedTime < waitTime)
            {
                transform.position = Vector3.Lerp(transform.position, newPos, (elapsedTime / waitTime));
                elapsedTime += Time.deltaTime;

                // Yield here
                yield return null;
            }

            if(flaggedForDeath)
            {
                Destroy(this.gameObject);

            }

            if (collisionObjectToDestroyAfterLERP)
            {
                // if we're destroyed and not replaced, then null out the occupier
                if (collisionObjectToDestroyAfterLERP == curTile.GetTileOuccupier())
                {
                    collisionObjectToDestroyAfterLERP.curTile.SetTileOccupier(null);
                }

                Destroy(collisionObjectToDestroyAfterLERP.gameObject);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEMPJAMNAMEREPLACEME
{
    public abstract class TileOccupier : MonoBehaviour
    {
        private DataManager.OccupierType occupierType;

        protected int health;
        public int GetHealth() { return health; }

        protected Tile curTile;
        public Tile GetCurTile() { return curTile; }
        public void SetCurTile(Tile tile) { curTile = tile; }

        // the direction is not the direction it came from, but the direction the occupier that initiated the direction was coming from
        // ie if a player is at (0,0) and moves right to (0,1) and collides with something, collisionDirection is right
        // even though it came from the other side relative to the object being collided with
        public abstract bool ReactToExplosion(int fromRow, int fromCol, DataManager.Direction explosionDirection);
        public abstract bool ReactToCollision(TileOccupier collidingOccupier, DataManager.Direction collisionDirection);

        public bool SubtractHealth(int amount)
        {
            health -= amount;
            if (health <= 0)
            {
                curTile.SetTileOccupier(null);
                Destroy(this.gameObject);

                return true;
            }

            return false;
        }

        protected IEnumerator HandleMovePhysical(Vector3 newPos)
        {
            float elapsedTime = 0;
            float waitTime = DataManager.LERP_COROUTINE_WAIT_TIME;

            while (elapsedTime < waitTime)
            {
                transform.position = Vector3.Lerp(transform.position, newPos, (elapsedTime / waitTime));
                elapsedTime += Time.deltaTime;

                // Yield here
                yield return null;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEMPJAMNAMEREPLACEME
{
    public class PlayerController : MonoBehaviour
    {
        public TileOccupier player;
        private Coroutine lerpCoroutine = null;

        void Update()
        {
            LoadedLevel curLevel = GameManager.Instance.GetCurLevel();
            if (!(curLevel is null))
            {
                bool isValidTile = false;
                int newRow = 0;
                int newCol = 0;
                if (Input.GetKeyDown(KeyCode.D))
                {
                    // moving right
                    newRow = player.GetCurTile().GetRow();
                    newCol = player.GetCurTile().GetCol() + 1;

                    isValidTile = IsValidTile(newRow, newCol);
                }
                else if(Input.GetKeyDown(KeyCode.A))
                {
                    // moving left
                    newRow = player.GetCurTile().GetRow();
                    newCol = player.GetCurTile().GetCol() - 1;

                    isValidTile = IsValidTile(newRow, newCol);

                }
                else if(Input.GetKeyDown(KeyCode.W))
                {
                    // moving up
                    newRow = player.GetCurTile().GetRow() - 1;
                    newCol = player.GetCurTile().GetCol();

                    isValidTile = IsValidTile(newRow, newCol);
                }
                else if(Input.GetKeyDown(KeyCode.S))
                {
                    // moving down
                    newRow = player.GetCurTile().GetRow() + 1;
                    newCol = player.GetCurTile().GetCol();

                    isValidTile = IsValidTile(newRow, newCol);
                }

                if (isValidTile)
                {
                    Tile newTile = GameManager.Instance.GetTileAtLocation(newRow, newCol);
                    Vector3 newPos = new Vector3(newTile.GetPhysicalXPos(), newTile.GetPhysicalYPos());

                    newTile.SetTileOccupier(curLevel.GetPlayer());
                    GameManager.Instance.GetOccupierAtLocation(newRow, newCol).SetCurTile(newTile);

                    // stop our old lerp if it exists, and lerp to our new position
                    if (lerpCoroutine != null)
                    {
                        StopCoroutine(lerpCoroutine);
                    }

                    lerpCoroutine = StartCoroutine(HandleMovePlayer(newPos));
                }
            }
        }

        public void SetPlayerObject(TileOccupier player)
        {
            this.player = player;
        }

        private bool IsValidTile(int newRow, int newCol)
        {
            if (newRow >= 0 && newRow < DataManager.NUM_ROWS && newCol >= 0 && newCol < DataManager.NUM_COLS)
            {
                return true;
            }

            return false;
        }

        private IEnumerator HandleMovePlayer(Vector3 newPos)
        {
            float elapsedTime = 0;
            float waitTime = 3f;

            while (elapsedTime < waitTime)
            {                
                player.transform.position = Vector3.Lerp(player.transform.position, newPos, (elapsedTime / waitTime));
                elapsedTime += Time.deltaTime;

                // Yield here
                yield return null;
            }
        }
    }
}
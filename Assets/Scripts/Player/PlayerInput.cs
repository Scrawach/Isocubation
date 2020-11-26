using Environment;
using UnityEngine;

namespace Player
{
    public class PlayerInput : Mind
    {
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                MoveLeft();
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                MoveUp();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                MoveRight();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                MoveDown();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                Jump();
            }
        }
    }
}
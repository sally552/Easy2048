

using UnityEngine;

namespace UserInput
{
    public class UserInputKeyboard : IInputController
    {
        public bool InputUp()
        {
            return CheckKeysInput(KeyCode.W, KeyCode.UpArrow);
        }

        public bool InputLeft()
        {
            return CheckKeysInput(KeyCode.A, KeyCode.LeftArrow);
        }

        public bool InputDown()
        {
            return CheckKeysInput(KeyCode.S, KeyCode.DownArrow);
        }

        public bool InputRight()
        {
            return CheckKeysInput(KeyCode.D, KeyCode.RightArrow);
        }

        private bool CheckKeysInput(KeyCode key1, KeyCode key2)
        {
            return Input.GetKeyDown(key1) || Input.GetKeyDown(key2);
        }
    }
}
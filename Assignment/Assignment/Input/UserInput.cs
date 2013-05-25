using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Assignment
{
    public class UserInput
    {
        public delegate void UserInputEvent();

        KeyboardState PrevKeyboardState;        

      
        public static UserInput GetUserInput()
        {
            if (singleton == null)
            {
                singleton = new UserInput();
            }
            return singleton;
        }
        private static UserInput singleton;

        public UserInput()
        {
            PrevKeyboardState = Keyboard.GetState();
        }


        public void Update(GameTime gameTime)
        {
            PrevKeyboardState = Keyboard.GetState();
        }


        public bool KeyJustPressed(Keys a_key)
        {
            return PrevKeyboardState.IsKeyUp(a_key) && Keyboard.GetState().IsKeyDown(a_key);
        }

        public bool KeyJustReleased(Keys a_key)
        {
            return PrevKeyboardState.IsKeyDown(a_key) && Keyboard.GetState().IsKeyUp(a_key);
        }

        public bool IsKeyDown(Keys a_key)
        {
            return PrevKeyboardState.IsKeyDown(a_key) && Keyboard.GetState().IsKeyDown(a_key);
        }

        public bool IsKeyUp(Keys a_key)
        {
            return PrevKeyboardState.IsKeyUp(a_key) && Keyboard.GetState().IsKeyUp(a_key);
        }
    }
}
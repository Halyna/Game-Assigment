using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment
{
    public class InputManager
    {

        // Input Variables
        public static UserInput Input;

        public static void Initialize() {
            Input = UserInput.GetUserInput();
        }

        public static bool IsMovingLeft() {
            if (Input.IsKeyDown(Keys.Left) || Input.IsKeyDown(Keys.A)) return true;

            return false;
        }

        public static bool IsMovingRight() {
            if (Input.IsKeyDown(Keys.Right) || Input.IsKeyDown(Keys.D)) return true;
            return false;
        }

        public static bool IsMovingUp() {
            if (Input.IsKeyDown(Keys.Up) || Input.IsKeyDown(Keys.W)) return true;
            return false;
        }

        public static bool IsMovingDown() {
            if (Input.IsKeyDown(Keys.Down) || Input.IsKeyDown(Keys.S)) return true;
            return false;
        }

        public static Vector2 GetDirection() {
            Vector2 vector = Vector2.Zero;
            if (Input.IsKeyDown(Keys.Down) || Input.IsKeyDown(Keys.S) ||
                Input.IsKeyDown(Keys.Up) || Input.IsKeyDown(Keys.W) ||
                Input.IsKeyDown(Keys.Left) || Input.IsKeyDown(Keys.A) ||
                Input.IsKeyDown(Keys.Right) || Input.IsKeyDown(Keys.D)) {
                    if (Input.IsKeyDown(Keys.Down) || Input.IsKeyDown(Keys.S)) {
                        vector += Vector2.UnitY;
                    }
                    if (Input.IsKeyDown(Keys.Up) || Input.IsKeyDown(Keys.W)) {
                        vector -= Vector2.UnitY;
                    }
                    if (Input.IsKeyDown(Keys.Left) || Input.IsKeyDown(Keys.A)) {
                        vector -= Vector2.UnitX;
                    }
                    if (Input.IsKeyDown(Keys.Right) || Input.IsKeyDown(Keys.D)) {
                        vector += Vector2.UnitX;
                    }
            } 
            
            return vector;
        }

        //static checks here
        public static bool PressedStart() {
            if (Input.KeyJustPressed(Keys.Enter)) return true;
         
            return false;
        }

        public static bool PressedBack() {
            if (Input.KeyJustPressed(Keys.Escape)) return true;
            
            return false;
        }

        public static bool PressedMute() {
            if (Input.KeyJustPressed(Keys.M)) return true;
            //no XBOX equivalent
            return false;
        }

        public static bool PressedBoost() {
            if (Input.KeyJustPressed(Keys.Space)) return true;
            
            return false;
        }

        public static bool PressedAnything() {
            return (PressedBoost() || PressedBack() || PressedStart());
        }

        public static void Update(GameTime gameTime) {
            Input.Update(gameTime);
        }
    }
}

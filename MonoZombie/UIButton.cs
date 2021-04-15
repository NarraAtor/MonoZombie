using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoZombie {
	public class UIButton : UIElement {
		private Action onClick;
		private bool isPressed;
		private string text;

		public UIButton (string text, Vector2 position, Action onClick, bool isCentered = true) : base(Main.buttonTexture, position, isCentered) {
			this.text = text;
			this.onClick = onClick;

			isPressed = false;
		}

		public override void Update (GameTime gameTime, MouseState mouse) {
			// Check to see if the mouse is within the area of the button
			bool inXBounds = mouse.X > rect.Left && mouse.X < rect.Right;
			bool inYBounds = mouse.Y > rect.Top && mouse.Y < rect.Bottom;

			// Make sure the button can only be pressed for 1 frame and not over and over again if the
			// mouse button is held down
			if (!isPressed) {
				// If the mouse is within the button bounds and the left mouse button is pressed,
				// execute the delegate method that was stored within the button
				if ((inXBounds && inYBounds) && mouse.LeftButton == ButtonState.Pressed) {
					isPressed = true;

					// Run the method
					onClick( );
				}
			} else {
				// Reset the isPressed variable when the mouse button is released to allow for the ui button
				// to be pressed again
				if (mouse.LeftButton == ButtonState.Released) {
					isPressed = false;
				}
			}
		}

		public override void Draw (SpriteBatch spriteBatch) {
			SpriteManager.DrawImage(spriteBatch, Main.buttonTexture, rect);
			SpriteManager.DrawText(spriteBatch, rect, text, Color.Black);
		}
	}
}

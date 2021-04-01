using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoZombie {
	public abstract class UIElement {
		// How much to scale the UI up by in-game
		public static int UIScale = 1;

		protected Point position;
		protected Rectangle rect;
		protected Texture2D texture;

		public UIElement (Texture2D texture, Point position) {
			this.texture = texture;
			this.position = position;

			// Get the dimensions of the button as well as the position
			int rectWidth = texture.Width * UIScale;
			int rectHeight = texture.Height * UIScale;
			int rectX = position.X - (rectWidth / 2);
			int rectY = position.Y - (rectHeight / 2);
			rect = new Rectangle(rectX, rectY, rectWidth, rectHeight);
		}

		public UIElement (Vector2 dimensions, Point position) {
			this.position = position;

			// Get the dimensions of the button as well as the position
			int rectWidth = (int) dimensions.X * UIScale;
			int rectHeight = (int) dimensions.Y * UIScale;
			int rectX = position.X - (rectWidth / 2);
			int rectY = position.Y - (rectHeight / 2);
			rect = new Rectangle(rectX, rectY, rectWidth, rectHeight);
		}

		public UIElement (Rectangle rect) {
			this.rect = rect;
		}

		public abstract void Update (GameTime gameTime, MouseState mouse);

		public virtual void Draw (SpriteBatch spriteBatch) {
			spriteBatch.Draw(texture, rect, Color.White);
		}

		/*
		 * Create a button UI object
		 * 
		 * Texture2D texture				: The button texture
		 * Point position					: The position of the button on the screen (this position is the center of the button)
		 * SpriteFont font					: The font that text overlayed onto the button is
		 * string text						: The text ovelayed onto the button
		 * 
		 * return UIButton					: The UIButton object created with the specified parameters
		 */
		public static UIButton CreateButton (Texture2D texture, Point position, Action onClick, SpriteFont font, int fontSize, string text) {
			// Get the dimensions of the button as a rectangle object based on its texture
			int textRectWidth = texture.Width * UIScale;
			int textRectHeight = texture.Height * UIScale;
			int textRectX = position.X - (textRectWidth / 2);
			int textRectY = position.Y - (textRectHeight / 2);
			Rectangle textRect = new Rectangle(textRectX, textRectY, textRectWidth, textRectHeight);
			
			// Create a UIText object that is going to be overlayed onto the button
			UIText buttonText = new UIText(font, fontSize, text, Color.Black, textRect);

			return new UIButton(texture, buttonText, position, onClick);
		}
	}
}

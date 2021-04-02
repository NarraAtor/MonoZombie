using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

// Author : Frank Alfano
// Purpose : A class that has methods to draw images and text to the screen with a lot of customization

namespace MonoZombie {
	public static class SpriteManager {
		// How much to scale the UI up by in-game
		// * This should be modified in the Game class
		public static float UIScale = 5;
		public static float ObjectScale = 2f;

		/*
		 * Author : Frank Alfano
		 * 
		 * Draw an image to the screen
		 * 
		 * SpriteBatch spriteBatch					: The spritebatch used to draw the images
		 * Texture2D texture						: The texture to draw
		 * Vector2 position							: The position of the image
		 * bool centered							: Whether or not the position given should be the where the center of the image lies or the top-left corner
		 * 
		 * return									: 
		 */
		public static void DrawImage (SpriteBatch spriteBatch, Texture2D texture, Vector2 position, bool centered = false, float angle = -1, float scale = 1) {
			// Get the dimensions of the image
			Vector2 imageDimensions = texture.Bounds.Size.ToVector2( ) * scale;

			// Get the draw rectangle of the image
			Rectangle drawRect = new Rectangle(position.ToPoint( ), imageDimensions.ToPoint( ));

			// If the image needs to be rotated, then draw it differently that a sprite that does not need to be rotated. This prevents
			// the sprite from being drawn/rotated around the wrong position
			if (angle >= 0) {
				spriteBatch.Draw(texture, drawRect, null, Color.White, angle, texture.Bounds.Size.ToVector2( ) / 2f, SpriteEffects.None, 1f);
			} else {
				// If the image is to be centered at the position given, shift the position the image is drawn at
				if (centered) {
					position = new Vector2(position.X - (imageDimensions.X / 2), position.Y - (imageDimensions.Y / 2));
				}

				// Recalculate the draw rectangle of the image
				drawRect = new Rectangle(position.ToPoint( ), imageDimensions.ToPoint( ));

				spriteBatch.Draw(texture, drawRect, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1f);
			}
		}

		/*
		 * Author : Frank Alfano
		 * 
		 * Draw a string of text to the screen
		 * 
		 * SpriteBatch spriteBatch					: The spritebatch used to draw the images
		 * float fontScale							: How much to scale up or down the font size
		 * string text								: The string of text to draw
		 * Color color								: The color of the text
		 * Vector2 position							: The position of the text
		 * bool centered							: Whether or not the position given should be the where the center of the text lies or the top-left corner
		 * 
		 * return									: 
		 */
		public static void DrawText (SpriteBatch spriteBatch, float fontScale, string text, Color color, Vector2 position, bool centered = false) {
			// Calculate the width and height of the text
			Vector2 textDimensions = Game1.font.MeasureString(text) * fontScale;

			// If the text is to be centered at the position given, shift the position the text is drawn at
			if (centered) {
				position = new Vector2(position.X - (textDimensions.X / 2), position.Y - (textDimensions.Y / 2));
			}

			// Draw the text to the screen
			spriteBatch.DrawString(Game1.font, text, position, color, 0, Vector2.Zero, fontScale, SpriteEffects.None, 1f);
		}
	}
}

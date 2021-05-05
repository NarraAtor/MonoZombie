using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

// Author : Frank Alfano
// Purpose : A class that has methods to draw images and text to the screen with a lot of customization

namespace MonoZombie {
	public static class SpriteUtils {
		// How much to scale the UI and game objects up by in-game
		public const float UI_SCALE = 5f;
		public const float OBJECT_SCALE = 4f;
		public const float PARTICLE_SCALE = 0.5f;

		/*
		 * Author : Frank Alfano
		 * 
		 * Draw an image to the screen
		 * 
		 * SpriteBatch spriteBatch					: The spritebatch used to draw the images
		 * Texture2D texture						: The texture to draw
		 * Vector2 position							: The position of the image
		 * float scale								: How much to scale the image up by
		 * bool isCentered							: Whether or not the position given should be the where the center of the image lies or the top-left corner
		 * 
		 * return									: 
		 */
		public static void DrawImage (SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Color tint, float scale = OBJECT_SCALE, bool isCentered = false) {
			// Get the dimensions of the image
			Vector2 imageDimensions = texture.Bounds.Size.ToVector2( ) * scale;

			// If the image is to be centered at the position given, shift the position the image is drawn at
			if (isCentered) {
				position = new Vector2(position.X - (imageDimensions.X / 2), position.Y - (imageDimensions.Y / 2));
			}

			// Get the draw rectangle of the image
			Rectangle drawRect = new Rectangle(position.ToPoint( ), imageDimensions.ToPoint( ));

			spriteBatch.Draw(texture, drawRect, null, tint, 0, Vector2.Zero, SpriteEffects.None, 1f);
		}

		/*
		 * Author : Frank Alfano
		 * 
		 * Draw an image to the screen
		 * 
		 * SpriteBatch spriteBatch					: The spritebatch used to draw the images
		 * Texture2D texture						: The texture to draw
		 * Rectangle rect							: The rectangle bounds of the image
		 * float angle								: The angle (in radians) that the image is rotated at
		 * 
		 * return									: 
		 */
		public static void DrawImage (SpriteBatch spriteBatch, Texture2D texture, Rectangle rect, Color tint, float angle = 0) {
			// The draw rectangle needs to be shifted because when you use SpriteBatch.draw(), it draws the texture at the top left corner
			// of the rectangle instead of in the center. This line of code below just shifts the rectangle so it draws the texture in the middle.
			Rectangle drawRect = new Rectangle(rect.X + (rect.Width / 2), rect.Y + (rect.Height / 2), rect.Width, rect.Height);

			spriteBatch.Draw(texture, drawRect, null, tint, angle, texture.Bounds.Size.ToVector2( ) / 2f, SpriteEffects.None, 1f);
		}

		/*
		 * Author : Frank Alfano
		 * 
		 * Draw a string of text to the screen
		 * 
		 * SpriteBatch spriteBatch					: The spritebatch used to draw the images
		 * Rectangle rect							: The rectangle bounds of the text
		 * string text								: The string of text to draw
		 * Color color								: The color of the text
		 * float fontScale							: How much to scale up or down the font size
		 * bool isCentered							: Whether or not the position given should be the where the center of the text lies or the top-left corner
		 * 
		 * return									: 
		 */
		public static void DrawText (SpriteBatch spriteBatch, Rectangle rect, string text, Color color, float fontScale = 1, bool isCentered = true) {
			// Calculate the width and height of the text
			Vector2 textDimensions = Main.font.MeasureString(text) * fontScale;

			// If the text is to be centered at the position given, shift the position the text is drawn at
			Vector2 textPosition = (isCentered) ? rect.Center.ToVector2( ) - (textDimensions / 2) : rect.Location.ToVector2( );

			// Draw the text to the screen
			spriteBatch.DrawString(Main.font, text, textPosition, color, 0, Vector2.Zero, fontScale, SpriteEffects.None, 1f);
		}

		/*
		 * Author : Frank Alfano
		 * 
		 * Draw a string of text to the screen
		 * 
		 * SpriteBatch spriteBatch					: The spritebatch used to draw the images
		 * Vector2 position							: The position of the text
		 * string text								: The string of text to draw
		 * Color color								: The color of the text
		 * float fontScale							: How much to scale up or down the font size
		 * bool isCentered							: Whether or not the position given should be the where the center of the text lies or the top-left corner
		 * 
		 * return									: 
		 */
		public static void DrawText (SpriteBatch spriteBatch, Vector2 position, string text, Color color, float fontScale = 1, bool isCentered = false) {
			// Calculate the width and height of the text
			Vector2 textDimensions = Main.font.MeasureString(text) * fontScale;

			// If the text is to be centered at the position given, shift the position the text is drawn at
			if (isCentered) {
				position = new Vector2(position.X - (textDimensions.X / 2), position.Y - (textDimensions.Y / 2));
			}

			// Draw the text to the screen
			spriteBatch.DrawString(Main.font, text, position, color, 0, Vector2.Zero, fontScale, SpriteEffects.None, 1f);
		}

		/*
		 * Author : Frank Alfano
		 * 
		 * Get a rectangle that encompasses the scaled texture around the position given
		 * 
		 * Texture2D texture						: The texture to get the rectangle around (this is used to get the dimensions of the rectangle)
		 * Vector2 position							: The position of the rectangle
		 * float scale								: How much to scale up the dimensions of the given texture by
		 * bool isCentered							: Whether or not the given position is to be the center or the top-left corner of the rectangle
		 */
		public static Rectangle GetBoundingRect (Texture2D texture, Vector2 position, float scale = 1, bool isCentered = true) {
			// Calculate the dimensions of the bounding rect
			Vector2 rectSize = texture.Bounds.Size.ToVector2( ) * scale;
			Vector2 rectPosition = position;

			// If the position given for the bounding rectangle is supposed to be the center of the rectangle, adjust the final bounding rectangle
			// accordingly
			if (isCentered) {
				rectPosition -= rectSize / 2;
			}

			return new Rectangle(rectPosition.ToPoint( ), rectSize.ToPoint( ));
		}

		public static Rectangle GetBoundingRect (string text, Vector2 position, float scale = 1, bool isCentered = true) {
			// Calculate the dimensions of the bounding rect
			Vector2 rectSize = Main.font.MeasureString(text) * scale;
			Vector2 rectPosition = position;

			// If the position given for the bounding rectangle is supposed to be the center of the rectangle, adjust the final bounding rectangle
			// accordingly
			if (isCentered) {
				rectPosition -= rectSize / 2;
			}

			return new Rectangle(rectPosition.ToPoint( ), rectSize.ToPoint( ));
		}

		/*
		 * Author : Frank Alfano
		 * 
		 * Draw a solid color rectangle to the screen for debug purposes
		 * 
		 * SpriteBatch spriteBatch					: The SpriteBatch object used for the game
		 * GraphicsDeviceManager graphics			: The GraphicsDeviceManager object used for the game
		 * Rectangle rect							: The rectangle bounds of the debug rect
		 * Color color								: The color of the debug rectangle
		 */
		public static void DrawRect (SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Rectangle rect, Color color, bool isFilled = true, float opacity = 1) {
			// Create a blank rectangular texture
			Texture2D rectTexture = new Texture2D(graphics.GraphicsDevice, rect.Width, rect.Height);

			color = new Color(color.R, color.G, color.B, opacity);

			// Create a blank color array the size of the debug texture
			Color[ ] data = new Color[rect.Width * rect.Height];

			// Set all of the color values in the array to the color specified
			for (int x = 0; x < rect.Width; x++) {
				for (int y = 0; y < rect.Height; y++) {
					if (!isFilled) {
						if ((x != 0 && x != rect.Width - 1) && (y != 0 && y != rect.Height - 1)) {
							continue;
						}
					}

					data[(x * rect.Height) + y] = color;
				}
			}

			// Apply the color changes to the blank debug texture
			rectTexture.SetData(data);

			// Draw the debug rectangle
			spriteBatch.Draw(rectTexture, rect, Color.White);
		}
	}
}

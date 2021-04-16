using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

// Author : Frank Alfano
// Purpose : An overarching class that deals with all UI elements with special features (like buttons)

namespace MonoZombie {
	public abstract class UIElement {
		protected Rectangle rect;
		protected bool isCentered;

		public UIElement (Texture2D texture, Vector2 position, bool isCentered = true) {
			this.isCentered = isCentered;

			rect = SpriteManager.GetBoundingRect(texture, position, scale: SpriteManager.UIScale, isCentered: this.isCentered);
		}

		public abstract void Update (GameTime gameTime, MouseState mouse);

		public abstract void Draw (SpriteBatch spriteBatch);
	}
}

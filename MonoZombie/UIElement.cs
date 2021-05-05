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
		private Texture2D texture;
		private Vector2 position;

		protected Rectangle rect;
		protected float scale;
		protected bool isCentered;

		public UIElement (Texture2D texture, Vector2 position, bool isCentered = true) {
			this.texture = texture;
			this.position = position;
			this.isCentered = isCentered;

			rect = SpriteUtils.GetBoundingRect(texture, position, scale: SpriteUtils.UI_SCALE, isCentered: isCentered);
		}

		public virtual void Update (GameTime gameTime, MouseState mouse) {
			rect = SpriteUtils.GetBoundingRect(texture, position, scale: scale, isCentered: isCentered);
		}

		public abstract void Draw (SpriteBatch spriteBatch);
	}
}

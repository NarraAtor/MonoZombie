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

		protected Rectangle rect;
		protected float buttonScale;
		protected bool isCentered;

		public Vector2 Position {
			get;
			private set;
		}

		public UIElement (Texture2D texture, Vector2 position, bool isCentered = true) {
			this.texture = texture;
			Position = position;
			this.isCentered = isCentered;

			rect = SpriteUtils.GetBoundingRect(texture, position, scale: SpriteUtils.UI_SCALE, isCentered: isCentered);
		}

		public virtual void Update (GameTime gameTime, MouseState mouse) {
			rect = SpriteUtils.GetBoundingRect(texture, Position, scale: buttonScale, isCentered: isCentered);
		}

		public abstract void Draw (SpriteBatch spriteBatch);
	}
}

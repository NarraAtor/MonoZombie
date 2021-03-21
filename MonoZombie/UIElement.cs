using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoZombie {
	public abstract class UIElement {
		protected Rectangle rect;
		protected Texture2D texture;

		public UIElement (Texture2D texture, Rectangle rect) {
			this.texture = texture;
			this.rect = rect;
		}

		public UIElement (Rectangle rect) {
			this.rect = rect;
		}

		public abstract void Update (GameTime gameTime, MouseState mouse);

		public virtual void Draw (SpriteBatch spriteBatch) {
			spriteBatch.Draw(texture, rect, Color.White);
		}
	}
}

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
		protected bool centered;

		public UIElement (Vector2 dimensions, Vector2 position, bool centered = false) : this(new Rectangle(position.ToPoint( ), dimensions.ToPoint( )), centered) {
		}

		public UIElement (Rectangle rect, bool centered = false) {
			this.rect = rect;
			this.centered = centered;

			// If the UIElement should be centered, shift its rectangle accordingly
			if (centered) {
				this.rect.Location -= (rect.Size.ToVector2( ) / 2).ToPoint( );
			}
		}

		public abstract void Update (GameTime gameTime, MouseState mouse);

		public abstract void Draw (SpriteBatch spriteBatch);
	}
}

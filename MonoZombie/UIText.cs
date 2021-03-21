using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoZombie {
	public class UIText : UIElement {
		private SpriteFont font;
		private string text;
		private Color color;

		public string Text {
			get {
				return text;
			}
			set {
				text = value;
			}
		}

		public UIText (SpriteFont font, string text, Color color, Rectangle rect) : base(rect) {
			this.font = font;
			this.text = text;
			this.color = color;
		}

		public override void Update (GameTime gameTime, MouseState mouse) { }

		public override void Draw (SpriteBatch spriteBatch) {
			spriteBatch.DrawString(font, text, rect.Location.ToVector2( ), color);
		}
	}
}

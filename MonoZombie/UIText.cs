using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoZombie {
	public class UIText : UIElement {
		private SpriteFont font;
		private int fontScale;
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

		public UIText (SpriteFont font, int fontScale, string text, Color color, Point position) : base(font.MeasureString(text), position) {
			this.font = font;
			this.fontScale = fontScale;
			this.text = text;
			this.color = color;
		}

		public UIText (SpriteFont font, int fontScale, string text, Color color, Rectangle rect) : base(rect) {
			this.font = font;
			this.fontScale = fontScale;
			this.text = text;
			this.color = color;
		}

		public override void Update (GameTime gameTime, MouseState mouse) { }

		public override void Draw (SpriteBatch spriteBatch) {
			// Calculate the centered position of the text within its rectangle
			float textWidth = font.MeasureString(Text).X * fontScale;
			float textHeight = font.MeasureString(Text).Y * fontScale;
			Vector2 textPosition = new Vector2(rect.Center.X - (textWidth / 2), rect.Center.Y - (textHeight / 2));

			spriteBatch.DrawString(font, text, textPosition, color, 0, Vector2.Zero, fontScale, SpriteEffects.None, 1f);
		}
	}
}

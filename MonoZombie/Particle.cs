using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoZombie {
	public class Particle : GameObject {
		private float startTime = -1;

		public float DurationSeconds {
			get;
			private set;
		}

		public string Text {
			get;
			private set;
		}

		public Color Color {
			get;
			private set;
		}

		public new Rectangle Rect {
			get {
				if (texture == null) {
					return SpriteUtils.GetBoundingRect(Text, Position, SpriteUtils.PARTICLE_SCALE);
				} else {
					return base.Rect;
				}
			}
		}

		public new bool IsOnScreen {
			get {
				if (texture == null) {
					return (Rect.Right >= 0 && Rect.Left <= Main.SCREEN_DIMENSIONS.X && Rect.Bottom >= 0 && Rect.Top <= Main.SCREEN_DIMENSIONS.Y);
				} else {
					return base.IsOnScreen;
				}
				
			}
		}

		public Particle (string text, Color color, Vector2 centerPosition, float durationSeconds, GameObject parent) : base(null, centerPosition, parent: parent, canMove: false) {
			Text = text;
			Color = color;
			DurationSeconds = durationSeconds;
		}

		public Particle (Texture2D texture, Color color, Vector2 centerPosition, float duration, GameObject parent) : base(texture, centerPosition, parent: parent, canMove: false) {
			Color = color;
			DurationSeconds = duration;
		}

		public void Update (GameTime gameTime) {
			if (startTime == -1) {
				startTime = (float) gameTime.TotalGameTime.TotalSeconds;
			}

			if (gameTime.TotalGameTime.TotalSeconds - startTime >= DurationSeconds) {
				Destroy( );
			}
		}

		public new void Draw (GameTime gameTime, SpriteBatch spriteBatch) {
			if (IsOnScreen) {
				if (texture != null) {
					SpriteUtils.DrawImage(spriteBatch, texture, Rect, Color, angle: Angle);
				} else {
					SpriteUtils.DrawText(spriteBatch, Position, Text, Color, fontScale: SpriteUtils.PARTICLE_SCALE);
				}
			}
		}
	}
}

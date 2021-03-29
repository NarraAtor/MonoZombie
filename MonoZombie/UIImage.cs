using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoZombie {
	public class UIImage : UIElement {
		public UIImage (Texture2D texture, Point position) : base(texture, position) { }

		public override void Update (GameTime gameTime, MouseState mouse) { }
	}
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using EVCS_Projekt.Location;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace EVCS_Projekt.Renderer
{
    public class StaticRenderer : IRenderBehavior
    {
        private Texture2D texture;

        // ***************************************************************************
        // Die Größe, des zu rendernen Objektes => Größe der Textur
        public Vector2 Size
        {
            get
            {
                return new Vector2(texture.Width, texture.Height);
            }
        }

        public StaticRenderer(Texture2D texture)
        {
            this.texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch, ILocationBehavior locationBehavoir)
        {
            // Texture zeichnen
            Draw(spriteBatch, locationBehavoir, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, ILocationBehavior locationBehavoir, Color color)
        {
            // Texture zeichnen
            spriteBatch.Draw(texture, locationBehavoir.RelativeBoundingBox, null, color, locationBehavoir.Rotation, locationBehavoir.Size / 2, SpriteEffects.None, 0);
        }

        // ***************************************************************************
        // Wird nichts geupdated.. imer die gleiche Textur
        public void Update()
        {
            // Nothing todo
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using EVCS_Projekt.Location;
using System.Diagnostics;

namespace EVCS_Projekt.Renderer
{
    public class AnimationRenderer : IRenderBehavior
    {
        private Texture2D[] textures;
        private float framesPerSecond;
        private float animationTimer;
        private float frameDuration;

        private delegate void UpdateDelegate ();
        private UpdateDelegate update;

        // ***************************************************************************
        // Aktuelle Framenummer
        public int Frame
        {
            get
            {
                return (int)(animationTimer / frameDuration);
            }
        }

        // ***************************************************************************
        // Die Größe, des zu rendernen Objektes => Größe der Textur
        public Vector2 Size
        {
            get
            {
                return new Vector2(textures[0].Width, textures[0].Height);
            }
        }

        public AnimationRenderer(Texture2D[] textures, float fps)
        {
            this.textures = textures;
            this.framesPerSecond = fps;
            this.animationTimer = 0;
            this.frameDuration = 1 / framesPerSecond;
            update = UpdateLoop;
        }

        public void Draw(SpriteBatch spriteBatch, ILocationBehavior locationBehavoir)
        {
            // Texture zeichnen
            Draw(spriteBatch, locationBehavoir, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, ILocationBehavior locationBehavoir, Color color)
        {
            // Texture zeichnen
            spriteBatch.Draw(textures[Frame], locationBehavoir.RelativeBoundingBox, null, color, locationBehavoir.Rotation, locationBehavoir.Size / 2, SpriteEffects.None, 0);
        }

        // ***************************************************************************
        // Frame weiterzählen
        public void Update()
        {
            update();
        }

        // ***************************************************************************
        // Frame weiterzählen - Schleife
        private void UpdateLoop()
        {
            // Gametime auf timer adden
            animationTimer += (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds;

            // animationTimer resetten
            if (textures.Length / framesPerSecond <= animationTimer)
            {
                animationTimer = 0;
            }

        }

        // ***************************************************************************
        // Frame weiterzählen - Einmal abspielen dann stop
        private void UpdateOnce()
        {
            // Gametime auf timer adden - aber nur wenn es nicht durchgelaufen ist
            if (textures.Length / framesPerSecond > animationTimer + (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds )
            {
                animationTimer += (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds;
            }

        }

        // ***************************************************************************
        // Frame weiterzählen
        public void PlayOnce()
        {
            update = UpdateOnce;
        }

        // ***************************************************************************
        // Animation auf 0 setzten
        public void ResetAnimation()
        {
            animationTimer = 0;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace EVCS_Projekt.UI
{
    abstract class UIElement
    {
        protected int width
        {
            get
            {
                return ( int ) ( Helper.DrawHelper.Get( this.GetHashCode() + "Size" ).X );
            }
        }
        protected int height
        {
            get
            {
                return ( int ) ( Helper.DrawHelper.Get( this.GetHashCode() + "Size" ).Y );
            }
        }
        protected Vector2 position
        {
            get
            {
                Vector2 pos = Helper.DrawHelper.Get( this.GetHashCode() + "Position" );
                int x = ( int ) ( pos.X );
                int y = ( int ) ( pos.Y );

                if ( parent != null )
                {
                    x += ( int ) parent.GetPosition().X;
                    y += ( int ) parent.GetPosition().Y;
                }

                return new Vector2( x, y );
            }
            private set
            {
                string key = this.GetHashCode() + "Position";
                Helper.DrawHelper.AddDimension( key, ( int ) value.X, ( int ) value.Y );
            }
        }

        public virtual Texture2D CurrentTexture
        {
            get
            {
                if ( isHover )
                {
                    return hoverTexture;
                }
                else
                    return texture;
            }
        }

        private Texture2D texture;
        private Texture2D hoverTexture;

        protected List<UIActionListener> listener;
        protected UIElement parent;
        protected bool isHover;
        private bool mouseDown;

        protected MouseState oldState = Mouse.GetState();

        public UIElement( int width, int height, Vector2 position )
        {
            string key = this.GetHashCode() + "Size";
            Helper.DrawHelper.AddDimension( key, width, height );

            this.position = position;
            listener = new List<UIActionListener>();
        }

        public UIElement( int width, int height, Vector2 position, Texture2D texture, Texture2D hoverTexture ) :
            this( width, height, position )
        {
            this.texture = texture;
            this.hoverTexture = hoverTexture;
        }


        public void SetParent( UIElement parent )
        {
            this.parent = parent;
        }

        public int GetHeight()
        {
            return height;
        }

        public int GetWidth()
        {
            return width;
        }

        public Vector2 GetPosition()
        {

            return position;
        }

        public void SetPosition( Vector2 pos )
        {
            position = pos;
        }

        // Wird von den Unterklassen überschrieben
        public virtual void Draw( SpriteBatch sb )
        {
        }

        // Wird von den Unterklassen überschrieben
        public virtual void Update()
        {


            // Wenn nicht mehr gedrückt, aber im vorherigen Durchgang gedrückt war => Man kann die Maustaste gedrückt halten ohne das jedesmal ein Event ausgelöst wird
            if ( IsMousePressed() )
            {
                List<UIActionListener> listenerList = new List<UIActionListener>( listener );
                foreach ( UIActionListener al in listenerList )
                {
                    al.OnMouseDown( this );
                }
            }


            //if ( state.LeftButton == ButtonState.Pressed )
            //{
            //    mouseDown = true;
            //}
            //else
            //{
            //    mouseDown = false;
            //}
        }


        protected bool IsMouseOver()
        {
            MouseState state = Mouse.GetState();
            int mX = state.X;
            int mY = state.Y;
            int x = ( int ) ( GetPosition().X );
            int y = ( int ) ( GetPosition().Y );
            int w = GetWidth();
            int h = GetHeight();

            if ( mX > x && mX < x + w && mY > y && mY < y + h )
            {
                isHover = true;
                return true;
            }
            else
            {
                isHover = false;
                return false;
            }
        }


        protected bool IsMousePressed()
        {

            MouseState newState = Mouse.GetState();

            bool isPressed = IsMouseOver() && oldState.LeftButton != ButtonState.Pressed && newState.LeftButton == ButtonState.Pressed;

            oldState = newState;
            
            return isPressed;
        }

        public void AddActionListener( UIActionListener al )
        {
            listener.Add( al );
        }

        public void RemoveActionListener( UIActionListener al )
        {
            listener.Remove( al );
        }
    }
}

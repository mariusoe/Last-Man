﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using EVCS_Projekt.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EVCS_Projekt.UI
{
    class UIList : UIPanel, UIActionListener
    {
        private  List<UIElement> buttonList;

        public Dictionary<int,int> ItemList
        {
            get { return itemList; }
            set { itemList = value;
            GenerateButtons();}
        }


        private  Dictionary<int, int> countItemsDict;

        private int firsVisibleButtonIndex = 0;
        private int MAX_VISIBLE_BUTTON_COUNT = 5;
        private const int DEFAULT_HEIGHT = 48;
        private Rectangle box;

        private readonly UIButton btnPrevious;
        private readonly UIButton btnNext;
        private Dictionary<int, int> itemList;

        public UIList( int width, int height, Vector2 position)
            : base( width, height, position )
        {
            MAX_VISIBLE_BUTTON_COUNT = height/DEFAULT_HEIGHT;
            itemList = new Dictionary<int,int>();
            buttonList = new List<UIElement>();
            countItemsDict = new Dictionary<int, int>();
            
            var imgPreviousButton = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/list_previous" );
            var imgPreviousButtonHover = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/list_previous_hover" );

            var imgNextButton = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/list_next" );
            var imgNextButtonHover = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/list_next_hover" );

            btnPrevious = new UIButton( width, imgPreviousButton.Height, new Vector2( 0, 0 ), imgPreviousButton, imgPreviousButtonHover, "" );
            btnNext = new UIButton( width, imgNextButton.Height, new Vector2( 0, height - imgNextButton.Height ), imgNextButton, imgNextButtonHover, "" );

            

            btnPrevious.AddActionListener( this );
            btnNext.AddActionListener( this );
        }

        public void AddItemList(Dictionary<int, int> list)
        {
            foreach (KeyValuePair<int, int> pair in list)
            {
                ItemList.Add(pair.Key, pair.Value);
                GenerateButtons();
            }
        }

        public void RemoveItems(Dictionary<int, int> list)
        {
            foreach (KeyValuePair<int, int> pair in list)
            {
                if (ItemList.ContainsKey(pair.Key))
                    ItemList.Remove(pair.Key);
            }
            GenerateButtons();
        }

        private void GenerateButtons()
        {
            buttonList.Clear();
            int i = 0;
            foreach(int typeId in itemList.Keys)
            {
                Item item = Item.Get(typeId);
                var x = ( int ) ( position.X );
                var y = (int)((50 * i)) + btnPrevious.GetHeight();
                var button = new UIListButton(width, 24, new Vector2(0, y), item.Icon, item.Name, itemList[typeId], item.Weight);

                buttonList.Add( button );
                i++;
            }
        }

        public override void Draw( SpriteBatch sb )
        {
            Clear();
//            box = new Rectangle((int)position.X, (int)position.Y, width, height);
//            sb.Draw( Main.ContentManager.Load<Texture2D>( "images/pixelWhite" ), box, Color.Fuchsia );
            Add( btnPrevious );

            if(buttonList.Count>0)
            {
                int j = 0;
                for (int i = firsVisibleButtonIndex; (i < MAX_VISIBLE_BUTTON_COUNT + firsVisibleButtonIndex) && (i < buttonList.Count) ; i++, j++)
                {

                    UIListButton button = (UIListButton)buttonList[i];
                    button.SetPosition(new Vector2(0, j * 50 + 18));
                    Add(buttonList[i]);
                }
            }
            
            Add( btnNext );
            base.Draw( sb );
        }


        /// <summary>
        /// Ein Button fuer die Liste
        /// </summary>
        private class UIListButton : UIPanel
        {
            private readonly UIButton btnIcon;
            private readonly UIButton btnName;
            private readonly UIButton btnItemCount;
            private readonly UIButton btnWeightIcon;
            private readonly UIButton btnWeightCount;
            public Color Color;
            public Color HoverColor;

            public UIListButton( int width, int height, Vector2 position, Texture2D itemIcon, string name, int count, float weigth )
                : base(width, DEFAULT_HEIGHT, position)
            {
                var weightIcon = Main.ContentManager.Load<Texture2D>( "images/gui/inventar/weight" );

                this.btnIcon = new UIButton(DEFAULT_HEIGHT,DEFAULT_HEIGHT, new Vector2( 0, 0 ), itemIcon, itemIcon, "");
                this.btnName = new UIButton(width - DEFAULT_HEIGHT*4, DEFAULT_HEIGHT, new Vector2(DEFAULT_HEIGHT, 0), name);
                this.btnItemCount = new UIButton(DEFAULT_HEIGHT, DEFAULT_HEIGHT, new Vector2(width - DEFAULT_HEIGHT * 3, 0), count + "");
                this.btnWeightIcon = new UIButton(DEFAULT_HEIGHT, DEFAULT_HEIGHT, new Vector2(width - DEFAULT_HEIGHT * 2, 0), weightIcon, weightIcon, "");
                this.btnWeightCount = new UIButton(DEFAULT_HEIGHT, DEFAULT_HEIGHT, new Vector2(width - DEFAULT_HEIGHT, 0), weigth.ToString());

//                btnIcon.BackgroundColor = Color.Peru;
//                btnName.BackgroundColor = Color.Salmon;
//                btnItemCount.BackgroundColor = Color.Green;
//                btnWeightIcon.BackgroundColor = Color.RoyalBlue;
//                btnWeightCount.BackgroundColor = Color.White;

                Color = Color.Gray;
                HoverColor = Color.White;

                Add( this.btnIcon );
                Add( this.btnName );
                Add( this.btnItemCount );
                Add( this.btnWeightIcon );
                Add( this.btnWeightCount );
            }

            public override void  Draw(SpriteBatch sb)
            {
                Color active;
                if (IsMouseOver())
                {
                    active = HoverColor;
                }
                else
                {
                    active = Color;
                }

                foreach (UIButton button in children)
                    button.BackgroundColor = active;
                sb.Draw(Main.ContentManager.Load<Texture2D>("images/pixelWhite"), new Rectangle((int)position.X, (int)position.Y, width, height), active);

                base.Draw(sb);
            }
        }

        public void OnMouseDown(UIElement element)
        {
            
                if (element == btnPrevious && firsVisibleButtonIndex > 0)
                {
                    firsVisibleButtonIndex--;
                }
                   
                else if (element == btnNext)
                {
                    if ((firsVisibleButtonIndex + MAX_VISIBLE_BUTTON_COUNT) < buttonList.Count)
                    {
                        firsVisibleButtonIndex++;
                    }
                }
            
        }

        

        public void OnMouseUp(UIElement element)
        {
            // leer
        }
    }
}

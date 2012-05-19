using System;
using EVCS_Projekt.Location;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt.Objects.Items
{
    /**
     * @UMLVersion = 12.Mai2012
     * @Last Changes = 14.Mai2012
     *
     */
    public class Weapon : Item
    {

        //Attributes
        private Visier visier;
        private Munition munition;
        public Munition Munition
        {
            get
            {
                return munition;
            }
            set
            {
                munition = value;
            }
        }
        private Antrieb antrieb;
        private Stabilisator stabilisator;
        private Hauptteil hauptteil;

        //Constructor
        public Weapon(Game game, Visier visier, Antrieb antrieb, Stabilisator stabilisator, Hauptteil hauptteil, int id, EGroup group, String name, float weight, string description, ILocationBehavior locationBehavior)
            : base(   game,  id,  group,  name,  description,  weight,  locationBehavior)
        {
            this.visier = visier;
            this.antrieb = antrieb;
            this.stabilisator = stabilisator;
            this.hauptteil = hauptteil;
        }

        //Functions
        public Item Shoot (float accuratcy) 
        {
            return null; //Schuss generiert neues Item
        }

    }
}
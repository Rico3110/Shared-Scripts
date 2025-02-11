﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.HexGrid;
using UnityEngine;
using Shared.DataTypes;

namespace Shared.Structures
{
    public abstract class Building : Structure
    {
        public byte Tribe;       
        public byte Level;
        public byte Health;

        public abstract byte MaxLevel { get; }

        public abstract string description { get; }

        public byte MaxHealth { get { return MaxHealths[Level - 1]; } }
        public abstract byte[] MaxHealths { get; }

        public abstract Dictionary<RessourceType, int>[] Recipes { get; }

        public Building() : base()
        {
            this.Tribe = 255;
            this.Level = 1;
            this.Health = MaxHealth;
        }

        public Building(HexCell Cell, byte Tribe, byte Level, byte Health) : base(Cell)
        {
            this.Tribe = Tribe;
            this.Level = Level;
            this.Health = Health;
        }

        public override void DoTick()
        {
            this.increaseHealth(1);
        }

        private void increaseHealth(byte count)
        {
            this.Health = (byte)Mathf.Min(this.MaxHealth, this.Health + count);
        }

        public virtual void Upgrade()
        {
            if(Level < MaxLevel)
                Level++;
        }

        public virtual void Downgrade()
        {
            if (Level > 0)
                Level--;
            Health = MaxHealth;
        }

        public override bool IsPlaceable(HexCell cell) 
        {
            List<Building> buildings = cell.GetNeighborStructures<Building>(2);
            if (buildings.FindIndex(elem => elem.Tribe != this.Tribe) != -1)
                return false;

            if (cell.Structure != null && typeof(Building).IsAssignableFrom(cell.Structure.GetType()))
            {
                return false;
            }
            
            return true;
        }

        public bool IsUpgradable()
        {
            return (Level < MaxLevel);
        }
    }
}

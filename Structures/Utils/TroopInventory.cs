using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTypes;
using UnityEngine;

namespace Shared.Structures
{
    public class TroopInventory
    {
        public Dictionary<TroopType, int> Troops;

        public List<Tuple<TroopType, bool>> Strategy;

        public int TroopLimit;

        public TroopInventory()
        {
            TroopLimit = 20;
            Troops = new Dictionary<TroopType, int>() { { TroopType.ARCHER, 0 }, { TroopType.KNIGHT, 0 }, { TroopType.SPEARMAN, 0 } };
            
            Strategy = new List<Tuple<TroopType, bool>>();
            Strategy.Add(new Tuple<TroopType, bool>(TroopType.ARCHER, true));
            Strategy.Add(new Tuple<TroopType, bool>(TroopType.KNIGHT, true));
            Strategy.Add(new Tuple<TroopType, bool>(TroopType.SPEARMAN, true));
        }

        public TroopInventory(Dictionary<TroopType, int> troops, int troopLimit, List<Tuple<TroopType, bool>> strategy)
        {
            this.TroopLimit = troopLimit;
            this.Strategy = strategy;
            this.Troops = troops;
        }

        public void UpdateStrategy(List<Tuple<TroopType, bool>> strategy)
        {
            if (strategy.Count > 3)
                return;
            this.Strategy = strategy;
        }

        public void UpdateStrategy(int oldIndex, int newIndex)
        {
            Tuple<TroopType, bool> tpl = this.Strategy[oldIndex]; 
            this.Strategy.RemoveAt(oldIndex);
            this.Strategy.Insert(newIndex, tpl);
        }

        public void UpdateTroopLimit(int newValue)
        {
            this.TroopLimit = newValue;
        }
        
        public void AddUnit(TroopType type, int amount)
        {
            this.Troops[type] += Mathf.Min(amount, this.GetAvailableSpace());
        }

        public void RemoveUnit(TroopType type, int amount)
        {
            if (amount < 0)
                return;
            this.Troops[type] -= Mathf.Min(amount, this.Troops[type]);
        }

        public bool MoveTroops(TroopInventory destination, TroopType troopType, int amount)
        {
            if (this.Troops[troopType] - amount >= 0 && destination.GetTroopCount() + amount <= destination.TroopLimit)
            {
                //Move troops
                this.Troops[troopType] -= amount;
                destination.Troops[troopType] += amount;
                return true;
            }
            return false;
        }

        public int GetAvailableSpace()
        {
            return TroopLimit - this.Troops.Values.Aggregate((agg, elem) => agg += elem);
        }

        public int GetTroopCount()
        {
            return this.Troops.Values.Aggregate((agg, elem) => agg += elem);
        }

        public bool Fight(TroopInventory defender)
        {
            TroopInventory attacker = this;

            Troop attackerTroop = attacker.GetInitialTroop();
            Troop defenderTroop = defender.GetInitialTroop();

            while(attackerTroop != null && defenderTroop != null)
            {
                attackerTroop.Fight(defenderTroop);
                if (attackerTroop.health <= 0)
                {
                    attacker.RemoveUnit(attackerTroop.type, 1);
                    attackerTroop = attacker.GetNextTroop(attackerTroop.type);
                }
                if (defenderTroop.health <= 0)
                {
                    defender.RemoveUnit(defenderTroop.type, 1);
                    defenderTroop = defender.GetNextTroop(defenderTroop.type);
                }
            }
            return defender.GetTroopCount() > 0 ? false : true;
        }

        public bool Fight(Building building)
        {
            Troop troop = GetInitialTroop();
            while (troop != null)
            {
                building.Health -= 1;
                this.RemoveUnit(troop.type, 1);
                GetNextTroop(troop.type);
                if (building.Health <= 0)
                    return true;
            }
            return false;
        }

        protected Troop GetInitialTroop()
        {
            for (int i = 0; i < this.Strategy.Count; i++)
            {
                if (this.Troops[this.Strategy[i].Item1] > 0)
                    return new Troop(this.Strategy[i].Item1);
            }
            return null;
        }

        protected Troop GetNextTroop(TroopType currentType)
        {
            try 
            {
                int foundIndex = this.Strategy.FindIndex(elem => elem.Item1 == currentType);

                if (Strategy[foundIndex].Item2 == true)
                {
                    for (int i = foundIndex + 1; i < this.Strategy.Count; i++)
                    {
                        if (this.Strategy[i].Item2 == false)
                            break;

                        if (this.Troops[this.Strategy[i].Item1] > 0)
                        {
                            return new Troop(this.Strategy[i].Item1);
                        }
                    }
                    return GetInitialTroop();
                }
                else if(Troops[currentType] > 0)
                {
                    return new Troop(currentType); 
                }
                else
                {
                    return GetInitialTroop();
                }
            }
            catch(Exception e)
            {
                return GetInitialTroop();
            }            
        }

        protected class Troop 
        {
            public int health;
            public TroopType type;

            public Troop(TroopType type)
            {
                this.type = type;
                this.health = 12;
            }

            public void Fight(Troop troop)
            {
                if (this.type == troop.type)
                {
                    //Console.WriteLine(this.type.ToString() + " Draw " + troop.type.ToString());
                    this.health -= 1;
                    troop.health -= 1;
                }
                else if (this.type == (TroopType)(((int)troop.type + 1) % 3))
                {
                    //Console.WriteLine(this.type.ToString() + " Wins " + troop.type.ToString());
                    this.health -= 1;
                    troop.health -= 6;
                }
                else 
                {
                    //Console.WriteLine(this.type.ToString() + " Loose " + troop.type.ToString());
                    this.health -= 6;
                    troop.health -= 1;
                }
            }
        }
    }
}

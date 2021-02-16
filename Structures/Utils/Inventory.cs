using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTypes;
using UnityEngine;

namespace Shared.Structures
{
    public class Inventory
    {
        public Dictionary<RessourceType, int> Storage;

        public int RessourceLimit;

        public Inventory()
        {
            this.Storage = new Dictionary<RessourceType, int>();
            this.RessourceLimit = 100;
        }

        public Inventory(
            Dictionary<RessourceType, int> Storage, 
            int RessourceLimit
        )
        {
            this.Storage = Storage;
            this.RessourceLimit = RessourceLimit;
        }

        public void AddRessource(RessourceType ressourceType)
        {
            if (this.Storage.ContainsKey(ressourceType))
                return;
            this.Storage.Add(ressourceType, 0);
        }

        public int GetRessourceAmount(RessourceType ressourceType)
        {
            if (Storage.ContainsKey(ressourceType))
                return Storage[ressourceType];
            
            return 0;
        }

        public virtual int AddRessource(RessourceType ressourceType, int count)
        {
            int availableSpace = 0;
            if (Storage.ContainsKey(ressourceType))
            {
                availableSpace = AvailableSpace();
                if (availableSpace > 0)
                {
                    Storage[ressourceType] += Mathf.Min(availableSpace, count);
                }
            }

            return Mathf.Min(availableSpace, count);
        }

        public int AddRessource(Dictionary<RessourceType, int> recipe)
        {
            int amount = 0;
            foreach(KeyValuePair<RessourceType, int> ressource in recipe)
            {
                amount += AddRessource(ressource.Key, ressource.Value);
            }
            return amount;
        }

        public bool RemoveRessource(RessourceType ressourceType, int amount)
        {
            if (GetRessourceAmount(ressourceType) >= amount)
            {
                Storage[ressourceType] -= amount;
                return true;
            }
            return false;
        }

        public int AvailableSpace()
        {
            int totalCount = 0;
            foreach (int count in Storage.Values)
            {
                totalCount += count;
            }
            return RessourceLimit - totalCount;
        } 

        public virtual void MoveInto(BuildingInventory receiver, int count)
        {
            Inventory sender = this;

            bool transmittedARessource = true;
            while (count > 0 && transmittedARessource)
            {
                transmittedARessource = false;
                List<RessourceType> iterList = new List<RessourceType>(sender.Storage.Keys);
                foreach (RessourceType ressourceType in iterList)
                {
                    if (count <= 0)
                    {
                        break;
                    }
                    if (receiver.Incoming.Contains(ressourceType))
                    {
                        int senderAmount = sender.GetRessourceAmount(ressourceType);
                        int received = receiver.AddRessource(ressourceType, Mathf.Min(senderAmount, 1));
                        count -= received;
                        if (received > 0)
                        {
                            transmittedARessource = true;
                        }
                        sender.RemoveRessource(ressourceType, received);
                    }
                }
            }
        }

        public bool RecipeApplicable(Dictionary<RessourceType, int> recipe)
        {
            foreach(RessourceType ressourceType in recipe.Keys)
            {
                if (this.GetRessourceAmount(ressourceType) < recipe[ressourceType])
                    return false;
            }
            return true;
        }

        public void ApplyRecipe(Dictionary<RessourceType, int> recipe)
        {
            if (!RecipeApplicable(recipe))
                return;
            foreach (RessourceType ressourceType in recipe.Keys)
            {
                this.RemoveRessource(ressourceType, recipe[ressourceType]);
            }
        }

        public static Dictionary<RessourceType, int> GetDictionaryForAllRessources()
        {
            Dictionary<RessourceType, int> dic = new Dictionary<RessourceType, int>();
            foreach(RessourceType ressourceType in Enum.GetValues(typeof(RessourceType)))
            {
                dic.Add(ressourceType, 0);
            }

            return dic;
        }

        public static List<RessourceType> GetListOfAllRessources()
        {
            List<RessourceType> list = new List<RessourceType>();
            foreach (RessourceType ressourceType in Enum.GetValues(typeof(RessourceType)))
            {
                list.Add(ressourceType);
            }

            return list;
        }

        public void Clear()
        {
            foreach(RessourceType ressourceType in Storage.Keys)
            {
                Storage[ressourceType] = 0;
            }
        }

        public bool IsEmpty()
        {
            foreach(int amount in this.Storage.Values)
            {
                if (amount > 0)
                    return false;
            }
            return true;
        }

        public RessourceType GetMainRessource()
        {
            RessourceType mainRessource = RessourceType.WOOD;
            int mainRessourceAmount = 0;

            foreach (RessourceType ressourceType in this.Storage.Keys)
            {
                if(Storage[ressourceType] > mainRessourceAmount)
                {
                    mainRessourceAmount = Storage[ressourceType];
                    mainRessource = ressourceType;
                }
            }

            return mainRessource;
        }
    }
}


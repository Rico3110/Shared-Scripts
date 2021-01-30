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

        public Dictionary<RessourceType, int> RessourceLimits;

        public List<RessourceType> Outgoing;

        public List<RessourceType> Incoming;

        public Inventory()
        {
            this.Storage = new Dictionary<RessourceType, int>();
            this.RessourceLimit = 100;
            this.RessourceLimits = new Dictionary<RessourceType, int>();
            this.Outgoing = new List<RessourceType>();
            this.Incoming = new List<RessourceType>();
        }

        public Inventory(
            Dictionary<RessourceType, int> Storage, 
            int RessourceLimit, 
            Dictionary<RessourceType, int> RessourceLimits,  
            List<RessourceType> Outgoing,
            List<RessourceType> Incoming
        )
        {
            this.Storage = Storage;
            this.RessourceLimit = RessourceLimit;
            this.UpdateRessourceLimits(RessourceLimits);
            this.UpdateOutgoing(Outgoing);
            this.UpdateIncoming(Incoming);
        }


        public int GetRessourceAmount(RessourceType ressourceType)
        {
            if (Storage.ContainsKey(ressourceType))
                return Storage[ressourceType];
            return 0;
        }

        public int AddRessource(RessourceType ressourceType, int count)
        {
            int availableSpace = 0;
            if (Storage.ContainsKey(ressourceType))
            {
                availableSpace = AvailableSpace(ressourceType);
                if (availableSpace > 0)
                {
                    Storage[ressourceType] += Mathf.Min(availableSpace, count);
                }
            }

            return Mathf.Min(availableSpace, count);
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

        public int AvailableSpace(RessourceType ressourceType)
        {
            int totalRessourceSpace = AvailableSpace();
            int localRessourceSpace = totalRessourceSpace;
            if (RessourceLimits.TryGetValue(ressourceType, out localRessourceSpace))
            {
                int count;
                if (Storage.TryGetValue(ressourceType, out count))
                    localRessourceSpace = localRessourceSpace - count;
                return Mathf.Min(totalRessourceSpace, localRessourceSpace);
            }
            else
            {
                return totalRessourceSpace;
            }
        }

        public void UpdateRessourceLimits(Dictionary<RessourceType, int> newRessourceLimits)
        {
            this.RessourceLimits.Clear();
            foreach(KeyValuePair<RessourceType, int> limit in newRessourceLimits)
            {
                if (Storage.ContainsKey(limit.Key))
                    this.RessourceLimits.Add(limit.Key, limit.Value);
            }
        }

        public void UpdateOutgoing(List<RessourceType> newOutgoing)
        {
            this.Outgoing.Clear();
            foreach(RessourceType ressourceType in newOutgoing)
            {
                if (Storage.ContainsKey(ressourceType))
                {
                    this.Outgoing.Add(ressourceType);
                }
            }
        }

        public void UpdateIncoming(List<RessourceType> newIncoming)
        {
            this.Incoming.Clear();
            foreach(RessourceType ressourceType in newIncoming)
            {
                if (Storage.ContainsKey(ressourceType))
                {
                    this.Incoming.Add(ressourceType);
                }
            }
        }

        public void MoveInto(Inventory receiver, int count)
        {
            Inventory sender = this;

            bool transmittedARessource = true;
            while (count > 0 && transmittedARessource)
            {
                transmittedARessource = false;
                foreach (RessourceType ressourceType in sender.Outgoing)
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
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTypes;
using UnityEngine;

namespace Shared.Structures
{
    public class BuildingInventory : Inventory
    {
        public Dictionary<RessourceType, int> RessourceLimits;

        public List<RessourceType> Outgoing;

        public List<RessourceType> Incoming;

        public BuildingInventory() : base()
        {
            this.RessourceLimits = new Dictionary<RessourceType, int>();
            this.Outgoing = new List<RessourceType>();
            this.Incoming = new List<RessourceType>();
        }

        public BuildingInventory(
            Dictionary<RessourceType, int> Storage, 
            int RessourceLimit, 
            Dictionary<RessourceType, int> RessourceLimits,  
            List<RessourceType> Outgoing,
            List<RessourceType> Incoming
        ) : base(
            Storage, 
            RessourceLimit
        )
        {
            this.UpdateRessourceLimits(RessourceLimits);
            this.UpdateOutgoing(Outgoing);
            this.UpdateIncoming(Incoming);
        }

        public override int AddRessource(RessourceType ressourceType, int count)
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

        public bool HasAvailableSpace(Dictionary<RessourceType, int> recipe)
        {
            foreach(KeyValuePair<RessourceType, int> kvp in recipe)
            {
                if (AvailableSpace(kvp.Key) > 0)
                    return true;
            }
            return false;
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

        public override void MoveInto(BuildingInventory receiver, int count)
        {
            BuildingInventory sender = this;

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
    }
}

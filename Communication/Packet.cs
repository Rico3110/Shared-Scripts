using System;
using System.Collections.Generic;
using System.Text;
using Shared.HexGrid;
using Shared.DataTypes;
using Shared.Structures;
using Shared.Game;

namespace Shared.Communication
{
    /// <summary>Sent from server to client.</summary>
    public enum ServerPackets
    {
        welcome = 1,
        ping = 2,
        initGameLogic = 3,
        sendStructure = 4,
        gameTick = 5,
        upgradeBuilding = 6,
        broadcastPlayer = 7,
        broadcastTribe = 8,
        applyBuild = 9,
        applyBuildHQ = 10,
        testBuilding = 420
    }

    /// <summary>Sent from client to server.</summary>
    public enum ClientPackets
    {
        welcomeReceived = 1,
        ping = 2,
        requestHexGrid = 3,
        requestPlaceBuilding = 4,
        requestUpgradeBuilding = 5,
        positionUpdate = 6,
        requestBuildHQ = 7,
        requestJoinTribe = 8,
        testBuilding = 420
    }

    public class Packet : IDisposable
    {
        private List<byte> buffer;
        private byte[] readableBuffer;
        private int readPos;

        /// <summary>Creates a new empty packet (without an ID).</summary>
        public Packet()
        {
            buffer = new List<byte>(); // Intitialize buffer
            readPos = 0; // Set readPos to 0
        }

        /// <summary>Creates a new packet with a given ID. Used for sending.</summary>
        /// <param name="_id">The packet ID.</param>
        public Packet(int _id)
        {
            buffer = new List<byte>(); // Intitialize buffer
            readPos = 0; // Set readPos to 0

            Write(_id); // Write packet id to the buffer
        }

        /// <summary>Creates a packet from which data can be read. Used for receiving.</summary>
        /// <param name="_data">The bytes to add to the packet.</param>
        public Packet(byte[] _data)
        {
            buffer = new List<byte>(); // Intitialize buffer
            readPos = 0; // Set readPos to 0

            SetBytes(_data);
        }

        #region Functions
        /// <summary>Sets the packet's content and prepares it to be read.</summary>
        /// <param name="_data">The bytes to add to the packet.</param>
        public void SetBytes(byte[] _data)
        {
            Write(_data);
            readableBuffer = buffer.ToArray();
        }

        /// <summary>Inserts the length of the packet's content at the start of the buffer.</summary>
        public void WriteLength()
        {
            buffer.InsertRange(0, BitConverter.GetBytes(buffer.Count)); // Insert the byte length of the packet at the very beginning
        }

        /// <summary>Inserts the given int at the start of the buffer.</summary>
        /// <param name="_value">The int to insert.</param>
        public void InsertInt(int _value)
        {
            buffer.InsertRange(0, BitConverter.GetBytes(_value)); // Insert the int at the start of the buffer
        }

        /// <summary>Gets the packet's content in array form.</summary>
        public byte[] ToArray()
        {
            readableBuffer = buffer.ToArray();
            return readableBuffer;
        }

        /// <summary>Gets the length of the packet's content.</summary>
        public int Length()
        {
            return buffer.Count; // Return the length of buffer
        }

        /// <summary>Gets the length of the unread data contained in the packet.</summary>
        public int UnreadLength()
        {
            return Length() - readPos; // Return the remaining length (unread)
        }

        /// <summary>Resets the packet instance to allow it to be reused.</summary>
        /// <param name="_shouldReset">Whether or not to reset the packet.</param>
        public void Reset(bool _shouldReset = true)
        {
            if (_shouldReset)
            {
                buffer.Clear(); // Clear buffer
                readableBuffer = null;
                readPos = 0; // Reset readPos
            }
            else
            {
                readPos -= 4; // "Unread" the last read int
            }
        }
        #endregion

        #region Write Data

        #region Write Primitives
        /// <summary>Adds a byte to the packet.</summary>
        /// <param name="_value">The byte to add.</param>
        public void Write(byte _value)
        {
            buffer.Add(_value);
        }
        /// <summary>Adds an array of bytes to the packet.</summary>
        /// <param name="_value">The byte array to add.</param>
        public void Write(byte[] _value)
        {
            buffer.AddRange(_value);
        }
        /// <summary>Adds a short to the packet.</summary>
        /// <param name="_value">The short to add.</param>
        public void Write(short _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }
        /// <summary>Adds a ushort to the packet.</summary>
        /// <param name="_value">The ushort to add.</param>
        public void Write(ushort _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }
        /// <summary>Adds an int to the packet.</summary>
        /// <param name="_value">The int to add.</param>
        public void Write(int _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }
        /// <summary>Adds an uint to the packet.</summary>
        /// <param name="_value">The uint to add.</param>
        public void Write(uint _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }
        /// <summary>Adds an uint array to the packet.</summary>
        /// <param name="_value">The uint array to add.</param>
        public void Write(uint[] _value)
        {
            Write(_value.Length);
            for (int i = 0; i < _value.Length; i++)
            {
                buffer.AddRange(BitConverter.GetBytes(_value[i]));
            }
        }
        /// <summary>Adds a long to the packet.</summary>
        /// <param name="_value">The long to add.</param>
        public void Write(long _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }
        /// <summary>Adds a float to the packet.</summary>
        /// <param name="_value">The float to add.</param>
        public void Write(float _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }
        /// <summary>Adds a bool to the packet.</summary>
        /// <param name="_value">The bool to add.</param>
        public void Write(bool _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }
        /// <summary>Adds a string to the packet.</summary>
        /// <param name="_value">The string to add.</param>
        public void Write(string _value)
        {
            Write(_value.Length); // Add the length of the string to the packet
            buffer.AddRange(Encoding.ASCII.GetBytes(_value)); // Add the string itself
        }
        #endregion

        #region Write HexStuff

        /// <summary>Adds a HexCoordinates to the packet.</summary>
        /// <param name="_value">The HexCoordinate to add.</param>
        public void Write(HexCoordinates _value)
        {
            Write(_value.X);
            Write(_value.Z);
        }       
        /// <summary>Adds a HexCellData to the packet.</summary>
        /// <param name="_value">The HexCellData to add.</param>
        public void Write(HexCellBiome _value)
        {
            Write((byte)_value);
        }
        /// <summary>Adds a HexCellData to the packet.</summary>
        /// <param name="_value">The HexCellData to add.</param>
        public void Write(HexCellData _value)
        {
            Write(_value.Elevation);
            Write(_value.Biome);
            Write(_value.WaterDepth);
        }
        /// <summary>Adds a HexCell to the packet.</summary>
        /// <param name="_value">The HexCell to add.</param>
        public void Write(HexCell _value)
        {
            Write(_value.coordinates);
            Write(_value.Data);           
        }
        /// <summary>Adds a HexCell[] to the packet.</summary>
        /// <param name="_value">The HexCell[] to add.</param>
        public void Write(HexCell[] _value)
        {
            Write(_value.Length);
            foreach (HexCell cell in _value)
            {
                Write(cell);
            }
        }
        /// <summary>Adds a HexGrid to the packet.</summary>
        /// <param name="_value">The HexGrid to add.</param>
        public void Write(HexGrid.HexGrid _value)
        {
            Write(_value.chunkCountX);
            Write(_value.chunkCountZ);

            Write(_value.cornerLon);
            Write(_value.cornerLat);
            Write(_value.deltaLon);
            Write(_value.deltaLat);

            Write(_value.cells);
            List<Building> buildings = new List<Building>();
            foreach (HexCell cell in _value.cells)
            {
                Write(cell.Structure);
                if (cell.Structure is ICartHandler)
                    buildings.Add((Building)cell.Structure);
            }

            //Write Carts
            Write(buildings.Count);
            foreach (Building building in buildings)
            {
                Write(building.Cell.coordinates);
                Write(((ICartHandler)building).Carts);
            }
        }
        /// <summary>Adds a List<Cart> to the packet.</summary>
        /// <param name="_value">The List<Cart> to add.</param>
        public void Write(List<Cart> _value)
        {
            Write(_value.Count);
            foreach (Cart cart in _value)
            {
                Write(cart);
            }
        }
        /// <summary>Adds a Cart to the packet.</summary>
        /// <param name="_value">The Cart to add.</param>
        public void Write(Cart _value)
        {
            Write(_value.Inventory);
            Write(_value.Origin.Cell.coordinates);
            Write(_value.Destination.Cell.coordinates);
        }


        #endregion

        #region Write Structures

        /// <summary>Adds a Structure to the packet.</summary>
        /// <param name="_value">The Structure to add.</param>
        public void Write(Structure _value)
        {
            Write(_value.ToByte());
            if(_value == null)
            {
                return;
            }
            if(_value is Ressource)
            {
                Write((Ressource)_value);
            }
            if(_value is Building)
            {
                Write((Building)_value);
            }
        }
        /// <summary>Adds a Ressource to the packet.</summary>
        /// <param name="_value">The Ressource to add.</param>
        private void Write(Ressource _value)
        {
            Write(_value.Progress);
        }
        /// <summary>Adds a Building to the packet.</summary>
        /// <param name="_value">The Building to add.</param>
        private void Write(Building _value)
        {
            Write(_value.Tribe);
            Write(_value.Level);
            Write(_value.Health);

            if (_value is ProtectedBuilding)
            {
                Write((ProtectedBuilding)_value);
            }
        }
        /// <summary>Adds a ProtectedBuilding to the packet.</summary>
        /// <param name="_value">The ProtectedBuilding to add.</param>
        private void Write(ProtectedBuilding _value)
        {
            Write(_value.TroopInventory);

            if(_value is InventoryBuilding)
            {
                Write((InventoryBuilding)_value);
            }
        }
        /// <summary>Adds an InventoryBuilding to the packet.</summary>
        /// <param name="_value">The InventoryBuilding to add.</param>
        private void Write(InventoryBuilding _value)
        {
            Write(_value.Inventory);
            if (_value is ProgressBuilding)
            {
                Write((ProgressBuilding)_value);
                                }
            else if (_value is StorageBuilding)
            {
                Write((StorageBuilding)_value);
            }
        }
        /// <summary>Adds a StorageBuilding to the packet.</summary>
        /// <param name="_value">The StorageBuilding to add.</param>
        private void Write(StorageBuilding _value)
        {

        }
        /// <summary>Adds an ProgressBuilding to the packet.</summary>
        /// <param name="_value">The ProgressBuilding to add.</param>
        private void Write(ProgressBuilding _value)
        {
            Write(_value.Progress);
            if (_value is ProductionBuilding)
            {
                Write((ProductionBuilding) _value);
            }
            else if (_value is RefineryBuilding)
            {
                Write((RefineryBuilding) _value);
            }
        }
        /// <summary>Adds an ProductionBuilding to the packet.</summary>
        /// <param name="_value">The ProductionBuilding  to add.</param>
        private void Write(ProductionBuilding _value)
        {

        }
        /// <summary>Adds an RefineryBuilding to the packet.</summary>
        /// <param name="_value">The RefineryBuilding to add.</param>
        private void Write(RefineryBuilding _value)
        {

        }
        /// <summary>Adds an Inventory to the packet.</summary>
        /// <param name="_value">The Inventory to add.</param>
        private void Write(Inventory _value)
        {
            Write(_value.Storage);
            Write(_value.RessourceLimit);
        }
        /// <summary>Adds an Inventory to the packet.</summary>
        /// <param name="_value">The Inventory to add.</param>
        private void Write(BuildingInventory _value)
        {
            Write(_value.Storage);
            Write(_value.RessourceLimit);
            Write(_value.RessourceLimits);
            Write(_value.Outgoing);
            Write(_value.Incoming);
        }
        /// <summary>Adds a TrropInventory to the packet.</summary>
        /// <param name="_value">The TroopInventory to add.</param>
        private void Write(TroopInventory _value)
        {
            Write(_value.Troops);
            Write(_value.TroopLimit);
            Write(_value.Strategy);
        }
        /// <summary>Adds a Dictionary to the packet.</summary>
        /// <param name="_value">The Dictionary to add.</param>
        public void Write(Dictionary<RessourceType, int> _value)
        {
            Write(_value.Count);
            foreach(KeyValuePair<RessourceType, int> pair in _value)
            {
                Write((byte)pair.Key);
                Write(pair.Value);
            }
        }
        /// <summary>Adds a Dictionary to the packet.</summary>
        /// <param name="_value">The Dictionary to add.</param>
        public void Write(Dictionary<TroopType, int> _value)
        {
            Write(_value.Count);
            foreach (KeyValuePair<TroopType, int> pair in _value)
            {
                Write((byte)pair.Key);
                Write(pair.Value);
            }
        }
        /// <summary>Adds a List<RessourceType> to the packet.</summary>
        /// <param name="_value">The List<RessourceType> to add.</param>
        public void Write(List<RessourceType> _value)
        {
            Write(_value.Count);
            foreach (RessourceType ressourceType in _value)
            {
                Write((byte)ressourceType);
            }
        }
        /// <summary>Adds a List<RessourceType> to the packet.</summary>
        /// <param name="_value">The List<RessourceType> to add.</param>
        public void Write(List<Tuple<TroopType, bool>> _value)
        {
            Write(_value.Count);
            foreach (Tuple<TroopType, bool> tuple in _value)
            {
                Write((byte)tuple.Item1);
                Write(tuple.Item2);
            }
        }
        /// <summary>Adds a List<Structure> to the packet.</summary>
        /// <param name="_value">The List<Structure> to add.</param>
        public void Write(List<Structure> _value)
        {
            Write(_value.Count);
            foreach(Structure structure in _value)
            {
                Write(structure);
            }
        }
        /// <summary>Adds a Type to the packet.</summary>
        /// <param name="_value">The Type to add.</param>
        public void Write(Type _value)
        {
            Write(_value.ToByte());
        }

        #endregion

        #region Write GameLogic

        /// <summary>Adds a Tribe to the packet.</summary>
        /// <param name="_value">The Tribe to add.</param>
        public void Write(Tribe _value)
        {
            Write(_value.Id);
            Write(_value.HQ.Cell.coordinates);
        }

        #endregion

        #endregion


        #region Read Data

        #region Read Primitives
        /// <summary>Reads a byte from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public byte ReadByte(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                // If there are unread bytes
                byte _value = readableBuffer[readPos]; // Get the byte at readPos' position
                if (_moveReadPos)
                {
                    // If _moveReadPos is true
                    readPos += 1; // Increase readPos by 1
                }
                return _value; // Return the byte
            }
            else
            {
                throw new Exception("Could not read value of type 'byte'!");
            }
        }

        /// <summary>Reads an array of bytes from the packet.</summary>
        /// <param name="_length">The length of the byte array.</param>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public byte[] ReadBytes(int _length, bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                // If there are unread bytes
                byte[] _value = buffer.GetRange(readPos, _length).ToArray(); // Get the bytes at readPos' position with a range of _length
                if (_moveReadPos)
                {
                    // If _moveReadPos is true
                    readPos += _length; // Increase readPos by _length
                }
                return _value; // Return the bytes
            }
            else
            {
                throw new Exception("Could not read value of type 'byte[]'!");
            }
        }

        /// <summary>Reads a short from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public short ReadShort(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                // If there are unread bytes
                short _value = BitConverter.ToInt16(readableBuffer, readPos); // Convert the bytes to a short
                if (_moveReadPos)
                {
                    // If _moveReadPos is true and there are unread bytes
                    readPos += 2; // Increase readPos by 2
                }
                return _value; // Return the short
            }
            else
            {
                throw new Exception("Could not read value of type 'short'!");
            }
        }

        /// <summary>Reads a ushort from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public ushort ReadUShort(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                // If there are unread bytes
                ushort _value = BitConverter.ToUInt16(readableBuffer, readPos); // Convert the bytes to a ushort
                if (_moveReadPos)
                {
                    // If _moveReadPos is true and there are unread bytes
                    readPos += 2; // Increase readPos by 2
                }
                return _value; // Return the ushort
            }
            else
            {
                throw new Exception("Could not read value of type 'ushort'!");
            }
        }

        /// <summary>Reads an int from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public int ReadInt(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                // If there are unread bytes
                int _value = BitConverter.ToInt32(readableBuffer, readPos); // Convert the bytes to an int
                if (_moveReadPos)
                {
                    // If _moveReadPos is true
                    readPos += 4; // Increase readPos by 4
                }
                return _value; // Return the int
            }
            else
            {
                throw new Exception("Could not read value of type 'int'!");
            }
        }

        /// <summary>Reads an uint from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public uint ReadUInt(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                // If there are unread bytes
                uint _value = BitConverter.ToUInt32(readableBuffer, readPos); // Convert the bytes to an uint
                if (_moveReadPos)
                {
                    // If _moveReadPos is true
                    readPos += 4; // Increase readPos by 4
                }
                return _value; // Return the int
            }
            else
            {
                throw new Exception("Could not read value of type 'Uint'!");
            }
        }

        /// <summary>Reads an uint array from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public uint[] ReadUIntArray(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                // If there are unread bytes
                int length = ReadInt(_moveReadPos);
                uint[] array = new uint[length];
                for (int i = 0; i < length; i++)
                {
                    array[i] = ReadUInt(_moveReadPos);
                }
                return array; // Return the uint array
            }
            else
            {
                throw new Exception("Could not read value of type 'Uint Array'!");
            }
        }

        /// <summary>Reads a long from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public long ReadLong(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                // If there are unread bytes
                long _value = BitConverter.ToInt64(readableBuffer, readPos); // Convert the bytes to a long
                if (_moveReadPos)
                {
                    // If _moveReadPos is true
                    readPos += 8; // Increase readPos by 8
                }
                return _value; // Return the long
            }
            else
            {
                throw new Exception("Could not read value of type 'long'!");
            }
        }

        /// <summary>Reads a float from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public float ReadFloat(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                // If there are unread bytes
                float _value = BitConverter.ToSingle(readableBuffer, readPos); // Convert the bytes to a float
                if (_moveReadPos)
                {
                    // If _moveReadPos is true
                    readPos += 4; // Increase readPos by 4
                }
                return _value; // Return the float
            }
            else
            {
                throw new Exception("Could not read value of type 'float'!");
            }
        }

        /// <summary>Reads a bool from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public bool ReadBool(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                // If there are unread bytes
                bool _value = BitConverter.ToBoolean(readableBuffer, readPos); // Convert the bytes to a bool
                if (_moveReadPos)
                {
                    // If _moveReadPos is true
                    readPos += 1; // Increase readPos by 1
                }
                return _value; // Return the bool
            }
            else
            {
                throw new Exception("Could not read value of type 'bool'!");
            }
        }

        /// <summary>Reads a string from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public string ReadString(bool _moveReadPos = true)
        {
            try
            {
                int _length = ReadInt(_moveReadPos); // Get the length of the string
                string _value = Encoding.ASCII.GetString(readableBuffer, readPos, _length); // Convert the bytes to a string
                if (_moveReadPos && _value.Length > 0)
                {
                    // If _moveReadPos is true string is not empty
                    readPos += _length; // Increase readPos by the length of the string
                }
                return _value; // Return the string
            }
            catch
            {
                throw new Exception("Could not read value of type 'string'!");
            }
        }

        #endregion

        #region  Read HexStuff
        /// <summary>Reads a HexCoordinates from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public HexCoordinates ReadHexCoordinates(bool _moveReadPos = true)
        {
            try
            {
                int x = ReadInt(_moveReadPos);
                int z = ReadInt(_moveReadPos);
                HexCoordinates _value = new HexCoordinates(x, z);
                return _value; // Return the HexCoordinates
            }
            catch
            {
                throw new Exception("Could not read value of type 'HexCoordinates'!");
            }
        }      
        /// <summary>Reads a HexCellBiome from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public HexCellBiome ReadHexCellBiome(bool _moveReadPos = true)
        {
            try
            {
                HexCellBiome _value = (HexCellBiome)ReadByte(_moveReadPos);
                return _value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'HexCellBiome'!");
            }
        }
        /// <summary>Reads a HexCellData from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public HexCellData ReadHexCellData(bool _moveReadPos = true)
        {
            try
            {
                ushort Elevation = ReadUShort(_moveReadPos);
                HexCellBiome Biome = ReadHexCellBiome(_moveReadPos);
                byte WaterDepth = ReadByte(_moveReadPos);
                HexCellData _value = new HexCellData(Elevation, Biome, WaterDepth);
                return _value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'HexCellData'!");
            }
        }
        /// <summary>Reads a HexCell from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public HexCell ReadHexCell(bool _moveReadPos = true)
        {
            try
            {
                HexCoordinates coordinates = ReadHexCoordinates(_moveReadPos);
                HexCellData Data = ReadHexCellData(_moveReadPos);               

                HexCell _value = new HexCell();
                _value.coordinates = coordinates;
                _value.Data = Data;                
                return _value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'HexCell'!");
            }
        }
        /// <summary>Reads a HexCell[] from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public HexCell[] ReadHexCells(bool _moveReadPos = true)
        {
            try
            {
                int length = ReadInt(_moveReadPos);
                HexCell[] _value = new HexCell[length];
                for (int i = 0; i < length; i++)
                {
                    _value[i] = ReadHexCell(_moveReadPos);
                }
                return _value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'HexCell[]'!");
            }
        }
        /// <summary>Reads a HexGrid from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public HexGrid.HexGrid ReadHexGrid(bool _moveReadPos = true)
        {
            try
            {
                int chunkCountX = ReadInt(_moveReadPos);
                int chunkCountZ = ReadInt(_moveReadPos);

                float cornerLon = ReadFloat(_moveReadPos);
                float cornerLat = ReadFloat(_moveReadPos);
                float deltaLon = ReadFloat(_moveReadPos);
                float deltaLat = ReadFloat(_moveReadPos);

                HexGrid.HexGrid _value = new HexGrid.HexGrid(chunkCountX, chunkCountZ, cornerLon, cornerLat, deltaLon, deltaLat);

                HexCell[] cells = ReadHexCells(_moveReadPos);

                foreach (HexCell cell in cells)
                {
                    Structure structure = ReadStructure();
                    cell.Structure = structure;
                    if (structure != null)
                        structure.Cell = cell;
                }

                _value.cells = cells;

                for (int i = 0; i < _value.cells.Length; i++)
                {
                    int x = i % (_value.cellCountX);
                    int z = i / (_value.cellCountX);
                    _value.UpdateNeighbors(x, z, i);
                    _value.AddCellToChunk(x, z, _value.cells[i]);
                }

                //Read Carts
                int size = ReadInt(_moveReadPos);
                for (int i = 0; i < size; i++)
                {
                    HexCoordinates coords = ReadHexCoordinates(_moveReadPos);
                    List<Cart> carts = ReadListOfCart(_value, _moveReadPos);
                    ((ICartHandler)_value.GetCell(coords).Structure).Carts = carts;
                }

                return _value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'HexGrid'!");
            }
        }
        /// <summary>Reads a Cart from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public Cart ReadCart(HexGrid.HexGrid grid, bool _moveReadPos = true)
        {
            try
            {
                Inventory Inventory = ReadInventory(_moveReadPos);
                HexCoordinates origin = ReadHexCoordinates(_moveReadPos);
                HexCoordinates destination = ReadHexCoordinates(_moveReadPos);
                
                return new Cart( 
                    Inventory, 
                    (InventoryBuilding)grid.GetCell(origin).Structure, 
                    (InventoryBuilding)grid.GetCell(destination).Structure
                );
            }
            catch
            {
                throw new Exception("Could not read value of type 'Cart'!");
            }
        }
        /// <summary>Reads a List<Cart> from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public List<Cart> ReadListOfCart(HexGrid.HexGrid grid, bool _moveReadPos = true)
        {
            try
            {
                int size = ReadInt(_moveReadPos);
                List<Cart> _value = new List<Cart>();
                for (int i = 0; i < size; i++)
                {
                    _value.Add(ReadCart(grid, _moveReadPos));
                }
                return _value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'List<Cart>'!");
            }
        }

        #endregion

        #region Read Structures
        /// <summary>Reads a Structure from the packet directly onto the HexGrid.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public Structure ReadStructure(HexGrid.HexGrid grid, bool _moveReadPos = true)
        {
            try
            {
                Structure _value = ReadStructure(_moveReadPos);
                if(_value != null)
                {
                    HexCoordinates coordinates = ReadHexCoordinates(_moveReadPos);

                    HexCell cell = grid.GetCell(coordinates);
                    _value.Cell = cell;
                    cell.Structure = _value;
                    if(_value is ICartHandler)
                        ((ICartHandler)_value).Carts = ReadListOfCart(grid, _moveReadPos);
                }
                return _value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'Structure' onto HexGrid!");
            }
        }
        /// <summary>Reads a Structure from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public Structure ReadStructure(bool _moveReadPos = true)
        {
            try
            {
                Type type = ReadByte(false).ToType();
                if (type == null)
                {
                    ReadByte(_moveReadPos);
                    return null;
                }
                if (typeof(Building).IsAssignableFrom(type))
                    return ReadBuilding(_moveReadPos);
                if (typeof(Ressource).IsAssignableFrom(type))
                    return ReadRessource(_moveReadPos);
                
                ReadByte(_moveReadPos);

                Structure _value = (Structure)Activator.CreateInstance(type);
                
                return _value; 
            }
            catch
            {
                throw new Exception("Could not read value of type 'Structure'!");
            }
        }
        /// <summary>Reads a Ressource from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public Ressource ReadRessource(bool _moveReadPos = true)
        {
            try
            {
                Type type = ReadByte(_moveReadPos).ToType();

                               
                byte progress = ReadByte(_moveReadPos);

                Ressource _value = (Ressource)Activator.CreateInstance(type, new object[]{
                    null,
                    progress
                });
                return _value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'Ressource'!");
            }
        }
        /// <summary>Reads a Building from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public Building ReadBuilding(bool _moveReadPos = true)
        {
            try
            {
                Type type = ReadByte(false).ToType();
                if (typeof(ProtectedBuilding).IsAssignableFrom(type))
                    return ReadProtectedBuilding(_moveReadPos);
                else
                {
                    ReadByte(_moveReadPos);

                    byte Tribe = ReadByte(_moveReadPos);
                    byte Level = ReadByte(_moveReadPos);
                    byte Health = ReadByte(_moveReadPos);

                    Building _value = (Building)Activator.CreateInstance(type, new object[]{
                        null,
                        Tribe,
                        Level,
                        Health
                    });
                    return _value;
                }
            }
            catch
            {
                throw new Exception("Could not read value of type 'Building'!");
            }
        }
        /// <summary>Reads a ProtectedBuilding from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public ProtectedBuilding ReadProtectedBuilding(bool _moveReadPos = true)
        {
            try
            {
                Type type = ReadByte(false).ToType();
                if (typeof(InventoryBuilding).IsAssignableFrom(type))
                    return ReadInventoryBuilding(_moveReadPos);

                ReadByte(_moveReadPos);

                byte Tribe = ReadByte(_moveReadPos);
                byte Level = ReadByte(_moveReadPos);
                byte Health =  ReadByte(_moveReadPos);
                TroopInventory TroopInventory = ReadTroopInventory(_moveReadPos);
                ProtectedBuilding _value = (ProtectedBuilding)Activator.CreateInstance(type, new object[]{
                    null,
                    Tribe,
                    Level,
                    Health,
                    TroopInventory
                });
                return _value;                
            }
            catch
            {
                throw new Exception("Could not read value of type 'ProtectedBuilding'!");
            }
        }
        /// <summary>Reads an InventoryBuilding from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public InventoryBuilding ReadInventoryBuilding(bool _moveReadPos = true)
        {
            try
            {
                Type type = ReadByte(false).ToType();
                if (typeof(ProgressBuilding).IsAssignableFrom(type))
                    return ReadProgressBuilding(_moveReadPos);
                if (typeof(StorageBuilding).IsAssignableFrom(type))
                    return ReadStorageBuilding(_moveReadPos);

                ReadByte(_moveReadPos);

                byte Tribe = ReadByte(_moveReadPos);
                byte Level = ReadByte(_moveReadPos);
                byte Health = ReadByte(_moveReadPos);
                TroopInventory TroopInventory = ReadTroopInventory(_moveReadPos);
                BuildingInventory Inventory = ReadBuildingInventory(_moveReadPos);
                InventoryBuilding _value = (InventoryBuilding)Activator.CreateInstance(type, new object[]{
                    null,
                    Tribe,
                    Level,
                    Health,
                    TroopInventory,
                    Inventory
                });

                return _value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'InventoryBuilding'!");
            }
        }
        /// <summary>Reads an StorageBuilding from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public InventoryBuilding ReadStorageBuilding(bool _moveReadPos = true)
        {
            try
            {
                Type type = ReadByte(_moveReadPos).ToType();                

                byte Tribe = ReadByte(_moveReadPos);
                byte Level = ReadByte(_moveReadPos);
                byte Health = ReadByte(_moveReadPos);
                TroopInventory TroopInventory = ReadTroopInventory(_moveReadPos);
                BuildingInventory Inventory = ReadBuildingInventory(_moveReadPos);

                InventoryBuilding _value = (InventoryBuilding)Activator.CreateInstance(type, new object[]{
                    null,
                    Tribe,
                    Level,
                    Health,
                    TroopInventory,
                    Inventory
                });

                return _value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'StorageBuilding'!");
            }
        }
        /// <summary>Reads an ProgressBuilding from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public ProgressBuilding ReadProgressBuilding(bool _moveReadPos = true)
        {
            try
            {
                Type type = ReadByte(false).ToType();
                if (typeof(ProductionBuilding).IsAssignableFrom(type))
                    return ReadProductionBuilding(_moveReadPos);
                if (typeof(RefineryBuilding).IsAssignableFrom(type))
                    return ReadRefineryBuilding(_moveReadPos);

                ReadByte(_moveReadPos);

                byte Tribe = ReadByte(_moveReadPos);
                byte Level = ReadByte(_moveReadPos);
                byte Health = ReadByte(_moveReadPos);
                TroopInventory TroopInventory = ReadTroopInventory(_moveReadPos);
                BuildingInventory Inventory = ReadBuildingInventory(_moveReadPos);
                int Progress = ReadInt(_moveReadPos);

                ProgressBuilding _value = (ProgressBuilding)Activator.CreateInstance(type, new object[]{
                    null,
                    Tribe,
                    Level,
                    Health,
                    TroopInventory,
                    Inventory,
                    Progress 
                });
                return _value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'ProgressBuilding'!");
            }
        }
        /// <summary>Reads an ProductionBuilding from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public ProductionBuilding ReadProductionBuilding(bool _moveReadPos = true)
        {
            try
            {
                Type type = ReadByte(_moveReadPos).ToType();

                byte Tribe = ReadByte(_moveReadPos);
                byte Level = ReadByte(_moveReadPos);
                byte Health = ReadByte(_moveReadPos);
                TroopInventory TroopInventory = ReadTroopInventory(_moveReadPos);
                BuildingInventory Inventory = ReadBuildingInventory(_moveReadPos);
                int Progress = ReadInt(_moveReadPos);

                ProductionBuilding _value = (ProductionBuilding)Activator.CreateInstance(type, new object[]{
                    null,
                    Tribe,
                    Level,
                    Health,
                    TroopInventory,
                    Inventory,
                    Progress
                });
                return _value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'ProductionBuilding'!");
            }
        }
        /// <summary>Reads an RefineryBuilding from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public RefineryBuilding ReadRefineryBuilding(bool _moveReadPos = true)
        {
            try
            {
                Type type = ReadByte(_moveReadPos).ToType();

                byte Tribe = ReadByte(_moveReadPos);
                byte Level = ReadByte(_moveReadPos);
                byte Health = ReadByte(_moveReadPos);
                TroopInventory TroopInventory = ReadTroopInventory(_moveReadPos);
                BuildingInventory Inventory = ReadBuildingInventory(_moveReadPos);
                int Progress = ReadInt(_moveReadPos);

                RefineryBuilding _value = (RefineryBuilding)Activator.CreateInstance(type, new object[]{
                    null,
                    Tribe,
                    Level,
                    Health,
                    TroopInventory,
                    Inventory,
                    Progress
                });
                return _value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'RefineryBuilding'!");
            }
        }
        /// <summary>Reads an Inventory from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public Inventory ReadInventory(bool _moveReadPos = true)
        {
            try 
            {
                Inventory _value = new Inventory();
                _value.Storage = ReadDictionaryRessourceTypeInt(_moveReadPos);
                _value.RessourceLimit = ReadInt(_moveReadPos);

                return _value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'Inventory'!");
            }
        }
        /// <summary>Reads a BuildingInventory from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public BuildingInventory ReadBuildingInventory(bool _moveReadPos = true)
        {
            try 
            {
                BuildingInventory _value = new BuildingInventory();
                _value.Storage = ReadDictionaryRessourceTypeInt(_moveReadPos);
                _value.RessourceLimit = ReadInt(_moveReadPos);
                _value.RessourceLimits = ReadDictionaryRessourceTypeInt(_moveReadPos);
                _value.UpdateOutgoing(ReadRessourceTypes(_moveReadPos));
                _value.UpdateIncoming(ReadRessourceTypes(_moveReadPos));
                
                return _value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'BuildingInventory'!");
            }
        }
        /// <summary>Reads an TroopInventory from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public TroopInventory ReadTroopInventory(bool _moveReadPos = true)
        {
            try
            {
                Dictionary<TroopType, int> troops = ReadDictionaryTroopTypeInt(_moveReadPos);
                int troopLimit = ReadInt(_moveReadPos);
                List<Tuple<TroopType, bool>> strategy = ReadListTroopTypeBool(_moveReadPos);

                TroopInventory _value = new TroopInventory(troops, troopLimit, strategy);

                return _value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'Inventory'!");
            }
        }
        /// <summary>Reads a Dictionary<RessourceType, int> from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public Dictionary<RessourceType, int> ReadDictionaryRessourceTypeInt(bool _moveReadPos = true)
        {
            try
            {
                Dictionary<RessourceType, int> _value = new Dictionary<RessourceType, int>();
                int count = ReadInt(_moveReadPos);
                while(count > 0)
                {
                    RessourceType ressourceType = (RessourceType)ReadByte(_moveReadPos);
                    int amount = ReadInt(_moveReadPos);
                    _value.Add(ressourceType, amount);
                    count--;
                }
                return _value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'Dictionary<RessourceType, int>'!");
            }
        }
        /// <summary>Reads a Dictionary<TroopType, int> from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public Dictionary<TroopType, int> ReadDictionaryTroopTypeInt(bool _moveReadPos = true)
        {
            try
            {
                Dictionary<TroopType, int> _value = new Dictionary<TroopType, int>();
                int count = ReadInt(_moveReadPos);
                while (count > 0)
                {
                    TroopType troopType = (TroopType)ReadByte(_moveReadPos);
                    int amount = ReadInt(_moveReadPos);
                    _value.Add(troopType, amount);
                    count--;
                }
                return _value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'Dictionary<TroopType, int>'!");
            }
        }
        /// <summary>Reads a List<RessourceType> from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public List<RessourceType> ReadRessourceTypes(bool _moveReadPos = true)
        {
            try
            {
                List<RessourceType> _value = new List<RessourceType>();
                int count = ReadInt(_moveReadPos);
                while (count > 0)
                {
                    RessourceType ressourceType = (RessourceType)ReadByte(_moveReadPos);
                    _value.Add(ressourceType);
                    count--;
                }
                return _value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'List<RessourceType'!");
            }
        }
        /// <summary>Reads a List<Tuple<TroopType, bool>> from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public List<Tuple<TroopType, bool>> ReadListTroopTypeBool(bool _moveReadPos = true)
        {
            try
            {
                List<Tuple<TroopType, bool>> _value = new List<Tuple<TroopType, bool>>();
                int count = ReadInt(_moveReadPos);
                while (count > 0)
                {
                    TroopType troopType = (TroopType)ReadByte(_moveReadPos);
                    bool cycling = ReadBool(_moveReadPos);
                    _value.Add(new Tuple<TroopType, bool>(troopType, cycling));
                    count--;
                }
                return _value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'List<Tuple<TroopType, bool>>'!");
            }
        }
        /// <summary>Reads a List<Structure> from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public List<Structure> ReadStructures(bool _moveReadPos = true)
        {
            try
            {
                List<Structure> _value = new List<Structure>();
                int count = ReadInt(_moveReadPos);
                while (count > 0)
                {
                    Structure structure = ReadStructure(_moveReadPos);
                    _value.Add(structure);
                    count--;
                }
                return _value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'List<Structure>'!");
            }
        }
        public Type ReadType(bool _moveReadPos = true)
        {
            try
            {
                return ReadByte(_moveReadPos).ToType();
            }
            catch
            {
                throw new Exception("Could not read value of type 'List<Structure>'!");
            }
        }

        #endregion

        #endregion

        private bool disposed = false;

        protected virtual void Dispose(bool _disposing)
        {
            if (!disposed)
            {
                if (_disposing)
                {
                    buffer = null;
                    readableBuffer = null;
                    readPos = 0;
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    internal static class StructureTypeExtension
    {
        private static List<Tuple<Type, byte>> mapping = new List<Tuple<Type, byte>>
        {
            new Tuple<Type, byte>(typeof(Woodcutter), 0),
            new Tuple<Type, byte>(typeof(Tree), 1),
            new Tuple<Type, byte>(typeof(Rock), 2),
            new Tuple<Type, byte>(typeof(Fish), 3),
            new Tuple<Type, byte>(typeof(Scrub), 4),
            new Tuple<Type, byte>(typeof(Grass), 5),
            new Tuple<Type, byte>(typeof(Quarry), 6),
            new Tuple<Type, byte>(typeof(LandRoad), 7),
            new Tuple<Type, byte>(typeof(IronOre), 8),
            new Tuple<Type, byte>(typeof(CoalOre), 9),
            new Tuple<Type, byte>(typeof(Wheat), 10),
            new Tuple<Type, byte>(typeof(Storage), 11 ),
            new Tuple<Type, byte>(typeof(Headquarter), 12 ),
            new Tuple<Type, byte>(typeof(CoalMine), 13 ),
            new Tuple<Type, byte>(typeof(Smelter), 14),
            new Tuple<Type, byte>(typeof(IronMine), 15),
            new Tuple<Type, byte>(typeof(Fisher), 16)
        };

        internal static byte ToByte(this Structure structure)
        {
            if (structure == null)
                return 255;
            return mapping.Find(elem => elem.Item1 == structure.GetType()).Item2;
        }

        internal static byte ToByte(this Type type)
        {
            if (type == null)
                return 255;
            return mapping.Find(elem => elem.Item1 == type).Item2;
        }

        internal static Type ToType(this byte b)
        { 
            if(b == 255)
            {
                return null;
            }
            return mapping.Find(elem => elem.Item2 == b).Item1;
        }
    }
}
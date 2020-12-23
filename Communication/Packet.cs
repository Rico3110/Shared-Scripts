using System;
using System.Collections.Generic;
using System.Text;
using Shared.HexGrid;
using Shared.DataTypes;

namespace Shared.Communication
{
    /// <summary>Sent from server to client.</summary>
    public enum ServerPackets
    {
        welcome = 1,
        ping = 2,
        testArray = 3,
        hexMap = 4,
        hexData = 10,
        sendBuildingData = 11,
        hexCell = 13,
<<<<<<< HEAD
        sendHexGrid = 14
=======
        sendHexGrid = 14,
>>>>>>> 5e2d353215e53a4c6aa292b4af1fa5b038ee5680

    }

    /// <summary>Sent from client to server.</summary>
    public enum ClientPackets
    {
        welcomeReceived = 1,
        ping = 2,
        testArray = 3,
        hexMap = 4,
        hexData = 10,
        requestBuildingData = 11,
        requestBuildBuilding = 12,
<<<<<<< HEAD
        requestAllMapData = 14
=======
        requestAllMapData = 14,
>>>>>>> 5e2d353215e53a4c6aa292b4af1fa5b038ee5680
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
        /// <summary>Adds a HexCoordinates to the packet.</summary>
        /// <param name="_value">The HexCoordinate to add.</param>
        public void Write(HexCoordinates _value)
        {
            Write(_value.X);
            Write(_value.Z);
        }
        /// <summary>Adds a BuildingType to the packet.</summary>
        /// <param name="_value">The BuildingType to add.</param>
        public void Write(BuildingType _value)
        {
            Write((byte)_value);
        }
        /// <summary>Adds a BuildingData to the packet.</summary>
        /// <param name="_value">The BuildingData to add.</param>
        public void Write(BuildingData _value)
        {
            Write(_value.Type);
            Write(_value.TeamID);
            Write(_value.Level);
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
            Write(_value.Building);
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
            Write(_value.cells);
        }
        #endregion

        #region Read Data
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
        /// <summary>Reads a BuildingType from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public BuildingType ReadBuildingType(bool _moveReadPos = true)
        {
            try
            {
                BuildingType _value = (BuildingType)ReadByte(_moveReadPos);
                return _value; // Return the BuildingType
            }
            catch
            {
                throw new Exception("Could not read value of type 'BuildingType'!");
            }
        }
        /// <summary>Reads a BuildingData from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public BuildingData ReadBuildingData(bool _moveReadPos = true)
        {
            try
            {
                BuildingType type = ReadBuildingType(_moveReadPos);
                byte teamID = ReadByte(_moveReadPos);
                byte level = ReadByte(_moveReadPos);

                BuildingData _value = new BuildingData();
                _value.Type = type;
                _value.TeamID = teamID;
                _value.Level = level;
                return _value; // Return the BuildingData
            }
            catch
            {
                throw new Exception("Could not read value of type 'BuildingData'!");
            }
        }
        /// <summary>Reads a HexCellBiome from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public HexCellBiome ReadHexCellBiome(bool _moveReadPos = true)
        {
            try
            {
                HexCellBiome _value = (HexCellBiome)ReadByte();
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
                BuildingData Building = ReadBuildingData(_moveReadPos);

                HexCell _value = new HexCell();
                _value.coordinates = coordinates;
                _value.Data = Data;
                _value.Building = Building;
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
                HexCell[] cells = ReadHexCells(_moveReadPos);
                
                HexGrid.HexGrid _value = new HexGrid.HexGrid(chunkCountX, chunkCountZ);
                _value.cells = cells;
                return _value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'HexGrid'!");
            }
        }
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
}
using System;

namespace IGHW_Editor
{
	public class TextFile : IGHW_File
	{
		//The number of items (only used for text)
		public uint numberOfItems;

		//The addresses of the contained items (only used for text)
		public uint[] itemAddresses;

		public TextFile(string filePath) : base(filePath, 0x49474857u)
		{
			if(ReadUInt32(0x04) != 0x00000002 && ReadUInt32(0x04) != 0x02000000)
			{
				validFile = false;
			}
			if (!validFile)															//If the magic number is not valid throw an error message
			{
				throw new InvalidOperationException("This is not an IGHW text file. Try loading an IGHW text file, or Ratchet will step on you.");
			}

			numberOfItems = ReadUInt32(0x64);														//Read the number of files

			itemAddresses = new uint[numberOfItems];												//Initialise an array to fit all the addresses

			for (uint i = 0; i < numberOfItems; i++)												//For every item
			{
				itemAddresses[i] = ReadUInt32(0x88 + 0x0C * i);										//Read and set the address to this item
			}
		}
	}
}
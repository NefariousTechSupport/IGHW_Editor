using System;
using System.IO;

namespace IGHW_Editor
{
	public class TextFile : IGHW_File
	{
		//The number of items (only used for text)
		public uint numberOfItems;
		public uint numberOfChecksums;

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
			numberOfChecksums = ReadUInt32(0x60);														//Read the number of files
			/*if(numberOfChecksums == 0)
			{
				numberOfChecksums = 1;
			}*/

			itemAddresses = new uint[numberOfItems];												//Initialise an array to fit all the addresses

			fs.Seek(0x74 + numberOfChecksums * 0x04, SeekOrigin.Begin);
			uint readPadding = 0;
			while((readPadding & 0xFFFFFF00) == 0x50414400)
			{
				Console.WriteLine(fs.Position.ToString("X08"));
				readPadding = ReadUInt32();
			}
			uint addressStart = (uint)fs.Position;
			Console.WriteLine(addressStart.ToString("X08"));


			for (uint i = 0; i < numberOfItems; i++)												//For every item
			{
				itemAddresses[i] = ReadUInt32(addressStart + 0x0C * i);			//Read and set the address to this item
			}
		}
		public void Save(string output, string[] items)
		{
			FileStream ofs = new FileStream(output, FileMode.Create, FileAccess.ReadWrite);
			StreamHelper osh = new StreamHelper(ofs, StreamHelper.Endianness.Big);

			osh.WriteUInt32(0x49474857);
			osh.WriteUInt32(0x00000002);
			osh.WriteUInt32(0x00000004);
			osh.WriteUInt32(0x00000000);
			osh.WriteUInt32(0x00026000);
			osh.WriteUInt32(0x00000050);
			osh.WriteUInt32(0x0000001C);
			osh.WriteUInt32(0x00000000);
			osh.WriteUInt32(0xFFFFFFFF);
			osh.WriteUInt32(0x00000070);
		}
	}
}
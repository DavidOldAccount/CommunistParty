//
// Program.cs
//
// Author:
//       Xi Jinping
//
// Copyright (c) 2014 Communist Party of China
//
// The Communist Party of China Open Source License
// ================================================
//
// FIRST YOU NEED TO JOIN THE COMMUNIST PARTY OF CHINA,
// THEN YOU CAN DO WHAT THE FUCK YOU WANT TO.
//
//
using System;
using System.IO;
using System.Text;
using System.Numerics;

namespace CommunistParty
{
	public class Program
	{
		public static void Main (string[] args)
		{
			Scanner scanner = new Scanner ();
			VM vm = new VM ();

			while (true) {
				Console.Write ("> ");
				string s = Console.ReadLine ();
				if (s == null)
					return;
				vm.Run (scanner.Scan (s));
			}
		}
	}
}
//
// Utils.cs
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

namespace CommunistParty
{
	public class Utils
	{
		public static void PrintError (string format, params object[] args)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write ("[ERROR] ");
			Console.WriteLine (format, args);
			Console.ResetColor ();
		}
	}
}


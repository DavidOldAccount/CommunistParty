//
// CommunistStack.cs
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
using System.Collections.Generic;
using System.Linq;

namespace CommunistParty
{
	public class CommunistStack<T> : List<T>
	{
		public void Push (T value)
		{
			this.Add (value);
		}

		public T Pop ()
		{
			if (this.Count == 0)
				throw new ArgumentOutOfRangeException ();
			T ret = this.Last ();
			this.RemoveAt (this.Count - 1);
			return ret;
		}

		public T Peek ()
		{
			if (this.Count == 0)
				throw new ArgumentNullException ();
			return this.Last ();
		}
	}
}


//
// OpCode.cs
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
	public enum OpCode
	{
		Push,
		Duplicate,
		CopyNth,
		Swap,
		Discard,
		Slide,
		Add,
		Sub,
		Mul,
		Div,
		Mod,
		Store,
		Retrieve,
		Mark,
		Call,
		Jump,
		JumpIfZero,
		JumpIfNeg,
		Return,
		Halt,
		PrintChar,
		PrintNum,
		ReadChar,
		ReadNum
	}
}


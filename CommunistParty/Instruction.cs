//
// Instruction.cs
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
	public class Instruction
	{
		int lineno, column;

		public OpCode opcode;

		public int operand = 0;

		public Instruction (OpCode opcode, int lineno, int column, int operand = 0)
		{
			this.opcode = opcode;
			this.lineno = lineno;
			this.column = column;
			this.operand = operand;
		}

		public override string ToString ()
		{
			return string.Format ("[Instruction: OpCode={0}, Operand={1}, Position=({2},{3})]",
				opcode, operand, lineno, column);
		}
	}
}


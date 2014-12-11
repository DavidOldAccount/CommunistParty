//
// VM.cs
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
	public class VM
	{
		int[] heap;
		CommunistStack<int> stack;
		Stack<int> frame;
		Dictionary<OpCode, Action<int>> instructions;
		int pc = 0;

		public VM ()
		{
			heap = new int[65536];
			frame = new Stack<int> ();
			stack = new CommunistStack<int> ();
			InitInstructions ();
		}

		static int intToInt (int n)
		{
			return int.Parse (n.ToString ());
		}

		void InitInstructions ()
		{
			instructions = new Dictionary<OpCode, Action<int>> ();

			instructions.Add (OpCode.Push, (operand) => stack.Push (operand));
			instructions.Add (OpCode.Duplicate, (operand) => stack.Push (stack.Peek ()));
			instructions.Add (OpCode.CopyNth, (operand) => stack.Push (stack [operand]));
			instructions.Add (OpCode.Swap, (operand) => {
				int tmp = stack.Peek ();
				stack [stack.Count - 1] = stack [stack.Count - 2];
				stack [stack.Count - 2] = tmp;
			});
			instructions.Add (OpCode.Discard, (operand) => stack.Pop ());
			instructions.Add (OpCode.Slide, (operand) => {
				for (int i = stack.Count - operand; i < stack.Count; i++)
					stack [i] = (int)0;
			});

			instructions.Add (OpCode.Add, (operand) => {
				stack.Push (stack.Pop () + stack.Pop ());
			});
			instructions.Add (OpCode.Sub, (operand) => {
				stack.Push (stack.Pop () - stack.Pop ());
			});
			instructions.Add (OpCode.Mul, (operand) => {
				stack.Push (stack.Pop () * stack.Pop ());
			});
			instructions.Add (OpCode.Div, (operand) => {
				stack.Push (stack.Pop () / stack.Pop ());
			});
			instructions.Add (OpCode.Mod, (operand) => {
				stack.Push (stack.Pop () % stack.Pop ());
			});

			instructions.Add (OpCode.Store, (operand) => {
				int value = stack.Pop ();
				int addr = stack.Pop ();
				heap [addr] = value;
			});
			instructions.Add (OpCode.Retrieve, (operand) => {
				int addr = stack.Pop ();
				stack.Push (heap [addr]);
			});

			instructions.Add (OpCode.Mark, (operand) => {
			});
			instructions.Add (OpCode.Call, (operand) => {
				frame.Push (pc);
				pc = operand;
			});
			instructions.Add (OpCode.Jump, (operand) => {
				pc = operand;
			});
			instructions.Add (OpCode.JumpIfZero, (operand) => {
				if (stack.Pop () == 0)
					pc = operand;
				else
					pc++;
			});
			instructions.Add (OpCode.JumpIfNeg, (operand) => {
				if (stack.Pop () < 0)
					pc = operand;
				else
					pc++;
			});
			instructions.Add (OpCode.Return, (operand) => {
				pc = frame.Pop () + 1;
			});
			instructions.Add (OpCode.Halt, (operand) => {
				Environment.Exit (0);
			});

			instructions.Add (OpCode.PrintChar, (operand) => {
				Console.Write (Convert.ToChar (stack.Pop ()));
			});
			instructions.Add (OpCode.PrintNum, (operand) => {
				Console.Write (stack.Pop ());
			});
			instructions.Add (OpCode.ReadChar, (operand) => {
				int addr = stack.Pop ();
				heap [addr] = (int)Console.Read ();
			});
			instructions.Add (OpCode.ReadNum, (operand) => {
				int addr = stack.Pop ();
				heap [addr] = int.Parse (Console.ReadLine ());
			});
		}

		bool isBranchOpCode (OpCode opcode)
		{
			if (opcode == OpCode.Call
			    || opcode == OpCode.Jump
			    || opcode == OpCode.JumpIfNeg
			    || opcode == OpCode.JumpIfZero
			    || opcode == OpCode.Return)
				return true;
			return false;
		}

		public void Run (List<Instruction> prog)
		{
			for (pc = 0; pc < prog.Count;) {
				Instruction instr = prog [pc];
				instructions [instr.opcode] (instr.operand);
				if (!isBranchOpCode (instr.opcode))
					pc++;
			}
		}
	}
}


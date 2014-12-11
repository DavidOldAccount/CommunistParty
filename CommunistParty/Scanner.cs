//
// Scanner.cs
//
// Author:
//       Xi Jinping
//
// Copyright (c) 2014 Communist Party of China
//
// The Communist Party of China Open Source License
// ================================================
//
// AT FRIST, YOU NEED TO JOIN THE COMMUNIST PARTY OF CHINA, 
// THEN YOU CAN DO WHAT THE FUCK YOU WANT TO.
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace CommunistParty
{
	public class Scanner
	{
		int lineno, column;
		int tokLineno, tokColumn;
		bool readingToken;

		static char tokenGong = '共', tokenChan = '产', tokenDang = '党';
		static string tokenHalt = "万岁";
		static Dictionary<string, Tuple<OpCode, bool>> opcodeMapping = new Dictionary<string, Tuple<OpCode, bool>> () {
			{ new string (new char[] { tokenGong, tokenGong }), new Tuple<OpCode, bool> (OpCode.Push, true) },
			{ new string (new char[] { tokenGong, tokenDang, tokenGong }), new Tuple<OpCode, bool> (OpCode.Duplicate, false) },
			{ new string (new char[] { tokenGong, tokenChan, tokenGong }) , new Tuple<OpCode, bool> (OpCode.CopyNth, true) },
			{ new string (new char[] { tokenGong, tokenDang, tokenChan }), new Tuple<OpCode, bool> (OpCode.Swap, false) },
			{ new string (new char[] { tokenGong, tokenDang, tokenDang }), new Tuple<OpCode, bool> (OpCode.Discard, false) },
			{ new string (new char[] { tokenGong, tokenChan, tokenDang }), new Tuple<OpCode, bool> (OpCode.Slide, true) },

			{ new string (new char[] { tokenChan, tokenGong, tokenGong, tokenGong }), new Tuple<OpCode, bool> (OpCode.Add, false) },
			{ new string (new char[] { tokenChan, tokenGong, tokenGong, tokenChan }), new Tuple<OpCode, bool> (OpCode.Sub, false) },
			{ new string (new char[] { tokenChan, tokenGong, tokenGong, tokenDang }), new Tuple<OpCode, bool> (OpCode.Mul, false) },
			{ new string (new char[] { tokenChan, tokenGong, tokenChan, tokenGong }), new Tuple<OpCode, bool> (OpCode.Div, false) },
			{ new string (new char[] { tokenChan, tokenGong, tokenChan, tokenChan }), new Tuple<OpCode, bool> (OpCode.Mod, false) },

			{ new string (new char[] { tokenChan, tokenChan, tokenGong }), new Tuple<OpCode, bool> (OpCode.Store, false) },
			{ new string (new char[] { tokenChan, tokenChan, tokenChan }), new Tuple<OpCode, bool> (OpCode.Retrieve, false) },

			{ new string (new char[] { tokenDang, tokenGong, tokenGong }), new Tuple<OpCode, bool> (OpCode.Mark, true) },
			{ new string (new char[] { tokenDang, tokenGong, tokenChan }), new Tuple<OpCode, bool> (OpCode.Call, true) },
			{ new string (new char[] { tokenDang, tokenGong, tokenDang }), new Tuple<OpCode, bool> (OpCode.Jump, true) },
			{ new string (new char[] { tokenDang, tokenChan, tokenGong }), new Tuple<OpCode,bool> (OpCode.JumpIfZero, true) },
			{ new string (new char[] { tokenDang, tokenChan, tokenChan }), new Tuple<OpCode, bool> (OpCode.JumpIfNeg, true) },
			{ new string (new char[] { tokenDang, tokenChan, tokenDang }), new Tuple<OpCode, bool> (OpCode.Return, false) },
			{ new string (new char[] { tokenDang, tokenDang, tokenDang, }), new Tuple<OpCode, bool> (OpCode.Halt, false) },
			{ tokenHalt, new Tuple<OpCode, bool> (OpCode.Halt, false) },

			{ new string (new char[] { tokenChan, tokenDang, tokenGong, tokenGong }), new Tuple<OpCode, bool> (OpCode.PrintChar, false) },
			{ new string (new char[] { tokenChan, tokenDang, tokenGong, tokenChan }), new Tuple<OpCode,bool> (OpCode.PrintNum, false) },
			{ new string (new char[] { tokenChan, tokenDang, tokenChan, tokenGong }), new Tuple<OpCode, bool> (OpCode.ReadChar, false) },
			{ new string (new char[] { tokenChan, tokenDang, tokenChan, tokenChan }), new Tuple<OpCode,bool> (OpCode.ReadNum, false) }
		};

		public Scanner ()
		{
		}

		int ReadNumber (string s, int start, out int offset)
		{
			int num = 0;
			bool readingPrefix = true;
			bool isNegative = false;
			int i;
			for (i = start; i < s.Length; i++) {
				if (readingPrefix && (s [i] == tokenGong || s [i] == tokenChan)) {
					isNegative = (s [i] == tokenChan) ? true : false;
					tokLineno = lineno;
					tokColumn = column;
					readingPrefix = false;
					column++;
				} else if (s [i] == tokenGong) {
					num = num << 1;
					column++;
				} else if (s [i] == tokenChan) {
					num = (num << 1) | 1;
					column++;
				} else if (s [i] == tokenDang) {
					column++;
					break;
				} else if (s [i] == '\n') {
					lineno++;
					column = 0;
				} else
					column++;
			}
			offset = i - start;
			return (isNegative ? -num : num);
		}

		List<Instruction> PreProccess (List<Instruction> prog)
		{
			var labels = prog
				.Select ((x, i) => new { Instruction = x, Index = i})
				.Where (x => x.Instruction.opcode == OpCode.Mark)
				.ToDictionary (k => k.Instruction.operand, v => v.Index);
			foreach (var instr in prog.Where(x => x.opcode == OpCode.Call || x.opcode == OpCode.Jump || x.opcode == OpCode.JumpIfZero || x.opcode == OpCode.JumpIfNeg))
				instr.operand = labels [instr.operand];
			return prog.ToList ();
		}

		public IEnumerable<Instruction> Scan (Stream stream)
		{
			StreamReader reader = new StreamReader (stream);
			return Scan (reader.ReadToEnd ());
		}

		public List<Instruction> Scan (string s)
		{
			List<Instruction> prog = new List<Instruction> ();
			StringBuilder buf = new StringBuilder ();
			int offset;
			for (int i = 0; i < s.Length; i++) {
				if (s [i] == tokenGong || s [i] == tokenChan || s [i] == tokenDang) {
					if (!readingToken) {
						readingToken = true;
						tokLineno = lineno;
						tokColumn = column;
					}
					buf.Append (s [i]);
					column++;
				} else if (s [i] == '\n') {
					lineno++;
					column = 0;
				} else
					column++;

				if (opcodeMapping.ContainsKey (buf.ToString ())) {
					Tuple<OpCode, bool> opInfo = opcodeMapping [buf.ToString ()];
					Instruction instr = new Instruction (opInfo.Item1, tokLineno, tokColumn);
					if (opInfo.Item2) {
						instr.operand = ReadNumber (s, i + 1, out offset);
						i += offset + 1;
					}
					buf.Clear ();
					prog.Add (instr);
					readingToken = false;
				}
			}
			return PreProccess (prog);
		}
	}
}


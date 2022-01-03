﻿//
//  ModeValue32.cs
//
//  Author:
//       Jae Stutzman <jaebird@gmail.com>
//
//  Copyright (c) 2016 Jae Stutzman
//
//  This library is free software; you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as
//  published by the Free Software Foundation; either version 2.1 of the
//  License, or (at your option) any later version.
//
//  This library is distributed in the hope that it will be useful, but
//  WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//  Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public
//  License along with this library; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

using System;
using System.Runtime.InteropServices;

namespace HamLibSharp.x86
{
	[StructLayout (LayoutKind.Sequential)]
	internal struct ModeValue32 : IModeValue
	{
		public RigMode Modes { get { return modes; } }

		public int Value { get { return val; } }

		public const int Any = 0;

		// Bit field of RIG_MODE's
		RigMode modes;
		// val
		int val;
	};
}


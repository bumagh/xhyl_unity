using System;

namespace FullInspector.Rotorz.ReorderableList
{
	[Flags]
	public enum ReorderableListFlags
	{
		DisableReordering = 0x1,
		HideAddButton = 0x2,
		HideRemoveButtons = 0x4,
		DisableContextMenu = 0x8,
		DisableDuplicateCommand = 0x10,
		DisableAutoFocus = 0x20,
		ShowIndices = 0x40,
		DisableClipping = 0x80
	}
}

﻿namespace Microsoft.Fast.Components.FluentUI;

public enum IconSize
{
    /// <summary>
    /// FluentUI System Icon font size 10x10
    /// </summary>
    Size10 = 10,
    /// <summary>
    /// FluentUI System Icon font size 12x12
    /// </summary>
    Size12 = 12,
    /// <summary>
    /// FluentUI System Icon font size 16x16
    /// </summary>
    Size16 = 16,
    /// <summary>
    /// FluentUI System Icon font size 20x20
    /// </summary>
    Size20 = 20,
    /// <summary>
    /// FluentUI System Icon font size 24x24
    /// </summary>
    Size24 = 24,
    /// <summary>
    /// FluentUI System Icon font size 28x28
    /// </summary>
    Size28 = 28,
    /// <summary>
    /// FluentUI System Icon font size 32x32
    /// </summary>
    Size32 = 32,
    /// <summary>
    /// FluentUI System Icon font size 48x48
    /// </summary>
    Size48 = 48

}

public static class IconSizeExtensions
{
    private static readonly Dictionary<IconSize, int> _iconSizeValues =
        Enum.GetValues<IconSize>().ToDictionary(id => id, id => (int)id);

    public static int? ToAttributeValue(this IconSize? value) => value == null ? null : _iconSizeValues[value.Value];
}
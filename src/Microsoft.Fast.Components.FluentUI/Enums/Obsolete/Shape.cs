﻿namespace Microsoft.Fast.Components.FluentUI;

[Obsolete("Use SkeletonShape instead", true)]
public enum Shape
{
    Rect,
    Circle
}

[Obsolete("Use SkeletonShape instead", true)]
public static class ShapeExtensions
{
    private static readonly Dictionary<Shape, string> _orientationValues =
        Enum.GetValues<Shape>().ToDictionary(id => id, id => Enum.GetName(id)!.ToLowerInvariant());

    public static string? ToAttributeValue(this Shape? value) => value == null ? null : _orientationValues[value.Value];
}
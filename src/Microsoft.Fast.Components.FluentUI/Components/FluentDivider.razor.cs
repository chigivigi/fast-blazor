using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentDivider : FluentComponentBase
{
    /// <summary>
    /// The role of the element.
    /// </summary>
    [Parameter]
    public DividerRole? Role { get; set; }

    /// <summary>
    /// The orientation of the divider.
    /// </summary>
    [Parameter]
    public Orientation? Orientation { get; set; }

}


using Microsoft.AspNetCore.Components;

namespace Jina.Passion.Client.Pages.Components.Trend;

public partial class Trend
{
    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public string Flag { get; set; }
}
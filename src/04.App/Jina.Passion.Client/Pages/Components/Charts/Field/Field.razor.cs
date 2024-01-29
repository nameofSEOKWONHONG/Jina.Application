using Microsoft.AspNetCore.Components;

namespace Jina.Passion.Client.Pages.Components.Charts.Field
{
    public partial class Field
    {
        [Parameter]
        public string Label { get; set; }

        [Parameter]
        public string Value { get; set; }
    }
}
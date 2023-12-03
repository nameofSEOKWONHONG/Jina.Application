using Jina.Domain.Account;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Jina.Passion.Client.Pages.Weather.ViewModels
{
    public class WeatherPdfDocument : IDocument
    {
        private WeatherViewModel _viewModel;

        public WeatherPdfDocument(WeatherViewModel model)
        {
            _viewModel = model;
        }

        public void Compose(IDocumentContainer container)
        {
        }
    }
}
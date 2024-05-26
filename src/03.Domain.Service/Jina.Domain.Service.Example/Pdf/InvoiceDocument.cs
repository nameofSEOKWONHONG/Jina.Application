using eXtensionSharp;
using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.SKCharts;
using LiveChartsCore.SkiaSharpView.VisualElements;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SkiaSharp;

namespace Jina.Domain.Service.Example.Weather;

public class InvoiceDocument : IDocument
{
    public InvoiceModel Model { get; }

    public InvoiceDocument(InvoiceModel model)
    {
        Model = model;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    public DocumentSettings GetSettings() => DocumentSettings.Default;

    /* code omitted */

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(50);
            
                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);

                    
                page.Footer().AlignCenter().Text(x =>
                {
                    x.CurrentPageNumber();
                    x.Span(" / ");
                    x.TotalPages();
                });
            });
    }

    void ComposeHeader(IContainer container)
    {
        var titleStyle = TextStyle.Default.FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);
    
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Text($"Invoice #{Model.InvoiceNumber}").Style(titleStyle);

                column.Item().Text(text =>
                {
                    text.Span("Issue date: ").SemiBold();
                    text.Span($"{Model.IssueDate:d}");
                });
                
                column.Item().Text(text =>
                {
                    text.Span("Due date: ").SemiBold();
                    text.Span($"{Model.DueDate:d}");
                });
            });

            row.ConstantItem(100).Height(50).Placeholder();
        });
    }

    void ComposeContent(IContainer container)
    {
        container.PaddingVertical(40).Column(column =>
        {
            column.Spacing(10);
            
            column.Item().Row(row =>
            {
                row.RelativeItem().Component(new AddressComponent("From", Model.SellerAddress));
                row.ConstantItem(50);
                row.RelativeItem().Component(new AddressComponent("For", Model.CustomerAddress));
            });

            column.Item().Element(ComposeTable);
            
            var totalPrice = Model.Items.Sum(x => x.Price * x.Quantity);
            column.Item().AlignRight().Text($"Grand total: {totalPrice}$").FontSize(14);

            var cartesianChart = new SKCartesianChart
            {
                Width = 900,
                Height = 600,
                Series = new ISeries[]
                {
                    new LineSeries<int> { Values = new int[] { 1, 5, 4, 6 }, Name = "test"},
                    new ColumnSeries<int> { Values = new int[] { 4, 8, 2, 4 }, Name = "test2"}
                },
                Title = new LabelVisual
                {
                    Text = "Hello LiveCharts",
                    TextSize = 30,
                    Padding = new Padding(15),
                    Paint = new SolidColorPaint(SKColors.Brown)
                },
                LegendBackgroundPaint = new SolidColorPaint(SKColors.White),
                LegendPosition = LiveChartsCore.Measure.LegendPosition.Left,
                LegendTextPaint = new LinearGradientPaint(new []{SKColors.Red, SKColors.Red}),
                Background = SKColors.White,
            };
            
            // you can save the image to png (by default)
            // or use the second argument to specify another format.
            cartesianChart.SaveImage("cartesianChart.png");

            // additionally you can save a chart as svg:
            // for more info see: https://github.com/mono/SkiaSharp/blob/main/tests/Tests/SKCanvasTest.cs#L396
            using var stream = new MemoryStream();
            var svgCanvas = SKSvgCanvas.Create(SKRect.Create(cartesianChart.Width, cartesianChart.Height), stream);
            cartesianChart.DrawOnCanvas(svgCanvas);
            svgCanvas.Dispose(); // <- dispose it before using the stream, otherwise the svg could not be completed.

            stream.Position = 0;
            using (var fs = new FileStream("cartesianChart.svg", FileMode.OpenOrCreate))
            {
                stream.CopyTo(fs);                
            }

            column.Item()
                .Svg("cartesianChart.svg");
            
            if (!string.IsNullOrWhiteSpace(Model.Comments))
                column.Item().PaddingTop(150).Element(ComposeComments);
        });
    }

    void ComposeTable(IContainer container)
    {
        container.Table(table =>
        {
            // step 1
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(25);
                columns.RelativeColumn(3);
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
            });
            
            // step 2
            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("#");
                header.Cell().Element(CellStyle).Text("Product");
                header.Cell().Element(CellStyle).AlignRight().Text("Unit price");
                header.Cell().Element(CellStyle).AlignRight().Text("Quantity");
                header.Cell().Element(CellStyle).AlignRight().Text("Total");
                
                static IContainer CellStyle(IContainer container)
                {
                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                }
            });
            
            // step 3
            foreach (var item in Model.Items)
            {
                table.Cell().Element(CellStyle).Text((Model.Items.IndexOf(item) + 1).xValue<string>());
                table.Cell().Element(CellStyle).Text(item.Name);
                table.Cell().Element(CellStyle).AlignRight().Text($"{item.Price}$");
                table.Cell().Element(CellStyle).AlignRight().Text(item.Quantity.xValue<string>());
                table.Cell().Element(CellStyle).AlignRight().Text($"{item.Price * item.Quantity}$");
                
                static IContainer CellStyle(IContainer container)
                {
                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                }
            }
        });
    }

    void ComposeComments(IContainer container)
    {
        container.Background(Colors.Grey.Lighten3).Padding(10).Column(column =>
        {
            column.Spacing(5);
            column.Item().Text("Comments").FontSize(14);
            column.Item().Text(Model.Comments);
        });
    }
}
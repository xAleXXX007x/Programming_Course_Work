using ElectronicsShopBusinessLogic.HelperModels;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftFactoryBusinessLogic
{
    static class SaveToPdf
    {
        public static void CreateDoc(OrderPaymentsInfo info)
        {
            Document document = new Document();
            DefineStyles(document);
            Section section = document.AddSection();
            Paragraph paragraph = section.AddParagraph(info.Title);
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.Style = "NormalTitle";

            foreach (var order in info.Orders)
            {
                var orderLabel = section.AddParagraph("Заказ №" + order.Id + " от " + order.Date.ToString());
                orderLabel.Style = "NormalTitle";
                orderLabel.Format.SpaceBefore = "1cm";
                orderLabel.Format.SpaceAfter = "0,25cm";

                section.AddParagraph("Тип доставки: " + order.Shipping.ToString());
                section.AddParagraph("Адрес: " + (order.Address != null ? order.Address : "Адрес магазина"));

                var productsLabel = section.AddParagraph("Товары:");
                productsLabel.Style = "NormalTitle";

                var productTable = document.LastSection.AddTable();

                List<string> headerWidths = new List<string> { "1cm", "3cm", "4cm", "4cm", "2cm" };

                foreach (var elem in headerWidths)
                {
                    productTable.AddColumn(elem);
                }

                CreateRow(new PdfRowParameters
                {
                    Table = productTable,
                    Texts = new List<string> { "№", "Название", "Описание", "Количество", "Цена" },
                    Style = "NormalTitle",
                    ParagraphAlignment = ParagraphAlignment.Center
                });

                int i = 1;

                foreach (var product in order.Products)
                {
                    CreateRow(new PdfRowParameters
                    {
                        Table = productTable,
                        Texts = new List<string> { i.ToString(), product.Name, product.Desc, product.Count.ToString(), (product.Price * product.Count).ToString() },
                        Style = "Normal",
                        ParagraphAlignment = ParagraphAlignment.Left
                    });

                    i++;
                }

                CreateRow(new PdfRowParameters
                {
                    Table = productTable,
                    Texts = new List<string> { "", "", "", "Итого:", order.Sum.ToString() },
                    Style = "Normal",
                    ParagraphAlignment = ParagraphAlignment.Left
                });

                CreateRow(new PdfRowParameters
                {
                    Table = productTable,
                    Texts = new List<string> { "", "", "", "Оплачено:", order.SumPaid.ToString() },
                    Style = "Normal",
                    ParagraphAlignment = ParagraphAlignment.Left
                });

                if (info.Payments[order.Id].Count == 0)
                {
                    continue;
                }

                var paymentsLabel = section.AddParagraph("Оплаты:");
                paymentsLabel.Style = "NormalTitle";

                var paymentTable = document.LastSection.AddTable();

                headerWidths = new List<string> { "1cm", "3cm", "3cm", "3cm", "2cm" };

                foreach (var elem in headerWidths)
                {
                    paymentTable.AddColumn(elem);
                }

                CreateRow(new PdfRowParameters
                {
                    Table = paymentTable,
                    Texts = new List<string> { "№", "Дата", "Клиент", "Счёт", "Сумма" },
                    Style = "NormalTitle",
                    ParagraphAlignment = ParagraphAlignment.Center
                });

                i = 1;

                foreach (var payment in info.Payments[order.Id])
                {
                    CreateRow(new PdfRowParameters
                    {
                        Table = paymentTable,
                        Texts = new List<string> { i.ToString(), payment.Date.ToString(), payment.ClientLogin, payment.Account, payment.Sum.ToString() },
                        Style = "Normal",
                        ParagraphAlignment = ParagraphAlignment.Left
                    });

                    i++;
                }
            }

            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true)
            {
                Document = document
            };
            renderer.RenderDocument();
            renderer.PdfDocument.Save(info.FileName);
        }

        private static void DefineStyles(Document document)
        {
            Style style = document.Styles["Normal"];
            style.Font.Name = "Times New Roman";
            style.Font.Size = 14;
            style = document.Styles.AddStyle("NormalTitle", "Normal");
            style.Font.Bold = true;
        }

        private static void CreateRow(PdfRowParameters rowParameters)
        {
            Row row = rowParameters.Table.AddRow();
            for (int i = 0; i < rowParameters.Texts.Count; ++i)
            {
                FillCell(new PdfCellParameters
                {
                    Cell = row.Cells[i],
                    Text = rowParameters.Texts[i],
                    Style = rowParameters.Style,
                    BorderWidth = 0.5,
                    ParagraphAlignment = rowParameters.ParagraphAlignment
                });
            }
        }

        private static void FillCell(PdfCellParameters cellParameters)
        {
            cellParameters.Cell.AddParagraph(cellParameters.Text);
            if (!string.IsNullOrEmpty(cellParameters.Style))
            {
                cellParameters.Cell.Style = cellParameters.Style;
            }
            cellParameters.Cell.Borders.Left.Width = cellParameters.BorderWidth;
            cellParameters.Cell.Borders.Right.Width = cellParameters.BorderWidth;
            cellParameters.Cell.Borders.Top.Width = cellParameters.BorderWidth;
            cellParameters.Cell.Borders.Bottom.Width = cellParameters.BorderWidth;
            cellParameters.Cell.Format.Alignment = cellParameters.ParagraphAlignment;
            cellParameters.Cell.VerticalAlignment = VerticalAlignment.Center;
        }
    }
}

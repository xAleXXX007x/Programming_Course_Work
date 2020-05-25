using AircraftFactoryBusinessLogic;
using ElectronicsShopBusinessLogic.BindingModels;
using ElectronicsShopBusinessLogic.Enums;
using ElectronicsShopBusinessLogic.HelperModels;
using ElectronicsShopBusinessLogic.Interfaces;
using ElectronicsShopBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ElectronicsShopBusinessLogic.BusinessLogics
{
    public class ReportLogic
    {
        private readonly IProductLogic productLogic;
        private readonly IOrderLogic orderLogic;
        private readonly IPaymentLogic paymentLogic;

        public ReportLogic(IProductLogic productLogic, IOrderLogic orderLogic, IPaymentLogic paymentLogic)
        {
            this.productLogic = productLogic;
            this.orderLogic = orderLogic;
            this.paymentLogic = paymentLogic;
        }

        public void SendOrderProducts(OrderViewModel order, string email, FileExtension ext)
        {
            string fileName = Directory.GetCurrentDirectory() + "\\Reports\\" + order.Id + (ext == FileExtension.Word ? ".docx" : ".xlsx");
            string subject = "Список товаров по заказу №" + order.Id;
            var products = new List<ProductViewModel>();

            foreach (var product in order.Products)
            {
                products.Add(productLogic.Read(new ProductBindingModel { Id = product.ProductId }).FirstOrDefault());
            }

            if (ext == FileExtension.Word)
            {
                SaveToWord.CreateDoc(new OrderProductsInfo
                {
                    FileName = fileName,
                    Title = subject,
                    Products = products
                });
            } else
            {
                SaveToExcel.CreateDoc(new OrderProductsInfo
                {
                    FileName = fileName,
                    Title = subject,
                    Products = products
                });
            }


            SendMail(email, fileName, subject);
        }

        public void SendOrdersReport(OrderBindingModel model, string email)
        {
            string fileName = Directory.GetCurrentDirectory() + "\\Reports\\periodreport.pdf";
            string subject = "Список заказов в период с " + model.Date.ToString() + " по " + model.DateTo.ToString();

            var orders = orderLogic.Read(model).ToList();

            Dictionary<int, List<PaymentViewModel>> payments = new Dictionary<int, List<PaymentViewModel>>();

            foreach (var order in orders)
            {
                var orderPayments = paymentLogic.Read(new PaymentBindingModel { OrderId = order.Id }).ToList();

                payments.Add(order.Id, orderPayments);
            }

            SaveToPdf.CreateDoc(new OrderPaymentsInfo
            {
                FileName = fileName,
                Title = subject,
                Orders = orders,
                Payments = payments
            });

            SendMail(email, fileName, subject);
        }

        public void SendMail(string email, string fileName, string subject)
        {
            MailAddress from = new MailAddress("alexandersmirnov1105@gmail.com", "Магазин электроники «Я только посмотреть»");
            MailAddress to = new MailAddress(email);

            MailMessage m = new MailMessage(from, to);
            m.Subject = subject;
            m.Attachments.Add(new Attachment(fileName));

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("alexandersmirnov1105@gmail.com", "1q2w3e$R");
            smtp.EnableSsl = true;
            smtp.Send(m);
        }
    }
}

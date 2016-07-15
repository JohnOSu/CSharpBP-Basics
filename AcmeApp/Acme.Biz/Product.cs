using Acme.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Biz
{
    /// <summary>
    /// Manages products carried in inventory
    /// </summary>
    public class Product
    {
        #region Constructors
        public Product()
        {
            Console.WriteLine("Product instance created");
            this.Category = "Tools";
        }
        public Product(int productId, string productName, string description) : this()
        {
            this.ProductName = productName;
            this.Description = description;
            this.ProductId = productId;
            Console.WriteLine(string.Format("Product Instance has a name: {0}", ProductName));
        }
        #endregion

        #region Properties
        public DateTime? AvailabilityDate { get; set; }
        public decimal Cost { get; set; }

        private string productName;

        public string ProductName
        {
            get
            {
                return productName?.Trim();
            }
            set
            {
                if (value.Trim().Length < 3)
                {
                    ValidationMessage = "Product Name must be at least 3 characters";
                }
                else if (value.Trim().Length > 20)
                {
                    ValidationMessage = "Product Name cannot be more than 20 characters";
                }
                else
                {
                    productName = value;
                }
            }
        }
        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        private int productId;

        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }
        private Vendor productVendor;

        public Vendor ProductVendor
        {
            get
            {
                if (productVendor == null)
                {
                    productVendor = new Vendor();
                }
                return productVendor;
            }
            set { productVendor = value; }
        }

        public string ValidationMessage { get; private set; }
        internal string Category { get; set; }
        public int SequenceNumber { get; set; } = 1;

        public string ProductCode => string.Format("{0}-{1}", Category, SequenceNumber);

        #endregion

        /// <summary>
        /// Calculates the suggested retail price
        /// </summary>
        /// <param name="markupPerCent">Percent used to calculate the cost</param>
        /// <returns></returns>
        public decimal CalculateSuggestedPrice(decimal markupPerCent) => this.Cost + (this.Cost * markupPerCent / 100);


        public string SayHello()
        {
            var emailService = new EmailService();
            var confirmation = emailService.SendMessage("New Product", this.ProductName, "sales@abc.com");
            var result = LoggingService.LogAction("saying hello");
            return string.Format("Hello {0} ({1}): {2}", ProductName, ProductId, Description);
        }

        public override string ToString() => string.Format("{0} ({1})", ProductName, ProductId);

    }
}

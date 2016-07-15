using Acme.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Biz
{
    /// <summary>
    /// Manages the vendors from whom we purchase our inventory.
    /// </summary>
    public class Vendor 
    {
        public enum IncludeAddress { Yes, No };
        public enum SendCopy { Yes, No };

        public int VendorId { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }

        /// <summary>
        /// Sends a product order to the vendor
        /// </summary>
        /// <param name="product">Product to order.</param>
        /// <param name="quantity">Quantity of the product to order.</param>
        /// <param name="deliverBy">When to deliver the order.</param>
        /// <param name="instructions">Delivery instructions</param>
        /// <returns></returns>
        public OperationResult<bool> PlaceOrder(Product product, int quantity, DateTimeOffset? deliverBy = null, string instructions = "Standard delivery")
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
            if (deliverBy <= DateTimeOffset.Now) throw new ArgumentOutOfRangeException(nameof(deliverBy));

            var success = false;

            var orderText = string.Format("Order from Acme, Inc\nProduct: {0}\nQuantity: {1}", product.ProductCode, quantity);

            if (deliverBy.HasValue)
            {
                orderText = string.Format("{0}\nDeliver By: {1}", orderText, deliverBy.Value.ToString("d"));
            }

            if (!String.IsNullOrEmpty(instructions))
            {
                orderText = string.Format("{0}\nInstructions: {1}", orderText, instructions);
            }

            var emailService = new EmailService();
            var confirmation = emailService.SendMessage("New Order", orderText, this.Email);

            if (confirmation.StartsWith("Message sent:"))
            {
                success = true;
            }

            return new OperationResult<bool>(success, orderText);
        }


        /// <summary>
        /// Sends a product order to the vendor
        /// </summary>
        /// <param name="product">Product to order.</param>
        /// <param name="quantity">Quantity of the product to order.</param>
        /// <param name="includeAddress">True to include the shipping address</param>
        /// <param name="sendCopy">True to send copy of email to customer</param>
        /// <returns>Success flag and order text</returns>
        public OperationResult<bool> PlaceOrder(Product product, int quantity, IncludeAddress includeAddress, SendCopy sendCopy)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));

            var success = false;

            var orderText = string.Format("Order from Acme, Inc\nProduct: {0}\nQuantity: {1}", product.ProductCode, quantity);
            if (includeAddress == IncludeAddress.Yes) orderText += "\nWithAddress";
            if (sendCopy.Equals(IncludeAddress.Yes)) orderText += "\nWith Copy";

            var emailService = new EmailService();
            var confirmation = emailService.SendMessage("New Order", orderText, this.Email);

            if (confirmation.StartsWith("Message sent:"))
            {
                success = true;
            }

            return new OperationResult<bool>(success, orderText);
        }

        /// <summary>
        /// Sends an email to welcome a new vendor.
        /// </summary>
        /// <returns></returns>
        public string SendWelcomeEmail(string message)
        {
            var emailService = new EmailService();
            var subject = string.Format("Hello {0}", this.CompanyName).Trim();
            //var subject = "Hello" + this.CompanyName;
            var confirmation = emailService.SendMessage(subject,
                                                        message, 
                                                        this.Email);
            return confirmation;
        }

        public override string ToString() => $"Vendor: {CompanyName} ({VendorId})";

        /// <summary>
        /// Overridden to support comparison
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Vendor compareVendor = obj as Vendor;
            if (compareVendor != null &&
                VendorId == compareVendor.VendorId &&
                CompanyName == compareVendor.CompanyName &&
                Email == compareVendor.Email)
                return true;

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Acme.Biz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acme.Common;

namespace Acme.Biz.Tests
{
    [TestClass()]
    public class VendorTests
    {
        [TestMethod()]
        public void SendWelcomeEmail_ValidCompany_Success()
        {
            // Arrange
            var vendor = new Vendor();
            vendor.CompanyName = "ABC Corp";
            var expected = "Message sent: Hello ABC Corp";

            // Act
            var actual = vendor.SendWelcomeEmail("Test Message");

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void SendWelcomeEmail_EmptyCompany_Success()
        {
            // Arrange
            var vendor = new Vendor();
            vendor.CompanyName = "";
            var expected = "Message sent: Hello";

            // Act
            var actual = vendor.SendWelcomeEmail("Test Message");

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void SendWelcomeEmail_NullCompany_Success()
        {
            // Arrange
            var vendor = new Vendor();
            vendor.CompanyName = null;
            var expected = "Message sent: Hello";

            // Act
            var actual = vendor.SendWelcomeEmail("Test Message");

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void PlaceOrderTest()
        {
            // Arrange
            var vendor = new Vendor();
            var product = new Product(1, "Saw", "");
            var expected = new OperationResult<bool>(true, "Order from Acme, Inc\nProduct: Tools-1\nQuantity: 12\nInstructions: Standard delivery");

            // Act
            var actual = vendor.PlaceOrder(product, 12);

            // Assert
            Assert.AreEqual(expected.Result, actual.Result);
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [TestMethod()]
        public void PlaceOrderTest_3Parameters()
        {
            // Arrange
            var vendor = new Vendor();
            var product = new Product(1, "Saw", "");
            var expected = new OperationResult<bool>(true, "Order from Acme, Inc\nProduct: Tools-1\nQuantity: 12\nDeliver By: 25/10/2016\nInstructions: Standard delivery");

            // Act
            var actual = vendor.PlaceOrder(product, 12, new DateTimeOffset(2016, 10, 25, 0, 0, 0, new TimeSpan(-7, 0, 0)));

            // Assert
            Assert.AreEqual(expected.Result, actual.Result);
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PlaceOrder_NullProduct_Exception()
        {
            // Arrange
            var vendor = new Vendor();

            // Act
            var actual = vendor.PlaceOrder(null, 12);

            // Assert
            //Expected Exception
        }

        [TestMethod()]
        public void PlaceOrderTest_WithAddress()
        {
            // Arrange
            var vendor = new Vendor();
            var product = new Product(1, "Saw", "");
            var expected = new OperationResult<bool>(true, "Order from Acme, Inc\nProduct: Tools-1\nQuantity: 12\nWithAddress");

            // Act
            var actual = vendor.PlaceOrder(product, 12, Vendor.IncludeAddress.Yes, Vendor.SendCopy.No);

            // Assert
            Assert.AreEqual(expected.Result, actual.Result);
            Assert.AreEqual(expected.Message, actual.Message);
        }


        [TestMethod()]
        public void PlaceOrderTest_NoDeliveryDate()
        {
            // Arrange
            var vendor = new Vendor();
            var product = new Product(1, "Saw", "");
            var expected = new OperationResult<bool>(true, "Order from Acme, Inc\nProduct: Tools-1\nQuantity: 12\nInstructions: Deliver to Suite 42");

            // Act
            var actual = vendor.PlaceOrder(product, 12, instructions: "Deliver to Suite 42" );

            // Assert
            Assert.AreEqual(expected.Result, actual.Result);
            Assert.AreEqual(expected.Message, actual.Message);
        }
    }
}
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OhmValue.Controllers;
using System.Web.Mvc;

namespace OhmValue.Tests
{
    [TestClass]
    public class OhaValueControllerTest
    {
        [TestMethod]
        public void OhmValueCalculator()
        {
            // Arrange
            OhmValueController ovc = new OhmValueController();

            // Act
            ViewResult result = ovc.OhmValueCalculator() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}

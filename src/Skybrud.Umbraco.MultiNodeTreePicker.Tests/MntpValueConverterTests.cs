using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Skybrud.Umbraco.MultiNodeTreePicker.Composers;
using Skybrud.Umbraco.MultiNodeTreePicker.Converters;
using Skybrud.Umbraco.MultiNodeTreePicker.PropertyEditors.ValueConverters;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;

namespace Skybrud.Umbraco.MultiNodeTreePicker.Tests
{
    [TestClass]
    public class MntpValueConverterTests
    {
        private readonly MntpValueConverter _converter;
        private MntpConverterCollection _converterCollection;

        public MntpValueConverterTests() {
            _converterCollection = new MntpConverterCollection(() => new List<IMntpItemConverter>());
            _converter = new MntpValueConverter(Mock.Of<IPublishedSnapshotAccessor>(), Mock.Of<IUmbracoContextAccessor>(), Mock.Of<IMemberService>(), _converterCollection);
        }

        [DataTestMethod]
        [DataRow(null)]
        public void ConvertSourceToIntermediate_Source_Should_Be_Null(object? source)
        {
            var response = _converter.ConvertSourceToIntermediate(Mock.Of<IPublishedElement>(), Mock.Of<IPublishedPropertyType>(), source, false);

            Assert.IsNull(response);
        }

        [DataTestMethod]
        [DataRow(",")]
        [DataRow("")]
        [DataRow(",,,,,,")]
        public void ConvertSourceToIntermediate_Source_Should_Be_Empty(object? source) {
            var response = _converter.ConvertSourceToIntermediate(Mock.Of<IPublishedElement>(), Mock.Of<IPublishedPropertyType>(), source, false);

            Assert.IsInstanceOfType(response, typeof(Udi[]));
        }

        [DataTestMethod]
        [DataRow("umb://document/4fed18d8c5e34d5e88cfff3a5b457bf2umb://document/4fed18d8c5e34d5e88cfff3a5b457bf2")]
        [DataRow(",umb://document/4fed18d8c5e34d5e88cfff3a5b457bf2umb://document/4fed18d8c5e34d5e88cfff3a5b457bf2,")]
        [DataRow("umb://document/4fed18d8c5e34d5e88cfff3a5b457bf2umb://document/4fed18d8c5e34d5e88cfff3a5b457bf2")]
        [DataRow("abc")]
        public void ConvertSourceToIntermediate_Source_Should_Fail(object? source) {
            Assert.ThrowsException<FormatException>(() => _converter.ConvertSourceToIntermediate(Mock.Of<IPublishedElement>(), Mock.Of<IPublishedPropertyType>(), source, false));
        }

        [DataTestMethod]
        [DataRow("umb://document/4fed18d8c5e34d5e88cfff3a5b457bf2,umb://document/4fed18d8c5e34d5e88cfff3a5b457bf2")]
        [DataRow("umb://document/4fed18d8c5e34d5e88cfff3a5b457bf2")]
        [DataRow("umb://document/4fed18d8c5e34d5e88cfff3a5b457bf2,,umb://document/4fed18d8c5e34d5e88cfff3a5b457bf2")]
        [DataRow(",umb://document/4fed18d8c5e34d5e88cfff3a5b457bf2,umb://document/4fed18d8c5e34d5e88cfff3a5b457bf2,")]
        public void ConvertSourceToIntermediate_Source_Should_Not_Be_Null(object source) {

            var response = _converter.ConvertSourceToIntermediate(Mock.Of<IPublishedElement>(), Mock.Of<IPublishedPropertyType>(), source, false);

            Assert.IsNotNull(response);
        }
    }
}
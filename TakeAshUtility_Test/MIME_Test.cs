using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TakeAshUtility;

namespace TakeAshUtility_Test {

    [TestFixture]
    class MIME_Test {

        public const string DefaultExtension = ".bin";
        public const string DefaultMIMEType = "application/octet-stream";

        [TestCase(null, DefaultExtension)]
        [TestCase("", DefaultExtension)]
        [TestCase("NotExists", DefaultExtension)]
        [TestCase("text/plain", ".txt")]
        [TestCase("text/html", ".htm")]
        [TestCase("text/css", ".css")]
        //[TestCase("application/javascript", ".js")]
        [TestCase("image/jpeg", ".jpg")]
        [TestCase("image/png", ".png")]
        [TestCase("video/mp4", ".mp4")]
        [TestCase("application/x-zip-compressed", ".zip")]
        [TestCase(DefaultMIMEType, DefaultExtension)]
        public void GetDefaultExtension_Test(string mimeType, string expected) {
            var actual = MIME.GetDefaultExtension(mimeType);
            Assert.AreEqual(expected, actual);
        }

        [TestCase(null, DefaultMIMEType)]
        [TestCase("", DefaultMIMEType)]
        [TestCase("NotExists", DefaultMIMEType)]
        [TestCase(".txt", "text/plain")]
        [TestCase(".htm", "text/html")]
        [TestCase(".html", "text/html")]
        [TestCase(".css", "text/css")]
        [TestCase(".js", "application/javascript")]
        [TestCase(".jpg", "image/jpeg")]
        [TestCase(".jpeg", "image/jpeg")]
        [TestCase(".png", "image/png")]
        [TestCase(".mp4", "video/mp4")]
        [TestCase(".zip", "application/x-zip-compressed")]
        [TestCase(DefaultExtension, DefaultMIMEType)]
        public void GetMimeTypeFromExtension_Test(string extension, string expected) {
            var actual = MIME.GetMimeTypeFromExtension(extension);
            Assert.AreEqual(expected, actual);
        }
    }
}

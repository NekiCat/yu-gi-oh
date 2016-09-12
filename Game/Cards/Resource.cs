using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigeR.YuGiOh.Core.Cards
{
    /// <summary>
    /// A resource embedded in the card, for example custom images.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Resource<T>
    {
        /// <summary>
        /// The identifier of the resource.
        /// </summary>
        public string Filename { get; }

        /// <summary>
        /// The raw data of the resource.
        /// </summary>
        public Stream Stream { get; }

        /// <summary>
        /// The mime type of the resource.
        /// </summary>
        public string Mimetype { get; }

        /// <summary>
        /// The parsed data of the resource. May be null.
        /// </summary>
        public T Value { get; }

        public Resource(string filename, Stream stream, string mime, T value)
        {
            Filename = filename;
            Stream = stream;
            Mimetype = mime;
            Value = value;
        }

        public Resource(string filename, Stream stream, string mime) : this(filename, stream, mime, default(T)) { }

        public Resource(string filename, Stream stream) : this(filename, stream, "application/octet-stream", default(T)) { }
    }

    public class Resource : Resource<Object>
    {
        public Resource(string filename, Stream stream, string mime, object value) : base(filename, stream, mime, value) { }

        public Resource(string filename, Stream stream, string mime) : base(filename, stream, mime) { }

        public Resource(string filename, Stream stream) : base(filename, stream) { }
    }
}

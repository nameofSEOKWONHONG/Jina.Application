using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jina.Domain.SharedKernel.File
{
    public class ExportOption
    {
        /// <summary>
        /// MimeType
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// byte array
        /// </summary>
        public byte[] Bytes { get; set; }

        /// <summary>
        /// file name
        /// </summary>
        public string FileName { get; set; }
    }
}
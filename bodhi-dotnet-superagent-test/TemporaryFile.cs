﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Superagent.Test { 
    public sealed class TemporaryFile : IDisposable
    {
        private readonly FileInfo file;
        public FileInfo FileInfo { get { return file; } }

        public TemporaryFile() : this(Path.GetTempFileName()) { }
        public TemporaryFile(string fileName) : this(new FileInfo(fileName)) { }
        public TemporaryFile(FileInfo temporaryFile)
        {
            file = temporaryFile;
        }
        public TemporaryFile(Stream initialFileContents) : this()
        {
            using (var file = new FileStream(this, FileMode.Open))
                initialFileContents.CopyTo(file);
        }

        public static implicit operator FileInfo(TemporaryFile temporaryFile)
        {
            return temporaryFile.file;
        }
        public static implicit operator string(TemporaryFile temporaryFile)
        {
            return temporaryFile.file.FullName;
        }
        public static explicit operator TemporaryFile(FileInfo temporaryFile)
        {
            return new TemporaryFile(temporaryFile);
        }

        private volatile bool disposed;
        public void Dispose()
        {
            try
            {
                file.Delete();
                disposed = true;
            }
            catch (Exception) { } // Ignore
        }
        ~TemporaryFile()
        {
            if (!disposed) Dispose();
        }
    }
}

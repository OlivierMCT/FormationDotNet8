using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CATodos.BusinessModels {
    public class CATodoException : Exception {     
        public int Code { get; init; }   

        public CATodoException(int code, string? message) : base(message) {
            Code = code;
        }

        public CATodoException(int code, string? message, Exception? innerException) : base(message, innerException) {
            Code = code;
        }
    }
}

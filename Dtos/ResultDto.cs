using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.Dtos
{
    public class ResultDto
    {
        public ResultDto(bool succedded, List<Error> errors)
        {
            Succedded = succedded;
            Errors = errors;
        }

        public bool Succedded { get; set; }
        public List<Error> Errors { get; set; }
    }
    public class Error
    {
        public Error(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public string Code { get; set; }
        public string Description { get; set; }
    }
}

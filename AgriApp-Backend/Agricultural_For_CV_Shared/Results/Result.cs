using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agricultural_For_CV_Shared.Results
{
    public class Result
    {

        public bool IsSuccess { get; protected set; }
        public string Message { get; protected set; }
        public string Error { get; protected set; }

        public static Result Success() => new Result { IsSuccess = true };
        public static Result Success(string success) => new Result { IsSuccess = true , Message =  success};

        public static Result Failure(string error) => new Result { IsSuccess = false, Error = error };

    }


    public class Result<T> : Result
    {

        public T? Data {  get; private set; }

        public static Result<T> Success(T data) => new Result<T> {  Data = data, IsSuccess = true  };
        public static Result<T> Success(T data, string success) => new Result<T> {  Data = data, IsSuccess = true , Message = success };
        public static Result<T> Failure(string error) => new Result<T> { IsSuccess = false, Error = error };

    }
}

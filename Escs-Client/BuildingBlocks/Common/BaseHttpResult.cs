﻿namespace BuildingBlocks.Common
{
    public class HttpCallOptions
    {
        public string BaseUrl { get; set; } = default!;
        public List<string> QueryStringElements { get; set; } = default!;
        public string AuthenticationType = default!;
        public string ApiKey = default!;
        public bool IsRetry = default!;
        public bool IsSerializedObject = default!;




        protected HttpCallOptions(bool isSerializedObject, bool isRetry, string baseUrl, List<string> queryStringElements, string authenticationType = "", string apiKey = "")
        {
            BaseUrl = baseUrl;
            QueryStringElements = queryStringElements;
            AuthenticationType = authenticationType;
            ApiKey = apiKey;
            IsRetry = isRetry;
            IsSerializedObject = isSerializedObject;

        }

        public static HttpCallOptions Authenticated(bool isSerialized, bool isRetry, string baseUrl, List<string> queryStringElements, string authenticationType, string apiKey)
        {
            return new HttpCallOptions(isSerialized, isRetry, baseUrl, queryStringElements, authenticationType, apiKey);

        }

        public static HttpCallOptions UnAuthenticated(bool isSerialized, bool isRetry, string baseUrl, List<string> queryStringElements)
        {
            return new HttpCallOptions(isSerialized, isRetry, baseUrl, queryStringElements, string.Empty, string.Empty);

        }
    }

    public class HttpCallOptions<T> : HttpCallOptions
    {

        public T Body { get; set; } = default!;

        private HttpCallOptions(bool isSerialized, bool isRetry, T body, string baseUrl, List<string> queryStringElements, string authenticationType = "", string apiKey = "")
            : base(isSerialized, isRetry, baseUrl, queryStringElements, authenticationType, apiKey)
        {
            Body = body;
        }



        public static HttpCallOptions<T> Authenticated(bool isSerialized, bool isRetry, T body, string baseUrl, List<string> queryStringElements, string authenticationType, string apiKey)
        {
            return new HttpCallOptions<T>(isSerialized, isRetry, body, baseUrl, queryStringElements, authenticationType, apiKey);
        }

        public static HttpCallOptions<T> UnAuthenticated(bool isSerialized, bool isRetry, T body, string baseUrl, List<string> queryStringElements)
        {
            return new HttpCallOptions<T>(isSerialized, isRetry, body, baseUrl, queryStringElements, string.Empty, string.Empty);
        }
    }


    public class BaseHttpResult
    {
        public bool Succeeded { get; set; }
        public int StatusCode { get; set; } = default!;
        public ExceptionError ExceptionError { get; set; }

        public BaseHttpResult()
        {

        }

        protected BaseHttpResult(bool succeeded = true, int statusCode = 200, ExceptionError exceptionError = default)
        {
            Succeeded = succeeded;
            StatusCode = statusCode;
            ExceptionError = exceptionError;
        }

        public static BaseHttpResult Success(int statusCode)
        {
            return new BaseHttpResult(true, statusCode);
        }

        public static BaseHttpResult Failure(int statusCode, ExceptionError exceptionError)
        {
            return new BaseHttpResult(false, statusCode, exceptionError);
        }
    }


    public class BaseHttpResult<T> : BaseHttpResult
    {

        public T Data { get; set; } = default!;

        protected BaseHttpResult(T data, bool succeeded = true, int statusCode = 0, ExceptionError exceptionError = default) : base(succeeded, statusCode, exceptionError)
        {
            Data = data;
        }

        public BaseHttpResult()
        {

        }

        public static BaseHttpResult<T> Success(T data, int statusCode)
        {
            return new BaseHttpResult<T>(data, true, statusCode);
        }

        public static BaseHttpResult Failure(T data, int statusCode, ExceptionError exceptionError)
        {
            return new BaseHttpResult<T>(data, false, statusCode, exceptionError);
        }
    }
}
using System;

namespace isriding.Web.Helper.Wxpay
{
    [Serializable]
    public class SDKRuntimeException : Exception
    {

        private const long serialVersionUID = 1L;

        public SDKRuntimeException(String str) : base(str)
        {

        }
    }
}
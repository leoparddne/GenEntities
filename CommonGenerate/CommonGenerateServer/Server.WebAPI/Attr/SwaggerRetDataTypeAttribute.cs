using System;

namespace Server.WebAPI.Attr
{
    public class SwaggerRetDataTypeAttribute : Attribute
    {
        public Type RetType;

        public SwaggerRetDataTypeAttribute(Type type)
        {
            RetType = type;
        }
    }
}

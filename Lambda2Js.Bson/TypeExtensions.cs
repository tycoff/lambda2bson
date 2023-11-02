using MongoDB.Bson.Serialization;
using System;

namespace Lambda2Js.Bson
{
    internal static class TypeExtensions
    {
        internal static string GetMongoName(this Type type, string memberName)
        {
            var serializer = BsonSerializer.LookupSerializer(type) as IBsonDocumentSerializer;
            BsonSerializationInfo memberSerializationInfo = null;
            serializer?.TryGetMemberSerializationInfo(memberName, out memberSerializationInfo);

            return memberSerializationInfo?.ElementName ?? memberName;
        }
    }
}
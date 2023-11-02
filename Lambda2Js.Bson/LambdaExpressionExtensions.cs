using MongoDB.Bson.Serialization;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Lambda2Js.Bson
{
    public static partial class LambdaExpressionExtensions
    {
        private static readonly JavascriptCompilationOptions Options = new JavascriptCompilationOptions(new StripQuotesFromIndexers())
        {
            CustomMetadataProvider = new MongoBsonMetadataProvider(),
        };

        public static string CompileToBson(
                [NotNull] this LambdaExpression expr)
        {
            return expr.CompileToJavascript(Options);
        }

        public static string GetMongoName(this Type type, string memberName)
        {
            var serializer = BsonSerializer.LookupSerializer(type) as IBsonDocumentSerializer;
            BsonSerializationInfo? memberSerializationInfo = null;
            serializer?.TryGetMemberSerializationInfo(memberName, out memberSerializationInfo);

            return memberSerializationInfo?.ElementName ?? memberName;
        }
    }
}
using Lambda2Js.Bson.Plugins;
using System.Linq.Expressions;

namespace Lambda2Js.Bson
{
    public static partial class LambdaExpressionExtensions
    {
        private static readonly JavascriptCompilationOptions Options = new JavascriptCompilationOptions(
                new StripQuotesFromIndexers(),
                new DotNotationForMapIndexers())
        {
            CustomMetadataProvider = new MongoBsonMetadataProvider(),
        };

        public static string CompileToBson(this LambdaExpression expr)
        {
            return expr.CompileToJavascript(Options);
        }
    }
}
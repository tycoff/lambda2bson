using System.Linq.Expressions;

namespace Lambda2Js.Bson.Plugins
{
    internal class DotNotationForMapIndexers : JavascriptConversionExtension
    {
        public override void ConvertToJavascript(JavascriptConversionContext context)
        {
            if (context.Node is MethodCallExpression methodContext)
            {
                if (methodContext.Method.IsSpecialName && methodContext.Method.Name == "get_Item" && methodContext.Arguments.Count == 1)
                {
                    var writer = context.GetWriter();
                    using (writer.Operation(JavascriptOperationTypes.IndexerProperty))
                    {
                        context.Visitor.Visit(methodContext.Object);

                        writer.Write('.');
                        using (writer.Operation(0))
                        {
                            context.Visitor.Visit(methodContext.Arguments[0]);
                        }
                    }

                    context.PreventDefault();
                }
            }
        }
    }
}

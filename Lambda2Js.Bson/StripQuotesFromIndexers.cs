﻿using System.Linq.Expressions;

namespace Lambda2Js.Bson
{
    public static partial class LambdaExpressionExtensions
    {
        public class StripQuotesFromIndexers : JavascriptConversionExtension
        {
            public override void ConvertToJavascript(JavascriptConversionContext context)
            {
                if (context.Node.NodeType == ExpressionType.Constant)
                {
                    if (context.Node is ConstantExpression constantExpression)
                    {
                        context.Write(constantExpression.Value.ToString());
                        context.PreventDefault();
                    }
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Lambda2Js.Bson
{
    /// <summary>
    /// Provides metadata about the objects that are going to be converted to JavaScript in some way.
    /// </summary>
    internal class MongoBsonMetadataProvider : JavascriptMetadataProvider
    {
        private readonly object locker = new object();

        private IJavascriptMemberMetadata GetMemberMetadataNoCache(MemberInfo memberInfo)
        {
            if (memberInfo == null)
                throw new ArgumentNullException(nameof(memberInfo));

            var attr0 = memberInfo
                .GetCustomAttributes(typeof(JavascriptMemberAttribute), true)
                .OfType<IJavascriptMemberMetadata>()
                .SingleOrDefault();

            if (attr0 != null)
                return attr0;

            return new JavascriptMemberAttribute
            {
                MemberName = memberInfo.DeclaringType.GetMongoName(memberInfo.Name),
            };
        }

        private Dictionary<MemberInfo, IJavascriptMemberMetadata> cache
            = new Dictionary<MemberInfo, IJavascriptMemberMetadata>();

        private IJavascriptMemberMetadata GetMemberMetadataWithCache(MemberInfo memberInfo)
        {
            IJavascriptMemberMetadata value;
            if (this.cache.TryGetValue(memberInfo, out value))
                return value;

            lock (this.locker)
            {
                if (this.cache.TryGetValue(memberInfo, out value))
                    return value;

                var meta = GetMemberMetadataNoCache(memberInfo);

                // Have to create a new instance here, because readers don't have any
                // syncronization with writers. I.e. when executing the line
                // `cache.TryGetValue` outside of the lock (above), if the same
                // instance was added to, then that method could get the wrong result.
                var newCache = new Dictionary<MemberInfo, IJavascriptMemberMetadata>(cache)
                {
                    [memberInfo] = meta
                };

                // This memory barrier is needed if this class is ever used in
                // a weaker memory model implementation, allowed by the CLR
                // specification.
                Interlocked.MemoryBarrier();

                this.cache = newCache;
                return meta;
            }
        }

        /// <summary>
        /// Gets metadata about a property that is going to be used in JavaScript code.
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public override IJavascriptMemberMetadata GetMemberMetadata(MemberInfo memberInfo)
        {
            return this.GetMemberMetadata(memberInfo, this.UseCache);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use cache by default.
        /// Applyes to `GetMemberMetadata` overload without `useCache` parameter.
        /// </summary>
        public bool UseCache { get; set; }

        /// <summary>
        /// Gets metadata about a property that is going to be used in JavaScript code.
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <param name="useCache"></param>
        /// <returns></returns>
        public IJavascriptMemberMetadata GetMemberMetadata(MemberInfo memberInfo, bool useCache)
        {
            if (useCache)
                return this.GetMemberMetadataWithCache(memberInfo);

            return this.GetMemberMetadataNoCache(memberInfo);
        }
    }
}
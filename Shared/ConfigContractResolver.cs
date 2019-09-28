using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using ParseTreeVisualizer.Util;

namespace ParseTreeVisualizer {
    internal class ConfigContractResolver : DefaultContractResolver {
        private static readonly string[] globalNames = new[] { "" };
        public bool ForGlobal { get; set; } = true;

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization) {
            var ret = base.CreateProperties(type, memberSerialization);
            var predicate = ForGlobal ?
                x => x.PropertyName.In(globalNames) :
                (Func<JsonProperty, bool>)(x => x.PropertyName.NotIn(globalNames));
            ret = ret.Where(predicate).ToList();
            return ret;
        }
    }
}

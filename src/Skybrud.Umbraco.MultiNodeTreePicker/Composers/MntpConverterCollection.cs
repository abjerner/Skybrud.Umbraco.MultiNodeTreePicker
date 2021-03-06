﻿using System;
using System.Collections.Generic;
using Skybrud.Umbraco.MultiNodeTreePicker.Converters;
using Umbraco.Core.Composing;

namespace Skybrud.Umbraco.MultiNodeTreePicker.Composers {
    
    public sealed class MntpConverterCollection : BuilderCollectionBase<IMntpItemConverter> {
        
        private readonly Dictionary<string, IMntpItemConverter> _lookup;

        public MntpConverterCollection(IEnumerable<IMntpItemConverter> items) : base(items) {
            
            _lookup = new Dictionary<string, IMntpItemConverter>(StringComparer.OrdinalIgnoreCase);

            foreach (IMntpItemConverter item in this) {
                
                string typeName = item.GetType().AssemblyQualifiedName;
                if (typeName != null && _lookup.ContainsKey(typeName) == false) {
                    _lookup.Add(typeName, item);
                }

            }

        }

        public bool TryGet(string typeName, out IMntpItemConverter item) {
            return _lookup.TryGetValue(typeName, out item);
        }

    }

}
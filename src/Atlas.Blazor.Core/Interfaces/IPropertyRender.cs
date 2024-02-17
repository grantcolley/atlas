﻿using System;
using System.Collections.Generic;

namespace Atlas.Blazor.Core.Interfaces
{
    public interface IPropertyRender<T>
    {
        string PropertyName { get; }
        int Order { get; set; }
        string? Label { get; set; }
        string? Tooltip { get; set; }
        Dictionary<string, string> Parameters { get; set; }
        Type? ComponentType { get; set; }
    }
}

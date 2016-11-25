using System;

namespace Stugo.Config
{
    public interface IConfigProvider
    {
        Uri BaseUri { get; }
        Uri[] GetChildren(Uri path);
        TValue GetValue<TValue>(Uri path, TValue defaultValue = default(TValue));
        void SetValue<TValue>(Uri path, TValue value);
    }
}

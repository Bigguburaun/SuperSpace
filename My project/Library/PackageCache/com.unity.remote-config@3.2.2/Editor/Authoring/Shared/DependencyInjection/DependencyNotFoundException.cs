// WARNING: Auto generated code. Modifications will be lost!
using System;

namespace Unity.Services.RemoteConfig.Authoring.Shared.DependencyInversion
{
    class DependencyNotFoundException : Exception
    {
        public DependencyNotFoundException(Type serviceType)
            : base($"Could not find factory for {serviceType.Name}")
        {
        }
    }
}
